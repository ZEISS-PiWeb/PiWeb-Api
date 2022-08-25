#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System;
	using System.Collections;
	using System.Xml;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class AttributeDtoExtensionsTest
	{
		#region methods

		[Test]
		public void Test_GetAttributeValue_For_Not_Existing_Attribute()
		{
			// arrange
			var attribute = new AttributeDto( 1, "foo" );
			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetAttributeValue( 10 );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateCatalogEntryAttributeTestCases ) )]
		public void Test_GetAttributeValue( AttributeConversionTestCase testCase )
		{
			// arrange
			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetAttributeValue( testCase.Attribute.Key );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedStringValue ) );
		}

		[Test]
		public void Test_GetIntAttributeValue_For_Not_Existing_Attribute()
		{
			// arrange
			var attribute = new AttributeDto( 1, 1 );
			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetIntAttributeValue( 10 );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateCatalogEntryAttributeTestCases ) )]
		public void Test_GetIntAttributeValue( AttributeConversionTestCase testCase )
		{
			// arrange
			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetIntAttributeValue( testCase.Attribute.Key );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedIntValue ) );
		}

		[Test]
		public void Test_GetDoubleAttributeValue_For_Not_Existing_Attribute()
		{
			// arrange
			var attribute = new AttributeDto( 1, 1.23 );
			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetDoubleAttributeValue( 10 );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateCatalogEntryAttributeTestCases ) )]
		public void Test_GetDoubleAttributeValue( AttributeConversionTestCase testCase )
		{
			// arrange
			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetDoubleAttributeValue( testCase.Attribute.Key );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedDoubleValue ) );
		}

		[Test]
		public void Test_Test_GetDateAttributeValue_For_Not_Existing_Attribute()
		{
			// arrange
			var attribute = new AttributeDto( 1, DateTime.Now );
			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetDoubleAttributeValue( 10 );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateCatalogEntryAttributeTestCases ) )]
		public void Test_GetDateAttributeValue( AttributeConversionTestCase testCase )
		{
			// arrange
			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetDateAttributeValue( testCase.Attribute.Key );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedDateValue ) );
		}

		[Test]
		public void Test_GetRawAttributeValue_For_Not_Existing_Attribute()
		{
			// arrange
			var attribute = new AttributeDto( 1, "foo" );
			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetRawAttributeValue( 10, new ConfigurationDto(), new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		public void Test_GetRawAttributeValue_From_Null_Attribute( AttributeConversionTestCase testCase )
		{
			// arrange
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = testCase.Attribute.Key, Type = AttributeTypeDto.AlphaNumeric } }
			};

			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetRawAttributeValue( testCase.Attribute.Key, configuration, new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedStringValue ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		public void Test_GetRawAttributeValue_From_AlphaNumeric_Attribute( AttributeConversionTestCase testCase )
		{
			// arrange
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = testCase.Attribute.Key, Type = AttributeTypeDto.AlphaNumeric } }
			};

			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetRawAttributeValue( testCase.Attribute.Key, configuration, new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedStringValue ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		public void Test_GetRawAttributeValue_From_Integer_Attribute( AttributeConversionTestCase testCase )
		{
			// arrange
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = testCase.Attribute.Key, Type = AttributeTypeDto.Integer } }
			};

			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetRawAttributeValue( testCase.Attribute.Key, configuration, new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedRawValue ?? testCase.ExpectedIntValue ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		public void Test_GetRawAttributeValue_From_Float_Attribute( AttributeConversionTestCase testCase )
		{
			// arrange
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = testCase.Attribute.Key, Type = AttributeTypeDto.Float } }
			};

			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetRawAttributeValue( testCase.Attribute.Key, configuration, new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedRawValue ?? testCase.ExpectedDoubleValue ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		public void Test_GetRawAttributeValue_From_Datetime_Attribute( AttributeConversionTestCase testCase )
		{
			// arrange
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = testCase.Attribute.Key, Type = AttributeTypeDto.DateTime } }
			};

			var sut = new InspectionPlanPartDto { Attributes = new[] { testCase.Attribute } };

			// act
			var value = sut.GetRawAttributeValue( testCase.Attribute.Key, configuration, new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.EqualTo( testCase.ExpectedRawValue ?? testCase.ExpectedDateValue ) );
		}

		[Test]
		public void Test_GetRawAttributeValue_From_Catalog_Entry()
		{
			// arrange
			var catalogEntry = new CatalogEntryDto { Key = 2 };

			var attribute = new AttributeDto( 1, catalogEntry );
			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetRawAttributeValue( attribute.Key, new ConfigurationDto(), new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.EqualTo( catalogEntry ) );
		}

		[Test]
		public void Test_GetRawAttributeValue_From_Catalog_Entry_Key_When_Catalog_Does_Not_Exist()
		{
			// arrange
			var attribute = new AttributeDto( 1, "2" );

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attribute.Key, Catalog = Guid.NewGuid() } }
			};

			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetRawAttributeValue( attribute.Key, configuration, new CatalogCollectionDto() );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		public void Test_GetRawAttributeValue_From_Catalog_Entry_Key_When_Catalog_Entry_Does_Not_Exist()
		{
			// arrange
			var attribute = new AttributeDto( 1, "2" );
			var catalog = new CatalogDto { Uuid = Guid.NewGuid() };

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attribute.Key, Catalog = catalog.Uuid } }
			};
			var catalogs = new CatalogCollectionDto( new[] { catalog } );

			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetRawAttributeValue( attribute.Key, configuration, catalogs );

			// assert
			Assert.That( value, Is.Null );
		}

		[Test]
		public void Test_GetRawAttributeValue_From_Catalog_Entry_Key_When_Catalog_Entry_Exists()
		{
			// arrange
			var attribute = new AttributeDto( 1, "2" );

			var catalogEntry = new CatalogEntryDto { Key = 2 };
			var catalog = new CatalogDto
			{
				Uuid = Guid.NewGuid(),
				CatalogEntries = new[] { catalogEntry }
			};

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attribute.Key, Catalog = catalog.Uuid } }
			};
			var catalogs = new CatalogCollectionDto( new[] { catalog } );

			var sut = new InspectionPlanPartDto { Attributes = new[] { attribute } };

			// act
			var value = sut.GetRawAttributeValue( attribute.Key, configuration, catalogs );

			// assert
			Assert.That( value, Is.EqualTo( catalogEntry ) );
		}

		[Test]
		public void Test_RemoveAttribute_Removes_Existing_Attribute()
		{
			// arrange
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
					new AttributeDto( 2, "Test2" ),
					new AttributeDto( 3, "Test3" ),
					new AttributeDto( 4, "Test4" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 3, "Test3" ),
				new AttributeDto( 4, "Test4" ),
			};

			// act
			sut.RemoveAttribute( 2 );

			// assert
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void Test_RemoveAttribute_Skips_Not_Existing_Attribute()
		{
			// arrange
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
					new AttributeDto( 2, "Test2" ),
					new AttributeDto( 3, "Test3" ),
					new AttributeDto( 4, "Test4" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 2, "Test2" ),
				new AttributeDto( 3, "Test3" ),
				new AttributeDto( 4, "Test4" ),
			};

			// act
			sut.RemoveAttribute( 22 );

			// assert
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void Test_InsertNullAttribute_Inserts_Attribute_Without_Value()
		{
			// arrange
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 22, null ),
			};

			// act
			sut.InsertNullAttribute( 22 );

			// assert
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void Test_SetAttribute_Sets_Attribute_Value()
		{
			// arrange
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 22, "New Value" ),
			};

			// act
			sut.SetAttribute( new AttributeDto( 22, "New Value" ) );

			// assert
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		[Test]
		public void Test_SetAttributeValue_Sets_Attribute_Value()
		{
			// arrange
			var sut = new InspectionPlanPartDto
			{
				Attributes = new[]
				{
					new AttributeDto( 1, "Test1" ),
				}
			};

			var expected = new[]
			{
				new AttributeDto( 1, "Test1" ),
				new AttributeDto( 22, "New Value" ),
			};

			// act
			sut.SetAttributeValue( 22, "New Value" );

			// assert
			Assert.That( sut.Attributes, Is.EquivalentTo( expected ) );
		}

		#endregion

		#region helper methods

		/// <remark>
		/// The null attribute is handled differently than other alpha numeric attributes because <see cref="AttributeDto.GetRawValue"/>
		/// for a null attribute, unlike other alpha numeric attributes, returns <see cref="AttributeDto.Value"/> rather than
		/// <see cref="AttributeDto._Value"/> which is <see langword="null" />.
		/// </remark>
		private static IEnumerable CreateNullAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: null
		yield return CreateTestCase( null,  null, null, "",    null, null,   "null" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateAlphaNumericAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: "foobar"
		yield return CreateTestCase( "foo", null, null, "foo", null, null,"\"foo\"" );

		// value: "" (empty string)
		yield return CreateTestCase( "", null, null,    "" ,   null, null,   "\"\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateIntegerAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: 1 as integer, short and string
		yield return CreateTestCase( 1,        1,  1.0,    "1", null, 1, "1" );
		yield return CreateTestCase( (short)1, 1,  1.0,    "1", null, 1, "1 (short)" );
		yield return CreateTestCase( "1",      1,  1.0,    "1", null, null, "\"1\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateFloatAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: 1.0 as double and string
		yield return CreateTestCase( 1.0,      null,  1.0,     "1", null,     1, "1.0" );
		yield return CreateTestCase( "1.0",    null,  1.0,   "1.0", null,  null, "\"1.0\"" );

		// value: -1.78 as double and string
		yield return CreateTestCase(  -1.78,  null, -1.78, "-1.78", null,   -1.78, "-1.78" );
		yield return CreateTestCase( "-1.78", null, -1.78, "-1.78", null,    null, "\"-1.78\"" );

		// value: special double values like NaN and Infinity
		yield return CreateTestCase( double.NaN,              null,               double.NaN,       "NaN", null, double.NaN,              "NaN" );
		yield return CreateTestCase( double.PositiveInfinity, null,  double.PositiveInfinity,  "Infinity", null, double.PositiveInfinity, "Infinity" );
		yield return CreateTestCase( double.NegativeInfinity, null,  double.NegativeInfinity, "-Infinity", null, double.NegativeInfinity, "-Infinity" );
		yield return CreateTestCase( "NaN",                   null,               double.NaN,       "NaN", null, null,                    "\"NaN\"" );
		yield return CreateTestCase( "Infinity",              null,  double.PositiveInfinity,  "Infinity", null, null,                    "\"Infinity\"" );
		yield return CreateTestCase( "-Infinity",             null,  double.NegativeInfinity, "-Infinity", null, null,                    "\"-Infinity\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateDateTimeAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: date
		var universalTime = DateTime.Parse( "2015-03-09T19:12:00Z" ).ToUniversalTime();
		var universalTimeAsString = XmlConvert.ToString( universalTime, XmlDateTimeSerializationMode.RoundtripKind );

		var unspecifiedTime = new DateTime( universalTime.Ticks, DateTimeKind.Unspecified );
		var unspecifiedTimeAsString = XmlConvert.ToString( unspecifiedTime, XmlDateTimeSerializationMode.RoundtripKind );

		yield return CreateTestCase( universalTimeAsString, null, null, universalTimeAsString,   universalTime, null,           $"\"{universalTimeAsString}\"" );
		yield return CreateTestCase( universalTime,         null, null, universalTimeAsString,   universalTime, universalTime,  $"utc: {universalTimeAsString}" );
		yield return CreateTestCase( unspecifiedTime,       null, null, unspecifiedTimeAsString, universalTime, unspecifiedTime, $"unspecified: {unspecifiedTimeAsString}" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateCatalogEntryAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: date
		var catalogEntry = new CatalogEntryDto{ Key = 4 };

		yield return CreateTestCase( catalogEntry, 4, 4,       "4", null,catalogEntry, "catalog entry" );

			// @formatter:on — enable formatter after this line
		}

		private static AttributeConversionTestCase CreateTestCase(
			object attributeValue,
			int? expectedIntValue, double? expectedDoubleValue, string expectedStringValue, DateTime? expectedDateValue,
			object expectedRawValue,
			string displayText = null )
		{
			return new AttributeConversionTestCase
			{
				Attribute = new AttributeDto( 1, attributeValue ),
				ExpectedIntValue = expectedIntValue,
				ExpectedDoubleValue = expectedDoubleValue,
				ExpectedStringValue = expectedStringValue,
				ExpectedDateValue = expectedDateValue,
				ExpectedRawValue = expectedRawValue,
				DisplayText = displayText
			};
		}

		#endregion
	}
}