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
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataService.Attribute"/>-objects.
	/// </summary>
	public class AttributeConverter : Newtonsoft.Json.JsonConverter
	{
		#region methods

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( DataService.Attribute[] ) == objectType || typeof( DataService.Attribute ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			if( typeof( DataService.Attribute ) == objectType )
			{
				var result = default( DataService.Attribute );
				if( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
				{
					var key = ushort.Parse( reader.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture );
					var value = reader.ReadAsString();

					result = new DataService.Attribute( key, value );
				}
				return result;
			}
			else
			{
				var result = new List<DataService.Attribute>();
				if( reader.TokenType == Newtonsoft.Json.JsonToken.StartObject )
				{
					while( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
					{
						var key = ushort.Parse( reader.Value.ToString(), System.Globalization.CultureInfo.InvariantCulture );
						var value = reader.ReadAsString();

						result.Add( new DataService.Attribute( key, value ) );
					}
				}
				return result.ToArray();
			}
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			var attribute = value as DataService.Attribute;
			if( attribute != null )
			{
				writer.WritePropertyName( attribute.Key.ToString( System.Globalization.CultureInfo.InvariantCulture ) );
				writer.WriteValue( attribute.Value );
			}
			else
			{
				writer.WriteStartObject();

				var attributes = (DataService.Attribute[])value;
				if( attributes != null && attributes.Length > 0 )
				{
					foreach( var att in attributes )
					{
						writer.WritePropertyName( att.Key.ToString( System.Globalization.CultureInfo.InvariantCulture ) );
						writer.WriteValue( att.Value );
					}
				}
				writer.WriteEndObject();
			}
		}

		#endregion
	}
}