namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data

{
	#region usings

	using System;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class AttributeDtoTest
	{
		#region methods

		[Test]
		public void GetDoubleValue_HappyPath_ValueCorrect()
		{
			var attribute = new AttributeDto( 1, "1.23" );

			var actualResult = attribute.GetDoubleValue();

			actualResult.Should().BeOfType( typeof( double ) );
			actualResult.Should().Be( 1.23 );
		}

		[Test]
		public void GetDoubleValue_NoDouble_ReturnsNull()
		{
			var attribute = new AttributeDto( 1, "foo" );

			var actualResult = attribute.GetDoubleValue();

			actualResult.Should().BeNull();
		}

		[Test]
		public void GetIntValue_HappyPath_ValueCorrect()
		{
			var attribute = new AttributeDto( 1, "7" );

			var actualResult = attribute.GetIntValue();

			actualResult.Should().BeOfType( typeof( int ) );
			actualResult.Should().Be( 7 );
		}

		[Test]
		public void GetIntValue_NoDouble_ReturnsNull()
		{
			var attribute = new AttributeDto( 1, "foo" );

			var actualResult = attribute.GetIntValue();

			actualResult.Should().BeNull();
		}

		[Test]
		public void GetDateValue_HappyPath_ValueCorrect()
		{
			var attribute = new AttributeDto( 1, "2015-03-09T19:12:00Z" );

			var actualResult = attribute.GetDateValue();

			var expected = DateTime.Parse( "2015-03-09T19:12:00Z" ).ToUniversalTime();
			actualResult.Should().Be( expected );
		}

		[Test]
		public void GetDateValue_NoDouble_ReturnsNull()
		{
			var attribute = new AttributeDto( 1, "foo" );

			var actualResult = attribute.GetDateValue();

			actualResult.Should().BeNull();
		}


		[Test]
		public void Equals_DifferentKey_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 2, "bar" );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeFalse();
		}


		[Test]
		public void Equals_SameValue_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "foo" );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void Equals_DifferentValues_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "bar" );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeFalse();
		}

		[Test]
		public void Equals_SameRawValue_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, 42 );
			var attribute2 = new AttributeDto( 1, 42 );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void Equals_DifferentRawValue_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, 42 );
			var attribute2 = new AttributeDto( 1, 7 );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeFalse();
		}

		[Test]
		public void Equals_ValueAndRawValue_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, 42 );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeFalse();
		}

		[Test]
		public void Equals_DifferentDataTypes_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, DateTime.Now );
			var attribute2 = new AttributeDto( 1, 42 );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeFalse();
		}

		[Test]
		public void Equals_ReferenceEquals_ReturnsTrue()
		{
			const string value = "foo";

			var attribute1 = new AttributeDto( 1, (object)value );
			var attribute2 = new AttributeDto( 1, (object)value );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void Equals_CatalogEntries_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, new CatalogEntryDto { Key = 7 } );
			var attribute2 = new AttributeDto( 1, new CatalogEntryDto { Key = 7 } );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void Equals_Short_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, (short)7 );
			var attribute2 = new AttributeDto( 1, (short)7 );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void Equals_Double_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, 7.77 );
			var attribute2 = new AttributeDto( 1, 7.77 );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void Equals_DateTime_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, DateTime.Parse( "2015-03-09T19:12:00Z" ).ToUniversalTime() );
			var attribute2 = new AttributeDto( 1, DateTime.Parse( "2015-03-09T19:12:00Z" ).ToUniversalTime() );

			var result = attribute1.Equals( attribute2 );

			result.Should().BeTrue();
		}

		[Test]
		public void EqualOperator_SameValue_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "foo" );

			var result = attribute1 == attribute2;

			result.Should().BeTrue();
		}

		[Test]
		public void EqualOperator_DifferentValue_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "bar" );

			var result = attribute1 == attribute2;

			result.Should().BeFalse();
		}

		[Test]
		public void NotEqualOperator_SameValue_ReturnsFalse()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "foo" );

			var result = attribute1 != attribute2;

			result.Should().BeFalse();
		}

		[Test]
		public void NotEqualOperator_DifferentValue_ReturnsTrue()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "bar" );

			var result = attribute1 != attribute2;

			result.Should().BeTrue();
		}

		[Test]
		public void GetHashCode_SameValue_SameHashes()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "foo" );

			var hash1 = attribute1.GetHashCode();
			var hash2 = attribute2.GetHashCode();

			hash1.Should().Be( hash2 );
		}

		[Test]
		public void GetHashCode_DifferentValue_DifferentHashes()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "bar" );

			var hash1 = attribute1.GetHashCode();
			var hash2 = attribute2.GetHashCode();

			hash1.Should().NotBe( hash2 );
		}

		[Test]
		public void GetString_ValidAttribute_CorrectString()
		{
			var attribute = new AttributeDto( 1, "foo" );

			attribute.ToString().Should().Be( "K1: foo" );
		}

		#endregion
	}
}