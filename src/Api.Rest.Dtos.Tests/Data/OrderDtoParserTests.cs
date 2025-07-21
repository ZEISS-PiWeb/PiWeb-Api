#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class OrderDtoParserTests
	{
		#region methods

		[Test]
		public void Parse_HappyPath_ReturnsOrderDto()
		{
			var result = OrderDtoParser.Parse( "4 asc", EntityDto.Measurement );

			result.Should().NotBeNull();
			result.Attribute.Should().Be( 4 );
			result.Direction.Should().Be( OrderDirectionDto.Asc );
			result.Entity.Should().Be( EntityDto.Measurement );
		}

		[Test]
		public void Parse_InvalidDirection_ReturnsOrderDtoWithDefaultDirection()
		{
			var result = OrderDtoParser.Parse( "4 invalidDirection", EntityDto.Measurement );

			result.Direction.Should().Be( OrderDirectionDto.Desc );
		}

		[Test]
		public void Parse_StringIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => OrderDtoParser.Parse( null!, EntityDto.Measurement ) );
		}

		[Test]
		public void Parse_StringIsEmpty_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => OrderDtoParser.Parse( "", EntityDto.Measurement ) );
		}

		[Test]
		public void Parse_StringIsInvalid_ExceptionThrown()
		{
			Assert.Throws<FormatException>( () => OrderDtoParser.Parse( "no order string", EntityDto.Measurement ) );
		}

		#endregion
	}
}