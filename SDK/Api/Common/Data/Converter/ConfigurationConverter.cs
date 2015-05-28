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

	using DataService;

	#endregion

	/// <summary>
	/// Specialized <see cref="Newtonsoft.Json.JsonConverter"/> for <see cref="DataService.Configuration"/>-objects.
	/// </summary>
	public class ConfigurationConverter : Newtonsoft.Json.JsonConverter
	{
		#region members

		/// <summary>
		/// Determines whether this instance can convert the specified object type.
		/// </summary>
		public override bool CanConvert( Type objectType )
		{
			return typeof( Configuration ) == objectType;
		}

		/// <summary>
		/// Reads the JSON representation of the object.
		/// </summary>
		public override object ReadJson( Newtonsoft.Json.JsonReader reader, Type objectType, object existingValue, Newtonsoft.Json.JsonSerializer serializer )
		{
			var configuration = new Configuration();

			while( reader.Read() && reader.TokenType == Newtonsoft.Json.JsonToken.PropertyName )
			{
				switch( reader.Value.ToString() )
				{
					case "partAttributes":
						reader.Read();
						configuration.PartAttributes = serializer.Deserialize<AbstractAttributeDefinition[]>( reader );
						break;
					case "characteristicAttributes":
						reader.Read();
						configuration.CharacteristicAttributes = serializer.Deserialize<AbstractAttributeDefinition[]>( reader );
						break;
					case "measurementAttributes":
						reader.Read();
						configuration.MeasurementAttributes = serializer.Deserialize<AbstractAttributeDefinition[]>( reader );
						break;
					case "valueAttributes":
						reader.Read();
						configuration.ValueAttributes = serializer.Deserialize<AbstractAttributeDefinition[]>( reader );
						break;
					case "catalogAttributes":
						reader.Read();
						configuration.CatalogueAttributes = serializer.Deserialize<AttributeDefinition[]>( reader );
						break;
				}
			}
			return configuration;
		}

		/// <summary>
		/// Writes the JSON representation of the object.
		/// </summary>
		public override void WriteJson( Newtonsoft.Json.JsonWriter writer, object value, Newtonsoft.Json.JsonSerializer serializer )
		{
			var configuration = value as Configuration;
			if( configuration != null )
			{
				writer.WriteStartObject();

				if(configuration.PartAttributes.Length > 0 )
				{
					writer.WritePropertyName( "partAttributes" );
					writer.WriteStartArray();
					foreach( var att in configuration.PartAttributes )
					{
						serializer.Serialize( writer, att );
					}
					writer.WriteEndArray();
				}

				if( configuration.CharacteristicAttributes.Length > 0 )
				{
					writer.WritePropertyName( "characteristicAttributes" );
					writer.WriteStartArray();
					foreach( var att in configuration.CharacteristicAttributes )
					{
						serializer.Serialize( writer, att );
					}
					writer.WriteEndArray();
				}

				if( configuration.MeasurementAttributes.Length > 0 )
				{
					writer.WritePropertyName( "measurementAttributes" );
					writer.WriteStartArray();
					foreach( var att in configuration.MeasurementAttributes )
					{
						serializer.Serialize( writer, att );
					}
					writer.WriteEndArray();
				}

				if( configuration.ValueAttributes.Length > 0 )
				{
					writer.WritePropertyName( "valueAttributes" );
					writer.WriteStartArray();
					foreach( var att in configuration.ValueAttributes )
					{
						serializer.Serialize( writer, att );
					}
					writer.WriteEndArray();
				}

				if( configuration.CatalogueAttributes.Length > 0 )
				{
					writer.WritePropertyName( "catalogAttributes" );
					writer.WriteStartArray();
					foreach( var att in configuration.CatalogueAttributes )
					{
						serializer.Serialize( writer, att );
					}
					writer.WriteEndArray();
				}

				writer.WriteEndObject();
			}
		}

		#endregion
	}
}