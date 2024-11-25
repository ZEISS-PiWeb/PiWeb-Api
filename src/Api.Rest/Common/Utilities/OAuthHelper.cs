#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IdentityModel.Tokens.Jwt;
	using System.IO;
	using System.Linq;
	using System.Security.Claims;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;
	using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth.AuthenticationFlows;

	#endregion

	/// <summary>
	/// Provides a number of useful methods for handling OAuth authentication.
	/// </summary>
	public static class OAuthHelper
	{
		#region members

		private static readonly string CacheFilePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), @"Zeiss\PiWeb\OpenIdTokens.dat" );

		// FUTURE: replace by a real cache with cleanup, this is a memory leak for long running processes
		private static readonly CredentialRepository AccessTokenCache = new CredentialRepository( CacheFilePath );

		#endregion

		#region methods

		/// <summary>
		/// Decode a JSON Web Token (JWT).
		/// </summary>
		/// <param name="jwtEncodedString">The encoded string which represents the JWT.</param>
		/// <returns>The decoded <see cref="JwtSecurityToken"/>.</returns>
		public static JwtSecurityToken DecodeSecurityToken( string jwtEncodedString )
		{
			return new JwtSecurityToken( jwtEncodedString );
		}

		/// <summary>
		/// Get all included claims from an encoded JSON Web Token (JWT).
		/// </summary>
		/// <param name="jwtEncodedString">The encoded string which represents the JWT.</param>
		/// <returns>A collection of all included claims.</returns>
		public static IEnumerable<Claim> GetClaimsFromSecurityToken( string jwtEncodedString )
		{
			return DecodeSecurityToken( jwtEncodedString ).Claims;
		}

		/// <summary>
		/// Creates a friendly text representation of identity information contained in the provided claims, mainly name and email.
		/// </summary>
		/// <param name="claims">A collection of claims.</param>
		/// <returns>A string containing name and email, e.g. for displaying.</returns>
		public static string IdentityClaimsToFriendlyText( IList<Claim> claims )
		{
			var name = IdentityClaimsToUsername( claims );
			var email = IdentityClaimsToEmail( claims );

			return $"{name} ({email})";
		}

		/// <summary>
		/// Extract the value of the JWT 'exp' token claim and convert it to the expiration date.
		/// </summary>
		/// <param name="jwtEncodedString">The JWT encoded token string.</param>
		/// <returns>The expiration date of the token in UTC time.</returns>
		public static DateTime TokenToExpirationTime( string jwtEncodedString )
		{
			if( string.IsNullOrEmpty( jwtEncodedString ) )
				return default;

			try
			{
				var claims = GetClaimsFromSecurityToken( jwtEncodedString ).ToList();
				var expClaim = claims.SingleOrDefault( claim => claim.Type == "exp" )?.Value;

				return long.TryParse( expClaim, out var secondsSinceEpoch )
					? DateTimeOffset.FromUnixTimeSeconds( secondsSinceEpoch ).UtcDateTime
					: default;
			}
			catch
			{
				// ignored
			}

			return default;
		}

		/// <summary>
		/// Extract the value of the email identity claim.
		/// </summary>
		/// <param name="claims">A collection of claims.</param>
		/// <returns>The email of the user.</returns>
		public static string IdentityClaimsToEmail( IList<Claim> claims )
		{
			return claims.SingleOrDefault( claim => claim.Type == "email" )?.Value;
		}

		/// <summary>
		/// Extract the value of the name identity claim.
		/// </summary>
		/// <param name="claims">A collection of claims.</param>
		/// <returns>The name of the user.</returns>
		public static string IdentityClaimsToUsername( IList<Claim> claims )
		{
			return claims.SingleOrDefault( claim => claim.Type == "name" )?.Value;
		}

		/// <summary>
		/// Creates a friendly text representation of identity information from the provided JSON Web Token (JWT), mainly name and email.
		/// </summary>
		/// <param name="jwtEncodedString">The encoded string which represents the JWT.</param>
		/// <returns>A string containing name and email, e.g. for displaying, or an empty string if no values could be extracted.</returns>
		public static string TokenToFriendlyText( string jwtEncodedString )
		{
			if( string.IsNullOrEmpty( jwtEncodedString ) )
				return "";

			try
			{
				var claims = GetClaimsFromSecurityToken( jwtEncodedString ).ToList();

				return IdentityClaimsToFriendlyText( claims );
			}
			catch
			{
				// ignored
			}

			return "";
		}

		/// <summary>
		/// Extracts the username from the provided JSON Web Token (JWT).
		/// </summary>
		/// <param name="jwtEncodedString">The encoded string which represents the JWT.</param>
		/// <returns>The username, or an empty string if no value could be extracted.</returns>
		public static string TokenToUsername( string jwtEncodedString )
		{
			if( string.IsNullOrEmpty( jwtEncodedString ) )
				return "";

			try
			{
				var claims = GetClaimsFromSecurityToken( jwtEncodedString ).ToList();

				return IdentityClaimsToUsername( claims );
			}
			catch
			{
				// ignored
			}

			return "";
		}

		/// <summary>
		/// Extracts the email from the provided JSON Web Token (JWT).
		/// </summary>
		/// <param name="jwtEncodedString">The encoded string which represents the JWT.</param>
		/// <returns>The email of the user, or an empty string if no value could be extracted.</returns>
		public static string TokenToMailAddress( string jwtEncodedString )
		{
			if( string.IsNullOrEmpty( jwtEncodedString ) )
				return "";

			try
			{
				var claims = GetClaimsFromSecurityToken( jwtEncodedString ).ToList();

				return IdentityClaimsToEmail( claims );
			}
			catch
			{
				// ignored
			}

			return "";
		}

		private static string GetInstanceUrl( [NotNull] string databaseUrl )
		{
			if( databaseUrl == null )
				throw new ArgumentNullException( nameof( databaseUrl ) );

			databaseUrl = databaseUrl.Replace( "/DataServiceSoap", string.Empty );
			databaseUrl = databaseUrl.Trim( '/' );
			return databaseUrl;
		}

		private static OAuthTokenCredential TryGetCurrentOAuthToken( string instanceUrl, ref string refreshToken )
		{
			// if access token is still valid (5min margin to allow for clock skew), just return it from the cache
			if( !AccessTokenCache.TryGetCredential( instanceUrl, out var result ) )
				return null;

			if( string.IsNullOrEmpty( refreshToken ) )
				refreshToken = result.RefreshToken;

			if( result.AccessTokenExpiration.AddMinutes( -5 ) > DateTime.UtcNow )
				return result;

			return null;
		}

		/// <summary>
		/// Get public OAuth configuration from PiWeb server.
		/// FUTURE: use the WebFinger method discovery method described in OpenID connect specification section 2
		/// <seealso>
		///     <cref>https://openid.net/specs/openid-connect-discovery-1_0.html#IssuerDiscovery</cref>
		/// </seealso>
		/// </summary>
		private static async Task<OAuthConfiguration> GetOAuthConfigurationAsync( string instanceUrl )
		{
			var oauthServiceRest = new OAuthServiceRestClient( new Uri( instanceUrl ) )
			{
				UseDefaultWebProxy = true
			};

			var tokenInformation = await oauthServiceRest.GetOAuthConfiguration().ConfigureAwait( false );

			if( tokenInformation == null )
				throw new InvalidOperationException( "Cannot detect OpenID token information from resource URL." );

			return tokenInformation;
		}

		private static IOidcAuthenticationFlow ChooseSuitableAuthenticationFlow( OAuthConfiguration tokenInformation )
		{
			// Authorization code flow is preferred if supported
			if( tokenInformation.SupportedOidcAuthenticationFlows.HasFlag( OidcAuthenticationFlows.AuthorizationCodeFlow ) )
				return new AuthorizationCodeFlow();

			return new HybridFlow();
		}

		/// <summary>
		/// Retrieves authentication information for a given database URL.
		/// </summary>
		/// <param name="databaseUrl">Database URL to retrieve authentication information for.</param>
		/// <param name="refreshToken">Optional refresh token that is used to renew the authentication information.</param>
		/// <param name="requestCallbackAsync">Optional callback to request the user to interactively authenticate.</param>
		/// <param name="bypassLocalCache">Defines whether locally cached token information are neither used nor updated.</param>
		/// <returns>A new <see cref="OAuthTokenCredential"/> instance, or <c>null</c>, if no token could be retrieved.</returns>
		public static async Task<OAuthTokenCredential> GetAuthenticationInformationForDatabaseUrlAsync(
			string databaseUrl,
			string refreshToken = null,
			Func<OAuthRequest, Task<OAuthResponse>> requestCallbackAsync = null,
			bool bypassLocalCache = false )
		{
			var instanceUrl = GetInstanceUrl( databaseUrl );

			if( !bypassLocalCache )
			{
				var cachedToken = TryGetCurrentOAuthToken( instanceUrl, ref refreshToken );
				if( cachedToken != null )
					return cachedToken;
			}

			var tokenInformation = await GetOAuthConfigurationAsync( instanceUrl ).ConfigureAwait( false );

			var authenticationFlow = ChooseSuitableAuthenticationFlow( tokenInformation );
			var result = await authenticationFlow.ExecuteAuthenticationFlowAsync( refreshToken, tokenInformation, requestCallbackAsync ).ConfigureAwait( false );

			if( result == null )
				return null;

			if( !bypassLocalCache )
				AccessTokenCache.Store( instanceUrl, result );

			return result;
		}

		/// <summary>
		/// Retrieves authentication information for a given database URL.
		/// </summary>
		/// <param name="databaseUrl">Database URL to retrieve authentication information for.</param>
		/// <param name="refreshToken">Optional refresh token that is used to renew the authentication information.</param>
		/// <param name="requestCallback">Optional callback to request the user to interactively authenticate.</param>
		/// <param name="bypassLocalCache">Defines whether locally cached token information are neither used nor updated.</param>
		/// <returns>A new <see cref="OAuthTokenCredential"/> instance, or <c>null</c>, if no token could be retrieved.</returns>
		public static OAuthTokenCredential GetAuthenticationInformationForDatabaseUrl(
			string databaseUrl,
			string refreshToken = null,
			Func<OAuthRequest, OAuthResponse> requestCallback = null,
			bool bypassLocalCache = false )
		{
			var instanceUrl = GetInstanceUrl( databaseUrl );

			if( !bypassLocalCache )
			{
				var cachedToken = TryGetCurrentOAuthToken( instanceUrl, ref refreshToken );
				if( cachedToken != null )
					return cachedToken;
			}

			var tokenInformation = GetOAuthConfigurationAsync( instanceUrl ).GetAwaiter().GetResult();

			var authenticationFlow = ChooseSuitableAuthenticationFlow( tokenInformation );
			var result = authenticationFlow.ExecuteAuthenticationFlow( refreshToken, tokenInformation, requestCallback );

			if( result == null )
				return null;

			if( !bypassLocalCache )
				AccessTokenCache.Store( instanceUrl, result );

			return result;
		}

		/// <summary>
		/// Clears locally cached authentication information for a given database URL.
		/// </summary>
		/// <param name="databaseUrl">Database URL to clear authentication information for.</param>
		public static void ClearAuthenticationInformationForDatabaseUrl( string databaseUrl )
		{
			var instanceUrl = GetInstanceUrl( databaseUrl );
			AccessTokenCache.Remove( instanceUrl );

			// FUTURE: call endsession endpoint of identity server
			// https://identityserver.github.io/Documentation/docsv2/endpoints/endSession.html
		}

		/// <summary>
		/// Clears all locally cached authentication information.
		/// </summary>
		public static void ClearAllAuthenticationInformation()
		{
			AccessTokenCache.Clear();
		}

		#endregion
	}
}