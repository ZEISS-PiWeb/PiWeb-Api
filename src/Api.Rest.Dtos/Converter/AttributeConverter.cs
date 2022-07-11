#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region usings

	using System;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	public sealed class AttributeConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( AttributeDto ) == objectType;
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( reader.Read() && reader.TokenType == JsonToken.PropertyName )
			{
				var key = ushort.Parse( (string)reader.Value );
				var value = reader.ReadAsString();

				return new AttributeDto( key, value );
			}

			return new AttributeDto();
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();

			var att = (AttributeDto)value;
			writer.WritePropertyName( AttributeKeyCache.StringForKey( att.Key ) );
			writer.WriteValue( att.RawValue ?? att.Value );

			writer.WriteEndObject();
		}

		#endregion
	}
}