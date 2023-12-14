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
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// This class contains a measurement value search criteria.
	/// </summary>
	public class MeasurementValueFilterAttributesDto : AbstractMeasurementFilterAttributesDto
	{
		#region constants

		private const string RequestedValueAttributesParamName = "requestedValueAttributes";
		private const string RequestedMeasurementAttributesParamName = "requestedMeasurementAttributes";
		public const string CharacteristicsUuidListParamName = "characteristicUuids";

		private const string MergeAttributesParamName = "mergeAttributes";
		private const string MergeConditionParamName = "mergeCondition";
		private const string MergeMasterPartParamName = "mergeMasterPart";

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MeasurementValueFilterAttributesDto"/> class.
		/// </summary>
		public MeasurementValueFilterAttributesDto()
		{
			RequestedValueAttributes = new AttributeSelector( AllAttributeSelectionDto.True );
			RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelectionDto.True );

			MergeCondition = MeasurementMergeConditionDto.MeasurementsInAllParts;
			MergeMasterPart = null;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the selector for the measurement value attributes.
		/// </summary>
		public AttributeSelector RequestedValueAttributes { get; set; }

		/// <summary>
		/// Gets or sets the selector for the measurement attributes.
		/// </summary>
		public AttributeSelector RequestedMeasurementAttributes { get; set; }

		/// <summary>
		/// Gets or sets the list of characteristic uuids that should be returned.
		/// </summary>
		public IReadOnlyCollection<Guid> CharacteristicsUuidList { get; set; }

		/// <summary>
		/// Specifies the list of primary measurement keys to be used for joining measurements accross multiple parts on the server side.
		/// </summary>
		public IReadOnlyCollection<ushort> MergeAttributes { get; set; }

		/// <summary>
		/// Specifies the condition that must be adhered to
		/// when merging measurements accross multiple parts using a primary key.
		/// Default value is <code>MeasurementMergeCondition.MeasurementsInAllParts</code>.
		/// </summary>
		public MeasurementMergeConditionDto MergeCondition { get; set; }

		/// <summary>
		/// Specifies the part to be used as master part
		/// when merging measurements accross multiple parts using a primary key.
		/// Default value is <code>false</code>.
		/// </summary>
		public Guid? MergeMasterPart { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="MeasurementValueFilterAttributesDto"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="MeasurementValueFilterAttributesDto"/> with the parsed information.</returns>
		[NotNull]
		public static MeasurementValueFilterAttributesDto Parse(
			string partUuids,
			string measurementUuids,
			string characteristicUuids,
			string deep,
			string limitResult,
			string order,
			string requestedMeasurementAttributes,
			string requestedValueAttributes,
			string searchCondition,
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
				ValueTuple.Create( CharacteristicsUuidListParamName, characteristicUuids ),
				ValueTuple.Create( DeepParamName, deep ),
				ValueTuple.Create( LimitResultParamName, limitResult ),
				ValueTuple.Create( LimitResultPerPartParamName, limitResultPerPart ),
				ValueTuple.Create( OrderByParamName, order ),
				ValueTuple.Create( RequestedValueAttributesParamName, requestedValueAttributes ),
				ValueTuple.Create( RequestedMeasurementAttributesParamName, requestedMeasurementAttributes ),
				ValueTuple.Create( SearchConditionParamName, searchCondition ),
				ValueTuple.Create( CaseSensitiveParamName, caseSensitive ),
				ValueTuple.Create( AggregationParamName, aggregation ),
				ValueTuple.Create( FromModificationDateParamName, fromModificationDate ),
				ValueTuple.Create( ToModificationDateParamName, toModificationDate ),
				ValueTuple.Create( MergeAttributesParamName, mergeAttributes ),
				ValueTuple.Create( MergeConditionParamName, mergeCondition ),
				ValueTuple.Create( MergeMasterPartParamName, mergeMasterPart )
			};

			var result = new MeasurementValueFilterAttributesDto();
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
						case CharacteristicsUuidListParamName:
							result.CharacteristicsUuidList = RestClientHelper.ConvertStringToGuidList( value );
							break;
						case LimitResultParamName:
							result.LimitResult = short.Parse( value, CultureInfo.InvariantCulture );
							break;
						case LimitResultPerPartParamName:
							result.LimitResultPerPart = int.Parse( value, CultureInfo.InvariantCulture );
							break;
						case RequestedValueAttributesParamName:
							result.RequestedValueAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
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
					throw new InvalidOperationException( $"Invalid filter value '{value}' for parameter '{key}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {"partUuids = [list of part uuids]\r\n" + "deep = [True|False]\r\n" + "limitResult = [short]\r\n" + "measurementUuids = [list of measurement uuids]\r\n" + "characteristicUuids = [list of characteristic uuids]\r\n" + "valueAttributes = [attribute keys csv|Empty for all attributes]\r\n" + "measurementAttributes = [attribute keys csv|Empty for all attributes]\r\n" + "orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" + "searchCondition:[search filter string]\r\n" + "caseSensitive = [True|False]\r\n" + "aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" + "mergeAttributes:[list of measurement attributes]\r\n" + "mergeCondition: [None|MeasurementsInAtLeastTwoParts|MeasurementsInAllParts]\r\n" + "mergeMasterPart: [part uuid]\r\n" + "fromModificationDate:[Date]\r\n" + "toModificationDate:[Date]"}", ex );
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a clone of this filter.
		/// </summary>
		public MeasurementValueFilterAttributesDto Clone()
		{
			return new MeasurementValueFilterAttributesDto
			{
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
				LimitResultPerPart = LimitResultPerPart,
				CharacteristicsUuidList = CharacteristicsUuidList,
				OrderBy = OrderBy,
				RequestedValueAttributes = RequestedValueAttributes,
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

			if( CharacteristicsUuidList != null && CharacteristicsUuidList.Count > 0 )
				result.Add( ParameterDefinition.Create( CharacteristicsUuidListParamName, RestClientHelper.ConvertGuidListToString( CharacteristicsUuidList ) ) );

			if( RequestedValueAttributes != null && RequestedValueAttributes.AllAttributes != AllAttributeSelectionDto.True && RequestedValueAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedValueAttributesParamName, RestClientHelper.ConvertUshortArrayToString( RequestedValueAttributes.Attributes ) ) );

			if( RequestedMeasurementAttributes != null && RequestedMeasurementAttributes.AllAttributes != AllAttributeSelectionDto.True && RequestedMeasurementAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedMeasurementAttributesParamName, RestClientHelper.ConvertUshortArrayToString( RequestedMeasurementAttributes.Attributes ) ) );

			if( OrderBy != null && OrderBy.Count > 0 )
				result.Add( ParameterDefinition.Create( OrderByParamName, OrderByToString( OrderBy ) ) );

			if( SearchCondition != null )
				result.Add( ParameterDefinition.Create( SearchConditionParamName, SearchConditionParser.GenericConditionToString( SearchCondition ) ) );

			if( CaseSensitive != null && CaseSensitive.Value )
				result.Add( ParameterDefinition.Create( CaseSensitiveParamName, CaseSensitive.ToString() ) );

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