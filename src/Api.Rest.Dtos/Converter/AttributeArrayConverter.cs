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
	using Zeiss.PiWeb.Api.Core;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Zeiss.PiWeb.Api.Core.Attribute;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="Attribute"/> collections.
	/// </summary>
	public sealed class AttributeArrayConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( IReadOnlyList<Attribute> ) == objectType;
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( reader.TokenType != JsonToken.StartObject )
				return Array.Empty<Attribute>();

			return ReadAttributes( reader );
		}

		internal static IReadOnlyList<Attribute> ReadAttributes( JsonReader reader )
		{
			var result = new List<Attribute>();
			while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
			{
				var key = ushort.Parse( (string)reader.Value );
				var value = reader.ReadAsString();

				result.Add( new Attribute( key, value ) );
			}

			return result;
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();
			WriteAttributes( writer, (IReadOnlyList<Attribute>)value );
			writer.WriteEndObject();
		}

		internal static void WriteAttributes( JsonWriter writer, IReadOnlyList<Attribute> attributes )
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
