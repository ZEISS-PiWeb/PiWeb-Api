#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using System.Net;
	using System.Net.Http;
	using System.Runtime.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// Base class for server side exceptions that are thrown by the REST clients
	/// (<see cref="IRawDataServiceRestClient"/>, <see cref="IDataServiceRestClient"/>) in case of errors.
	/// </summary>
	[Serializable]
	public class WrappedServerErrorException : RestClientException
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="WrappedServerErrorException" /> class.
		/// </summary>
		/// <param name="error">The server side error.</param>
		/// <param name="response">The http response that failed.</param>
		public WrappedServerErrorException( Error error, HttpResponseMessage response ) :
			base( error?.Message ?? $"Server error while processing the request ({response.ReasonPhrase})." )
		{
			Error = error;
			Response = response;
			StatusCode = response.StatusCode;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WrappedServerErrorException" /> class.
		/// </summary>
		/// <param name="info">
		/// The <see cref="SerializationInfo"></see> that holds the serialized object data about the exception being thrown.
		/// </param>
		/// <param name="context">
		/// The <see cref="StreamingContext"></see> that contains contextual information about the source or destination.
		/// </param>
		/// <exception cref="ArgumentNullException">
		/// The <paramref name="info" /> parameter is <see langword="null" />.
		/// </exception>
		/// <exception cref="SerializationException">
		/// The class name is <see langword="null" /> or <see cref="Exception.HResult"></see> is zero (0).
		/// </exception>
		protected WrappedServerErrorException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			foreach( var entry in info )
			{
				switch( entry.Name )
				{
					case nameof( Error ):
						Error = (Error)entry.Value;
						break;

					case nameof( Response ):
						Response = (HttpResponseMessage)entry.Value;
						break;
				}
			}

			if( Response != null )
			{
				StatusCode = Response.StatusCode;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="WrappedServerErrorException" /> class.
		/// </summary>
		/// <param name="error">The server side error.</param>
		public WrappedServerErrorException( Error error ) :
			base( error?.Message ?? "Server error while processing the request." )
		{
			Error = error;
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the server side error.
		/// </summary>
		public Error Error { get; }

		/// <summary>
		/// Returns the http status.
		/// </summary>
		public HttpStatusCode StatusCode { get; }

		/// <summary>
		/// Returns the failed http request.
		/// </summary>
		public HttpRequestMessage Request => Response?.RequestMessage;

		/// <summary>
		/// Returns the failed http response.
		/// </summary>
		public HttpResponseMessage Response { get; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );
			info.AddValue( nameof( Error ), Error );
			info.AddValue( nameof( Response ), Response );
		}

		#endregion
	}
}