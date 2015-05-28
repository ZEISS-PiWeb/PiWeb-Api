#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Client
{
	#region usings

	using Common.Data;

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Net;
	using System.Net.Cache;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Security.Cryptography.X509Certificates;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	#endregion

	/// <summary>
	/// Base class for communication with REST based web services.
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough]
	public class RestClient
	{
		#region constants

		private const int DefaultTimeout = 5 * 60 * 1000;
		private const int AbsoluteMaxUriLength = 15 * 1024;

		/// <summary>Mimetype für JSON</summary>
		public const string MimeTypeJson = "application/json";

		#endregion

		#region members

		private readonly HttpClient _HttpClient;
		private readonly WebRequestHandler _WebRequestHandler;

		protected readonly int MaxUriLength;

		#endregion

		#region constructors

		/// <summary>Constructor.</summary>
		protected RestClient( Uri serverUri, string endpointName, int timeoutInMilliseconds = DefaultTimeout, int? maxUriLength = null )
		{
			if( serverUri == null )
				throw new ArgumentNullException( "serverUri" );

			ServiceLocation = new UriBuilder( serverUri )
			{
				Path = String.Concat( serverUri.AbsolutePath, "/", endpointName ).Replace( "//", "/" )
			}.Uri;

			_WebRequestHandler = new WebRequestHandler
			{
				CachePolicy = new HttpRequestCachePolicy( HttpRequestCacheLevel.Revalidate ),
				AllowPipelining = true,
				PreAuthenticate = true,
				AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
			};

			_HttpClient = new HttpClient( _WebRequestHandler )
			{
				Timeout = TimeSpan.FromMilliseconds( timeoutInMilliseconds ),
				BaseAddress = ServiceLocation
			};
			SetHeaders( _HttpClient.DefaultRequestHeaders );

			MaxUriLength = Math.Min( maxUriLength ?? AbsoluteMaxUriLength, AbsoluteMaxUriLength );
		}

		/// <summary> Sets the HTTP headers fields for the <paramref name="headers"/> object. </summary>
		/// <param name="headers">The headers object the values should be set for.</param>
		private static void SetHeaders( HttpRequestHeaders headers )
		{
			headers.Accept.Add( new MediaTypeWithQualityHeaderValue( MimeTypeJson ) );
			headers.AcceptEncoding.Add( new StringWithQualityHeaderValue( "gzip" ) );
			headers.AcceptEncoding.Add( new StringWithQualityHeaderValue( "deflate" ) );
			headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentCulture.Name ) );
			headers.UserAgent.Add( new ProductInfoHeaderValue( ClientIdHelper.ClientProduct, ClientIdHelper.ClientVersion ) );
			headers.TransferEncodingChunked = true;
			headers.Add( "Keep-Alive", "true" );
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets if system default proxy should be used.
		/// </summary>
		public bool UseDefaultWebProxy
		{
			get { return _WebRequestHandler.UseProxy; }
			set { _WebRequestHandler.UseProxy = value; }
		}

		/// <summary>
		/// Returns the endpoint address of the webservice.
		/// </summary>
		public Uri ServiceLocation { get; private set; }

		/// <summary>
		/// Gets or sets the authentication (username + password) used by this class. 
		/// </summary>
		public NetworkCredential Credentials
		{
			get { return _WebRequestHandler.Credentials as NetworkCredential; }
			set
			{
				_WebRequestHandler.Credentials = value;
				UpdateAuthenticationHeader();
			}
		}

		/// <summary>
		/// Gets or sets whether the current login credentials (single sign on) should be sent for authorization.
		/// </summary>
		public bool UseDefaultCredentials
		{
			get { return _WebRequestHandler.UseDefaultCredentials; }
			set
			{
				_WebRequestHandler.UseDefaultCredentials = value;
				UpdateAuthenticationHeader();
			}
		}

		/// <summary> 
		/// Gets or sets the client certificate that should be used for authorization.
		/// </summary>
		public X509Certificate ClientCertificate
		{
			get { return _WebRequestHandler.ClientCertificates.Count > 0 ? _WebRequestHandler.ClientCertificates[ 0 ] : null; }
			set
			{
				if( value != null )
					_WebRequestHandler.ClientCertificates.Add( value );
				else
					_WebRequestHandler.ClientCertificates.Clear();

				UpdateAuthenticationHeader();
			}
		}

		#endregion

		#region methods

		#region GET

		/// <summary>GETs data asynchronously and returns a <see cref="Task"/> that contains a result of type <typeparamref name="T"/>.</summary>
		/// <typeparam name="T">Result type that should be returned within the Task object.</typeparam>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task<T> Get<T>( string requestUri, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse<T>( BuildRequest( HttpMethod.Get, requestUri, parameterDefinitions ), cancellationToken );
		}

		/// <summary>GETs data asynchronously and returns a <see cref="Task"/> that contains a result of type <see cref="IEnumerable{T}"/>.</summary>
		/// <typeparam name="T">Result type that should be returned within the Task object.</typeparam>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task<IEnumerable<T>> GetEnumerated<T>( string requestUri, CancellationToken cancellationToken, ParameterDefinition[] parameterDefinitions )
		{
			return GetEnumeratedResponse<T>( BuildRequest( HttpMethod.Get, requestUri, parameterDefinitions ), cancellationToken );
		}

		/// <summary>GETs data asynchronously and returns a <see cref="Task"/> that contains a result of type <see cref="Byte"/>-Array.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task<Byte[]> GetBytes( string requestUri, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponseBytes( BuildRequest( HttpMethod.Get, requestUri, parameterDefinitions ), cancellationToken );
		}

		/// <summary>GETs data asynchronously and returns a <see cref="Task"/> that contains a result of type <see cref="Stream"/>.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task<Stream> GetStream( string requestUri, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponseStream( BuildRequest( HttpMethod.Get, requestUri, parameterDefinitions ), cancellationToken );
		}

		#endregion

		#region POST

		/// <summary>POSTs data asynchronously and returns a <see cref="Task"/> that contains a result of type <typeparamref name="T"/>.</summary>
		/// <typeparam name="T">Result type that should be returned within the Task object.</typeparam>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="data">The data that should be posted within the HTTP body.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task<T> Post<T>( string requestUri, object data, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse<T>( BuildRequest( HttpMethod.Post, requestUri, data, parameterDefinitions ), cancellationToken );
		}

		/// <summary>POSTs data asynchronously and returns a <see cref="Task"/>.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="data">The data that should be posted within the HTTP body.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task Post( string requestUri, object data, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse( BuildRequest( HttpMethod.Post, requestUri, data, parameterDefinitions ), cancellationToken );
		}

		/// <summary>POSTs data asynchronously and returns a <see cref="Task"/>.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="data">The data that should be posted within the HTTP body.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		/// <param name="contentLength">The length of the content that should be send.</param>
		/// <param name="mimeType">The mime type of the content to be sent.</param>
		/// <param name="contentDisposition">The name of the file to be streamed.</param>
		/// <param name="contentMD5">The MD5 sum of the file to be streamed.</param>
		protected Task Post( string requestUri, Stream data, CancellationToken cancellationToken, long? contentLength, string mimeType, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse( BuildStreamRequest( HttpMethod.Post, requestUri, data, mimeType, contentLength, contentMD5, contentDisposition, parameterDefinitions ), cancellationToken );
		}

		#endregion

		#region PUT

		/// <summary>PUTs data asynchronously and returns a <see cref="Task"/>.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="data">The data that should be posted within the HTTP body.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task Put( string requestUri, object data, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse( BuildRequest( HttpMethod.Put, requestUri, data, parameterDefinitions ), cancellationToken );
		}

		/// <summary>PUTs data asynchronously and returns a <see cref="Task"/>.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="data">The data that should be posted within the HTTP body.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		/// <param name="contentLength">The length of the content that should be send.</param>
		/// <param name="mimeType">The mime type of the content to be sent.</param>
		/// <param name="contentDisposition">The name of the file to be streamed.</param>
		/// <param name="contentMD5">The MD5 sum of the file to be streamed.</param>
		protected Task Put( string requestUri, Stream data, CancellationToken cancellationToken, long? contentLength, string mimeType, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse( BuildStreamRequest( HttpMethod.Put, requestUri, data, mimeType, contentLength, contentMD5, contentDisposition, parameterDefinitions ), cancellationToken );
		}

		#endregion

		#region DELETE

		/// <summary>DELETEs data asynchronously and returns a <see cref="Task"/>.</summary>
		/// <param name="requestUri">The string that should the base url extended by.</param>
		/// <param name="cancellationToken">A <see cref="CancellationToken"/> the async call can be canceled with.</param>
		/// <param name="parameterDefinitions">Query parameters the url can be extended by.</param>
		protected Task Delete( string requestUri, CancellationToken cancellationToken, params ParameterDefinition[] parameterDefinitions )
		{
			return GetResponse( BuildRequest( HttpMethod.Delete, requestUri, parameterDefinitions ), cancellationToken );
		}

		#endregion

		#region Helper methods

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> to call the url <code>requestUri</code> with the HTTP 
		/// verb <code>method</code> (GET, POST, PUT, DELETE).
		/// </summary>
		protected HttpRequestMessage BuildRequest( HttpMethod method, string requestUri, params ParameterDefinition[] parameterDefinitions )
		{
			return BuildRequest( method, requestUri, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> to call the url <code>requestUri</code> with the HTTP 
		/// verb <code>method</code> (GET, POST, PUT, DELETE) and data <code>payload</code>.
		/// </summary>
		protected HttpRequestMessage BuildRequest( HttpMethod method, string requestUri, object payload, params ParameterDefinition[] parameterDefinitions )
		{
			var request = BuildRequestInternal( method, requestUri, parameterDefinitions );
			if( payload != null )
			{
				request.Content = new PushStreamContent( ( outputStream, content, context ) =>
				{
					using( var sw = new StreamWriter( outputStream, Encoding.UTF8, 64 * 1024, false ) )
					{
						RestClientHelper.CreateJsonSerializer().Serialize( sw, payload );
					}
				}, new MediaTypeWithQualityHeaderValue( MimeTypeJson ) );
			}
			return request;
		}

		private HttpRequestMessage BuildStreamRequest( HttpMethod method, string requestUri, Stream stream, string mimeType, long? contentLength, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			var request = BuildRequestInternal( method, requestUri, parameterDefinitions );
			if( stream != null )
			{
				request.Content = new StreamContent( stream );

				if( contentLength.HasValue )
					request.Content.Headers.Add( HttpRequestHeader.ContentLength.ToString(), contentLength.Value.ToString() );
				if( !String.IsNullOrEmpty( mimeType ) )
					request.Content.Headers.ContentType = new MediaTypeHeaderValue( mimeType );
				if( !String.IsNullOrEmpty( contentDisposition ) )
					request.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue( "attachment" ) { FileName = contentDisposition };
				if( contentMD5.HasValue )
					request.Content.Headers.ContentMD5 = contentMD5.Value.ToByteArray();
			}
			return request;
		}

		private HttpRequestMessage BuildRequestInternal( HttpMethod method, string requestUri, params ParameterDefinition[] parameterDefinitions )
		{
			var uriBuilder = new UriBuilder( ServiceLocation );
			uriBuilder.Path += requestUri;

			if( parameterDefinitions != null )
			{
			var queryString = new StringBuilder();
			foreach( var parameterDefinition in parameterDefinitions )
			{
				if( queryString.Length > 0 )
					queryString.Append( "&" );

				queryString.Append( parameterDefinition.Name ).Append( "=" ).Append( Uri.EscapeDataString( parameterDefinition.Value ) );
			}
			uriBuilder.Query = queryString.ToString();
			}
			var request = new HttpRequestMessage( method, uriBuilder.Uri );
			request.Headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentUICulture.IetfLanguageTag, 1.0 ) );
			request.Headers.AcceptLanguage.Add( new StringWithQualityHeaderValue( CultureInfo.CurrentUICulture.TwoLetterISOLanguageName, 0.8 ) );

			return request;
		}

		[System.Diagnostics.DebuggerStepThrough]
		private async Task<T> GetResponse<T>( HttpRequestMessage request, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			try
			{
				using( var response = await _HttpClient.SendAsync( request, cancellationToken ).ConfigureAwait( false ) )
				{
					await CheckForFaultedResponses( response ).ConfigureAwait( false );
					using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
					{
						return RestClientHelper.DeserializeObject<T>( responseStream );
					}
				}
			}
			catch( HttpRequestException e )
			{
				throw new RestClientException( string.Format( "Error fetching web service response for request [{0}]: {1}", request.RequestUri, e.Message ), e );
			}
		}

		private async Task<IEnumerable<T>> GetEnumeratedResponse<T>( HttpRequestMessage request, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var response = await _HttpClient.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, cancellationToken ).ConfigureAwait( false );

			await CheckForFaultedResponses( response );
			var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false );

			return GetEnumeratedResponse<T>( responseStream, response );
		}

		private static IEnumerable<T> GetEnumeratedResponse<T>( Stream responseStream, HttpResponseMessage response )
		{
			using( response )
			using( responseStream )
			{
				foreach( var item in RestClientHelper.DeserializeEnumeratedObject<T>( responseStream ) )
				{
					yield return item;
				}
			}
		}

		private async Task<byte[]> GetResponseBytes( HttpRequestMessage request, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			using( var response = await _HttpClient.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, cancellationToken ).ConfigureAwait( false ) )
			{
				await CheckForFaultedResponses( response ).ConfigureAwait( false );
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
		}

		/// <summary>
		/// Reads and returns the response stream.
		/// </summary>
		protected async Task<Stream> GetResponseStream( HttpRequestMessage request, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var response = await _HttpClient.SendAsync( request, HttpCompletionOption.ResponseHeadersRead, cancellationToken ).ConfigureAwait( false );
			await CheckForFaultedResponses( response ).ConfigureAwait( false );
			return await response.Content.ReadAsStreamAsync().ConfigureAwait( false );
		}

		/// <summary>
		/// Reads the response stream and throws an exception in case of an error.
		/// </summary>
		protected async Task GetResponse( HttpRequestMessage request, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			using( var response = await _HttpClient.SendAsync( request, cancellationToken ).ConfigureAwait( false ) )
			{
				await CheckForFaultedResponses( response ).ConfigureAwait( false );
			}
		}
		
		private static async Task CheckForFaultedResponses( HttpResponseMessage response )
		{
			if( response.IsSuccessStatusCode )
				return;

			using( var responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait( false ) )
			{
				Error error = null;
				try
				{
					error = RestClientHelper.DeserializeObject<Error>( responseStream );
				}
				catch
				{
					// ignored
				}

				if( error == null )
					error = new Error( string.Format( "Request {0} {1} was not successful: {2} ({3})", response.RequestMessage.Method, response.RequestMessage.RequestUri, (int)response.StatusCode, response.ReasonPhrase ) );

				throw new WrappedServerErrorException( error, response.StatusCode );
			}
		}

		private void UpdateAuthenticationHeader()
		{
			if( UseDefaultCredentials || ClientCertificate != null || Credentials == null )
				_HttpClient.DefaultRequestHeaders.Authorization = null;
			else
				_HttpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue( "basic", Convert.ToBase64String( Encoding.UTF8.GetBytes( string.Format( "{0}:{1}", Credentials.UserName, Credentials.Password ) ) ) );
		}

		#endregion

		#endregion
	}
}
