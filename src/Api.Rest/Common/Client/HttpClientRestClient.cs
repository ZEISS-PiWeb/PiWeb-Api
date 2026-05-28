#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client;

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Contracts;

/// <summary>
/// Provides a REST client implementation that uses a supplied HttpClient for sending HTTP requests, supporting
/// configurable URI length limits, chunked transfer encoding, and custom object serialization.
/// </summary>
/// <remarks><see cref="HttpClientRestClient"/> is designed for scenarios where you need to integrate with an existing HttpClient
/// instance, allowing for advanced configuration such as custom handlers, authentication, or proxy settings. Many
/// standard RestClient features, such as authentication management, timeout, and caching, must be configured directly
/// on the HttpClient or its handler chain. Properties related to these features will throw NotSupportedException if
/// accessed or set. This class is sealed and cannot be inherited.</remarks>
public sealed class HttpClientRestClient : RestClientBase
{
	#region members

	private readonly HttpClient _HttpClient;
	private readonly string _EndpointName;

	#endregion

	#region constructor

	/// <summary>
	/// Initializes a new instance of the <see cref="HttpClientRestClient"/> class with the specified HTTP client, maximum URI length,
	/// chunked transfer setting, and object serializer.
	/// </summary>
	/// <param name="httpClient">The HttpClient instance used to send HTTP requests.</param>
	/// <param name="endpointName">An optional endpoint name to prepend to request URIs. If provided, this will be combined with the request URI for each request.</param>
	/// <param name="maxUriLength">The maximum allowed length of the request URI. Requests exceeding this length may be split or handled differently.</param>
	/// <param name="chunked">A value indicating whether chunked transfer encoding is enabled for requests. If set to true, requests may be sent in chunks.</param>
	/// <param name="serializer">The object serializer used to serialize and deserialize request and response bodies.</param>
	public HttpClientRestClient(
		HttpClient httpClient,
		string endpointName = null,
		int maxUriLength = DefaultMaxUriLength,
		bool chunked = true,
		[CanBeNull] IObjectSerializer serializer = null )
		: base( maxUriLength, chunked, serializer ?? ObjectSerializer.Default )
	{
		_HttpClient = httpClient ?? throw new ArgumentNullException( nameof( httpClient ) );
		_EndpointName = endpointName;
	}

	#endregion

	#region methods

	/// <inheritdoc />
	protected override async Task<TResult> PerformRequestAsync<TResult>(
		Func<HttpRequestMessage> requestCreationHandler,
		bool streamed, Func<HttpResponseMessage, Task<TResult>> handler = null,
		bool autoDisposeResponse = true,
		CancellationToken cancellationToken = default )
	{
		HttpRequestMessage request = null;
		HttpResponseMessage response = null;

		try
		{
			var completionOptions = streamed ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead;

			request = requestCreationHandler();
			SetDefaultHttpHeaders( request );

			if( !string.IsNullOrWhiteSpace( _EndpointName ) )
				request.RequestUri = new Uri( _EndpointName.TrimEnd( '/' ) + "/" + request.RequestUri?.OriginalString?.TrimStart( '/' ), UriKind.Relative );

			response = await _HttpClient.SendAsync( request, completionOptions, cancellationToken ).ConfigureAwait( false );

			if( response.IsSuccessStatusCode )
			{
				if( handler != null )
				{
					return await handler( response ).ConfigureAwait( false );
				}

				return default;
			}

			await HandleFaultedResponse( response, cancellationToken ).ConfigureAwait( false );

			return default;
		}
		catch( HttpRequestException ex )
		{
			throw new RestClientException( $"Error fetching web service response for request [{request?.RequestUri}]: {ex.Message}", ex );
		}
		finally
		{
			if( autoDisposeResponse )
			{
				response?.Dispose();
			}
		}
	}

	#endregion

	#region interface IRestClient

	/// <inheritdoc />
	public override Uri ServiceLocation => new Uri( _HttpClient.BaseAddress ?? throw new InvalidOperationException( "HttpClient BaseAddress is not set." ), _EndpointName );

	#endregion
}
