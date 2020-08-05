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
	using System.Collections.Generic;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Contains list of entries which are part of an archive,
	/// as well as information about original archive (raw data).
	/// </summary>
	[Serializable]
	public class RawDataArchiveIndexDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveIndexDto() { }

		#endregion

		#region properties

		/// <summary>
		/// <see cref="RawDataTargetEntityDto"/> of raw data.
		/// </summary>
		[JsonProperty( "target" )]
		public RawDataTargetEntityDto Target { get; set; }

		/// <summary>
		/// Key to specify raw data of target entity.
		/// </summary>
		[JsonProperty( "key" )]
		public int Key { get; set; }

		/// <summary>
		/// List of files in specified archive.
		/// </summary>
		[JsonProperty( "files" )]
		public IEnumerable<string> Files { get; set; }

		#endregion
	}
}