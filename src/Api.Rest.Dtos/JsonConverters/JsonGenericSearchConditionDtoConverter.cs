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
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="GenericSearchConditionDto"/>-objects.
	/// </summary>
	public sealed class JsonGenericSearchConditionDtoConverter : JsonConverter<GenericSearchConditionDto>
	{
		#region methods

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, GenericSearchConditionDto value, JsonSerializerOptions options )
		{
			switch( value )
			{
				case null:
					writer.WriteNullValue();
					break;

				case GenericSearchNotDto condition:
					JsonSerializer.Serialize( writer, condition, options );
					break;

				case GenericSearchAndDto condition:
					JsonSerializer.Serialize( writer, condition, options );
					break;

				case GenericSearchAttributeConditionDto condition:
					JsonSerializer.Serialize( writer, condition, options );
					break;

				case GenericSearchFieldConditionDto condition:
					JsonSerializer.Serialize( writer, condition, options );
					break;

				default:
					throw new NotImplementedException( $"{value.GetType().Name}" );
			}
		}

		/// <inheritdoc />
		[CanBeNull]
		public override GenericSearchConditionDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			var propertyNames = CollectPropertyNames( reader );

			if( propertyNames.Contains( GenericSearchConditionDto.ConditionFieldName ) )
				return JsonSerializer.Deserialize<GenericSearchNotDto>( ref reader, options );

			if( propertyNames.Contains( GenericSearchConditionDto.ConditionsFieldName ) )
				return JsonSerializer.Deserialize<GenericSearchAndDto>( ref reader, options );

			if( propertyNames.Contains( GenericSearchConditionDto.AttributeFieldName ) )
				return JsonSerializer.Deserialize<GenericSearchAttributeConditionDto>( ref reader, options );

			if( propertyNames.Contains( GenericSearchConditionDto.FieldNameFieldName ) )
				return JsonSerializer.Deserialize<GenericSearchFieldConditionDto>( ref reader, options );

			throw new NotImplementedException( "Encountered unknown search condition type" );
		}


		[NotNull]
		private static HashSet<string> CollectPropertyNames( Utf8JsonReader reader )
		{
			var propertyNames = new HashSet<string>( StringComparer.OrdinalIgnoreCase );

			var startDepth = reader.CurrentDepth;

			while( reader.Read() && ( reader.TokenType != JsonTokenType.EndObject || reader.CurrentDepth != startDepth ) )
			{
				if( reader.TokenType == JsonTokenType.PropertyName && reader.CurrentDepth == startDepth + 1 )
					propertyNames.Add( reader.GetString() );
				else
					reader.TrySkip();
			}

			return propertyNames;
		}

		#endregion
	}
}