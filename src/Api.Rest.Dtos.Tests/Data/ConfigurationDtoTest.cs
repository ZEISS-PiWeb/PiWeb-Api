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
	using Zeiss.PiWeb.Api.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Zeiss.PiWeb.Api.Contracts.Attribute;

	#endregion

	[TestFixture]
	public class ConfigurationDtoTest
	{
		#region methods

		[Test]
		public void Test_Parse_Not_Existing_Attribute_ThrowsException()
		{
			// arrange
			var configuration = new ConfigurationDto();

			// act/ assert
			Assert.That( () => configuration.ParseValue( 1, "foo", new CatalogCollectionDto() ), Throws.ArgumentException );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		public void Test_Parse_Null_Value( AttributeParsingTestCase testCase )
		{
			// arrange
			const ushort attributeKey = 123;
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = attributeKey, Type = AttributeTypeDto.AlphaNumeric } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, testCase.AttributeValue, new CatalogCollectionDto() );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		public void Test_Parse_AlphaNumeric_Value( AttributeParsingTestCase testCase )
		{
			// arrange
			const ushort attributeKey = 123;
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = attributeKey, Type = AttributeTypeDto.AlphaNumeric } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, testCase.AttributeValue, new CatalogCollectionDto() );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		public void Test_Parse_Integer_Value( AttributeParsingTestCase testCase )
		{
			// arrange
			const ushort attributeKey = 123;
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = attributeKey, Type = AttributeTypeDto.Integer } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, testCase.AttributeValue, new CatalogCollectionDto() );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		public void Test_Parse_Double_Value( AttributeParsingTestCase testCase )
		{
			// arrange
			const ushort attributeKey = 123;
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = attributeKey, Type = AttributeTypeDto.Float } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, testCase.AttributeValue, new CatalogCollectionDto() );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		public void Test_Parse_Datetime_Value( AttributeParsingTestCase testCase )
		{
			// arrange
			const ushort attributeKey = 123;
			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = attributeKey, Type = AttributeTypeDto.DateTime } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, testCase.AttributeValue, new CatalogCollectionDto() );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( testCase.ExpectedResult ) );
		}

		[Test]
		public void Test_Parse_Invalid_DateTime_Value_ThrowsException()
		{
			// arrange
			const ushort attributeKey = 123;

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new AttributeDefinitionDto { Key = attributeKey, Type = AttributeTypeDto.DateTime } }
			};

			// act/ assert
			Assert.That( () => configuration.ParseValue( attributeKey, "foo", new CatalogCollectionDto() ), Throws.TypeOf<FormatException>() );
		}

		[Test]
		public void Test_Parse_CatalogEntry_When_CatalogCollection_Is_Invalid()
		{
			// arrange
			const ushort attributeKey = 123;

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attributeKey, Catalog = Guid.NewGuid() } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, "1", null );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( "1" ) );
		}

		[Test]
		public void Test_Parse_CatalogEntry_When_Catalog_Does_Not_Exist()
		{
			// arrange
			const ushort attributeKey = 123;

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attributeKey, Catalog = Guid.NewGuid() } }
			};

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, "1", new CatalogCollectionDto() );

			// assert
			Assert.That( parsedAttributeValue, Is.Null );
		}

		[Test]
		public void Test_Parse_CatalogEntry_When_Catalog_Is_Empty()
		{
			// arrange
			const ushort attributeKey = 123;

			var catalogUuid = Guid.NewGuid();
			var catalog = new CatalogDto { Uuid = catalogUuid };

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attributeKey, Catalog = catalogUuid } }
			};
			var catalogs = new CatalogCollectionDto( new[] { catalog } );

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, "1", catalogs );

			// assert
			Assert.That( parsedAttributeValue, Is.Null );
		}

		[Test]
		public void Test_Parse_CatalogEntry_When_Catalog_Does_Not_Contain_CatalogEntry()
		{
			// arrange
			const ushort attributeKey = 123;

			var catalogUuid = Guid.NewGuid();

			var defaultCatalogEntry = new CatalogEntryDto { Key = 0, Attributes = new[] { new Attribute( 1, "n.def." ) } };
			var catalog = new CatalogDto
			{
				Uuid = catalogUuid,
				CatalogEntries = new[] { defaultCatalogEntry }
			};

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attributeKey, Catalog = catalogUuid } }
			};
			var catalogs = new CatalogCollectionDto( new[] { catalog } );

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, "1", catalogs );

			// assert
			Assert.That( parsedAttributeValue, Is.Null );
		}

		[Test]
		public void Test_Parse_CatalogEntry_When_Catalog_Does_Contain_CatalogEntry()
		{
			// arrange
			const ushort attributeKey = 123;

			var catalogUuid = Guid.NewGuid();

			var defaultCatalogEntry = new CatalogEntryDto { Key = 0, Attributes = new[] { new Attribute( 1, "n.def." ) } };
			var catalogEntry1 = new CatalogEntryDto { Key = 1, Attributes = new[] { new Attribute( 1, "first entry" ) } };
			var catalog = new CatalogDto
			{
				Uuid = catalogUuid,
				CatalogEntries = new[] { defaultCatalogEntry, catalogEntry1 }
			};

			var configuration = new ConfigurationDto
			{
				PartAttributes = new[] { new CatalogAttributeDefinitionDto { Key = attributeKey, Catalog = catalogUuid } }
			};
			var catalogs = new CatalogCollectionDto( new[] { catalog } );

			// act
			var parsedAttributeValue = configuration.ParseValue( attributeKey, "1", catalogs );

			// assert
			Assert.That( parsedAttributeValue, Is.EqualTo( catalogEntry1 ) );
		}

		#endregion

		#region helper methods

		/// <remark>
		/// The null attribute is handled differently than other alpha numeric attributes because <see cref="Attribute.GetRawValue"/>
		/// for a null attribute, unlike other alpha numeric attributes, returns <see cref="Attribute.Value"/> rather than
		/// <see cref="Attribute._Value"/> which is <see langword="null" />.
		/// </remark>
		private static IEnumerable CreateNullAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: null
		yield return CreateTestCase( null,  null,   "null" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateAlphaNumericAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: "foo"
		yield return CreateTestCase( "foo", "foo","\"foo\"" );

		// value: "" (empty string)
		yield return CreateTestCase(    "",    "",  "\"\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateIntegerAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: 1 as string
		yield return CreateTestCase(         "1", 1, "\"1\"" );

		// value: 1 from attribute value
		yield return CreateTestCase( ValueFromAttribute( 1 ), 1, "1" );

			// @formatter:on — enable formatter after this line
		}

		private static string ValueFromAttribute( object rawValue )
		{
			return new Attribute( 1, rawValue ).Value;
		}

		private static IEnumerable CreateFloatAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: 1.0 as string
		yield return CreateTestCase(           "1.0",    1.0, "\"1.0\"" );

		// value: 1.0 from attribute value
		yield return CreateTestCase(   ValueFromAttribute( 1.0 ),    1.0, "1.0" );

		// value: -1.78 as string
		yield return CreateTestCase(         "-1.78",  -1.78, "\"-1.78\"" );

		// value: -1.78 from attribute value
		yield return CreateTestCase( ValueFromAttribute( -1.78 ),  -1.78, "-1.78" );

		// value: special double values like NaN and Infinity
		yield return CreateTestCase( "NaN",       double.NaN,              "\"NaN\"" );
		yield return CreateTestCase( "Infinity",  double.PositiveInfinity, "\"Infinity\"" );
		yield return CreateTestCase( "-Infinity", double.NegativeInfinity, "\"-Infinity\"" );

		// value: special double values like NaN and Infinity from attribute value
		yield return CreateTestCase( ValueFromAttribute( double.NaN ),              double.NaN,              "NaN" );
		yield return CreateTestCase( ValueFromAttribute( double.PositiveInfinity ), double.PositiveInfinity, "Infinity" );
		yield return CreateTestCase( ValueFromAttribute( double.NegativeInfinity ), double.NegativeInfinity, "-Infinity" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateDateTimeAttributeTestCases()
		{
		// @formatter:off — disable formatter after this line

		// value: date
		const string dateAsRoundtrip = "2015-03-09T19:12:00Z";

		var universalTime = DateTime.Parse( dateAsRoundtrip ).ToUniversalTime();
		var universalTimeAsString = XmlConvert.ToString( universalTime, XmlDateTimeSerializationMode.RoundtripKind );

		var localTime = DateTime.Parse( dateAsRoundtrip ).ToLocalTime();
		var localTimeAsString = XmlConvert.ToString( localTime, XmlDateTimeSerializationMode.RoundtripKind );

		var unspecifiedTime = new DateTime( universalTime.Ticks, DateTimeKind.Unspecified );
		var unspecifiedTimeAsString = XmlConvert.ToString( unspecifiedTime, XmlDateTimeSerializationMode.RoundtripKind );

		yield return CreateTestCase( dateAsRoundtrip,         universalTime,  $"\"{dateAsRoundtrip}\"" );
		yield return CreateTestCase( universalTimeAsString,   universalTime,  $"utc from string: {universalTimeAsString}" );
		yield return CreateTestCase( localTimeAsString,       localTime,      $"local from string: {localTimeAsString}" );
		yield return CreateTestCase( unspecifiedTimeAsString, unspecifiedTime,$"unspecified from string: {unspecifiedTimeAsString}" );

		// value: date from attribute value
		yield return CreateTestCase( ValueFromAttribute( universalTime ),   universalTime,   $"utc: {universalTimeAsString}" );
		yield return CreateTestCase( ValueFromAttribute( localTime ),       localTime,       $"local: {localTimeAsString}" );
		yield return CreateTestCase( ValueFromAttribute( unspecifiedTime ), unspecifiedTime, $"unspecified: {unspecifiedTimeAsString}" );

			// @formatter:on — enable formatter after this line
		}

		private static AttributeParsingTestCase CreateTestCase(
			string attributeValue,
			object expectedResult,
			string displayText = null )
		{
			return new AttributeParsingTestCase
			{
				AttributeValue = attributeValue,
				ExpectedResult = expectedResult,
				DisplayText = displayText
			};
		}

		#endregion

		#region class AttributeParsingTestCase

		public class AttributeParsingTestCase
		{
			#region properties

			public string DisplayText { get; set; }

			public string AttributeValue { get; set; }

			public object ExpectedResult { get; set; }

			#endregion

			#region methods

			public override string ToString()
			{
				return DisplayText;
			}

			#endregion
		}

		#endregion
	}
}