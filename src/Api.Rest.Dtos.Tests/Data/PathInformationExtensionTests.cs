#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Tests.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class PathInformationExtensionTests
	{
		#region constants

		private const string Root = "/";

		#endregion

		#region methods

		[Test]
		public void Test_FindCommonParent_For_Null_Reference()
		{
			// arrange

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( (IEnumerable<PathInformationDto>)null );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		public void Test_FindCommonParent_For_Empty_Path()
		{
			// arrange

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( Enumerable.Empty<PathInformationDto>() );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/" )]
		[TestCase( "PPC:/A/B/C/" )]
		[TestCase( Root )]
		public void Test_FindCommonParent_For_One_Path( string pathString )
		{
			// arrange
			var path = PathHelper.RoundtripString2PathInformation( pathString );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( new List<PathInformationDto> { path } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( path ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/D/E/F/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/D/E/F/" )]
		public void Test_FindCommonParent_For_Two_Paths_Is_Root( string path1String, string path2String )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( new List<PathInformationDto> { path1, path2 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/C/", "PPP:/A/B/C/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/C/", "PCC:/A/B/C/" )]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/D/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/D/", "PC:/A/B/" )]
		[TestCase( "PPP:/A/B/C/", "PP:/A/B/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PC:/A/B/", "PC:/A/B/" )]
		public void Test_FindCommonParent_For_Two_Paths( string path1String, string path2String, string expectedPathString )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( new List<PathInformationDto> { path1, path2 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathHelper.RoundtripString2PathInformation( expectedPathString ) ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/D/E/F/", "PPP:/G/H/I/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/D/E/F/", "PCC:/G/H/I/" )]
		public void Test_FindCommonParent_For_More_Than_Two_Paths_Is_Root( string path1String, string path2String, string path3String )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );
			var path3 = PathHelper.RoundtripString2PathInformation( path3String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( new List<PathInformationDto> { path1, path2, path3 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/C/", "PPP:/A/B/C/", "PPP:/A/B/C/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/C/", "PCC:/A/B/C/", "PCC:/A/B/C/" )]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/D/", "PPP:/A/B/E/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/D/", "PCC:/A/B/E/", "PC:/A/B/" )]
		[TestCase( "PPP:/A/B/C/", "PP:/A/B/", "PPP:/A/B/D/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PC:/A/B/", "PCC:/A/B/D/", "PC:/A/B/" )]
		public void Test_FindCommonParent_For_More_Than_Two_Paths( string path1String, string path2String, string path3String, string expectedPathString )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );
			var path3 = PathHelper.RoundtripString2PathInformation( path3String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParent( new List<PathInformationDto> { path1, path2, path3 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathHelper.RoundtripString2PathInformation( expectedPathString ) ) );
		}

		[Test]
		public void Test_FindCommonParentPart_For_Null_Reference()
		{
			// arrange

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( (IEnumerable<PathInformationDto>)null );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		public void Test_FindCommonParentPart_For_Empty_Path()
		{
			// arrange

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( Enumerable.Empty<PathInformationDto>() );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/C/" )]
		[TestCase( "PPC:/A/B/C/", "PP:/A/B/" )]
		[TestCase( Root, Root )]
		public void Test_FindCommonParentPart_For_One_Path( string pathString, string expectedPathString )
		{
			// arrange
			var path = PathHelper.RoundtripString2PathInformation( pathString );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( new List<PathInformationDto> { path } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathHelper.RoundtripString2PathInformation( expectedPathString ) ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/D/E/F/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/D/E/F/" )]
		[TestCase( "CCC:/A/B/C/", "CCC:/A/E/F/" )]
		public void Test_FindCommonParentPart_For_Two_Paths_Is_Root( string path1String, string path2String )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( new List<PathInformationDto> { path1, path2 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/C/", "PPP:/A/B/C/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/C/", "P:/A/" )]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/D/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/D/", "P:/A/" )]
		[TestCase( "PPP:/A/B/C/", "PP:/A/B/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PC:/A/B/", "P:/A/" )]
		public void Test_FindCommonParentPart_For_Two_Paths( string path1String, string path2String, string expectedPathString )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( new List<PathInformationDto> { path1, path2 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathHelper.RoundtripString2PathInformation( expectedPathString ) ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/D/E/F/", "PPP:/G/H/I/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/D/E/F/", "PCC:/G/H/I/" )]
		[TestCase( "CCC:/A/B/C/", "CCC:/A/E/F/", "CCC:/A/H/I/" )]
		public void Test_FindCommonParentPart_For_More_Than_Two_Paths_Is_Root( string path1String, string path2String, string path3String )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );
			var path3 = PathHelper.RoundtripString2PathInformation( path3String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( new List<PathInformationDto> { path1, path2, path3 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/C/", "PPP:/A/B/C/", "PPP:/A/B/C/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/C/", "PCC:/A/B/C/", "P:/A/" )]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/D/", "PPP:/A/B/E/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PCC:/A/B/D/", "PCC:/A/B/E/", "P:/A/" )]
		[TestCase( "PPP:/A/B/C/", "PP:/A/B/", "PPP:/A/B/D/", "PP:/A/B/" )]
		[TestCase( "PCC:/A/B/C/", "PC:/A/B/", "PCC:/A/B/D/", "P:/A/" )]
		public void Test_FindCommonParentPart_For_More_Than_Two_Paths( string path1String, string path2String, string path3String, string expectedPathString )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );
			var path3 = PathHelper.RoundtripString2PathInformation( path3String );

			// act
			var commonParentPath = PathInformationExtension.FindCommonParentPart( new List<PathInformationDto> { path1, path2, path3 } );

			// assert
			Assert.That( commonParentPath, Is.EqualTo( PathHelper.RoundtripString2PathInformation( expectedPathString ) ) );
		}

		[Test]
		public void Test_FindParentParts_For_Null_Reference()
		{
			// arrange

			// act
			var result = PathInformationExtension.FindParentParts( (IEnumerable<PathInformationDto>)null );

			// assert
			Assert.That( result, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		public void Test_FindParentParts_For_Empty_Path()
		{
			// arrange

			// act
			var result = PathInformationExtension.FindParentParts( Enumerable.Empty<PathInformationDto>() );

			// assert
			Assert.That( result, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "PPP:/A/B/C/", "PPP:/A/B/C/" )]
		[TestCase( "PPC:/A/B/C/", "PP:/A/B/" )]
		[TestCase( "CCC:/A/B/C/", Root )]
		[TestCase( Root, Root )]
		public void Test_FindParentParts_For_One_Path( string pathString, string expectedPathString )
		{
			// arrange
			var path = PathHelper.RoundtripString2PathInformation( pathString );

			// act
			var result = PathInformationExtension.FindParentParts( new List<PathInformationDto> { path } );

			// assert
			Assert.That( result, Has.Length.EqualTo( 1 ) );
			Assert.That( result, Has.ItemAt( 0 ).EqualTo( PathHelper.RoundtripString2PathInformation( expectedPathString ) ) );
		}

		[Test]
		public void Test_FindParentParts_For_Multiple_Paths()
		{
			// arrange
			var paths = new[]
			{
				PathHelper.RoundtripString2PathInformation( Root ),
				PathHelper.RoundtripString2PathInformation( "P:/P1/" ),
				PathHelper.RoundtripString2PathInformation( "PP:/P1/P2/" ),
				PathHelper.RoundtripString2PathInformation( "PPC:/P1/P2/C1/" ),
				PathHelper.RoundtripString2PathInformation( "PPCC:/P1/P2/C1/C2/" ),
				PathHelper.RoundtripString2PathInformation( "CC:/C1/C2/" )
			};

			var expectedPaths = new[]
			{
				PathHelper.RoundtripString2PathInformation( Root ),
				PathHelper.RoundtripString2PathInformation( "P:/P1/" ),
				PathHelper.RoundtripString2PathInformation( "PP:/P1/P2/" )
			};

			// act
			var result = PathInformationExtension.FindParentParts( paths );

			// assert
			Assert.That( result, Is.EquivalentTo( expectedPaths ) );
		}

		[Test]
		[TestCase( StringComparison.CurrentCulture )]
		[TestCase( StringComparison.CurrentCultureIgnoreCase )]
		public void Test_Compare_Paths_By_ReferenceEquality( StringComparison comparison )
		{
			// arrange
			var path = PathHelper.RoundtripString2PathInformation( "P:/P1/" );

			// act
			var result = path.Equals( path, comparison );

			// assert
			Assert.That( result, Is.True );
		}

		[Test]
		[TestCase( Root, Root, StringComparison.CurrentCulture, true )]
		[TestCase( "PPC:/P1/P2/C1/", "PP:/P1/P2/", StringComparison.CurrentCulture, false )]
		[TestCase( "PPC:/P1/P2/C1/", "PPC:/P1/P2/C1/", StringComparison.CurrentCulture, true )]
		[TestCase( "PPC:/P1/P2/C1/", "PPP:/P1/P2/C1/", StringComparison.CurrentCulture, false )]
		[TestCase( "PPC:/P1/P2/C1/", "PPC:/P1/P3/C1/", StringComparison.CurrentCulture, false )]
		[TestCase( "PPC:/P1/P2/C1/", "PPC:/p1/P2/C1/", StringComparison.CurrentCulture, false )]
		[TestCase( "PPC:/P1/P2/C1/", "PPC:/p1/P2/C1/", StringComparison.CurrentCultureIgnoreCase, true )]
		public void Test_Compare_Paths(
			string path1String,
			string path2String,
			StringComparison comparison,
			bool expected )
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( path1String );
			var path2 = PathHelper.RoundtripString2PathInformation( path2String );

			// act
			var result = path1.Equals( path2, comparison );

			// assert
			Assert.That( result, Is.EqualTo( expected ) );
		}

		[Test]
		public void Test_GetParts_Of_Paths()
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( "PP:/Part1/Part2/" );
			var path2 = PathHelper.RoundtripString2PathInformation( "PPC:/Part3/Part4/Char1/" );
			var path3 = PathHelper.RoundtripString2PathInformation( "PP:/Part1/Part2/" );
			var path4 = PathHelper.RoundtripString2PathInformation( "PC:/Part1/Char2/" );
			var path5 = PathHelper.RoundtripString2PathInformation( "PPP:/Part1/Part2/Part3/" );
			var paths = new[] { path1, path2, path3, path4, path5 };

			// act
			var parts = paths.GetParts();

			// assert
			Assert.That( parts, Has.Length.EqualTo( 3 ) );
			Assert.That( parts, Is.EquivalentTo( new[] { path1, path3, path5 } ) );
		}

		[Test]
		public void Test_GetCharacteristics_Of_Paths()
		{
			// arrange
			var path1 = PathHelper.RoundtripString2PathInformation( "PP:/Part1/Part2/" );
			var path2 = PathHelper.RoundtripString2PathInformation( "PPC:/Part3/Part4/Char1/" );
			var path3 = PathHelper.RoundtripString2PathInformation( "PP:/Part1/Part2/" );
			var path4 = PathHelper.RoundtripString2PathInformation( "PC:/Part1/Char2/" );
			var path5 = PathHelper.RoundtripString2PathInformation( "PPP:/Part1/Part2/Part3/" );
			var paths = new[] { path1, path2, path3, path4, path5 };

			// act
			var characteristics = paths.GetCharacteristics();

			// assert
			Assert.That( characteristics, Has.Length.EqualTo( 2 ) );
			Assert.That( characteristics, Is.EquivalentTo( new[] { path2, path4 } ) );
		}

		[Test]
		[TestCase( null, "P:/Part1/", Root )]
		[TestCase( Root, "P:/Part1/", Root )]
		[TestCase( "P:/Part1/", null, "P:/Part1/" )]
		[TestCase( "P:/Part1/", Root, "P:/Part1/" )]
		[TestCase( "P:/Part1/", "P:/Part1/", Root )]
		[TestCase( "P:/Part1/", "P:/Part2/", "P:/Part1/" )]
		[TestCase( "PP:/Part1/Part2/", "P:/Part1/", "P:/Part2/" )]
		[TestCase( "PPC:/Part1/Part2/Char1/", "P:/Part1/", "PC:/Part2/Char1/" )]
		[TestCase( "PPC:/Part1/Part2/Char1/", "PP:/Part1/Part3/", "PC:/Part2/Char1/" )]
		public void Test_Provide_Relative_Path( string pathString, string basePathString, string expectedPathString )
		{
			// arrange
			var path = CreatePath( pathString );
			var basePath = CreatePath( basePathString );
			var expectedPath = CreatePath( expectedPathString );

			// act
			var result = path.RelativeTo( basePath );

			// assert
			Assert.That( result, Is.EqualTo( expectedPath ) );
		}

		private static PathInformationDto CreatePath( string pathString )
		{
			if( pathString is null )
				return null;
			return PathHelper.RoundtripString2PathInformation( pathString );
		}

		#endregion
	}
}