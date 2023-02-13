#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2023                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="GenericSearchConditionDto"/>-objects.
	/// </summary>
	public sealed class GenericSearchConditionConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return objectType == typeof( GenericSearchConditionDto );
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( reader == null )
				throw new ArgumentNullException( nameof( reader ) );

			if( reader.TokenType == JsonToken.Null )
				return null;

			var jToken = JToken.Load( reader );

			if( jToken == null )
				throw new ArgumentNullException( nameof( jToken ) );

			var target = existingValue ?? ReadJson( jToken, serializer );

			if( target == null )
				throw new JsonSerializationException( "No object created." );

			return target;
		}

		[CanBeNull]
		private static GenericSearchConditionDto ReadJson( JToken jToken, JsonSerializer serializer )
		{
			using var jsonReader = new JTokenReader( jToken );

			var fields = CollectFields( jToken );

			if( fields.Contains( GenericSearchConditionDto.NotDiscriminator ) )
				return serializer.Deserialize<GenericSearchNotDto>( jsonReader );

			if( fields.Contains( GenericSearchConditionDto.AndDiscriminator ) )
				return serializer.Deserialize<GenericSearchAndDto>( jsonReader );

			if( fields.Contains( GenericSearchConditionDto.FieldConditionDiscriminator ) )
				return serializer.Deserialize<GenericSearchFieldConditionDto>( jsonReader );

			if( fields.Contains( GenericSearchConditionDto.AttributeConditionDiscriminator ) )
				return serializer.Deserialize<GenericSearchAttributeConditionDto>( jsonReader );

			return null;
		}

		[NotNull]
		private static HashSet<string> CollectFields( JToken jToken )
		{
			return new HashSet<string>( jToken.Children<JProperty>().Select( i => i.Path ), StringComparer.OrdinalIgnoreCase );
		}

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			switch( value )
			{
				case null:
					writer.WriteNull();
					break;

				case GenericSearchNotDto _:
					serializer.Serialize( writer, value, typeof( GenericSearchNotDto ) );
					break;

				case GenericSearchAndDto _:
					serializer.Serialize( writer, value, typeof( GenericSearchAndDto ) );
					break;

				case GenericSearchAttributeConditionDto _:
					serializer.Serialize( writer, value, typeof( GenericSearchAttributeConditionDto ) );
					break;

				case GenericSearchFieldConditionDto _:
					serializer.Serialize( writer, value, typeof( GenericSearchFieldConditionDto ) );
					break;

				default:
					throw new JsonSerializationException( $"{nameof( value )} is invalid!" );
			}
		}

		#endregion
	}
}