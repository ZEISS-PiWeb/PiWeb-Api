#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region using

	using System.Net;
	using System.Net.Http;
	using Zeiss.PiWeb.Api.Dtos;

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
		/// <param name="response">The http response that failed.</param>
		public WrappedServerErrorException( Error error, HttpResponseMessage response ) :
			base( error?.Message ?? $"Server error while processing the request ({response.ReasonPhrase})." )
		{
			Error = error;
			Response = response;
			StatusCode = response.StatusCode;
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
		public HttpStatusCode StatusCode { get;  }

		/// <summary>
		/// Returns the failed http request.
		/// </summary>
		public HttpRequestMessage Request => Response?.RequestMessage;

		/// <summary>
		/// Returns the failed http response.
		/// </summary>
		public HttpResponseMessage Response { get; }

		#endregion
	}
}
