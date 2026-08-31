#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Runtime.CompilerServices;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	/// <summary>
	/// Provides a base implementation for REST client functionality, including methods for sending HTTP requests, handling
	/// responses, and managing serialization. Intended to be extended by concrete REST client implementations.
	/// </summary>
	public abstract class RestClientBase : IRestClient
	{
		#region constants

		/// <summary>Mimetype für JSON</summary>
		public const string MimeTypeJson = "application/json";

		/// <summary>
		/// Default maximum length of the full URL inclusive any query string
		/// </summary>
		public const int DefaultMaxUriLength = 8 * 1024;

		/// <summary>
		/// Maximum length a path segment within an uri my have
		/// </summary>
		public const int MaximumPathSegmentLength = 255;

		#endregion

		#region members

		private bool _Chunked;
		private IObjectSerializer _Serializer;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientBase"/> class.
		/// </summary>
		/// <param name="maxUriLength">The maximum allowed length of the request URI. Requests exceeding this length may be split or handled differently.</param>
		/// <param name="chunked">A value indicating whether HTTP requests should use chunked transfer encoding.</param>
		/// <param name="serializer">The serializer used to convert objects to and from the request and response payloads.</param>
		protected RestClientBase( int maxUriLength, bool chunked, [NotNull] IObjectSerializer serializer )
		{
			MaxUriLength = maxUriLength;

			_Chunked = chunked;
			_Serializer = serializer ?? throw new ArgumentNullException( nameof( serializer ) );
		}

		#endregion

		#region methods

		/// <summary>
		/// Performs the actual request by invoking the given request creation handler and processing the response with the given response handler.
		/// The implementation of this method is responsible for handling all aspects of the request execution,
		/// including but not limited to: sending the request, handling retries, processing the response, and disposing of resources as necessary.
		/// </summary>
		/// <typeparam name="TResult">The type of the result returned by the response handler.</typeparam>
		/// <param name="requestCreationHandler">A delegate that creates and returns the <see cref="HttpRequestMessage"/> to be sent.</param>
		/// <param name="streamed">Indicates whether the response should be streamed.</param>
		/// <param name="handler">A delegate that processes the <see cref="HttpResponseMessage"/> and returns a result.</param>
		/// <param name="autoDisposeResponse">Indicates whether the response should be automatically disposed after processing.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
		/// <returns>A task that represents the asynchronous operation of sending the HTTP request and processing the response.</returns>
		protected abstract Task<TResult> PerformRequestAsync<TResult>(
			Func<HttpRequestMessage> requestCreationHandler,
			bool streamed, Func<HttpResponseMessage, Task<TResult>> handler = null,
			bool autoDisposeResponse = true,
			CancellationToken cancellationToken = default );

		private static async Task<T> ResponseToObjectAsync<T>( HttpResponseMessage response, IObjectSerializer serializer, CancellationToken cancellationToken )
		{
			if( response.StatusCode != HttpStatusCode.NoContent )
			{
				using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
				{
					return await serializer.DeserializeAsync<T>( responseStream, cancellationToken ).ConfigureAwait( false );
				}
			}

			return default;
		}

		private static Task<Stream> ResponseToStreamAsync( HttpResponseMessage response )
		{
			return response.Content.ReadAsStreamAsync();
		}

		private static async IAsyncEnumerable<T> ResponseToAsyncEnumerable<T>( HttpResponseMessage response, IObjectSerializer serializer, [EnumeratorCancellation] CancellationToken cancellationToken )
		{
			using( response )
			{
				using( var responseStream = await ResponseToStreamAsync( response ).ConfigureAwait( false ) )
				{
					await foreach( var item in serializer.DeserializeAsyncEnumerable<T>( responseStream, cancellationToken ).ConfigureAwait( false ) )
					{
						yield return item;
					}
				}
			}
		}

		private static async Task<byte[]> ResponseToBytesAsync( HttpResponseMessage response )
		{
			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				if( response.Content.Headers.ContentLength.HasValue )
				{
					using( var memStream = new MemoryStream( new byte[ (int)response.Content.Headers.ContentLength.Value ], 0, (int)response.Content.Headers.ContentLength.Value, true, true ) )
					{
						await responseStream.CopyToAsync( memStream ).ConfigureAwait( false );
						return memStream.GetBuffer();
					}
				}

				using( var memStream = new MemoryStream() )
				{
					await responseStream.CopyToAsync( memStream ).ConfigureAwait( false );
					return memStream.ToArray();
				}
			}
		}

		private static async Task<T> BinaryResponseToObjectAsync<T>( HttpResponseMessage response )
		{
			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				return RestClientHelper.DeserializeBinaryObject<T>( responseStream );
			}
		}

		private static Task<Uri> LocationHeaderToUrl( HttpResponseMessage response )
		{
			var result = response.Headers.Location;
			if( result == null && response.StatusCode == HttpStatusCode.Accepted )
				throw new RestClientException( $"Error fetching status URL: The response indicated status Accepted, but did not contain polling information." );

			return Task.FromResult( result );
		}

		/// <summary>
		/// Configures the specified HTTP request message with standard default headers.
		/// </summary>
		/// <param name="request">The HTTP request message to which default headers will be applied. Cannot be null.</param>
		protected void SetDefaultHttpHeaders( HttpRequestMessage request )
		{
			request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( MimeTypeJson ) );
			request.Headers.AcceptEncoding.Add( new StringWithQualityHeaderValue( "gzip" ) );
			request.Headers.AcceptEncoding.Add( new StringWithQualityHeaderValue( "deflate" ) );
			request.Headers.UserAgent.Add( new ProductInfoHeaderValue( ClientIdHelper.ClientProduct, ClientIdHelper.ClientVersion ) );
			if( request.Content != null )
				request.Headers.TransferEncodingChunked = _Chunked;
			request.Headers.Add( "Keep-Alive", "true" );

			if( !Equals( CultureInfo.CurrentUICulture, CultureInfo.InvariantCulture ) )
			{
				request.Headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentUICulture.IetfLanguageTag, 1.0 ) );
				request.Headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, 0.8 ) );
			}
		}

		/// <summary>
		/// Processes an HTTP response to detect and handle fault conditions based on client or server errors.
		/// </summary>
		/// <param name="response">The HTTP response message to inspect for faulted states.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
		/// <returns>A task that represents the asynchronous operation.</returns>
		protected async Task HandleFaultedResponse( HttpResponseMessage response, CancellationToken cancellationToken )
		{
			await HandleClientBasedFaults( response, cancellationToken ).ConfigureAwait( false );
			HandleServerBasedFaults( response );
		}

		/// <summary>
		/// Handles all responses with status codes between 400 and 499
		/// </summary>
		private async Task HandleClientBasedFaults( HttpResponseMessage response, CancellationToken cancellationToken )
		{
			if( response.StatusCode < HttpStatusCode.BadRequest || response.StatusCode >= HttpStatusCode.InternalServerError )
				return;

			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				Error error;
				try
				{
					error = await _Serializer.DeserializeAsync<Error>( responseStream, cancellationToken ).ConfigureAwait( false )
							?? new Error( $"Request {response.RequestMessage.Method} {response.RequestMessage.RequestUri} was not successful: {(int)response.StatusCode} ({response.ReasonPhrase})" );
				}
				catch( ObjectSerializerException )
				{
					var buffer = ( responseStream as MemoryStream )?.ToArray();
					var content = buffer != null ? Encoding.UTF8.GetString( buffer ) : null;
					error = new Error( $"Request {response.RequestMessage.Method} {response.RequestMessage.RequestUri} was not successful: {(int)response.StatusCode} ({response.ReasonPhrase})" )
					{
						ExceptionMessage = content
					};
				}
				catch( Exception )
				{
					error = new Error( $"Request {response.RequestMessage.Method} {response.RequestMessage.RequestUri} was not successful: {(int)response.StatusCode} ({response.ReasonPhrase})" );
				}

				throw new WrappedServerErrorException( error, response );
			}
		}

		/// <summary>
		/// Handles all responses with status codes between 500 and 505
		/// </summary>
		private static void HandleServerBasedFaults( HttpResponseMessage response )
		{
			if( response.StatusCode < HttpStatusCode.InternalServerError || response.StatusCode >= HttpStatusCode.HttpVersionNotSupported )
				return;

			var error = new Error( response.ReasonPhrase );

			if( WrappedServerErrorExceptionExtensions.ServerBasedExceptions.TryGetValue( response.StatusCode, out var exceptionType ) )
				error.ExceptionType = exceptionType;

			throw new WrappedServerErrorException( error, response );
		}

		#endregion

		#region interface IRestClient

		/// <inheritdoc />
		public int MaxUriLength { get; }


		/// <inheritdoc />
		public abstract Uri ServiceLocation
		{
			get;
		}

		/// <inheritdoc />
		public Task Request( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( requestCreationHandler, false, null, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task Request( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( () => requestCreationHandler( _Serializer, cancellationToken ), false, null, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<T> Request<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, response => ResponseToObjectAsync<T>( response, _Serializer, cancellationToken ), true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<T> Request<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( _Serializer, cancellationToken ), false, response => ResponseToObjectAsync<T>( response, _Serializer, cancellationToken ), true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<Stream> RequestStream( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, true, ResponseToStreamAsync, false, cancellationToken );
		}

		/// <inheritdoc />
		public Task<Stream> RequestStream( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( _Serializer, cancellationToken ), true, ResponseToStreamAsync, false, cancellationToken );
		}

		/// <inheritdoc />
		public async IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken )
		{
			var items = await PerformRequestAsync( requestCreationHandler, true, response => Task.FromResult( ResponseToAsyncEnumerable<T>( response, _Serializer, cancellationToken ) ), false, cancellationToken );

			await foreach( var item in items.ConfigureAwait( false ) )
			{
				yield return item;
			}
		}

		/// <inheritdoc />
		public async IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken )
		{
			var items = await PerformRequestAsync( () => requestCreationHandler( _Serializer, cancellationToken ), true, response => Task.FromResult( ResponseToAsyncEnumerable<T>( response, _Serializer, cancellationToken ) ), false, cancellationToken );

			await foreach( var item in items.ConfigureAwait( false ) )
			{
				yield return item;
			}
		}

		/// <inheritdoc />
		public Task<byte[]> RequestBytes( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, ResponseToBytesAsync, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<byte[]> RequestBytes( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( _Serializer, cancellationToken ), false, ResponseToBytesAsync, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<T> RequestBinary<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, BinaryResponseToObjectAsync<T>, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<T> RequestBinary<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( _Serializer, cancellationToken ), false, BinaryResponseToObjectAsync<T>, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<Uri> RequestAsyncOperation( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, LocationHeaderToUrl, true, cancellationToken );
		}

		/// <inheritdoc />
		public Task<Uri> RequestAsyncOperation( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( _Serializer ), false, LocationHeaderToUrl, true, cancellationToken );
		}

		#endregion
	}
}
