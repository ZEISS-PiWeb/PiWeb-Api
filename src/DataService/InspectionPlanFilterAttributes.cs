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
	using Common.Client;
	using Common.Data;
	
	using DataService;

	#endregion

	/// <summary>
	/// Class that encapsulates the url parameter for an inspection plan search via PiWeb-REST web service.
	/// </summary>
	public class InspectionPlanFilterAttributes
	{
		#region constants

		/// <summary>
		/// Default values for the filter <see cref="InspectionPlanFilterAttributes"/> that is used for fetching parts.
		/// </summary>
		public static readonly InspectionPlanFilterAttributes DefaultForParts = new InspectionPlanFilterAttributes { Depth = 1, RequestedPartAttributes = new AttributeSelector( AllAttributeSelection.True ) };

		/// <summary>
		/// Default values for the filter <see cref="InspectionPlanFilterAttributes"/> that is used for fetching characteristics.
		/// </summary>
		public static readonly InspectionPlanFilterAttributes DefaultForCharacteristics = new InspectionPlanFilterAttributes { Depth = ushort.MaxValue, RequestedCharacteristicAttributes = new AttributeSelector( AllAttributeSelection.True ) };

		private const string DepthParamName = "depth";
		private const string WithHistoryParamName = "withhistory";
		private const string PartUuidsParamName = "partuuids";
		private const string RequestedPartAttributesParamName = "partattributes";
		private const string RequestedCharacteristicsAttributesParamName = "characteristicattributes";

		#endregion

		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public InspectionPlanFilterAttributes()
		{
			WithHistory = false;
			RequestedPartAttributes = new AttributeSelector( AllAttributeSelection.True );
			RequestedCharacteristicAttributes = new AttributeSelector( AllAttributeSelection.True );
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the search depth for the search.
		/// </summary>
		public ushort? Depth { get; set; }

		/// <summary>
		/// Gets or sets the selector for the part attributes.
		/// </summary>
		public AttributeSelector RequestedPartAttributes { get; set; }

		/// <summary>
		/// Gets or sets the selector for the characteristic attributes.
		/// </summary>
		public AttributeSelector RequestedCharacteristicAttributes { get; set; }
		
		/// <summary>
		/// Gets or sets a flag that determines whether the version history should be returned.
		/// </summary>
		public bool WithHistory { get; set; }

		/// <summary>
		/// Gets or sets a list of part uuids that should be used for searching for parts or characteristics.
		/// </summary>
		public Guid[] PartUuids { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Parses the filter and returns a <see cref="InspectionPlanFilterAttributes"/> object that represents the filter values.
		/// If the parse operation was not successful, an <see cref="InvalidOperationException"/> will be thrown.
		/// </summary>
		/// <returns>The <see cref="InspectionPlanFilterAttributes"/> with the parsed information.</returns>
		public static InspectionPlanFilterAttributes Parse( string depth, string partUuids, string withHistory, string requestedPartAttributes, string requestedCharacteristicsAttributes )
		{
			var items = new[]
			{
				Tuple.Create( DepthParamName, depth ),
				Tuple.Create( PartUuidsParamName, partUuids ),
				Tuple.Create( WithHistoryParamName, withHistory ),
				Tuple.Create( RequestedPartAttributesParamName, requestedPartAttributes ),
				Tuple.Create( RequestedCharacteristicsAttributesParamName, requestedCharacteristicsAttributes ),
			};

			var result = new InspectionPlanFilterAttributes();
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
						case DepthParamName:
							result.Depth = ushort.Parse( value, System.Globalization.CultureInfo.InvariantCulture );
							break;
						case PartUuidsParamName:
							result.PartUuids = RestClientHelper.ConvertStringToGuidList( value );
							break;
						case WithHistoryParamName:
							result.WithHistory = bool.Parse( value );
							break;
						case RequestedPartAttributesParamName:
							if( string.Equals( value, "None", StringComparison.OrdinalIgnoreCase ) )
								result.RequestedPartAttributes = new AttributeSelector( AllAttributeSelection.False );
							else if( string.Equals( value, "All", StringComparison.OrdinalIgnoreCase ) )
								result.RequestedPartAttributes = new AttributeSelector( AllAttributeSelection.True );
							else
								result.RequestedPartAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
							break;
						case RequestedCharacteristicsAttributesParamName:
							if( string.Equals( value, "None", StringComparison.OrdinalIgnoreCase ) )
								result.RequestedCharacteristicAttributes = new AttributeSelector( AllAttributeSelection.False );
							else if( string.Equals( value, "All", StringComparison.OrdinalIgnoreCase ) )
								result.RequestedCharacteristicAttributes = new AttributeSelector( AllAttributeSelection.True );
							else
								result.RequestedCharacteristicAttributes = new AttributeSelector( RestClientHelper.ConvertStringToUInt16List( value ) );
							break;
					}
				}
				catch( Exception ex )
				{
					throw new InvalidOperationException( string.Format( "Invalid filter value '{0}' for parameter '{1}'. The can be specified via url parameter in the form of 'key=value'. The following keys are valid: {2}",
						value, key,
						"withHistory = [True|False]\r\n" +
						"depth = [short]\r\n" +
						"partUuids = [list of part uuids]\r\n" +
						"partAttributes = [All|None|Attribute keys csv|Empty for all attributes]\r\n" +
						"characteristicAttributes = [All|None|Attribute keys csv|Empty for all attributes]\r\n" ), ex );
				}
			}
			return result;
		}

		/// <summary>
		/// Creates a <see cref="ParameterDefinition"/> list that represents this filter.
		/// </summary>
		public IEnumerable<ParameterDefinition> ToParameterDefinition()
		{
			var result = new List<ParameterDefinition>();

			if( Depth.HasValue )
				result.Add( ParameterDefinition.Create( DepthParamName, Depth.ToString() ) );

			if( WithHistory )
				result.Add( ParameterDefinition.Create( WithHistoryParamName, WithHistory.ToString() ) );

			if( PartUuids != null && PartUuids.Length > 0 )
				result.Add( ParameterDefinition.Create( PartUuidsParamName, RestClientHelper.ConvertGuidListToString( PartUuids ) ) );

			if( RequestedPartAttributes != null && RequestedPartAttributes.AllAttributes != AllAttributeSelection.True && RequestedPartAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedPartAttributesParamName, RestClientHelper.ConvertUInt16ListToString( RequestedPartAttributes.Attributes ) ) );

			if( RequestedCharacteristicAttributes != null && RequestedCharacteristicAttributes.AllAttributes != AllAttributeSelection.True && RequestedCharacteristicAttributes.Attributes != null )
				result.Add( ParameterDefinition.Create( RequestedCharacteristicsAttributesParamName, RestClientHelper.ConvertUInt16ListToString( RequestedCharacteristicAttributes.Attributes ) ) );

			return result;
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