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
	using JetBrains.Annotations;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class PathInformationExtensionTests
	{
		#region constants

		private const char Delimiter = ';';
		private const string Root = "\\";

		#endregion

		#region methods

		[Test]
		[TestCase( "P:Part1:P:Part11;C:Char1", "P:Part1;P:Part11;C:Char1", StringComparison.CurrentCulture, true )]
		[TestCase( "P:Part1;P:Part11;C:Char1", "P:Part1;P:Part11;P:Char1", StringComparison.CurrentCulture, false )]
		[TestCase( "P:Part1;P:Part11;C:Char1", "P:Part1;P:Part21;C:Char1", StringComparison.CurrentCulture, false )]
		[TestCase( "P:Part1;P:Part11;C:Char1", "P:part1;P:part11;C:char1", StringComparison.CurrentCulture, false )]
		[TestCase( "P:Part1;P:Part11;C:Char1", "P:part1;P:part11;C:char1", StringComparison.CurrentCultureIgnoreCase, true )]
		public void Should_Check_Paths_For_Equal(
			string path1String,
			string path2String,
			StringComparison comparison,
			bool expected )
		{
			//***** arrange *****
			var path1 = CreatePath( path1String );
			var path2 = CreatePath( path2String );

			//***** act *****
			var result = path1.Equals( path2, comparison );

			//***** assert *****
			Assert.That( result, Is.EqualTo( expected ) );
		}

		private static PathInformationDto CreatePath( string pathString )
		{
			if( string.IsNullOrEmpty( pathString ) )
				return null;

			if( pathString[0] == Root[0] )
				return PathInformationDto.Root;

			var pathSegments = pathString.Split( Delimiter );
			var pathElements = new List<PathElementDto>( pathSegments.Length );
			var isParentChar = false;
			foreach( var pathSegment in pathSegments )
			{
				var pathElement = CreatePathElement( pathSegment );
				if( isParentChar && pathElement.Type == InspectionPlanEntityDto.Part )
					throw new ArgumentException( "Cannot add part as child of characteristic." );

				isParentChar = pathElement.Type == InspectionPlanEntityDto.Characteristic;
				pathElements.Add( pathElement );
			}

			return new PathInformationDto( pathElements );
		}

		private static PathElementDto CreatePathElement( [NotNull] string pathSegment )
		{
			if( pathSegment.IndexOf( ':' ) != 1 )
				throw new ArgumentException( $"Invalid format of path segment \"{pathSegment}\"." );

			switch( pathSegment[ 0 ] )
			{
				case 'P':
				case 'p':
					return PathElementDto.Part( pathSegment.Substring( 2 ) );

				case 'C':
				case 'c':
					return PathElementDto.Char( pathSegment.Substring( 2 ) );

				default:
					throw new ArgumentException( $"Invalid format of path segment \"{pathSegment}\"." );
			}
		}

		[Test]
		public void Should_Get_Parts_Of_Paths()
		{
			//***** arrange *****
			var path1 = CreatePath( "P:Part1;P:Part2" );
			var path2 = CreatePath( "P:Part3;P:Part4;C:Char1" );
			var path3 = CreatePath( "P:Part1;P:Part2" );
			var path4 = CreatePath( "P:Part1;C:Char2" );
			var path5 = CreatePath( "P:Part1;P:Part2;P:Part3" );
			var paths = new[] { path1, path2, path3, path4, path5 };

			//***** act *****
			var parts = paths.GetParts();

			//***** assert *****
			Assert.That( parts, Has.Length.EqualTo( 3 ) );
			Assert.That( parts, Is.EquivalentTo( new[] { path1, path3, path5 } ) );
		}

		[Test]
		public void Should_Get_Characteristics_Of_Paths()
		{
			//***** arrange *****
			var path1 = CreatePath( "P:Part1;P:Part2" );
			var path2 = CreatePath( "P:Part3;P:Part4;C:Char1" );
			var path3 = CreatePath( "P:Part1;P:Part2" );
			var path4 = CreatePath( "P:Part1;C:Char2" );
			var path5 = CreatePath( "P:Part1;P:Part2;P:Part3" );
			var paths = new[] { path1, path2, path3, path4, path5 };

			//***** act *****
			var characteristics = paths.GetCharacteristics();

			//***** assert *****
			Assert.That( characteristics, Has.Length.EqualTo( 2 ) );
			Assert.That( characteristics, Is.EquivalentTo( new[] { path2, path4 } ) );
		}

		[Test]
		[TestCase( "P:Part1", "P:Part1" )]
		[TestCase( "P:PartX", "P:PartX;P:Part2;C:Char1", "P:PartX;P:Part3;C:Char1" )]
		[TestCase( "P:PartY;C:CharY", "P:PartY;C:CharY;C:Char1", "P:PartY;C:CharY;C:Char2;C:Char3", "P:PartY;C:CharY;C:Char2;C:Char4" )]
		public void Should_Find_Common_Parent_Path( string expectedString, params string[] pathStrings )
		{
			//***** arrange *****
			var expected = CreatePath( expectedString );
			var paths = pathStrings.Select( CreatePath ).ToArray();

			//***** act *****
			var parentPath = PathInformationExtension.FindCommonParent( paths );

			//***** assert *****
			Assert.That( parentPath, Is.EqualTo( expected ) );
		}

		[Test]
		[TestCase( "P:Part1", "P:Part2" )]
		[TestCase( "P:PartX", "P:PartY;C:Char1", "P:PartY;P:Part3;C:Char1" )]
		public void Should_Find_Common_Parent_Path_Return_Root( params string[] pathStrings )
		{
			//***** arrange *****
			var paths = pathStrings.Select( CreatePath ).ToArray();

			//***** act *****
			var parentPath = PathInformationExtension.FindCommonParent( paths );

			//***** assert *****
			Assert.That( parentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( "P:Part1", "P:Part1" )]
		[TestCase( "P:PartX", "P:PartX;P:Part2;C:Char1", "P:PartX;P:Part3;C:Char1" )]
		[TestCase( "P:PartY", "P:PartY;C:CharY;C:Char1", "P:PartY;C:CharY;C:Char2;C:Char3", "P:PartY;C:CharY;C:Char2;C:Char4" )]
		public void Should_Find_Common_Parent_Part_Path( string expectedString, params string[] pathStrings )
		{
			//***** arrange *****
			var expected = CreatePath( expectedString );
			var paths = pathStrings.Select( CreatePath ).ToArray();

			//***** act *****
			var parentPath = PathInformationExtension.FindCommonParentPart( paths );

			//***** assert *****
			Assert.That( parentPath, Is.EqualTo( expected ) );
		}

		[Test]
		[TestCase( "P:Part1", "P:Part2" )]
		[TestCase( "P:PartX", "P:PartY;C:Char1", "P:PartY;P:Part3;C:Char1" )]
		[TestCase( "C:CharX;C:Char1", "C:CharX;C:Char2" )]
		public void Should_Find_Common_Parent_Part_Path_Return_Root( params string[] pathStrings )
		{
			//***** arrange *****
			var paths = pathStrings.Select( CreatePath ).ToArray();

			//***** act *****
			var parentPath = PathInformationExtension.FindCommonParentPart( paths );

			//***** assert *****
			Assert.That( parentPath, Is.EqualTo( PathInformationDto.Root ) );
		}

		[Test]
		[TestCase( Root, Root )]
		[TestCase( "P:Part1", "P:Part1" )]
		[TestCase( "P:Part1;P:Part2", "P:Part1;P:Part2" )]
		[TestCase( "P:Part1;P:Part2;C:Char1", "P:Part1;P:Part2" )]
		[TestCase( "P:Part1;P:Part2;C:Char1;C:Char2", "P:Part1;P:Part2" )]
		[TestCase( "C:Char1;C:Char2", Root )]
		public void Should_Find_Parent_Part_For_Single_Path( string pathString, string expectedPathString )
		{
			//***** arrange *****

			var paths = new[] { CreatePath( pathString ) };
			var expectedPath = CreatePath( expectedPathString );

			//***** act *****
			var result = PathInformationExtension.FindParentParts( paths );

			//***** assert *****
			Assert.That( result, Has.Length.EqualTo( 1 ) );
			Assert.That( result, Has.ItemAt( 0 ).EqualTo( expectedPath ) );
		}

		[Test]
		public void Should_Find_Parent_Parts_For_Multiple_Paths()
		{
			//***** arrange *****
			var paths = new[]
			{
				CreatePath( Root ),
				CreatePath( "P:Part1" ),
				CreatePath( "P:Part1;P:Part2" ),
				CreatePath( "P:Part1;P:Part2;C:Char1" ),
				CreatePath( "P:Part1;P:Part2;C:Char1;C:Char2" ),
				CreatePath( "C:Char1;C:Char2" )
			};

			var expectedPaths = new[]
			{
				CreatePath( Root ),
				CreatePath( "P:Part1" ),
				CreatePath( "P:Part1;P:Part2" )
			};

			//***** act *****
			var result = PathInformationExtension.FindParentParts( paths );

			//***** assert *****
			Assert.That( result, Is.EquivalentTo( expectedPaths ) );
		}

		[Test]
		[TestCase( null, "P:Part1", Root )]
		[TestCase( Root, "P:Part1", Root )]
		[TestCase( "P:Part1", null, "P:Part1" )]
		[TestCase( "P:Part1", Root, "P:Part1" )]
		[TestCase( "P:Part1", "P:Part1", Root )]
		[TestCase( "P:Part1", "P:Part2", "P:Part1" )]
		[TestCase( "P:Part1;P:Part2", "P:Part1", "P:Part2" )]
		[TestCase( "P:Part1;P:Part2;C:Char1", "P:Part1", "P:Part2;C:Char1" )]
		[TestCase( "P:Part1;P:Part2;C:Char1", "P:Part1;P:Part3", "P:Part2;C:Char1" )]
		public void Should_Provide_Relative_Path( string pathString, string basePathString, string expectedPathString )
		{
			//***** arrange *****
			var path = CreatePath( pathString );
			var basePath = CreatePath( basePathString );
			var expectedPath = CreatePath( expectedPathString );

			//***** act *****
			var result = path.RelativeTo( basePath );

			//***** assert *****
			Assert.That( result, Is.EqualTo( expectedPath ) );
		}

		#endregion
	}
}