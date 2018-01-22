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
	/// Aufzählung, mit der eingestellt werden kann, ob ein Zertifikat an den Server gesendet werden soll.
	/// </summary>
	public enum AuthenticationMode
	{
		/// <summary>
		/// Kein Zertifikat und kein Windowstoken senden - eventuell nach Nutzername/Passwort fragen, falls der Server Basic-Authentifizierung nutzt.
		/// </summary>
		NoneOrBasic,

		/// <summary>
		/// Integrierte Windows-Authentifizierung nutzen.
		/// </summary>
		Windows,

		/// <summary>
		/// Zertifikat nutzen.
		/// </summary>
		Certificate,

		/// <summary>
		/// Hardware-Zertifikat nutzen.
		/// </summary>
		HardwareCertificate,

		/// <summary>
		/// Authentifizierung via OAuth-Protokoll.
		/// </summary>
		OAuth,
	}
}
