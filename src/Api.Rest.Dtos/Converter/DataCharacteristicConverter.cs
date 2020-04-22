#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region using

	using System;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Data.Attribute;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataCharacteristic"/>-objects.
	/// </summary>
	public class DataCharacteristicConverter : JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return objectType == typeof( DataCharacteristic[] );
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( typeof( DataCharacteristic ) == objectType )
			{
				var characteristic = new DataCharacteristic();
				if( reader.Read() && reader.TokenType == JsonToken.PropertyName )
				{
					characteristic.Uuid = new Guid( (string)reader.Value );

					var valueAttributes = new System.Collections.Generic.List<Attribute>();
					if( reader.Read() && reader.TokenType == JsonToken.StartObject )
					{
						while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
						{
							var key = ushort.Parse( reader.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture );
							var value = reader.ReadAsString();

							valueAttributes.Add( new Attribute( key, value ) );
						}
					}
					characteristic.Value = new DataValue( valueAttributes.ToArray() );
				}
				return characteristic;
			}
			else
			{
				var characteristics = new System.Collections.Generic.List<DataCharacteristic>();
				while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
				{
					var characteristic = new DataCharacteristic();
					characteristic.Uuid = new Guid( (string) reader.Value );

					var valueAttributes = new System.Collections.Generic.List<Attribute>();
					if( reader.Read() && reader.TokenType == JsonToken.StartObject )
					{
						while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
						{
							var key = AttributeKeyCache.Cache.StringToKey( reader.Value.ToString() );
							var value = reader.ReadAsString();

							valueAttributes.Add( new Attribute( key, value ) );
						}
					}
					characteristic.Value = new DataValue( valueAttributes.ToArray() );
					characteristics.Add( characteristic );
				}
				return characteristics.ToArray();
			}
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();

			var dataCharacteristics = (DataCharacteristic[])value;
			if( dataCharacteristics.Length > 0 )
			{
				foreach( var dataCharacteristic in dataCharacteristics )
				{
					if( dataCharacteristic.Value != null && dataCharacteristic.Value.Attributes != null && dataCharacteristic.Value.Attributes.Length > 0 )
					{
						writer.WritePropertyName( dataCharacteristic.Uuid.ToString() );
						writer.WriteStartObject();
						foreach( var att in dataCharacteristic.Value.Attributes )
						{
							writer.WritePropertyName( AttributeKeyCache.Cache.KeyToString( att.Key ) );
							writer.WriteValue( att.RawValue ?? att.Value );
						}
						writer.WriteEndObject();
					}
				}
			}
			writer.WriteEndObject();
		}

		#endregion
	}
}
