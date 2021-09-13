#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Provides common used extension methods for instances of <see cref="PathInformationDto"/>.
	/// </summary>
	public static class PathInformationExtension
	{
		#region methods

		/// <summary>
		/// Get parts from given paths
		/// </summary>
		/// <param name="paths">Given paths</param>
		/// <returns>Parts</returns>
		public static PathInformationDto[] GetParts( this IEnumerable<PathInformationDto> paths )
		{
			var selectedPaths = paths ?? Enumerable.Empty<PathInformationDto>();
			return selectedPaths.Where( p => p.Type == InspectionPlanEntityDto.Part ).ToArray();
		}

		/// <summary>
		/// Get characteristics from given paths
		/// </summary>
		/// <param name="paths">Given paths</param>
		/// <returns>Characteristics</returns>
		public static PathInformationDto[] GetCharacteristics( this IEnumerable<PathInformationDto> paths )
		{
			var selectedPaths = paths ?? Enumerable.Empty<PathInformationDto>();
			return selectedPaths.Where( p => p.Type == InspectionPlanEntityDto.Characteristic ).ToArray();
		}

		/// <summary>
		/// Finds the lowest common path all paths are part of.
		/// </summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		public static PathInformationDto FindCommonParent( params PathInformationDto[] paths )
		{
			if( paths == null )
				return PathInformationDto.Root;

			return FindCommonParentInternal( paths );
		}

		/// <summary>
		/// Finds the lowest common path all paths are part of.
		/// </summary>
		/// <param name="pathList">The paths that should be analyzed.</param>
		[NotNull]
		public static PathInformationDto FindCommonParent( [CanBeNull] IEnumerable<PathInformationDto> pathList )
		{
			if( pathList == null )
				return PathInformationDto.Root;

			var paths = pathList.ToArray();
			return FindCommonParentInternal( paths );
		}

		private static PathInformationDto FindCommonParentInternal( [NotNull] PathInformationDto[] paths )
		{
			switch( paths.Length )
			{
				case 0:
					return PathInformationDto.Root;
				case 1:
					return paths[ 0 ];
				case 2:
				{
					var p1 = paths[ 0 ];
					var p2 = paths[ 1 ];
					var count = 0;
					while( count < p1.Count && count < p2.Count && p1[ count ] == p2[ count ] )
					{
						count++;
					}

					return p1.StartPath( count );
				}
			}

			var currentParent = PathInformationDto.Root;
			var currentIndex = 0;

			while( true )
			{
				if( paths[ 0 ].Count <= currentIndex )
					return currentParent;

				var currentElement = paths[ 0 ][ currentIndex ];

				for( var i = 1; i < paths.Length; i++ )
				{
					if( paths[ i ].Count <= currentIndex || paths[ i ][ currentIndex ] != currentElement )
						return currentParent;
				}

				currentIndex++;
				currentParent += currentElement;
			}
		}

		/// <summary> Finds the lowest common part all paths are part of.</summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		public static PathInformationDto FindCommonParentPart( params PathInformationDto[] paths )
		{
			if( paths == null || paths.Length == 0 )
				return PathInformationDto.Root;

			var result = FindCommonParentInternal( paths );
			if( !result.IsRoot && result[ result.Count - 1 ].Type != InspectionPlanEntityDto.Part )
			{
				result = result.ParentPartPath;
			}

			return result;
		}

		/// <summary> Finds the lowest common part all paths are part of.</summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		public static PathInformationDto FindCommonParentPart( IEnumerable<PathInformationDto> paths )
		{
			var result = FindCommonParent( paths );
			if( !result.IsRoot && result[ result.Count - 1 ].Type != InspectionPlanEntityDto.Part )
			{
				result = result.ParentPartPath;
			}

			return result;
		}

		/// <summary>Returns a list of the superordinated parts. For a characteristic the superordinated part is returned even if the
		/// characteristic has superordinated characteristics. For a part the part itself is returned. </summary>
		public static PathInformationDto[] FindParentParts( IEnumerable<PathInformationDto> paths )
		{
			var parents = new HashSet<PathInformationDto>();

			if( paths == null )
				return parents.ToArray();

			foreach( var p in paths )
			{
				parents.Add( p.Type == InspectionPlanEntityDto.Characteristic ? p.ParentPartPath : p );
			}

			return parents.ToArray();
		}

		/// <summary>
		/// Returns <paramref name="path"/> as relative path to <paramref name="basePath"/>.
		/// </summary>
		/// <param name="path">The path that should be relative to <paramref name="basePath"/>.</param>
		/// <param name="basePath">The base path.</param>
		/// <returns>The resulting relative path.</returns>
		public static PathInformationDto RelativeTo( this PathInformationDto path, PathInformationDto basePath )
		{
			return InternalRelativeTo( path, basePath );
		}

		private static PathInformationDto InternalRelativeTo( PathInformationDto path, PathInformationDto basePath )
		{
			if( path == null || path.IsRoot ) return PathInformationDto.Root;
			if( basePath == null || path.IsRoot ) return path;

			// Count equal path elements
			var common = 0;
			var max = Math.Min( path.Count, basePath.Count );
			while( common < max && path[ common ].Equals( basePath[ common ] ) )
			{
				common++;
			}

			var newPath = new List<PathElementDto>( path.Count - common );

			// Append the rest of the target path
			newPath.AddRange( path.Skip( common ) );

			return new PathInformationDto( newPath );
		}

		/// <summary>
		/// Compares two paths by <see cref="StringComparison"/>.
		/// </summary>
		/// <param name="p1">The first comparison path.</param>
		/// <param name="p2">The second comparison path.</param>
		/// <param name="comparison">The type of string comparison to use.</param>
		/// <returns>True if both paths are equals, otherwise false.</returns>
		public static bool Equals( this PathInformationDto p1, PathInformationDto p2, StringComparison comparison )
		{
			if( ReferenceEquals( p1, null ) && ReferenceEquals( p2, null ) )
				return true;

			if( ReferenceEquals( p1, null ) || p1.Count != p2.Count )
				return false;

			for( var i = p1.Count - 1; i >= 0; i-- )
			{
				if( !p1[ i ].Value.Equals( p2[ i ].Value, comparison ) || !p1[ i ].Type.Equals( p2[ i ].Type ) )
					return false;
			}

			return true;
		}

		#endregion
	}
}