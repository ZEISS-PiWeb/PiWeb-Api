#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	/// <summary>
	/// Interface that is used as callback handler for the different kinds of credentials that can be used for authentification.
	/// </summary>
	public interface ILoginRequestHandler : ICredentialLoginRequestHandler, ICertificateLoginRequestHandler, IOAuthLoginRequestHandler
	{
	}
}