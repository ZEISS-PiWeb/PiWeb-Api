#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.Common.Data
{
	#region usings

	using System;
	using System.Linq;
	using FluentAssertions;
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

			result.Length.Should().Be( 3 );
		}

		[Test]
		public void ConvertStringToUInt16List_StringWithSpaces_ReturnsArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{ 1, 2, 3 }" );

			result.Length.Should().Be( 3 );
		}

		[Test]
		public void ConvertStringToUInt16List_HappyPath_ReturnsArrayInOrder()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{5,2,9}" );

			result[ 0 ].Should().Be( 5 );
			result[ 1 ].Should().Be( 2 );
			result[ 2 ].Should().Be( 9 );
		}

		[Test]
		public void ConvertStringToUInt16List_StringNull_ReturnsEmptyArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( null );

			result.Should().NotBeNull();
			result.Should().BeEmpty();
		}

		[Test]
		public void ConvertStringToUInt16List_StringEmpty_ReturnsEmptyArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "" );

			result.Should().NotBeNull();
			result.Should().BeEmpty();
		}

		[Test]
		public void ConvertStringToUInt16List_EmptyList_ReturnsEmptyArray()
		{
			var result = RestClientHelper.ConvertStringToUInt16List( "{}" );

			result.Should().NotBeNull();
			result.Should().BeEmpty();
		}

		[Test]
		public void ConvertStringToUInt16List_InvalidString_ExceptionThrown()
		{
			Assert.Throws<FormatException>( () => RestClientHelper.ConvertStringToUInt16List( "Invalid" ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_ServiceLocationIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( null, "test", 200, "paramName", [], [] ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_EmptyParameterName_ExceptionThrown()
		{
			Assert.Throws<ArgumentException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "", 200, "paramName", [], [] ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_PathsToSplitIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", pathsToSplit: null, [] ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_OtherParametersArrayIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", [], null ) );
		}

		[Test]
		public void SplitAndMergePathsParameters_HappyPath()
		{
			var pathsToSplits = new[]
			{
				"Test_Part_1",
				"Test_Part_2",
				"Test_Part_3",
				"Test_Part_4",
				"Test_Part_5",
				"Test_Part_6"
			};
			var otherParams = new[]
			{
				ParameterDefinition.Create( "param", "foo" )
			};

			var paramSets = RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "testEndpoint", 90, "testParam", pathsToSplits, otherParams );

			paramSets.Count().Should().Be( 3 );
		}

		[Test]
		public void SplitAndMergeUuidParameters_ServiceLocationIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( null, "test", 200, "paramName", [], [] ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_EmptyParameterName_ExceptionThrown()
		{
			Assert.Throws<ArgumentException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "", 200, "paramName", Array.Empty<Guid>(), [] ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_PathsToSplitIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", uuidsToSplit: null, [] ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_OtherParametersArrayIsNull_ExceptionThrown()
		{
			Assert.Throws<ArgumentNullException>( () => RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "test", 200, "paramName", new Guid[] { }, null ) );
		}

		[Test]
		public void SplitAndMergeUuidParameters_HappyPath()
		{
			var uuidsToSplits = new[]
			{
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid(),
				Guid.NewGuid()
			};
			var otherParams = new[]
			{
				ParameterDefinition.Create( "param", "foo" )
			};

			var paramSets = RestClientHelper.SplitAndMergeParameters( new Uri( "http://localhost" ), "testEndpoint", 70, "testParam", uuidsToSplits, otherParams );

			paramSets.Count().Should().Be( 4 );
		}

		#endregion
	}
}