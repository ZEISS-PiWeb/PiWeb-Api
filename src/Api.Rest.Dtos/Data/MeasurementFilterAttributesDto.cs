#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Xml;

	#endregion

	/// <summary>
	/// This class contains a measurement search criteria for a measurement search.
	/// </summary>
	public class MeasurementFilterAttributesDto : AbstractMeasurementFilterAttributesDto
	{
		#region constants

		private const string RequestedMeasurementAttributesParamName = "requestedmeasurementattributes";
		private const string StatisticsParamName = "statistics";

		private const string MergeAttributesParamName = "mergeattributes";
		private const string MergeConditionParamName = "mergecondition";
		private const string MergeMasterPartParamName = "mergemasterpart";

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MeasurementFilterAttributesDto"/> class.
		/// </summary>
		public MeasurementFilterAttributesDto()
		{
			RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelectionDto.True );

			MergeCondition = MeasurementMergeConditionDto.MeasurementsInAllParts;
			MergeMasterPart = null;

			Statistics = MeasurementStatisticsDto.None;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the selector for the measurement attributes.
		/// </summary>
		public AttributeSelector RequestedMeasurementAttributes { get; set; }

		/// <summary>
		/// Specifies if statistical information (<see cref="MeasurementStatisticsDto"/>: number characteristics OOT, OOT, etc.) should be returned.
		/// </summary>
		public MeasurementStatisticsDto Statistics { get; set; }

		/// <summary>
		/// Specifies the list of primary measurement keys to be used for joining measurements accross multiple parts on the server side.
		/// </summary>
		public IReadOnlyList<ushort> MergeAttributes { get; set; }

		/// <summary>
		/// Specifies the condition that must be adhered to
		/// when merging measurements accross multiple parts using a primary key.
		/// Default value is <code>MeasurementMergeCondition.MeasurementsInAllParts</code>.
		/// </summary>
		public MeasurementMergeConditionDto MergeCondition { get; set; }

		/// <summary>
		/// Specifies the part to be used as master part
		/// when merging measurements accross multiple parts using a primary key.
		/// Default value is <code>null</code>.
		/// </summary>
		public Guid? MergeMasterPart { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="MeasurementFilterAttributesDto"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="MeasurementFilterAttributesDto"/> with the parsed information.</returns>
		public static MeasurementFilterAttributesDto Parse(
			string partUuids,
			string measurementUuids,
			string deep,
			string limitResult,
			string order,
			string requestedMeasurementAttributes,
			string searchCondition,
			string statistics,
			string aggregation,
			string fromModificationDate,
			string toModificationDate,
			string mergeAttributes,
			string mergeCondition,
			string mergeMasterPart,
			string limitResultPerPart = "-1",
			string caseSensitive = null )
		{
			var items = new[]
			{
				ValueTuple.Create( PartUuidsParamName, partUuids ),
				ValueTuple.Create( MeasurementUuidsParamName, measurementUuids ),
				ValueTuple.Create( DeepParamName, deep ),
				ValueTuple.Create( LimitResultParamName, limitResult ),
				ValueTuple.Create( LimitResultPerPartParamName, limitResultPerPart ),
				ValueTuple.Create( OrderByParamName, order ),
				ValueTuple.Create( RequestedMeasurementAttributesParamName, requestedMeasurementAttributes ),
				ValueTuple.Create( SearchConditionParamName, searchCondition ),
				ValueTuple.Create( CaseSensitiveParamName, caseSensitive ),
				ValueTuple.Create( StatisticsParamName, statistics ),
				ValueTuple.Create( AggregationParamName, aggregation ),
				ValueTuple.Create( FromModificationDateParamName, fromModificationDate ),
				ValueTuple.Create( ToModificationDateParamName, toModificationDate ),
				ValueTuple.Create( MergeAttributesParamName, mergeAttributes ),
				ValueTuple.Create( MergeConditionParamName, mergeCondition ),
				ValueTuple.Create( MergeMasterPartParamName, mergeMasterPart )
			};

			var result = new MeasurementFilterAttributesDto();
			foreach( var (key, value) in items )
			{
				if( string.IsNullOrEmpty( value ) )
					continue;

				try
				{
					switch( key )
					{
						case PartUuidsParamName:
							result.PartUuids = RestClientHelper.ConvertStringToGuidList( value );
							break;
						case DeepParamName:
							result.Deep = bool.Parse( value );
							break;
						case MeasurementUuidsParamName:
							result.MeasurementUuids = RestClientHelper.ConvertStringToGuidList( value );
							break;
						case LimitResultParamName:
							result.LimitResult = int.Parse( value, CultureInfo.InvariantCulture );
							break;
						case LimitResultPerPartParamName:
							result.LimitResultPerPart = int.Parse( value, CultureInfo.InvariantCulture );
							break;
						case RequestedMeasurementAttributesParamName:
							result.RequestedMeasurementAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
							break;
						case OrderByParamName:
							result.OrderBy = value.Split( ',' ).Select( element => OrderDtoParser.Parse( element, EntityDto.Measurement ) ).ToArray();
							break;
						case SearchConditionParamName:
							result.SearchCondition = SearchConditionParser.Parse( value );
							break;
						case CaseSensitiveParamName:
							result.CaseSensitive = bool.Parse( value );
							break;
						case StatisticsParamName:
							result.Statistics = (MeasurementStatisticsDto)Enum.Parse( typeof( MeasurementStatisticsDto ), value );
							break;
						case AggregationParamName:
							result.AggregationMeasurements = (AggregationMeasurementSelectionDto)Enum.Parse( typeof( AggregationMeasurementSelectionDto ), value );
							break;
						case ToModificationDateParamName:
							result.ToModificationDate = XmlConvert.ToDateTime( value, XmlDateTimeSerializationMode.RoundtripKind );
							break;
						case FromModificationDateParamName:
							result.FromModificationDate = XmlConvert.ToDateTime( value, XmlDateTimeSerializationMode.RoundtripKind );
							break;
						case MergeAttributesParamName:
							result.MergeAttributes = RestClientHelper.ConvertStringToUInt16List( value );
							break;
						case MergeConditionParamName:
							result.MergeCondition = (MeasurementMergeConditionDto)Enum.Parse( typeof( MeasurementMergeConditionDto ), value );
							break;
						case MergeMasterPartParamName:
							result.MergeMasterPart = string.IsNullOrWhiteSpace( value ) ? (Guid?)null : Guid.Parse( value );
							break;
					}
				}
				catch( Exception ex )
				{
					throw new InvalidOperationException( $"Invalid filter value '{value}' for parameter '{key}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {"partUuids: [list of part uuids]\r\n" + "deep = [True|False]\r\n" + "limitResult: [short]\r\n" + "measurementUuids: [list of measurement uuids]\r\n" + "measurementAttributes: [attribute keys csv|Empty for all attributes]\r\n" + "orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" + "searchCondition:[search filter string]\r\n" + "caseSensitive: [True|False]\r\n" + "aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" + "statistics:[None|Simple|Detailed]\r\n" + "mergeAttributes:[list of measurement attributes]\r\n" + "mergeCondition: [None|MeasurementsInAtLeastTwoParts|MeasurementsInAllParts]\r\n" + "mergeMasterPart: [part uuid]\r\n" + "fromModificationDate:[Date]\r\n" + "toModificationDate:[Date]"}", ex );
				}
			}

			return result;
		}

		/// <summary>
		/// Converts this <see cref="MeasurementFilterAttributesDto"/> filter object into an equivalent <see cref="MeasurementValueFilterAttributesDto"/> object.
		/// </summary>
		public MeasurementValueFilterAttributesDto ToMeasurementValueFilterAttributes()
		{
			if( Statistics != MeasurementStatisticsDto.None )
				throw new InvalidOperationException( "Unable to create a 'MeasurementValueFilterAttributes' object when MeasurementFilterAttributes.Statistics is not MeasurementStatistics.None" );

			return new MeasurementValueFilterAttributesDto
			{
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
				LimitResultPerPart = LimitResultPerPart,
				OrderBy = OrderBy,
				RequestedMeasurementAttributes = RequestedMeasurementAttributes,
				SearchCondition = SearchCondition,
				MeasurementUuids = MeasurementUuids,
				AggregationMeasurements = AggregationMeasurements,
				FromModificationDate = FromModificationDate,
				ToModificationDate = ToModificationDate,
				MergeAttributes = MergeAttributes,
				MergeCondition = MergeCondition,
				MergeMasterPart = MergeMasterPart
			};
		}

		/// <summary>
		/// Creates a shallow clone of this filter.
		/// </summary>
		public MeasurementFilterAttributesDto Clone()
		{
			return new MeasurementFilterAttributesDto
			{
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
				LimitResultPerPart = LimitResultPerPart,
				OrderBy = OrderBy,
				RequestedMeasurementAttributes = RequestedMeasurementAttributes,
				SearchCondition = SearchCondition,
				CaseSensitive = CaseSensitive,
				MeasurementUuids = MeasurementUuids,
				AggregationMeasurements = AggregationMeasurements,
				FromModificationDate = FromModificationDate,
				ToModificationDate = ToModificationDate,
				MergeAttributes = MergeAttributes,
				MergeCondition = MergeCondition,
				MergeMasterPart = MergeMasterPart
			};
		}

		/// <inheritdoc />
		public override IReadOnlyCollection<ParameterDefinition> ToParameterDefinition()
		{
			var result = new List<ParameterDefinition>();

			if( PartUuids != null && PartUuids.Count > 0 )
				result.Add( ParameterDefinition.Create( PartUuidsParamName, RestClientHelper.ConvertGuidListToString( PartUuids ) ) );

			if( Deep )
				result.Add( ParameterDefinition.Create( DeepParamName, Deep.ToString() ) );

			if( LimitResult >= 0 )
				result.Add( ParameterDefinition.Create( LimitResultParamName, LimitResult.ToString() ) );

			if( LimitResultPerPart >= 0 )
				result.Add( ParameterDefinition.Create( LimitResultPerPartParamName, LimitResultPerPart.ToString() ) );

			if( MeasurementUuids != null && MeasurementUuids.Count > 0 )
				result.Add( ParameterDefinition.Create( MeasurementUuidsParamName, RestClientHelper.ConvertGuidListToString( MeasurementUuids ) ) );

			if( RequestedMeasurementAttributes != null && RequestedMeasurementAttributes.AllAttributes != AllAttributeSelectionDto.True && RequestedMeasurementAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedMeasurementAttributesParamName, RestClientHelper.ConvertUshortArrayToString( RequestedMeasurementAttributes.Attributes ) ) );

			if( OrderBy != null && OrderBy.Count > 0 )
				result.Add( ParameterDefinition.Create( OrderByParamName, OrderByToString( OrderBy ) ) );

			if( SearchCondition != null )
				result.Add( ParameterDefinition.Create( SearchConditionParamName, SearchConditionParser.GenericConditionToString( SearchCondition ) ) );

			if( CaseSensitive != null && CaseSensitive.Value )
				result.Add( ParameterDefinition.Create( CaseSensitiveParamName, CaseSensitive.ToString() ) );

			if( Statistics != MeasurementStatisticsDto.None )
				result.Add( ParameterDefinition.Create( StatisticsParamName, Statistics.ToString() ) );

			if( AggregationMeasurements != AggregationMeasurementSelectionDto.Default )
				result.Add( ParameterDefinition.Create( AggregationParamName, AggregationMeasurements.ToString() ) );

			if( FromModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( FromModificationDateParamName, XmlConvert.ToString( FromModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			if( ToModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( ToModificationDateParamName, XmlConvert.ToString( ToModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			if( MergeAttributes != null && MergeAttributes.Count > 0 )
				result.Add( ParameterDefinition.Create( MergeAttributesParamName, RestClientHelper.ConvertUshortArrayToString( MergeAttributes ) ) );

			if( MergeCondition != MeasurementMergeConditionDto.MeasurementsInAllParts )
				result.Add( ParameterDefinition.Create( MergeConditionParamName, MergeCondition.ToString() ) );

			if( MergeMasterPart != null )
				result.Add( ParameterDefinition.Create( MergeMasterPartParamName, MergeMasterPart.ToString() ) );

			return result;
		}

		#endregion
	}
}