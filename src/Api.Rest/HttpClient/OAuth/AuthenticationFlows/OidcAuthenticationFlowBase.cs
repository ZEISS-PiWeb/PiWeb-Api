#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth.AuthenticationFlows;

#region usings

using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// Base class for OIDC authentication flows which offers reusable methods.
/// </summary>
public abstract class OidcAuthenticationFlowBase
{
	#region methods

	/// <summary>
	/// Choose a suitable security token to use as an access token for requesting data, based on given configuration.
	/// </summary>
	/// <param name="tokenResponse">Response of a token request containing different security token.</param>
	/// <param name="configuration">OAuth configuration containing the settings for authentication.</param>
	/// <param name="expiration">Expiration time of the chosen token.</param>
	protected static string ChooseAccessToken( TokenResponse tokenResponse, OAuthConfiguration configuration, out DateTime expiration )
	{
		expiration = default;
		switch( configuration.AccessTokenType )
		{
			case AccessTokenType.Auto:
				try
				{
					OAuthHelper.DecodeSecurityToken( tokenResponse.AccessToken );
					expiration = DateTime.UtcNow + TimeSpan.FromSeconds( tokenResponse.ExpiresIn );
					return tokenResponse.AccessToken;
				}
				catch( SecurityTokenMalformedException )
				{
					expiration = OAuthHelper.TokenToExpirationTime( tokenResponse.IdentityToken );
					return tokenResponse.IdentityToken;
				}
			case AccessTokenType.OidcIdentityToken:
				return tokenResponse.IdentityToken;
			case AccessTokenType.OAuthAccessToken:
			default:
				return tokenResponse.AccessToken;
		}
	}

	/// <summary>
	/// Request target server for authentication settings.
	/// </summary>
	/// <param name="tokenInformation">Token information containing the discovery location and other settings.</param>
	protected static async Task<DiscoveryDocumentResponse> GetDiscoveryInfoAsync( OAuthTokenInformation tokenInformation )
	{
		var discoveryCache = new DiscoveryCache( tokenInformation.OpenIdAuthority,
			new DiscoveryPolicy
			{
				AdditionalEndpointBaseAddresses = tokenInformation.AdditionalEndpointBaseAddresses
			} );

		var discoveryInfo = await discoveryCache.GetAsync().ConfigureAwait( false );
		return discoveryInfo;
	}

	/// <summary>
	/// Create a client to interact with the token endpoint of the identity provider.
	/// </summary>
	/// <param name="tokenEndpoint">Location of the token endpoint.</param>
	/// <param name="clientId">Registered ID of the client at the identity provider.</param>
	protected static TokenClient CreateTokenClient( string tokenEndpoint, string clientId )
	{
		var tokenClient = new HttpClient();
		return new TokenClient( tokenClient, new TokenClientOptions
		{
			Address = tokenEndpoint,
			ClientId = clientId,
		} );
	}

	/// <summary>
	/// Create the start URL for authentication containing all necessary query parameter.
	/// </summary>
	/// <param name="authorizeEndpoint">Location of the authorize endpoint.</param>
	/// <param name="responseType">OAuth response type/mode.</param>
	/// <param name="cryptoNumbers">Cryptographic numbers like Nonce or Challenge.</param>
	/// <param name="tokenInformation">Token information containing the settings for authentication.</param>
	protected static string CreateOAuthStartUrl( string authorizeEndpoint, string responseType, CryptoNumbers cryptoNumbers, OAuthTokenInformation tokenInformation )
	{
		var request = new RequestUrl( authorizeEndpoint );
		return request.CreateAuthorizeUrl(
			clientId: tokenInformation.ClientID,
			responseType: responseType,
			responseMode: "form_post",
			scope: tokenInformation.RequestedScopes,
			redirectUri: tokenInformation.RedirectUri,
			state: cryptoNumbers.State,
			nonce: cryptoNumbers.Nonce,
			codeChallenge: cryptoNumbers.Challenge,
			codeChallengeMethod: OidcConstants.CodeChallengeMethods.Sha256 );
	}

	/// <summary>
	/// Request a new authentication token using a refresh token.
	/// </summary>
	/// <param name="tokenClient">A client to interact with the token endpoint of the identity provider.</param>
	/// <param name="userInfoEndpoint">Location of the user info endpoint of the identity provider.</param>
	/// <param name="refreshToken">Refresh token to acquire a new authentication token.</param>
	/// <param name="configuration">OAuth configuration of the PiWeb Server.</param>
	/// <returns>A valid <see cref="OAuthTokenCredential"/> or <see langword="null"/> if no token could be retrieved.</returns>
	protected static async Task<OAuthTokenCredential> TryGetOAuthTokenFromRefreshTokenAsync( TokenClient tokenClient, string userInfoEndpoint, string refreshToken, OAuthConfiguration configuration )
	{
		// when a refresh token is present try to use it to acquire a new access token
		if( string.IsNullOrEmpty( refreshToken ) )
			return null;

		var tokenResponse = await tokenClient.RequestRefreshTokenAsync( refreshToken ).ConfigureAwait( false );
		if( tokenResponse.IsError )
			return null;

		var accessToken = ChooseAccessToken( tokenResponse, configuration, out var expirationDate );
		if( accessToken == null )
			return null;

		if( tokenResponse.IdentityToken == null )
			return await CreateCredentialsWithClaimsFromUserInfo( userInfoEndpoint, tokenResponse, configuration ).ConfigureAwait( false );

		return OAuthTokenCredential.CreateWithIdentityToken(
			tokenResponse.IdentityToken,
			accessToken,
			expirationDate,
			tokenResponse.RefreshToken );
	}

	private static async Task<OAuthTokenCredential> CreateCredentialsWithClaimsFromUserInfo( string userInfoEndpoint, TokenResponse tokenResponse, OAuthConfiguration configuration )
	{
		var accessToken = ChooseAccessToken( tokenResponse, configuration, out var expirationDate );

		if( accessToken == null )
			return null;

		using var httpClient = new HttpClient();
		var response = await httpClient
			.GetUserInfoAsync( new UserInfoRequest { Address = userInfoEndpoint, Token = tokenResponse.AccessToken } )
			.ConfigureAwait( false );

		if( response.IsError )
			return null;

		return OAuthTokenCredential.CreateWithClaims(
			response.Claims,
			accessToken,
			expirationDate,
			tokenResponse.RefreshToken );
	}

	#endregion
}