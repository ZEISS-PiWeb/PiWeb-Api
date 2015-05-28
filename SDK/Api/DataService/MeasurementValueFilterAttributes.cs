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
	/// Class that encapsulates the url parameter for a measurement search via PiWeb-REST web service.
	/// </summary>
	public class MeasurementValueFilterAttributes
	{
		#region constants

		private const string DeepParamName = "deep";
		private const string LimitResultParamName = "limitresult";
		private const string OrderByParamName = "orderby";
		private const string RequestedValueAttributesParamName = "valueattributes";
		private const string RequestedMeasurementAttributesParamName = "measurementattributes";
		private const string SearchConditionParamName = "searchcondition";
		private const string MeasurementUuidsParamName = "measurementuuids";
		private const string CharacteristicsUuidListParamName = "characteristicsuuidlist";
		private const string AggregationParamName = "aggregation";
		private const string FromModificationDateParamName = "frommodificationdate";
		private const string ToModificationDateParamName = "tomodificationdate";

		#endregion
		
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public MeasurementValueFilterAttributes()
		{
			Deep = false;
			LimitResult = -1;
			OrderBy = new[] { new Order( 4, OrderDirection.Desc, Entity.Measurement ) };
			RequestedValueAttributes = new AttributeSelector( AllAttributeSelection.True );
			RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelection.True );
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
		/// Gets or sets the selector for the measurement value attributes.
		/// </summary>
		public AttributeSelector RequestedValueAttributes { get; set; }

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
		/// Gets or sets the list of characteristic uuids that should be returned.
		/// </summary>
		public Guid[] CharacteristicsUuidList { get; set; }

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
		/// Parses the filter and returns a <see cref="MeasurementValueFilterAttributes"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="MeasurementValueFilterAttributes"/> with the parsed information.</returns>
		public static MeasurementValueFilterAttributes Parse( string measurementUuids, string characteristicUuids, string deep, string limitResult, string order, string requestedMeasurementAttributes, string requestedValueAttributes, string searchCondition, string aggregation, string fromModificationDate, string toModificationDate )
		{
			var items = new[]
			{
				Tuple.Create( MeasurementUuidsParamName, measurementUuids ),
				Tuple.Create( CharacteristicsUuidListParamName, characteristicUuids ),
				Tuple.Create( DeepParamName, deep ),
				Tuple.Create( LimitResultParamName, limitResult ),
				Tuple.Create( OrderByParamName, order ),
				Tuple.Create( RequestedValueAttributesParamName, requestedValueAttributes ),
				Tuple.Create( RequestedMeasurementAttributesParamName, requestedMeasurementAttributes ),
				Tuple.Create( SearchConditionParamName, searchCondition ),
				Tuple.Create( AggregationParamName, aggregation ),
				Tuple.Create( FromModificationDateParamName, fromModificationDate ),
				Tuple.Create( ToModificationDateParamName, toModificationDate )
			};

			var result = new MeasurementValueFilterAttributes();
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
						case CharacteristicsUuidListParamName:
							result.CharacteristicsUuidList = RestClientHelper.ConvertStringToGuidList( value );
							break;
						case LimitResultParamName:
							result.LimitResult = short.Parse( value, System.Globalization.CultureInfo.InvariantCulture );
							break;
						case RequestedValueAttributesParamName:
							result.RequestedValueAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
							break;
						case RequestedMeasurementAttributesParamName:
							result.RequestedMeasurementAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
							break;
						case OrderByParamName:
							result.OrderBy = value.Split( ',' ).Select( MeasurementFilterAttributes.ParseOrderBy ).ToArray();
							break;
						case SearchConditionParamName:
							result.SearchCondition = SearchConditionParser.Parse( value );
							break;
						case AggregationParamName:
							result.AggregationMeasurements = (AggregationMeasurementSelection)Enum.Parse( typeof( AggregationMeasurementSelection ), value );
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
						"characteristicUuids = [list of characteristic uuids]\r\n" +
						"valueAttributes = [attribute keys csv|Empty for all attributes]\r\n" +
						"measurementAttributes = [attribute keys csv|Empty for all attributes]\r\n" +
						"orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" +
						"searchCondition:[search filter string]\r\n" +
						"aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" +
						"fromModificationDate:[Date]\r\n" +
						"toModificationDate:[Date]" ), ex );
				}
			}
			return result;
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

			if( CharacteristicsUuidList != null && CharacteristicsUuidList.Length > 0 )
				result.Add( ParameterDefinition.Create( CharacteristicsUuidListParamName, RestClientHelper.ConvertGuidListToString( CharacteristicsUuidList ) ) );

			if( RequestedValueAttributes != null && RequestedValueAttributes.AllAttributes != AllAttributeSelection.True && RequestedValueAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedValueAttributesParamName, RestClientHelper.ConvertUInt16ListToString( RequestedValueAttributes.Attributes ) ) );

			if( RequestedMeasurementAttributes != null && RequestedMeasurementAttributes.AllAttributes != AllAttributeSelection.True && RequestedMeasurementAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedMeasurementAttributesParamName, RestClientHelper.ConvertUInt16ListToString( RequestedMeasurementAttributes.Attributes ) ) );

			if( OrderBy != null && OrderBy.Length > 0 )
				result.Add( ParameterDefinition.Create( OrderByParamName, MeasurementFilterAttributes.OrderByToString( OrderBy ) ) );

			if( SearchCondition != null )
				result.Add( ParameterDefinition.Create( SearchConditionParamName, SearchConditionParser.GenericConditionToString( SearchCondition ) ) );

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

		#endregion
	}
}