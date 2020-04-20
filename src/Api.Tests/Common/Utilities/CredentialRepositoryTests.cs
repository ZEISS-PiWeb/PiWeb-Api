#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Tests.Common.Utilities
{
	#region usings

	using System;
	using System.Diagnostics.CodeAnalysis;
	using System.IO;
	using FluentAssertions;
	using NUnit.Framework;
	using Zeiss.PiWeb.Api.Common.Utilities;

	#endregion

	[TestFixture]
	public class CredentialRepositoryTests
	{
		#region methods

		[Test]
		[SuppressMessage( "ReSharper", "ObjectCreationAsStatement" )]
		public void Construction_FileNameEmpty_ThrowsException()
		{
			// When
			Action ctor = () => new CredentialRepository( string.Empty );

			// Then
			ctor.Should().Throw<ArgumentException>();
		}

		[Test]
		public void Remove_ExistingItem_ItemRemoved()
		{
			var url = "http://portal.azure.com";
			using var temp = new TempFile();

			// Given
			var sut = new CredentialRepository( temp.Location );
			sut.Store( url, new OAuthTokenCredential( "[DisplayId]", "[AccessToken]", DateTime.UtcNow, "[RefreshToken]" ) );

			// When
			sut.Remove( url );

			// Then
			sut.TryGetCredential( url, out _ ).Should().BeFalse();
		}

		[Test]
		public void Remove_NoCredentialsStoredForUrl_NothingHappen()
		{
			// Given
			using var temp = new TempFile();
			var sut = new CredentialRepository( temp.Location );

			// When
			var removeAction = sut.Invoking( s => s.Remove( "http://portal.azure.com" ) );

			// Then
			removeAction.Should().NotThrow();
		}

		[Test]
		[SuppressMessage( "ReSharper", "AssignNullToNotNullAttribute" )]
		public void Store_CredentialNull_ThrowsException()
		{
			// Given
			using var temp = new TempFile();
			var sut = new CredentialRepository( temp.Location );

			// When
			var storeAction = sut.Invoking( s => s.Store( "http://portal.azure.com", null ) );

			// Then
			storeAction.Should().Throw<ArgumentNullException>();
		}

		[Test]
		public void Store_HappyPath_StorageFileCreated()
		{
			// Given
			using var temp = new TempFile( false );
			var sut = new CredentialRepository( temp.Location );

			// When
			var credential = new OAuthTokenCredential( "[DisplayId]", "[AccessToken]", DateTime.UtcNow, "[RefreshToken]" );
			sut.Store( "http://portal.azure.com", credential );

			// Then
			File.Exists( temp.Location ).Should().BeTrue();
		}

		[Test]
		public void Store_UpdateExisting_ExistingItemOverwritten()
		{
			using var temp = new TempFile();
			var url = "http://portal.azure.com";

			// Given
			var sut = new CredentialRepository( temp.Location );
			sut.Store( url, new OAuthTokenCredential( "[DisplayId]", "[AccessToken]", DateTime.UtcNow, "[RefreshToken]" ) );

			// When
			var expected = new OAuthTokenCredential( "[DisplayId_1]", "[AccessToken_1]", DateTime.UtcNow, "[RefreshToken_1]" );
			sut.Store( url, expected );

			// Then
			sut.TryGetCredential( url, out var actual );
			actual.Should().BeEquivalentTo( expected );
		}

		[Test]
		public void TryGetCredential_DataPreviouslyStored_ReturnsExpectedCredentials()
		{
			using var temp = new TempFile();
			var expected = new OAuthTokenCredential( "[DisplayId]", "[AccessToken]", DateTime.UtcNow, "[RefreshToken]" );
			var url = "http://portal.azure.com";

			// Given
			var sut = new CredentialRepository( temp.Location );
			sut.Store( url, expected );

			// When
			sut.TryGetCredential( url, out var actual );

			// Then
			actual.Should().BeEquivalentTo( expected );
		}

		[Test]
		public void TryGetCredential_DataPreviouslyStored_ReturnsTrue()
		{
			using var temp = new TempFile();
			var url = "http://portal.azure.com";

			// Given
			var storingSut = new CredentialRepository( temp.Location );
			storingSut.Store( url, new OAuthTokenCredential( "[DisplayId]", "[AccessToken]", DateTime.UtcNow, "[RefreshToken]" ) );
			var readingSut = new CredentialRepository( temp.Location );

			// When
			var actual = readingSut.TryGetCredential( url, out _ );

			// Then
			actual.Should().BeTrue();
		}

		[Test]
		public void TryGetCredentials_StorageExistsButEmpty_ReturnsFalse()
		{
			// Given
			using var temp = new TempFile();
			var sut = new CredentialRepository( temp.Location );

			// When
			var actual = sut.TryGetCredential( "http://portal.azure.com", out _ );

			// Then
			actual.Should().BeFalse();
		}

		[Test]
		public void TryGetCredentials_StorageLocationDoesNotExists_ReturnsFalse()
		{
			// Given
			var location = Path.GetTempPath() + Guid.NewGuid() + ".dat";
			var sut = new CredentialRepository( location );

			// When
			var actual = sut.TryGetCredential( "http://portal.azure.com", out _ );

			// Then
			actual.Should().BeFalse();
		}

		#endregion

		#region class TempFile

		// Helper class to ensure that temporary files will be deleted
		private class TempFile : IDisposable
		{
			#region constructors

			public TempFile( bool create = true )
			{
				Location = Path.GetTempFileName();
				if( !create ) File.Delete( Location );
			}

			#endregion

			#region properties

			public string Location { get; }

			#endregion

			#region interface IDisposable

			public void Dispose()
			{
				try
				{
					if( File.Exists( Location ) ) File.Delete( Location );
				}
				catch
				{
					// Nothing to do, here
				}
			}

			#endregion
		}

		#endregion
	}
}