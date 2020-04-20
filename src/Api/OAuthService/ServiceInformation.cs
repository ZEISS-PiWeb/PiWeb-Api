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

	using System;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This element holds general information about the service, like its current version.
	/// </summary>
	public class ServiceInformation
	{
		#region properties

		/// <summary>
		/// The major interface version of the service.
		/// </summary>
		[JsonProperty( "versionWsdlMajor" )]
		public string VersionWsdlMajor { get; set; }

		/// <summary>
		/// The minor interface version of the service.
		/// </summary>
		[JsonProperty( "versionWsdlMinor" )]
		public string VersionWsdlMinor { get; set; }

		/// <summary>
		/// Convenience property for <see cref="VersionWsdlMajor"/> and <see cref="VersionWsdlMinor"/>.
		/// </summary>
		[JsonIgnore]
		public Version VersionWsdl
		{
			get { return new Version( string.Format( "{0}.{1}", string.IsNullOrEmpty( VersionWsdlMajor ) ? "0" : VersionWsdlMajor, string.IsNullOrEmpty( VersionWsdlMinor ) ? "0" : VersionWsdlMinor ) ); }
		}

		#endregion
	}
}