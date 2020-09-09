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
	using System.Text;

	#endregion

	[Serializable]
	public class OperationNotSupportedOnServerException : RestClientException
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationNotSupportedOnServerException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="requiredVersion">The required version.</param>
		/// <param name="currentVersion">The current version.</param>
		public OperationNotSupportedOnServerException( string message, Version requiredVersion, Version currentVersion )
			: base( MakeMessage( message, requiredVersion, currentVersion ) )
		{
			RequiredVersion = requiredVersion;
			CurrentVersion = currentVersion;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="OperationNotSupportedOnServerException" /> class.
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
		protected OperationNotSupportedOnServerException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			foreach( var entry in info )
			{
				switch( entry.Name )
				{
					case nameof( RequiredVersion ):
						RequiredVersion = (Version)entry.Value;
						break;

					case nameof( CurrentVersion ):
						CurrentVersion = (Version)entry.Value;
						break;
				}
			}
		}

		#endregion

		#region properties

		public Version RequiredVersion { get; private set; }
		public Version CurrentVersion { get; private set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );
			info.AddValue( nameof( RequiredVersion ), RequiredVersion );
			info.AddValue( nameof( CurrentVersion ), CurrentVersion );
		}

		private static string MakeMessage( string message, Version requiredVersion, Version currentVersion )
		{
			var builder = new StringBuilder();
			builder.Append( message ).AppendLine();
			builder.Append( "Required server interface: " ).Append( requiredVersion ).AppendLine();
			builder.Append( "Current server interface: " ).Append( currentVersion );

			return builder.ToString();
		}

		#endregion
	}
}