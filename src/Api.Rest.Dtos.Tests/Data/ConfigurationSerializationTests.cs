#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class ConfigurationSerializationTests
	{
		#region members

		private static readonly string ConfigurationJson = SerializationTestHelper.ReadResourceString( "Samples.configuration.json" );

		#endregion

		#region methods

		[Test]
		public void SerializingConfiguration_SerializesProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<ConfigurationDto>( ConfigurationJson );
			var serialized = JsonConvert.SerializeObject( deserialized );

			Assert.That( serialized, Is.Not.Null.Or.Empty );
		}

		[Test]
		public void DeserializingConfiguration_RestoresStructureProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<ConfigurationDto>( ConfigurationJson );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized.AllAttributes, Has.Exactly( 80 ).Items );
			Assert.That( deserialized.CatalogAttributes, Has.Exactly( 16 ).Items );
			Assert.That( deserialized.CharacteristicAttributes, Has.Exactly( 41 ).Items );
			Assert.That( deserialized.PartAttributes, Has.Exactly( 10 ).Items );
			Assert.That( deserialized.MeasurementAttributes, Has.Exactly( 12 ).Items );
			Assert.That( deserialized.ValueAttributes, Has.Exactly( 1 ).Items );

			Assert.That( deserialized.VersioningType, Is.EqualTo( VersioningTypeDto.Off ) );

			var measurementAttribute = deserialized.GetDefinition( EntityDto.Measurement, WellKnownKeys.Measurement.Time ) as AttributeDefinitionDto;
			Assert.That( measurementAttribute, Is.Not.Null );
			Assert.That( measurementAttribute.Key, Is.EqualTo( WellKnownKeys.Measurement.Time ) );
			Assert.That( measurementAttribute.QueryEfficient, Is.False );
			Assert.That( measurementAttribute.Type, Is.EqualTo( AttributeTypeDto.DateTime ) );
		}

		#endregion
	}
}