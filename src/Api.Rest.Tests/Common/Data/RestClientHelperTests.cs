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
	using System.Linq;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Dtos;

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

		[Test]
		public void SplitAndMergePathsParameters_ServiceLocationIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( null, "test", 200, "paramName", new string[]{}, new ParameterDefinition[]{} ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_EmptyParameterName_ExceptionThrown()
		{
			Assert.Throws<ArgumentException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "", 200, "paramName", new string[]{}, new ParameterDefinition[]{} ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_PathsToSplitIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", pathsToSplit: null, new ParameterDefinition[]{} ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_OtherParametersArrayIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", new string[]{}, null ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_HappyPath()
		{
			var pathsToSplits = new []
			{
				"Test_Part_1",
				"Test_Part_2",
				"Test_Part_3",
				"Test_Part_4",
				"Test_Part_5",
				"Test_Part_6"
			};
			var otherParams = new []
			{
				ParameterDefinition.Create( "param", "foo" )
			};

			var paramSets = RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "testEndpoint", 90, "testParam", pathsToSplits, otherParams );

			Assert.AreEqual( 3, paramSets.Count() );
		}

		[Test]
		public void SplitAndMergeUuidParameters_ServiceLocationIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( null, "test", 200, "paramName", new string[]{}, new ParameterDefinition[]{} ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_EmptyParameterName_ExceptionThrown()
		{
			Assert.Throws<ArgumentException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "", 200, "paramName", new Guid[]{}, new ParameterDefinition[]{} ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_PathsToSplitIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", uuidsToSplit: null, new ParameterDefinition[]{} ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_OtherParametersArrayIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", new Guid[]{}, null ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_HappyPath()
		{
			var uuidsToSplits = new []
			{
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid()
			};
			var otherParams = new []
			{
				ParameterDefinition.Create( "param", "foo" )
			};

			var paramSets = RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "testEndpoint", 70, "testParam", uuidsToSplits, otherParams );

			Assert.AreEqual( 4, paramSets.Count() );
		}

		#endregion
	}
}