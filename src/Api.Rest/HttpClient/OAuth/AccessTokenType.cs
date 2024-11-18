#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;

/// <summary>
/// Enum representing the different modes to select the token type which can be used as an PiWeb access token.
/// </summary>
public enum AccessTokenType
{
	/// <summary>
	/// The clients automatically chooses a matching token in JWT format based on the following criteria:
	/// If the OAuth Access Token is a JWT, use this.
	/// If not, use the ID Token (JWT per OIDC definition).
	/// </summary>
	Auto,

	/// <summary>
	/// Force the client to use the ID Token.
	/// </summary>
	OidcIdentityToken,

	/// <summary>
	/// Force the client to use the OAuth Access Token.
	/// </summary>
	OAuthAccessToken
}