#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Authentication;

#region usings

using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Client;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// A processor method that executes a given refresh token consumer on an internally stored OIDC refresh token. The consumer returns
/// a new refresh token that then replaces the internally stored refresh token for any future calls. If the input refresh token of the
/// consumer can be reused again after this operation, the consumer may return this input token instead of a new token.
/// If no internally stored refresh token is available, the consumer is not executed at all.
/// The return value of this processor indicates whether the consumer was executed (<c>true</c>), or not executed (<c>false</c>).
/// Using a processor instead of a static refresh token enables refresh token rotation.
/// </summary>
public delegate Task<bool> RefreshTokenProcessorAsync( Func<string, Task<string>> consumer, CancellationToken token );

/// <summary>
/// <inheritdoc />
/// This authentication handler allows authentication via statically predefined authentication data and avoids any user interaction.
/// In case of OpenIDC authentication a static refresh token must be given that can be used to derive an access token when necessary.
/// </summary>
public class NonInteractiveAuthenticationHandler : IAuthenticationHandler
{
	#region members

	private static readonly SemaphoreSlim LoginRefreshSemaphore = new(1, 1);

	#endregion

	#region constructors

	private NonInteractiveAuthenticationHandler(
		AuthenticationMode authenticationMode,
		[CanBeNull] string username = null,
		[CanBeNull] string password = null,
		[CanBeNull] X509Certificate2 clientCertificate = null,
		[CanBeNull] RefreshTokenProcessorAsync refreshTokenProcessor = null )
	{
		AuthenticationMode = authenticationMode;
		Username = username;
		Password = password;
		ClientCertificate = clientCertificate;
		RefreshTokenProcessor = refreshTokenProcessor;
	}

	#endregion

	#region properties

	/// <summary>
	/// The authentication mode.
	/// </summary>
	public AuthenticationMode AuthenticationMode { get; }

	/// <summary>
	/// The username used for authentication. Only used when <see cref="AuthenticationMode"/> is either
	/// <see cref="AuthenticationMode.NoneOrBasic"/> or <see cref="AuthenticationMode.Windows"/>.
	/// </summary>
	[CanBeNull]
	public string Username { get; }

	/// <summary>
	/// The password used for authentication. Only used when <see cref="AuthenticationMode"/> is either
	/// <see cref="AuthenticationMode.NoneOrBasic"/> or <see cref="AuthenticationMode.Windows"/>.
	/// </summary>
	[CanBeNull]
	private string Password { get; }

	[CanBeNull]
	private X509Certificate2 ClientCertificate { get; }

	[CanBeNull]
	private RefreshTokenProcessorAsync RefreshTokenProcessor { get; }

	#endregion

	#region methods

	[CanBeNull]
	private static X509Certificate2 FindCertificate( string certificateThumbprint )
	{
		if( string.IsNullOrEmpty( certificateThumbprint ) )
			return null;

		return CertificateHelper.FindCertificateByThumbprint( certificateThumbprint, storeLocation: StoreLocation.CurrentUser )
				?? CertificateHelper.FindCertificateByThumbprint( certificateThumbprint, storeLocation: StoreLocation.LocalMachine );
	}

	#endregion

	#region interface IAuthenticationHandler

	/// <inheritdoc />
	public void InitializeRestClient( IInitializationContext context )
	{
		switch( AuthenticationMode )
		{
			case AuthenticationMode.NoneOrBasic when Username != null:
				context.UpdateAuthenticationContainer(
					AuthenticationContainer.FromUsernamePasswordCredential(
						new UsernamePasswordCredential( Username, Password ?? string.Empty ) ) );
				break;

			case AuthenticationMode.Windows when Username != null:
				context.UpdateAuthenticationContainer(
					new AuthenticationContainer(
						AuthenticationMode.Windows,
						new NetworkCredential( Username, Password ?? string.Empty ) ) );
				break;

			case AuthenticationMode.Windows:
				context.UpdateAuthenticationContainer( new AuthenticationContainer( AuthenticationMode.Windows ) );
				break;

			case AuthenticationMode.HardwareCertificate:
			case AuthenticationMode.Certificate:
				context.UpdateAuthenticationContainer(
					AuthenticationContainer.FromCertificateCredential(
						CertificateCredential.CreateFromCertificate( ClientCertificate ) ) );
				break;

			case AuthenticationMode.OAuth:
				context.UpdateAuthenticationContainer( new AuthenticationContainer( AuthenticationMode.OAuth ) );
				break;

			default:
				context.UpdateAuthenticationContainer( new AuthenticationContainer( AuthenticationMode.NoneOrBasic ) );
				break;
		}
	}

	/// <inheritdoc />
	public Task HandleRequest( IRequestContext context )
	{
		return Task.CompletedTask;
	}

	/// <inheritdoc />
	public async Task HandleResponse( IResponseContext context )
	{
		// Since we are non-interactive, the only authentication error we can meaningfully handle is an expired OIDC access token.
		// If this is indeed the case, we can try to get a new access token for our known refresh token. If the new access token
		// does not work either, we can only give up.

		var isOAuth = context.CurrentAuthenticationContainer.Mode == AuthenticationMode.OAuth;
		var isUnauthorized = context.CurrentResponse.StatusCode == HttpStatusCode.Unauthorized;
		if( !isUnauthorized || !isOAuth || RefreshTokenProcessor == null || context.Attempt > 1 )
			return;

		await LoginRefreshSemaphore.WaitAsync( context.CancellationToken ).ConfigureAwait( false );

		try
		{
			var instanceUri = new Uri( context.ServiceLocation, "../" );
			OAuthTokenCredential credential = null;
			await RefreshTokenProcessor(
				async ( currentRefreshToken ) =>
				{
					credential = await OAuthHelper.GetAuthenticationInformationForDatabaseUrlAsync(
						instanceUri.AbsoluteUri,
						currentRefreshToken,
						bypassLocalCache: true ).ConfigureAwait( false );

					return credential?.RefreshToken ?? currentRefreshToken;
				},
				context.CancellationToken ).ConfigureAwait( false );

			if( credential == null )
				return;

			var newContainer = AuthenticationContainer.FromOAuthTokenCredential( credential );
			if( newContainer == context.CurrentAuthenticationContainer )
				return;

			context.UpdateAuthenticationContainer( newContainer );
			context.RetryRequest = true;
		}
		finally
		{
			LoginRefreshSemaphore.Release();
		}
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which uses a given refresh token to implement a
	/// non-interactive OIDC authentication.
	/// </summary>
	/// <param name="refreshToken">The refresh token to use to fetch access token.</param>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler OIDC( string refreshToken )
	{
		return new NonInteractiveAuthenticationHandler(
			AuthenticationMode.OAuth,
			refreshTokenProcessor: async ( consumer, _ ) =>
			{
				await consumer( refreshToken ).ConfigureAwait( false );
				return true;
			} );
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which uses a given refresh token processor method to
	/// implement a non-interactive OIDC authentication with refresh token rotation.
	/// Each time a new access token is required, the given processor is called with a predefined consumer method. The given processor
	/// method must execute the refresh token consumer on an internally stored OIDC refresh token. The consumer returns
	/// a new refresh token that must then replace the internally stored refresh token for any future calls. If the input refresh token of
	/// the consumer can be reused again after this operation, the consumer may return this input refresh token instead of a new token.
	/// If no internally stored refresh token is available for any reason, the processor method should not execute the consumer at all.
	/// The return value of the processor must indicate whether the consumer was executed (<c>true</c>), or not executed (<c>false</c>).
	/// Using a processor instead of a static refresh token enables refresh token rotation.
	/// </summary>
	/// <param name="refreshTokenProcessor">A method to process and potentially rotate the existing refresh token.</param>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler OIDC( RefreshTokenProcessorAsync refreshTokenProcessor )
	{
		return new NonInteractiveAuthenticationHandler(
			AuthenticationMode.OAuth,
			refreshTokenProcessor: refreshTokenProcessor );
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which implements Single Sign-on authentication based
	/// on Kerberos or NTLM authentication.
	/// </summary>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler WindowsSSO()
	{
		return new NonInteractiveAuthenticationHandler( AuthenticationMode.Windows );
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which implements username/password authentication based
	/// on Kerberos or NTLM authentication.
	/// </summary>
	/// <param name="username">The username.</param>
	/// <param name="password">The password.</param>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler Windows( string username, string password )
	{
		return new NonInteractiveAuthenticationHandler( AuthenticationMode.Windows, username, password );
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which implements username/password authentication based
	/// on Basic authentication.
	/// </summary>
	/// <param name="username">The username.</param>
	/// <param name="password">The password.</param>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler Basic( string username, string password )
	{
		return new NonInteractiveAuthenticationHandler( AuthenticationMode.NoneOrBasic, username, password );
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which implements authentication based
	/// on a client certificate.
	/// </summary>
	/// <param name="certificate">The certificate.</param>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler Certificate( X509Certificate2 certificate )
	{
		return new NonInteractiveAuthenticationHandler( AuthenticationMode.Certificate, clientCertificate: certificate );
	}

	/// <summary>
	/// Creates a new <see cref="NonInteractiveAuthenticationHandler"/> instance which implements authentication based
	/// on a client certificate.
	/// </summary>
	/// <param name="certificateThumbprint">
	/// The thumbprint of the certificate to use. The certificate will be looked up in the local certificate storage of the current
	/// user first and additionally in the local computer storage if it is not found in the local user storage.
	/// If no certificate is found, no certificate will be send to the server when connecting.
	/// </param>
	/// <returns>The new instance.</returns>
	public static NonInteractiveAuthenticationHandler Certificate( string certificateThumbprint )
	{
		var certificate = FindCertificate( certificateThumbprint );
		return new NonInteractiveAuthenticationHandler( AuthenticationMode.Certificate, clientCertificate: certificate );
	}

	#endregion
}