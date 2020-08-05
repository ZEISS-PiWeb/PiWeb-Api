#region Copyright

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
	/// Represents a file which is part of an archive, containing actual data and meta information.
	/// </summary>
	[Serializable]
	public class RawDataArchiveContentDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveContentDto() { }

		#endregion

		#region properties

		/// <summary>
		/// Full name of the file.
		/// </summary>
		[JsonProperty( "fileName" )]
		public string FileName { get; set; }

		/// <summary>
		/// Length of data.
		/// </summary>
		[JsonProperty( "size" )]
		public int Size { get; set; }

		/// <summary>
		/// Actual data representing the file.
		/// </summary>
		[JsonProperty( "data" )]
		public byte[] Data { get; set; }

		/// <summary>
		/// MD5 checksum of data.
		/// </summary>
		[JsonProperty( "md5" )]
		public Guid MD5 { get; set; }

		/// <summary>
		/// <see cref="RawDataInformationDto"/> of original archive.
		/// </summary>
		[JsonProperty( "archiveInfo" )]
		public RawDataInformationDto ArchiveInfo { get; set; }

		#endregion
	}
}