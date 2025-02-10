#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth.AuthenticationFlows;

#region usings

using System;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// Provides methods for authentication flows using OIDC.
/// </summary>
public interface IOidcAuthenticationFlow
{
	#region methods

	/// <summary>
	/// Execute the OIDC authentication flow.
	/// </summary>
	/// <param name="refreshToken">Refresh token to acquire a new authentication token.</param>
	/// <param name="configuration">OAuth configuration containing the settings for authentication.</param>
	/// <param name="requestCallback">The callback to execute for authentication, e.g opening a browser window.</param>
	OAuthTokenCredential ExecuteAuthenticationFlow( [CanBeNull] string refreshToken,
		OAuthConfiguration configuration,
		Func<OAuthRequest, OAuthResponse> requestCallback );

	/// <summary>
	/// Asynchronously execute the OIDC authentication flow.
	/// </summary>
	/// <param name="refreshToken">Refresh token to acquire a new authentication token.</param>
	/// <param name="configuration">OAuth configuration containing the settings for authentication.</param>
	/// <param name="requestCallbackAsync">The asynchronous callback to execute for authentication, e.g opening a browser window.</param>
	/// <param name="cancellationToken">The <see cref="CancellationToken"/> that can be used to cancel the operation.</param>
	Task<OAuthTokenCredential> ExecuteAuthenticationFlowAsync( [CanBeNull] string refreshToken,
		OAuthConfiguration configuration,
		Func<OAuthRequest, Task<OAuthResponse>> requestCallbackAsync,
		CancellationToken cancellationToken );

	#endregion
}