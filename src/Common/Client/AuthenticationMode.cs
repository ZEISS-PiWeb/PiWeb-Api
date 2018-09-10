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
	/// Enum which is used to define the authentication mode when communicating to a server.
	/// </summary>
	public enum AuthenticationMode
	{
		/// <summary>
		/// Send no certificate or windows token - eventually use username / password if the Server uses Basic-Authentication.
		/// </summary>
		NoneOrBasic,

		/// <summary>
		/// Use integrated Windows authentication.
		/// </summary>
		Windows,

		/// <summary>
		/// Use certificate authentication.
		/// </summary>
		Certificate,

		/// <summary>
		/// Use hardware certification authentication
		/// </summary>
		HardwareCertificate,

		/// <summary>
		/// Use OAuth protocol authentication.
		/// </summary>
		OAuth,
	}
}
