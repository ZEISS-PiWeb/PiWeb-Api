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
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="AttributeDto"/> arrays.
	/// </summary>
	public class AttributeArrayConverter : JsonConverter
	{
		#region methods

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( AttributeDto[] ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			var result = new List<AttributeDto>();
			if( reader.TokenType == JsonToken.StartObject )
			{
				while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
				{
					var key = AttributeKeyCache.Cache.StringToKey( reader.Value.ToString() );
					var value = reader.ReadAsString();

					result.Add( new AttributeDto( key, value ) );
				}
			}

			return result.ToArray();
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();

			var attributes = (AttributeDto[])value;
			if( attributes != null && attributes.Length > 0 )
			{
				foreach( var att in attributes )
				{
					writer.WritePropertyName( AttributeKeyCache.Cache.KeyToString( att.Key ) );
					writer.WriteValue( att.RawValue ?? att.Value );
				}
			}

			writer.WriteEndObject();
		}

		#endregion
	}
}