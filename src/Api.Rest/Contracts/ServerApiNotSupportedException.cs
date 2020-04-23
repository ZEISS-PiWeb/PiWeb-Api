#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using System.Runtime.Serialization;

	#endregion

	[Serializable]
	public class ServerApiNotSupportedException : RestClientException
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerApiNotSupportedException" /> class.
		/// </summary>
		public ServerApiNotSupportedException() : base( "The server supports no known interface version." )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ExportException" /> class.
		/// </summary>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no
		/// inner exception is specified.
		/// </param>
		public ServerApiNotSupportedException( Exception innerException )
			: base( "The server supports no known interface version.", innerException )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerApiNotSupportedException" /> class.
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
		protected ServerApiNotSupportedException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{ }

		#endregion
	}
}