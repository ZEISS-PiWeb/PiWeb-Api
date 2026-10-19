#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.Common.Client;

#region usings

using System;
using System.Net.Http;
using FluentAssertions;
using NUnit.Framework;
using Zeiss.PiWeb.Api.Rest.Common.Client;
using Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

#endregion

[TestFixture]
public sealed class RestClientTests
{
	#region methods

	[Test]
	public void Constructor_CachingEnabled_CreatesCachingHandler()
	{
		var cachingHandlerFactory = new TestCachingHandlerFactory();

		using var restClient = new RestClient( new Uri( "https://localhost:8080" ), "DataServiceRest/" );
		restClient.CachingHandlerFactory = cachingHandlerFactory;

		cachingHandlerFactory.CreateCachingHandlerCallCount.Should().Be( 1 );
	}

	[Test]
	public void CreateDataServiceRestClient_CachingEnabled_CreatesCachingHandler()
	{
		var cachingHandlerFactory = new TestCachingHandlerFactory();

		using var restClient = new RestClientBuilder( new Uri( "http://localhost/" ) )
			.EnableHttpCaching( cachingHandlerFactory )
			.CreateDataServiceRestClient();

		cachingHandlerFactory.CreateCachingHandlerCallCount.Should().Be( 1 );
	}

	#endregion

	#region class TestCachingHandlerFactory

	private sealed class TestCachingHandlerFactory : ICachingHandlerFactory
	{
		#region properties

		public int CreateCachingHandlerCallCount { get; private set; }

		#endregion

		#region interface ICachingHandlerFactory

		public DelegatingHandler CreateCachingHandler()
		{
			CreateCachingHandlerCallCount++;
			return new TestDelegatingHandler();
		}

		#endregion
	}

	#endregion

	#region class TestDelegatingHandler

	private sealed class TestDelegatingHandler : DelegatingHandler;

	#endregion
}