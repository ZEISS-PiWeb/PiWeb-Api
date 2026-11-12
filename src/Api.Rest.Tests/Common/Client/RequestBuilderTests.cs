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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

[TestFixture]
public sealed class RequestBuilderTests
{
	#region methods

	[Test]
	public async Task CreatePost_WithPayload_StreamsSerializedJsonBody()
	{
		var value = new { Name = "abc", Count = 3, Nothing = (string)null };
		var serializer = ObjectSerializer.Default;

		var request = RequestBuilder.CreatePost( "test", Payload.Create( value ) )( serializer, CancellationToken.None );

		using var expected = new MemoryStream();
		await serializer.SerializeAsync( expected, value, CancellationToken.None );
		var body = await request.Content!.ReadAsByteArrayAsync();
		body.Should().Equal( expected.ToArray() );
	}

	[Test]
	public void CreatePost_WithPayload_SetsJsonContentTypeAndChunkedLength()
	{
		var request = RequestBuilder.CreatePost( "test", Payload.Create( new { Name = "abc" } ) )( ObjectSerializer.Default, CancellationToken.None );

		request.Content!.Headers.ContentType!.MediaType.Should().Be( "application/json" );
		request.Content.Headers.ContentLength.Should().BeNull();
	}

	[Test]
	public async Task CreatePost_WithPayload_SendsChunkedJsonBodyOverHttp()
	{
		const string prefix = "http://localhost:8093/";
		var value = new { Name = "abc", Values = new[] { 1, 2, 3 } };
		var serializer = ObjectSerializer.Default;

		using var expected = new MemoryStream();
		await serializer.SerializeAsync( expected, value, CancellationToken.None );

		using var listener = new HttpListener();
		listener.Prefixes.Add( prefix );
		listener.Start();

		var serverTask = Task.Run( async () =>
		{
			var context = await listener.GetContextAsync();
			using var received = new MemoryStream();
			await context.Request.InputStream.CopyToAsync( received );
			var result = ( Body: received.ToArray(), context.Request.ContentType, TransferEncoding: context.Request.Headers[ "Transfer-Encoding" ] );
			context.Response.StatusCode = (int)HttpStatusCode.OK;
			context.Response.Close();
			return result;
		} );

		using var client = new HttpClient { BaseAddress = new Uri( prefix ) };
		using var request = RequestBuilder.CreatePost( "test", Payload.Create( value ) )( serializer, CancellationToken.None );
		using var response = await client.SendAsync( request );

		var (body, contentType, transferEncoding) = await serverTask;
		response.StatusCode.Should().Be( HttpStatusCode.OK );
		body.Should().Equal( expected.ToArray() );
		contentType.Should().Be( "application/json" );
		transferEncoding.Should().Be( "chunked" );
	}

	#endregion
}
