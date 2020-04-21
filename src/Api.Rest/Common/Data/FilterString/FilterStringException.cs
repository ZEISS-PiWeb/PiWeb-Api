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

	#endregion

	public class FilterStringException : Exception
	{
		#region constructors

		public FilterStringException( string message, int? linePosition ) : base( FormatMessageWithPosition( message, linePosition ) )
		{
			CoreMessage = message;
			LinePosition = linePosition;
		}

		#endregion

		#region properties

		public string CoreMessage { get; }
		public int? LinePosition { get; }

		#endregion

		#region methods

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
