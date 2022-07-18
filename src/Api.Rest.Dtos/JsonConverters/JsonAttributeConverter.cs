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
	using System.Globalization;
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
			{
				TryReadFromProperty( ref reader, out result );
			}

			return result;
		}

		internal static bool TryReadFromProperty( ref Utf8JsonReader reader, out AttributeDto attribute )
		{
			var key = ushort.Parse( reader.GetString() );

			reader.Read();

			switch( reader.TokenType )
			{
				case JsonTokenType.String:
					attribute = new AttributeDto( key, reader.GetString() );
					return true;

				case JsonTokenType.Number:
					if( reader.TryGetInt32( out var intValue ) )
					{
						attribute = new AttributeDto( key, intValue.ToString( CultureInfo.InvariantCulture ) );
						return true;
					}
					else if( reader.TryGetDouble( out var doubleValue ) )
					{
						attribute = new AttributeDto( key, doubleValue.ToString( "G17", CultureInfo.InvariantCulture ) );
						return true;
					}
					break;
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
			{
				writer.WriteString( key, value.Value );
			}
		}

		#endregion
	}
}