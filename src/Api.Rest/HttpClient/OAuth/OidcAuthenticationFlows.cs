#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;

#region usings

using System;

#endregion

/// <summary>
/// Flags enum which represents possible OIDC authentication flows and their combinations supported by the PiWeb Server.
/// </summary>
[Flags]
public enum OidcAuthenticationFlows
{
	/// <summary>
	/// No authentication flow is supported.
	/// </summary>
	None = 0,

	/// <summary>
	/// OIDC hybrid flow is supported.
	/// </summary>
	HybridFlow = 1,

	/// <summary>
	/// OIDC authorization code flow is supported.
	/// </summary>
	AuthorizationCodeFlow = 2
}