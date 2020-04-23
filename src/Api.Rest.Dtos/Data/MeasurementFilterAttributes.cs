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

	#endregion

	/// <summary>
	/// Class that encapsulates the url parameter for a measurement value search via PiWeb-REST web service.
	/// </summary>
	public class MeasurementFilterAttributes : AbstractMeasurementFilterAttributes
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
		/// Initializes a new instance of the <see cref="MeasurementFilterAttributes"/> class.
		/// </summary>
		public MeasurementFilterAttributes()
		{
			RequestedMeasurementAttributes = new AttributeSelector( AllAttributeSelection.True );

			MergeCondition = MeasurementMergeCondition.MeasurementsInAllParts;
			MergeMasterPart = null;

			Statistics = MeasurementStatistics.None;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the selector for the measurement attributes.
		/// </summary>
		public AttributeSelector RequestedMeasurementAttributes { get; set; }

		/// <summary>
		/// Specifies if statistical information (<see cref="MeasurementStatistics"/>: number characteristics OOT, OOT, etc.) should be returned.
		/// </summary>
		public MeasurementStatistics Statistics { get; set; }

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
		/// Default value is <code>null</code>.
		/// </summary>
		public Guid? MergeMasterPart { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="MeasurementFilterAttributes"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="MeasurementFilterAttributes"/> with the parsed information.</returns>
		public static MeasurementFilterAttributes Parse(
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
			string mergeMasterPart )
		{
			var items = new[]
			{
				Tuple.Create( PartUuidsParamName, partUuids ),
				Tuple.Create( MeasurementUuidsParamName, measurementUuids ),
				Tuple.Create( DeepParamName, deep ),
				Tuple.Create( LimitResultParamName, limitResult ),
				Tuple.Create( OrderByParamName, order ),
				Tuple.Create( RequestedMeasurementAttributesParamName, requestedMeasurementAttributes ),
				Tuple.Create( SearchConditionParamName, searchCondition ),
				Tuple.Create( StatisticsParamName, statistics ),
				Tuple.Create( AggregationParamName, aggregation ),
				Tuple.Create( FromModificationDateParamName, fromModificationDate ),
				Tuple.Create( ToModificationDateParamName, toModificationDate ),
				Tuple.Create( MergeAttributesParamName, mergeAttributes ),
				Tuple.Create( MergeConditionParamName, mergeCondition ),
				Tuple.Create( MergeMasterPartParamName, mergeMasterPart )
			};

			var result = new MeasurementFilterAttributes();
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
							result.Statistics = (MeasurementStatistics)Enum.Parse( typeof( MeasurementStatistics ), value );
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
					throw new InvalidOperationException( $"Invalid filter value '{value}' for parameter '{key}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {"partUuids: [list of part uuids]\r\n" + "deep: [True|False]\r\n" + "limitResult: [short]\r\n" + "measurementUuids: [list of measurement uuids]\r\n" + "measurementAttributes: [attribute keys csv|Empty for all attributes]\r\n" + "orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" + "searchCondition:[search filter string]\r\n" + "aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" + "statistics:[None|Simple|Detailed]\r\n" + "mergeAttributes:[list of measurement attributes]\r\n" + "mergeCondition: [None|MeasurementsInAtLeastTwoParts|MeasurementsInAllParts]\r\n" + "mergeMasterPart: [part uuid]\r\n" + "fromModificationDate:[Date]\r\n" + "toModificationDate:[Date]"}", ex );
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
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
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
		public MeasurementFilterAttributes Clone()
		{
			return new MeasurementFilterAttributes
			{
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
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

			if( RequestedMeasurementAttributes != null && RequestedMeasurementAttributes.AllAttributes != AllAttributeSelection.True && RequestedMeasurementAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedMeasurementAttributesParamName, RestClientHelper.ConvertUshortArrayToString( RequestedMeasurementAttributes.Attributes ) ) );

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