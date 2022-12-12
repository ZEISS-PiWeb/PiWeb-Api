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
	using System.IdentityModel.Tokens.Jwt;
	using System.IO;
	using System.Linq;
	using System.Net.Http;
	using System.Security.Claims;
	using System.Security.Cryptography;
	using System.Text;
	using System.Threading.Tasks;
	using IdentityModel;
	using IdentityModel.Client;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;

	#endregion

	public static class OAuthHelper
	{
		#region constants

		private const string ClientId = "f1ddf74a-7ed1-4963-ab60-a1138a089791";
		private const string ClientSecret = "d2940022-7469-4790-9498-776e3adac79f";
		private const string RedirectUri = "urn:ietf:wg:oauth:2.0:oob";

		#endregion

		#region members

		private static readonly string CacheFilePath = Path.Combine( Environment.GetFolderPath( Environment.SpecialFolder.ApplicationData ), @"Zeiss\PiWeb\OpenIdTokens.dat" );

		// FUTURE: replace by a real cache with cleanup, this is a memory leak for long running processes
		private static readonly CredentialRepository AccessTokenCache = new CredentialRepository( CacheFilePath );

		#endregion

		#region methods

		public static JwtSecurityToken DecodeSecurityToken( string jwtEncodedString )
		{
			return new JwtSecurityToken( jwtEncodedString );
		}

		public static IEnumerable<Claim> GetClaimsFromSecurityToken( string jwtEncodedString )
		{
			return DecodeSecurityToken( jwtEncodedString ).Claims;
		}

		public static string IdentityClaimsToFriendlyText( IList<Claim> claims )
		{
			var name = IdentityClaimsToUsername( claims );
			var email = IdentityClaimsToEmail( claims );

			return $"{name} ({email})";
		}

		public static string IdentityClaimsToEmail( IList<Claim> claims )
		{
			return claims.SingleOrDefault( claim => claim.Type == "email" )?.Value;
		}

		public static string IdentityClaimsToUsername( IList<Claim> claims )
		{
			return claims.SingleOrDefault( claim => claim.Type == "name" )?.Value;
		}

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

		private static async Task<OAuthTokenCredential> TryGetOAuthTokenFromRefreshTokenAsync( TokenClient tokenClient, IDiscoveryCache discoveryCache, string refreshToken )
		{
			// when a refresh token is present try to use it to acquire a new access token
			if( string.IsNullOrEmpty( refreshToken ) )
				return null;

			var tokenResponse = await tokenClient.RequestRefreshTokenAsync( refreshToken ).ConfigureAwait( false );
			if( tokenResponse.IsError )
				return null;

			var discoveryInfo = await discoveryCache.GetAsync().ConfigureAwait( false );
			if( discoveryInfo.IsError )
				return null;

			using var httpClient = new HttpClient();
			var response = await httpClient
				.GetUserInfoAsync( new UserInfoRequest { Address = discoveryInfo.UserInfoEndpoint, Token = tokenResponse.AccessToken } )
				.ConfigureAwait( false );

			if( response.IsError )
				return null;

			return OAuthTokenCredential.CreateWithClaims(
				response.Claims,
				tokenResponse.AccessToken,
				DateTime.UtcNow + TimeSpan.FromSeconds( tokenResponse.ExpiresIn ),
				tokenResponse.RefreshToken );
		}

		private static async Task<OAuthTokenCredential> TryGetOAuthTokenFromAuthorizeResponseAsync( TokenClient tokenClient, CryptoNumbers cryptoNumbers, AuthorizeResponse response )
		{
			if( response == null )
				return null;

			// claims des IdentityToken decodieren
			var claims = DecodeSecurityToken( response.IdentityToken ).Claims.ToArray();

			// die folgenden validierungen sind notwendig, um diversen CSRF / man in the middle / etc. Angriffsszenarien zu begegnen
			// state validieren
			if( !string.Equals( cryptoNumbers.State, response.State, StringComparison.Ordinal ) )
				throw new InvalidOperationException( "invalid state value in openid service responce." );

			// nonce validieren
			if( !ValidateNonce( cryptoNumbers.Nonce, claims ) )
				throw new InvalidOperationException( "invalid nonce value in identity token." );

			// c_hash validieren
			if( !ValidateCodeHash( response.Code, claims ) )
				throw new InvalidOperationException( "invalid c_hash value in identity token." );

			// code eintauschen gegen access token und refresh token, dabei den code verifier mitschicken, um man-in-the-middle Angriff auf authorization code zu begegnen (PKCE)
			var tokenResponse = await tokenClient.RequestAuthorizationCodeTokenAsync(
				code: response.Code,
				redirectUri: RedirectUri,
				codeVerifier: cryptoNumbers.Verifier ).ConfigureAwait( false );

			if( tokenResponse.IsError )
				throw new InvalidOperationException( "error during request of access token using authorization code: " + tokenResponse.Error );

			return OAuthTokenCredential.CreateWithIdentityToken( tokenResponse.IdentityToken, tokenResponse.AccessToken, DateTime.UtcNow + TimeSpan.FromSeconds( tokenResponse.ExpiresIn ), tokenResponse.RefreshToken );
		}

		private static async Task<string> CreateOAuthStartUrl( IDiscoveryCache discoveryCache, CryptoNumbers cryptoNumbers )
		{
			var discoveryInfo = await discoveryCache.GetAsync().ConfigureAwait( false );
			if( discoveryInfo.IsError )
				return null;

			var request = new RequestUrl( discoveryInfo.AuthorizeEndpoint );
			return request.CreateAuthorizeUrl(
				clientId: ClientId,
				responseType: "id_token code",
				responseMode: "form_post",
				scope: "openid profile email offline_access piweb",
				redirectUri: RedirectUri,
				state: cryptoNumbers.State,
				nonce: cryptoNumbers.Nonce,
				codeChallenge: cryptoNumbers.Challenge,
				codeChallengeMethod: OidcConstants.CodeChallengeMethods.Sha256 );
		}

		private static async Task<string> CreateAuthorityAsync( string instanceUrl )
		{
			var authority = await DiscoverOpenIdAuthorityAsync( instanceUrl ).ConfigureAwait( false );
			if( authority == null )
				throw new InvalidOperationException( "cannot detect OpenID authority from resource URL." );

			return authority;
		}

		private static async Task<TokenClient> CreateTokenClient( IDiscoveryCache discoveryCache )
		{
			var discoveryInfo = await discoveryCache.GetAsync().ConfigureAwait( false );

			var tokenClient = new HttpClient();
			return new TokenClient( tokenClient, new TokenClientOptions
			{
				Address = discoveryInfo.TokenEndpoint,
				ClientId = ClientId,
				ClientSecret = ClientSecret
			} );
		}

		public static async Task<OAuthTokenCredential> GetAuthenticationInformationForDatabaseUrlAsync( string databaseUrl, string refreshToken = null, Func<OAuthRequest, Task<OAuthResponse>> requestCallbackAsync = null )
		{
			var instanceUrl = GetInstanceUrl( databaseUrl );

			var result = TryGetCurrentOAuthToken( instanceUrl, ref refreshToken );
			if( result != null )
				return result;

			var discoveryCache = new DiscoveryCache( await CreateAuthorityAsync( instanceUrl ).ConfigureAwait( false ));
			var tokenClient = await CreateTokenClient( discoveryCache ).ConfigureAwait( false );

			result = await TryGetOAuthTokenFromRefreshTokenAsync( tokenClient, discoveryCache, refreshToken ).ConfigureAwait( false );
			if( result != null )
			{
				AccessTokenCache.Store( instanceUrl, result );
				return result;
			}

			// no refresh token or refresh token expired, do the full auth cycle eventually involving a password prompt
			if( requestCallbackAsync == null )
				return null;

			var cryptoNumbers = new CryptoNumbers();
			var startUrl = await CreateOAuthStartUrl( discoveryCache, cryptoNumbers ).ConfigureAwait( false );

			var request = new OAuthRequest( startUrl, RedirectUri );
			var response = ( await requestCallbackAsync( request ).ConfigureAwait( false ) )?.ToAuthorizeResponse();
			if( response == null )
				return null;

			result = await TryGetOAuthTokenFromAuthorizeResponseAsync( tokenClient, cryptoNumbers, response ).ConfigureAwait( false );
			if( result == null )
				return null;

			AccessTokenCache.Store( instanceUrl, result );
			return result;
		}

		public static OAuthTokenCredential GetAuthenticationInformationForDatabaseUrl( string databaseUrl, string refreshToken = null, Func<OAuthRequest, OAuthResponse> requestCallback = null )
		{
			var instanceUrl = GetInstanceUrl( databaseUrl );

			var result = TryGetCurrentOAuthToken( instanceUrl, ref refreshToken );
			if( result != null )
				return result;

			var discoveryCache = new DiscoveryCache( CreateAuthorityAsync( instanceUrl ).GetAwaiter().GetResult() );
			var tokenClient = CreateTokenClient( discoveryCache ).GetAwaiter().GetResult();

			result = TryGetOAuthTokenFromRefreshTokenAsync( tokenClient, discoveryCache, refreshToken ).GetAwaiter().GetResult();

			if( result != null )
			{
				AccessTokenCache.Store( instanceUrl, result );
				return result;
			}

			// no refresh token or refresh token expired, do the full auth cycle eventually involving a password prompt
			if( requestCallback == null )
				return null;

			var cryptoNumbers = new CryptoNumbers();
			var startUrl = CreateOAuthStartUrl( discoveryCache, cryptoNumbers ).GetAwaiter().GetResult();

			var request = new OAuthRequest( startUrl, RedirectUri );
			var response = requestCallback( request )?.ToAuthorizeResponse();
			if( response == null )
				return null;

			result = TryGetOAuthTokenFromAuthorizeResponseAsync( tokenClient, cryptoNumbers, response ).GetAwaiter().GetResult();
			if( result == null )
				return null;

			AccessTokenCache.Store( instanceUrl, result );
			return result;
		}

		/// <summary>
		/// get openid authority info from service without any authentication
		/// FUTURE: use the WebFinger method discovery method described in OpenID connect specification section 2
		/// <seealso>
		///     <cref>https://openid.net/specs/openid-connect-discovery-1_0.html#IssuerDiscovery</cref>
		/// </seealso>
		/// </summary>
		private static async Task<string> DiscoverOpenIdAuthorityAsync( string resourceUrl )
		{
			var oauthServiceRest = new OAuthServiceRestClient( new Uri( resourceUrl ) )
			{
				UseDefaultWebProxy = true
			};

			var tokenInfo = await oauthServiceRest.GetOAuthTokenInformation().ConfigureAwait( false );
			return tokenInfo.OpenIdAuthority;
		}

		public static bool ValidateNonce( string expectedNonce, IEnumerable<Claim> tokenClaims )
		{
			var tokenNonce = tokenClaims.FirstOrDefault( c => c.Type == JwtClaimTypes.Nonce );

			return tokenNonce != null && string.Equals( tokenNonce.Value, expectedNonce, StringComparison.Ordinal );
		}

		private static bool ValidateCodeHash( string authorizationCode, IEnumerable<Claim> tokenClaims )
		{
			var cHash = tokenClaims.FirstOrDefault( c => c.Type == JwtClaimTypes.AuthorizationCodeHash );

			using var sha = SHA256.Create();
			var codeHash = sha.ComputeHash( Encoding.ASCII.GetBytes( authorizationCode ) );
			var leftBytes = new byte[ 16 ];
			Array.Copy( codeHash, leftBytes, 16 );

			var codeHashB64 = Base64Url.Encode( leftBytes );

			return string.Equals( cHash.Value, codeHashB64, StringComparison.Ordinal );
		}

		public static void ClearAuthenticationInformationForDatabaseUrl( string databaseUrl )
		{
			var instanceUrl = GetInstanceUrl( databaseUrl );
			AccessTokenCache.Remove( instanceUrl );

			// FUTURE: call endsession endpoint of identity server
			// https://identityserver.github.io/Documentation/docsv2/endpoints/endSession.html
		}

		public static void ClearAllAuthenticationInformation()
		{
			AccessTokenCache.Clear();
		}

		#endregion

		#region class CryptoNumbers

		private class CryptoNumbers
		{
			#region constructors

			public CryptoNumbers()
			{
				Nonce = CryptoRandom.CreateUniqueId(); // only default length of 16 as this is included in the access token which should small
				State = CryptoRandom.CreateUniqueId( 32 );
				Verifier = CryptoRandom.CreateUniqueId( 32 );
				Challenge = Verifier.ToCodeChallenge();
			}

			#endregion

			#region properties

			public string Nonce { get; }
			public string State { get; }
			public string Verifier { get; }
			public string Challenge { get; }

			#endregion
		}

		#endregion
	}
}