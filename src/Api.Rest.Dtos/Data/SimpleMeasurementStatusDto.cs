#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Status object that has detailed information about the number of characteristics in/out tolerance. This status object can be
	/// requested when performing a measurement search using one of the values of <see cref="MeasurementStatisticsDto"/>.
	/// </summary>
	[DebuggerDisplay( "Status ({Id} = {Count})" )]
	public class SimpleMeasurementStatusDto
	{
		#region properties

		/// <summary>
		/// Gets or sets the unique id of the status. Currently this can be one of the following:
		/// <code>InTol</code>, <code>OutWarn</code> and <code>OutTol</code>.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "id" )]
		[JsonPropertyName( "id" )]
		public string Id { get; set; }

		/// <summary>
		/// Gets or sets the number of characteristics with this measurement status.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "count" )]
		[JsonPropertyName( "count" )]
		public int Count { get; set; }

		/// <summary>
		/// Gets or sets the uuids of the characteristics that have this status. This property will only be populated
		/// when the detailed status is requested with <see cref="MeasurementStatisticsDto.Detailed"/>.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "uuid" )]
		[JsonPropertyName( "uuid" )]
		public IReadOnlyCollection<Guid> Uuid { get; set; }

		#endregion
	}
}