#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Core;

	#endregion

	/// <summary>
	/// Selects a set of measurement values specified by a part path or filter.
	/// </summary>
	[Serializable]
	public class MeasurementValueQueryDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public MeasurementValueQueryDto()
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		public MeasurementValueQueryDto( [CanBeNull] PathInformation partPath, [CanBeNull] MeasurementValueFilterAttributesDto filter )
		{
			PartPath = partPath;
			Filter = filter;
		}

		#endregion

		#region properties

		/// <summary>
		/// The part path.
		/// </summary>
		[JsonProperty( "partPath" )]
		[JsonPropertyName( "partPath" )]
		[CanBeNull]
		public PathInformation PartPath { get; set; }

		/// <summary>
		/// The filter.
		/// </summary>
		[JsonProperty( "filter" )]
		[JsonPropertyName( "filter" )]
		[CanBeNull]
		public MeasurementValueFilterAttributesDto Filter { get; set; }

		#endregion
	}
}