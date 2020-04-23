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
	using Attribute = Zeiss.PiWeb.Api.Rest.Dtos.Data.Attribute;

	#endregion

	public class AttributeConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( Attribute ) == objectType;
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			var result = new Attribute();

			if( reader.Read() && reader.TokenType == JsonToken.PropertyName )
			{
				var key = AttributeKeyCache.Cache.StringToKey( reader.Value.ToString() );
				var value = reader.ReadAsString();

				result = new Attribute( key, value );
			}

			return result;
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();

			var att = (Attribute)value;
			writer.WritePropertyName( AttributeKeyCache.Cache.KeyToString( att.Key ) );
			writer.WriteValue( att.RawValue ?? att.Value );

			writer.WriteEndObject();
		}

		#endregion
	}
}