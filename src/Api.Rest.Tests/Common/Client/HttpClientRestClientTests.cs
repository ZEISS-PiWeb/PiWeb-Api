#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests.Common.Client;

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;
using Zeiss.PiWeb.Api.Rest.Common.Client;
using Zeiss.PiWeb.Api.Rest.Contracts;
using Zeiss.PiWeb.Api.Rest.Dtos;

[TestFixture]
[NonParallelizable]
public sealed class HttpClientRestClientTests
{
	#region constants

	private const int Port = 8081;

	#endregion

	#region members

	private static readonly Uri Uri = new( $"http://localhost:{Port}/" );

	#endregion

	#region methods

	[Test]
	public void ServiceLocation_BaseAddressIsSet_ReturnsBaseAddress()
	{
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient );

		client.ServiceLocation.Should().Be( Uri );
	}

	[Test]
	public void ServiceLocation_BaseAddressAndEndpointNameIsSet_ReturnsBaseAddress()
	{
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient, endpointName: $"api/" );

		client.ServiceLocation.Should().Be( $"{Uri}api/" );
	}

	[Test]
	public void ServiceLocation_BaseAddressIsNotSet_ThrowsInvalidOperationException()
	{
		using var httpClient = new HttpClient();
		var client = new HttpClientRestClient( httpClient );

		client.Invoking( c => c.ServiceLocation )
			.Should()
			.Throw<InvalidOperationException>()
			.WithMessage( "HttpClient BaseAddress is not set." );
	}

	[Test]
	public async Task Request_ResponseIsSuccessful_ReturnsDeserializedObject()
	{
		using var server = WebServer.StartNew( Port );
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient );
		var expected = new Error( "everything is fine" );
		server.RegisterResponse( "/items/42", JsonConvert.SerializeObject( expected ) );

		var result = await client.Request<Error>(
			() => new HttpRequestMessage( HttpMethod.Get, "items/42" ),
			CancellationToken.None );

		result.Should().BeEquivalentTo( expected );
		server.ReceivedRequests.Should().ContainSingle().Which.Should().Be( $"{Uri}items/42" );
	}

	[Test]
	public async Task RequestStream_ResponseIsSuccessful_ReturnsReadableStream()
	{
		using var server = WebServer.StartNew( Port );
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient );
		const string expectedContent = "stream-content";
		server.RegisterResponse( "/stream", expectedContent );

		using var stream = await client.RequestStream(
			() => new HttpRequestMessage( HttpMethod.Get, "stream" ),
			CancellationToken.None );
		using var reader = new StreamReader( stream );

		var result = await reader.ReadToEndAsync();

		result.Should().Be( expectedContent );
	}

	[Test]
	public async Task Request_EndpointNameIsSpecified_PrependsEndpointName()
	{
		using var server = WebServer.StartNew( Port );
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient, endpointName: "api/" );
		var expected = new Error( "from api endpoint" );
		server.RegisterResponse( "/api/items/42", JsonConvert.SerializeObject( expected ) );

		var result = await client.Request<Error>(
			() => new HttpRequestMessage( HttpMethod.Get, "items/42" ),
			CancellationToken.None );

		result.Should().BeEquivalentTo( expected );
		server.ReceivedRequests.Should().ContainSingle().Which.Should().Be( $"{Uri}api/items/42" );
	}

	[Test]
	public async Task Request_ResponseIsClientError_ThrowsWrappedServerErrorException()
	{
		using var server = WebServer.StartNew( Port );
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient );
		var expectedError = new Error( "request failed" ) { ExceptionMessage = "validation error" };
		server.RegisterResponse( "/items/42", JsonConvert.SerializeObject( expectedError ), HttpStatusCode.BadRequest );

		Func<Task> act = () => client.Request(
			() => new HttpRequestMessage( HttpMethod.Get, "items/42" ),
			CancellationToken.None );

		var exception = await act.Should().ThrowAsync<WrappedServerErrorException>();

		exception.Which.StatusCode.Should().Be( HttpStatusCode.BadRequest );
		exception.Which.Error.Should().BeEquivalentTo( expectedError );
		exception.Which.Response.Should().NotBeNull();
		exception.Which.Request.Should().NotBeNull();
	}

	[Test]
	public async Task Request_ResponseIsServerError_ThrowsWrappedServerErrorException()
	{
		using var server = WebServer.StartNew( Port );
		using var httpClient = new HttpClient { BaseAddress = Uri };
		var client = new HttpClientRestClient( httpClient );
		server.RegisterResponse( "/items/42", string.Empty, HttpStatusCode.InternalServerError );

		Func<Task> act = () => client.Request(
			() => new HttpRequestMessage( HttpMethod.Get, "/items/42" ),
			CancellationToken.None );

		var exception = await act.Should().ThrowAsync<WrappedServerErrorException>();

		exception.Which.StatusCode.Should().Be( HttpStatusCode.InternalServerError );
		exception.Which.Error.Should().NotBeNull();
	}

	#endregion
}
