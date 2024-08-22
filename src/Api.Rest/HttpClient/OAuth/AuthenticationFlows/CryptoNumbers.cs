#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth.AuthenticationFlows;

#region usings

using IdentityModel;
using Zeiss.PiWeb.Api.Rest.Common.Utilities;

#endregion

/// <summary>
/// Represents cryptographically used numbers for authentication purposes.
/// </summary>
public record CryptoNumbers
{
	#region constructors

	/// <summary>
	/// Create a new set of cryptographic numbers.
	/// </summary>
	public CryptoNumbers()
	{
		Nonce = CryptoRandom.CreateUniqueId();
		State = CryptoRandom.CreateUniqueId();
		Verifier = CryptoRandom.CreateUniqueId();
		Challenge = Verifier.ToCodeChallenge();
	}

	#endregion

	#region properties

	/// <summary>
	/// A nonce for one time use.
	/// </summary>
	public string Nonce { get; }

	/// <summary>
	/// State of the authentication procedure.
	/// </summary>
	public string State { get; }

	/// <summary>
	/// Code verifier.
	/// </summary>
	public string Verifier { get; }

	/// <summary>
	/// Code challenge.
	/// </summary>
	public string Challenge { get; }

	#endregion
}