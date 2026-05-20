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

		#region properties

		protected abstract IObjectSerializer Serializer { get; }

		protected abstract bool Chunked { get; }

		#endregion

		#region methods

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

		/// <summary>
		/// Reads and returns the response stream.
		/// </summary>
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

		protected void SetDefaultHttpHeaders( HttpRequestMessage request )
		{
			request.Headers.Accept.Add( new MediaTypeWithQualityHeaderValue( MimeTypeJson ) );
			request.Headers.AcceptEncoding.Add( new StringWithQualityHeaderValue( "gzip" ) );
			request.Headers.AcceptEncoding.Add( new StringWithQualityHeaderValue( "deflate" ) );
			request.Headers.UserAgent.Add( new ProductInfoHeaderValue( ClientIdHelper.ClientProduct, ClientIdHelper.ClientVersion ) );
			if( request.Content != null )
				request.Headers.TransferEncodingChunked = Chunked;
			request.Headers.Add( "Keep-Alive", "true" );

			if( !Equals( CultureInfo.CurrentUICulture, CultureInfo.InvariantCulture ) )
			{
				request.Headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentUICulture.IetfLanguageTag, 1.0 ) );
				request.Headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, 0.8 ) );
			}
		}

		protected static async Task HandleFaultedResponse( HttpResponseMessage response, IObjectSerializer serializer, CancellationToken cancellationToken )
		{
			await HandleClientBasedFaults( response, serializer, cancellationToken ).ConfigureAwait( false );
			HandleServerBasedFaults( response );
		}

		/// <summary>
		/// Handles all responses with status codes between 400 and 499
		/// </summary>
		protected static async Task HandleClientBasedFaults( HttpResponseMessage response, IObjectSerializer serializer, CancellationToken cancellationToken )
		{
			if( response.StatusCode < HttpStatusCode.BadRequest || response.StatusCode >= HttpStatusCode.InternalServerError )
				return;

			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				Error error;
				try
				{
					error = await serializer.DeserializeAsync<Error>( responseStream, cancellationToken ).ConfigureAwait( false )
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
		protected static void HandleServerBasedFaults( HttpResponseMessage response )
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
		public abstract int MaxUriLength { get; }


		/// <inheritdoc />
		public abstract Uri ServiceLocation
		{
			get;
		}

		public Task Request( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( requestCreationHandler, false, null, true, cancellationToken );
		}

		public Task Request( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( () => requestCreationHandler( Serializer, cancellationToken ), false, null, true, cancellationToken );
		}

		public Task<T> Request<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, response => ResponseToObjectAsync<T>( response, Serializer, cancellationToken ), true, cancellationToken );
		}

		public Task<T> Request<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( Serializer, cancellationToken ), false, response => ResponseToObjectAsync<T>( response, Serializer, cancellationToken ), true, cancellationToken );
		}

		public Task<Stream> RequestStream( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, true, ResponseToStreamAsync, false, cancellationToken );
		}

		public Task<Stream> RequestStream( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( Serializer, cancellationToken ), true, ResponseToStreamAsync, false, cancellationToken );
		}

		public async IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken )
		{
			var items = await PerformRequestAsync( requestCreationHandler, true, response => Task.FromResult( ResponseToAsyncEnumerable<T>( response, Serializer, cancellationToken ) ), false, cancellationToken );

			await foreach( var item in items.ConfigureAwait( false ) )
			{
				yield return item;
			}
		}

		public async IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken )
		{
			var items = await PerformRequestAsync( () => requestCreationHandler( Serializer, cancellationToken ), true, response => Task.FromResult( ResponseToAsyncEnumerable<T>( response, Serializer, cancellationToken ) ), false, cancellationToken );

			await foreach( var item in items.ConfigureAwait( false ) )
			{
				yield return item;
			}
		}

		public Task<byte[]> RequestBytes( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, ResponseToBytesAsync, true, cancellationToken );
		}

		public Task<byte[]> RequestBytes( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( Serializer, cancellationToken ), false, ResponseToBytesAsync, true, cancellationToken );
		}

		public Task<T> RequestBinary<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, BinaryResponseToObjectAsync<T>, true, cancellationToken );
		}

		public Task<T> RequestBinary<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( Serializer, cancellationToken ), false, BinaryResponseToObjectAsync<T>, true, cancellationToken );
		}

		/// <summary>
		/// Start an async operation. Returns an URL to poll for status updates if the operation is accepted, otherwise the operation is done synchronously.
		/// </summary>
		/// <returns>Returns a Task that represents the duration of the initial REST request. The result of the task contains
		/// the URI for polling the operation result or null in case the server already finished the request synchronously.</returns>
		/// <exception cref="RestClientException">The response indicated status Accepted, but did not contain polling information.</exception>
		public Task<Uri> RequestAsyncOperation( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, LocationHeaderToUrl, true, cancellationToken );
		}

		/// <summary>
		/// Start an async operation. Returns an URL to poll for status updates if the operation is accepted, otherwise the operation is done synchronously.
		/// </summary>
		/// <returns>Returns a Task that represents the duration of the initial REST request. The result of the task contains
		/// the URI for polling the operation result or null in case the server already finished the request synchronously.</returns>
		/// <exception cref="RestClientException">The response indicated status Accepted, but did not contain polling information.</exception>
		public Task<Uri> RequestAsyncOperation( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler( Serializer ), false, LocationHeaderToUrl, true, cancellationToken );
		}

		#endregion
	}
}
