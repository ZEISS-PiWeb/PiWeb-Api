#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
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
	/// Class that encapsulates the url parameter for a measurement search via PiWeb-REST web service.
	/// </summary>
	public class MeasurementValueFilterAttributes : AbstractMeasurementFilterAttributes
	{
		#region constants

		private const string RequestedValueAttributesParamName = "requestedvalueattributes";
		private const string RequestedMeasurementAttributesParamName = "requestedmeasurementattributes";
		public const string CharacteristicsUuidListParamName = "characteristicuuids";

		private const string MergeAttributesParamName = "mergeattributes";
		private const string MergeConditionParamName = "mergecondition";
		private const string MergeMasterPartParamName = "mergemasterpart";

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="MeasurementValueFilterAttributes"/> class.
		/// </summary>
		public MeasurementValueFilterAttributes()
		{
			RequestedValueAttributes = new AttributeSelector( AllAttributeSelection.True );
			RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelection.True );

			MergeCondition = MeasurementMergeCondition.MeasurementsInAllParts;
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
		public Guid[] CharacteristicsUuidList { get; set; }

		/// <summary>
		/// Specifies the list of primary measurement keys to be used for joining measurements accross multiple parts on the server side.
		/// </summary>
		public ushort[] MergeAttributes { get; set; }

		/// <summary>
		/// Specifies the condition that must be adhered to
		/// when merging measurements accross multiple parts using a primary key.
		/// Default value is <code>MeasurementMergeCondition.MeasurementsInAllParts</code>.
		/// </summary>
		public MeasurementMergeCondition MergeCondition { get; set; }

		/// <summary>
		/// Specifies the part to be used as master part
		/// when merging measurements accross multiple parts using a primary key.
		/// Default value is <code>false</code>.
		/// </summary>
		public Guid? MergeMasterPart { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="MeasurementValueFilterAttributes"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="MeasurementValueFilterAttributes"/> with the parsed information.</returns>
		[NotNull]
		public static MeasurementValueFilterAttributes Parse(
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
			string mergeMasterPart )
		{
			var items = new[]
			{
				Tuple.Create( PartUuidsParamName, partUuids ),
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
				Tuple.Create( ToModificationDateParamName, toModificationDate ),
				Tuple.Create( MergeAttributesParamName, mergeAttributes ),
				Tuple.Create( MergeConditionParamName, mergeCondition ),
				Tuple.Create( MergeMasterPartParamName, mergeMasterPart )
			};

			var result = new MeasurementValueFilterAttributes();
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
						case RequestedValueAttributesParamName:
							result.RequestedValueAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
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
						case AggregationParamName:
							result.AggregationMeasurements = (AggregationMeasurementSelection)Enum.Parse( typeof( AggregationMeasurementSelection ), value );
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
							result.MergeCondition = (MeasurementMergeCondition)Enum.Parse( typeof( MeasurementMergeCondition ), value );
							break;
						case MergeMasterPartParamName:
							result.MergeMasterPart = string.IsNullOrWhiteSpace( value ) ? (Guid?)null : Guid.Parse( value );
							break;
					}
				}
				catch( Exception ex )
				{
					throw new InvalidOperationException( $"Invalid filter value '{value}' for parameter '{key}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {"partUuids = [list of part uuids]\r\n" + "deep = [True|False]\r\n" + "limitResult = [short]\r\n" + "measurementUuids = [list of measurement uuids]\r\n" + "characteristicUuids = [list of characteristic uuids]\r\n" + "valueAttributes = [attribute keys csv|Empty for all attributes]\r\n" + "measurementAttributes = [attribute keys csv|Empty for all attributes]\r\n" + "orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" + "searchCondition:[search filter string]\r\n" + "aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" + "mergeAttributes:[list of measurement attributes]\r\n" + "mergeCondition: [None|MeasurementsInAtLeastTwoParts|MeasurementsInAllParts]\r\n" + "mergeMasterPart: [part uuid]\r\n" + "fromModificationDate:[Date]\r\n" + "toModificationDate:[Date]"}", ex );
				}
			}

			return result;
		}

		/// <summary>
		/// Creates a clone of this filter.
		/// </summary>
		public MeasurementValueFilterAttributes Clone()
		{
			return new MeasurementValueFilterAttributes
			{
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
				CharacteristicsUuidList = CharacteristicsUuidList,
				OrderBy = OrderBy,
				RequestedValueAttributes = RequestedValueAttributes,
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

		/// <inheritdoc />
		public override ParameterDefinition[] ToParameterDefinition()
		{
			var result = new List<ParameterDefinition>();

			if( PartUuids != null && PartUuids.Length > 0 )
				result.Add( ParameterDefinition.Create( PartUuidsParamName, RestClientHelper.ConvertGuidListToString( PartUuids ) ) );

			if( Deep )
				result.Add( ParameterDefinition.Create( DeepParamName, Deep.ToString() ) );

			if( LimitResult >= 0 )
				result.Add( ParameterDefinition.Create( LimitResultParamName, LimitResult.ToString() ) );

			if( MeasurementUuids != null && MeasurementUuids.Length > 0 )
				result.Add( ParameterDefinition.Create( MeasurementUuidsParamName, RestClientHelper.ConvertGuidListToString( MeasurementUuids ) ) );

			if( CharacteristicsUuidList != null && CharacteristicsUuidList.Length > 0 )
				result.Add( ParameterDefinition.Create( CharacteristicsUuidListParamName, RestClientHelper.ConvertGuidListToString( CharacteristicsUuidList ) ) );

			if( RequestedValueAttributes != null && RequestedValueAttributes.AllAttributes != AllAttributeSelection.True && RequestedValueAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedValueAttributesParamName, RestClientHelper.ConvertUshortArrayToString( RequestedValueAttributes.Attributes ) ) );

			if( RequestedMeasurementAttributes != null && RequestedMeasurementAttributes.AllAttributes != AllAttributeSelection.True && RequestedMeasurementAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedMeasurementAttributesParamName, RestClientHelper.ConvertUshortArrayToString( RequestedMeasurementAttributes.Attributes ) ) );

			if( OrderBy != null && OrderBy.Length > 0 )
				result.Add( ParameterDefinition.Create( OrderByParamName, OrderByToString( OrderBy ) ) );

			if( SearchCondition != null )
				result.Add( ParameterDefinition.Create( SearchConditionParamName, SearchConditionParser.GenericConditionToString( SearchCondition ) ) );

			if( AggregationMeasurements != AggregationMeasurementSelection.Default )
				result.Add( ParameterDefinition.Create( AggregationParamName, AggregationMeasurements.ToString() ) );

			if( FromModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( FromModificationDateParamName, XmlConvert.ToString( FromModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			if( ToModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( ToModificationDateParamName, XmlConvert.ToString( ToModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			if( MergeAttributes != null && MergeAttributes.Length > 0 )
				result.Add( ParameterDefinition.Create( MergeAttributesParamName, RestClientHelper.ConvertUshortArrayToString( MergeAttributes ) ) );

			if( MergeCondition != MeasurementMergeCondition.MeasurementsInAllParts )
				result.Add( ParameterDefinition.Create( MergeConditionParamName, MergeCondition.ToString() ) );

			if( MergeMasterPart != null )
				result.Add( ParameterDefinition.Create( MergeMasterPartParamName, MergeMasterPart.ToString() ) );

			return result.ToArray();
		}

		#endregion
	}
}