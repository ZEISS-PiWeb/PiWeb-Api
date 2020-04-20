#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	/// <summary>
	/// Enumeration that specifies the which types of measurements will be fetched from the database.
	/// </summary>
	public enum AggregationMeasurementSelection
	{
		/// <summary>Fetch normal measurements.</summary>
		Measurements = 1,

		/// <summary>Fetch aggregated measurements</summary>
		AggregationMeasurements = 2,

		/// <summary>Default behavior (fetch normal measurements only)</summary>
		Default = Measurements,

		/// <summary>Fetch all measurements (normal and aggregated).</summary>
		All = Measurements | AggregationMeasurements,
	}
}
