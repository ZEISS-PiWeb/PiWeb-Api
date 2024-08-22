#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;

#region usings

using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Newtonsoft.Json;

#endregion

/// <summary>
/// Represents general OAuth configuration settings, including token information for different identity provider.
/// </summary>
public class OAuthConfiguration
{
	#region properties

	/// <summary>
	/// Flags enum representing the OIDC authentication flows supported by this PiWeb Server.
	/// </summary>
	[JsonProperty( "supportedOidcAuthenticationFlows" )]
	[JsonPropertyName( "supportedOidcAuthenticationFlows" )]
	public OidcAuthenticationFlows SupportedOidcAuthenticationFlows { get; set; } = OidcAuthenticationFlows.HybridFlow;

	/// <summary>
	/// OAuth configuration of the external identity provider configured in PiWeb Server, or <see langword="null"/> if no external provider is configured.
	/// </summary>
	[JsonProperty( "upstreamTokenInformation" )]
	[JsonPropertyName( "upstreamTokenInformation" )]
	public OAuthTokenInformation UpstreamTokenInformation { get; set; }

	/// <summary>
	/// OAuth configuration of the built-in identity provider of PiWeb Server, or <see langword="null"/> if no internal provider is configured.
	/// </summary>
	[JsonProperty( "localTokenInformation" )]
	[JsonPropertyName( "localTokenInformation" )]
	public OAuthTokenInformation LocalTokenInformation { get; set; }

	#endregion
}