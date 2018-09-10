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

	public interface ICredentialLoginRequestHandler : ICacheClearable
	{
		#region methods

		Task<UsernamePasswordCredential> CredentialRequestAsync( Uri uri );

		UsernamePasswordCredential CredentialRequest( Uri uri );

		#endregion
	}
}