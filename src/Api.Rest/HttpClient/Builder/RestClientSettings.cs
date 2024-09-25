#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

#region usings

using System;
using System.Collections.Generic;
using System.Net.Http;
using CacheCow.Client;
using CacheCow.Common;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Authentication;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

/// <summary>
/// Represents settings for a PiWeb rest client.
/// </summary>
public record RestClientSettings
{
	#region properties

	/// <summary>
	/// The uri of the PiWeb Server to connect to. This uri must include a port and also an instance id if required.
	/// </summary>
	[NotNull]
	public Uri ServerUri { get; set; } = new Uri( "http://localhost/" );

	/// <summary>
	/// The timespan to wait before rest requests time out.
	/// </summary>
	public TimeSpan Timeout { get; set; } = RestClientBase.DefaultTimeout;

	/// <summary>
	/// The maximum length of URIs generated for rest requests.
	/// </summary>
	public int MaxUriLength { get; set; } = RestClientBase.DefaultMaxUriLength;

	/// <summary>
	/// The maximum number of parallel requests that may occur when splitting requests because of the maximum uri length.
	/// </summary>
	public int MaxRequestsInParallel { get; set; } = 8;

	/// <summary>
	/// Indicates whether chunked transfer encoding should be used.
	/// </summary>
	public bool AllowChunkedDataTransfer { get; set; } = true;

	/// <summary>
	/// The serializer to use for serializing and deserializing of data transfer objects.
	/// </summary>
	[NotNull]
	public IObjectSerializer Serializer { get; set; } = ObjectSerializer.Default;

	/// <summary>
	/// The external cache store to use for http caching or <c>null</c> if an internal cache store should be used.
	/// </summary>
	[CanBeNull]
	public ICacheStore CacheStore { get; set; }

	/// <summary>
	/// The external vary header store to use for http caching or <c>null</c> if an internal vary header store should be used.
	/// </summary>
	[CanBeNull]
	public IVaryHeaderStore VaryHeaderStore { get; set; }

	/// <summary>
	/// Specifies whether a system-wide http proxy setting will be respected.
	/// </summary>
	public bool UseSystemProxy { get; set; } = true;

	/// <summary>
	/// Specifies whether the certificate is checked against the certificate authority revocation list.
	/// </summary>
	public bool CheckCertificateRevocationList { get; set; } = false;

	/// <summary>
	/// The delegating handler factories to use for creating delegating handler chains.
	/// </summary>
	public IReadOnlyCollection<Func<DelegatingHandler>> DelegatingHandlerFactories { get; set; }

	/// <summary>
	/// The authentication handler or <c>null</c> when no handler is set.
	/// </summary>
	[CanBeNull]
	public IAuthenticationHandler AuthenticationHandler { get; set; }

	#endregion
}