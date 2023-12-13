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
	using System.Linq;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class ValueSerializationTests
	{
		#region members

		private static readonly string ValuesJson = SerializationTestHelper.ReadResourceString( "Samples.values.json" );

		#endregion

		#region methods

		[Test]
		public void SerializingValues_SerializesProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<DataMeasurementDto>>( ValuesJson );
			var serialized = JsonConvert.SerializeObject( deserialized );

			Assert.That( serialized, Is.Not.Null.Or.Empty );
		}

		[Test]
		public void DeserializingValues_RestoresStructureProperly()
		{
			var deserialized = JsonConvert.DeserializeObject<IReadOnlyList<DataMeasurementDto>>( ValuesJson );

			Assert.That( deserialized, Is.Not.Null );
			Assert.That( deserialized, Has.Exactly( 600 ).Items );

			var firstMeasurement = deserialized[ 0 ];
			Assert.That( firstMeasurement.Uuid, Is.EqualTo( new Guid( "2d0652c7-8da8-47f8-8185-dc8fc19b56e9" ) ) );
			Assert.That( firstMeasurement.PartUuid, Is.EqualTo( new Guid( "fe85eefe-f08d-4e78-9f06-0e3b3cc9275e" ) ) );
			Assert.That( firstMeasurement.Attributes, Has.Exactly( 7 ).Items );
			Assert.That( firstMeasurement.Characteristics, Has.Exactly( 45 ).Items );

			var firstValue = firstMeasurement.Characteristics.First();
			Assert.That( firstValue.Value.Attributes, Has.One.Items );
			Assert.That( firstValue.Value.MeasuredValue, Is.Not.Null );
		}

		#endregion
	}
}