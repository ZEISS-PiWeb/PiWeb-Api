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
	public class CatalogConverter : Newtonsoft.Json.JsonConverter
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
			var catalog = new Catalog();
			if( reader.TokenType == Newtonsoft.Json.JsonToken.StartObject )
			{
				while( reader.Read() && reader.TokenType != Newtonsoft.Json.JsonToken.EndObject )
				{
					switch( reader.Value.ToString() )
					{
						case "name": catalog.Name = reader.ReadAsString(); break;
						case "uuid": catalog.Uuid = new Guid( reader.ReadAsString() ); break;
						case "validAttributes":
							if( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.StartArray )
							{
								var atts = new List<ushort>();
								while( reader.Read() && reader.TokenType != Newtonsoft.Json.JsonToken.EndArray )
								{
									atts.Add( Convert.ToUInt16( reader.Value ) );
								}
								catalog.ValidAttributes = atts.ToArray();
							}
							break;
						case "catalogEntries":
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
									catalog.CatalogEntries = entries.ToArray();
								}
							}
							break;

					}
				}
			}
			return catalog;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			var catalog = (Catalog)value;
			if( catalog.Uuid != Guid.Empty )
			{
				writer.WriteStartObject();

				writer.WritePropertyName( "uuid" );
				writer.WriteValue( catalog.Uuid );

				if( !String.IsNullOrEmpty( catalog.Name ) )
				{
					writer.WritePropertyName( "name" );
					writer.WriteValue( catalog.Name );
				}

				if( catalog.ValidAttributes.Length > 0 )
				{
					writer.WritePropertyName( "validAttributes" );
					writer.WriteStartArray();
					foreach( var att in catalog.ValidAttributes )
					{
						writer.WriteValue( att );
					}
					writer.WriteEndArray();
				}

				if( catalog.CatalogEntries.Length > 0 )
				{
					writer.WritePropertyName( "catalogEntries" );
					serializer.Serialize( writer, catalog.CatalogEntries );
				}

				writer.WriteEndObject();
			}
		}

		#endregion
	}
}
