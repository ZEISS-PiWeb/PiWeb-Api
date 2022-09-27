#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Contracts
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Provides common used extension methods for instances of <see cref="PathInformation"/>.
	/// </summary>
	public static class PathInformationExtension
	{
		#region methods

		/// <summary>
		/// Get parts from given paths
		/// </summary>
		/// <param name="paths">Given paths</param>
		/// <returns>Parts</returns>
		[NotNull]
		public static IReadOnlyList<PathInformation> GetParts( [CanBeNull] this IEnumerable<PathInformation> paths )
		{
			var selectedPaths = paths ?? Enumerable.Empty<PathInformation>();
			return selectedPaths.Where( p => p.Type == InspectionPlanEntityDto.Part ).ToArray();
		}

		/// <summary>
		/// Get characteristics from given paths
		/// </summary>
		/// <param name="paths">Given paths</param>
		/// <returns>Characteristics</returns>
		[NotNull]
		public static IReadOnlyList<PathInformation> GetCharacteristics( [CanBeNull] this IEnumerable<PathInformation> paths )
		{
			var selectedPaths = paths ?? Enumerable.Empty<PathInformation>();
			return selectedPaths.Where( p => p.Type == InspectionPlanEntityDto.Characteristic ).ToArray();
		}

		/// <summary>
		/// Finds the lowest common path all paths are part of.
		/// </summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		[NotNull]
		public static PathInformation FindCommonParent( [CanBeNull] params PathInformation[] paths )
		{
			if( paths == null )
				return PathInformation.Root;

			return FindCommonParentInternal( paths );
		}

		/// <summary>
		/// Finds the lowest common path all paths are part of.
		/// </summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		[NotNull]
		public static PathInformation FindCommonParent( [CanBeNull] IEnumerable<PathInformation> paths )
		{
			return FindCommonParent( paths?.ToArray() );
		}

		[NotNull]
		private static PathInformation FindCommonParentInternal( [NotNull] IReadOnlyList<PathInformation> paths )
		{
			switch( paths.Count )
			{
				case 0:
					return PathInformation.Root;
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

			var currentParent = PathInformation.Root;
			var currentIndex = 0;

			while( true )
			{
				if( paths[ 0 ].Count <= currentIndex )
					return currentParent;

				var currentElement = paths[ 0 ][ currentIndex ];

				for( var i = 1; i < paths.Count; i++ )
				{
					if( paths[ i ].Count <= currentIndex || paths[ i ][ currentIndex ] != currentElement )
						return currentParent;
				}

				currentIndex++;
				currentParent += currentElement;
			}
		}

		/// <summary>
		/// Finds the lowest common part all paths are part of.
		/// </summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		[NotNull]
		public static PathInformation FindCommonParentPart( [CanBeNull] params PathInformation[] paths )
		{
			if( paths == null || paths.Length == 0 )
				return PathInformation.Root;

			var result = FindCommonParentInternal( paths );
			if( !result.IsRoot && result[ result.Count - 1 ].Type != InspectionPlanEntityDto.Part )
			{
				result = result.ParentPartPath;
			}

			return result;
		}

		/// <summary>
		/// Finds the lowest common part all paths are part of.
		/// </summary>
		/// <param name="paths">The paths that should be analyzed.</param>
		[NotNull]
		public static PathInformation FindCommonParentPart( [CanBeNull] IEnumerable<PathInformation> paths )
		{
			return FindCommonParentPart( paths?.ToArray() );
		}

		/// <summary>
		/// Returns a list of the parent parts.
		/// <list type="bullet">
		///		<item>For a characteristic the parent part is returned even if the characteristic has parent characteristics.</item>
		///		<item>For a part the part itself is returned.</item>
		/// </list>
		/// </summary>
		[NotNull]
		public static PathInformation[] FindParentParts( [CanBeNull] IEnumerable<PathInformation> paths )
		{
			if( paths == null )
				return Array.Empty<PathInformation>();

			var parents = new HashSet<PathInformation>();
			foreach( var p in paths )
				parents.Add( p.Type == InspectionPlanEntityDto.Characteristic ? p.ParentPartPath : p );

			return parents.ToArray();
		}

		/// <summary>
		/// Returns <paramref name="path"/> as relative path to <paramref name="basePath"/>.
		/// </summary>
		/// <param name="path">The path that should be relative to <paramref name="basePath"/>.</param>
		/// <param name="basePath">The base path.</param>
		/// <returns>The resulting relative path.</returns>
		[NotNull]
		public static PathInformation RelativeTo( [CanBeNull] this PathInformation path, [CanBeNull] PathInformation basePath )
		{
			return InternalRelativeTo( path, basePath );
		}

		[NotNull]
		private static PathInformation InternalRelativeTo( [CanBeNull] PathInformation path, [CanBeNull] PathInformation basePath )
		{
			if( path == null || path.IsRoot ) return PathInformation.Root;
			if( basePath == null || path.IsRoot ) return path;

			// Count equal path elements
			var common = 0;
			var max = Math.Min( path.Count, basePath.Count );
			while( common < max && path[ common ].Equals( basePath[ common ] ) )
			{
				common++;
			}

			var newPath = new List<PathElement>( path.Count - common );

			// Append the rest of the target path
			newPath.AddRange( path.Skip( common ) );

			return new PathInformation( newPath );
		}

		/// <summary>
		/// Compares two paths by <see cref="StringComparison"/>.
		/// </summary>
		/// <param name="p1">The first comparison path.</param>
		/// <param name="p2">The second comparison path.</param>
		/// <param name="comparison">The type of string comparison to use.</param>
		/// <returns>True if both paths are equals, otherwise false.</returns>
		public static bool Equals( [CanBeNull] this PathInformation p1, [CanBeNull] PathInformation p2, StringComparison comparison )
		{
			if( ReferenceEquals( p1, p2 ) )
				return true;

			if( p1 is null || p2 is null || p1.Count != p2.Count )
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