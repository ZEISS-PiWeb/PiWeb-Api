#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Common.Data.Converter
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for streaming a list or an array.
	/// </summary>
	public class StreamingReaderConverter<T> : JsonConverter
	{
		#region members

		/// <summary>
		/// Returns <code>false</code>. No write support.
		/// </summary>
		public override bool CanWrite => false;

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( T[] ) == objectType ||
			       typeof( List<T> ) == objectType ||
			       typeof( IEnumerable<T> ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object. Not supported right now.
		/// </summary>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( typeof( T[] ) == objectType )
				return StreamedReadJson( reader, serializer ).ToArray();
			if( typeof( List<T> ) == objectType )
				return StreamedReadJson( reader, serializer ).ToList();
			return StreamedReadJson( reader, serializer );
		}

		private static IEnumerable<T> StreamedReadJson( JsonReader reader, JsonSerializer serializer )
		{
			if( reader.TokenType == JsonToken.StartArray )
			{
				while( reader.Read() && reader.TokenType == JsonToken.StartObject )
				{
					yield return serializer.Deserialize<T>( reader );
				}
			}
		}

		/// <summary>
		/// Not supported right now.
		/// </summary>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			throw new NotImplementedException( "This converter does not support reading." );
		}

		#endregion
	}

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for streamed writing a list or an array.
	/// </summary>
	public class StreamingWriterConverter<T> : JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( IEnumerable<T> ).IsAssignableFrom( objectType );
		}

		/// <summary>
		/// Returns <code>false</code>. No read support.
		/// </summary>
		public override bool CanRead => false;

		/// <summary>
		/// Not supported right now.
		/// </summary>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			throw new NotImplementedException( "This converter does not support reading." );
		}
		
		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			var items = (IEnumerable<T>)value;

			writer.WriteStartArray();
			foreach( var item in items )
			{
				serializer.Serialize( writer, item );
			}
			writer.WriteEndArray();
		}

		#endregion
	}
}