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

	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Core;

	#endregion

	[TestFixture]
	public class PathInformationTest
	{
		#region methods

		[Test]
		public void Test_OnePath_IsBelow_AnotherPath()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "P1" ) );
			var path2 = new PathInformation( PathElement.Part( "P1" ), PathElement.Char( "C" ) );

			// act, assert
			Assert.That( path2.IsBelow( path1 ), Is.True );
			Assert.That( path1.IsBelow( path2 ), Is.False );
		}

		[Test]
		public void Test_SamePath_IsBelow_ReturnsTrue()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Char( "C1" ), PathElement.Char( "X" ) );
			var path2 = new PathInformation( PathElement.Char( "C1" ), PathElement.Char( "X" ) );

			// act, assert
			Assert.That( path1.IsBelow( path2 ), Is.True );
		}

		[Test]
		public void Test_ComplexPath_IsBelow_AnotherPath()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "P1" ), PathElement.Char( "C" ), PathElement.Char( "X" ) );
			var path2 = new PathInformation( PathElement.Part( "P1" ), PathElement.Char( "C" ), PathElement.Char( "Y" ) );

			// act, assert
			Assert.That( path1.IsBelow( path2 ), Is.False );
			Assert.That( path1.ParentPath.IsBelow( path2 ), Is.False );
			Assert.That( path1.ParentPartPath.IsBelow( path2.StartPath( 1 ) ), Is.True );
		}

		[Test]
		public void Test_RootPath_SpecialCase_IsHandled()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "P1" ), PathElement.Char( "C" ), PathElement.Char( "X" ) );

			// act, assert
			Assert.That( path1.IsBelow( PathInformation.Root ), Is.True );
			Assert.That( PathInformation.Root.IsBelow( path1 ), Is.False );
			Assert.That( path1.ParentPath.IsBelow( PathInformation.Root ), Is.True );
		}

		[Test]
		public void Test_Equals_SamePaths_ReturnsTrue()
		{
			// arrange
			var path = new PathInformation( PathElement.Part( "A" ) );

			// act, assert
			Assert.That( path.Equals( path ), Is.True );
		}

		[Test]
		public void Test_Equals_PathWithNull_ReturnsFalse()
		{
			// arrange
			var path = new PathInformation( PathElement.Part( "A" ) );

			// act, assert
			Assert.That( path.Equals( null ), Is.False );
		}

		[Test]
		public void Test_Equals_EmptyPaths_ReturnsTrue()
		{
			// arrange
			var path1 = new PathInformation();
			var path2 = new PathInformation();

			// act, assert
			Assert.That( path1.Equals( path2 ), Is.True );
		}

		[Test]
		public void Test_Equals_PathsWithDifferentLength_ReturnsFalse()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "A" ), PathElement.Part( "B" ) );
			var path2 = new PathInformation( PathElement.Part( "A" ) );

			// act, assert
			Assert.That( path1.Equals( path2 ), Is.False );
		}

		[Test]
		public void Test_Equals_PathsNotEqual_ReturnsFalse()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "A" ) );
			var path2 = new PathInformation( PathElement.Part( "B" ) );

			// act, assert
			Assert.That( path1.Equals( path2 ), Is.False );
		}

		[Test]
		public void Test_Equals_PathsEqual_ReturnsTrue()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "A" ) );
			var path2 = new PathInformation( PathElement.Part( "A" ) );

			// act, assert
			Assert.That( path1.Equals( path2 ), Is.True );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Paths_Have_Different_Length()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "A" ), PathElement.Part( "B" ) );
			var path2 = new PathInformation( PathElement.Part( "A" ) );

			// act
			var hashCode1 = path1.GetHashCode();
			var hashCode2 = path2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsDifferent_When_Paths_Are_Not_Equal()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "A" ) );
			var path2 = new PathInformation( PathElement.Part( "B" ) );

			// act
			var hashCode1 = path1.GetHashCode();
			var hashCode2 = path2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.Not.EqualTo( hashCode2 ) );
		}

		[Test]
		public void Test_GetHashCode_IsSame_When_Paths_Are_Equal()
		{
			// arrange
			var path1 = new PathInformation( PathElement.Part( "A" ) );
			var path2 = new PathInformation( PathElement.Part( "A" ) );

			// act
			var hashCode1 = path1.GetHashCode();
			var hashCode2 = path2.GetHashCode();

			// assert
			Assert.That( hashCode1, Is.EqualTo( hashCode2 ) );
		}

		#endregion
	}
}