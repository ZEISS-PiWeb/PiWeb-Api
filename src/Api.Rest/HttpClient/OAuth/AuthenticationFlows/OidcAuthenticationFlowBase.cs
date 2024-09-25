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
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// Base class for OIDC authentication flows which offers reusable methods.
/// </summary>
public abstract class OidcAuthenticationFlowBase
{
	#region methods

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
	/// <returns>A valid <see cref="OAuthTokenCredential"/> or <see langword="null"/> if no token could be retrieved.</returns>
	protected static async Task<OAuthTokenCredential> TryGetOAuthTokenFromRefreshTokenAsync( TokenClient tokenClient, string userInfoEndpoint, string refreshToken )
	{
		// when a refresh token is present try to use it to acquire a new access token
		if( string.IsNullOrEmpty( refreshToken ) )
			return null;

		var tokenResponse = await tokenClient.RequestRefreshTokenAsync( refreshToken ).ConfigureAwait( false );
		if( tokenResponse.IsError )
			return null;

		using var httpClient = new HttpClient();
		var response = await httpClient
			.GetUserInfoAsync( new UserInfoRequest { Address = userInfoEndpoint, Token = tokenResponse.AccessToken } )
			.ConfigureAwait( false );

		if( response.IsError )
			return null;

		return OAuthTokenCredential.CreateWithClaims(
			response.Claims,
			tokenResponse.AccessToken,
			DateTime.UtcNow + TimeSpan.FromSeconds( tokenResponse.ExpiresIn ),
			tokenResponse.RefreshToken );
	}

	#endregion
}