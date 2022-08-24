#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters
{
	#region usings

	using System;
	using System.Buffers;
	using System.Buffers.Text;
	using System.Text;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="AttributeDto"/>-objects.
	/// </summary>
	public sealed class JsonAttributeConverter : JsonConverter<AttributeDto>
	{
		#region methods

		/// <inheritdoc />
		public override AttributeDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			AttributeDto result = default;

			while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
				TryReadFromProperty( ref reader, out result );

			return result;
		}

		internal static bool TryReadFromProperty( ref Utf8JsonReader reader, out AttributeDto attribute )
		{
			var keySpan = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;

			if( !Utf8Parser.TryParse( keySpan, out ushort key, out var bytesConsumed ) || keySpan.Length != bytesConsumed )
				throw new FormatException( $"Input span was not in a correct format, on converting to '{nameof( UInt16 )}'" );

			reader.Read();

			switch( reader.TokenType )
			{
				case JsonTokenType.String:
					attribute = new AttributeDto( key, reader.GetString() );
					return true;

				case JsonTokenType.Number:
					var valueSpan = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
#if NETSTANDARD
					var value = Encoding.UTF8.GetString( valueSpan.ToArray() );
#else
					var value = Encoding.UTF8.GetString( valueSpan );
#endif
					attribute = new AttributeDto( key, value );
					return true;
			}

			attribute = default;

			return false;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, AttributeDto value, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			WriteAsProperty( writer, value, options );

			writer.WriteEndObject();
		}

		internal static void WriteAsProperty( Utf8JsonWriter writer, in AttributeDto value, JsonSerializerOptions options )
		{
			var key = AttributeKeyCache.StringForKey( value.Key );

			if( value.RawValue is not null )
			{
				writer.WritePropertyName( key );
				JsonSerializer.Serialize( writer, value.RawValue, options );
			}
			else
				writer.WriteString( key, value.Value );
		}

		#endregion
	}
}