#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2019                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

#if NETFRAMEWORK
using System.Net.Cache;
#endif

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	public abstract class RestClientBase : IDisposable
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

		/// <summary>
		/// Default-Timeout of 5 minutes, which is used by RestClient if no timeout is given explicitly.
		/// </summary>
		public static readonly TimeSpan DefaultTimeout = TimeSpan.FromMinutes( 5 );

		/// <summary>
		/// Default-Timeout which should be used for connection checks or other simple operations
		/// </summary>
		public static readonly TimeSpan DefaultTestTimeout = TimeSpan.FromSeconds( 5 );

		/// <summary>
		/// Default-Timeout which should be used for short running operations.
		/// </summary>
		public static readonly TimeSpan DefaultShortTimeout = TimeSpan.FromSeconds( 15 );

		private readonly bool _Chunked;

		private HttpClient _HttpClient;
		private HttpClientHandler _WebRequestHandler;
		private bool _IsDisposed;
		private AuthenticationContainer _AuthenticationContainer = new AuthenticationContainer( AuthenticationMode.NoneOrBasic );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientBase"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		protected RestClientBase( [NotNull] Uri serverUri, string endpointName, TimeSpan? timeout = null, int maxUriLength = DefaultMaxUriLength, bool chunked = true )
		{
			if( serverUri == null )
				throw new ArgumentNullException( nameof( serverUri ) );

			ServiceLocation = new UriBuilder( serverUri )
			{
				Path = ( serverUri.AbsolutePath.Replace( "/DataServiceSoap", "" ) + "/" + endpointName ).Replace( "//", "/" )
			}.Uri;

			_Chunked = chunked;

			MaxUriLength = maxUriLength;

			BuildHttpClient( timeout );
		}

		#endregion

		#region events

		public event EventHandler AuthenticationChanged;

		#endregion

		#region properties

		public int MaxUriLength { get; }

		/// <summary>
		/// Gets or sets the request timeout.
		/// </summary>
		public TimeSpan Timeout
		{
			get => _HttpClient.Timeout;
			set
			{
				if( _HttpClient.Timeout != value )
				{
					_HttpClient.Timeout = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets if system default proxy should be used.
		/// </summary>
		public bool UseDefaultWebProxy
		{
			get => _WebRequestHandler.UseProxy;
			set
			{
				if( _WebRequestHandler.UseProxy != value )
				{
					_WebRequestHandler.UseProxy = value;
				}
			}
		}

		/// <summary>
		/// Returns the endpoint address of the webservice.
		/// </summary>
		public Uri ServiceLocation { get; }

		public AuthenticationContainer AuthenticationContainer
		{
			get => _AuthenticationContainer;
			set
			{
				if( value == null ) throw new ArgumentNullException( nameof( value ) );

				if( _AuthenticationContainer == value ) return;

				_AuthenticationContainer = value;
				UpdateAuthenticationInformation();
			}
		}

		#endregion

		#region methods

		public Task Request( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( requestCreationHandler, false, null, true, cancellationToken );
		}

		public Task<T> Request<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, ResponseToObjectAsync<T>, true, cancellationToken );
		}

		public Task<Stream> RequestStream( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, true, ResponseToStreamAsync, false, cancellationToken );
		}

		public Task<IEnumerable<T>> RequestEnumerated<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, true, ResponseToEnumerationAsync<T>, false, cancellationToken );
		}

		public Task<byte[]> RequestBytes( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, ResponseToBytesAsync, true, cancellationToken );
		}

		public Task<T> RequestBinary<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, BinaryResponseToObjectAsync<T>, true, cancellationToken );
		}

		private static async Task<T> ResponseToObjectAsync<T>( HttpResponseMessage response )
		{
			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				return RestClientHelper.DeserializeObject<T>( responseStream );
			}
		}

		private static async Task<IEnumerable<T>> ResponseToEnumerationAsync<T>( HttpResponseMessage response )
		{
			var stream = await ResponseToStreamAsync( response ).ConfigureAwait( false );
			return GetEnumeratedResponse<T>( response, stream );
		}

		private static IEnumerable<T> GetEnumeratedResponse<T>( IDisposable response, Stream responseStream )
		{
			using( response )
			{
				using( responseStream )
				{
					foreach( var item in RestClientHelper.DeserializeEnumeratedObject<T>( responseStream ) )
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

		private async Task<TResult> PerformRequestAsync<TResult>(
			Func<HttpRequestMessage> requestCreationHandler,
			bool streamed, Func<HttpResponseMessage, Task<TResult>> handler = null,
			bool autoDisposeResponse = true,
			CancellationToken cancellationToken = default )
		{
			HttpRequestMessage request = null;
			HttpResponseMessage response = null;

			try
			{
				await CheckAuthenticationInformationAsync( cancellationToken ).ConfigureAwait( false );

				var completionOptions = streamed ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead;

				while( true )
				{
					request = requestCreationHandler();
					SetDefaultHttpHeaders( request );
					response = await _HttpClient.SendAsync( request, completionOptions, cancellationToken ).ConfigureAwait( false );

					if( response.IsSuccessStatusCode )
					{
						if( handler != null )
						{
							return await handler( response ).ConfigureAwait( false );
						}

						return default;
					}

					if( await UpdateAuthenticationInformationAsync( response, cancellationToken ).ConfigureAwait( false ) )
					{
						response.Dispose();
						continue;
					}

					await HandleFaultedResponse( response ).ConfigureAwait( false );
				}
			}
			catch( HttpRequestException ex )
			{
				throw new RestClientException( $"Error fetching web service response for request [{request?.RequestUri}]: {ex.Message}", ex );
			}
			catch( TaskCanceledException ex ) when( !cancellationToken.IsCancellationRequested )
			{
				// we expect the TaskCanceledException to be a timeout if the passed token has not been canceled
				throw new TimeoutException( "Timeout reached", ex );
			}
			finally
			{
				if( autoDisposeResponse )
				{
					response?.Dispose();
				}
			}
		}

		/// <summary>
		/// Checks for complete authentication information before doing the request.
		/// </summary>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
		/// <returns>True if the authentication information was updated; otherwise, false.</returns>
		/// <remarks>This callback can be used to show a login screen to the user for authentication.</remarks>
		protected virtual Task<bool> CheckAuthenticationInformationAsync( CancellationToken cancellationToken = default )
		{
			return Task.FromResult( false );
		}

		/// <summary>
		/// Updates the authentication information based on the given <paramref name="response"/>.
		/// </summary>
		/// <param name="response">For checking the reasons for the unsuccessful request.</param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
		/// <returns>True if the authentication information was updated; otherwise, false.</returns>
		/// <remarks>This callback can be used to show a login screen to the user for authentication.</remarks>
		protected virtual Task<bool> UpdateAuthenticationInformationAsync( HttpResponseMessage response, CancellationToken cancellationToken = default )
		{
			return Task.FromResult( false );
		}

		private void SetDefaultHttpHeaders( HttpRequestMessage request )
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
		/// Reads and returns the response stream.
		/// </summary>
		private static Task<Stream> ResponseToStreamAsync( HttpResponseMessage response )
		{
			return response.Content.ReadAsStreamAsync();
		}

		private static async Task HandleFaultedResponse( HttpResponseMessage response )
		{
			await HandleClientBasedFaults( response ).ConfigureAwait( false );
			HandleServerBasedFaults( response );
		}

		/// <summary>
		/// Handles all responses with status codes between 400 and 499
		/// </summary>
		private static async Task HandleClientBasedFaults( HttpResponseMessage response )
		{
			if( response.StatusCode < HttpStatusCode.BadRequest || response.StatusCode >= HttpStatusCode.InternalServerError )
				return;

			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				Error error;
				try
				{
					error = RestClientHelper.DeserializeObject<Error>( responseStream )
							?? new Error( $"Request {response.RequestMessage.Method} {response.RequestMessage.RequestUri} was not successful: {(int)response.StatusCode} ({response.ReasonPhrase})" );
				}
				catch( JsonReaderException )
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

		private void RebuildHttpClient()
		{
			var timeout = _HttpClient.Timeout;

			_HttpClient?.Dispose();
			_WebRequestHandler?.Dispose();

			BuildHttpClient( timeout );
		}

		private void BuildHttpClient( TimeSpan? timeout )
		{
#if NETFRAMEWORK
			_WebRequestHandler = new WebRequestHandler
			{
				CachePolicy = new HttpRequestCachePolicy( HttpCacheAgeControl.MaxAge, TimeSpan.FromDays( 0 ) ),
				AllowPipelining = true,
				PreAuthenticate = true,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
				// PIWEB-8519
				// When using WindowsAuthentication, PiWeb Clients create lots of sockets in TIME_WAIT state.
				// During QDB export with RawData this may lead to exhaustion of ephemeral ports for new connections
				// resulting in exceptions.
				// The following assignment disables closing of the socket connection but has a few security
				// implications. After careful review I think these are not relevant for us currently.
				// Therefore it should be safe for us to switch this on.
				// Review:
				// - HttpWebRequest.ConnectionGroupName is set to a hash of the instance hash code
				// - PiWeb Clients do not do impersonation
				// - PiWeb Clients are using single sign on exclusively, which means no user B can hijack a connection of user A
				// - Inspecting Client/Server communication with Fiddler reveals that every request does a 401 roundtrip,
				//     i.e. the Server is already authenticating every single request
				UnsafeAuthenticatedConnectionSharing = true
			};
#else
			_WebRequestHandler = new HttpClientHandler
			{
				PreAuthenticate = true,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
			};
#endif

			_HttpClient = new HttpClient( _WebRequestHandler )
			{
				Timeout = timeout ?? DefaultTimeout,
				BaseAddress = ServiceLocation
			};
		}

		private void UpdateAuthenticationInformation()
		{
			_WebRequestHandler.ClientCertificates.Clear();
			_HttpClient.DefaultRequestHeaders.Authorization = null;

			switch( _AuthenticationContainer.Mode )
			{
				case AuthenticationMode.NoneOrBasic:
					UpdateUseWindowsCredentials( false );
					if( _AuthenticationContainer.Credentials != null )
					{
						var parameter = $"{_AuthenticationContainer.Credentials.UserName}:{_AuthenticationContainer.Credentials.Password}";
						_HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "basic", Convert.ToBase64String( Encoding.UTF8.GetBytes( parameter ) ) );
					}

					break;

				case AuthenticationMode.Windows:
					UpdateUseWindowsCredentials( _AuthenticationContainer.Credentials == null, _AuthenticationContainer.Credentials );
					break;

				case AuthenticationMode.Certificate:
				case AuthenticationMode.HardwareCertificate:
					UpdateUseWindowsCredentials( false );
					if( _AuthenticationContainer.Certificate != null )
					{
						_WebRequestHandler.ClientCertificates.Add( _AuthenticationContainer.Certificate );
					}

					break;

				case AuthenticationMode.OAuth:
					UpdateUseWindowsCredentials( false );
					if( _AuthenticationContainer.OAuthAccessToken != null )
					{
						_HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "Bearer", _AuthenticationContainer.OAuthAccessToken );
					}

					break;
				default:
					throw new NotSupportedException( $"unknown authentication mode '{_AuthenticationContainer.Mode}'" );
			}

			RaiseAuthenticationChanged();
		}

		private void UpdateUseWindowsCredentials( bool useDefaultCredentials, ICredentials credentials = null )
		{
			// if one of those properties changes we unfortunately need to rebuild the request handler and therefore the http client
			if( _WebRequestHandler.UseDefaultCredentials == useDefaultCredentials && _WebRequestHandler.Credentials == null && credentials == null ) return;

			RebuildHttpClient();
			_WebRequestHandler.Credentials = credentials;
			_WebRequestHandler.UseDefaultCredentials = useDefaultCredentials;
		}

		private void RaiseAuthenticationChanged()
		{
			AuthenticationChanged?.Invoke( this, EventArgs.Empty );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
			if( !_IsDisposed )
			{
				if( disposing )
				{
					_HttpClient?.Dispose();
					_WebRequestHandler?.Dispose();
				}

				_IsDisposed = true;
			}
		}

		#endregion

		#region interface IDisposable

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		#endregion
	}
}