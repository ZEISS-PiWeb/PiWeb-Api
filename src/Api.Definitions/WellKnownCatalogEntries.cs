#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Definitions
{
	/// <summary>
	/// Static class with well known catalog entries.
	/// </summary>
	public static class WellKnownCatalogEntries
	{
		#region class Characteristic

		/// <summary>
		/// Well known catalog entries for characteristics.
		/// </summary>
		public static class Characteristic
		{
			#region Type enum

			/// <summary>
			/// Catalog entries for the characteristic type.
			/// </summary>
			public enum Type : ushort
			{
				/// <summary>
				/// The characteristic is a variable characteristic (measurement values are double values).
				/// </summary>
				Variable = 0,

				/// <summary>
				/// The characteristic is a attributive characteristic (measurement values are integers that 
				/// specify the index of the corresponding measurement value catalog).
				/// </summary>
				Attributive = 1,

				/// <summary>
				/// The characteristic is a counting characteristic (measurement values are integers specify a count).
				/// </summary>
				Counter = 2,
			}

			#endregion
		}

		#endregion

		#region class Measurement

		/// <summary>
		/// Well known catalog entries for measurements.
		/// </summary>
		public static class Measurement
		{
			#region AggregationInterval enum

			/// <summary>
			/// Catalog entries for the measurement aggregation interval.
			/// </summary>
			public enum AggregationInterval : ushort
			{
				/// <summary>Undefined.</summary>
				Undefined = 0,

				/// <summary>Hourly.</summary>
				Hourly = 1,

				/// <summary>Daily.</summary>
				Daily = 2,

				/// <summary>Weekly.</summary>
				Weekly = 3,

				/// <summary>Monthly.</summary>
				Monthly = 4,
			}

			#endregion

			#region Status enum

			/// <summary>
			/// Catalog entries for the measurement status.
			/// </summary>
			public enum Status : ushort
			{
				/// <summary>Undefined.</summary>
				Undefined = 0,

				/// <summary>Valid</summary>
				Valid = 1,

				/// <summary>Blocked</summary>
				Blocked = 2
			}

			#endregion
		}

		#endregion
	}
}