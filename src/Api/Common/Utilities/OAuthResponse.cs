#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Utilities
{
	#region usings

	using IdentityModel.Client;

	#endregion

	public class OAuthResponse
	{
		#region members

		private readonly AuthorizeResponse _AuthorizeResponse;

		#endregion

		#region constructors

		public OAuthResponse( string resultUrl )
		{
			if( !string.IsNullOrEmpty( resultUrl ) )
			{
				_AuthorizeResponse = new AuthorizeResponse( resultUrl );
			}
		}

		#endregion

		#region properties

		public string AccessToken => _AuthorizeResponse.AccessToken;

		#endregion

		#region methods

		public AuthorizeResponse ToAuthorizeResponse() => _AuthorizeResponse;

		#endregion
	}
}