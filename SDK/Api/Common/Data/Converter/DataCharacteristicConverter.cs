#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Data.Converter
{
	#region using

	using System;

	using DataService;

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataService.DataCharacteristic"/>-objects.
	/// </summary>
	public class DataCharacteristicConverter : JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return objectType.IsSubclassOf( typeof( DataCharacteristic ) ) || typeof( DataCharacteristic ) == objectType || typeof( DataCharacteristic[] ) == objectType;
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

					var valueAttributes = new System.Collections.Generic.List<DataService.Attribute>();
					if( reader.Read() && reader.TokenType == JsonToken.StartObject )
					{
						while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
						{
							var key = ushort.Parse( reader.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture );
							var value = reader.ReadAsString();

							valueAttributes.Add( new DataService.Attribute( key, value ) );
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

					var valueAttributes = new System.Collections.Generic.List<DataService.Attribute>();
					if( reader.Read() && reader.TokenType == JsonToken.StartObject )
					{
						while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
						{
							var key = ushort.Parse( reader.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture );
							var value = reader.ReadAsString();

							valueAttributes.Add( new DataService.Attribute( key, value ) );
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
			if( value is DataCharacteristic )
			{
				var dataCharacteristic = (DataCharacteristic)value;
				if( dataCharacteristic.Value != null && dataCharacteristic.Value.Attributes.Length > 0 )
				{
					writer.WritePropertyName( dataCharacteristic.Uuid.ToString() );
					writer.WriteStartObject();
					foreach( var att in dataCharacteristic.Value.Attributes )
					{
						writer.WritePropertyName( att.Key.ToString( System.Globalization.CultureInfo.InvariantCulture ) );
						writer.WriteValue( att.Value );
					}
					writer.WriteEndObject();
				}
			}
			else
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
								writer.WritePropertyName( att.Key.ToString( System.Globalization.CultureInfo.InvariantCulture ) );
								writer.WriteValue( att.Value );
							}
							writer.WriteEndObject();
						}
					}
				}
				writer.WriteEndObject();
			}
		}

		#endregion
	}
}
