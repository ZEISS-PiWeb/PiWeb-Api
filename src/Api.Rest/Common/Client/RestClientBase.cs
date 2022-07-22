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
	using System.Runtime.Versioning;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;
	using CacheCow.Client;
	using CacheCow.Common;
	using JetBrains.Annotations;
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

		private readonly bool _IsBrowser
#if NET5_0_OR_GREATER
			= OperatingSystem.IsBrowser();
#else
			= false;
#endif

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
		[CanBeNull] private readonly DelegatingHandler _CustomHttpMessageHandler;

		private readonly IObjectSerializer _Serializer;

		private HttpClient _HttpClient;
		private TimeoutHandler _TimeoutHandler;
		private HttpClientHandler _HttpClientHandler;
		private CachingHandler _CachingHandler;
		private ICacheStore _CacheStore;
		private IVaryHeaderStore _VaryHeaderStore;
		private bool _IsDisposed;
		private AuthenticationContainer _AuthenticationContainer = new AuthenticationContainer( AuthenticationMode.NoneOrBasic );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientBase"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		protected RestClientBase(
			[NotNull] Uri serverUri,
			string endpointName,
			TimeSpan? timeout = null,
			int maxUriLength = DefaultMaxUriLength,
			bool chunked = true,
			[CanBeNull] DelegatingHandler customHttpMessageHandler = null,
			[CanBeNull] IObjectSerializer serializer = null)
		{
			if( serverUri == null )
				throw new ArgumentNullException( nameof( serverUri ) );

			ServiceLocation = new UriBuilder( serverUri )
			{
				Path = ( serverUri.AbsolutePath.Replace( "/DataServiceSoap", "" ) + "/" + endpointName ).Replace( "//", "/" )
			}.Uri;

			_Chunked = chunked;
			_CustomHttpMessageHandler = customHttpMessageHandler;
			_Serializer = serializer ?? ObjectSerializer.Default;

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
			get => _TimeoutHandler.Timeout;
			set
			{
				if( _TimeoutHandler.Timeout != value )
				{
					_TimeoutHandler.Timeout = value;
				}
			}
		}

		/// <summary>
		/// Gets or sets if system default proxy should be used.
		/// </summary>
		/// <remarks>
		/// For executing within a browser this property is ignored and the browser's default is used.
		/// </remarks>
#pragma warning disable CA1416
		public bool UseDefaultWebProxy
		{
			get => _IsBrowser || _HttpClientHandler.UseProxy;
			set
			{
				if( !_IsBrowser && _HttpClientHandler.UseProxy != value )
					_HttpClientHandler.UseProxy = value;
			}
		}
#pragma warning restore CA1416

		/// <summary>
		/// Returns the endpoint address of the webservice.
		/// </summary>
		public Uri ServiceLocation { get; }

		/// <summary>
		/// Gets or sets the information for authenticating requests.
		/// </summary>
		/// <remarks>
		/// For executing within a browser this property is ignored.
		/// Instead inject a custom <see cref="DelegatingHandler"/> to appropriately modify the request.
		/// </remarks>
		public AuthenticationContainer AuthenticationContainer
		{
			get => _AuthenticationContainer;
			set
			{
				if( value == null ) throw new ArgumentNullException( nameof( value ) );

				if( _AuthenticationContainer == value ) return;

				_AuthenticationContainer = value;

				// Use the custom HTTP message handler for passing authentication relevant information in browser instead.
				if( _IsBrowser )
					return;

				UpdateAuthenticationInformation();
			}
		}

		/// <summary>
		/// Gets or sets the client-side cache store to cache requests (triggered by HTTP headers, like Cache-Control and Etag).
		/// If the value is <see langword="null" />, built-in caching is used (.Net Framework) or caching is disabled (.Net Standard, .Net Core).
		/// </summary>
		/// <remarks>
		/// For example use <see cref="InMemoryCacheStore"/> or <see cref="FilesystemCacheStore"/>.
		/// </remarks>
		public ICacheStore CacheStore
		{
			get => _CacheStore;

			set
			{
				if( value == _CacheStore )
					return;

				_CacheStore = value;

				RebuildHttpClient();
			}
		}

		/// <summary>
		/// Gets or sets the header store to build header-dependent keys for the cache (see "Vary" HTTP header).
		/// If the value is <see langword="null" />, the default VaryHeaderStore is used.
		/// </summary>
		/// <remarks>
		/// For example use <see cref="InMemoryVaryHeaderStore"/> or <see cref="FilesystemVaryHeaderStore"/>.
		/// </remarks>
		public IVaryHeaderStore VaryHeaderStore
		{
			get => _VaryHeaderStore;

			set
			{
				if( value == _VaryHeaderStore )
					return;

				_VaryHeaderStore = value;

				RebuildHttpClient();
			}
		}

		#endregion

		#region methods

		public Task Request( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( requestCreationHandler, false, null, true, cancellationToken );
		}

		public Task Request( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync<object>( () => requestCreationHandler(_Serializer), false, null, true, cancellationToken );
		}

		public Task<T> Request<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, response => ResponseToObjectAsync<T>(response, _Serializer), true, cancellationToken );
		}

		public Task<T> Request<T>( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler(_Serializer), false, response => ResponseToObjectAsync<T>(response, _Serializer), true, cancellationToken );
		}

		public Task<Stream> RequestStream( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, true, ResponseToStreamAsync, false, cancellationToken );
		}

		public Task<Stream> RequestStream( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler(_Serializer), true, ResponseToStreamAsync, false, cancellationToken );
		}

		public Task<IEnumerable<T>> RequestEnumerated<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, true, response => ResponseToEnumerationAsync<T>(response, _Serializer), false, cancellationToken );
		}

		public Task<IEnumerable<T>> RequestEnumerated<T>( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( () => requestCreationHandler(_Serializer), true, response => ResponseToEnumerationAsync<T>(response, _Serializer), false, cancellationToken );
		}

		public Task<byte[]> RequestBytes( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, ResponseToBytesAsync, true, cancellationToken );
		}

		public Task<T> RequestBinary<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken )
		{
			return PerformRequestAsync( requestCreationHandler, false, BinaryResponseToObjectAsync<T>, true, cancellationToken );
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

		private static async Task<T> ResponseToObjectAsync<T>( HttpResponseMessage response, IObjectSerializer serializer )
		{
			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				return serializer.Deserialize<T>( responseStream );
			}
		}

		private static Task<Uri> LocationHeaderToUrl( HttpResponseMessage response )
		{
			var result = response.Headers.Location;
			if( result == null && response.StatusCode == HttpStatusCode.Accepted )
				throw new RestClientException( $"Error fetching status URL: The response indicated status Accepted, but did not contain polling information." );

			return Task.FromResult( result );
		}

		private static async Task<IEnumerable<T>> ResponseToEnumerationAsync<T>( HttpResponseMessage response, IObjectSerializer serializer )
		{
			var stream = await ResponseToStreamAsync( response ).ConfigureAwait( false );
			return GetEnumeratedResponse<T>( response, stream, serializer );
		}

		private static IEnumerable<T> GetEnumeratedResponse<T>( IDisposable response, Stream responseStream, IObjectSerializer serializer )
		{
			using( response )
			{
				using( responseStream )
				{
					foreach( var item in serializer.DeserializeEnumeratedObject<T>( responseStream ) )
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

					await HandleFaultedResponse( response, _Serializer ).ConfigureAwait( false );
				}
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

		private static async Task HandleFaultedResponse( HttpResponseMessage response, IObjectSerializer serializer )
		{
			await HandleClientBasedFaults( response, serializer ).ConfigureAwait( false );
			HandleServerBasedFaults( response );
		}

		/// <summary>
		/// Handles all responses with status codes between 400 and 499
		/// </summary>
		private static async Task HandleClientBasedFaults( HttpResponseMessage response, IObjectSerializer serializer )
		{
			if( response.StatusCode < HttpStatusCode.BadRequest || response.StatusCode >= HttpStatusCode.InternalServerError )
				return;

			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				Error error;
				try
				{
					error = serializer.Deserialize<Error>( responseStream )
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

		private void RebuildHttpClient()
		{
			var timeout = _TimeoutHandler.Timeout;

			_HttpClient?.Dispose();
			_HttpClientHandler?.Dispose();
			_CachingHandler?.Dispose();

			BuildHttpClient( timeout );
		}

		private void BuildHttpClient( TimeSpan? timeout )
		{
#if NETFRAMEWORK
			var webRequestHandler = new WebRequestHandler
			{
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

			if( _CacheStore == null )
				webRequestHandler.CachePolicy = new HttpRequestCachePolicy( HttpCacheAgeControl.MaxAge, TimeSpan.FromDays( 0 ) );

			_HttpClientHandler = webRequestHandler;
#else
			_HttpClientHandler = new HttpClientHandler();

			// Almost all options are not available when running in browser
			// and need to be done either manually or are not possible at all.
			if ( !_IsBrowser )
			{
#pragma warning disable CA1416
				_HttpClientHandler.PreAuthenticate = true;
				_HttpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
#pragma warning restore CA1416
			}
#endif

			if( _CacheStore == null )
				_CachingHandler = null;
			else
			{
				_CachingHandler = _VaryHeaderStore == null
					? new CachingHandler( _CacheStore )
					: new CachingHandler( _CacheStore, _VaryHeaderStore );

				_CachingHandler.InnerHandler = _HttpClientHandler;
				_CachingHandler.DoNotEmitCacheCowHeader = true;
			}

			_TimeoutHandler = new TimeoutHandler
			{
				Timeout = timeout ?? DefaultTimeout,
				InnerHandler = (HttpMessageHandler)_CachingHandler ?? _HttpClientHandler
			};

			if( _CustomHttpMessageHandler != null )
				_CustomHttpMessageHandler.InnerHandler = _TimeoutHandler;

			_HttpClient = new HttpClient( _CustomHttpMessageHandler ?? _TimeoutHandler )
			{
				Timeout = System.Threading.Timeout.InfiniteTimeSpan,
				BaseAddress = ServiceLocation
			};
		}

#if NET5_0_OR_GREATER
		[UnsupportedOSPlatform( "browser" )]
#endif
		private void UpdateAuthenticationInformation()
		{
			_HttpClientHandler.ClientCertificates.Clear();
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
						_HttpClientHandler.ClientCertificates.Add( _AuthenticationContainer.Certificate );
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

#if NET5_0_OR_GREATER
		[UnsupportedOSPlatform( "browser" )]
#endif
		private void UpdateUseWindowsCredentials( bool useDefaultCredentials, ICredentials credentials = null )
		{
			// if one of those properties changes we unfortunately need to rebuild the request handler and therefore the http client
			if( _HttpClientHandler.UseDefaultCredentials == useDefaultCredentials && _HttpClientHandler.Credentials == null && credentials == null ) return;

			RebuildHttpClient();
			_HttpClientHandler.Credentials = credentials;
			_HttpClientHandler.UseDefaultCredentials = useDefaultCredentials;
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
					_HttpClientHandler?.Dispose();
					_CachingHandler?.Dispose();
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