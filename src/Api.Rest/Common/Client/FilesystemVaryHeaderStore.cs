#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2022                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using CacheCow.Client;

	#endregion

	/// <summary>
	/// Implements <see cref="IVaryHeaderStore"/> and stores the vary-headers in a filesystem directory, managed by the operating system.
	/// </summary>
	public class FilesystemVaryHeaderStore : IVaryHeaderStore
	{
		#region constants

		private const string HeaderFileExtension = ".piweb~header";

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FilesystemVaryHeaderStore"/> class
		/// and uses a default path based on the temporary directory as header store.
		/// </summary>
		public FilesystemVaryHeaderStore()
		{
			StoragePath = Path.Combine( Path.GetTempPath(), "PiWeb_Cache" );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilesystemVaryHeaderStore"/> class
		/// and uses a specified path as header store.
		/// </summary>
		/// <param name="path">
		/// The path of the directory that is used as header store.
		/// The directory should be empty or contain only header files.
		/// If the directory does not exist, it will be created.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="path"/> is invalid or not accessible.</exception>
		public FilesystemVaryHeaderStore( string path )
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
				throw new ArgumentException( @$"Failed to use the path '{path}' as header store.", nameof( path ), exception );
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets the path of the directory that is used as header store.
		/// </summary>
		public string StoragePath { get; }

		#endregion

		#region methods

		private string GetHeaderFileFullName( string uri )
		{
			using var sha1 = SHA1.Create();
			var hashBytes = sha1.ComputeHash( Encoding.UTF8.GetBytes( uri ) );
			var hashBase64 = Convert.ToBase64String( hashBytes ).Replace( '/', '-' );

			return Path.Combine( StoragePath, hashBase64 + HeaderFileExtension );
		}

		#endregion

		#region interface IVaryHeaderStore

		/// <inheritdoc/>
		public bool TryGetValue( string uri, out IEnumerable<string> headers )
		{
			try
			{
				headers = File.ReadAllLines( GetHeaderFileFullName( uri ) );
				return true;
			}
			catch
			{
				// no matter what happened, pretend it is not stored
				headers = null;
				return false;
			}
		}

		/// <inheritdoc/>
		public void AddOrUpdate( string uri, IEnumerable<string> headers )
		{
			try
			{
				File.WriteAllLines( GetHeaderFileFullName( uri ), headers );
			}
			catch
			{
				// nothing to do, don't store if not possible
			}
		}

		/// <inheritdoc/>
		public bool TryRemove( string uri )
		{
			try
			{
				var fileFullName = GetHeaderFileFullName( uri );

				if( !File.Exists( fileFullName ) )
					return false;

				File.Delete( fileFullName );
				return true;
			}
			catch
			{
				return false;
			}
		}

		/// <inheritdoc/>
		public void Clear()
		{
			IEnumerable<string> files;
			try
			{
				files = Directory.EnumerateFiles( StoragePath, "*" + HeaderFileExtension, SearchOption.TopDirectoryOnly );
			}
			catch
			{
				return;
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
		}

		/// <inheritdoc/>
		public void Dispose()
		{
			GC.SuppressFinalize( this );
		}

		#endregion
	}
}