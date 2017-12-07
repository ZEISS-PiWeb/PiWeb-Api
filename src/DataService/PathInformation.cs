#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Every inspection plan entity is uniquely identified by a path. The path contains a typed list of path elements (<see cref="PathElement"/>).
	/// This list of path elements describes a hierarchy with the top most path element as the first element in that list and the deepest path element
	/// as the last element in that list. Please note that a path has the following rules:
	/// * The path elements are compared ignoring the case
	/// * A part path element can have part path and characteristic path elements as children
	/// * A characteristic path element can have other characteristic elements as children
	/// * A characteristic path element can not have part elements as children
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.PathInformationConverter ) )]
	public sealed class PathInformation : IFormattable, IEnumerable<PathElement>
	{
		#region constants

		/// <summary> Returns the root path. </summary>
		public static readonly PathInformation Root = new PathInformation();

		#endregion

		#region members

		private ArraySegment<PathElement> _PathElements;
		private int? _HashCode;

		#endregion

		#region constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public PathInformation()
		{
			_PathElements = new ArraySegment<PathElement>( new PathElement[ 0 ] );
		}

		/// <summary>Constructor. Initialies the path with the given <paramref name="paths"/>.</summary>
		public PathInformation( IEnumerable<PathElement> paths )
		{
			_PathElements = new ArraySegment<PathElement>( paths.ToArray() );
		}

		/// <summary>Constructor. Initialies the path with the given <paramref name="paths"/>.</summary>
		public PathInformation( params PathElement[] paths )
		{
			_PathElements = new ArraySegment<PathElement>( paths );
		}

		/// <summary>Constructor. Initialies the path as a view on the given <paramref name="path"/> with an offset and a length.</summary>
		public PathInformation( PathInformation path, int offset, int length = -1 )
		{
			var currentPathElements = path._PathElements;

			if( offset < 0 || offset > currentPathElements.Count )
				throw new ArgumentException( "Parameter offset must be greater or equal 0 and less than or equal to the path element count of path." );
			
			if( length > currentPathElements.Count - offset )
				throw new ArgumentException( "Parameter length cannot be greater than the path element count of path minus offset." );

			if( length < 0 )
				length = currentPathElements.Count - offset;
			
			_PathElements = new ArraySegment<PathElement>( currentPathElements.Array, currentPathElements.Offset + offset, length );
		}

		private PathInformation( ArraySegment<PathElement> path )
		{
			_PathElements = path;
		}

		#endregion

		#region properties and indexer

		/// <summary>
		/// Indexer to access the path elements by its index.
		/// </summary>
		public PathElement this[ int index ]
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
		public PathInformation ParentPath
		{
			get { return Parent( 1 ); }
		}

		/// <summary>
		/// Returns the parent part path.
		/// </summary>
		public PathInformation ParentPartPath
		{
			get
			{
				var upcount = 1;
				while( upcount < Count && this[ Count - 1 - upcount ].Type == InspectionPlanEntity.Characteristic )
				{
					upcount += 1;
				}
				return Parent( upcount );
			}
		}

		/// <summary> 
		/// Returns the root part of ths path.
		/// </summary>
		public PathInformation RootPartPath
		{
			get { return IsRoot ? Root : Parent( Count - 1 ); }
		}

		/// <summary> 
		/// Returns true if this path is the root part. 
		/// </summary>
		public bool IsRoot
		{
			get { return _PathElements.Count == 0; }
		}

		/// <summary> 
		/// Returns the number of path elements. 
		/// </summary>
		public int Count
		{
			get { return _PathElements.Count; }
		}

		/// <summary> 
		/// Returns the name part of the path. 
		/// </summary>
		public string Name
		{
			get { return _PathElements.Count == 0 ? "" : _PathElements.Array[ _PathElements.Offset + _PathElements.Count - 1 ].Value; }
		}

		/// <summary> 
		/// Returns the typed name of the path - a path element. As the root part does not have a name <code>null</code> is returned. 
		/// </summary>
		public PathElement TypedName
		{
			get { return _PathElements.Count == 0 ? PathElement.EmptyPart : this[ Count - 1 ]; }
		}

		/// <summary> 
		/// Returns the type of the last path element. 
		/// </summary>
		public InspectionPlanEntity Type
		{
			get { return _PathElements.Count == 0 ? InspectionPlanEntity.Part : this[ Count - 1 ].Type; }
		}

		#endregion

		#region methods

		/// <summary> Checks if the <paramref name="path"/> is <code>null</code> or the root part. </summary>
		/// <returns>True, if the <paramref name="path"/> is <code>null</code> or the root part otherwise false.</returns>
		public static bool IsNullOrRoot( PathInformation path )
		{
			return ( path == null || path.IsRoot );
		}

		/// <summary> Checks if this path is below <paramref name="path"/> or is equal to <paramref name="path"/>.
		/// Also checks the path elements type!
		/// </summary>
		public bool IsBelow( PathInformation path )
		{
			if( path == null || path.Count > Count ) 
				return false;

			return !path.Where( ( t, i ) => !this[ i ].Equals( t ) ).Any();
		}

		/// <summary> 
		/// Returns the parent path by stepping up the <code>upcount</code> number of levels in the path hierarchy.
		/// </summary>
		/// <param name="upcount">Number of levels to go upwards the path hierarchy (&gt;= 0).</param>
		/// <returns>The path without the last n levels, the path itself or the root path.</returns>
		public PathInformation Parent( int upcount )
		{
			if( upcount < 0 )
				throw new ArgumentOutOfRangeException( "upcount" );

			if( upcount == 0 )
				return this;
	
			if( upcount >= Count )
				return Root;

			return new PathInformation( new ArraySegment<PathElement>( _PathElements.Array, _PathElements.Offset, Count - upcount ) );
		}

		/// <summary>
		/// Add operator. Combines the path <paramref name="p1"/> and the path <paramref name="p2"/> to a new <see cref="PathInformation"/> instance.
		/// </summary>
		public static PathInformation operator +( PathInformation p1, PathInformation p2 )
		{
			return Combine( p1, p2 );
		}

		/// <summary> 
		/// Combines the path <paramref name="p1"/> and the path <paramref name="p2"/> to a new <see cref="PathInformation"/> instance. 
		/// </summary>
		public static PathInformation Combine( PathInformation p1, PathInformation p2 )
		{
			if( p1 == null || p1.IsRoot )
				return p2;

			if( p2 == null || p2.IsRoot )
				return p1;

			var newpath = new List<PathElement>( p1.Count + p2.Count );
			newpath.AddRange( p1 );
			newpath.AddRange( p2 );

			return new PathInformation( newpath );
		}

		/// <summary>
		/// Combines the path <paramref name="path"/> and the path element <paramref name="elem"/> to a new <see cref="PathInformation"/> instance. 
		/// </summary>
		public static PathInformation Combine( PathInformation path, PathElement elem )
		{
			if( elem == null || elem.IsEmpty ) 
				return path;

			if( path == null )
				return new PathInformation( elem );

			var newpath = new List<PathElement>( path.Count + 1 );
			newpath.AddRange( path ); 
			newpath.Add( elem );

			return new PathInformation( newpath );
		}

		/// <summary> 
		/// Returns the a path that contains the first <code>count</code> number of path elements from this path. 
		/// </summary>
		public PathInformation StartPath( int count )
		{
			if( count > _PathElements.Count )
				throw new ArgumentOutOfRangeException( "count" );

			return new PathInformation( new ArraySegment<PathElement>( _PathElements.Array, _PathElements.Offset, count ) );
		}

		/// <summary> 
		/// Equality operator. Comparison of path elements is case insensitive.
		/// </summary>
		public static bool operator ==( PathInformation p1, PathInformation p2 )
		{
			return Equals( p1, p2 );
		}

		/// <summary> 
		/// Inequality operator. Comparison of path elements is case insensitive!.
		/// </summary>
		public static bool operator !=( PathInformation p1, PathInformation p2 )
		{
			return !Equals( p1, p2 );
		}

		/// <summary> 
		/// Overridden <see cref="System.Object.Equals(object)"/> method. 
		/// </summary>
		public override bool Equals( object obj )
		{
			var other = obj as PathInformation;
			if( ReferenceEquals( other, null ) || other._PathElements.Count != _PathElements.Count )
				return false;

			for( var i = _PathElements.Count - 1; i >= 0; i-- )
			{
				if( !_PathElements.Array[ _PathElements.Offset + i ].Equals( other._PathElements.Array[ other._PathElements.Offset + i ] ) )
					return false;
			}
			return true;
		}

		/// <summary> 
		/// Overridden <see cref="System.Object.GetHashCode"/> method. 
		/// </summary>
		public override int GetHashCode()
		{
			if( !_HashCode.HasValue )
			{
				var hash = 1;
				foreach( var elem in this )
				{
					hash ^= elem.GetHashCode();
				}
				_HashCode = hash;
			}
			return _HashCode.Value;
		}

		/// <summary> 
		/// Overridden <see cref="System.Object.ToString"/> method. 
		/// </summary>
		public override string ToString()
		{
			var sb = new System.Text.StringBuilder();
			foreach( var elem in this )
			{
				sb.Append( "/" ).Append( elem.Value );
			}
			return sb.ToString();
		}

		#endregion

		#region interface IEnumerable

		/// <summary>
		/// Returns an enumerator which contains all <see cref="PathElement"/> objects which this <see cref="PathInformation"/> consists of. 
		/// </summary>
		public IEnumerator<PathElement> GetEnumerator()
		{
			return ( (IEnumerable<PathElement>)_PathElements ).GetEnumerator();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return ( (IEnumerable<PathElement>)_PathElements ).GetEnumerator();
		}

		#endregion

		#region interface IFormattable

		/// <summary>
		/// Creates a string with the following formatting types:
		/// * "S", "Name": Returns just the name (last path component) of the path.
		/// * "Full": Returns the name and the full path in brackets.
		/// * No or unknown format: Returns the whole path.
		/// </summary>
		public string ToString( string format, IFormatProvider formatProvider )
		{
			switch( format )
			{
				case "S":
				case "Name":
					return Name;
				case "Full":
					return string.Format( "{0} ({1})", Name, ToString() );
				default:
					return ToString();
			}
		}

		#endregion
	}
}