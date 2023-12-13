#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Linq;
	using System.Text;

	#endregion

	/// <summary>
	/// Helper class for REST webservice calls.
	/// </summary>
	internal static class RestClientHelper
	{
		#region constants

		/// <summary>Start identifier for a list inside a HTTP query.</summary>
		private const string QueryListStart = "{";

		/// <summary>End identifier for a list inside a HTTP query.</summary>
		private const string QueryListStop = "}";

		#endregion

		#region methods

		/// <summary>
		/// Parses a string to a list of ushorts.
		/// </summary>
		public static IReadOnlyList<ushort> ConvertStringToUInt16List( string value )
		{
			if( value == null )
				return Array.Empty<ushort>();
			if( value.StartsWith( QueryListStart ) && value.EndsWith( QueryListStop ) )
				value = value.Substring( 1, value.Length - 2 );
			if( string.IsNullOrEmpty( value ) )
				return Array.Empty<ushort>();
			try
			{
				return value.Split( ',' ).Select( s => ushort.Parse( s, CultureInfo.InvariantCulture ) ).ToArray();
			}
			catch( Exception )
			{
				throw new FormatException( $"Error on parsing {value} due to bad formatting." );
			}
		}

		/// <summary>
		/// Parses a string to a list of Guids.
		/// </summary>
		public static IReadOnlyList<Guid> ConvertStringToGuidList( string value )
		{
			return StringUuidTools.StringUuidListToGuidList( ParseListToStringArray( value ) );
		}

		/// <summary>Parses a list of strings.</summary>
		public static IEnumerable<string> ParseListToStringArray( string value )
		{
			if( value.StartsWith( QueryListStart ) && value.EndsWith( QueryListStop ) )
				value = value.Substring( 1, value.Length - 2 );

			return value.Split( ',' ).Select( p => p.Trim() );
		}

		/// <summary>Creates a list string from the ushorts <code>value</code>.</summary>
		public static string ConvertUshortArrayToString( IReadOnlyCollection<ushort> value )
		{
			return ConvertFormattableArrayToString( value, formatProvider: CultureInfo.InvariantCulture );
		}

		/// <summary>Creates a list string from the uuids <code>value</code>.</summary>
		public static string ConvertGuidListToString( IReadOnlyCollection<Guid> value )
		{
			return ConvertFormattableArrayToString( value, "D" );
		}

		private static string ConvertFormattableArrayToString<T>( IReadOnlyCollection<T> value, string format = null, IFormatProvider formatProvider = null ) where T : IFormattable
		{
			if( value == null || value.Count == 0 )
				return "";

			return ToListString( value.Select( v => v.ToString( format, formatProvider ) ) );
		}

		/// <summary>Creates a list string from <paramref name="list"/>.</summary>
		private static string ToListString( IEnumerable<string> list )
		{
			var listString = string.Join( ",", list );
			if( string.IsNullOrEmpty( listString ) )
				return "";

			var sb = new StringBuilder();
			sb.Append( QueryListStart );
			sb.Append( listString );
			sb.Append( QueryListStop );

			return sb.ToString();
		}

		#endregion
	}
}