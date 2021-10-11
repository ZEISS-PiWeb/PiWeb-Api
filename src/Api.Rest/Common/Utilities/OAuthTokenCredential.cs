#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Security.Claims;

	#endregion

	public sealed class OAuthTokenCredential : ICredential, IEquatable<OAuthTokenCredential>
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthTokenCredential"/> class.
		/// </summary>
		public OAuthTokenCredential( string displayId, string accessToken, DateTime accessTokenExpiration, string refreshToken, string username = null, string email = null )
		{
			DisplayId = displayId;
			AccessToken = accessToken;
			AccessTokenExpiration = accessTokenExpiration;
			RefreshToken = refreshToken;
			Username = username ?? OAuthHelper.TokenToUsername( AccessToken );
			MailAddress = email ?? OAuthHelper.TokenToMailAddress( AccessToken );
		}

		#endregion

		#region properties

		public string AccessToken { get; }

		public DateTime AccessTokenExpiration { get; }

		public string RefreshToken { get; }

		public string Username { get; }

		public string MailAddress { get; }

		#endregion

		#region methods

		public static OAuthTokenCredential CreateWithIdentityToken( string identityToken, string accessToken, DateTime accessTokenExpiration, string refreshToken )
		{
			var claims = OAuthHelper.GetClaimsFromSecurityToken( identityToken );

			return CreateWithClaims( claims, accessToken, accessTokenExpiration, refreshToken );
		}

		public static OAuthTokenCredential CreateWithClaims( IEnumerable<Claim> claims, string accessToken, DateTime accessTokenExpiration, string refreshToken )
		{
			var claimList = claims.ToList();

			var identity = OAuthHelper.IdentityClaimsToFriendlyText( claimList );
			var username = OAuthHelper.IdentityClaimsToUsername( claimList );
			var email = OAuthHelper.IdentityClaimsToEmail( claimList );

			return new OAuthTokenCredential( identity, accessToken, accessTokenExpiration, refreshToken, username, email );
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			return Equals( obj as OAuthTokenCredential );
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			unchecked
			{
				var hashCode = DisplayId != null ? DisplayId.GetHashCode() : 0;
				hashCode = ( hashCode * 397 ) ^ ( AccessToken != null ? AccessToken.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ AccessTokenExpiration.GetHashCode();
				hashCode = ( hashCode * 397 ) ^ ( RefreshToken != null ? RefreshToken.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ ( Username != null ? Username.GetHashCode() : 0 );
				hashCode = ( hashCode * 397 ) ^ ( MailAddress != null ? MailAddress.GetHashCode() : 0 );
				return hashCode;
			}
		}

		#endregion

		#region interface ICredential

		/// <inheritdoc />
		public string DisplayId { get; }

		/// <inheritdoc />
		public bool Equals( ICredential other )
		{
			return Equals( other as OAuthTokenCredential );
		}

		#endregion

		#region interface IEquatable<OAuthTokenCredential>

		/// <inheritdoc />
		public bool Equals( OAuthTokenCredential other )
		{
			return other != null
					&& string.Equals( DisplayId, other.DisplayId )
					&& string.Equals( AccessToken, other.AccessToken )
					&& AccessTokenExpiration.Equals( other.AccessTokenExpiration )
					&& string.Equals( RefreshToken, other.RefreshToken )
					&& string.Equals( Username, other.Username )
					&& string.Equals( MailAddress, other.MailAddress );
		}

		#endregion
	}
}