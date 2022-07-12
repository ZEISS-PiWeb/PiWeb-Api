#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converters;

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
public sealed class AttributeJsonConverter : JsonConverter<AttributeDto>
{
	#region methods

	/// <inheritdoc />
	public override AttributeDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
	{
		AttributeDto result = default;

		while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
		{
			var key = ushort.Parse( reader.GetString() );

			reader.Read();

			switch( reader.TokenType )
			{
				case JsonTokenType.String:
					result = new AttributeDto( key, reader.GetString() );
					break;

				case JsonTokenType.Number:
					if( reader.TryGetInt32( out var intValue ) )
					{
						result = new AttributeDto( key, intValue.ToString( CultureInfo.InvariantCulture ) );
					}
					else if( reader.TryGetDouble( out var doubleValue ) )
					{
						result = new AttributeDto( key, doubleValue.ToString( "G17", CultureInfo.InvariantCulture ) );
					}
					break;
			}
		}

		return result;
	}

	/// <inheritdoc />
	public override void Write( Utf8JsonWriter writer, AttributeDto value, JsonSerializerOptions options )
	{
		writer.WriteStartObject();

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

		writer.WriteEndObject();
	}

#endregion
}