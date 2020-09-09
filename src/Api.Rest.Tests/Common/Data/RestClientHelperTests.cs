#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.Common.Data
{
	#region usings

	using System;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Common.Data;

	#endregion

	[TestFixture]
	public class RestClientHelperTests
	{
		#region methods

		[Test]
		public void ConvertStringToUInt16List_StringWithoutSpaces_ReturnsArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{1,2,3}" );

			Assert.AreEqual( 3, result.Length );
		}

		[Test]
		public void ConvertStringToUInt16List_StringWithSpaces_ReturnsArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{ 1, 2, 3 }" );

			Assert.AreEqual( 3, result.Length );
		}

		[Test]
		public void ConvertStringToUInt16List_HappyPath_ReturnsArrayInOrder()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{5,2,9}" );

			Assert.AreEqual( 5, result[ 0 ] );
			Assert.AreEqual( 2, result[ 1 ] );
			Assert.AreEqual( 9, result[ 2 ] );
		}

		[Test]
		public void ConvertStringToUInt16List_StringNull_ReturnsEmptyArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( null );

			Assert.NotNull( result );
			Assert.IsEmpty( result );
		}

		[Test]
		public void ConvertStringToUInt16List_StringEmpty_ReturnsEmptyArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "" );

			Assert.NotNull( result );
			Assert.IsEmpty( result );
		}

		[Test]
		public void ConvertStringToUInt16List_EmptyList_ReturnsEmptyArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{}" );

			Assert.NotNull( result );
			Assert.IsEmpty( result );
		}

		[Test]
		public void ConvertStringToUInt16List_InvalidString_ExceptionThrown()
		{
			Assert.Throws<FormatException>( () => RestClientHelper.ConvertStringToUInt16List( "Invalid" ) );
		}

		#endregion
	}
}