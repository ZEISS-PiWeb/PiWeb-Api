#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="AttributeDto"/> collections.
	/// </summary>
	public sealed class JsonAttributeArrayConverter : JsonConverter<IReadOnlyList<AttributeDto>>
	{
		#region methods

		/// <inheritdoc />
		public override IReadOnlyList<AttributeDto> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if( reader.TokenType != JsonTokenType.StartObject )
				return Array.Empty<AttributeDto>();

			return ReadAttributes( ref reader );
		}

		internal static IReadOnlyList<AttributeDto> ReadAttributes( ref Utf8JsonReader reader )
		{
			var result = new List<AttributeDto>();

			while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
			{
				if( JsonAttributeConverter.TryReadFromProperty( ref reader, out var attribute ) )
					result.Add( attribute );
			}

			return result;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, IReadOnlyList<AttributeDto> value, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			WriteAttributes( writer, value, options );

			writer.WriteEndObject();
		}

		internal static void WriteAttributes( Utf8JsonWriter writer, IEnumerable<AttributeDto> value, JsonSerializerOptions options )
		{
			foreach( var attribute in value )
				JsonAttributeConverter.WriteAsProperty( writer, attribute, options );
		}

		#endregion
	}
}
