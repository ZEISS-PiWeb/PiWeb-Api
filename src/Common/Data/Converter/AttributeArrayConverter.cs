#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.Converter
{
	#region using

	using System;
	using System.Collections.Generic;
	using Attribute = Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="Attribute"/> arrays.
	/// </summary>
	public class AttributeArrayConverter : Newtonsoft.Json.JsonConverter
	{
		#region methods

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( Attribute[] ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			var result = new List<Attribute>();
			if( reader.TokenType == Newtonsoft.Json.JsonToken.StartObject )
			{
				while( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
				{
					var key = AttributeKeyCache.Cache.StringToKey( reader.Value.ToString() );
					var value = reader.ReadAsString();

					result.Add( new Attribute( key, value ) );
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			writer.WriteStartObject();

			var attributes = (Attribute[])value;
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