#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class contains general information about the RawDataService, like its current version.
	/// </summary>
	public class ServiceInformationDto
	{
		#region properties

		/// <summary>
		/// Gets or sets the version of the server backend.
		/// </summary>
		[JsonProperty( "version" )]
		public string Version { get; set; }

		[JsonProperty( "requestHeaderSize" )]
		public int RequestHeaderSize { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return Version == null ? string.Empty : $"Version {Version}";
		}

		#endregion
	}
}