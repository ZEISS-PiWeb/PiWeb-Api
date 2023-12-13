#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests
{
	#region usings

	using System;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Core;

	#endregion

	[TestFixture]
	public class PathHelperTests
	{
		#region methods

		[Test]
		public void String2PartPathInformation_PathIsEmpty_ThrowsException()
		{
			Assert.Throws<ArgumentException>( () => PathHelper.String2PartPathInformation( string.Empty ) );
		}

		[Test]
		public void String2PartPathInformation_PathIsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "\u0001";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathStartsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "/foo";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "foo" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathStartsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "/\u0001";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathEndsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "foo/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "foo" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathEndsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "\u0001/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathContainsEscapeString_ReturnsPathInformationDto()
		{
			const string path = "\\foo";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "foo" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathIsDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( PathInformation.Root, actualResult );
		}

		[Test]
		public void String2PartPathInformation_HappyPath_ReturnsPathInformationDto()
		{
			const string path = "/foo/bar/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "foo" ), PathElement.Part( "bar" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathEndsWithDoubleDelimiterStrings_ThrowsException()
		{
			const string path = "foo//";

			Assert.Throws<InvalidOperationException>( () => PathHelper.String2PartPathInformation( path ) );
		}

		[Test]
		public void String2PartPathInformation_PathStartsWithDoubleDelimiterStrings_ThrowsException()
		{
			const string path = "//foo";

			Assert.Throws<InvalidOperationException>( () => PathHelper.String2PartPathInformation( path ) );
		}

		[Test]
		public void String2CharPathInformation_PathIsEmpty_ThrowsException()
		{
			Assert.Throws<ArgumentException>( () => PathHelper.String2CharPathInformation( string.Empty ) );
		}

		[Test]
		public void String2CharPathInformation_PathIsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "\u0001";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathStartsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "/foo";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "foo" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathStartsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "/\u0001";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathEndsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "foo/";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "foo" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathEndsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "\u0001/";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathIsDelimiterString_ThrowsException()
		{
			const string path = "/";

			Assert.Throws<ArgumentException>( () => PathHelper.String2CharPathInformation( path ) );
		}

		[Test]
		public void String2CharPathInformation_HappyPath_ReturnsPathInformationDto()
		{
			const string path = "/foo/bar/";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "foo" ), PathElement.Char( "bar" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathEndsWithDoubleDelimiterStrings_ThrowsException()
		{
			const string path = "bar//";

			Assert.Throws<InvalidOperationException>( () => PathHelper.String2CharPathInformation( path ) );
		}

		[Test]
		public void String2CharPathInformation_PathStartsWithDoubleDelimiterStrings_ThrowsException()
		{
			const string path = "//foo";

			Assert.Throws<InvalidOperationException>( () => PathHelper.String2CharPathInformation( path ) );
		}

		[Test]
		public void String2CharPathInformation_PathContainsEscapeString_ReturnsPathInformationDto()
		{
			const string path = "\\foo";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Char( "foo" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathNotEndingWithUnquotedDelimiter_ThrowsCorrectException()
		{
			const string path = "\\foo\\";

			FluentActions.Invoking( () => PathHelper.String2CharPathInformation( path ) ).Should().Throw<InvalidOperationException>()
				.Where( e => e.Message.Contains( "does not end with an unquoted delimiter character" ) );
		}

		[Test]
		public void RoundtripString2PathInformation_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentException>( () => PathHelper.RoundtripString2PathInformation( null! ) );
		}

		[Test]
		public void RoundtripString2PathInformation_PathIsEmpty_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentException>( () => PathHelper.RoundtripString2PathInformation( null! ) );
		}

		[Test]
		public void RoundtripString2PathInformation_HappyPath_ReturnsPathInformationDto()
		{
			const string path = "PP:/foo/bar/";
			var actualResult = PathHelper.RoundtripString2PathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "foo" ), PathElement.Part( "bar" ) ), actualResult );
		}

		[Test]
		public void RoundtripString2PathInformation_PartsAndCharacteristic_ReturnsPathInformationDto()
		{
			const string path = "PPC:/foo/bar/char1/";
			var actualResult = PathHelper.RoundtripString2PathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( new PathInformation( PathElement.Part( "foo" ), PathElement.Part( "bar" ), PathElement.Char( "char1" ) ), actualResult );
		}

		[Test]
		public void RoundtripString2PathInformation_MissingStructure_ThrowsCorrectException()
		{
			const string path = "/foo/bar/char1/";

			FluentActions.Invoking( () => PathHelper.RoundtripString2PathInformation( path ) ).Should().Throw<ArgumentException>()
				.WithMessage( "The path must have the following structure:\"structure:path\", e.g.: \"PC:/part/characteristic/\"." );
		}

		[Test]
		public void RoundtripString2PathInformation_WrongStructure_ThrowsCorrectException()
		{
			const string path = "PP:/foo/bar/char1/";

			FluentActions.Invoking( () => PathHelper.RoundtripString2PathInformation( path ) ).Should().Throw<ArgumentException>();
		}

		[Test]
		public void RoundtripString2PathInformation_FastPathRoot_ReturnsPathInformationDto()
		{
			const string path = "/";
			var actualResult = PathHelper.RoundtripString2PathInformation( path );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( PathInformation.Root, actualResult );
		}

		[Test]
		public void RoundtripString2PathInformation_MissingFirstDelimiter_ThrowsException()
		{
			const string path = "P:NoDelimiter/";

			FluentActions.Invoking( () => PathHelper.RoundtripString2PathInformation( path ) ).Should().Throw<InvalidOperationException>()
				.Where( e => e.Message.Contains( "The first character of path string" ) );
		}

		[Test]
		public void DatabaseString2PathInformation_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentException>( () => PathHelper.DatabaseString2PathInformation( null!, "".AsSpan() ) );
		}

		[Test]
		public void DatabaseString2PathInformation_FastPathRoot_ReturnsPathInformationDto()
		{
			const string path = "/";
			var actualResult = PathHelper.DatabaseString2PathInformation( path.AsSpan(), "".AsSpan() );

			actualResult.Should().BeOfType<PathInformation>();
			Assert.AreEqual( PathInformation.Root, actualResult );
		}

		[Test]
		public void DatabaseString2PathInformation_MissingLastDelimiter_ThrowsCorrectException()
		{
			const string path = "/foo/bar";

			FluentActions.Invoking( () => PathHelper.DatabaseString2PathInformation( path.AsSpan(), "PP".AsSpan() ) ).Should().Throw<InvalidOperationException>()
				.Where( e => e.Message.Contains( "The last character of path string" ) );
		}

		[Test]
		public void PathInformation2String_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentNullException>( () => PathHelper.PathInformation2String( null! ) );
		}

		[Test]
		public void PathInformation2String_HappyPath_ReturnsCorrectString()
		{
			var pathInformation = new PathInformation( PathElement.Part( "foo" ), PathElement.Part( "bar" ) );
			var actualResult = PathHelper.PathInformation2String( pathInformation );

			const string expected = "foo/bar";

			Assert.AreEqual( expected, actualResult );
		}

		[Test]
		public void PathInformation2String_FastPathRoot_ReturnsDelimiter()
		{
			var actualResult = PathHelper.PathInformation2String( PathInformation.Root );

			Assert.AreEqual( PathHelper.DelimiterString, actualResult );
		}

		[Test]
		public void PathInformation2RoundtripString_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentNullException>( () => PathHelper.PathInformation2RoundtripString( null! ) );
		}

		[Test]
		public void PathInformation2RoundtripString_HappyPath_ReturnsCorrectString()
		{
			var pathInformation = new PathInformation( PathElement.Part( "foo" ), PathElement.Part( "bar" ), PathElement.Char( "char1" ) );
			var actualResult = PathHelper.PathInformation2RoundtripString( pathInformation );

			const string expected = "PPC:/foo/bar/char1/";

			Assert.AreEqual( expected, actualResult );
		}

		[Test]
		public void PathInformation2RoundtripString_FastPathRoot_ReturnsDelimiter()
		{
			var actualResult = PathHelper.PathInformation2RoundtripString( PathInformation.Root );

			Assert.AreEqual( PathHelper.DelimiterString, actualResult );
		}

		[Test]
		public void PathInformation2DatabaseString_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentNullException>( () => PathHelper.PathInformation2DatabaseString( null! ) );
		}


		[Test]
		public void PathInformation2DatabaseString_FastPathRoot_ReturnsDelimiter()
		{
			var actualResult = PathHelper.PathInformation2DatabaseString( PathInformation.Root );

			Assert.AreEqual( PathHelper.DelimiterString, actualResult );
		}

		[Test]
		public void GetStructure_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentNullException>( () => PathHelper.GetStructure( null! ) );
		}

		[Test]
		public void GetStructure_HappyPath_ReturnsCorrectString()
		{
			var pathInformation = new PathInformation( PathElement.Part( "foo" ), PathElement.Part( "bar" ), PathElement.Char( "char1" ) );
			var actualResult = PathHelper.GetStructure( pathInformation );

			const string expected = "PPC";

			Assert.AreEqual( expected, actualResult );
		}

		#endregion
	}
}