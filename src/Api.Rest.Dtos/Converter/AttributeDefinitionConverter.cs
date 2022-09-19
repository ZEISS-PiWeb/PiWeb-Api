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
	using System.Text;
	using System.Text.Json;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="AbstractAttributeDefinitionDto"/>-objects.
	/// </summary>
	public sealed class AbstractAttributeDefinitionConverter : JsonConverter<AbstractAttributeDefinitionDto>
	{
		#region members

		private static readonly byte[] KeyPropertyName = Encoding.UTF8.GetBytes( "key" );
		private static readonly byte[] QueryEfficientPropertyName = Encoding.UTF8.GetBytes( "queryEfficient" );
		private static readonly byte[] DescriptionPropertyName = Encoding.UTF8.GetBytes( "description" );
		private static readonly byte[] LengthPropertyName = Encoding.UTF8.GetBytes( "length" );
		private static readonly byte[] TypePropertyName = Encoding.UTF8.GetBytes( "type" );
		private static readonly byte[] CatalogPropertyName = Encoding.UTF8.GetBytes( "catalog" );
		private static readonly byte[] DefinitionTypePropertyName = Encoding.UTF8.GetBytes( "definitionType" );

		private static readonly byte[] AttributeDefinitionTypeId = Encoding.UTF8.GetBytes( "AttributeDefinition" );
		private static readonly byte[] CatalogAttributeDefinitionTypeId = Encoding.UTF8.GetBytes( "CatalogAttributeDefinition" );

		#endregion

		#region methods

		/// <inheritdoc />
		public override bool CanConvert( Type typeToConvert )
		{
			return typeof( AbstractAttributeDefinitionDto ).IsAssignableFrom( typeToConvert );
		}

		/// <inheritdoc />
		public override AbstractAttributeDefinitionDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			return ReadFromObject( ref reader, options );
		}

		internal static AbstractAttributeDefinitionDto ReadFromObject( ref Utf8JsonReader reader, JsonSerializerOptions options )
		{
			var key = default( ushort );
			var length = default( ushort? );
			var queryEfficient = default( bool );
			var description = default( string );
			var attributeDefinitionType = default( ReadOnlySpan<byte> );
			var type = default( AttributeTypeDto );
			var catalogUuid = default( Guid );

			while( reader.Read() && reader.TokenType == JsonTokenType.PropertyName )
			{
				if( reader.ValueTextEquals( KeyPropertyName ) )
				{
					reader.Read();
					key = reader.GetUInt16();
				}
				else if( reader.ValueTextEquals( QueryEfficientPropertyName ) )
				{
					reader.Read();
					queryEfficient = reader.GetBoolean();
				}
				else if( reader.ValueTextEquals( DescriptionPropertyName ) )
				{
					reader.Read();
					description = reader.GetString();
				}
				else if( reader.ValueTextEquals( LengthPropertyName ) )
				{
					reader.Read();
					length = reader.GetUInt16();
				}
				else if( reader.ValueTextEquals( TypePropertyName ) )
				{
					reader.Read();
					type = JsonSerializer.Deserialize<AttributeTypeDto>( ref reader, options );
				}
				else if( reader.ValueTextEquals( CatalogPropertyName ) )
				{
					reader.Read();
					catalogUuid = reader.GetGuid();
				}
				else if( reader.ValueTextEquals( DefinitionTypePropertyName ) )
				{
					reader.Read();
					attributeDefinitionType = reader.ValueSpan;
				}
			}

			if( attributeDefinitionType.SequenceEqual( AttributeDefinitionTypeId ) )
				return new AttributeDefinitionDto { Description = description, Key = key, Length = length, QueryEfficient = queryEfficient, Type = type };
			else if( attributeDefinitionType.SequenceEqual( CatalogAttributeDefinitionTypeId ) )
				return new CatalogAttributeDefinitionDto { Description = description, Key = key, QueryEfficient = queryEfficient, Catalog = catalogUuid };

			return null;
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, AbstractAttributeDefinitionDto value, JsonSerializerOptions options )
		{
			WriteAsObject( writer, value, options );
		}

		internal static void WriteAsObject( Utf8JsonWriter writer, AbstractAttributeDefinitionDto value, JsonSerializerOptions options )
		{
			writer.WriteStartObject();

			writer.WriteNumber( KeyPropertyName, value.Key );
			writer.WriteString( DescriptionPropertyName, value.Description );

			if( value is AttributeDefinitionDto attributeDef )
			{
				if( attributeDef.Length.HasValue )
					writer.WriteNumber( LengthPropertyName, attributeDef.Length.Value );

				writer.WritePropertyName( TypePropertyName );
				JsonSerializer.Serialize( writer, attributeDef.Type, options );

				writer.WriteString( DefinitionTypePropertyName, AttributeDefinitionTypeId );
			}

			if( value is CatalogAttributeDefinitionDto catalogDef )
			{
				writer.WriteString( CatalogPropertyName, catalogDef.Catalog );
				writer.WriteString( DefinitionTypePropertyName, CatalogAttributeDefinitionTypeId );
			}

			writer.WriteEndObject();
		}

		#endregion
	}

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="AttributeDefinitionDto"/>-objects.
	/// </summary>
	public sealed class JsonAttributeDefinitionConverter : JsonConverter<AttributeDefinitionDto>
	{
		/// <inheritdoc />
		public override AttributeDefinitionDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			return (AttributeDefinitionDto)AbstractAttributeDefinitionConverter.ReadFromObject( ref reader, options );
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, AttributeDefinitionDto value, JsonSerializerOptions options )
		{
			AbstractAttributeDefinitionConverter.WriteAsObject( writer, value, options );
		}
	}

	/// <summary>
	/// Specialized <see cref="JsonConverter"/> for <see cref="CatalogAttributeDefinitionDto"/>-objects.
	/// </summary>
	public sealed class JsonCatalogAttributeDefinitionConverter : JsonConverter<CatalogAttributeDefinitionDto>
	{
		/// <inheritdoc />
		public override CatalogAttributeDefinitionDto Read( ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options )
		{
			return (CatalogAttributeDefinitionDto)AbstractAttributeDefinitionConverter.ReadFromObject( ref reader, options );
		}

		/// <inheritdoc />
		public override void Write( Utf8JsonWriter writer, CatalogAttributeDefinitionDto value, JsonSerializerOptions options )
		{
			AbstractAttributeDefinitionConverter.WriteAsObject( writer, value, options );
		}
	}
}