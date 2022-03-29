#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Concurrent;
	using System.Collections.Generic;
	using System.IO;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading;
	using JetBrains.Annotations;
	using Newtonsoft.Json;

	#endregion

	internal sealed class CredentialRepository
	{
		#region members

		private readonly string _Filename;
		private readonly string _Directory;

		private readonly ConcurrentDictionary<string, OAuthTokenCredential> _CredentialCache = new ConcurrentDictionary<string, OAuthTokenCredential>();
		private DateTime _FileLastWriteTime;
		private long _FileLength;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CredentialRepository"/> class.
		/// </summary>
		/// <exception cref="ArgumentException"><paramref name="filename"/> is <see langword="null" /> or whitespace.</exception>
		public CredentialRepository( string filename )
		{
			if( string.IsNullOrWhiteSpace( filename ) )
			{
				throw new ArgumentException( @"Missing storage location", nameof( filename ) );
			}

			_Filename = filename;
			_Directory = Directory.GetParent( _Filename ).FullName;
		}

		#endregion

		#region methods

		public bool TryGetCredential( string url, out OAuthTokenCredential credential )
		{
			ReadFromFile();

			return _CredentialCache.TryGetValue( url, out credential );
		}

		/// <exception cref="ArgumentException"><paramref name="credential"/> is <see langword="null" />.</exception>
		public void Store( string url, [NotNull] OAuthTokenCredential credential )
		{
			if( credential == null )
			{
				throw new ArgumentNullException( nameof( credential ) );
			}

			_CredentialCache.AddOrUpdate( url, key => credential, ( key, value ) => credential );

			SaveToFile();
		}

		public void Remove( string url )
		{
			if( _CredentialCache.TryRemove( url, out _ ) )
			{
				SaveToFile();
			}
		}

		public void Clear()
		{
			if( _CredentialCache.Count > 0 )
			{
				_CredentialCache.Clear();
				SaveToFile();
			}
		}

		public void SaveToFile()
		{
			if( !Environment.UserInteractive )
				return;

			if( !Directory.Exists( _Directory ) )
				Directory.CreateDirectory( _Directory );

			var serialized = JsonConvert.SerializeObject( _CredentialCache );
			var bytes = Encoding.UTF8.GetBytes( serialized );
			bytes = ProtectedData.Protect( bytes, null, DataProtectionScope.CurrentUser );

			using( var stream = WaitForFileStream( _Filename, FileMode.Create, FileAccess.Write, FileShare.None ) )
			{
				stream.Write( bytes, 0, bytes.Length );
			}

			var fileInfo = new FileInfo( _Filename );
			_FileLastWriteTime = fileInfo.LastWriteTimeUtc;
			_FileLength = fileInfo.Length;
		}

		private void ReadFromFile()
		{
			if( !Environment.UserInteractive )
				return;

			var fileInfo = new FileInfo( _Filename );
			if( fileInfo.Exists )
			{
				var fileChanged = ( _FileLastWriteTime != fileInfo.LastWriteTimeUtc ) || ( _FileLength != fileInfo.Length );

				if( fileChanged )
				{
					using( var memStream = new MemoryStream() )
					{
						using( var stream = WaitForFileStream( _Filename, FileMode.Open, FileAccess.Read, FileShare.Read ) )
						{
							stream.CopyTo( memStream );
							if( stream.Position == 0 )
							{
								// File is empty
								_FileLastWriteTime = default;
								_FileLength = 0;
								return;
							}
						}

						var bytes = ProtectedData.Unprotect( memStream.ToArray(), null, DataProtectionScope.CurrentUser );
						var serialized = Encoding.UTF8.GetString( bytes );

						var deserialized = JsonConvert.DeserializeObject<Dictionary<string, OAuthTokenCredential>>( serialized );

						_CredentialCache.Clear();
						foreach( var entry in deserialized )
						{
							_CredentialCache.AddOrUpdate( entry.Key, key => entry.Value, ( key, value ) => entry.Value );
						}

						_FileLastWriteTime = fileInfo.LastWriteTimeUtc;
						_FileLength = fileInfo.Length;
					}
				}
			}
			else
			{
				_FileLastWriteTime = default;
				_FileLength = 0;
			}
		}

		/// <summary>
		/// Inspired by https://stackoverflow.com/a/3677960
		/// </summary>
		private static FileStream WaitForFileStream( string path, FileMode mode, FileAccess access, FileShare share )
		{
			const int numTries = 10;
			var i = 0;
			while( true )
			{
				FileStream fileStream = null;
				try
				{
					fileStream = new FileStream( path, mode, access, share );
					return fileStream;
				}
				catch( IOException )
				{
					fileStream?.Dispose();

					if( i >= numTries )
						throw;

					Thread.Sleep( 50 );

					i += 1;
				}
			}
		}

		#endregion
	}
}