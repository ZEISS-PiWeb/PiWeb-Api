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

	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Core;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Zeiss.PiWeb.Api.Core.Attribute;

	#endregion

	[TestFixture]
	public class InspectionPlanSerializationTests
	{
		#region members

		private static readonly string PartJson = SerializationTestHelper.ReadResourceString( "Samples.parts.json" );
		private static readonly string CharacteristicJson = SerializationTestHelper.ReadResourceString( "Samples.characteristics.json" );

		#endregion

		#region methods

		[Test]
		public void SerializingParts_SerializesProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanPartDto>>( PartJson );
			var serialized = JsonConvert.SerializeObject( deserialized );

			Assert.That( serialized, Is.Not.Null.Or.Empty );
		}

		[Test]
		public void SerializingCharacteristics_SerializesProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );
			var serialized = JsonConvert.SerializeObject( deserialized );

			Assert.That( serialized, Is.Not.Null.Or.Empty );
		}

		[Test]
		public void DeserializingParts_RestoresStructureProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanPartDto>>( PartJson );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized, Has.Exactly( 2 ).Items );

			Assert.That( deserialized[ 0 ].Uuid, Is.EqualTo( Guid.Empty ) );
			Assert.That( deserialized[ 0 ].Path, Is.EqualTo( PathInformation.Root ) );
			Assert.That( deserialized[ 0 ].Comment, Is.Null );
			Assert.That( deserialized[ 0 ].Timestamp, Is.EqualTo( DateTime.Parse( "2022-01-31T19:00:37.15Z" ).ToUniversalTime() ) );
			Assert.That( deserialized[ 0 ].CharChangeDate, Is.EqualTo( DateTime.Parse( "2022-06-30T06:25:35.56Z" ).ToUniversalTime() ) );
			Assert.That( deserialized[ 0 ].Comment, Is.Null );
			Assert.That( deserialized[ 0 ].Version, Is.EqualTo( 0 ) );
			Assert.That( deserialized[ 0 ].Attributes, Is.Empty );

			Assert.That( deserialized[ 1 ].Uuid, Is.EqualTo( new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ) ) );
			Assert.That( deserialized[ 1 ].Path, Is.EqualTo( new PathInformation( PathElement.Part( "Blechteil" ) ) ) );
			Assert.That( deserialized[ 1 ].Comment, Is.Null );
			Assert.That( deserialized[ 1 ].Timestamp, Is.EqualTo( DateTime.Parse( "2022-06-30T06:25:35.46Z" ).ToUniversalTime() ) );
			Assert.That( deserialized[ 1 ].CharChangeDate, Is.EqualTo( DateTime.Parse( "2022-01-31T19:02:58.767Z" ).ToUniversalTime() ) );
			Assert.That( deserialized[ 1 ].Comment, Is.Null );
			Assert.That( deserialized[ 1 ].Version, Is.EqualTo( 0 ) );
			Assert.That( deserialized[ 1 ].Attributes, Has.Exactly( 10 ).Items );
			Assert.That( deserialized[ 1 ].Attributes[ 0 ], Is.EqualTo( new Attribute( 1001, "122345" ) ) );
			Assert.That( deserialized[ 1 ].Attributes[ 1 ], Is.EqualTo( new Attribute( 1003, "Blechteil" ) ) );
			Assert.That( deserialized[ 1 ].Attributes[ 9 ], Is.EqualTo( new Attribute( 1900, "Kommentar" ) ) );
		}

		[Test]
		public void DeserializingCharacteristics_RestoresStructureProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<InspectionPlanCharacteristicDto>>( CharacteristicJson );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized, Has.Exactly( 45 ).Items );

			Assert.That( deserialized[ 0 ].Uuid, Is.EqualTo( new Guid( "b71a5bd7-5406-46a3-a5b7-458ba1c0248d" ) ) );
			Assert.That( deserialized[ 0 ].Path, Is.EqualTo( new PathInformation( PathElement.Part( "Blechteil" ), PathElement.Char( "Abweichung_3" ) ) ) );
			Assert.That( deserialized[ 0 ].Comment, Is.Null );
			Assert.That( deserialized[ 0 ].Timestamp, Is.EqualTo( DateTime.Parse( "2022-01-31T19:02:58.683Z" ).ToUniversalTime() ) );
			Assert.That( deserialized[ 0 ].Comment, Is.Null );
			Assert.That( deserialized[ 0 ].Version, Is.EqualTo( 0 ) );
			Assert.That( deserialized[ 0 ].Attributes, Has.Exactly( 16 ).Items );

			Assert.That( deserialized[ 1 ].Uuid, Is.EqualTo( new Guid( "8c72afa6-fc67-4fbd-8606-e3727d79c8ff" ) ) );
			Assert.That( deserialized[ 1 ].Path, Is.EqualTo( new PathInformation( PathElement.Part( "Blechteil" ), PathElement.Char( "Abweichung_4" ) ) ) );
			Assert.That( deserialized[ 1 ].Comment, Is.Null );
			Assert.That( deserialized[ 1 ].Timestamp, Is.EqualTo( DateTime.Parse( "2022-01-31T19:02:58.683Z" ).ToUniversalTime() ) );
			Assert.That( deserialized[ 1 ].Comment, Is.Null );
			Assert.That( deserialized[ 1 ].Version, Is.EqualTo( 0 ) );
			Assert.That( deserialized[ 1 ].Attributes, Has.Exactly( 16 ).Items );
		}

		#endregion
	}
}