#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;
	using System.Threading.Tasks;

	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Utilities;

	#endregion

	/// <summary>
	/// Interface that is used as callback handler for requests that use an OAuth token for authentification.
	/// </summary>
	/// <remarks>
	/// This interface is mainly designed for showing a user interface to the user for doing a OAuth login which usually opens a HTTP login formular.
	/// </remarks>
	public interface IOAuthLoginRequestHandler : ICacheClearable
	{
		#region methods

		/// <summary>
		/// Asynchronous callback if an OAuth token for a <see cref="Uri"/> is requested.
		/// </summary>
		/// <param name="uri">The address for which an OAuth token is requested.</param>
		/// <param name="refreshToken">An optional refresh token that can be used to get a new OAuth token.</param>
		/// <returns>Returns an OAuth token that should be used for authentication or null if no OAuth token should be used.</returns>
		/// <exception cref="LoginCanceledException">Throws a <see cref="LoginCanceledException"/> if the request should be canceled.</exception>
		/// <remarks>
		/// The <see cref="Uri"/> usually refers to the base address of a server, since declared endpoints usually do not use different authentications.
		/// </remarks>
		Task<OAuthTokenCredential> OAuthRequestAsync( [NotNull] Uri uri, [CanBeNull] string refreshToken = null );

		/// <summary>
		/// Synchronous callback if an OAuth token for a <see cref="Uri"/> is requested.
		/// </summary>
		/// <param name="uri">The address for which an OAuth token is requested.</param>
		/// <param name="refreshToken">An optional refresh token that can be used to get a new OAuth token.</param>
		/// <returns>Returns an OAuth token that should be used for authentication or null if no OAuth token should be used.</returns>
		/// <exception cref="LoginCanceledException">Throws a <see cref="LoginCanceledException"/> if the request should be canceled.</exception>
		/// <remarks>
		/// This is the synchronous counter part to <see cref="OAuthRequestAsync"/> which stays usually unused by the <see cref="RestClient"/>,
		/// since its API is fully asynchronous. This mainly exists for purpose of introducing a synchronous API if needed in the future.
		/// The <see cref="Uri"/> usually refers to the base address of a server, since declared endpoints usually do not use different authentications.
		/// </remarks>
		OAuthTokenCredential OAuthRequest( [NotNull] Uri uri, [CanBeNull] string refreshToken = null );

		#endregion
	}
}