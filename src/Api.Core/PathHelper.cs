#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Core
{
	#region usings

	using System;
	using System.Buffers;
	using System.Collections.Generic;
	using System.Text;
	using CommunityToolkit.HighPerformance.Buffers;
	using JetBrains.Annotations;
	using Microsoft.Extensions.ObjectPool;

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
	public static class PathHelper
	{
		#region constants

		/// <summary> Delimiter character for the path components. </summary>
		public const char Delimiter = '/';

		/// <summary> Delimiter string for the path components </summary>
		public const string DelimiterString = "/";

		/// <summary> Escaped delimiter character for the path components </summary>
		private const string EscapedDelimiter = @"\/";

		/// <summary> Escape character for delimiter characters. </summary>
		private const char Escape = '\\';

		#endregion

		#region properties

		private static readonly ObjectPool<StringBuilder> ObjectPool = Microsoft.Extensions.ObjectPool.ObjectPool.Create( new StringBuilderPooledObjectPolicy() );

		#endregion

		#region methods

		/// <summary>
		/// Creates from <paramref name="path"/> in colloquial format a part <see cref="PathInformation"/> object.
		/// </summary>
		[NotNull]
		public static PathInformation String2PartPathInformation( [NotNull] string path )
		{
			return ColloquialString2PathInformationInternal( path, InspectionPlanEntity.Part, null );
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in colloquial format a part <see cref="PathInformation"/> object.
		/// </summary>
		[NotNull]
		public static PathInformation String2PartPathInformation( [NotNull] string path, StringPool stringPool )
		{
			return ColloquialString2PathInformationInternal( path, InspectionPlanEntity.Part, stringPool );
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in colloquial format a characteristic <see cref="PathInformation"/> object.
		/// </summary>
		[NotNull]
		public static PathInformation String2CharPathInformation( [NotNull] string path )
		{
			return ColloquialString2PathInformationInternal( path, InspectionPlanEntity.Characteristic, null );
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in colloquial format a characteristic <see cref="PathInformation"/> object.
		/// </summary>
		[NotNull]
		public static PathInformation String2CharPathInformation( [NotNull] string path, StringPool stringPool )
		{
			return ColloquialString2PathInformationInternal( path, InspectionPlanEntity.Characteristic, stringPool );
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in colloquial format a <see cref="PathInformation"/> object.
		/// All path elements will be of the type given in <paramref name="entity"/>.
		/// </summary>
		[NotNull]
		private static PathInformation ColloquialString2PathInformationInternal( [NotNull] string path, InspectionPlanEntity entity, [CanBeNull] StringPool stringPool )
		{
			if( string.IsNullOrEmpty( path ) )
				throw new ArgumentException( "The path string must not be null or empty.", nameof( path ) );

			// fast code path for root path
			if( path == DelimiterString )
			{
				if( entity != InspectionPlanEntity.Part )
					throw new ArgumentException( "The root path must always be of type part.", nameof( entity ) );

				return PathInformation.Root;
			}

			// convert to database format by prepending a delimiter if it is not already present
			if( !path.StartsWith( DelimiterString, StringComparison.Ordinal ) )
				path = DelimiterString + path;

			// convert to database format by appending a delimiter if it is not already present (beware of escaping)
			if(!EndsWithDelimiter( path ))
				path += DelimiterString;

			return String2PathInformationInternal( path.AsSpan(), "".AsSpan(), stringPool, entity );
		}

		/// <summary>
		/// Checks whether the path string already contains a delimiter at the end.
		/// </summary>
		private static bool EndsWithDelimiter( string path )
		{
			var isEscaped = false;
			var hasDelimiter = false;

			foreach( var character in path )
			{
				switch( character )
				{
					case Escape when !isEscaped:
						isEscaped = true;
						hasDelimiter = false;
						continue;
					case Delimiter:
					{
						if( !isEscaped )
							hasDelimiter = true;
						break;
					}
					default:
						hasDelimiter = false;
						break;
				}

				isEscaped = false;
			}
			return hasDelimiter;
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in roundtrip format ("structure:database path") a <see cref="PathInformation"/> object.
		/// </summary>
		[NotNull]
		public static PathInformation RoundtripString2PathInformation( [NotNull] string path )
		{
			if( string.IsNullOrEmpty( path ) )
				throw new ArgumentException( "The path string must not be null or empty.", nameof( path ) );

			// fast code path for root path
			if( DelimiterString.Equals( path  ) )
				return PathInformation.Root;

			var index = path.IndexOf( ':' );
			if( index < 1 )
				throw new ArgumentException( "The path must have the following structure:\"structure:path\", e.g.: \"PC:/part/characteristic/\"." );

			var pathString = path.AsSpan( index + 1 );
			var structureString = path.AsSpan( 0, index );

			return DatabaseString2PathInformation( pathString, structureString );
		}

		/// <summary>
		/// Creates from <paramref name="path"/> in database format a <see cref="PathInformation"/> object.
		/// Depending on <paramref name="structure"/> it will be a part or a characteristic.
		/// </summary>
		[NotNull]
		public static PathInformation DatabaseString2PathInformation( ReadOnlySpan<char> path, ReadOnlySpan<char> structure, [CanBeNull] StringPool stringPool = null )
		{
			if( path.Length == 0 )
				throw new ArgumentException( "The path string must not be null or empty.", nameof( path ) );

			// fast code path for root path
			if( path[ 0 ] == Delimiter && structure.Length == 0 )
				return PathInformation.Root;

			return String2PathInformationInternal( path, structure, stringPool, null );
		}

		private static InspectionPlanEntity StructureIdentifierToEntity( ReadOnlySpan<char> structure, int index )
		{
			if( structure.Length <= index )
				throw new ArgumentException( $"Invalid structure '{structure.ToString()}'. Expected a structure that has at least a length of {index}." );
			return structure[ index ] == 'P' ? InspectionPlanEntity.Part : InspectionPlanEntity.Characteristic;
		}

		[NotNull]
		private static PathInformation String2PathInformationInternal( ReadOnlySpan<char> path, ReadOnlySpan<char> structure, [CanBeNull] StringPool stringPool, InspectionPlanEntity? explicitEntity )
		{
			if( structure.Length > 0 && !explicitEntity.HasValue )
			{
				var result = new PathElement[ structure.Length ];
				if( path.IndexOf( Escape ) > 0 )
				{
					// difficult code with quoting
					GetPathElementsFromQuotedString( path, structure, null, result );
				}
				else
				{
					// easy code without quoting
					GetPathElementsFromUnquotedString( path, structure, null, stringPool, result );
				}

				return new PathInformation( result );
			}
			else
			{
				var result = new PathElement[ CountDelimiter( path ) ];
				var elementCount = 0;
				if( path.IndexOf( Escape ) > 0 )
				{
					// difficult code with quoting
					elementCount = GetPathElementsFromQuotedString( path, structure, explicitEntity, result );
				}
				else
				{
					// easy code without quoting
					elementCount = GetPathElementsFromUnquotedString( path, structure, explicitEntity, stringPool, result );
				}
				return new PathInformation( new ArraySegment<PathElement>( result, 0, elementCount ) );
			}
		}

		private static int CountDelimiter( ReadOnlySpan<char> path )
		{
			var result = 0;
			foreach( var ch in path )
			{
				if( ch == Delimiter )
					result++;
			}
			return result;
		}

		// correctly unescape quotes by evaluating all characters left to right, might be slow but gives correct results
		private static int GetPathElementsFromQuotedString( ReadOnlySpan<char> path, ReadOnlySpan<char> structure, InspectionPlanEntity? explicitEntity, IList<PathElement> result )
		{
			VerifyQuotedPath( path );

			var sb = ObjectPool.Get();
			sb.Clear();

			try
			{
				var pathIndex = 0;
				var escaped = false;
				var count = 0;

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
							throw new InvalidOperationException( $"The path string '{path.ToString()}' must not contain any empty path element." );

						// Bei Bauteilen den Pfadbestandteil internen, um Speicherplatz zu sparen
						var type = explicitEntity ?? StructureIdentifierToEntity( structure, pathIndex );

						result[ count++ ] = new PathElement( type, value );

						sb.Clear();
						pathIndex += 1;
						continue;
					}

					sb.Append( path[ i ] );
					escaped = false;
				}

				if( escaped )
					throw new InvalidOperationException( $"The path string '{path.ToString()}' contains invalid quoting. every quote character must be followed by another quote character. single quote characters at the end ar not allowed." );
				if( sb.Length > 0 )
					throw new InvalidOperationException( $"The path string '{path.ToString()}' does not end with an unquoted delimiter character." );

				return count;
			}
			finally
			{
				ObjectPool.Return( sb );
			}
		}

		private static int GetPathElementsFromUnquotedString( ReadOnlySpan<char> path, ReadOnlySpan<char> structure, InspectionPlanEntity? explicitEntity, [CanBeNull] StringPool stringPool, IList<PathElement> result )
		{
			VerifyUnquotedPath( path );

			// Remove leading and trailing slashes
			var slice = path.Slice( 1, path.Length - 1 );

			var i = 0;
			var count = 0;

			while( !slice.IsEmpty )
			{
				var substringLength = slice.IndexOf( Delimiter );
				substringLength = substringLength < 0 ? slice.Length : substringLength;

				var type = explicitEntity ?? StructureIdentifierToEntity( structure, i );
				var value = stringPool?.GetOrAdd( slice.Slice( 0, substringLength ) ) ?? slice.Slice( 0, substringLength ).ToString();

				if( value.Length == 0 )
					throw new InvalidOperationException( $"The path string '{path.ToString()}' must not contain any empty path element." );

				result[ count++ ] = new PathElement( type, value );

				slice = slice.Slice( substringLength + 1 );
				i++;
			}

			return count;
		}

		private static void VerifyQuotedPath( ReadOnlySpan<char> path )
		{
			// tests for path invariants
			// 1) start with delimiter
			// 2) end with delimiter - not tested easily as it might be escaped

			if( path[ 0 ] != Delimiter )
				throw new InvalidOperationException( $"The first character of path string '{path.ToString()}' has to a delimiter character." );
		}

		private static void VerifyUnquotedPath( ReadOnlySpan<char> path )
		{
			// tests for path invariants
			// 1) start with delimiter
			// 2) end with delimiter

			if( path.Length < 3 )
				throw new InvalidOperationException( $"The path string '{path.ToString()}' must have at least 3 components after splitting because it has to start and end with a delimiter character." );
			if( path[ 0 ] != Delimiter )
				throw new InvalidOperationException( $"The first character of path string '{path.ToString()}' has to a delimiter character." );
			if( path[ path.Length - 1 ] != Delimiter )
				throw new InvalidOperationException( $"The last character of path string '{path.ToString()}' has to a delimiter character." );
		}

		/// <summary>
		/// Converts <paramref name="path"/> to a string in colloquial format.
		/// The result for a the root path is always "/".
		/// For all other paths the result does NOT start with a delimiter.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null" />.</exception>
		[NotNull]
		public static string PathInformation2String( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			// fast code path for root path
			if( path.IsRoot )
				return DelimiterString;

			var sb = ObjectPool.Get();
			sb.Clear();

			try
			{
				PathInformation2StringInternal( sb, path );
				return sb.ToString();
			}
			finally
			{
				ObjectPool.Return( sb );
			}
		}

		/// <summary>
		/// Converts <paramref name="path"/> to a string in roundtrip format ("structure:database path").
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null" />.</exception>
		[NotNull]
		public static string PathInformation2RoundtripString( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			// fast code path for root path
			if( path.IsRoot )
				return DelimiterString;

			var sb = ObjectPool.Get();
			sb.Clear();

			try
			{
				AppendStructure( sb, path );
				sb.Append( ':' );
				sb.Append( PathInformation2DatabaseString( path ) );

				return sb.ToString();
			}
			finally
			{
				ObjectPool.Return( sb );
			}
		}

		private static void AppendStructure( StringBuilder sb, PathInformation path )
		{
			// ReSharper disable once ForCanBeConvertedToForeach
			for( var i = 0; i < path.Count; i++ )
			{
				sb.Append( path[ i ].Type == InspectionPlanEntity.Part ? 'P' : 'C' );
			}
		}

		/// <summary>
		/// Converts <paramref name="path"/> to a string in database format.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null" />.</exception>
		[NotNull]
		public static string PathInformation2DatabaseString( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			// fast code path for root path
			if( path.IsRoot )
				return DelimiterString;

			var sb = ObjectPool.Get();
			try
			{
				sb.Clear();
				sb.Append( Delimiter );
				PathInformation2StringInternal( sb, path );
				sb.Append( Delimiter );

				return sb.ToString();
			}
			finally
			{
				ObjectPool.Return( sb );
			}
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
		/// <exception cref="ArgumentNullException"><paramref name="path"/> is <see langword="null" />.</exception>
		[NotNull]
		public static string GetStructure( [NotNull] PathInformation path )
		{
			if( path == null ) throw new ArgumentNullException( nameof( path ) );

			var result = ArrayPool<char>.Shared.Rent( path.Count );
			try
			{
				for( var i = 0; i < path.Count; i++ )
				{
					result[ i ] = path[ i ].Type == InspectionPlanEntity.Part ? 'P' : 'C';
				}
				return new string( result, 0, path.Count );
			}
			finally
			{
				ArrayPool<char>.Shared.Return( result );
			}
		}

		[Obsolete( "Use DatabaseString2PathInformation or RoundtripString2PathInformation instead" )]
		public static PathInformation String2PathInformation( string path, string structure )
		{
			return DatabaseString2PathInformation( path.AsSpan(), structure.AsSpan() );
		}

		#endregion
	}
}