#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion

	/// <summary>
	/// This class is the base class for a measurement or measurement value search criteria.
	/// </summary>
	public abstract class AbstractMeasurementFilterAttributesDto
	{
		#region constants

		public const string PartUuidsParamName = "partUuids";
		public const string MeasurementUuidsParamName = "measurementUuids";

		protected const string DeepParamName = "deep";
		protected const string LimitResultParamName = "limitResult";
		protected const string LimitResultPerPartParamName = "limitResultPerPart";
		protected const string OrderByParamName = "order";

		protected const string SearchConditionParamName = "searchCondition";
		protected const string CaseSensitiveParamName = "caseSensitive";
		protected const string AggregationParamName = "aggregation";
		protected const string FromModificationDateParamName = "fromModificationDate";
		protected const string ToModificationDateParamName = "toModificationDate";

		private static readonly IReadOnlyList<OrderDto> DefaultOrder = new[] { new OrderDto( 4, OrderDirectionDto.Desc, EntityDto.Measurement ) };

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractMeasurementFilterAttributesDto"/> class.
		/// </summary>
		protected AbstractMeasurementFilterAttributesDto()
		{
			Deep = false;
			LimitResult = -1;
			LimitResultPerPart = -1;
			OrderBy = DefaultOrder;
			AggregationMeasurements = AggregationMeasurementSelectionDto.Default;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the list of part uuids that should be usded to restrict the measurement search.
		/// </summary>
		public IReadOnlyList<Guid> PartUuids { get; set; }

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
		/// Restricts the number of sub-items for each found part of the result (Default: -1).
		/// </summary>
		public int LimitResultPerPart { get; set; }

		/// <summary>
		/// Gets or sets the sort order of the resulting measurements.
		/// </summary>
		public IReadOnlyList<OrderDto> OrderBy { get; set; }

		/// <summary>
		/// Gets or sets the search condition that should be used.
		/// </summary>
		public GenericSearchConditionDto SearchCondition { get; set; }

		/// <summary>
		/// Gets or sets a flag if the search criteria should be compared case sensitive, case insensitive,
		/// or by database default (if left to null)
		/// </summary>
		public bool? CaseSensitive { get; set; }

		/// <summary>
		/// Gets or sets the list of measurement uuids that should be returned.
		/// </summary>
		public IReadOnlyList<Guid> MeasurementUuids { get; set; }

		/// <summary>
		/// Specifies what types of measurements will be returned (normal/aggregated measurements or both).
		/// </summary>
		public AggregationMeasurementSelectionDto AggregationMeasurements { get; set; }

		/// <summary>
		/// Specifies a date to select all measurements that where modified after that date. Please note that the system modification date
		/// (<see cref="SimpleMeasurementDto.LastModified"/>) is used and not the time attribute (<see cref="Definitions.WellKnownKeys.Measurement.Time"/>).
		/// </summary>
		public DateTime? FromModificationDate { get; set; }

		/// <summary>
		/// Specifies a date to select all measurements that where modified before that date. Please note that the system modification date
		/// (<see cref="SimpleMeasurementDto.LastModified"/>) is used and not the time attribute (<see cref="Definitions.WellKnownKeys.Measurement.Time"/>).
		/// </summary>
		public DateTime? ToModificationDate { get; set; }

		/// <summary>
		/// Specifies whether this filter will return all measurements of a part or a database (depending whether
		/// a part path is specified when fetching data).
		/// </summary>
		public bool IsUnrestricted => LimitResult <= 0 && SearchCondition == null && ( PartUuids == null || PartUuids.Count == 0 );

		#endregion

		#region methods

		public abstract IReadOnlyCollection<ParameterDefinition> ToParameterDefinition();

		/// <inheritdoc />
		public override string ToString()
		{
			return string.Join( " & ", ToParameterDefinition().Select( p => p.ToString() ) );
		}

		internal static string OrderByToString( IEnumerable<OrderDto> order )
		{
			return string.Join( ",", order.Select( o => $"{o.Attribute} {o.Direction}" ) );
		}

		#endregion
	}
}