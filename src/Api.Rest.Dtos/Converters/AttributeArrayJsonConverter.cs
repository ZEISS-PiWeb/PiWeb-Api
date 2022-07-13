#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converters
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
	public sealed class AttributeArrayJsonConverter : JsonConverter<IReadOnlyList<AttributeDto>>
	{
		#region methods

		/// <inheritdoc />
		public override IReadOnlyList<AttributeDto> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if( reader.TokenType != JsonTokenType.StartObject )
			{
				return Array.Empty<AttributeDto>();
			}

			var result = new List<AttributeDto>();

			while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
			{
				if( AttributeJsonConverter.TryReadFromProperty( ref reader, out var attribute ) )
				{
					result.Add( attribute );
				}
			}

			return result;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, IReadOnlyList<AttributeDto> value, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			foreach( var attribute in value )
			{
				AttributeJsonConverter.WriteAsProperty( writer, attribute, options );
			}

			writer.WriteEndObject();
		}

		#endregion
	}
}
