#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString
{
	#region usings

	using System;
	using System.Runtime.Serialization;

	#endregion

	[Serializable]
	public class FilterStringException : Exception
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterStringException"/> class.
		/// </summary>
		/// <param name="message">The message that describes the error.</param>
		/// <param name="linePosition">The line position.</param>
		public FilterStringException( string message, int? linePosition ) : base( FormatMessageWithPosition( message, linePosition ) )
		{
			CoreMessage = message;
			LinePosition = linePosition;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterStringException" /> class.
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
		protected FilterStringException( SerializationInfo info, StreamingContext context )
			: base( info, context )
		{
			foreach( var entry in info )
			{
				switch( entry.Name )
				{
					case nameof( CoreMessage ):
						CoreMessage = (string)entry.Value;
						break;

					case nameof( LinePosition ):
						LinePosition = (int?)entry.Value;
						break;
				}
			}
		}

		#endregion

		#region properties

		public string CoreMessage { get; }
		public int? LinePosition { get; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override void GetObjectData( SerializationInfo info, StreamingContext context )
		{
			base.GetObjectData( info, context );
			info.AddValue( nameof( CoreMessage ), CoreMessage );
			info.AddValue( nameof( LinePosition ), LinePosition );
		}

		private static string FormatMessageWithPosition( string message, int? linePosition )
		{
			if( linePosition != null && linePosition > -1 )
			{
				// Positions are 0-based. However, if we display a position it should be one based.
				return $"Position {linePosition + 1}: {message}";
			}

			return message;
		}

		#endregion
	}
}