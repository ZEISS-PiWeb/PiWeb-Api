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
	using System.Globalization;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataCharacteristicDto"/>-objects.
	/// </summary>
	public sealed class DataCharacteristicConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return objectType == typeof( IReadOnlyCollection<DataCharacteristicDto> ) || objectType == typeof( DataCharacteristicDto );
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( typeof( DataCharacteristicDto ) == objectType )
			{
				var characteristic = new DataCharacteristicDto();
				if( reader.Read() && reader.TokenType == JsonToken.PropertyName )
				{
					characteristic.Uuid = new Guid( (string)reader.Value );

					var valueAttributes = new List<AttributeDto>();
					if( reader.Read() && reader.TokenType == JsonToken.StartObject )
					{
						while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
						{
							var key = ushort.Parse( reader.Value.ToString(), CultureInfo.InvariantCulture );
							var value = reader.ReadAsString();

							valueAttributes.Add( new AttributeDto( key, value ) );
						}
					}

					characteristic.Value = new DataValueDto( valueAttributes.ToArray() );
				}

				return characteristic;
			}
			else
			{
				var characteristics = new List<DataCharacteristicDto>();
				while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
				{
					var characteristic = new DataCharacteristicDto { Uuid = new Guid( (string)reader.Value ) };

					var valueAttributes = new List<AttributeDto>();
					if( reader.Read() && reader.TokenType == JsonToken.StartObject )
					{
						while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
						{
							var key = ushort.Parse( reader.Value.ToString() );
							var value = reader.ReadAsString();

							valueAttributes.Add( new AttributeDto( key, value ) );
						}
					}

					characteristic.Value = new DataValueDto( valueAttributes.ToArray() );
					characteristics.Add( characteristic );
				}

				return characteristics;
			}
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			writer.WriteStartObject();

			var dataCharacteristics = (IReadOnlyCollection<DataCharacteristicDto>)value;
			if( dataCharacteristics.Count > 0 )
			{
				foreach( var dataCharacteristic in dataCharacteristics )
				{
					if( dataCharacteristic.Value?.Attributes != null && dataCharacteristic.Value.Attributes.Count > 0 )
					{
						writer.WritePropertyName( dataCharacteristic.Uuid.ToString() );
						writer.WriteStartObject();
						foreach( var att in dataCharacteristic.Value.Attributes )
						{
							writer.WritePropertyName( AttributeKeyCache.StringForKey( att.Key ) );
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