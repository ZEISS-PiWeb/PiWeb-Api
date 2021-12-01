#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net.Http;
	using System.Threading.Tasks;
	using CacheCow.Client;
	using CacheCow.Common;

	#endregion

	/// <summary>
	/// Implements <see cref="ICacheStore"/> and stores the cache in a filesystem directory, managed by the operating system.
	/// </summary>
	public class FilesystemCacheStore : ICacheStore
	{
		#region constants

		private const string CacheFileExtension = ".piweb~cache";

		#endregion

		#region members

		private readonly MessageContentHttpMessageSerializer _HttpMessageSerializer = new( true );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FilesystemCacheStore"/> class
		/// and uses a default path based on the temporary directory as cache store.
		/// </summary>
		public FilesystemCacheStore()
		{
			StoragePath = Path.Combine( Path.GetTempPath(), "Cache" );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilesystemCacheStore"/> class
		/// and uses a specified path as cache store.
		/// </summary>
		/// <param name="path">
		/// The path of the directory that is used as cache store.
		/// The directory should be empty or contain only cached files.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="path"/> is invalid or not accessible.</exception>
		public FilesystemCacheStore( string path )
		{
			if( path == null )
				throw new ArgumentNullException( nameof( path ) );

			try
			{
				Directory.CreateDirectory( path );
				StoragePath = path;
			}
			catch( Exception exception )
			{
				throw new ArgumentException( @$"Failed to use the path '{path}' as cache store.", nameof( path ), exception );
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the path of the directory that is used as cache store.
		/// </summary>
		public string StoragePath { get; }

		#endregion

		#region methods

		private string GetCacheFileFullName( CacheKey key )
		{
			var hash = key.HashBase64.Replace( '/', '-' );
			return Path.Combine( StoragePath, hash + CacheFileExtension );
		}

		#endregion

		#region interface ICacheStore

		/// <inheritdoc/>
		public async Task<HttpResponseMessage> GetValueAsync( CacheKey key )
		{
			try
			{
				using var stream = File.Open( GetCacheFileFullName( key ), FileMode.Open, FileAccess.Read, FileShare.Read );
				return await _HttpMessageSerializer.DeserializeToResponseAsync( stream ).ConfigureAwait( false );
			}
			catch
			{
				// no matter what happened, pretend it is not cached
				return null;
			}
		}

		/// <inheritdoc/>
		public async Task AddOrUpdateAsync( CacheKey key, HttpResponseMessage response )
		{
			try
			{
				using var stream = File.Open( GetCacheFileFullName( key ), FileMode.Create, FileAccess.Write, FileShare.None );
				await _HttpMessageSerializer.SerializeAsync( response, stream ).ConfigureAwait( false );
			}
			catch
			{
				// nothing to do, don't cache if not possible
			}
		}

		/// <inheritdoc/>
		public Task<bool> TryRemoveAsync( CacheKey key )
		{
			try
			{
				var fileFullName = GetCacheFileFullName( key );

				if( !File.Exists( fileFullName ) )
					return Task.FromResult( false );

				File.Delete( fileFullName );
				return Task.FromResult( true );
			}
			catch
			{
				return Task.FromResult( false );
			}
		}

		/// <inheritdoc/>
		public Task ClearAsync()
		{
			IEnumerable<string> files;
			try
			{
				files = Directory.EnumerateFiles( StoragePath, "*" + CacheFileExtension, SearchOption.TopDirectoryOnly );
			}
			catch
			{
				return Task.CompletedTask;
			}

			foreach( var file in files )
			{
				try
				{
					File.Delete( file );
				}
				catch
				{
					// nothing to do, don't delete if not possible
				}
			}

			return Task.CompletedTask;
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			GC.SuppressFinalize( this );
		}

		#endregion
	}
}