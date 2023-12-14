#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Interface for a cache that defines a method for clearing a cache entry for a specific <see cref="Uri" />.
	/// </summary>
	public interface ICacheClearable
	{
		#region methods

		/// <summary>
		/// Clears a cache entry for the given <see cref="Uri" />.
		/// </summary>
		/// <param name="uri">The address for which a cache entry should be cleared.</param>
		void InvalidateCache( [NotNull] Uri uri );

		#endregion
	}
}