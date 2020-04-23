#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.Converter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for streaming a list or an array.
	/// </summary>
	public class StreamingReaderConverter<T> : JsonConverter
	{
		#region properties

		/// <summary>
		/// Returns <code>false</code>. No write support.
		/// </summary>
		public override bool CanWrite => false;

		#endregion

		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( T[] ) == objectType ||
					typeof( List<T> ) == objectType ||
					typeof( IEnumerable<T> ) == objectType;
		}

		/// <inheritdoc />
		/// <remarks>Not supported right now.</remarks>
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

		/// <inheritdoc />
		/// <remarks>Not supported right now.</remarks>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			throw new NotImplementedException( "This converter does not support reading." );
		}

		#endregion
	}
}