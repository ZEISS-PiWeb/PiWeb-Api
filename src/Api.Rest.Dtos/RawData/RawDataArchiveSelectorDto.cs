#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion﻿

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System.Collections.Generic;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Selects a specific raw data entry which is an archive, as well as requested files of this archive.
	/// </summary>
	public class RawDataArchiveSelectorDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveSelectorDto() { }

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveSelectorDto( int key, RawDataTargetEntityDto target, IEnumerable<string> files )
		{
			Files = files;
			Key = key;
			Target = target;
		}

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
		/// List of requested files in specified archive.
		/// </summary>
		[JsonProperty( "files" )]
		public IEnumerable<string> Files { get; set; }

		#endregion
	}
}