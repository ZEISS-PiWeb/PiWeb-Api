#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Helper class that splits an array into smaller arrays with a fixed segment size.
	/// </summary>
	public static class ArrayHelper
	{
		#region methods

		/// <summary>
		/// This method splits the source enumeration into multiple smaller arrays with size <paramref name="targetSize"/>.
		/// The default weighting of every item is 1. A specific weighting can be calculated for every item with function <paramref name="calcWeightingOfOneItem"/>.
		/// </summary>
		public static IEnumerable<T[]> Split<T>( [CanBeNull] IEnumerable<T> items, int targetSize, Func<T, int> calcWeightingOfOneItem = null )
		{
			if( items == null ) yield break;

			var count = 0;
			var bulk = new List<T>();

			if( calcWeightingOfOneItem == null )
			{
				var blockSize = targetSize;

				if( items is ICollection<T> genericCollection )
					blockSize = Math.Min( targetSize, genericCollection.Count );
				else if( items is ICollection nonGenericCollection )
					blockSize = Math.Min( targetSize, nonGenericCollection.Count );

				bulk.Capacity = blockSize;
			}

			foreach( var item in items )
			{
				var size = calcWeightingOfOneItem?.Invoke( item ) ?? 1;

				if( count + size > targetSize )
				{
					if( bulk.Count > 0 )
					{
						yield return bulk.ToArray();
					}

					bulk.Clear();
					count = 0;
				}

				bulk.Add( item );
				count += size;
			}

			if( bulk.Count > 0 ) yield return bulk.ToArray();
		}

		#endregion
	}
}