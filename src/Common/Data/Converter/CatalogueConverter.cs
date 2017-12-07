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

	using DataService;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataService.Catalog"/>-objects.
	/// </summary>
	public class CatalogueConverter : Newtonsoft.Json.JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( Catalog ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			var catalogue = new Catalog();
			if( reader.TokenType == Newtonsoft.Json.JsonToken.StartObject )
			{
				while( reader.Read() && reader.TokenType != Newtonsoft.Json.JsonToken.EndObject )
				{
					switch( reader.Value.ToString() )
					{
						case "name": catalogue.Name = reader.ReadAsString(); break;
						case "uuid": catalogue.Uuid = new Guid( reader.ReadAsString() ); break;
						case "validAttributes":
							if( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.StartArray )
							{
								var atts = new List<ushort>();
								while( reader.Read() && reader.TokenType != Newtonsoft.Json.JsonToken.EndArray )
								{
									atts.Add( Convert.ToUInt16( reader.Value ) );
								}
								catalogue.ValidAttributes = atts.ToArray();
							}
							break;
						case "catalogueEntries":
							if( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.StartArray )
							{
								if( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.StartObject )
								{
									var entries = new List< CatalogEntry >();
									while( reader.TokenType != Newtonsoft.Json.JsonToken.EndArray )
									{
										entries.Add(serializer.Deserialize<CatalogEntry>( reader ));
										reader.Read();
									}
									catalogue.CatalogEntries = entries.ToArray();
								}
							}
							break;

					}
				}
			}
			return catalogue;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			var catalogue = (Catalog)value;
			if( catalogue.Uuid != Guid.Empty )
			{
				writer.WriteStartObject();

				writer.WritePropertyName( "uuid" );
				writer.WriteValue( catalogue.Uuid );

				if( !String.IsNullOrEmpty( catalogue.Name ) )
				{
					writer.WritePropertyName( "name" );
					writer.WriteValue( catalogue.Name );
				}

				if( catalogue.ValidAttributes.Length > 0 )
				{
					writer.WritePropertyName( "validAttributes" );
					writer.WriteStartArray();
					foreach( var att in catalogue.ValidAttributes )
					{
						writer.WriteValue( att );
					}
					writer.WriteEndArray();
				}

				if( catalogue.CatalogEntries.Length > 0 )
				{
					writer.WritePropertyName( "catalogueEntries" );
					serializer.Serialize( writer, catalogue.CatalogEntries );
				}

				writer.WriteEndObject();
			}
		}

		#endregion
	}
}
