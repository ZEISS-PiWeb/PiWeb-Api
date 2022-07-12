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
		public void Test_CompareAttributesWithDifferentKeys()
		{
			// arrange
			var xAttribute = new AttributeDto( Fixture.Create<ushort>(), Fixture.Create<string>() );
			var yAttribute = new AttributeDto( Fixture.Create<ushort>(), Fixture.Create<string>() );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.False );
		}

		[Test]
		public void Test_CompareAttributesWithDifferentTypes()
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, Fixture.Create<string>() );
			var yAttribute = new AttributeDto( attributeKey, Fixture.Create<double>() );

			// act, assert
			Assert.That( Equals( xAttribute, yAttribute ), Is.False );
		}

		[Test]
		[TestCaseSource( nameof( CreateCompareAttributesByReferenceEqualityTestCases ) )]
		public void Test_CompareAttributesByReferenceEquality( AttributeCompareTestCase testCase )
		{
			// arrange

			// act, assert
			Assert.That( Equals( testCase.X, testCase.Y ), Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCase( null, null, ExpectedResult = true )]
		[TestCase( "", "", ExpectedResult = true )]
		[TestCase( " ", " ", ExpectedResult = true )]
		[TestCase( "\t", "\t", ExpectedResult = true )]
		[TestCase( "Hello World", "Hello World", ExpectedResult = true )]
		[TestCase( null, "", ExpectedResult = true )]
		[TestCase( null, " ", ExpectedResult = false )]
		[TestCase( "", " ", ExpectedResult = false )]
		[TestCase( "Hello World", "", ExpectedResult = false )]
		[TestCase( "Hello World", " ", ExpectedResult = false )]
		[TestCase( "Hello World", "\t", ExpectedResult = false )]
		[TestCase( "Hello World", "hello World", ExpectedResult = false )]
		public bool Test_CompareStringAttributes( string x, string y )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			return Equals( xAttribute, yAttribute );
		}

		[Test]
		[TestCase( 1, 1, ExpectedResult = true )]
		[TestCase( 1, 2, ExpectedResult = false )]
		[TestCase( -1, -1, ExpectedResult = true )]
		[TestCase( -1, -2, ExpectedResult = false )]
		[TestCase( -1, 1, ExpectedResult = false )]
		[TestCase( 0, 0, ExpectedResult = true )]
		[TestCase( int.MinValue, int.MinValue, ExpectedResult = true )]
		[TestCase( int.MaxValue, int.MaxValue, ExpectedResult = true )]
		public bool Test_CompareIntegerAttributes( int x, int y )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			return Equals( xAttribute, yAttribute );
		}

		[Test]
		[TestCase( 1, 1, ExpectedResult = true )]
		[TestCase( 1, 2, ExpectedResult = false )]
		[TestCase( 0.1, 0.1, ExpectedResult = true )]
		[TestCase( 0.1, 0.2, ExpectedResult = false )]
		[TestCase( -1, -1, ExpectedResult = true )]
		[TestCase( -1, -2, ExpectedResult = false )]
		[TestCase( -0.1, -0.1, ExpectedResult = true )]
		[TestCase( -0.1, -0.2, ExpectedResult = false )]
		[TestCase( -0.1, 0.1, ExpectedResult = false )]
		[TestCase( 0.0, 0.0, ExpectedResult = true )]
		[TestCase( double.NaN, 1, ExpectedResult = false )]
		[TestCase( double.NaN, double.NaN, ExpectedResult = false )]
		[TestCase( double.PositiveInfinity, 1, ExpectedResult = false )]
		[TestCase( double.NegativeInfinity, 1, ExpectedResult = false )]
		[TestCase( double.PositiveInfinity, double.PositiveInfinity, ExpectedResult = true )]
		[TestCase( double.NegativeInfinity, double.NegativeInfinity, ExpectedResult = true )]
		[TestCase( double.PositiveInfinity, double.NegativeInfinity, ExpectedResult = false )]
		[TestCase( double.NegativeInfinity, double.PositiveInfinity, ExpectedResult = false )]
		public bool Test_CompareDoubleAttributes( double x, double y )
		{
			// arrange
			var attributeKey = Fixture.Create<ushort>();
			var xAttribute = new AttributeDto( attributeKey, x );
			var yAttribute = new AttributeDto( attributeKey, y );

			// act, assert
			return Equals( xAttribute, yAttribute );
		}

		[Test]
		[TestCaseSource( nameof( CreateCompareDateTimeAttributesTestCases ) )]
		public void Test_CompareDateTimeAttributes( AttributeCompareTestCase testCase )
		{
			// arrange

			// act, assert
			Assert.That( Equals( testCase.X, testCase.Y ), Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateCompareCatalogAttributesTestCases ) )]
		public void Test_CompareCatalogEntryAttributes( AttributeCompareTestCase testCase )
		{
			// arrange

			// act, assert
			Assert.That( Equals( testCase.X, testCase.Y ), Is.EqualTo( testCase.ExpectedResult ) );
		}

		#endregion

		#region helper methods

		private static IEnumerable CreateCompareAttributesByReferenceEqualityTestCases()
		{
			var attribute = new AttributeDto( Fixture.Create<ushort>(), Fixture.Create<string>() );
			yield return new AttributeCompareTestCase { X = attribute, Y = attribute, ExpectedResult = true };
		}

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

			var catalogEntryKey = (short)77;
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