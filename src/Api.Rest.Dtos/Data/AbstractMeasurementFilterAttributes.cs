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
	using System.Globalization;
	using System.Linq;

	#endregion

	public abstract class AbstractMeasurementFilterAttributes
	{
		#region constants

		public const string PartUuidsParamName = "partuuids";
		protected const string DeepParamName = "deep";
		protected const string LimitResultParamName = "limitresult";
		protected const string OrderByParamName = "order";
		public const string MeasurementUuidsParamName = "measurementuuids";
		protected const string SearchConditionParamName = "searchcondition";
		protected const string AggregationParamName = "aggregation";
		protected const string FromModificationDateParamName = "frommodificationdate";
		protected const string ToModificationDateParamName = "tomodificationdate";

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractMeasurementFilterAttributes"/> class.
		/// </summary>
		protected AbstractMeasurementFilterAttributes()
		{
			Deep = false;
			LimitResult = -1;
			OrderBy = new[] { new Order( 4, OrderDirection.Desc, Entity.Measurement ) };
			AggregationMeasurements = AggregationMeasurementSelection.Default;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the list of part uuids that should be usded to restrict the measurement search.
		/// </summary>
		public Guid[] PartUuids { get; set; }

		/// <summary>
		/// Gets or sets a flag if the search should only be performed for the specified part (<code>false</code>) or 
		/// if the part and child parts (<code>true</code>) should be searched.
		/// </summary>
		public bool Deep { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of measurements that should be returned. If this value is -1, no limit is used.
		/// </summary>
		public int LimitResult { get; set; }

		/// <summary>
		/// Gets or sets the sort order of the resulting measurements.
		/// </summary>
		public Order[] OrderBy { get; set; }

		/// <summary>
		/// Gets or sets the search condition that should be used.
		/// </summary>
		public GenericSearchCondition SearchCondition { get; set; }

		/// <summary>
		/// Gets or sets the list of measurement uuids that should be returned.
		/// </summary>
		public Guid[] MeasurementUuids { get; set; }

		/// <summary>
		/// Specifies what types of measurements will be returned (normal/aggregated measurements or both). 
		/// </summary>
		public AggregationMeasurementSelection AggregationMeasurements { get; set; }

		/// <summary>
		/// Specifies a date to select all measurements that where modified after that date. Please note that the system modification date 
		/// (<see cref="SimpleMeasurement.LastModified"/>) is used and not the time attribute (<see cref="Definitions.WellKnownKeys.Measurement.Time"/>).
		/// </summary>
		public DateTime? FromModificationDate { get; set; }

		/// <summary>
		/// Specifies a date to select all measurements that where modified before that date. Please note that the system modification date 
		/// (<see cref="SimpleMeasurement.LastModified"/>) is used and not the time attribute (<see cref="Definitions.WellKnownKeys.Measurement.Time"/>).
		/// </summary>
		public DateTime? ToModificationDate { get; set; }

		/// <summary>
		/// Specifies whether this filter will return all measurements of a part or a database (depending whether 
		/// a part path is specified when fetching data).
		/// </summary>
		public bool IsUnrestricted => LimitResult <= 0 && SearchCondition == null && ( PartUuids == null || PartUuids.Length == 0 );

		#endregion

		#region methods

		public abstract ParameterDefinition[] ToParameterDefinition();

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Join( " & ", ToParameterDefinition().Select( p => p.ToString() ) );
		}

		internal static string OrderByToString( Order[] order )
		{
			return string.Join( ",", order.Select( o => $"{o.Attribute} {o.Direction}" ) );
		}

		internal static Order ParseOrderBy( string value )
		{
			var items = value.Split( ' ' );

			var key = ushort.Parse( items[ 0 ], CultureInfo.InvariantCulture );
			var direction = string.Equals( "asc", items[ 1 ], StringComparison.OrdinalIgnoreCase ) ? OrderDirection.Asc : OrderDirection.Desc;

			return new Order( key, direction, Entity.Measurement );
		}

		#endregion
	}
}