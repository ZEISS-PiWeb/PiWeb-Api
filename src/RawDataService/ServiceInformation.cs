#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace RawDataService
{
	#region using

	using System;

	#endregion

	/// <summary>
	/// This class contains general information about the RawDataService, like its current version.
	/// </summary>
	public class ServiceInformation
	{
		#region properties

		/// <summary>
		/// Gets or sets the major interface version.
		/// </summary>
		public string VersionWsdlMajor { get; set; }

		/// <summary>
		/// Gets or sets the minor interface version.
		/// </summary>
		public string VersionWsdlMinor { get; set; }

		/// <summary>
		/// Gets or sets the version of the server backend.
		/// </summary>
		public string Version { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Overridden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			if( Version == null )
				return "";

			var result = "Version " + Version;
			if( !string.IsNullOrEmpty( VersionWsdlMajor ) )
				result += " (WSDL: " + VersionWsdlMajor + "." + VersionWsdlMinor + ")";

			return result;
		}

		#endregion
	}
}