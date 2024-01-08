#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Holds information about necessary configuration to negotiate OAuth token.
	/// </summary>
	public class OAuthTokenInformation
	{
		#region properties

		/// <summary>
		/// The URL of the trusted OpenID Connect authority for this service.
		/// </summary>
		[JsonProperty( "openIdAuthority" )]
		[JsonPropertyName( "openIdAuthority" )]
		public string OpenIdAuthority { get; set; }

		/// <summary>
		/// ClientID of the app registration at the OpenID provider.
		/// </summary>
		[JsonProperty( "clientId" )]
		[JsonPropertyName( "clientId" )]
		public string ClientID { get; set; } = "f1ddf74a-7ed1-4963-ab60-a1138a089791";

		/// <summary>
		/// Redirect URI after successful authentication.
		/// </summary>
		[JsonProperty( "redirectUri" )]
		[JsonPropertyName( "redirectUri" )]
		public string RedirectUri { get; set; } = "urn:ietf:wg:oauth:2.0:oob";

		/// <summary>
		/// Scopes which the client can request.
		/// </summary>
		[JsonProperty( "requestedScopes" )]
		[JsonPropertyName( "requestedScopes" )]
		public string RequestedScopes { get; set; } = "openid profile email offline_access piweb";

		/// <summary>
		/// Additional trusted base addresses for OpenID endpoints.
		/// </summary>
		[JsonProperty( "additionalEndpointBaseAddresses" )]
		[JsonPropertyName( "additionalEndpointBaseAddresses" )]
		public ICollection<string> AdditionalEndpointBaseAddresses { get; set; } = Array.Empty<string>();

		#endregion
	}
}