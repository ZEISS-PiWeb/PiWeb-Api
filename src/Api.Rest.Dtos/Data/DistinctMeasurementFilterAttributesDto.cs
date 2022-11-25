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
	/// This class contains a measurement search criteria for a distinct measurement search.
	/// </summary>
	public class DistinctMeasurementFilterAttributesDto : AbstractMeasurementFilterAttributesDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DistinctMeasurementFilterAttributesDto"/> class.
		/// </summary>
		public DistinctMeasurementFilterAttributesDto() { }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="DistinctMeasurementFilterAttributesDto"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="DistinctMeasurementFilterAttributesDto"/> with the parsed information.</returns>
		public static DistinctMeasurementFilterAttributesDto Parse(
			string partUuids,
			string measurementUuids,
			string deep,
			string limitResult,
			string order,
			string searchCondition,
			string aggregation,
			string fromModificationDate,
			string toModificationDate,
			string limitResultPerPart = "-1" )
		{
			var items = new[]
			{
				ValueTuple.Create( PartUuidsParamName, partUuids ),
				ValueTuple.Create( MeasurementUuidsParamName, measurementUuids ),
				ValueTuple.Create( DeepParamName, deep ),
				ValueTuple.Create( LimitResultParamName, limitResult ),
				ValueTuple.Create( LimitResultPerPartParamName, limitResultPerPart ),
				ValueTuple.Create( OrderByParamName, order ),
				ValueTuple.Create( SearchConditionParamName, searchCondition ),
				ValueTuple.Create( AggregationParamName, aggregation ),
				ValueTuple.Create( FromModificationDateParamName, fromModificationDate ),
				ValueTuple.Create( ToModificationDateParamName, toModificationDate )
			};

			var result = new DistinctMeasurementFilterAttributesDto();
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
						case OrderByParamName:
							result.OrderBy = value.Split( ',' ).Select( element => OrderDtoParser.Parse( element, EntityDto.Measurement ) ).ToArray();
							break;
						case SearchConditionParamName:
							result.SearchCondition = SearchConditionParser.Parse( value );
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
					}
				}
				catch( Exception ex )
				{
					throw new InvalidOperationException( $"Invalid filter value '{value}' for parameter '{key}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {"partUuids = [list of part uuids]\r\n" + "deep = [True|False]\r\n" + "limitResult = [short]\r\n" + "measurementUuids = [list of measurement uuids]\r\n" + "measurementAttributes = [attribute keys csv|Empty for all attributes]\r\n" + "orderBy:[ushort asc|desc, ushort asc|desc, ...]\r\n" + "searchCondition:[search filter string]\r\n" + "aggregation:[Measurements|AggregationMeasurements|Default|All]\r\n" + "statistics:[None|Simple|Detailed]\r\n" + "mergeAttributes:[list of measurement attributes]\r\n" + "fromModificationDate:[Date]\r\n" + "toModificationDate:[Date]"}", ex );
				}
			}

			return result;
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

			if( OrderBy != null && OrderBy.Count > 0 )
				result.Add( ParameterDefinition.Create( OrderByParamName, OrderByToString( OrderBy ) ) );

			if( SearchCondition != null )
				result.Add( ParameterDefinition.Create( SearchConditionParamName, SearchConditionParser.GenericConditionToString( SearchCondition ) ) );

			if( AggregationMeasurements != AggregationMeasurementSelectionDto.Default )
				result.Add( ParameterDefinition.Create( AggregationParamName, AggregationMeasurements.ToString() ) );
			if( FromModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( FromModificationDateParamName, XmlConvert.ToString( FromModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			if( ToModificationDate.HasValue )
				result.Add( ParameterDefinition.Create( ToModificationDateParamName, XmlConvert.ToString( ToModificationDate.Value, XmlDateTimeSerializationMode.RoundtripKind ) ) );

			return result;
		}

		/// <summary>
		/// Creates a clone of this filter.
		/// </summary>
		public DistinctMeasurementFilterAttributesDto Clone()
		{
			return new DistinctMeasurementFilterAttributesDto
			{
				PartUuids = PartUuids,
				Deep = Deep,
				LimitResult = LimitResult,
				LimitResultPerPart = LimitResultPerPart,
				OrderBy = OrderBy,
				SearchCondition = SearchCondition,
				MeasurementUuids = MeasurementUuids,
				AggregationMeasurements = AggregationMeasurements,
				FromModificationDate = FromModificationDate,
				ToModificationDate = ToModificationDate
			};
		}

		#endregion
	}
}