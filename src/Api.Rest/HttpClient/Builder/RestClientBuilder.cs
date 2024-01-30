#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using CacheCow.Client;
using CacheCow.Common;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Client;
using Zeiss.PiWeb.Api.Rest.HttpClient.Data;
using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;
using Zeiss.PiWeb.Api.Rest.HttpClient.RawData;

/// <summary>
/// <inheritdoc cref="Zeiss.PiWeb.Api.Rest.HttpClient.Builder.IRestClientBuilder" />
/// Using this builder should be preferred over directly instantiating a rest client.
/// In its default setup this builder uses an internal http cache for any rest client build. This cache is disposed when this builder is
/// disposed. This means the builder needs to life longer then any rest client it builds. It is possible to use an external cache instead
/// by calling <see cref="EnableExternalHttpCaching"/>.
/// </summary>
public class RestClientBuilder : IRestClientBuilder, IDisposable
{
	// The internal cache store used when http caching is enabled and no external cache store is set.
	private readonly Lazy<ICacheStore> _InternalSharedCacheStore = new(
		() => new InMemoryCacheStore(),
		LazyThreadSafetyMode.ExecutionAndPublication );

	// The internal vary header store used when http caching is enabled and no external vary header store is set.
	private readonly Lazy<IVaryHeaderStore> _InternalSharedVaryHeaderStore = new(
		() => new InMemoryVaryHeaderStore(),
		LazyThreadSafetyMode.ExecutionAndPublication );

	// Factories used for creating a delegating handler chains
	[NotNull] private readonly List<Func<DelegatingHandler>> _DelegatingHandlerFactories = new List<Func<DelegatingHandler>>();

	/// <summary>
	/// The uri of the PiWeb Server to connect to. This uri must include a port and also an instance id if required.
	/// </summary>
	[NotNull] private readonly Uri _ServerUri;

	/// <summary>
	/// The timespan to wait before rest requests time out. Default value is 5 minutes.
	/// </summary>
	private TimeSpan _Timeout = RestClientBase.DefaultTimeout;

	/// <summary>
	/// The maximum length of URIs generated for rest requests. Default value is 8192.
	/// </summary>
	private int _MaxUriLength = RestClientBase.DefaultMaxUriLength;

	/// <summary>
	/// The maximum number of parallel requests that may occur when splitting requests because of the maximum uri length.
	/// Default value is 8.
	/// </summary>
	private int _MaxRequestsInParallel = 8;

	/// <summary>
	/// Indicates whether chunked transfer encoding should be used. Default value is <c>true</c>.
	/// </summary>
	private bool _AllowChunkedDataTransfer = true;

	/// <summary>
	/// The serializer to use for serializing and deserializing of data transfer objects. Default value is
	/// <see cref="ObjectSerializer.Default"/>.
	/// </summary>
	[NotNull] private IObjectSerializer _Serializer = ObjectSerializer.Default;

	/// <summary>
	/// Indicates whether http caching is enabled. Default value is <c>null</c>.
	/// </summary>
	private bool _HttpCachingEnabled = true;

	/// <summary>
	/// The external cache store to use for http caching or <c>null</c> if an internal cache store should be used.
	/// Default value is <c>null</c>.
	/// </summary>
	[CanBeNull] private ICacheStore _CacheStore;

	/// <summary>
	/// The external vary header store to use for http caching or <c>null</c> if an internal vary header store should be used.
	/// Default value is <c>null</c>.
	/// </summary>
	[CanBeNull] private IVaryHeaderStore _VaryHeaderStore;

	/// <summary>
	/// Specifies whether a system-wide http proxy setting will be respected.
	/// </summary>
	private bool _UseSystemProxy = true;

	/// <summary>
	/// Initializes a new instance of the <see cref="RestClientBuilder"/> class.
	/// </summary>
	/// <param name="serverUri">The uri of the rest services.</param>
	/// <exception cref="ArgumentNullException">Thrown when <paramref name="serverUri"/> is <c>null</c>.</exception>
	public RestClientBuilder( [NotNull] Uri serverUri )
	{
		if( serverUri == null )
			throw new ArgumentNullException( nameof( serverUri ) );

		_ServerUri = serverUri;
	}

	/// <summary>
	/// Creates an instance of <see cref="RestClientSettings"/> representing the current configuration of this builder.
	/// </summary>
	public RestClientSettings GetSettings()
	{
		return new RestClientSettings()
		{
			ServerUri = _ServerUri,
			Timeout = _Timeout,
			MaxUriLength = _MaxUriLength,
			MaxRequestsInParallel = _MaxRequestsInParallel,
			AllowChunkedDataTransfer = _AllowChunkedDataTransfer,
			Serializer = _Serializer,
			CacheStore = _CacheStore ?? ( _HttpCachingEnabled ? _InternalSharedCacheStore.Value : null ),
			VaryHeaderStore = _VaryHeaderStore ?? ( _HttpCachingEnabled ? _InternalSharedVaryHeaderStore.Value : null ),
			UseSystemProxy = _UseSystemProxy,
			DelegatingHandlerFactories = new List<Func<DelegatingHandler>>( _DelegatingHandlerFactories )
		};
	}

	/// <summary>
	/// Sets the timespan to wait before rest requests time out. Default value is 5 minutes.
	/// </summary>
	/// <param name="timeout">The timespan to wait.</param>
	public virtual RestClientBuilder SetTimeout(TimeSpan timeout)
	{
		_Timeout = timeout;
		return this;
	}

	/// <summary>
	/// Sets the timespan to wait before rest requests time out to a predefined value. Default timeout is 5 minutes.
	/// </summary>
	/// <param name="timeoutType">The standard timeout to set.</param>
	public virtual RestClientBuilder SetStandardTimeout(StandardTimeoutType timeoutType)
	{
		_Timeout = timeoutType switch
		{
			StandardTimeoutType.Default         => RestClientBase.DefaultTimeout,
			StandardTimeoutType.ConnectionCheck => RestClientBase.DefaultTestTimeout,
			StandardTimeoutType.ShortOperation  => RestClientBase.DefaultShortTimeout,
			StandardTimeoutType.Infinite        => Timeout.InfiniteTimeSpan,
			_                                   => throw new ArgumentOutOfRangeException( nameof( timeoutType ), timeoutType, null )
		};
		return this;
	}

	/// <summary>
	/// Sets the maximum length of URIs generated for rest requests. Default value is 8192.
	/// </summary>
	/// <param name="maxUriLength">The maximum length.</param>
	public virtual RestClientBuilder SetMaxUriLength(int maxUriLength)
	{
		_MaxUriLength = maxUriLength;
		return this;
	}

	/// <summary>
	/// Sets the maximum number of parallel requests that may occur when splitting requests because of the maximum uri length.
	/// Default value is 8.
	/// </summary>
	/// <param name="maxRequests">The maximum number of requests that may occur in parallel.</param>
	public virtual RestClientBuilder SetMaxRequestsInParallel(int maxRequests)
	{
		_MaxRequestsInParallel = maxRequests;
		return this;
	}

	/// <summary>
	/// Specifies whether chunked transfer encoding should be used. Default value is <c>true</c>.
	/// </summary>
	/// <param name="allowed"><c>True</c> if chunked transfer encoding should be used; otherwise, <c>false</c>.</param>
	public virtual RestClientBuilder SetAllowChunkedTransferEncoding(bool allowed)
	{
		_AllowChunkedDataTransfer = allowed;
		return this;
	}

	/// <summary>
	/// Sets a custom serializer implementation to use for serializing and deserializing of data transfer objects. Default value is
	/// <see cref="ObjectSerializer.SystemTextJsonSerializer"/>.
	/// </summary>
	/// <param name="serializer">The custom serializer implementation.</param>
	public virtual RestClientBuilder SetCustomSerializer( [NotNull] IObjectSerializer serializer)
	{
		_Serializer = serializer ?? throw new ArgumentNullException( nameof( serializer ) );
		return this;
	}

	/// <summary>
	/// Enables http caching using internal cache implementations shared for all rest clients created with this builder.
	/// This corresponds to the default state.
	/// </summary>
	/// <remarks>
	/// The shared internal cache is disposed when this builder is disposed. You need to make sure this builder lives longer than any
	/// rest client created by this builder. Use <see cref="EnableExternalHttpCaching"/> instead to have explicit control over the lifetime
	/// of the cache.
	/// </remarks>
	public virtual RestClientBuilder EnableStandardHttpCaching()
	{
		_HttpCachingEnabled = true;
		_CacheStore = null; // forces use of the internal cache store
		_VaryHeaderStore = null; // forces use of the internal vary header store

		return this;
	}

	/// <summary>
	/// Sets an externally provided cache store and very header store to be used as http cache.
	/// </summary>
	public virtual RestClientBuilder EnableExternalHttpCaching(
		[NotNull] ICacheStore cacheStore,
		[NotNull] IVaryHeaderStore varyHeaderStore)
	{
		_HttpCachingEnabled = true;
		_CacheStore = cacheStore ?? throw new ArgumentNullException( nameof( cacheStore ) );
		_VaryHeaderStore = varyHeaderStore ?? throw new ArgumentNullException( nameof( varyHeaderStore ) );

		return this;
	}

	/// <summary>
	/// Disables http caching entirely.
	/// </summary>
	public virtual RestClientBuilder DisableHttpCaching()
	{
		_HttpCachingEnabled = false;
		_CacheStore = null;
		_VaryHeaderStore = null;

		return this;
	}

	/// <summary>
	/// Specifies if a system wide http proxy should be used if such a proxy is set up for the system. The default value is <c>true</c>.
	/// </summary>
	public virtual RestClientBuilder SetUseSystemHttpProxy( bool useSystemProxy )
	{
		_UseSystemProxy = useSystemProxy;
		return this;
	}

	/// <summary>
	/// Adds a factory for delegating handlers. By adding factories, custom delegating handlers implementing additional functionality can
	/// be added to rest clients. When a new rest client is build by this builder, the given factories are used to create a chain
	/// of <see cref="DelegatingHandler"/>s to be used by the inner <see cref="HttpClient"/> instance on top of the internal http message
	/// handler.
	/// Any instance of a <see cref="DelegatingHandler"/> can only be part of a single chain. Avoid reusing any
	/// <see cref="DelegatingHandler"/> instances in your factories once they are part of a chain as this may result in exceptions or
	/// unpredictable behavior on other rest clients.
	/// </summary>
	/// <param name="factory">The delegating handler factory to add.</param>
	/// <exception cref="ArgumentNullException">Thrown when factory is <c>null</c>.</exception>
	public virtual RestClientBuilder AddDelegatingHandlerFactory( [NotNull] Func<DelegatingHandler> factory )
	{
		// Normally I would like to add the delegating handlers instances directly without any factory methods (similar to what
		// IHttpClientFactory does). However, currently we do not share a common HttpClient instance between different rest clients and
		// even create new HttpClient instances within a rest client on certain occasions. Using factories here allows us to build a new
		// chain per HttpClient instance.

		_DelegatingHandlerFactories.Add( factory ?? throw new ArgumentNullException( nameof( factory ) ) );
		return this;
	}

	/// <summary>
	/// Clears all delegating handler factories added previously. No additional delegating handlers are added to the chain.
	/// This is the default setup.
	/// </summary>
	public virtual RestClientBuilder ClearDelegatingHandlerFactories()
	{
		_DelegatingHandlerFactories.Clear();
		return this;
	}

	/// <inheritdoc />
	public DataServiceRestClient CreateDataServiceRestClient()
	{
		return new DataServiceRestClient( GetSettings() );
	}

	/// <inheritdoc />
	public RawDataServiceRestClient CreateRawDataServiceRestClient()
	{
		return new RawDataServiceRestClient( GetSettings() );
	}

	/// <inheritdoc />
	public OAuthServiceRestClient CreateOAuthServiceRestClient()
	{
		return new OAuthServiceRestClient( GetSettings() );
	}

	/// <summary>
	/// Performs cleanup.
	/// </summary>
	/// <param name="disposing">
	/// Indicates whether the method call comes from a Dispose method (<c>true</c>) or from a finalizer (<c>false</c>).
	/// </param>
	protected virtual void Dispose( bool disposing )
	{
		if( _InternalSharedCacheStore.IsValueCreated )
			_InternalSharedCacheStore.Value.Dispose();

		if( _InternalSharedVaryHeaderStore.IsValueCreated )
			_InternalSharedVaryHeaderStore.Value.Dispose();
	}

	/// <inheritdoc />
	public void Dispose()
	{
		Dispose( true );
		GC.SuppressFinalize( this );
	}
}