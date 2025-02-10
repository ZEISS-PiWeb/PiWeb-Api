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
using System.Threading;
using System.Threading.Tasks;
using IdentityModel.Client;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// Represents the hybrid OIDC authentication flow (the combination of implicit and authorization code flow).
/// </summary>
public class HybridFlow : OidcAuthenticationFlowBase, IOidcAuthenticationFlow
{
	#region methods

	private static async Task<OAuthTokenCredential> TryGetOAuthTokenFromAuthorizeResponseAsync(
		TokenClient tokenClient,
		CryptoNumbers cryptoNumbers,
		AuthorizeResponse response,
		OAuthConfiguration configuration,
		DiscoveryDocumentResponse discoveryDocument,
		CancellationToken cancellationToken = default )
	{
		if( response == null )
			return null;

		var tokenInformation = configuration.LocalTokenInformation;

		// decode the IdentityToken claims
		var decodedToken = OAuthHelper.DecodeSecurityToken( response.IdentityToken );
		var claims = decodedToken.Claims.ToArray();

		// the following validations are necessary to protect against several kinds of CSRF / man-in-the-middle and other attack scenarios
		var validationResult = await OAuthTokenValidator.ValidateToken( response.IdentityToken, discoveryDocument, tokenInformation );

		if( !validationResult.IsValid )
			throw new InvalidOperationException( "Error during validation of token.", validationResult.Exception );

		// further validation steps
		// state validation
		if( !string.Equals( cryptoNumbers.State, response.State, StringComparison.Ordinal ) )
			throw new InvalidOperationException( "Invalid state value in openID service response." );

		// nonce validation
		if( !OAuthTokenValidator.ValidateNonce( cryptoNumbers.Nonce, claims ) )
			throw new InvalidOperationException( "Invalid nonce value in identity token." );

		// c_hash validation
		if( !OAuthTokenValidator.ValidateCodeHash( response.Code, claims, decodedToken.SignatureAlgorithm ) )
			throw new InvalidOperationException( "Invalid c_hash value in identity token." );

		// exchange the code for the access and refresh token, also send the code verifier,
		// to handle man-in-the-middle attacks against the authorization code (PKCE).
		// Code not null here since ValidateCodeHash succeeded.
		var tokenResponse = await tokenClient.RequestAuthorizationCodeTokenAsync(
			code: response.Code!,
			redirectUri: tokenInformation.RedirectUri,
			codeVerifier: cryptoNumbers.Verifier,
			cancellationToken: cancellationToken ).ConfigureAwait( false );

		if( tokenResponse.IsError )
			throw new InvalidOperationException( $"Error during request of access token using authorization code: {tokenResponse.Error}." );

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
		var discoveryInfo = GetDiscoveryInfoAsync( configuration.LocalTokenInformation ).GetAwaiter().GetResult();
		ThrowOnInvalidDiscoveryDocument( discoveryInfo );

		var tokenClient = CreateTokenClient( discoveryInfo.TokenEndpoint, configuration.LocalTokenInformation.ClientID );
		var result = TryGetOAuthTokenFromRefreshTokenAsync( tokenClient, discoveryInfo.UserInfoEndpoint, refreshToken, configuration ).GetAwaiter().GetResult();
		if( result != null )
			return result;

		// no refresh token or refresh token expired, do the full auth cycle eventually involving a password prompt
		if( requestCallback == null )
			return null;

		var cryptoNumbers = new CryptoNumbers();
		var startUrl = CreateOAuthStartUrl( discoveryInfo.AuthorizeEndpoint, "id_token code", cryptoNumbers, configuration.LocalTokenInformation );

		var request = new OAuthRequest( startUrl, configuration.LocalTokenInformation.RedirectUri );
		var response = requestCallback( request )?.ToAuthorizeResponse();

		ThrowOnInvalidAuthorizeResponse( response );

		result = TryGetOAuthTokenFromAuthorizeResponseAsync( tokenClient, cryptoNumbers, response, configuration, discoveryInfo ).GetAwaiter().GetResult();

		return result;
	}

	/// <inheritdoc />
	public async Task<OAuthTokenCredential> ExecuteAuthenticationFlowAsync( string refreshToken, OAuthConfiguration configuration, Func<OAuthRequest, Task<OAuthResponse>> requestCallbackAsync, CancellationToken cancellationToken )
	{
		var discoveryInfo = await GetDiscoveryInfoAsync( configuration.LocalTokenInformation ).ConfigureAwait( false );
		ThrowOnInvalidDiscoveryDocument( discoveryInfo );

		var tokenClient = CreateTokenClient( discoveryInfo.TokenEndpoint, configuration.LocalTokenInformation.ClientID );
		var result = await TryGetOAuthTokenFromRefreshTokenAsync( tokenClient, discoveryInfo.UserInfoEndpoint, refreshToken, configuration, cancellationToken ).ConfigureAwait( false );
		if( result != null )
			return result;

		// no refresh token or refresh token expired, do the full auth cycle eventually involving a password prompt
		if( requestCallbackAsync == null )
			return null;

		var cryptoNumbers = new CryptoNumbers();
		var startUrl = CreateOAuthStartUrl( discoveryInfo.AuthorizeEndpoint, "id_token code", cryptoNumbers, configuration.LocalTokenInformation );

		var request = new OAuthRequest( startUrl, configuration.LocalTokenInformation.RedirectUri );
		var response = ( await requestCallbackAsync( request ).ConfigureAwait( false ) )?.ToAuthorizeResponse();

		ThrowOnInvalidAuthorizeResponse( response );

		result = await TryGetOAuthTokenFromAuthorizeResponseAsync( tokenClient, cryptoNumbers, response, configuration, discoveryInfo, cancellationToken ).ConfigureAwait( false );

		return result;
	}

	#endregion
}