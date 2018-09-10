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
	using PiWebApi.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Utilities;

	#endregion

	/// <summary>
	/// Interface that is used as callback handler for requests that need a username/password credential for authentification.
	/// </summary>
	/// <remarks>
	/// This interface is mainly designed for showing a user interface to the user for entering the right username and password.
	/// </remarks>
	public interface ICredentialLoginRequestHandler : ICacheClearable
	{
		#region methods

		/// <summary>
		/// Asynchronous callback if a username nad password for a <see cref="Uri"/> is requested.
		/// </summary>
		/// <param name="uri">The address for which a username and password is requested.</param>
		/// <returns>Returns a the username/password credential that should be used for authentication or null if no credential should be used.</returns>
		/// <exception cref="LoginCanceledException">Throws a <see cref="LoginCanceledException"/> if the request should be canceled.</exception>
		/// <remarks>
		/// The <see cref="Uri"/> usually refers to the base address of a server, since declared endpoints usually do not use different authentications.
		/// </remarks>
		Task<UsernamePasswordCredential> CredentialRequestAsync( [NotNull] Uri uri );

		/// <summary>
		/// Synchronous callback if a username and password for a <see cref="Uri"/> is requested.
		/// </summary>
		/// <param name="uri">The address for which a username and password is requested.</param>
		/// <returns>Returns a the username/password credential that should be used for authentication or null if no credential should be used.</returns>
		/// <exception cref="LoginCanceledException">Throws a <see cref="LoginCanceledException"/> if the request should be canceled.</exception>
		/// <remarks>
		/// This is the synchronous counter part to <see cref="CredentialRequestAsync"/> which stays usually unused by the <see cref="RestClient"/>,
		/// since its API is fully asynchronous. This mainly exists for purpose of introducing a synchronous API if needed in the future.
		/// The <see cref="Uri"/> usually refers to the base address of a server, since declared endpoints usually do not use different authentications.
		/// </remarks>
		UsernamePasswordCredential CredentialRequest( [NotNull] Uri uri );

		#endregion
	}
}