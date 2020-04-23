#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Runtime.CompilerServices;
	using System.Text;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>Helper class for converting <see cref="PathInformation"/> objects to strings and vice versa.</summary>
	/// <remarks>
	/// There are two string models for paths. Database string and colloquial strings. Delimiter = "/". Escape = "\"
	/// 
	/// Database string is used in the database tables and has a very strict model. It starts and ends with a delimiter. No empty
	/// path components are allowed. Delimiters are quoted with escapes and escapes are quoted with escapes. The root path is
	/// represented as "/" with structure "" or <code>null</code>. Together with the structure full roundtrip is always possible
	/// and consistent.
	/// 
	/// Colloquial string is your everyday path as string representation. It has a less strict model with optional escaping. It allows for
	/// a full roundtrip conversion if handled correctly. It may start with a delimiter. It must not end with a delimiter.
	/// No empty path components are allowed. Delimiters are quoted with escapes and escapes are quoted with escapes.
	/// The root path is represented as "/". An empty string is not allowed! Full roundtrip conversion maybe be possible but may not be consistent.
	/// 
	/// Methods in this class are either internal and handle the database string format and enforce it or are public and handle the
	/// colloquial string format.
	/// 
	/// Examples (d = database, c = colloquial)
	/// d /
	/// c /
	/// 
	/// d /part/
	/// c /part
	/// c part
	/// 
	/// d /part/subpart/
	/// c /part/subpart
	/// c part/subpart
	/// 
	/// d /part_with_slash\//
	/// c /part_with_slash\/
	/// c part_with_slash\/
	/// 
	/// d /part_with_slash\//subpart_with_slash\//
	/// c /part_with_slash\//subpart_with_slash\/
	/// c part_with_slash\//subpart_with_slash\/
	/// 
	/// d /part_with_backslash\\/
	/// c /part_with_backslash\\
	/// c part_with_backslash\\
	/// 
	/// 3. format "roundtrip" needed that contains the path structure and the path as database string.
	/// This string can be converted back into a PathInformation object without further information.
	/// 
	/// Examples:
	/// P:/part/
	/// PP:/part/subpart/
	/// PC:/part/characteristic/
	/// PCC:/part/characteristic/characteristic/
	/// 
	/// The roundtrip string for the root part is special and is represented as "/" (no structure information needed).
	/// </remarks>
	internal static class PathHelper
	{
		#region constants

		/// <summary> Delimiter character for the path components. </summary>
		private const char Delimiter = '/';

		/// <summary> Escaped delimiter character for the path components </summary>
		private const string EscapedDelimiter = @"\/";

		/// <summary> Delimiter string for the path components </summary>
		public const string DelimiterString = "/";

		/// <summary> Escape character for delimiter characters. </summary>
		private const char Escape = '\\';

		/// <summary> Escape character as string for delimiter characters. </summary>
		private const string EscapeString = @"\";

		#endregion

		#region methods

		/// <summary>
		/// Creates from <paramref name="path"/> in roundtrip format ("structure:database path") a <see cref="PathInformation"/> object.
		/// </summary>
		[NotNull]
		public static PathInformation RoundtripString2PathInformation( [NotNull] string path )
		{
			if( string.IsNullOrEmpty( path ) )
				throw new ArgumentException( "The path string must not be null or empty.", nameof( path ) );

			// fast code path for root path
			if( path == DelimiterString ) return PathInformation.Root;

			var index = path.IndexOf( ':' );
			if( index < 1 )
				throw new ArgumentException( "The path must have the following structure:\"structure:path\", e.g.: \"PC:/part/characteristic/\"." );

			return DatabaseString2PathInformation( path.Substring( index + 1 ), path.Substring( 0, index ) );
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in database format a <see cref="PathInformation"/> object.
		/// Depending on <paramref name="structure"/> it will be a part or a characteristic.
		/// </summary>
		[NotNull]
		private static PathInformation DatabaseString2PathInformation( [NotNull] string path, string structure )
		{
			if( string.IsNullOrEmpty( path ) )
				throw new ArgumentException( "The path string must not be null or empty.", nameof( path ) );

			// fast code path for root path
			if( path == DelimiterString && string.IsNullOrEmpty( structure ) )
				return PathInformation.Root;

			return String2PathInformationInternal( path, structure, i => StructureIdentifierToEntity( structure, i ) );
		}

		[MethodImpl( MethodImplOptions.AggressiveInlining )]
		private static InspectionPlanEntity StructureIdentifierToEntity( string structure, int index )
		{
			return structure[ index ] == 'P' ? InspectionPlanEntity.Part : InspectionPlanEntity.Characteristic;
		}

		[NotNull]
		private static PathInformation String2PathInformationInternal( [NotNull] string path, string maybeStructure, Func<int, InspectionPlanEntity> entitySelector )
		{
			// tests for path invariants
			// 1) start with delimiter
			if( path[ 0 ] != Delimiter )
				throw new ArgumentException( $"The database path string must start with a delimiter '{DelimiterString}'. Actual value is '{path}'." );
			// 2) end with delimiter - not tested easily as it might be escaped

			var initialCount = maybeStructure?.Length ?? 2;
			var result = path.Contains( EscapeString )
				?
				// difficult code with quoting
				GetPathElementsFromQuotedString( path, entitySelector, initialCount, maybeStructure )
				:
				// easy code without quoting
				GetPathElementsFromUnquotedString( path, entitySelector, initialCount );

			if( maybeStructure != null && result.Count != maybeStructure.Length )
				throw new InvalidOperationException( $"Mismatch in number of path components between pathstring ('{path}') and structure ('{maybeStructure}')." );

			return new PathInformation( result );
		}

		private static List<PathElement> GetPathElementsFromQuotedString( [NotNull] string path, Func<int, InspectionPlanEntity> entitySelector, int initialCount, string maybeStructure )
		{
			var result = new List<PathElement>( initialCount );

			// correctly unescape quotes by evaluating all characters left to right, might be slow but gives correct results
			var sb = new StringBuilder( 25 );
			var pathIndex = 0;
			var escaped = false;
			// start at first character as the first character is always the delimiter
			for( var i = 1; i < path.Length; i += 1 )
			{
				if( !escaped && path[ i ] == Escape )
				{
					// Unmaskiertes Maskierungszeichen gefunden => nächstes Zeichen ist maskiert
					escaped = true;
					continue;
				}

				if( !escaped && path[ i ] == Delimiter )
				{
					// Unmaskiertes Pfadtrennzeichen gefunden => Ende des Namens des aktuellen Pfadelements erreicht
					var value = sb.ToString();
					if( value.Length == 0 )
						throw new InvalidOperationException( $"The path string '{path}' must not contain any empty path element." );

					// Bei Bauteilen den Pfadbestandteil internen, um Speicherplatz zu sparen
					var type = entitySelector( pathIndex );
					if( type == InspectionPlanEntity.Part )
						value = string.Intern( value );

					result.Add( new PathElement( type, value ) );

					sb.Clear();
					pathIndex += 1;
					continue;
				}

				sb.Append( path[ i ] );
				escaped = false;
			}

			if( escaped )
				throw new InvalidOperationException( $"The path string '{path}' contains invalid quoting. every quote character must be followed by another quote character. single quote characters at the end ar not allowed." );
			if( sb.Length > 0 )
				throw new InvalidOperationException( $"The path string '{path}' does not end with an unquoted delimiter character." );
			if( maybeStructure != null && pathIndex != maybeStructure.Length )
				throw new InvalidOperationException( $"The number of components in path string '{path}' and structure '{maybeStructure}' must be equal." );

			return result;
		}

		private static List<PathElement> GetPathElementsFromUnquotedString( [NotNull] string path, Func<int, InspectionPlanEntity> entitySelector, int initialCount )
		{
			var components = path.Split( Delimiter );
			if( components.Length < 3 )
				throw new InvalidOperationException( $"The path string '{path}' must have at least 3 components after splitting because it has to start and end with a delimiter character." );
			if( components[ 0 ].Length != 0 )
				throw new InvalidOperationException( $"The first component of path string '{path}' has to be an empty string after splitting because it has to start with a delimiter character." );
			if( components[ components.Length - 1 ].Length != 0 )
				throw new InvalidOperationException( $"The last component of path string '{path}' has to be an empty string after splitting because it has to end with a delimiter character." );

			var pathElements = new List<PathElement>( initialCount );
			for( var i = 1; i < components.Length - 1; i += 1 )
			{
				var value = components[ i ];
				if( value.Length == 0 )
					throw new InvalidOperationException( $"The path string '{path}' must not contain any empty path element." );
				var type = entitySelector( i - 1 );

				// Bei Bauteilen den Pfadbestandteil internen, um Speicherplatz zu sparen
				if( type == InspectionPlanEntity.Part )
					value = string.Intern( value );

				pathElements.Add( new PathElement( type, value ) );
			}

			return pathElements;
		}

		/// <summary>
		/// Converts <paramref name="path"/> to a string in roundtrip format ("structure:database path").
		/// </summary>
		[NotNull]
		public static string PathInformation2RoundtripString( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			// fast code path for root path
			if( path.IsRoot ) return DelimiterString;

			var sb = new StringBuilder( 25 );
			sb.Append( GetStructure( path ) );
			sb.Append( ":" );
			sb.Append( PathInformation2DatabaseString( path ) );

			return sb.ToString();
		}

		/// <summary>
		/// Converts <paramref name="path"/> to a string in database format.
		/// </summary>
		[NotNull]
		private static string PathInformation2DatabaseString( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			// fast code path for root path
			if( path.IsRoot ) return DelimiterString;

			var sb = new StringBuilder( 25 );
			sb.Append( Delimiter );
			PathInformation2StringInternal( sb, path );
			sb.Append( Delimiter );

			return sb.ToString();
		}

		private static void PathInformation2StringInternal( [NotNull] StringBuilder sb, [NotNull] PathInformation path )
		{
			var count = path.Count;
			for( var i = 0; i < path.Count; i++ )
			{
				sb.Append( path[ i ].Value.Replace( @"\", @"\\" ).Replace( DelimiterString, EscapedDelimiter ) );
				if( ( i + 1 ) < count )
					sb.Append( Delimiter );
			}
		}

		/// <summary>
		/// Returns the structure of the <paramref name="path"/>.
		/// </summary>
		[NotNull]
		private static string GetStructure( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			var result = new char[ path.Count ];
			for( var i = 0; i < path.Count; i++ )
			{
				result[ i ] = path[ i ].Type == InspectionPlanEntity.Part ? 'P' : 'C';
			}

			return new string( result );
		}

		#endregion
	}
}