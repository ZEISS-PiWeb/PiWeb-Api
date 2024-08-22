#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities;

#region usings

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.IdentityModel.Tokens;
using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;

#endregion

/// <summary>
/// Provides different methods to validate identity token.
/// </summary>
public static class OAuthTokenValidator
{
	#region methods

	/// <summary>
	/// Checks if the provided nonce in the token claims is valid and matching the expected nonce.
	/// </summary>
	/// <param name="expectedNonce">The expected nonce.</param>
	/// <param name="tokenClaims">A collection of claims.</param>
	/// <returns><see langword="true"/> if nonce is valid, otherwise <see langword="false"/>.</returns>
	public static bool ValidateNonce( string expectedNonce, IEnumerable<Claim> tokenClaims )
	{
		var tokenNonce = tokenClaims.FirstOrDefault( c => c.Type == JwtClaimTypes.Nonce );

		return tokenNonce != null && string.Equals( tokenNonce.Value, expectedNonce, StringComparison.Ordinal );
	}

	/// <summary>
	/// Checks if the code hash in the claims is valid.
	/// </summary>
	/// <param name="authorizationCode">The acquired authorization code from the identity provider.</param>
	/// <param name="tokenClaims">The claims of the acquired identity token.</param>
	/// <param name="signatureAlgorithm">The identifier of the used signature algorithm, e.g. RS256.</param>
	/// <returns><see langword="true"/> if code hash is valid, otherwise <see langword="false"/>.</returns>
	public static bool ValidateCodeHash( string authorizationCode, IEnumerable<Claim> tokenClaims, string signatureAlgorithm )
	{
		var cHash = tokenClaims.FirstOrDefault( c => c.Type == JwtClaimTypes.AuthorizationCodeHash );

		if( !Enum.TryParse<SignatureHashType>( signatureAlgorithm, true, out var signatureHashType ) )
			throw new SecurityTokenException( $"Token signature algorithm '{signatureAlgorithm}' is not supported." );

		var asciiCode = Encoding.ASCII.GetBytes( authorizationCode );
		var codeHash = ComputeHash( asciiCode, signatureHashType, out var length );
		var leftBytesLength = length / 8 / 2; // convert length in bits to bytes, then only take left half of that as specified by OIDC specification.

		var leftBytes = new byte[ leftBytesLength ];
		Array.Copy( codeHash, leftBytes, leftBytesLength );

		var codeHashB64 = Base64Url.Encode( leftBytes );

		return string.Equals( cHash?.Value, codeHashB64, StringComparison.Ordinal );
	}

	private static byte[] ComputeHash( byte[] authCodeHash, SignatureHashType hashType, out int length )
	{
		switch( hashType )
		{
			case SignatureHashType.RS256:
			case SignatureHashType.HS256:
			case SignatureHashType.PS256:
			case SignatureHashType.ES256:
			{
				length = 256;
				using var sha256 = SHA256.Create();
				return sha256.ComputeHash( authCodeHash );
			}

			case SignatureHashType.RS384:
			case SignatureHashType.HS384:
			case SignatureHashType.PS384:
			case SignatureHashType.ES384:
			{
				length = 384;
				using var sha384 = SHA384.Create();
				return sha384.ComputeHash( authCodeHash );
			}

			case SignatureHashType.RS512:
			case SignatureHashType.HS512:
			case SignatureHashType.PS512:
			case SignatureHashType.ES512:
			{
				length = 512;
				using var sha512 = SHA512.Create();
				return sha512.ComputeHash( authCodeHash );
			}

			default:
				throw new ArgumentOutOfRangeException( nameof( hashType ), hashType, "Signature hash is not supported." );
		}
	}

	/// <summary>
	/// Validate the token with regards to issuer, audience, expiration and signature.
	/// </summary>
	/// <param name="token">The token to validate.</param>
	/// <param name="discoveryDocument">The discovery document from the identity provider.</param>
	/// <param name="tokenInformation">The token information from the PiWeb Server.</param>
	public static async Task<TokenValidationResult> ValidateToken( string token, DiscoveryDocumentResponse discoveryDocument, OAuthTokenInformation tokenInformation )
	{
		var tokenHandler = new JwtSecurityTokenHandler();
		var tokenOptions = new TokenValidationParameters
		{
			ValidIssuer = discoveryDocument.Issuer,
			ValidAudience = tokenInformation.ClientID,
			IssuerSigningKeys = new JsonWebKeySet( discoveryDocument.KeySet?.RawData ).GetSigningKeys()
		};

		return await tokenHandler.ValidateTokenAsync( token, tokenOptions );
	}

	#endregion
}

/// <summary>
/// Represents different types of signature algorithms used when signing a JWT.
/// </summary>
[SuppressMessage( "ReSharper", "InconsistentNaming" )]
public enum SignatureHashType
{
	/// <summary>
	/// HMAC using SHA-256
	/// </summary>
	HS256,

	/// <summary>
	/// HMAC using SHA-384
	/// </summary>
	HS384,

	/// <summary>
	/// HMAC using SHA-512
	/// </summary>
	HS512,

	/// <summary>
	/// RSASSA-PKCS1-v1_5 using SHA-256
	/// </summary>
	RS256,

	/// <summary>
	/// RSASSA-PKCS1-v1_5 using SHA-384
	/// </summary>
	RS384,

	/// <summary>
	/// RSASSA-PKCS1-v1_5 using SHA-512
	/// </summary>
	RS512,

	/// <summary>
	/// ECDSA using P-256 and SHA-256
	/// </summary>
	ES256,

	/// <summary>
	/// ECDSA using P-384 and SHA-384
	/// </summary>
	ES384,

	/// <summary>
	/// ECDSA using P-512 and SHA-512
	/// </summary>
	ES512,

	/// <summary>
	/// RSASSA-PSS using SHA-256 and MGF1 with SHA-256
	/// </summary>
	PS256,

	/// <summary>
	/// RSASSA-PSS using SHA-384 and MGF1 with SHA-384
	/// </summary>
	PS384,

	/// <summary>
	/// RSASSA-PSS using SHA-512 and MGF1 with SHA-512
	/// </summary>
	PS512
}