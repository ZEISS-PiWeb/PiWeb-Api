#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Linq;
	using System.Text;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Zeiss.IMT.PiWeb.Api.Common.Client;
	using Zeiss.IMT.PiWeb.Api.DataService.Rest;

	#endregion

	/// <summary>
	/// Helper class for REST webservice calls.
	/// </summary>
	public static class RestClientHelper
	{
		#region constants

		/// <summary>Start identifier for a list inside a HTTP query.</summary>
		public const string QueryListStart = "{";

		/// <summary>End identifier for a list inside a HTTP query.</summary>
		public const string QueryListStop = "}";

		/// <summary>Length of escaped delimiter for list values within <see cref="QueryListStart"/> 
		/// and <see cref="QueryListStop"/> inside a HTTP query ( , escaped => %2C)</summary>
		private const int LengthOfEscapedDelimiter = 3;

		#endregion

		#region methods

		/// <summary>
		/// Deserializes the <paramref name="data"/>-stream into a new object of type <typeparamref name="T"/>. The data is expected to be in JSON format.
		/// </summary>
		public static T DeserializeObject<T>( Stream data )
		{
			using( var reader = new JsonTextReader( new StreamReader( data, Encoding.UTF8, true, 4096, true ) ) { CloseInput = false } )
			{
				return CreateJsonSerializer().Deserialize<T>( reader );
			}
		}

		/// <summary>
		/// Deserializes the <paramref name="data"/>-stream into a new enumerable object of type <typeparamref name="T"/>. The data is expected to be in JSON format.
		/// </summary>
		internal static IEnumerable<T> DeserializeEnumeratedObject<T>( Stream data )
		{
			using( var streamReader = new StreamReader( data, Encoding.UTF8, true, 4096, true ) )
			using( var reader = new JsonTextReader( streamReader ) { CloseInput = false } )
			{
				var result = CreateJsonSerializer().Deserialize<IEnumerable<T>>( reader );

				if( result == null ) yield break;

				foreach( var entity in result )
				{
					yield return entity;
				}
			}
		}

		/// <summary>
		/// Parses inspection plan filter criterias to a <see cref="ParameterDefinition"/> list.
		/// </summary>
		/// <param name="partPath">Path of the part the query should be restricted by.</param>
		/// <param name="partUuids">Uuids of the parts the query should be restricted by.</param>
		/// <param name="charUuids">Uuids of the parts the query should be restricted by.</param>
		/// <param name="depth">The depth determines how deep the response should be.</param>
		/// <param name="requestedPartAttributes">Restricts the part attributes that are returned.</param>
		/// <param name="requestedCharacteristicAttributes">Restricts the characteristic attributes that are returned.</param>
		/// <param name="withHistory">Determines if the history should be returned.</param>
		/// <returns></returns>
		internal static List<ParameterDefinition> ParseToParameter( PathInformation partPath = null, Guid[] partUuids = null, Guid[] charUuids = null, ushort? depth = null, AttributeSelector requestedPartAttributes = null, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false )
		{
			var parameter = new List<ParameterDefinition>();
			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2DatabaseString( partPath ) ) );

			if( depth.HasValue )
				parameter.Add( ParameterDefinition.Create( "depth", depth.ToString() ) );

			if( withHistory )
				parameter.Add( ParameterDefinition.Create( "withHistory", true.ToString() ) );

			if( partUuids != null && partUuids.Length > 0 )
				parameter.Add( ParameterDefinition.Create( "partUuids", ConvertGuidListToString( partUuids ) ) );

			if( charUuids != null && charUuids.Length > 0 )
				parameter.Add( ParameterDefinition.Create( "charUuids", ConvertGuidListToString( charUuids ) ) );

			if( requestedPartAttributes != null )
			{
				if( requestedPartAttributes.AllAttributes == AllAttributeSelection.False )
					parameter.Add( ParameterDefinition.Create( "requestedPartAttributes", "None" ) );
				else if( requestedPartAttributes.AllAttributes != AllAttributeSelection.True && requestedPartAttributes.Attributes != null )
					parameter.Add( ParameterDefinition.Create( "requestedPartAttributes", ConvertUshortArrayToString( requestedPartAttributes.Attributes ) ) );
			}
			if( requestedCharacteristicAttributes != null )
			{
				if( requestedCharacteristicAttributes.AllAttributes == AllAttributeSelection.False )
					parameter.Add( ParameterDefinition.Create( "requestedCharacteristicAttributes", "None" ) );
				else if( requestedCharacteristicAttributes.AllAttributes != AllAttributeSelection.True && requestedCharacteristicAttributes.Attributes != null )
					parameter.Add( ParameterDefinition.Create( "requestedCharacteristicAttributes", ConvertUshortArrayToString( requestedCharacteristicAttributes.Attributes ) ) );
			}
			return parameter;
		}

		/// <summary>
		/// Parses a string to a list of ushorts.
		/// </summary>
		public static ushort[] ConvertStringToUInt16List( string value )
		{
			if( value.StartsWith( QueryListStart ) && value.EndsWith( QueryListStop ) )
				value = value.Substring( 1, value.Length - 2 );
			
			if( string.IsNullOrEmpty( value ) )
				return new ushort[0];
			try
			{
				return value.Split( ',' ).Select( s => ushort.Parse( s, CultureInfo.InvariantCulture ) ).ToArray();
			}
			catch( Exception )
			{
				throw new FormatException( $"Error on parsing {value} due to bad formatting." );
			}
		}

		/// <summary>
		/// Parses a string to a list of Guids.
		/// </summary>
		public static Guid[] ConvertStringToGuidList( string value )
		{
			var stringArray = ParseListToStringArray( value );
			return StringUuidTools.StringUuidListToGuidList( stringArray ).ToArray();
		}

		/// <summary>Parses a list of strings.</summary>
		private static IEnumerable<string> ParseListToStringArray( string value )
		{
			if( value.StartsWith( QueryListStart ) && value.EndsWith( QueryListStop ) )
				value = value.Substring( 1, value.Length - 2 );

			return value.Split( ',' ).Select( p => p.Trim() ).ToArray();
		}

		/// <summary>Creates a list string from the ushorts <code>value</code>.</summary>
		public static string ConvertUshortArrayToString( ushort[] value )
		{
			if( value == null || value.Length == 0 )
				return "";

			return ToListString( value.Select( v => v.ToString( CultureInfo.InvariantCulture ) ) );
		}

		/// <summary>Creates a list string from the shorts <code>value</code>.</summary>
		internal static string ConvertShortArrayToString( short[] value )
		{
			if( value == null || value.Length == 0 )
				return "";

			return ToListString( value.Select( v => v.ToString( CultureInfo.InvariantCulture ) ) );
		}

		/// <summary>Creates a list string from the uuids <code>value</code>.</summary>
		public static string ConvertGuidListToString( Guid[] value )
		{
			if( value == null || value.Length == 0 )
				return "";

			return ToListString( value.Select( v => v.ToString( "D" ) ) );
		}

		/// <summary>Creates a list string from <paramref name="list"/>.</summary>
		internal static string ToListString( IEnumerable<string> list )
		{
			var listString = string.Join( ",", list );
			if( string.IsNullOrEmpty( listString ) )
				return "";

			var sb = new StringBuilder();
			sb.Append( QueryListStart );
			sb.Append( listString );
			sb.Append( QueryListStop );

			return sb.ToString();
		}

		/// <summary>
		/// Returns the length of an elment of type <typeparam name="T"/> within an uri
		/// </summary>
		internal static int LengthOfListElementInUri<T>( T listElement )
		{
			var escapedElement = Uri.EscapeDataString( listElement.ToString() );
			var length = escapedElement.Length;

			return length + LengthOfEscapedDelimiter;
		}
		
		/// <summary>
		/// Provides the remaining size for parameters calculated by difference of <paramref name="maxUriLength"/> and the combination <paramref name="serviceLocation"/>, <paramref name="requestPath"/> and <paramref name="parameterDefinitions"/>
		/// <remarks><paramref name="requestPath"/> and <paramref name="parameterDefinitions"/> must not contain any values (except for lists: empty brackets) but only fix parameter name </remarks>
		/// </summary>
		internal static int GetUriTargetSize( Uri serviceLocation, string requestPath, int maxUriLength, params ParameterDefinition[] parameterDefinitions )
		{
			var endpointUriString = RequestUriHelper.MakeRequestUri( serviceLocation, requestPath, parameterDefinitions ).ToString();
			return Math.Max( maxUriLength - endpointUriString.Length, 0 );
		}

		/// <summary>
		/// Creates and configures the <see cref="Newtonsoft.Json.JsonSerializer"/> that are needed by the services.
		/// </summary>
		internal static JsonSerializer CreateJsonSerializer()
		{
			return new JsonSerializer
			{
				Formatting = Formatting.None,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				NullValueHandling = NullValueHandling.Ignore,
				Converters = { new VersionConverter() }
			};
		}

		#endregion
	}
}
