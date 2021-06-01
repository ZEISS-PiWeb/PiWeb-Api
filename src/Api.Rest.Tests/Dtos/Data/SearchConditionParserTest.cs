#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.Dtos.Data
{
	#region usings

	using System;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class SearchConditionParserTest
	{
		#region methods

		[Test]
		public void Parse_ConditionContainsBrackets_Success()
		{
			var expected = new GenericSearchAttributeConditionDto()
			{
				Attribute = 123,
				Operation = OperationDto.Equal,
				Value = "condition[with[b[]rackets"
			};

			var actual = SearchConditionParser.Parse( "123=[condition[with[b[]rackets]" );

			actual.Should().BeEquivalentTo( expected );
		}

		[Test]
		public void Parse_MultiConditionContainsBrackets_Success()
		{
			var expected = new GenericSearchAndDto()
			{
				Conditions = new GenericSearchConditionDto[]
				{
					new GenericSearchAttributeConditionDto()
					{
						Attribute = 123,
						Operation = OperationDto.Equal,
						Value = "condition[]with[]b[ack]ets"
					},
					new GenericSearchAttributeConditionDto()
					{
						Attribute = 124,
						Operation = OperationDto.In,
						Value = "[100;-1;23];[2;-12;40];[-22;33;44]"
					}
				}
			};

			var actual = (GenericSearchAndDto)SearchConditionParser.Parse( "123=[condition[]with[]b[ack]ets]+124In[[100;-1;23];[2;-12;40];[-22;33;44]]" );

			// ReSharper disable once CoVariantArrayConversion
			actual.Conditions.Should().BeEquivalentTo( expected.Conditions );
		}

		[Test]
		public void Parse_MissingClosingBracket_ThrowsException()
		{
			Action action = () => SearchConditionParser.Parse( "123=[value" );
			action.Should().Throw<InvalidOperationException>().Where( e => e.Message.Contains( "']'" ) );
		}

		[Test]
		public void Parse_MissingOpeningBracket_ThrowsException()
		{
			Action action = () => SearchConditionParser.Parse( "123=value]" );
			action.Should().Throw<InvalidOperationException>().Where( e => e.Message.Contains( "[]" ) );
		}

		[Test]
		[TestCase( "", true )]
		[TestCase( "123=[]", true )]
		[TestCase( "123=]", false )]
		[TestCase( "123=[", false )]
		[TestCase( "123=condition", false )]
		[TestCase( "123=[condition", false )]
		[TestCase( "123=condition]", false )]
		[TestCase( "123=[condit]ion", false )]
		[TestCase( "123=[condit]ion]", true )]
		[TestCase( "123=[condition[]]", true )]
		[TestCase( "123=[]condition]", true )]
		[TestCase( "123=[con[]dition]", true )]
		[TestCase( "123=[con[]dition]+124=[con[]dition]", true )]
		[TestCase( "123=[con[]dition]+124=[con[]dition", false )]
		public void Parse_SearchConditionString_ReturnsExpectedResult( string condition, bool result )
		{
			if( result )
				Assert.DoesNotThrow( () => SearchConditionParser.Parse( condition ) );
			else
				Assert.Throws<InvalidOperationException>( () => SearchConditionParser.Parse( condition ) );
		}

		[Test]
		public void GenericConditionToString_SimpleCondition_ReturnsCorrectString()
		{
			var condition = new GenericSearchAttributeConditionDto
			{
				Attribute = 123,
				Operation = OperationDto.Equal,
				Value = "condition[with][b[]rackets"
			};

			const string expected = "123=[condition[with][b[]rackets]";

			var actual = SearchConditionParser.GenericConditionToString( condition );

			actual.Should().Be( expected );
		}

		[Test]
		public void GenericConditionToString_NestedCondition_ReturnsCorrectString()
		{
			var condition = new GenericSearchAndDto
			{
				Conditions = new GenericSearchConditionDto[]
				{
					new GenericSearchAttributeConditionDto
					{
						Attribute = 123,
						Operation = OperationDto.Equal,
						Value = "condition[]with[]b[ack]ets"
					},
					new GenericSearchAttributeConditionDto
					{
						Attribute = 124,
						Operation = OperationDto.In,
						Value = "[100;-1;23];[2;-12;40];[-22;33;44]"
					}
				}
			};

			const string expected = "123=[condition[]with[]b[ack]ets]+124In[[100;-1;23];[2;-12;40];[-22;33;44]]";

			var actual = SearchConditionParser.GenericConditionToString( condition );

			actual.Should().Be( expected );
		}

		[Test]
		public void CanParse_ValidCondition_ReturnsTrue()
		{
			var actual = SearchConditionParser.CanParse( "123=[condition[with][b[]rackets]" );
			actual.Should().BeTrue();
		}

		[Test]
		public void CanParse_InvalidCondition_ReturnsFalse()
		{
			var actual = SearchConditionParser.CanParse( "123=[condition[with[]b[]rackets" );
			actual.Should().BeFalse();
		}

		[Test]
		public void TryParse_ValidCondition_ReturnsTrue()
		{
			var expected = new GenericSearchAttributeConditionDto()
			{
				Attribute = 123,
				Operation = OperationDto.Equal,
				Value = "condition[with][b[]rackets"
			};

			var result = SearchConditionParser.TryParse( "123=[condition[with][b[]rackets]", out var condition );
			result.Should().BeTrue();
			condition.Should().BeEquivalentTo( expected );
		}

		[Test]
		public void TryParse_InvalidCondition_ReturnsFalse()
		{
			var actual = SearchConditionParser.TryParse( "123=[condition[with[b[rackets", out var condition );
			actual.Should().BeFalse();
			condition.Should().BeNull();
		}

		#endregion
	}
}