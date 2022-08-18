#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	public static class RequestBuilder
	{
		#region methods

		/// <summary>
		/// Creates a new GET-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateGet( string relativeUri )
		{
			return Create( HttpMethod.Get, relativeUri, null, Array.Empty<ParameterDefinition>() );
		}

		/// <summary>
		/// Creates a new GET-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinition"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateGet( string relativeUri, ParameterDefinition parameterDefinition )
		{
			return Create( HttpMethod.Get, relativeUri, null, new[] { parameterDefinition } );
		}

		/// <summary>
		/// Creates a new GET-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateGet( string relativeUri, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			return Create( HttpMethod.Get, relativeUri, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new DELETE-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateDelete( string relativeUri )
		{
			return Create( HttpMethod.Delete, relativeUri, null, Array.Empty<ParameterDefinition>() );
		}

		/// <summary>
		/// Creates a new DELETE-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinition"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateDelete( string relativeUri, ParameterDefinition parameterDefinition )
		{
			return Create( HttpMethod.Delete, relativeUri, null, new[] { parameterDefinition } );
		}

		/// <summary>
		/// Creates a new DELETE-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateDelete( string relativeUri, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			return Create( HttpMethod.Delete, relativeUri, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new POST-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>.
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<IObjectSerializer, HttpRequestMessage> CreatePost( string relativeUri, [NotNull] Payload payload )
		{
			return Create( HttpMethod.Post, relativeUri, payload, null, Array.Empty<ParameterDefinition>() );
		}

		/// <summary>
		/// Creates a new POST-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinition"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<IObjectSerializer, HttpRequestMessage> CreatePost( string relativeUri, [NotNull] Payload payload, ParameterDefinition parameterDefinition )
		{
			return Create( HttpMethod.Post, relativeUri, payload, null, new [] { parameterDefinition } );
		}

		/// <summary>
		/// Creates a new POST-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<IObjectSerializer, HttpRequestMessage> CreatePost( string relativeUri, [NotNull] Payload payload, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			return Create( HttpMethod.Post, relativeUri, payload, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new PUT-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>.
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<IObjectSerializer, HttpRequestMessage> CreatePut( string relativeUri, [NotNull] Payload payload )
		{
			return Create( HttpMethod.Put, relativeUri, payload, null, Array.Empty<ParameterDefinition>() );
		}

		/// <summary>
		/// Creates a new PUT-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinition"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<IObjectSerializer, HttpRequestMessage> CreatePut( string relativeUri, [NotNull] Payload payload, ParameterDefinition parameterDefinition )
		{
			return Create( HttpMethod.Put, relativeUri, payload, null, new[] { parameterDefinition } );
		}

		/// <summary>
		/// Creates a new PUT-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<IObjectSerializer, HttpRequestMessage> CreatePut( string relativeUri, [NotNull] Payload payload, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			return Create( HttpMethod.Put, relativeUri, payload, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="payload"/> is <see langword="null" />.</exception>
		public static Func<IObjectSerializer, HttpRequestMessage> Create( HttpMethod method, string relativeUri, [NotNull] Payload payload, KeyValuePair<string, string>[] additionalHttpRequestHeader, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			if( payload == null ) throw new ArgumentNullException( nameof( payload ) );

			return serializer => CreateInternal( method, relativeUri, payload, serializer, additionalHttpRequestHeader, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// </summary>
		public static Func<HttpRequestMessage> Create( HttpMethod method, string relativeUri, KeyValuePair<string, string>[] additionalHttpRequestHeader, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			return () => CreateInternal( method, relativeUri, Payload.Empty, null, additionalHttpRequestHeader, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		private static HttpRequestMessage CreateInternal( HttpMethod method, string relativeUri, Payload payload, IObjectSerializer serializer, KeyValuePair<string, string>[] additionalHttpRequestHeader, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			var request = SetParametersAndHeaders( method, relativeUri, additionalHttpRequestHeader, parameterDefinitions );
			if( payload != Payload.Empty )
			{
				request.Content = new PushStreamContent( ( outputStream, _, _ ) =>
				{
					return serializer.SerializeAsync( outputStream, payload.Value );
				}, new MediaTypeWithQualityHeaderValue( RestClientBase.MimeTypeJson ) );
			}

			return request;
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The attachment is provided by the <paramref name="stream"/> parameter.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="method"/> is <see langword="null" />.</exception>
		public static Func<HttpRequestMessage> CreateWithAttachment( [NotNull] HttpMethod method, string relativeUri, Stream stream, string mimeType, long? contentLength, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			if( method == null ) throw new ArgumentNullException( nameof( method ) );

			if( method != HttpMethod.Post && method != HttpMethod.Put )
				throw new ArgumentOutOfRangeException( $"CreateWithAttachment is not allowed for {method}. Valid HttpMethods are {HttpMethod.Post} or {HttpMethod.Put}" );

			return () => CreateWithAttachmentInternal( method, relativeUri, stream, mimeType, contentLength, contentMD5, contentDisposition, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/>
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The attachment is provided by the <paramref name="stream"/> parameter.
		/// </summary>
		private static HttpRequestMessage CreateWithAttachmentInternal( [NotNull] HttpMethod method, string relativeUri, Stream stream, string mimeType, long? contentLength, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			var request = CreateInternal( method, relativeUri, Payload.Empty, null, null, parameterDefinitions );

			if( stream == null )
				return request;

			request.Content = new StreamContent( stream );

			if( contentLength.HasValue )
				request.Content.Headers.Add( HttpRequestHeader.ContentLength.ToString(), contentLength.Value.ToString() );
			if( !string.IsNullOrEmpty( mimeType ) )
				request.Content.Headers.ContentType = new MediaTypeHeaderValue( mimeType );
			if( !string.IsNullOrEmpty( contentDisposition ) )
				request.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue( "attachment" ) { FileName = contentDisposition };
			if( contentMD5.HasValue )
				request.Content.Headers.ContentMD5 = contentMD5.Value.ToByteArray();

			return request;
		}

		private static HttpRequestMessage SetParametersAndHeaders( HttpMethod method, string relativeUri, KeyValuePair<string, string>[] additionalHttpRequestHeader, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			relativeUri = AppendParameters( relativeUri, parameterDefinitions );
			var request = new HttpRequestMessage( method, relativeUri );

			if( additionalHttpRequestHeader == null )
				return request;

			foreach( var header in additionalHttpRequestHeader )
			{
				request.Headers.Add( header.Key, header.Value );
			}

			return request;
		}

		internal static string AppendParameters( string relativeUri, IEnumerable<ParameterDefinition> parameterDefinitions )
		{
			if( parameterDefinitions == null )
				return relativeUri;

			var queryString = new StringBuilder();
			foreach( var parameterDefinition in parameterDefinitions )
			{
				if( queryString.Length > 0 )
					queryString.Append( "&" );

				queryString.Append( parameterDefinition.Name ).Append( "=" ).Append( Uri.EscapeDataString( parameterDefinition.Value ) );
			}

			if( queryString.Length > 0 )
			{
				relativeUri += "?" + queryString;
			}

			return relativeUri;
		}

		#endregion
	}
}