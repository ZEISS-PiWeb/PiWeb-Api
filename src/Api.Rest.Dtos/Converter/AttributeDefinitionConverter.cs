#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Converter
{
	#region usings

	using System;
	using System.Globalization;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="AbstractAttributeDefinition"/>-objects.
	/// </summary>
	public class AttributeDefinitionConverter : JsonConverter
	{
		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type objectType )
		{
			return typeof( AbstractAttributeDefinition ) == objectType || typeof( AttributeDefinition ) == objectType || typeof( CatalogAttributeDefinition ) == objectType;
		}

		/// <inheritdoc />
		public override object ReadJson( JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer )
		{
			var key = default( ushort );
			var length = default( ushort );
			var queryEfficient = default( bool );
			var description = default( string );
			var attributeDefinitionType = default( string );
			var type = default( AttributeType );
			var catalogUuid = default( Guid );

			while( reader.Read() && reader.TokenType == JsonToken.PropertyName )
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

		/// <inheritdoc />
		public override void WriteJson( JsonWriter writer, object value, JsonSerializer serializer )
		{
			if( value is AbstractAttributeDefinition definition )
			{
				writer.WriteStartObject();
				writer.WritePropertyName( "key" );
				writer.WriteValue( definition.Key );
				writer.WritePropertyName( "description" );
				writer.WriteValue( definition.Description );

				if( definition is AttributeDefinition attributeDef )
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

				if( definition is CatalogAttributeDefinition catalogDef )
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