#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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
		[Newtonsoft.Json.JsonProperty( "version" )]
		[JsonPropertyName( "version" )]
		public string Version { get; set; }

		[Newtonsoft.Json.JsonProperty( "requestHeaderSize" )]
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