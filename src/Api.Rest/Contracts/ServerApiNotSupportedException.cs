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
	using System.Collections.Generic;
	using System.Runtime.Serialization;
	using System.Text.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// An exception that is thrown if there is no matching API version found.
	/// </summary>
	[Serializable]
	public class ServerApiNotSupportedException : RestClientException
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerApiNotSupportedException" /> class.
		/// </summary>
		/// <param name="versions">The list of offered versions.</param>
		/// <param name="supportedMajorVersions">The list of supported major versions.</param>
		/// <param name="reason">The reason for the exception.</param>
		public ServerApiNotSupportedException(
			InterfaceVersionRange versions,
			IReadOnlyCollection<int> supportedMajorVersions = null,
			ServerApiNotSupportedReason reason = ServerApiNotSupportedReason.VersionsNotSupported )
			: base( CreateExceptionMessage( reason ) )
		{
			Versions = versions ?? throw new ArgumentNullException( nameof( versions ) );
			Reason = reason;
			SupportedMajorVersions = supportedMajorVersions;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServerApiNotSupportedException" /> class.
		/// </summary>
		/// <param name="versions">The list of offered versions.</param>
		/// <param name="innerException">
		/// The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no
		/// inner exception is specified.
		/// </param>
		/// <param name="reason">The reason for the exception.</param>
		/// <param name="supportedMajorVersions">The list of supported major versions.</param>
		public ServerApiNotSupportedException(
			InterfaceVersionRange versions,
			Exception innerException,
			IReadOnlyCollection<int> supportedMajorVersions = null,
			ServerApiNotSupportedReason reason = ServerApiNotSupportedReason.VersionsNotSupported )
			: base( CreateExceptionMessage( reason ), innerException )
		{
			Versions = versions ?? throw new ArgumentNullException( nameof( versions ) );
			SupportedMajorVersions = supportedMajorVersions;
			Reason = reason;
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
			Versions = JsonSerializer.Deserialize<InterfaceVersionRange>( info.GetString( nameof( Versions ) ) );
			SupportedMajorVersions = JsonSerializer.Deserialize<IReadOnlyCollection<int>>( info.GetString( nameof( SupportedMajorVersions ) ) );
			Reason = JsonSerializer.Deserialize<ServerApiNotSupportedReason>( info.GetString( nameof( Reason ) ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// A list of interface versions that are offered by the server. The list is empty if requesting offered
		/// versions failed.
		/// </summary>
		public InterfaceVersionRange Versions { get; }

		/// <summary>
		/// A list of known interface versions that are supported. This value is null if no supported versions were provided.
		/// </summary>
		public IReadOnlyCollection<int> SupportedMajorVersions { get; }

		/// <summary>
		/// An enum value defining the precise reason for the exception.
		/// </summary>
		public ServerApiNotSupportedReason Reason { get; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );

			info.AddValue( nameof( Versions ), JsonSerializer.Serialize( Versions ) );
			info.AddValue( nameof( SupportedMajorVersions ), JsonSerializer.Serialize( SupportedMajorVersions ) );
			info.AddValue( nameof( Reason ), JsonSerializer.Serialize( Reason ) );
		}

		private static string CreateExceptionMessage( ServerApiNotSupportedReason reason )
		{
			const string msg = "The server supports no known interface version.";
			return reason switch
			{
				ServerApiNotSupportedReason.VersionsTooHigh      => msg + " The versions offered by the server are too high.",
				ServerApiNotSupportedReason.VersionsTooLow       => msg + " The versions offered by the server are too low.",
				ServerApiNotSupportedReason.VersionsNotSupported => msg,
				_                                                => msg
			};
		}

		#endregion
	}

	/// <summary>
	/// An enumeration with possible reasons for a <see cref="ServerApiNotSupportedException"/>.
	/// </summary>
	public enum ServerApiNotSupportedReason
	{
		/// <summary>The interface versions offered by the server are not supported.</summary>
		VersionsNotSupported,

		/// <summary>The interface versions offered by the server are all lower than the supported major versions.</summary>
		VersionsTooLow,

		/// <summary>The interface versions offered by the server are all higher than the supported major versions.</summary>
		VersionsTooHigh
	}
}