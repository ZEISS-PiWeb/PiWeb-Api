#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.Contracts
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	[TestFixture]
	public class FeatureMatrixTest
	{
		#region methods

		[Test]
		[TestCase( "1.3", new[] { 1 }, "1.3" )]
		[TestCase( "1.3, 1.5, 2.0, 2.2", new[] { 1 }, "1.5" )]
		[TestCase( "1.3, 1.5, 2.0, 2.2", new[] { 1, 2 }, "2.2" )]
		[TestCase( "1.3, 1.5, 2.0, 2.2", new[] { 2 }, "2.2" )]
		public void Test_GetBestKnownVersion_ReturnsCorrectVersion( string interfaceVersions, int[] supportedMajorVersions, string expectedBestKnownVersion )
		{
			// Given
			var interfaceVersionRange = new InterfaceVersionRange { SupportedVersions = ParseVersions( interfaceVersions ) };
			var expectedVersion = new Version( expectedBestKnownVersion );

			// When
			var bestKnownVersion = FeatureMatrix.GetBestKnownVersion( interfaceVersionRange, supportedMajorVersions );

			// Then
			bestKnownVersion.Should().Be( expectedVersion );
		}

		[Test]
		[TestCase( "1.3", new[] { 2 } )]
		[TestCase( "2.0, 2.2", new[] { 1 } )]
		public void Test_GetBestKnownVersion_ThrowsServerApiNotSupportedException( string interfaceVersions, int[] supportedMajorVersions )
		{
			// Given
			var interfaceVersionRange = new InterfaceVersionRange { SupportedVersions = ParseVersions( interfaceVersions ) };

			// When
			Action getBestKnownVersion = () => FeatureMatrix.GetBestKnownVersion( interfaceVersionRange, supportedMajorVersions );

			// Then
			getBestKnownVersion.Should().Throw<ServerApiNotSupportedException>();
		}

		private static IEnumerable<Version> ParseVersions( string versionsString )
		{
			var versions = string.IsNullOrEmpty( versionsString )
				? Enumerable.Empty<Version>()
				: versionsString.Split( ',' )
					.Select( versionString => new Version( versionString.Trim() ) );

			return versions;
		}

		#endregion
	}
}