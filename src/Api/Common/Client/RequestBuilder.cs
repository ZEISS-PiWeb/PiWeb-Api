#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Common.Client
{
	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Net;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Common.Data;
	using Zeiss.PiWeb.Api.Dtos;

	public static class RequestBuilder
	{
		/// <summary>
		/// Creates a new GET-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateGet( string relativeUri, params ParameterDefinition[] parameterDefinitions)
		{
			return Create( HttpMethod.Get, relativeUri, Payload.Empty, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new DELETE-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// </summary>
		public static Func<HttpRequestMessage> CreateDelete( string relativeUri, params ParameterDefinition[] parameterDefinitions)
		{
			return Create( HttpMethod.Delete, relativeUri, Payload.Empty, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new POST-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<HttpRequestMessage> CreatePost( string relativeUri, Payload payload, params ParameterDefinition[] parameterDefinitions )
		{
			return Create( HttpMethod.Post, relativeUri, payload, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new PUT-<see cref="HttpRequestMessage"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<HttpRequestMessage> CreatePut( string relativeUri, Payload payload, params ParameterDefinition[] parameterDefinitions )
		{
			return Create( HttpMethod.Put, relativeUri, payload, null, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		public static Func<HttpRequestMessage> Create( HttpMethod method, string relativeUri, Payload payload, KeyValuePair<string, string>[] additionalHttpRequestHeader, params ParameterDefinition[] parameterDefinitions )
		{
			if( payload == null ) throw new ArgumentNullException( nameof( payload ) );

			return () => CreateInternal( method, relativeUri, payload, additionalHttpRequestHeader, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The body of the HTTP message is provided by the <paramref name="payload"/> parameter.
		/// </summary>
		private static HttpRequestMessage CreateInternal( HttpMethod method, string relativeUri, Payload payload, KeyValuePair<string, string>[] additionalHttpRequestHeader, params ParameterDefinition[] parameterDefinitions )
		{
			var request = SetParametersAndHeaders( method, relativeUri, additionalHttpRequestHeader, parameterDefinitions );
			if( payload != Payload.Empty )
			{
				request.Content = new PushStreamContent( ( outputStream, content, context ) =>
				{
					using( var sw = new StreamWriter( outputStream, Encoding.UTF8, 64 * 1024, false ) )
					{
						RestClientHelper.CreateJsonSerializer().Serialize( sw, payload.Value );
					}
				}, new MediaTypeWithQualityHeaderValue( RestClient.MimeTypeJson ) );
			}
			return request;
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The attachment is provided by the <paramref name="stream"/> parameter.
		/// </summary>
		public static Func<HttpRequestMessage> CreateWithAttachment( [NotNull]HttpMethod method, string relativeUri, Stream stream, string mimeType, long? contentLength, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			if( method == null ) throw new ArgumentNullException( nameof(method) );

			if( method != HttpMethod.Post && method != HttpMethod.Put )
				throw new ArgumentOutOfRangeException( $"CreateWithAttachment is not allowed for {method}. Valid HttpMethods are {HttpMethod.Post} or {HttpMethod.Put}" );

			return () => CreateWithAttachmentInternal( method, relativeUri, stream, mimeType, contentLength, contentMD5, contentDisposition, parameterDefinitions );
		}

		/// <summary>
		/// Creates a new <see cref="HttpRequestMessage"/> for http verb <paramref name="method"/> based on the <paramref name="relativeUri"/> 
		/// and extended by possible additional query parameters represented by <paramref name="parameterDefinitions"/>
		/// The attachment is provided by the <paramref name="stream"/> parameter.
		/// </summary>
		private static HttpRequestMessage CreateWithAttachmentInternal( [NotNull]HttpMethod method, string relativeUri, Stream stream, string mimeType, long? contentLength, Guid? contentMD5, string contentDisposition, params ParameterDefinition[] parameterDefinitions )
		{
			var request = CreateInternal( method, relativeUri, Payload.Empty, null, parameterDefinitions );

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

		private static HttpRequestMessage SetParametersAndHeaders( HttpMethod method, string relativeUri, KeyValuePair<string, string>[] additionalHttpRequestHeader = null, params ParameterDefinition[] parameterDefinitions )
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
	}
}
