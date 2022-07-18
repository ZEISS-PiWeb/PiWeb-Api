#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
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
	/// Specialized <see cref="JsonConverter"/> for <see cref="DataCharacteristicDto"/>-objects.
	/// </summary>
	public sealed class JsonDataCharacteristicArrayConverter : JsonConverter<IReadOnlyCollection<DataCharacteristicDto>>
	{
		#region methods

		/// <inheritdoc />
		public override IReadOnlyCollection<DataCharacteristicDto> Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			var characteristics = new List<DataCharacteristicDto>();

			while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
			{
				var characteristic = new DataCharacteristicDto { Uuid = Guid.Parse( reader.GetString() ) };

				var valueAttributes = new List<AttributeDto>();

				if( reader.Read() && reader.TokenType == JsonTokenType.StartObject )
				{
					while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
					{
						if( JsonAttributeConverter.TryReadFromProperty( ref reader, out var attribute ) )
						{
							valueAttributes.Add( attribute );
						}
					}
				}

				characteristic.Value = new DataValueDto( valueAttributes );

				characteristics.Add( characteristic );
			}

			return characteristics;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, IReadOnlyCollection<DataCharacteristicDto> value, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			foreach( var dataCharacteristic in value )
			{
				if( dataCharacteristic.Value?.Attributes != null && dataCharacteristic.Value.Attributes.Count > 0 )
				{
					writer.WritePropertyName( dataCharacteristic.Uuid.ToString() );

					writer.WriteStartObject();

					foreach( var attribute in dataCharacteristic.Value.Attributes )
					{
						JsonAttributeConverter.WriteAsProperty( writer, attribute, options );
					}

					writer.WriteEndObject();
				}
			}

			writer.WriteEndObject();
		}

		#endregion
	}
}