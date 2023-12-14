#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Formatter
{
	#region usings

	using System;
	using System.Runtime.Serialization;

	#endregion

	[Serializable]
	public class FormaterException : Exception
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FormaterException" /> class.
		/// </summary>
		public FormaterException()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="FormaterException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public FormaterException( string message )
			: base( message )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="FormaterException" /> class.
		/// </summary>
		/// <param name="message">The error message that explains the reason for the exception.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no
		/// inner exception is specified.
		/// </param>
		public FormaterException( string message, Exception innerException )
			: base( message, innerException )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="FormaterException" /> class.
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
		protected FormaterException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{ }

		#endregion
	}
}