#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using System.Runtime.Serialization;

	#endregion

	/// <summary>
	/// Base class for client side exceptions that are thrown by the REST clients
	/// (<see cref="IRawDataServiceRestClient"/>, <see cref="IDataServiceRestClient"/>) in case of errors.
	/// </summary>
	[Serializable]
	public class RestClientException : Exception
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientException" /> class.
		/// </summary>
		public RestClientException()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public RestClientException( string message )
			: base( message )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientException" /> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no
		/// inner exception is specified.
		/// </param>
		public RestClientException( string message, Exception innerException )
			: base( message, innerException )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClientException" /> class.
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
		protected RestClientException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{ }

		#endregion
	}
}