#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System;
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Contains list of entries which are part of an archive,
	/// as well as information about original archive (raw data).
	/// </summary>
	[Serializable]
	public class RawDataArchiveEntriesDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveEntriesDto() { }

		#endregion

		#region properties

		/// <summary>
		/// <see cref="RawDataInformationDto"/> of original archive.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "archiveInfo" )]
		[JsonPropertyName( "archiveInfo" )]
		public RawDataInformationDto ArchiveInfo { get; set; }

		/// <summary>
		/// List of files in specified archive.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "entries" )]
		[JsonPropertyName( "entries" )]
		public string[] Entries { get; set; }

		#endregion
	}
}