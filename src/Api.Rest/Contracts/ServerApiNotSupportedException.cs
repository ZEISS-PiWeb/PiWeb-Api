#region copyright

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
	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// An exception that is thrown if there is not matching API version found.
	/// </summary>
	[Serializable]
	public class ServerApiNotSupportedException : RestClientException
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerApiNotSupportedException" /> class.
		/// </summary>
		/// <param name="versions">The list of offered versions.</param>
		public ServerApiNotSupportedException( InterfaceVersionRange versions )
			: base( "The server supports no known interface version." )
		{
			Versions = versions ?? throw new ArgumentNullException( nameof( versions ) );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerApiNotSupportedException" /> class.
		/// </summary>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no
		/// inner exception is specified.
		/// </param>
		/// <param name="versions">The list of offered versions.</param>
		public ServerApiNotSupportedException( InterfaceVersionRange versions, Exception innerException )
			: base( "The server supports no known interface version.", innerException )
		{
			Versions = versions ?? throw new ArgumentNullException( nameof( versions ) );
		}

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
		{
			Versions = JsonConvert.DeserializeObject<InterfaceVersionRange>( info.GetString( nameof( Versions ) ) );
		}

		#endregion

		/// <summary>
		/// A list of interface versions that are offered by the server. This value is null if requesting offered
		/// versions failed.
		/// </summary>
		public InterfaceVersionRange Versions { get; }

		#region methods

		/// <inheritdoc />
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );

			info.AddValue( nameof( Versions ), JsonConvert.SerializeObject( Versions ) );
		}

		#endregion
	}
}