#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System;
	using System.Collections;
	using AutoFixture;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class AttributeComparerTest
	{
		#region members

		private static readonly Fixture Fixture = new Fixture();

		#endregion

		#region methods

		[Test]
		public void Test_Compare_Attributes_With_Different_Keys()
		{
			// arrange
			var xAttribute = new AttributeDto( Fixture.Create<ushort>(), "foo" );
			var yAttribute = new AttributeDto( Fixture.Create<ushort>(), "foo" );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.False );

			Assert.That( xAttribute == yAttribute, Is.False );
			Assert.That( xAttribute != yAttribute, Is.True );
		}

		[Test]
		public void Test_Compare_Attribute_With_Value_And_Attribute_With_RawValue()
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, 42 );
			var yAttribute = new AttributeDto( attributeKey, "foo" );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.False );

			Assert.That( xAttribute == yAttribute, Is.False );
			Assert.That( xAttribute != yAttribute, Is.True );
		}

		[Test]
		public void Test_Compare_Attributes_With_Different_RawValue_Types()
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, 42 );
			var yAttribute = new AttributeDto( attributeKey, 1.23 );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.False );

			Assert.That( xAttribute == yAttribute, Is.False );
			Assert.That( xAttribute != yAttribute, Is.True );
		}

		[Test]
		public void Test_Compare_Attributes_By_ReferenceEquality_Of_RawValue()
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var attributeValue = new CatalogEntryDto { Key = Fixture.Create<short>() };

			var xAttribute = new AttributeDto( attributeKey, attributeValue );
			var yAttribute = new AttributeDto( attributeKey, attributeValue );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.True );

			Assert.That( xAttribute == yAttribute, Is.True );
			Assert.That( xAttribute != yAttribute, Is.False );
		}

		[Test]
		[TestCase( null, null, true )]
		[TestCase( "", "", true )]
		[TestCase( " ", " ", true )]
		[TestCase( "\t", "\t", true )]
		[TestCase( "Hello World", "Hello World", true )]
		[TestCase( null, "", true )]
		[TestCase( null, " ", false )]
		[TestCase( "", " ",  false )]
		[TestCase( "Hello World", "",  false )]
		[TestCase( "Hello World", " ",  false )]
		[TestCase( "Hello World", "\t",  false )]
		[TestCase( "Hello World", "hello World",  false )]
		public void Test_Compare_String_Attributes( string x, string y, bool expectedResult )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.EqualTo( expectedResult ) );

			Assert.That( xAttribute == yAttribute, Is.EqualTo( expectedResult ) );
			Assert.That( xAttribute != yAttribute, Is.Not.EqualTo( expectedResult ) );
		}

		[Test]
		[TestCase( 1, 1,  true )]
		[TestCase( 1, 2,  false )]
		[TestCase( -1, -1,  true )]
		[TestCase( -1, -2,  false )]
		[TestCase( -1, 1,  false )]
		[TestCase( 0, 0,  true )]
		[TestCase( int.MinValue, int.MinValue,  true )]
		[TestCase( int.MaxValue, int.MaxValue,  true )]
		public void Test_Compare_Integer_Attributes( int x, int y, bool expectedResult )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.EqualTo( expectedResult ) );

			Assert.That( xAttribute == yAttribute, Is.EqualTo( expectedResult ) );
			Assert.That( xAttribute != yAttribute, Is.Not.EqualTo( expectedResult ) );
		}

		[Test]
		[TestCase( 1, 1,  true )]
		[TestCase( 1, 2,  false )]
		[TestCase( -1, -1,  true )]
		[TestCase( -1, -2,  false )]
		[TestCase( -1, 1,  false )]
		[TestCase( 0, 0,  true )]
		[TestCase( short.MinValue, short.MinValue,  true )]
		[TestCase( short.MaxValue, short.MaxValue,  true )]
		public void Test_Compare_Short_Attributes( short x, short y, bool expectedResult )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.EqualTo( expectedResult ) );

			Assert.That( xAttribute == yAttribute, Is.EqualTo( expectedResult ) );
			Assert.That( xAttribute != yAttribute, Is.Not.EqualTo( expectedResult ) );
		}

		[Test]
		[TestCase( 1, 1,  true )]
		[TestCase( 1, 2,  false )]
		[TestCase( 0.1, 0.1,  true )]
		[TestCase( 0.1, 0.2,  false )]
		[TestCase( -1, -1,  true )]
		[TestCase( -1, -2,  false )]
		[TestCase( -0.1, -0.1,  true )]
		[TestCase( -0.1, -0.2,  false )]
		[TestCase( -0.1, 0.1,  false )]
		[TestCase( 0.0, 0.0,  true )]
		[TestCase( double.NaN, 1,  false )]
		[TestCase( double.NaN, double.NaN,  false )]
		[TestCase( double.PositiveInfinity, 1,  false )]
		[TestCase( double.NegativeInfinity, 1,  false )]
		[TestCase( double.PositiveInfinity, double.PositiveInfinity,  true )]
		[TestCase( double.NegativeInfinity, double.NegativeInfinity,  true )]
		[TestCase( double.PositiveInfinity, double.NegativeInfinity,  false )]
		[TestCase( double.NegativeInfinity, double.PositiveInfinity,  false )]
		public void Test_Compare_Double_Attributes( double x, double y, bool expectedResult )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.EqualTo( expectedResult ) );

			Assert.That( xAttribute == yAttribute, Is.EqualTo( expectedResult ) );
			Assert.That( xAttribute != yAttribute, Is.Not.EqualTo( expectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateCompareDateTimeAttributesTestCases ) )]
		public void Test_Compare_DateTime_Attributes( AttributeCompareTestCase testCase )
		{
			// arrange

			// act, assert
			Assert.That( Equals( testCase.X, testCase.Y ), Is.EqualTo( testCase.ExpectedResult ) );

			Assert.That( testCase.X == testCase.Y, Is.EqualTo( testCase.ExpectedResult ) );
			Assert.That( testCase.X != testCase.Y, Is.Not.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateCompareCatalogAttributesTestCases ) )]
		public void Test_Compare_CatalogEntry_Attributes( AttributeCompareTestCase testCase )
		{
			// arrange

			// act, assert
			Assert.That( Equals( testCase.X, testCase.Y ), Is.EqualTo( testCase.ExpectedResult ) );

			Assert.That( testCase.X == testCase.Y, Is.EqualTo( testCase.ExpectedResult ) );
			Assert.That( testCase.X != testCase.Y, Is.Not.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		public void Test_GetHashCode_IsSame_When_Key_And_Value_Are_Same()
		{
			// arrange
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "foo" );

			// act
			var hashCode1 = attribute1.GetHashCode();
			var hashCode2 = attribute2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsSame_When_RawValues_Are_Different_But_Return_Same_Value()
		{
			// arrange
			var attribute1 = new AttributeDto( 1, 1.23 );
			var attribute2 = new AttributeDto( 1, "1.23" );

			// act
			var hashCode1 = attribute1.GetHashCode();
			var hashCode2 = attribute2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Keys_Are_Different()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 2, "foo" );

			// act
			var hashCode1 = attribute1.GetHashCode();
			var hashCode2 = attribute2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Values_Are_Different()
		{
			var attribute1 = new AttributeDto( 1, "foo" );
			var attribute2 = new AttributeDto( 1, "bar" );

			// act
			var hashCode1 = attribute1.GetHashCode();
			var hashCode2 = attribute2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		#endregion

		#region helper methods

		private static IEnumerable CreateCompareDateTimeAttributesTestCases()
		{
			var attributeKey = Fixture.Create<ushort>();
			var olderTime = new DateTime( 1985, 4, 16, 0, 0, 0, DateTimeKind.Utc );
			var newerTime = new DateTime( 1988, 4, 24, 0, 0, 0, DateTimeKind.Utc );

			yield return new AttributeCompareTestCase { X = new AttributeDto( attributeKey, olderTime ), Y = new AttributeDto( attributeKey, olderTime ), ExpectedResult = true };
			yield return new AttributeCompareTestCase { X = new AttributeDto( attributeKey, olderTime ), Y = new AttributeDto( attributeKey, newerTime ), ExpectedResult = false };
			yield return new AttributeCompareTestCase { X = new AttributeDto( attributeKey, newerTime ), Y = new AttributeDto( attributeKey, olderTime ), ExpectedResult = false };
		}

		private static IEnumerable CreateCompareCatalogAttributesTestCases()
		{
			var attributeKey = Fixture.Create<ushort>();

			yield return new AttributeCompareTestCase
			{
				X = new AttributeDto( attributeKey, new CatalogEntryDto { Key = 1 } ),
				Y = new AttributeDto( attributeKey, new CatalogEntryDto { Key = 2 } ),
				ExpectedResult = false
			};

			const short catalogEntryKey = 77;
			yield return new AttributeCompareTestCase
			{
				X = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey } ),
				Y = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey } ),
				ExpectedResult = true
			};

			// When the attribute encapsulates a CatalogEntryDto, we just compare the catalog entry key
			var catalogEntryAttribute1 = new AttributeDto( 12, 1 );
			var catalogEntryAttribute2 = new AttributeDto( 12, 2 );
			yield return new AttributeCompareTestCase
			{
				X = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey, Attributes = new[] { catalogEntryAttribute1 } } ),
				Y = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey, Attributes = new[] { catalogEntryAttribute2 } } ),
				ExpectedResult = true
			};
			yield return new AttributeCompareTestCase
			{
				X = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey, Attributes = new[] { catalogEntryAttribute1, catalogEntryAttribute2 } } ),
				Y = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey, Attributes = new[] { catalogEntryAttribute2 } } ),
				ExpectedResult = true
			};
			yield return new AttributeCompareTestCase
			{
				X = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey, Attributes = new[] { catalogEntryAttribute1, catalogEntryAttribute2 } } ),
				Y = new AttributeDto( attributeKey, new CatalogEntryDto { Key = catalogEntryKey, Attributes = new[] { catalogEntryAttribute1, catalogEntryAttribute2 } } ),
				ExpectedResult = true
			};
		}

		#endregion

		#region class AttributeCompareTestCase

		public class AttributeCompareTestCase
		{
			#region properties

			public AttributeDto X { get; set; }

			public AttributeDto Y { get; set; }

			public bool ExpectedResult { get; set; }

			#endregion

			#region methods

			public override string ToString()
			{
				return $"{PrintAttribute( X )}, {PrintAttribute( Y )}";

				string PrintAttribute( AttributeDto attribute )
				{
					if( attribute.RawValue is CatalogEntryDto catalogEntry )
						return $"Catalog entry {catalogEntry.Key}: {catalogEntry}";

					return attribute.Value ?? "null";
				}
			}

			#endregion
		}

		#endregion
	}
}