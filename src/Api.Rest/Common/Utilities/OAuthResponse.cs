#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
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

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthResponse"/> class.
		/// </summary>
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