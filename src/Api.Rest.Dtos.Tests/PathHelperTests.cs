#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
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
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

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

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathStartsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "/foo";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "foo" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathStartsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "/\u0001";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathEndsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "foo/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "foo" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathEndsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "\u0001/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathContainsEscapeString_ReturnsPathInformationDto()
		{
			const string path = "\\foo";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "foo" ) ), actualResult );
		}

		[Test]
		public void String2PartPathInformation_PathIsDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( PathInformationDto.Root, actualResult );
		}

		[Test]
		public void String2PartPathInformation_HappyPath_ReturnsPathInformationDto()
		{
			const string path = "/foo/bar/";
			var actualResult = PathHelper.String2PartPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "foo" ), PathElementDto.Part( "bar" ) ), actualResult );
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

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathStartsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "/foo";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "foo" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathStartsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "/\u0001";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "\u0001" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathEndsWithDelimiterString_ReturnsPathInformationDto()
		{
			const string path = "foo/";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "foo" ) ), actualResult );
		}

		[Test]
		public void String2CharPathInformation_PathEndsWithDelimiterStringAndContainsSpecialCharacter_ReturnsPathInformationDto()
		{
			const string path = "\u0001/";
			var actualResult = PathHelper.String2CharPathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "\u0001" ) ), actualResult );
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

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "foo" ), PathElementDto.Char( "bar" ) ), actualResult );
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

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Char( "foo" ) ), actualResult );
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

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "foo" ), PathElementDto.Part( "bar" ) ), actualResult );
		}

		[Test]
		public void RoundtripString2PathInformation_PartsAndCharacteristic_ReturnsPathInformationDto()
		{
			const string path = "PPC:/foo/bar/char1/";
			var actualResult = PathHelper.RoundtripString2PathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( new PathInformationDto( PathElementDto.Part( "foo" ), PathElementDto.Part( "bar" ), PathElementDto.Char( "char1" ) ), actualResult );
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

			FluentActions.Invoking( () => PathHelper.RoundtripString2PathInformation( path ) ).Should().Throw<IndexOutOfRangeException>();
		}

		[Test]
		public void RoundtripString2PathInformation_FastPathRoot_ReturnsPathInformationDto()
		{
			const string path = "/";
			var actualResult = PathHelper.RoundtripString2PathInformation( path );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( PathInformationDto.Root, actualResult );
		}

		[Test]
		public void RoundtripString2PathInformation_MissingFirstDelimiter_ThrowsException()
		{
			const string path = "P:NoDelimiter/";

			FluentActions.Invoking( () => PathHelper.RoundtripString2PathInformation( path ) ).Should().Throw<ArgumentException>()
				.Where( e => e.Message.Contains( "The database path string must start with a delimiter" ) );
		}

		[Test]
		public void DatabaseString2PathInformation_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentException>( () => PathHelper.DatabaseString2PathInformation( null!, string.Empty ) );
		}

		[Test]
		public void DatabaseString2PathInformation_FastPathRoot_ReturnsPathInformationDto()
		{
			const string path = "/";
			var actualResult = PathHelper.DatabaseString2PathInformation( path, string.Empty );

			actualResult.Should().BeOfType<PathInformationDto>();
			Assert.AreEqual( PathInformationDto.Root, actualResult );
		}

		[Test]
		public void DatabaseString2PathInformation_MissingLastDelimiter_ThrowsCorrectException()
		{
			const string path = "/foo/bar";

			FluentActions.Invoking( () => PathHelper.DatabaseString2PathInformation( path, "PP" ) ).Should().Throw<InvalidOperationException>()
				.Where( e => e.Message.Contains( "The last component of path string" ) );
		}

		[Test]
		public void PathInformation2String_PathIsNull_ThrowsCorrectException()
		{
			Assert.Throws<ArgumentNullException>( () => PathHelper.PathInformation2String( null! ) );
		}

		[Test]
		public void PathInformation2String_HappyPath_ReturnsCorrectString()
		{
			var pathInformation = new PathInformationDto( PathElementDto.Part( "foo" ), PathElementDto.Part( "bar" ) );
			var actualResult = PathHelper.PathInformation2String( pathInformation );

			const string expected = "foo/bar";

			Assert.AreEqual( expected, actualResult );
		}

		[Test]
		public void PathInformation2String_FastPathRoot_ReturnsDelimiter()
		{
			var actualResult = PathHelper.PathInformation2String( PathInformationDto.Root );

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
			var pathInformation = new PathInformationDto( PathElementDto.Part( "foo" ), PathElementDto.Part( "bar" ), PathElementDto.Char( "char1" ) );
			var actualResult = PathHelper.PathInformation2RoundtripString( pathInformation );

			const string expected = "PPC:/foo/bar/char1/";

			Assert.AreEqual( expected, actualResult );
		}

		[Test]
		public void PathInformation2RoundtripString_FastPathRoot_ReturnsDelimiter()
		{
			var actualResult = PathHelper.PathInformation2RoundtripString( PathInformationDto.Root );

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
			var actualResult = PathHelper.PathInformation2DatabaseString( PathInformationDto.Root );

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
			var pathInformation = new PathInformationDto( PathElementDto.Part( "foo" ), PathElementDto.Part( "bar" ), PathElementDto.Char( "char1" ) );
			var actualResult = PathHelper.GetStructure( pathInformation );

			const string expected = "PPC";

			Assert.AreEqual( expected, actualResult );
		}

		#endregion
	}
}