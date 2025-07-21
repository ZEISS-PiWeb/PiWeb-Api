#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data.Converter
{
	#region usings

	using FluentAssertions;
	using Newtonsoft.Json;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class InspectionPlanDtoBaseConverterTests
	{
		#region methods

		[Test]
		public void Parse_HappyPath_ReturnsInspectionPlanPartDto()
		{
			var json = "{\"attributes\": {\"1\": \"test\"}," +
						"\"path\": \"/\"," +
						"\"history\": null," +
						"\"charChangeDate\": \"0001-01-01T00:00:00\"," +
						"\"uuid\": \"00000000-0000-0000-0000-000000000000\"," +
						"\"comment\": null," +
						"\"version\": 0," +
						"\"timestamp\": \"0001-01-01T00:00:00\"}";

			var deserialized = JsonConvert.DeserializeObject<InspectionPlanDtoBase>(
				json,
				new InspectionPlanDtoBaseConverter() );

			deserialized.GetType().Should().Be( typeof( InspectionPlanPartDto ) );
		}

		[Test]
		public void Parse_HappyPath_ReturnsSimplePartDto()
		{
			var json = "{\"attributes\": {\"1\": \"test\"}," +
						"\"path\": \"/\"," +
						"\"charChangeDate\": \"0001-01-01T00:00:00\"," +
						"\"uuid\": \"00000000-0000-0000-0000-000000000000\"," +
						"\"timestamp\": \"0001-01-01T00:00:00\"}";

			var deserialized = JsonConvert.DeserializeObject<InspectionPlanDtoBase>(
				json,
				new InspectionPlanDtoBaseConverter() );

			deserialized.GetType().Should().Be( typeof( SimplePartDto ) );
		}

		[Test]
		public void Parse_HappyPath_ReturnsInspectionPlanCharacteristicDto()
		{
			var json = "{\"attributes\": {\"1\": \"test\"}," +
						"\"path\": \"C:/merkmal/\"," +
						"\"uuid\": \"00000000-0000-0000-0000-000000000000\"," +
						"\"comment\": null," +
						"\"version\": 0," +
						"\"timestamp\": \"0001-01-01T00:00:00\"}";

			var deserialized = JsonConvert.DeserializeObject<InspectionPlanDtoBase>(
				json,
				new InspectionPlanDtoBaseConverter() );

			deserialized.GetType().Should().Be( typeof( InspectionPlanCharacteristicDto ) );
		}

		[Test]
		public void Parse_EmptyJson_ReturnsNull()
		{
			var deserialized = JsonConvert.DeserializeObject<InspectionPlanDtoBase>( string.Empty, new InspectionPlanDtoBaseConverter() );

			deserialized.Should().BeNull();
		}

		#endregion
	}
}