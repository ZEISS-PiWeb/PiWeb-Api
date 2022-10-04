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
	using Zeiss.PiWeb.Api.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Zeiss.PiWeb.Api.Contracts.Attribute;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="Contracts.Attribute"/> collections.
	/// </summary>
	public sealed class JsonAttributeArrayConverter : JsonConverter<IReadOnlyList<Attribute>>
	{
		#region methods

		/// <inheritdoc />
		public override IReadOnlyList<Attribute> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			if( reader.TokenType != JsonTokenType.StartObject )
				return Array.Empty<Attribute>();

			return ReadAttributes( ref reader );
		}

		internal static IReadOnlyList<Attribute> ReadAttributes( ref Utf8JsonReader reader )
		{
			var result = new List<Attribute>();

			while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
			{
				if( JsonAttributeConverter.TryReadFromProperty( ref reader, out var attribute ) )
					result.Add( attribute );
			}

			return result;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, IReadOnlyList<Attribute> value, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			WriteAttributes( writer, value, options );

			writer.WriteEndObject();
		}

		internal static void WriteAttributes( Utf8JsonWriter writer, IEnumerable<Attribute> value, JsonSerializerOptions options )
		{
			foreach( var attribute in value )
				JsonAttributeConverter.WriteAsProperty( writer, attribute, options );
		}

		#endregion
	}
}
