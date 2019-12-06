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
	using System.Globalization;
	using Zeiss.IMT.PiWeb.Api.DataService.Rest;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="AbstractAttributeDefinition"/>-objects.
	/// </summary>
	public class AttributeDefinitionConverter : Newtonsoft.Json.JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( AbstractAttributeDefinition ) == objectType || typeof( AttributeDefinition ) == objectType || typeof( CatalogAttributeDefinition ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			var key = default( ushort );
			var length = default( ushort );
			var queryEfficient = default( bool );
			var description = default( string );
			var attributeDefinitionType = default( string );
			var type = default( AttributeType );
			var catalogUuid = default( Guid );

			while( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
			{
				switch( reader.Value.ToString() )
				{
					case "key":
						key = ushort.Parse( reader.ReadAsString(), CultureInfo.InvariantCulture );
						break;
					case "queryEfficient":
						queryEfficient = bool.Parse( reader.ReadAsString() );
						break;
					case "description":
						description = reader.ReadAsString();
						break;
					case "length":
						length = ushort.Parse( reader.ReadAsString(), CultureInfo.InvariantCulture );
						break;
					case "type":
						type = (AttributeType)Enum.Parse( typeof( AttributeType ), reader.ReadAsString() );
						break;
					case "catalog":
						catalogUuid = Guid.Parse( reader.ReadAsString() );
						break;
					case "definitionType":
						attributeDefinitionType = reader.ReadAsString();
						break;
				}
			}

			if( attributeDefinitionType == "AttributeDefinition" )
				return new AttributeDefinition { Description = description, Key = key, Length = length, QueryEfficient = queryEfficient, Type = type };
	
			if( attributeDefinitionType == "CatalogAttributeDefinition" )
				return new CatalogAttributeDefinition { Description = description, Key = key, QueryEfficient = queryEfficient, Catalog = catalogUuid };

			return null;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			var definition = value as AbstractAttributeDefinition;
			if( definition != null )
			{
				writer.WriteStartObject();
				writer.WritePropertyName( "key" );
				writer.WriteValue( definition.Key );
				writer.WritePropertyName( "description" );
				writer.WriteValue( definition.Description );

				var attributeDef = definition as AttributeDefinition;
				if( attributeDef != null )
				{
					if( attributeDef.Length.HasValue )
					{
						writer.WritePropertyName( "length" );
						writer.WriteValue( attributeDef.Length.Value );
					}
					writer.WritePropertyName( "type" );
					serializer.Serialize( writer, attributeDef.Type );
					writer.WritePropertyName( "definitionType" );
					writer.WriteValue( "AttributeDefinition" );
				}

				var catalogDef = definition as CatalogAttributeDefinition;
				if( catalogDef != null )
				{
					writer.WritePropertyName( "catalog" );
					writer.WriteValue( catalogDef.Catalog );
					writer.WritePropertyName( "definitionType" );
					writer.WriteValue( "CatalogAttributeDefinition" );
				}

				writer.WriteEndObject();
			}
		}

		#endregion
	}
}