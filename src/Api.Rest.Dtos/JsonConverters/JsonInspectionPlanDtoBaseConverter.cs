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
	using System.Linq;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="InspectionPlanDtoBase"/>-objects.
	/// </summary>
	public sealed class JsonInspectionPlanDtoBaseConverter : JsonConverter<InspectionPlanDtoBase>
	{
		#region methods

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, InspectionPlanDtoBase value, JsonSerializerOptions options )
		{
			switch( value )
			{
				case null:
					writer.WriteNullValue();
					break;

				case InspectionPlanPartDto inspectionPlanPart:
					JsonSerializer.Serialize( writer, inspectionPlanPart, options );
					break;

				case SimplePartDto simplePart:
					JsonSerializer.Serialize( writer, simplePart, options );
					break;

				case InspectionPlanCharacteristicDto inspectionPlanCharacteristic:
					JsonSerializer.Serialize( writer, inspectionPlanCharacteristic, options );
					break;

				default:
					throw new NotImplementedException( $"{value.GetType().Name}" );
			}
		}

		/// <inheritdoc />
		public override InspectionPlanDtoBase Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			var type = GetType( ref reader );

			return type switch
			{
				InspectionPlanItemType.InspectionPlanPartDto => JsonSerializer.Deserialize<InspectionPlanPartDto>( ref reader, options ),

				InspectionPlanItemType.SimplePartDto => JsonSerializer.Deserialize<SimplePartDto>( ref reader, options ),

				InspectionPlanItemType.InspectionPlanCharacteristicDto => JsonSerializer.Deserialize<InspectionPlanCharacteristicDto>( ref reader, options ),

				_ => throw new NotImplementedException($"{type}")
			};
		}

		private static InspectionPlanItemType GetType( ref Utf8JsonReader reader )
		{
			var propertyNames = CollectPropertyNames( reader );

			if( propertyNames.Any( propertyName => propertyName.Equals( nameof( InspectionPlanPartDto.History ), StringComparison.OrdinalIgnoreCase ) ) &&
				propertyNames.Any( propertyName => propertyName.Equals( nameof( InspectionPlanPartDto.CharChangeDate ), StringComparison.OrdinalIgnoreCase ) ) )
			{
				return InspectionPlanItemType.InspectionPlanPartDto;
			}

			if( propertyNames.Any( propertyName => propertyName.Equals( nameof( SimplePartDto.CharChangeDate ), StringComparison.OrdinalIgnoreCase ) ) )
			{
				return InspectionPlanItemType.SimplePartDto;
			}

			return InspectionPlanItemType.InspectionPlanCharacteristicDto;
		}

		private static IReadOnlyList<string> CollectPropertyNames( Utf8JsonReader reader )
		{
			var propertyNames = new List<string>();

			while( reader.Read() && reader.TokenType != JsonTokenType.EndObject )
			{
				if( reader.TokenType == JsonTokenType.PropertyName )
				{
					propertyNames.Add( reader.GetString() );
				}
				else
				{
					reader.Skip();
				}
			}

			return propertyNames;
		}

		#endregion

		#region InspectionPlanItemType

		private enum InspectionPlanItemType
		{
			InspectionPlanPartDto,
			SimplePartDto,
			InspectionPlanCharacteristicDto
		}

		#endregion
	}
}