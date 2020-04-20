#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Utilities
{
	public class OAuthRequest
	{
		public OAuthRequest( string startUrl, string callbackUrl )
		{
			StartUrl = startUrl;
			CallbackUrl = callbackUrl;
		}

		public string StartUrl { get; }
		public string CallbackUrl { get; }

	}
}
