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

	using System;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Can query a set of specific raw data entries in one go.
	/// </summary>
	[Serializable]
	public class RawDataBulkQueryDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataBulkQueryDto()
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="selectors"></param>
		public RawDataBulkQueryDto( [NotNull] RawDataSelectorDto[] selectors )
		{
			Selectors = selectors ?? throw new ArgumentNullException( nameof( selectors ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// Selectors containing information about target and key of requested archives.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "selectors" )]
		[JsonPropertyName( "selectors" )]
		public RawDataSelectorDto[] Selectors { get; set; } = { };

		#endregion
	}
}