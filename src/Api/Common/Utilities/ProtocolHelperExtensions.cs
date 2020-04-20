#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Common.Utilities
{
	#region usings

	using System;
	using System.Security.Cryptography;
	using System.Text;
	using IdentityModel;

	#endregion

	/// <remarks>
	/// The following code is originated from an earlier version of the IdentityModel nuget package.
	/// </remarks>
	internal static class ProtocolHelperExtensions
	{
		#region methods

		public static string ToCodeChallenge( this string input )
		{
			return input.ToSha256( HashStringEncoding.Base64Url );
		}

		private static string ToSha256( this string input, HashStringEncoding encoding = HashStringEncoding.Base64 )
		{
			if( string.IsNullOrWhiteSpace( input ) )
			{
				return string.Empty;
			}

			using( var sha256 = SHA256.Create() )
			{
				var bytes = Encoding.ASCII.GetBytes( input );
				return Encode( sha256.ComputeHash( bytes ), encoding );
			}
		}

		private static string Encode( byte[] hash, HashStringEncoding encoding )
		{
			switch( encoding )
			{
				case HashStringEncoding.Base64:
					return Convert.ToBase64String( hash );

				case HashStringEncoding.Base64Url:
					return Base64Url.Encode( hash );

				default:
					throw new ArgumentException( "Invalid encoding" );
			}
		}

		#endregion

		#region class HashStringEncoding

		private enum HashStringEncoding
		{
			Base64,
			Base64Url,
		}

		#endregion
	}
}