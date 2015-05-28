#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Xml;
	using Common.Client;
	using Common.Data;
	using DataService;

	#endregion

	/// <summary>
	/// Class that encapsulates the url parameter for a measurement value search via PiWeb-REST web service.
	/// </summary>
	public class MeasurementFilterAttributes
	{
		#region constants

		private const string DeepParamName = "deep";
		private const string LimitResultParamName = "limitresult";
		private const string OrderByParamName = "orderby";
		private const string RequestedMeasurementAttributesParamName = "measurementattributes";
		private const string MeasurementUuidsParamName = "measurementuuids";
		private const string SearchConditionParamName = "searchcondition";
		private const string StatisticsParamName = "statistics";
		private const string AggregationParamName = "aggregation";
		private const string FromModificationDateParamName = "frommodificationdate";
		private const string ToModificationDateParamName = "tomodificationdate";

		#endregion

		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public MeasurementFilterAttributes()
		{
			Deep = false;
			LimitResult = -1;
			OrderBy = new[] { new Order( 4, OrderDirection.Desc, Entity.Measurement ) };
			RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelection.True );
			Statistics = MeasurementStatistics.None;
			AggregationMeasurements = AggregationMeasurementSelection.Default;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets a flag if the search should only be performed for the specified part (<code>false</code>) or 
		/// if the part and child parts (<code>true</code>) should be searched.
		/// </summary>
		public bool Deep { get; set; }

		/// <summary>
		/// Gets or sets the maximum number of measurements that should be returned. If this value is -1, no limit is used.
		/// </summary>
		public short LimitResult { get; set; }

		/// <summary>
		/// Gets or sets the sort order of the resulting measurements.
		/// </summary>
		public Order[] OrderBy { get; set; }

		/// <summary>
		/// Gets or sets the selector for the measurement attributes.
		/// </summary>
		public AttributeSelector RequestedMeasurementAttributes { get; set; }

		/// <summary>
		/// Gets or sets the search condition that should be used.
		/// </summary>
		public GenericSearchCondition SearchCondition { get; set; }

		/// <summary>
		/// Gets or sets the list of measurement uuids that should be returned.
		/// </summary>
		public Guid[] MeasurementUuids { get; set; }

		/// <summary>
		/// Specifies if statistical information (<see cref="MeasurementStatistics"/>: number characteristics OOT, OOT, etc.) should be returned.
		/// </summary>
		public MeasurementStatistics Statistics { get; set; }

		/// <summary>
		/// Specifies what types of measurements will be returned (normal/aggregated measurements or both). 
		/// </summary>
		public AggregationMeasurementSelection AggregationMeasurements { get; set; }

		/// <summary>
		/// Specifies a date to select all measurements that where modified after that date. Please note that the system modification date 
		/// (<see cref="SimpleMeasurement.LastModified"/>) is used and not the time attribute (<see cref="WellKnownKeys.Measurement.Time"/>).
		/// </summary>
		public DateTime? FromModificationDate { get; set; }

		/// <summary>
		/// Specifies a date to select all measurements that where modified before that date. Please note that the system modification date 
		/// (<see cref="SimpleMeasurement.LastModified"/>) is used and not the time attribute (<see cref="WellKnownKeys.Measurement.Time"/>).
		/// </summary>
		public DateTime? ToModificationDate { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="MeasurementFilterAttributes"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="MeasurementFilterAttributes"/> with the parsed information.</returns>
		public static MeasurementFilterAttributes Parse( string measurementUuids, string deep, string limitResult, string order, string requestedMeasurementAttributes, string searchCondition, string statistics, string aggregation, string fromModificationDate, string toModificationDate )
		{
			var items = new[]
			{
				Tuple.Create( MeasurementUuidsParamName, measurementUuids ),
				Tuple.Create( DeepParamName, deep ),
				Tuple.Create( LimitResultParamName, limitResult ),
				Tuple.Create( OrderByParamName, order ),
				Tuple.Create( RequestedMeasurementAttributesParamName, requestedMeasurementAttributes ),
				Tuple.Create( SearchConditionParamName, searchCondition ),
				Tuple.Create( StatisticsParamName, statistics ),
				Tuple.Create( AggregationParamName, aggregation ),
				Tuple.Create( FromModificationDateParamName, fromModificationDate ),
				Tuple.Create( ToModificationDateParamName, toModificationDate )
			};

			var result = new MeasurementFilterAttributes();
			foreach( var item in items )
			{
				var key = item.Item1;
				var value = item.Item2;

				if( string.IsNullOrEmpty( value ) )
					continue;
				
				try
				{
					switch( key )
					{
						case DeepParamName:
							result.Deep = bool.Parse( value );
							break;
						case MeasurementUuidsParamName:
							result.MeasurementUuids = RestClientHelper.ConvertStringToGuidList( value );
							break;
						case LimitResultParamName:
							result.LimitResult = short.Parse( value, System.Globalization.CultureInfo.InvariantCulture );
							break;
						case RequestedMeasurementAttributesParamName:
							result.RequestedMeasurementAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
							break;
						case OrderByParamName:
							result.OrderBy = value.Split( ',' ).Select( ParseOrderBy ).ToArray();
							break;
						case SearchConditionParamName:
							result.SearchCondition = SearchConditionParser.Parse( value );
							break;
						case StatisticsParamName:
							result.Statistics = ( MeasurementStatistics )Enum.Parse( typeof( MeasurementStatistics ), value );
							break;
						case AggregationParamName:
							result.AggregationMeasurements = ( AggregationMeasurementSelection )Enum.Parse( typeof( AggregationMeasurementSelection ), value );
							break;
						case ToModificationDateParamName:
							result.ToModificationDate = XmlConvert.ToDateTime( value, XmlDateTimeSerializationMode.RoundtripKind );
							break;
						case FromModificationDateParamName:
							result.FromModificationDate = XmlConvert.ToDateTime( value, XmlDateTimeSerializationMode.RoundtripKind );
							break;
					}
				}
				catch( Exception ex )
				{
					throw new InvalidOperationException( string.Format( "Invalid filter value '{0}' for parameter '{1}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {2}",
						value, key,
						"deep = [True|False]\r\n" +
						"limitResult = [short]\r\n" +
						"measurementUuids = [list of measurement uuids]\r\n" +
						"measurementAttributes = [attribute keys csv|Empty for all attributes]\r\n" +
						"orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" +
						"searchCondition:[search filter string]\r\n" +
						"aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" + 
						"statistics:[None|Simple|Detailed]\r\n" +
						"fromModificationDate:[Date]\r\n" +
						"toModificationDate:[Date]" ), ex );
				}
			}
			return result;
		}

		/// <summary>
		/// Converts this <see cref="MeasurementFilterAttributes"/> filter object into an equivalent <see cref="MeasurementValueFilterAttributes"/> object.
		/// </summary>
		public MeasurementValueFilterAttributes ToMeasurementValueFilterAttributes()
		{
			if( Statistics != MeasurementStatistics.None )
				throw new InvalidOperationException( "Unable to create a 'MeasurementValueFilterAttributes' object when MeasurementFilterAttributes.Statistics is not MeasurementStatistics.None" );

			return new MeasurementValueFilterAttributes
			{
				Deep = Deep,
				LimitResult = LimitResult,
				OrderBy = OrderBy,
				RequestedMeasurementAttributes = RequestedMeasurementAttributes,
				SearchCondition = SearchCondition,
				MeasurementUuids = MeasurementUuids,
				AggregationMeasurements = AggregationMeasurements,
				FromModificationDate = FromModificationDate,
				ToModificationDate = ToModificationDate
			};
		}

		/// <summary>
		/// Creates a <see cref="ParameterDefinition"/> list that represents this filter.
		/// </summary>
		public ParameterDefinition[] ToParameterDefinition()
		{
			var result = new List<ParameterDefinition>();

			if( Deep )
				result.Add( ParameterDefinition.Create( DeepParamName, Deep.ToString() ) );

			if( LimitResult >= 0 )
				result.Add( ParameterDefinition.Create( LimitResultParamName, LimitResult.ToString() ) );

			if( MeasurementUuids != null && MeasurementUuids.Length > 0 )
				result.Add( ParameterDefinition.Create( MeasurementUuidsParamName, RestClientHelper.ConvertGuidListToString( MeasurementUuids ) ) );

			if( RequestedMeasurementAttributes != null && RequestedMeasurementAttributes.AllAttributes != AllAttributeSelection.True && RequestedMeasurementAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedMeasurementAttributesParamName, RestClientHelper.ConvertUInt16ListToString( RequestedMeasurementAttributes.Attributes ) ) );
				
			if( OrderBy != null && OrderBy.Length > 0 )
				result.Add( ParameterDefinition.Create( OrderByParamName, OrderByToString( OrderBy ) ) );
				
			if( SearchCondition != null )
				result.Add( ParameterDefinition.Create( SearchConditionParamName, SearchConditionParser.GenericConditionToString( SearchCondition ) ) );
				
			if( Statistics != MeasurementStatistics.None )
				result.Add( ParameterDefinition.Create( StatisticsParamName, Statistics.ToString() ) );

			if( AggregationMeasurements != AggregationMeasurementSelection.Default )
				result.Add( ParameterDefinition.Create( AggregationParamName, AggregationMeasurements.ToString() ) );

			if( FromModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( FromModificationDateParamName, XmlConvert.ToString( FromModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			if( ToModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( ToModificationDateParamName, XmlConvert.ToString( ToModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			return result.ToArray();
		}

		/// <summary>
		/// Overriden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return string.Join( " & ", ToParameterDefinition().Select( p => p.ToString() ) );
		}

		internal static string OrderByToString( Order[] order )
		{
			return string.Join( ",", order.Select( o => string.Format( "{0} {1}", o.Attribute, o.Direction ) ) );
		}

		internal static Order ParseOrderBy( string value )
		{
			var items = value.Split( ' ' );

			var key = ushort.Parse( items[ 0 ], System.Globalization.CultureInfo.InvariantCulture );
			var direction = string.Equals( "asc", items[ 1 ], StringComparison.OrdinalIgnoreCase ) ? OrderDirection.Asc : OrderDirection.Desc;

			return new Order( key, direction, Entity.Measurement );
		}

		#endregion
	}
}