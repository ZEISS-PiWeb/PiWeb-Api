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
	/// Can query a set of specific archives saved as raw data.
	/// </summary>
	[Serializable]
	public class RawDataArchiveBulkQueryDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="selectors"></param>
		public RawDataArchiveBulkQueryDto( RawDataArchiveSelectorDto[] selectors )
		{
			Selectors = selectors;
		}

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveBulkQueryDto() { }

		#endregion

		#region properties

		/// <summary>
		/// Selectors containing information about target, key and requested files of specified archives.
		/// </summary>
		[JsonProperty( "selectors" )]
		public RawDataArchiveSelectorDto[] Selectors { get; set; } = { };

		#endregion
	}
}