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
	using System.Globalization;

	using DataService;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="CatalogEntry"/>-objects.
	/// </summary>
	public class CatalogEntryConverter : Newtonsoft.Json.JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( CatalogEntry ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			var catalogEntry = new CatalogEntry();

			while( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
			{
				switch( reader.Value.ToString() )
				{
					case "key":
						catalogEntry.Key = short.Parse( reader.ReadAsString(), CultureInfo.InvariantCulture );
						break;
					case "attributes":
						reader.Read();
						catalogEntry.Attributes = serializer.Deserialize<DataService.Attribute[]>( reader );
						break;
				}
			}
			return catalogEntry;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			var catEntry = value as CatalogEntry;
			if( catEntry != null )
			{
				writer.WriteStartObject();
				writer.WritePropertyName( "key" );
				writer.WriteValue( catEntry.Key );

				if( catEntry.Attributes.Length > 0 )
				{
					writer.WritePropertyName( "attributes" );
					serializer.Serialize( writer, catEntry.Attributes );
				}
				writer.WriteEndObject();
			}
		}

		#endregion
	}
}