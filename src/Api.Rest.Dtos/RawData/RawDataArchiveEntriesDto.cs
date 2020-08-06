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
	using Newtonsoft.Json;

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
		[JsonProperty( "archiveInfo" )]
		public RawDataInformationDto ArchiveInfo { get; set; }

		/// <summary>
		/// List of files in specified archive.
		/// </summary>
		[JsonProperty( "entries" )]
		public string[] Entries { get; set; }

		#endregion
	}
}