#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;
	using System.Threading.Tasks;
	using Zeiss.IMT.PiWeb.Api.Common.Utilities;

	#endregion

	public interface IOAuthLoginRequestHandler : ICacheClearable
	{
		#region methods

		Task<OAuthTokenCredential> OAuthRequestAsync( Uri uri, string refreshToken = null );

		OAuthTokenCredential OAuthRequest( Uri uri, string refreshToken = null );

		#endregion
	}
}