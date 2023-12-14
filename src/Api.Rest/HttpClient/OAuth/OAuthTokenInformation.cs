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

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// This element holds information about valid OAuth token authorities.
	/// </summary>
	public class OAuthTokenInformation
	{
		#region properties

		/// <summary>
		/// The URL of the trusted OpenID Connect authority for this service.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "openIdAuthority" )]
		[JsonPropertyName( "openIdAuthority" )]
		public string OpenIdAuthority { get; set; }

		#endregion
	}
}