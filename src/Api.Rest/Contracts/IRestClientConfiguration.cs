#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts;

#region usings

using System;
using CacheCow.Client;
using CacheCow.Common;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

/// <summary>
/// Defines the interface for a REST client configuration that can be used to configure various aspects of a REST client,
/// such as authentication, timeout, proxy settings, and caching behavior.
/// </summary>
public interface IRestClientConfiguration
{
	#region properties

	/// <summary>
	/// Gets or sets the authentication container used to manage authentication state and credentials.
	/// </summary>
	AuthenticationContainer AuthenticationContainer { get; set; }

	/// <summary>
	/// Gets or sets the maximum duration to wait before the operation times out.
	/// </summary>
	TimeSpan Timeout { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the default system web proxy is used for network requests.
	/// </summary>
	bool UseDefaultWebProxy { get; set; }

	/// <summary>
	/// Gets or sets a value indicating whether the certificate revocation list is checked during certificate validation.
	/// </summary>
	bool CheckCertificateRevocationList { get; set; }

	/// <summary>
	/// Gets or sets the cache store used for storing and retrieving cached data.
	/// </summary>
	ICacheStore CacheStore { get; set; }

	/// <summary>
	/// Gets or sets the store used to manage HTTP Vary header values for caching purposes.
	/// </summary>
	IVaryHeaderStore VaryHeaderStore { get; set; }

	#endregion

	#region events

	/// <summary>
	/// Occurs when the authentication state changes.
	/// </summary>
	event EventHandler AuthenticationChanged;

	#endregion
}
