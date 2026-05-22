#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Caching;

using System;
using System.Net.Http;
using CacheCow.Client;
using CacheCow.Common;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Client;

/// <summary>
/// Factory for creating caching <see cref="DelegatingHandler"/> instances for caching HTTP requests and responses using CacheCow.
/// </summary>
public class CachingHandlerFactory : ICachingHandlerFactory
{
	#region members

	private readonly ICacheStore _CacheStore;
	private readonly IVaryHeaderStore _VaryHeaderStore;

	#endregion

	#region constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="CachingHandlerFactory"/> class.
	/// </summary>
	/// <param name="cacheStore">The cache store used to persist cached responses.</param>
	/// <param name="varyHeaderStore">An optional store for managing Vary header information.</param>
	public CachingHandlerFactory( ICacheStore cacheStore, [CanBeNull] IVaryHeaderStore varyHeaderStore )
	{
		_CacheStore = cacheStore ?? throw new ArgumentNullException( nameof( cacheStore ) );
		_VaryHeaderStore = varyHeaderStore;
	}

	#endregion

	#region interface ICachingHandlerFactory

	/// <inheritdoc />
	public DelegatingHandler CreateCachingHandler()
	{
		var cachingHandler = _VaryHeaderStore == null
				? new CachingHandler( _CacheStore )
				: new CachingHandler( _CacheStore, _VaryHeaderStore );

		cachingHandler.DoNotEmitCacheCowHeader = true;

		return cachingHandler;
	}

	#endregion
}
