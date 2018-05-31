#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2012                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Utilities
{
	#region using

	using System;
	using System.IO;
	using System.Security.Cryptography;

	using Microsoft.IdentityModel.Clients.ActiveDirectory;

	#endregion

	/// <summary>
	/// Remember OAuth tokens in an obfuscated file. This way new tokens can be aquired with the help of the saved refresh token.
	/// </summary>
	public class FileOAuthTokenCache : TokenCache
	{
		#region members

		private static readonly object FileLock = new object();
		private readonly string _CacheFilePath = Path.Combine( Environment.GetFolderPath( System.Environment.SpecialFolder.ApplicationData ), @"Zeiss\PiWeb\TokenCache.dat" );
		private DateTime _FileLastWriteTime;
		private long _FileLength;

		#endregion

		#region constructor

		/// <summary>
		/// Create a new TokenCache instance.
		/// </summary>
		public FileOAuthTokenCache()
		{
			AfterAccess = AfterAccessNotification;
			BeforeAccess = BeforeAccessNotification;
		}

		#endregion

		#region methods

		/// <summary>
		/// Empties the persistent store.
		/// </summary>
		public override void Clear()
		{
			base.Clear();
			File.Delete( _CacheFilePath );
		}

		/// <summary>
		/// Triggered right before Active Directory Authentication Library needs to access the cache. Reload the cache from the persistent store in case it changed since the last access.
		/// </summary>
		private void BeforeAccessNotification( TokenCacheNotificationArgs args )
		{
			var fileInfo = new FileInfo( _CacheFilePath );
			if( fileInfo.Exists )
			{
				var fileChanged = ( _FileLastWriteTime != fileInfo.LastWriteTimeUtc ) || ( _FileLength != fileInfo.Length );

				if( fileChanged )
				{
					lock( FileLock )
					{
						_FileLastWriteTime = fileInfo.LastWriteTimeUtc;
						_FileLength = fileInfo.Length;

						Deserialize( ProtectedData.Unprotect( File.ReadAllBytes( _CacheFilePath ), null, DataProtectionScope.CurrentUser ) );
					}
				}
			}
			else
			{
				_FileLastWriteTime = default( DateTime );
				_FileLength = 0;

				Deserialize( null );
			}
		}

		/// <summary>
		/// Triggered right after Active Directory Authentication Library accessed the cache.
		/// </summary>
		private void AfterAccessNotification( TokenCacheNotificationArgs args )
		{
			// if the access operation resulted in a cache update
			if( !HasStateChanged ) return;

			lock( FileLock )
			{
				// reflect changes in the persistent store
				File.WriteAllBytes( _CacheFilePath, ProtectedData.Protect( Serialize(), null, DataProtectionScope.CurrentUser ) );

				// once the write operation took place, restore the HasStateChanged bit to false
				HasStateChanged = false;
			}
		}

		#endregion
	}
}