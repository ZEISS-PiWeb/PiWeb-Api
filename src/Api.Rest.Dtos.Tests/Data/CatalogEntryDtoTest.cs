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
	using System.Globalization;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;
	using Attribute = Zeiss.PiWeb.Api.Core.Attribute;

	#endregion

	[TestFixture]
	public class CatalogEntryDtoTest
	{
		#region methods

		[Test]
		public void Test_Description_Of_Empty_CatalogEntry()
		{
			// arrange
			var catalogEntry = new CatalogEntryDto();

			// act
			var description = catalogEntry.ToString( CultureInfo.InvariantCulture );

			// assert
			Assert.That( description, Is.Empty );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		public void Test_Description_Of_CatalogEntry_With_One_Attribute( CatalogEntryDescriptionTestCase testCase )
		{
			// arrange
			var catalogEntry = new CatalogEntryDto { Attributes = new[] { new Attribute( 1, testCase.AttributeValue ) } };

			// act
			var description = catalogEntry.ToString( CultureInfo.InvariantCulture );

			// assert
			Assert.That( description, Is.EqualTo( testCase.ExpectedDescription ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateNullAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		public void Test_Description_Of_CatalogEntry_With_Duplicated_Attribute_Values( CatalogEntryDescriptionTestCase testCase )
		{
			// arrange
			var catalogEntry = new CatalogEntryDto
			{
				Attributes = new[]
				{
					new Attribute( 1, testCase.AttributeValue ),
					new Attribute( 2, testCase.AttributeValue )
				}
			};

			// act
			var description = catalogEntry.ToString( CultureInfo.InvariantCulture );

			// assert
			Assert.That( description, Is.EqualTo( testCase.ExpectedDescription ) );
		}

		[Test]
		[TestCaseSource( nameof( CreateAlphaNumericAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateIntegerAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateFloatAttributeTestCases ) )]
		[TestCaseSource( nameof( CreateDateTimeAttributeTestCases ) )]
		public void Test_Description_Of_CatalogEntry_With_Null_Attribute_And_One_Other_Attribute( CatalogEntryDescriptionTestCase testCase )
		{
			// arrange
			var catalogEntry = new CatalogEntryDto
			{
				Attributes = new[]
				{
					new Attribute( 1, null ),
					new Attribute( 2, testCase.AttributeValue )
				}
			};

			// act
			var description = catalogEntry.ToString( CultureInfo.InvariantCulture );

			// assert
			Assert.That( description, Is.EqualTo( testCase.ExpectedDescription ) );
		}

		[Test]
		public void Test_Description_Of_CatalogEntry_With_Not_Duplicated_Not_Null_Attributes()
		{
			// arrange
			var catalogEntry = new CatalogEntryDto
			{
				Attributes = new[]
				{
					new Attribute( 1, "foo" ),
					new Attribute( 2, "42.5" )
				}
			};

			// act
			var description = catalogEntry.ToString( CultureInfo.InvariantCulture );

			// assert
			Assert.That( description, Is.EqualTo( "foo - 42.5" ) );
		}

		#endregion

		#region helper methos

		private static IEnumerable CreateNullAttributeTestCases()
		{
			// @formatter:off — disable formatter after this line

			// value: null
			yield return CreateTestCase( null, "",   "null" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateAlphaNumericAttributeTestCases()
		{
			// @formatter:off — disable formatter after this line

			// value: "foo"
			yield return CreateTestCase( "foo", "foo","\"foo\"" );

			// value: "" (empty string)
			yield return CreateTestCase( "", "",   "\"\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateIntegerAttributeTestCases()
		{
			// @formatter:off — disable formatter after this line

			// value: 1 as integer, short and string
			yield return CreateTestCase( 1,        "1", "1" );
			yield return CreateTestCase( (short)1, "1", "1 (short)" );
			yield return CreateTestCase( "1",      "1", "\"1\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateFloatAttributeTestCases()
		{
			// @formatter:off — disable formatter after this line

			// value: 1.0 as double and string
			yield return CreateTestCase( 1.0,       "1", "1.0" );
			yield return CreateTestCase( "1.0",   "1.0", "\"1.0\"" );

			// value: -1.78 as double and string
			yield return CreateTestCase(  -1.78,  "-1.78", "-1.78" );
			yield return CreateTestCase( "-1.78", "-1.78", "\"-1.78\"" );

			// value: special double values like NaN and Infinity
			yield return CreateTestCase( double.NaN,                    "NaN", "NaN" );
			yield return CreateTestCase( double.PositiveInfinity,  "Infinity", "Infinity" );
			yield return CreateTestCase( double.NegativeInfinity, "-Infinity", "-Infinity" );
			yield return CreateTestCase( "NaN",                         "NaN", "\"NaN\"" );
			yield return CreateTestCase( "Infinity",               "Infinity", "\"Infinity\"" );
			yield return CreateTestCase( "-Infinity",             "-Infinity", "\"-Infinity\"" );

			// @formatter:on — enable formatter after this line
		}

		private static IEnumerable CreateDateTimeAttributeTestCases()
		{
			// @formatter:off — disable formatter after this line

			// value: date
			var universalTime = DateTime.Parse( "2015-03-09T19:12:00Z" ).ToUniversalTime();
			var universalTimeAsString = universalTime.ToLocalTime().ToString( CultureInfo.InvariantCulture );

			var unspecifiedTime = new DateTime( universalTime.Ticks, DateTimeKind.Unspecified );
			var unspecifiedTimeAsString = unspecifiedTime.ToLocalTime().ToString( CultureInfo.InvariantCulture );

			yield return CreateTestCase( universalTimeAsString, universalTimeAsString,   $"\"{universalTimeAsString}\"" );
			yield return CreateTestCase( universalTime,         universalTimeAsString,   $"utc: {universalTimeAsString}" );
			yield return CreateTestCase( unspecifiedTime,       unspecifiedTimeAsString, $"unspecified: {unspecifiedTimeAsString}" );

			// @formatter:on — enable formatter after this line
		}


		private static CatalogEntryDescriptionTestCase CreateTestCase(
			object attributeValue,
			string expectedDescription,
			string displayText = null )
		{
			return new CatalogEntryDescriptionTestCase
			{
				AttributeValue = attributeValue,
				ExpectedDescription = expectedDescription,
				DisplayText = displayText
			};
		}

		#endregion

		#region class CatalogEntryDescriptionTestCase

		public class CatalogEntryDescriptionTestCase
		{
			#region properties

			public string DisplayText { get; set; }

			public object AttributeValue { get; set; }

			public string ExpectedDescription { get; set; }

			#endregion

			#region methods

			public override string ToString()
			{
				return DisplayText ?? ExpectedDescription;
			}

			#endregion
		}

		#endregion
	}
}