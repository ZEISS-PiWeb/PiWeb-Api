#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Common.Client
{
	#region using

	using System;
	using System.Net;
	using Common.Data;

	#endregion

	/// <summary>
	/// Base class for server side exceptions that are thrown by the <see cref="RestClient"/> in case of errors.
	/// </summary>
	public class WrappedServerErrorException : RestClientException
	{
		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="error">The server side error.</param>
		/// <param name="statusCode">The http status.</param>
		public WrappedServerErrorException( Error error, HttpStatusCode statusCode ) :
			base( string.Format( "Server error while processing the request: {0}", error != null ? error.Message : "<unknown>" ) )
		{
			Error = error;
			StatusCode = statusCode;
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the server side error.
		/// </summary>
		public Error Error { get; private set; }

		/// <summary>
		/// Returns the http status.
		/// </summary>
		public HttpStatusCode StatusCode { get; private set; }

		#endregion
	}
}
