#region copyright
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
// /* Carl Zeiss IMT (IZM Dresden)                    */
// /* Softwaresystem PiWeb                            */
// /* (c) Carl Zeiss 2016                             */
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion
namespace Zeiss.IMT.PiWeb.Api.Common.Data.Converter
{
	#region usings

	using System;
	using Attribute = Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute;

	#endregion

	public class AttributeConverter : Newtonsoft.Json.JsonConverter
	{
		#region methods

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( Attribute ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			var result = new Attribute();

			if( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
			{
				var key = AttributeKeyCache.Cache.StringToKey( reader.Value.ToString() );
				var value = reader.ReadAsString();

				result = new Attribute( key, value );
			}

			return result;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
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