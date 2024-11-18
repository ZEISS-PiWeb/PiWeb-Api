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
using System.Threading.Tasks;
using IdentityModel.Client;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// Represents the standard OIDC authorization code flow.
/// </summary>
public class AuthorizationCodeFlow : OidcAuthenticationFlowBase, IOidcAuthenticationFlow
{
	#region methods

	private static async Task<OAuthTokenCredential> TryGetOAuthTokenFromAuthorizeResponseAsync(
		TokenClient tokenClient,
		CryptoNumbers cryptoNumbers,
		AuthorizeResponse response,
		OAuthConfiguration configuration,
		DiscoveryDocumentResponse discoveryDocument )
	{
		if( response?.Code == null )
			return null;

		var tokenInformation = configuration.UpstreamTokenInformation;

		// exchange the code for the access and refresh token, also send the code verifier,
		// to handle man-in-the-middle attacks against the authorization code (PKCE).
		var tokenResponse = await tokenClient.RequestAuthorizationCodeTokenAsync(
			code: response.Code,
			redirectUri: tokenInformation.RedirectUri,
			codeVerifier: cryptoNumbers.Verifier ).ConfigureAwait( false );

		if( tokenResponse.IsError )
			throw new InvalidOperationException(
				$"Error during request of access token using authorization code: {tokenResponse.Error}. {tokenResponse.ErrorDescription}." );

		// decode the IdentityToken claims
		var claims = OAuthHelper.DecodeSecurityToken( tokenResponse.IdentityToken ).Claims.ToArray();

		// the following validations are necessary to protect against several kinds of CSRF / man-in-the-middle and other attack scenarios
		var validationResult = await OAuthTokenValidator.ValidateToken( tokenResponse.IdentityToken, discoveryDocument, tokenInformation );

		if( !validationResult.IsValid )
			throw new InvalidOperationException( $"Error during validation of token.", validationResult.Exception );

		// further validation steps
		if( !string.Equals( cryptoNumbers.State, response.State, StringComparison.Ordinal ) )
			throw new InvalidOperationException( "Invalid state value in openid service response." );

		if( !OAuthTokenValidator.ValidateNonce( cryptoNumbers.Nonce, claims ) )
			throw new InvalidOperationException( "Invalid nonce value in identity token." );

		var accessToken = ChooseAccessToken( tokenResponse, configuration, out var expirationDate );

		return OAuthTokenCredential.CreateWithIdentityToken(
			tokenResponse.IdentityToken,
			accessToken,
			expirationDate,
			tokenResponse.RefreshToken );
	}

	#endregion

	#region interface IOidcAuthenticationFlow

	/// <inheritdoc />
	public OAuthTokenCredential ExecuteAuthenticationFlow( string refreshToken, OAuthConfiguration configuration, Func<OAuthRequest, OAuthResponse> requestCallback )
	{
		var discoveryInfo = GetDiscoveryInfoAsync( configuration.UpstreamTokenInformation ).GetAwaiter().GetResult();
		if( discoveryInfo.IsError )
			return null;

		var tokenClient = CreateTokenClient( discoveryInfo.TokenEndpoint, configuration.UpstreamTokenInformation.ClientID );
		var result = TryGetOAuthTokenFromRefreshTokenAsync( tokenClient, discoveryInfo.UserInfoEndpoint, refreshToken, configuration ).GetAwaiter().GetResult();
		if( result != null )
			return result;

		// no refresh token or refresh token expired, do the full auth cycle eventually involving a password prompt
		if( requestCallback == null )
			return null;

		var cryptoNumbers = new CryptoNumbers();
		var startUrl = CreateOAuthStartUrl( discoveryInfo.AuthorizeEndpoint, "code", cryptoNumbers, configuration.UpstreamTokenInformation );

		var request = new OAuthRequest( startUrl, configuration.UpstreamTokenInformation.RedirectUri );
		var response = requestCallback( request )?.ToAuthorizeResponse();

		if( response == null )
			return null;

		result = TryGetOAuthTokenFromAuthorizeResponseAsync( tokenClient, cryptoNumbers, response, configuration, discoveryInfo ).GetAwaiter().GetResult();

		return result;
	}

	/// <inheritdoc />
	public async Task<OAuthTokenCredential> ExecuteAuthenticationFlowAsync( string refreshToken, OAuthConfiguration configuration, Func<OAuthRequest, Task<OAuthResponse>> requestCallbackAsync )
	{
		var discoveryInfo = await GetDiscoveryInfoAsync( configuration.UpstreamTokenInformation ).ConfigureAwait( false );
		if( discoveryInfo.IsError )
			return null;

		var tokenClient = CreateTokenClient( discoveryInfo.TokenEndpoint, configuration.UpstreamTokenInformation.ClientID );
		var result = await TryGetOAuthTokenFromRefreshTokenAsync( tokenClient, discoveryInfo.UserInfoEndpoint, refreshToken, configuration ).ConfigureAwait( false );
		if( result != null )
			return result;

		// no refresh token or refresh token expired, do the full auth cycle eventually involving a password prompt
		if( requestCallbackAsync == null )
			return null;

		var cryptoNumbers = new CryptoNumbers();
		var startUrl = CreateOAuthStartUrl( discoveryInfo.AuthorizeEndpoint, "code", cryptoNumbers, configuration.UpstreamTokenInformation );

		var request = new OAuthRequest( startUrl, configuration.UpstreamTokenInformation.RedirectUri );
		var response = ( await requestCallbackAsync( request ).ConfigureAwait( false ) )?.ToAuthorizeResponse();

		if( response == null )
			return null;

		result = await TryGetOAuthTokenFromAuthorizeResponseAsync( tokenClient, cryptoNumbers, response, configuration, discoveryInfo ).ConfigureAwait( false );

		return result;
	}

	#endregion
}