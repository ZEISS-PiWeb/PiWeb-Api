#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.OAuthService
{
	#region using

	using Newtonsoft.Json;

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
		[JsonProperty( "openIdAuthority" )]
		public string OpenIdAuthority { get; set; }

		#endregion
	}
}