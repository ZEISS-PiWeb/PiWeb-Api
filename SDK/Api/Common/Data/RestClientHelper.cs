#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.IO;
	using System.Net;
	using System.Text;
	using Common.Client;
	using Converter;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;
	using Newtonsoft.Json.Serialization;

	using DataService;
	using RawDataService;

	using Formatting = Newtonsoft.Json.Formatting;

	#endregion

	/// <summary>
	/// Helper class for REST webservice calls.
	/// </summary>
	//[System.Diagnostics.DebuggerStepThrough]
	public static class RestClientHelper
	{
		#region constants

		/// <summary>Start identifier for a list inside a HTTP query.</summary>
		public const string QueryListStart = "{";

		/// <summary>End identifier for a list inside a HTTP query.</summary>
		public const string QueryListStop = "}";

		#endregion

		#region methods

		#region Remove from public API

		/// <summary>
		/// PiWeb-5.6 uses a response envelope that will be skipped by this compatibility code
		/// </summary>
		private static JsonTextReader CreateReaderAndSeekToResponseEnvelope( Stream stream )
		{
			if( stream.CanSeek )
			{
				var reader = new JsonTextReader( new StreamReader( stream, Encoding.UTF8, true, 1, true ) ) {CloseInput = false};
				if( reader.Read() && reader.TokenType == JsonToken.StartObject )
				{
					// "status" überlesen
					if( reader.Read() && Convert.ToString( reader.Value ) == "status" )
						reader.Skip();

					// "category" überlesen
					if( reader.Read() && Convert.ToString( reader.Value ) == "category" )
						reader.Skip();

					if( reader.Read() && Convert.ToString( reader.Value ) == "data" )
					{
						reader.Read();
						return reader;
					}
				}
				stream.Seek( 0, SeekOrigin.Begin );
			}
			return new JsonTextReader( new StreamReader( stream, Encoding.UTF8, true, 1, true ) ) {CloseInput = false};
		}

		#endregion

		/// <summary>
		/// Deserializes the <paramref name="data"/>-stream into a new object of type <typeparamref name="T"/>. The data is expected to be in JSON format.
		/// </summary>
		public static T DeserializeObject<T>( Stream data )
		{
			using( var reader = CreateReaderAndSeekToResponseEnvelope( data ) )
			{
				return CreateJsonSerializer().Deserialize<T>( reader );
			}
		}

		/// <summary>
		/// Deserializes the <paramref name="data"/>-stream into a new enumerable object of type <typeparamref name="T"/>. The data is expected to be in JSON format.
		/// </summary>
		public static IEnumerable<T> DeserializeEnumeratedObject<T>( Stream data )
		{
			using( var streamReader = new StreamReader( data, Encoding.UTF8, true, 1, true ) )
			using( var reader = new JsonTextReader( streamReader ) {CloseInput = false} )
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
		/// <param name="depth">The depth determines how deep the response should be.</param>
		/// <param name="requestedPartAttributes">Restricts the part attributes that are returned.</param>
		/// <param name="requestedCharacteristicAttributes">Restricts the characteristic attributes that are returned.</param>
		/// <param name="withHistory">Determines if the history should be returned.</param>
		/// <returns></returns>
		public static List<ParameterDefinition> ParseToParameter( PathInformation partPath = null, Guid[] partUuids = null, ushort? depth = null, AttributeSelector requestedPartAttributes = null, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false )
		{
			var parameter = new List<ParameterDefinition>();
			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2String( partPath ) ) );

			if( depth.HasValue )
				parameter.Add( ParameterDefinition.Create( "depth", depth.ToString() ) );

			if( withHistory )
				parameter.Add( ParameterDefinition.Create( "withHistory", true.ToString() ) );

			if( partUuids != null && partUuids.Length > 0 )
				parameter.Add( ParameterDefinition.Create( "partUuids", ConvertGuidListToString( partUuids ) ) );

			if( requestedPartAttributes != null && requestedPartAttributes.AllAttributes != AllAttributeSelection.True && requestedPartAttributes.Attributes != null )
				parameter.Add( ParameterDefinition.Create( "requestedPartAttributes", ConvertUInt16ListToString( requestedPartAttributes.Attributes ) ) );

			if( requestedCharacteristicAttributes != null && requestedCharacteristicAttributes.AllAttributes != AllAttributeSelection.True && requestedCharacteristicAttributes.Attributes != null )
				parameter.Add( ParameterDefinition.Create( "requestedCharacteristicAttributes", ConvertUInt16ListToString( requestedCharacteristicAttributes.Attributes ) ) );

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

			return value.Split( ',' ).Select( s => ushort.Parse( s, CultureInfo.InvariantCulture ) ).ToArray();
		}

		/// <summary>
		/// Parses a string to a list of Guids.
		/// </summary>
		public static Guid[] ConvertStringToGuidList( string value )
		{
			return ParseListToStringArray( value ).Select( s => new Guid( s ) ).ToArray();
		}

		/// <summary>Parses a list of strings.</summary>
		private static IEnumerable<string> ParseListToStringArray( string value )
		{
			if( value.StartsWith( QueryListStart ) && value.EndsWith( QueryListStop ) )
				value = value.Substring( 1, value.Length - 2 );

			return value.Split( ',' ).Select( p => p.Trim() ).ToArray();
		}

		/// <summary>Creates a list string from the ushorts <code>value</code>.</summary>
		public static string ConvertUInt16ListToString( ushort[] value )
		{
			if( value == null || value.Length == 0 )
				return "";

			return ToListString( string.Join( ",", value.Select( v => v.ToString( CultureInfo.InvariantCulture ) ) ) );
		}

		/// <summary>Creates a list string from the uuids <code>value</code>.</summary>
		public static string ConvertGuidListToString( Guid[] value )
		{
			if( value == null || value.Length == 0 )
				return "";

			return ToListString( string.Join( ",", value.Select( v => v.ToString( "D" ) ) ) );
		}

		/// <summary>Creates a list string from <paramref name="list"/>. </summary>
		public static string ToListString( string list )
		{
			if( string.IsNullOrEmpty( list ) )
				return "";

			var sb = new StringBuilder();
			sb.Append( QueryListStart );
			sb.Append( list );
			sb.Append( QueryListStop );

			return sb.ToString();
		}

		/// <summary>
		/// Creates and configures the <see cref="Newtonsoft.Json.JsonSerializer"/> that are needed by the services.
		/// </summary>
		public static JsonSerializer CreateJsonSerializer()
		{
			var serializer = new JsonSerializer
			{
				ContractResolver = new CamelCasePropertyNamesContractResolver(),
				Formatting = Formatting.None,
				DateFormatHandling = DateFormatHandling.IsoDateFormat,
				NullValueHandling = NullValueHandling.Ignore
			};
			serializer.Converters.Add( new StringEnumConverter() );
			serializer.Converters.Add( new AttributeConverter() );
			serializer.Converters.Add( new AttributeDefinitionConverter() );
			serializer.Converters.Add( new CatalogConverter() );
			serializer.Converters.Add( new CatalogEntryConverter() );
			serializer.Converters.Add( new ConfigurationConverter() );
			serializer.Converters.Add( new DataCharacteristicConverter() );
			serializer.Converters.Add( new PathInformationConverter() );

			serializer.Converters.Add( new StreamingReaderConverter<InspectionPlanPart>() );
			serializer.Converters.Add( new StreamingWriterConverter<InspectionPlanPart>() );

			serializer.Converters.Add( new StreamingReaderConverter<InspectionPlanCharacteristic>() );
			serializer.Converters.Add( new StreamingWriterConverter<InspectionPlanCharacteristic>() );
			
			serializer.Converters.Add( new StreamingReaderConverter<InspectionPlanBase>() );
			serializer.Converters.Add( new StreamingWriterConverter<InspectionPlanBase>() );
			
			serializer.Converters.Add( new StreamingReaderConverter<DataMeasurement>() );
			serializer.Converters.Add( new StreamingWriterConverter<DataMeasurement>() );
			
			serializer.Converters.Add( new StreamingReaderConverter<SimpleMeasurement>() );
			serializer.Converters.Add( new StreamingWriterConverter<SimpleMeasurement>() );

			serializer.Converters.Add( new StreamingReaderConverter<RawDataInformation>() );
			serializer.Converters.Add( new StreamingWriterConverter<RawDataInformation>() );

			return serializer;
		}

		#endregion
	}
}