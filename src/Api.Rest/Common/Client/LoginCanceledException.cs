#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Runtime.Serialization;

	#endregion

	/// <summary>
	/// An <see cref="Exception"/> that is being used for indicating that a request is canceled.
	/// </summary>
	/// <remarks>
	/// This is usually raised by the user at the moment where credentials are requested.
	/// E.g. clicking on cancel in a login window.
	/// </remarks>
	[Serializable]
	public class LoginCanceledException : Exception
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginCanceledException" /> class.
		/// </summary>
		public LoginCanceledException()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginCanceledException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		public LoginCanceledException( string message )
			: base( message )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginCanceledException" /> class.
		/// </summary>
		/// <param name="exception">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
		/// </param>
		public LoginCanceledException( Exception exception )
			: base( "Login canceled", exception )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginCanceledException" /> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="exception">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.
		/// </param>
		public LoginCanceledException( string message, Exception exception )
			: base( message, exception )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="LoginCanceledException" /> class.
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
		protected LoginCanceledException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{ }

		#endregion
	}
}