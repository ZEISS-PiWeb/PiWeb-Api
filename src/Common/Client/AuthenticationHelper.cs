#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;
	using System.Threading.Tasks;

	#endregion

	public static class AuthenticationHelper
	{
		#region methods

		public static bool IsAuthenticationContainerIncomplete( AuthenticationContainer authenticationContainer )
		{
			switch( authenticationContainer.Mode )
			{
				case AuthenticationMode.Certificate:
				case AuthenticationMode.HardwareCertificate:
					return authenticationContainer.Certificate == null;

				case AuthenticationMode.OAuth:
					return authenticationContainer.OAuthAccessToken == null;
			}

			return false;
		}

		public static async Task<AuthenticationContainer> RequestAuthenticationInformationUpdateAsync( AuthenticationMode authenticationMode, Uri instanceUri, ILoginRequestHandler loginRequestHandler )
		{
			try
			{
				switch( authenticationMode )
				{
					case AuthenticationMode.NoneOrBasic:
						return AuthenticationContainer.FromUsernamePasswordCredential(
							await loginRequestHandler.CredentialRequestAsync( instanceUri ).ConfigureAwait( false ) );

					case AuthenticationMode.Windows:
						return new AuthenticationContainer( AuthenticationMode.Windows );

					case AuthenticationMode.Certificate:
					case AuthenticationMode.HardwareCertificate:
						return AuthenticationContainer.FromCertificateCredential(
							await loginRequestHandler.CertificateRequestAsync( instanceUri, authenticationMode == AuthenticationMode.HardwareCertificate )
								.ConfigureAwait( false ) );

					case AuthenticationMode.OAuth:
						return AuthenticationContainer.FromOAuthTokenCredential(
							await loginRequestHandler.OAuthRequestAsync( instanceUri ).ConfigureAwait( false ) );

					default:
						throw new ArgumentOutOfRangeException( nameof( authenticationMode ), authenticationMode, null );
				}
			}
			catch( OperationCanceledException ex )
			{
				throw new LoginCanceledException( "Login canceled", ex );
			}
		}

		public static AuthenticationContainer RequestAuthenticationInformationUpdate( AuthenticationMode authenticationMode, Uri instanceUri, ILoginRequestHandler loginRequestHandler )
		{
			try
			{
				if( loginRequestHandler == null )
					return null;

				switch( authenticationMode )
				{
					case AuthenticationMode.NoneOrBasic:
						return AuthenticationContainer.FromUsernamePasswordCredential( 
							loginRequestHandler.CredentialRequest( instanceUri ) );

					case AuthenticationMode.Windows:
						return new AuthenticationContainer( AuthenticationMode.Windows );

					case AuthenticationMode.Certificate:
					case AuthenticationMode.HardwareCertificate:
						return AuthenticationContainer.FromCertificateCredential( 
							loginRequestHandler.CertificateRequest( instanceUri, authenticationMode == AuthenticationMode.HardwareCertificate ) );

					case AuthenticationMode.OAuth:
						return AuthenticationContainer.FromOAuthTokenCredential( 
							loginRequestHandler.OAuthRequest( instanceUri ) );

					default:
						throw new ArgumentOutOfRangeException( nameof( authenticationMode ), authenticationMode, null );
				}
			}
			catch( OperationCanceledException ex )
			{
				throw new LoginCanceledException( "Login canceled", ex );
			}
		}

		#endregion
	}
}