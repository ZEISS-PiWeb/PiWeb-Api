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

	using System.Text.Json.Serialization;

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
		[JsonPropertyName( "version" )]
		public string Version { get; set; }

		[JsonPropertyName( "requestHeaderSize" )]
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