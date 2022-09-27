#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data.Converter
{
	#region usings

	using System.Text.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class InspectionPlanDtoBaseConverterTests
	{
		#region members

		private static readonly JsonSerializerOptions Options = new()
		{
			DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull,
			Converters = { new InspectionPlanDtoBaseConverter() }
		};

		#endregion

		#region methods

		[Test]
		public void Parse_HappyPath_ReturnsInspectionPlanPartDto()
		{
			const string json = "{\"attributes\": {\"1\": \"test\"}," +
								"\"path\": \"/\"," +
								"\"history\": null," +
								"\"charChangeDate\": \"0001-01-01T00:00:00\"," +
								"\"uuid\": \"00000000-0000-0000-0000-000000000000\"," +
								"\"comment\": null," +
								"\"version\": 0," +
								"\"timestamp\": \"0001-01-01T00:00:00\"}";

			var deserialized = JsonSerializer.Deserialize<InspectionPlanDtoBase>( json, Options );

			Assert.That( deserialized, Is.TypeOf<InspectionPlanPartDto>() );
		}

		[Test]
		public void Parse_HappyPath_ReturnsSimplePartDto()
		{
			const string json = "{\"attributes\": {\"1\": \"test\"}," +
								"\"path\": \"/\"," +
								"\"charChangeDate\": \"0001-01-01T00:00:00\"," +
								"\"uuid\": \"00000000-0000-0000-0000-000000000000\"," +
								"\"timestamp\": \"0001-01-01T00:00:00\"}";

			var deserialized = JsonSerializer.Deserialize<InspectionPlanDtoBase>( json, Options );

			Assert.That( deserialized, Is.TypeOf<SimplePartDto>() );
		}

		[Test]
		public void Parse_HappyPath_ReturnsInspectionPlanCharacteristicDto()
		{
			const string json = "{\"attributes\": {\"1\": \"test\"}," +
								"\"path\": \"C:/merkmal/\"," +
								"\"uuid\": \"00000000-0000-0000-0000-000000000000\"," +
								"\"comment\": null," +
								"\"version\": 0," +
								"\"timestamp\": \"0001-01-01T00:00:00\"}";

			var deserialized = JsonSerializer.Deserialize<InspectionPlanDtoBase>( json, Options );

			Assert.That( deserialized, Is.TypeOf<InspectionPlanCharacteristicDto>() );
		}

		#endregion
	}
}