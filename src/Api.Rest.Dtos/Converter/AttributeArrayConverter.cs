#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="AttributeDto"/> collections.
	/// </summary>
	public sealed class AttributeArrayConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( IReadOnlyList<AttributeDto> ) == objectType;
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( reader.TokenType != JsonToken.StartObject )
				return Array.Empty<AttributeDto>();

			return ReadAttributes( reader );
		}

		internal static IReadOnlyList<AttributeDto> ReadAttributes( JsonReader reader )
		{
			var result = new List<AttributeDto>();
			while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
			{
				var key = ushort.Parse( (string)reader.Value );
				var value = reader.ReadAsString();

				result.Add( new AttributeDto( key, value ) );
			}

			return result;
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();
			WriteAttributes( writer, (IReadOnlyList<AttributeDto>)value );
			writer.WriteEndObject();
		}

		internal static void WriteAttributes( JsonWriter writer, IReadOnlyList<AttributeDto> attributes )
		{
			if( attributes == null || attributes.Count == 0 )
				return;

			foreach( var attribute in attributes )
			{
				writer.WritePropertyName( AttributeKeyCache.StringForKey( attribute.Key ) );
				writer.WriteValue( attribute.RawValue ?? attribute.Value );
			}
		}

		#endregion
	}
}
