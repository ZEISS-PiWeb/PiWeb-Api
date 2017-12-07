#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data
{
	/// <summary>
	/// Static class with constants that are used by <see cref="DataService.ServiceInformation.FeatureList"/> to indicate the availability of certain server features.
	/// </summary>
	public static class WellKnownServerFeatures
	{
		/// <summary>Server supports server side querying of merged measurements (like a primary key for measurement search)</summary>
		public const string MergeAttributes = "MergeAttributes";

		/// <summary>Server support the measurement aggregation feature</summary>
		public const string MeasurementAggregation = "MeasurementAggregation";

		/// <summary>Server supports the distinct search for specific attribute values</summary>
		public const string DistinctMeasurementSearch = "DistinctMeasurementSearch";

		/// <summary>The server database is readonly and cannot be modified</summary>
		public const string ReadOnlyDatabase = "ReadOnlyDB";

		/// <summary>The server does not support jobs.</summary>
		public const string JobEngineNotSupported = "JobEngineNotSupported";
	}
}