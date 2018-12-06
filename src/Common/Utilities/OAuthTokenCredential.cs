#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;

	#endregion

	public sealed class OAuthTokenCredential : ICredential
	{

		#region constructors

		public OAuthTokenCredential( string displayId, string accessToken, DateTime accessTokenExpiration, string refreshToken )
		{
			DisplayId = displayId;
			AccessToken = accessToken;
			AccessTokenExpiration = accessTokenExpiration;
			RefreshToken = refreshToken;
			MailAddress = OAuthHelper.TokenToMailAddress( AccessToken );
		}

		#endregion

		#region properties

		public string DisplayId { get; }

		public string AccessToken { get; }

		public DateTime AccessTokenExpiration { get; }

		public string RefreshToken { get; }

		public string MailAddress { get; }

		#endregion

		#region methods

		public static OAuthTokenCredential CreateWithIdentityToken( string identityToken, string accessToken, DateTime accessTokenExpiration, string refreshToken )
		{
			return new OAuthTokenCredential( OAuthHelper.TokenToFriendlyText( identityToken ), accessToken, accessTokenExpiration, refreshToken );
		}

		public static OAuthTokenCredential CreateWithClaims( IEnumerable<Tuple<string, string>> claims, string accessToken, DateTime accessTokenExpiration, string refreshToken )
		{
			return new OAuthTokenCredential( OAuthHelper.IdentityClaimsToFriendlyText( claims.ToList() ), accessToken, accessTokenExpiration, refreshToken );
		}

		#endregion

		public bool Equals( OAuthTokenCredential other )
		{
			return other != null
				&& string.Equals( DisplayId, other.DisplayId ) 
				&& string.Equals( AccessToken, other.AccessToken ) 
				&& AccessTokenExpiration.Equals( other.AccessTokenExpiration ) 
				&& string.Equals( RefreshToken, other.RefreshToken ) 
				&& string.Equals( MailAddress, other.MailAddress );
		}

		public bool Equals( ICredential other ) => Equals( other as OAuthTokenCredential );

		public override bool Equals( object other ) => Equals( other as OAuthTokenCredential );

		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = ( DisplayId != null ? DisplayId.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ ( AccessToken != null ? AccessToken.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ AccessTokenExpiration.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ ( RefreshToken != null ? RefreshToken.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ ( MailAddress != null ? MailAddress.GetHashCode() : 0 );
				return hashCode;
			}
		}
	}
}