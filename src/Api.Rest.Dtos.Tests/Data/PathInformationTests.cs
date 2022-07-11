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
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	[TestFixture]
	public class PathInformationTests
	{
		#region methods

		[Test]
		public void Test_OnePath_IsBelow_AnotherPath()
		{
			var path1 = new PathInformationDto( PathElementDto.Part( "P1" ) );
			var path2 = new PathInformationDto( PathElementDto.Part( "P1" ), PathElementDto.Char( "C" ) );

			Assert.That( path2.IsBelow( path1 ), Is.True );
			Assert.That( path1.IsBelow( path2 ), Is.False );
		}

		[Test]
		public void Test_SamePath_IsBelow_ReturnsTrue()
		{
			var path1 = new PathInformationDto( PathElementDto.Char( "C1" ), PathElementDto.Char( "X" ) );
			var path2 = new PathInformationDto( PathElementDto.Char( "C1" ), PathElementDto.Char( "X" ) );

			Assert.That( path1.IsBelow( path2 ), Is.True );
		}

		[Test]
		public void Test_ComplexPath_IsBelow_AnotherPath()
		{
			var path1 = new PathInformationDto( PathElementDto.Part( "P1" ), PathElementDto.Char( "C" ), PathElementDto.Char( "X" ) );
			var path2 = new PathInformationDto( PathElementDto.Part( "P1" ), PathElementDto.Char( "C" ), PathElementDto.Char( "Y" ) );

			Assert.That( path1.IsBelow( path2 ), Is.False );
			Assert.That( path1.ParentPath.IsBelow( path2 ), Is.False );
			Assert.That( path1.ParentPartPath.IsBelow( path2.StartPath( 1 ) ), Is.True );
		}

		[Test]
		public void Test_RootPath_SpecialCase_IsHandled()
		{
			var path1 = new PathInformationDto( PathElementDto.Part( "P1" ), PathElementDto.Char( "C" ), PathElementDto.Char( "X" ) );

			Assert.That( path1.IsBelow( PathInformationDto.Root ), Is.True );
			Assert.That( PathInformationDto.Root.IsBelow( path1 ), Is.False );
			Assert.That( path1.ParentPath.IsBelow( PathInformationDto.Root ), Is.True );
		}

		#endregion
	}
}