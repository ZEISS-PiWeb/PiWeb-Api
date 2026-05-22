#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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
	using System.Linq;
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
	using Zeiss.PiWeb.Api.Rest.Common.Authentication;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

	#endregion

	/// <summary>
	/// Provides a REST client implementation with configurable authentication, caching, and timeout settings.
	/// </summary>
#if NET5_0_OR_GREATER
	[UnsupportedOSPlatform( "browser" )]
#endif
	public class RestClient : RestClientBase, IRestClientConfiguration, IDisposable
	{
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

		private ICacheStore _CacheStore;
		private IVaryHeaderStore _VaryHeaderStore;

		/// <summary>
		/// This value will be increased atomically to uniquely identify a request session which consists of an original rest request
		/// and all of its retry attempts.
		/// </summary>
		private long _LatestSession = 0;

		// This is only used by the legacy constructor to preserve old behavior
		[CanBeNull] private readonly DelegatingHandler _LegacyDelegatingHandler;

		// These are set by the settings based constructor used by rest client builders
		[NotNull] private readonly IReadOnlyCollection<Func<DelegatingHandler>> _DelegatingHandlerFactories =
			new List<Func<DelegatingHandler>>();
		[NotNull] private ICollection<DelegatingHandler> _DelegatingHandlers = new List<DelegatingHandler>();

		private readonly IAuthenticationHandler _AuthenticationHandler;
		private AuthenticationContainer _AuthenticationContainer = new AuthenticationContainer( AuthenticationMode.NoneOrBasic );

		private HttpClient _HttpClient;
		private HttpClientHandler _HttpClientHandler;
		private TimeoutHandler _TimeoutHandler;
		private CachingHandler _CachingHandler;

		private bool _UseProxy = true;
		private bool _CheckCertificateRevocationList = false;
		private TimeSpan _Timeout;

		private bool _IsDisposed;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClient"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		public RestClient(
			[NotNull] Uri serverUri,
			string endpointName,
			TimeSpan? timeout = null,
			int maxUriLength = DefaultMaxUriLength,
			bool chunked = true,
			[CanBeNull] DelegatingHandler customHttpMessageHandler = null,
			[CanBeNull] IObjectSerializer serializer = null )
			: base( maxUriLength, chunked, serializer ?? ObjectSerializer.Default )
		{
			if( serverUri == null )
				throw new ArgumentNullException( nameof( serverUri ) );

			ServiceLocation = new UriBuilder( serverUri )
			{
				Path = ( serverUri.AbsolutePath.Replace( "/DataServiceSoap", "" ) + "/" + endpointName ).Replace( "//", "/" )
			}.Uri;

			_LegacyDelegatingHandler = customHttpMessageHandler;

			_Timeout = timeout ?? DefaultTimeout;

			BuildHttpClient();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClient"/> class. This constructor exists to support
		/// rest client creation via <see cref="RestClientBuilder"/>.
		/// </summary>
		public RestClient( [NotNull] string endpointName, [NotNull] RestClientSettings settings )
			: base( settings.MaxUriLength, settings.AllowChunkedDataTransfer, settings.Serializer )
		{
			if( endpointName == null )
				throw new ArgumentNullException( nameof( endpointName ) );
			if( settings == null )
				throw new ArgumentNullException( nameof( settings ) );

			ServiceLocation = new UriBuilder( settings.ServerUri )
			{
				Path = ( settings.ServerUri.AbsolutePath.Replace( "/DataServiceSoap", "" ) + "/" + endpointName ).Replace( "//", "/" )
			}.Uri;

			_Timeout = settings.Timeout;
			_UseProxy = settings.UseSystemProxy;
			_CheckCertificateRevocationList = settings.CheckCertificateRevocationList;

			_LegacyDelegatingHandler = null;
			_AuthenticationHandler = settings.AuthenticationHandler;

			_DelegatingHandlerFactories = new List<Func<DelegatingHandler>>( settings.DelegatingHandlerFactories );

			BuildHttpClient();

			_AuthenticationHandler?.InitializeRestClient( new InitializationContext( this ) );
		}

		#endregion

		#region events

		/// <summary>
		/// Occurs when the authentication state changes.
		/// </summary>
		public event EventHandler AuthenticationChanged;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the request timeout.
		/// </summary>
		public TimeSpan Timeout
		{
			get => _Timeout;
			set
			{
				_Timeout = Timeout;
				if( _TimeoutHandler.Timeout != value )
					_TimeoutHandler.Timeout = value;
			}
		}

		/// <summary>
		/// Gets or sets if system default proxy should be used.
		/// </summary>
		/// <remarks>
		/// For executing within a browser this property is ignored and the browser's default is used.
		/// </remarks>
		public bool UseDefaultWebProxy
		{
			get => _UseProxy;
			set
			{
				_UseProxy = value;
				if( _HttpClientHandler.UseProxy != value )
					_HttpClientHandler.UseProxy = value;
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates whether the certificate is checked against the certificate authority revocation list.
		/// </summary>
		/// <remarks>
		/// For executing within a browser this property is ignored.
		/// </remarks>
		public bool CheckCertificateRevocationList
		{
			get => _CheckCertificateRevocationList;

			set
			{
				_CheckCertificateRevocationList = value;
				_HttpClientHandler.CheckCertificateRevocationList = value;
			}
		}

		/// <summary>
		/// Returns the endpoint address of the webservice.
		/// </summary>
		public override Uri ServiceLocation { get; }

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

		/// <inheritdoc />
		protected override async Task<TResult> PerformRequestAsync<TResult>(
			Func<HttpRequestMessage> requestCreationHandler,
			bool streamed, Func<HttpResponseMessage, Task<TResult>> handler = null,
			bool autoDisposeResponse = true,
			CancellationToken cancellationToken = default )
		{
			HttpRequestMessage request = null;
			HttpResponseMessage response = null;

			var currentSession = Interlocked.Increment( ref _LatestSession );
			var attempt = 1;
			object retryPayload = null;

			try
			{
				await CheckAuthenticationInformationAsync( cancellationToken ).ConfigureAwait( false );

				var completionOptions = streamed ? HttpCompletionOption.ResponseHeadersRead : HttpCompletionOption.ResponseContentRead;

				while( true )
				{
					request = requestCreationHandler();
					SetDefaultHttpHeaders( request );

					if( _AuthenticationHandler != null )
					{
						var requestContext = new RequestContext( this, request, currentSession, attempt, retryPayload, cancellationToken );
						await _AuthenticationHandler.HandleRequest( requestContext ).ConfigureAwait( false );
					}

					response = await _HttpClient.SendAsync( request, completionOptions, cancellationToken ).ConfigureAwait( false );

					if( response.IsSuccessStatusCode )
					{
						if( handler != null )
						{
							return await handler( response ).ConfigureAwait( false );
						}

						return default;
					}

					if( _AuthenticationHandler != null )
					{
						var context = new ResponseContext( this, request, response, currentSession, attempt, cancellationToken );
						await _AuthenticationHandler.HandleResponse( context ).ConfigureAwait( false );
						if( context.RetryRequest )
						{
							response.Dispose();
							retryPayload = context.RetryPayload;
							++attempt;
							continue;
						}
					}

					if( await UpdateAuthenticationInformationAsync( response, cancellationToken ).ConfigureAwait( false ) )
					{
						response.Dispose();
						retryPayload = null;
						++attempt;
						continue;
					}

					await HandleFaultedResponse( response, cancellationToken ).ConfigureAwait( false );
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

		private void RebuildHttpClient()
		{
			var currentDelegatingHandlers = _DelegatingHandlers;
			_DelegatingHandlers = new List<DelegatingHandler>();

			_HttpClient?.Dispose();
			_HttpClientHandler?.Dispose();
			_CachingHandler?.Dispose();

			foreach( var delegatingHandler in currentDelegatingHandlers.Reverse() )
				delegatingHandler.Dispose();

			BuildHttpClient();
		}

		private void BuildHttpClient()
		{
			_HttpClientHandler = new HttpClientHandler();

			// Since the Windows update from january 2026 the pre-authentication will fail after 100 sec.
			// This applies to .Net Framework only, .Net Core seems to ignore the setting anyway.
			_HttpClientHandler.PreAuthenticate = false;

			_HttpClientHandler.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
			_HttpClientHandler.UseProxy = _UseProxy;
			_HttpClientHandler.CheckCertificateRevocationList = _CheckCertificateRevocationList;

			HttpMessageHandler outerDelegatingHandler = _HttpClientHandler;

			if( _CacheStore == null )
			{
				_CachingHandler = null;
			}
			else
			{
				_CachingHandler = _VaryHeaderStore == null
					? new CachingHandler( _CacheStore )
					: new CachingHandler( _CacheStore, _VaryHeaderStore );
				_CachingHandler.DoNotEmitCacheCowHeader = true;

				_CachingHandler.InnerHandler = outerDelegatingHandler;
				outerDelegatingHandler = _CachingHandler;
			}

			_TimeoutHandler = new TimeoutHandler
			{
				Timeout = _Timeout,
				InnerHandler = outerDelegatingHandler
			};
			outerDelegatingHandler = _TimeoutHandler;

			foreach( var handlerFactory in _DelegatingHandlerFactories )
			{
				var newHandler = handlerFactory();
				_DelegatingHandlers.Add( newHandler );

				newHandler.InnerHandler = outerDelegatingHandler;
				outerDelegatingHandler = newHandler;
			}

			if( _LegacyDelegatingHandler != null )
			{
				_LegacyDelegatingHandler.InnerHandler = outerDelegatingHandler;
				outerDelegatingHandler = _LegacyDelegatingHandler;
			}

			_HttpClient = new HttpClient( outerDelegatingHandler )
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
					var currentDelegatingHandlers = _DelegatingHandlers;
					_DelegatingHandlers = new List<DelegatingHandler>();

					_HttpClient?.Dispose();
					_HttpClientHandler?.Dispose();
					_CachingHandler?.Dispose();

					foreach( var delegatingHandler in currentDelegatingHandlers.Reverse() )
						delegatingHandler.Dispose();
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