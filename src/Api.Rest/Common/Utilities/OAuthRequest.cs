#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	public class OAuthRequest
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthRequest"/> class.
		/// </summary>
		public OAuthRequest( string startUrl, string callbackUrl )
		{
			StartUrl = startUrl;
			CallbackUrl = callbackUrl;
		}

		#endregion

		#region properties

		public string StartUrl { get; }
		public string CallbackUrl { get; }

		#endregion
	}
}