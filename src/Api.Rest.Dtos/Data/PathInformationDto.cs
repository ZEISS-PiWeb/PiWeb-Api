#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text;
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters;

	#endregion

	/// <summary>
	/// Every inspection plan entity is uniquely identified by a path. The path contains a typed list of path elements (<see cref="PathElementDto"/>).
	/// This list of path elements describes a hierarchy with the top most path element as the first element in that list and the deepest path element
	/// as the last element in that list. Please note that a path has the following rules:
	/// * The path elements are compared ignoring the case
	/// * A part path element can have part path and characteristic path elements as children
	/// * A characteristic path element can have other characteristic elements as children
	/// * A characteristic path element can not have part elements as children
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( PathInformationConverter ) )]
	[JsonConverter( typeof( JsonPathInformationConverter ) )]
	public sealed class PathInformationDto : IFormattable, IEnumerable<PathElementDto>
	{
		#region members

		/// <summary> Returns the root path. </summary>
		public static readonly PathInformationDto Root = new PathInformationDto();

		private readonly ArraySegment<PathElementDto> _PathElements;
		private int _HashCode = -1;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="PathInformationDto"/> class.
		/// </summary>
		public PathInformationDto()
		{
			_PathElements = new ArraySegment<PathElementDto>( Array.Empty<PathElementDto>() );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PathInformationDto"/> class.
		/// </summary>
		public PathInformationDto( IEnumerable<PathElementDto> paths )
		{
			_PathElements = new ArraySegment<PathElementDto>( paths.ToArray() );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PathInformationDto"/> class.
		/// </summary>
		public PathInformationDto( params PathElementDto[] paths )
		{
			_PathElements = new ArraySegment<PathElementDto>( paths );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PathInformationDto"/> class.
		/// </summary>
		internal PathInformationDto( ArraySegment<PathElementDto> path )
		{
			_PathElements = path;
		}

		#endregion

		#region properties

		/// <summary>
		/// Indexer to access the path elements by its index.
		/// </summary>
		public PathElementDto this[ int index ]
		{
			get
			{
				if( index - _PathElements.Offset >= _PathElements.Count )
					throw new IndexOutOfRangeException();

				return _PathElements.Array[ _PathElements.Offset + index ];
			}
		}

		/// <summary>
		/// Returns the parent path.
		/// </summary>
		public PathInformationDto ParentPath => Parent( 1 );

		/// <summary>
		/// Returns the parent part path.
		/// </summary>
		public PathInformationDto ParentPartPath
		{
			get
			{
				var upcount = 1;
				while( upcount < Count && this[ Count - 1 - upcount ].Type == InspectionPlanEntityDto.Characteristic )
				{
					upcount += 1;
				}

				return Parent( upcount );
			}
		}

		/// <summary>
		/// Returns the root part of ths path.
		/// </summary>
		public PathInformationDto RootPartPath => IsRoot ? Root : Parent( Count - 1 );

		/// <summary>
		/// Returns true if this path is the root part.
		/// </summary>
		public bool IsRoot => _PathElements.Count == 0;

		/// <summary>
		/// Returns the number of path elements.
		/// </summary>
		public int Count => _PathElements.Count;

		/// <summary>
		/// Returns the name part of the path.
		/// </summary>
		public string Name => _PathElements.Count == 0 ? "" : _PathElements.Array[ _PathElements.Offset + _PathElements.Count - 1 ].Value;

		/// <summary>
		/// Returns the typed name of the path - a path element. As the root part does not have a name <code>null</code> is returned.
		/// </summary>
		public PathElementDto TypedName => _PathElements.Count == 0 ? PathElementDto.EmptyPart : this[ Count - 1 ];

		/// <summary>
		/// Returns the type of the last path element.
		/// </summary>
		public InspectionPlanEntityDto Type => _PathElements.Count == 0 ? InspectionPlanEntityDto.Part : this[ Count - 1 ].Type;

		#endregion

		#region methods

		/// <summary> Checks if the <paramref name="path"/> is <code>null</code> or the root part. </summary>
		/// <returns>True, if the <paramref name="path"/> is <code>null</code> or the root part otherwise false.</returns>
		public static bool IsNullOrRoot( PathInformationDto path )
		{
			return path == null || path.IsRoot;
		}

		/// <summary> Checks if this path is below <paramref name="path"/> or is equal to <paramref name="path"/>.
		/// Also checks the path elements type!
		/// </summary>
		public bool IsBelow( PathInformationDto path )
		{
			if( path == null || path.Count > Count )
				return false;

			var i = 0;
			foreach( var pathElement in path )
			{
				if( !this[ i++ ].Equals( pathElement ) )
					return false;
			}

			return true;
		}

		/// <summary>
		/// Returns the parent path by stepping up the <code>upcount</code> number of levels in the path hierarchy.
		/// </summary>
		/// <param name="upcount">Number of levels to go upwards the path hierarchy (&gt;= 0).</param>
		/// <returns>The path without the last n levels, the path itself or the root path.</returns>
		public PathInformationDto Parent( int upcount )
		{
			if( upcount < 0 )
				throw new ArgumentOutOfRangeException( nameof( upcount ) );

			if( upcount == 0 )
				return this;

			if( upcount >= Count )
				return Root;

			return new PathInformationDto( new ArraySegment<PathElementDto>( _PathElements.Array, _PathElements.Offset, Count - upcount ) );
		}

		/// <summary>
		/// Add operator. Combines the path <paramref name="p1"/> and the path <paramref name="p2"/> to a new <see cref="PathInformationDto"/> instance.
		/// </summary>
		public static PathInformationDto operator +( PathInformationDto p1, PathInformationDto p2 )
		{
			return Combine( p1, p2 );
		}

		/// <summary>
		/// Add operator. Combines the path <paramref name="p1"/> and the path <paramref name="p2"/> to a new <see cref="PathInformationDto"/> instance.
		/// </summary>
		public static PathInformationDto operator +( PathInformationDto p1, PathElementDto p2 )
		{
			return Combine( p1, p2 );
		}

		/// <summary>
		/// Combines the path <paramref name="p1"/> and the path <paramref name="p2"/> to a new <see cref="PathInformationDto"/> instance.
		/// </summary>
		public static PathInformationDto Combine( PathInformationDto p1, PathInformationDto p2 )
		{
			if( p1 == null || p1.IsRoot )
				return p2;

			if( p2 == null || p2.IsRoot )
				return p1;

			var newpath = new List<PathElementDto>( p1.Count + p2.Count );
			newpath.AddRange( p1 );
			newpath.AddRange( p2 );

			return new PathInformationDto( newpath );
		}

		/// <summary>
		/// Combines the path <paramref name="path"/> and the path element <paramref name="elem"/> to a new <see cref="PathInformationDto"/> instance.
		/// </summary>
		public static PathInformationDto Combine( PathInformationDto path, PathElementDto elem )
		{
			if( elem.IsEmpty )
				return path;

			if( path == null )
				return new PathInformationDto( elem );

			var newpath = new List<PathElementDto>( path.Count + 1 );
			newpath.AddRange( path );
			newpath.Add( elem );

			return new PathInformationDto( newpath );
		}

		/// <summary>
		/// Retrieves a subpath from this instance. The subpath starts at a specified path element index and has a specified count of path elements.
		/// </summary>
		public PathInformationDto SubPath( int startIndex, int count )
		{
			if( startIndex < 0 || startIndex > _PathElements.Count )
				throw new ArgumentOutOfRangeException( nameof( startIndex ), "Parameter 'startIndex' must be greater or equal 0 and less than or equal to the path element count." );

			if( count < 0 || count > _PathElements.Count - startIndex )
				throw new ArgumentOutOfRangeException( nameof( count ), "Parameter 'count' must be greater or equal 0 and less than or equal to the path element count minus startIndex." );

			if( count == 0 ) return Root;
			if( startIndex == 0 && count == _PathElements.Count ) return this;

			return new PathInformationDto( new ArraySegment<PathElementDto>( _PathElements.Array, _PathElements.Offset + startIndex, count ) );
		}

		/// <summary>
		/// Retrieves a subpath from this instance. The subpath starts at a specified path element index and continues to the last path element.
		/// </summary>
		public PathInformationDto SubPath( int startIndex )
		{
			return SubPath( startIndex, _PathElements.Count - startIndex );
		}

		/// <summary>
		/// Retrieves a subpath from this instance that contains the first <code>count</code> number of path elements from this path.
		/// </summary>
		public PathInformationDto StartPath( int count )
		{
			return SubPath( 0, count );
		}

		/// <summary>
		/// Equality operator. Comparison of path elements is case insensitive.
		/// </summary>
		public static bool operator ==( PathInformationDto p1, PathInformationDto p2 )
		{
			return Equals( p1, p2 );
		}

		/// <summary>
		/// Inequality operator. Comparison of path elements is case insensitive!.
		/// </summary>
		public static bool operator !=( PathInformationDto p1, PathInformationDto p2 )
		{
			return !Equals( p1, p2 );
		}

		/// <inheritdoc />
		public override bool Equals( object obj )
		{
			if( obj is not PathInformationDto other )
				return false;

			if( other._PathElements.Count != _PathElements.Count )
				return false;

			if( GetHashCode() != other.GetHashCode() )
				return false;

			var offset = _PathElements.Offset;
			var otherOffset = other._PathElements.Offset;

			var array = _PathElements.Array;
			var otherArray = other._PathElements.Array;

			for( var i = _PathElements.Count - 1; i >= 0; i-- )
			{
				if( !array[ offset + i ].Equals( otherArray[ otherOffset + i ] ) )
					return false;
			}

			return true;
		}

		/// <inheritdoc />
		public override int GetHashCode()
		{
			if( _HashCode != -1 )
				return _HashCode;

			var hash = new HashCode();
			for( var i = _PathElements.Offset; i < _PathElements.Count; i++ )
			{
				hash.Add( _PathElements.Array[ i ] );
			}
			_HashCode = hash.ToHashCode();

			return _HashCode;
		}

		/// <summary>
		/// Returns a string representation of this PathInformation object to be used for display only.
		/// The path string will contain at least a leading delimiter for the root part.
		/// (Attention: The string cannot be converted back into a PathInformation object!)
		/// </summary>
		/// <inheritdoc />
		public override string ToString()
		{
			return ToString( true );
		}

		/// <summary>
		/// Returns a string representation of this PathInformation object to be used for display only.
		/// It will not contain a leading delimiter for the root part making it look 'relative'.
		/// The result for the root part will be an empty string.
		/// (Attention: The string cannot be converted back into a PathInformation object!)
		/// </summary>
		public string ToStringWithoutRoot()
		{
			return ToString( false );
		}

		/// <summary>
		/// Returns a string representation of this PathInformation object to be used for display only.
		/// The parameter <paramref name="withRoot"/> defines whether
		/// the path string contains the leading delimiter string for the root part.
		/// (Attention: The string cannot be converted back into a PathInformation object!)
		/// </summary>
		private string ToString( bool withRoot )
		{
			if( IsRoot ) return withRoot ? PathHelper.DelimiterString : string.Empty;

			var addDelimiter = withRoot;
			var sb = new StringBuilder();
			foreach( var elem in this )
			{
				if( addDelimiter ) sb.Append( PathHelper.DelimiterString );
				sb.Append( elem.Value );

				addDelimiter = true;
			}

			return sb.ToString();
		}

		#endregion

		#region interface IEnumerable<PathElementDto>

		/// <inheritdoc />
		public IEnumerator<PathElementDto> GetEnumerator()
		{
			return ( (IEnumerable<PathElementDto>)_PathElements ).GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ( (IEnumerable<PathElementDto>)_PathElements ).GetEnumerator();
		}

		#endregion

		#region interface IFormattable

		/// <summary>
		/// Creates a string with the following formatting types:
		/// * "S", "Name": Returns just the name (last path component) of the path.
		/// * "Full": Returns the name and the full path in brackets.
		/// * No or unknown format: Returns the whole path.
		/// </summary>
		/// <inheritdoc />
		public string ToString( string format, IFormatProvider formatProvider )
		{
			switch( format )
			{
				case "S":
				case "Name":
					return Name;
				case "Full":
					return $"{Name} ({ToString()})";
				default:
					return ToString();
			}
		}

		#endregion
	}
}