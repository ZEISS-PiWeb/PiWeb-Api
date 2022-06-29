#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Linq;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="InspectionPlanDtoBase"/>-objects.
	/// </summary>
	public sealed class InspectionPlanDtoBaseConverter : JsonConverter
	{
		#region constants

		private const string HistoryFieldName = "history";
		private const string CharChangeDateFieldName = "charChangeDate";

		#endregion

		#region methods

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			switch( value )
			{
				case null:
					writer.WriteNull();
					break;

				case InspectionPlanPartDto _:
					serializer.Serialize( writer, value, typeof( InspectionPlanPartDto ) );
					break;

				case InspectionPlanCharacteristicDto _:
					serializer.Serialize( writer, value, typeof( InspectionPlanCharacteristicDto ) );
					break;

				case SimplePartDto _:
					serializer.Serialize( writer, value, typeof( SimplePartDto ) );
					break;

				default:
					throw new JsonSerializationException( $"{nameof( value )} is invalid!" );
			}
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			if( reader == null )
				throw new ArgumentNullException( nameof( reader ) );

			if( reader.TokenType == JsonToken.Null )
				return null;

			var jToken = JToken.Load( reader );

			if( jToken == null )
				throw new ArgumentNullException( nameof( jToken ) );

			var target = existingValue ?? Create( jToken, serializer );

			if( target == null )
				throw new JsonSerializationException( "No object created." );

			return target;
		}

		private static object Create( JToken jObject, JsonSerializer serializer )
		{
			using( var jsonReader = new JTokenReader( jObject ) )
			{
				var type = GetType( jObject );

				switch( type )
				{
					case InspectionPlanItemType.SimplePartDto:
						return serializer.Deserialize<SimplePartDto>( jsonReader );

					case InspectionPlanItemType.InspectionPlanPartDto:
						return serializer.Deserialize<InspectionPlanPartDto>( jsonReader );

					case InspectionPlanItemType.InspectionPlanCharacteristicDto:
						return serializer.Deserialize<InspectionPlanCharacteristicDto>( jsonReader );

					default:
						throw new InvalidOperationException( "No object created." );
				}
			}
		}

		private static InspectionPlanItemType GetType( JToken jToken )
		{
			var fields = CollectFields( jToken );

			if( fields.Exists( x => x.Equals( HistoryFieldName, StringComparison.OrdinalIgnoreCase ) ) &&
				fields.Exists( x => x.Equals( CharChangeDateFieldName, StringComparison.OrdinalIgnoreCase ) ) )
				return InspectionPlanItemType.InspectionPlanPartDto;

			if( fields.Exists( x => x.Equals( CharChangeDateFieldName, StringComparison.OrdinalIgnoreCase ) ) )
				return InspectionPlanItemType.SimplePartDto;

			return InspectionPlanItemType.InspectionPlanCharacteristicDto;
		}

		private static List<string> CollectFields( JToken jToken )
		{
			return jToken.Children<JProperty>().Select( i => i.Path ).ToList();
		}

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return objectType == typeof( InspectionPlanDtoBase );
		}

		#endregion

		#region class InspectionPlanItemType

		private enum InspectionPlanItemType
		{
			SimplePartDto,
			InspectionPlanPartDto,
			InspectionPlanCharacteristicDto
		}

		#endregion
	}
}