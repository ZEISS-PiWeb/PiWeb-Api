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
	using System.IO;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class MeasurementSerializationTests
	{
		#region members

		private static readonly string MeasurementsJson = SerializationTestHelper.ReadResourceString( "Samples.measurements.json" );

		#endregion

		#region methods

		[Test]
		public void SerializingMeasurements_SerializesProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );
			var serialized = JsonConvert.SerializeObject( deserialized );

			Assert.That( serialized, Is.Not.Null.Or.Empty );
		}

		[Test]
		public void DeserializingMeasurements_RestoresStructureProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<SimpleMeasurementDto>>( MeasurementsJson );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized, Has.Exactly( 600 ).Items );

			var firstMeasurement = deserialized[ 0 ];
			Assert.That( firstMeasurement.Uuid, Is.EqualTo( new Guid( "2d0652c7-8da8-47f8-8185-dc8fc19b56e9" ) ) );
			Assert.That( firstMeasurement.PartUuid, Is.EqualTo( new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ) ) );
			Assert.That( firstMeasurement.Attributes, Has.Exactly( 7 ).Items );
		}

		#endregion
	}
}