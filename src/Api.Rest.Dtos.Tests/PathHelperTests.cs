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

		#endregion
	}
}