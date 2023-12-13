#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// Used to split collection parameters into smaller chunks, to prevent errors caused by long URIs.
	/// </summary>
	internal class ParameterSplitter
	{
		#region members

		private readonly Uri _ServiceLocation;
		private readonly int _MaxUriLength;
		private readonly string _RequestPath;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ParameterSplitter"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="serviceLocation"/> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentException"><paramref name="requestPath"/> is <see langword="null" /> or whitespace.</exception>
		public ParameterSplitter( Uri serviceLocation, int maxUriLength, string requestPath )
		{
			if( serviceLocation == null ) throw new ArgumentNullException( nameof( serviceLocation ) );
			if( string.IsNullOrWhiteSpace( requestPath ) )
				throw new ArgumentException( "Value cannot be null or whitespace.", nameof( requestPath ) );

			_RequestPath = requestPath;
			_ServiceLocation = serviceLocation;
			_MaxUriLength = maxUriLength;
		}

		#endregion

		#region methods

		/// <summary>
		/// Split the passed parameter collection into smaller chunks and merge each chunk with the rest of the parameters.
		/// </summary>
		/// <param name="collectionParameter">The collection parameter to split.</param>
		/// <param name="otherParameters">All other parameters that are part of the request, e.g. 'filter'.</param>
		/// <returns>A list of all parameter definitions for each chunk of the collection.</returns>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="collectionParameter"/> or <paramref name="otherParameters"/> is <see langword="null" />.
		/// </exception>
		public IEnumerable<IReadOnlyCollection<ParameterDefinition>> SplitAndMerge<T>(
			[NotNull] CollectionParameterDefinition<T> collectionParameter,
			[NotNull] IEnumerable<ParameterDefinition> otherParameters )
		{
			if( collectionParameter == null ) throw new ArgumentNullException( nameof( collectionParameter ) );
			if( otherParameters == null ) throw new ArgumentNullException( nameof( otherParameters ) );

			var otherParametersArray = otherParameters.ToArray();

			var maxLength = DetermineMaxLength( collectionParameter, otherParametersArray );
			var splitParameters = collectionParameter.Split( maxLength );

			return MergeWithOtherParameters( splitParameters, otherParametersArray );
		}

		/// <summary>
		/// Determine the maximum lenght per value string.
		/// </summary>
		private int DetermineMaxLength<T>( CollectionParameterDefinition<T> collectionParameter, IEnumerable<ParameterDefinition> otherParameters )
		{
			// Add the empty collection to the parameter collection
			// to also count the characters of the parameter name and the collection delimiters.
			var emptyCollection = collectionParameter.Empty;

			// Determinde maximum length per value string
			var maxLength = RestClientHelper.GetUriTargetSize(
				_ServiceLocation,
				_RequestPath,
				_MaxUriLength,
				new[] { emptyCollection }.Concat( otherParameters ).ToArray() );

			return maxLength;
		}

		/// <summary>
		/// Merge split parameters with the other parameters.
		/// </summary>
		/// <returns>The complete set of parameters for each of the <paramref name="splitParameters"/></returns>
		private static IEnumerable<IReadOnlyCollection<ParameterDefinition>> MergeWithOtherParameters(
			IEnumerable<ParameterDefinition> splitParameters,
			IEnumerable<ParameterDefinition> otherParameters )
		{
			return splitParameters.Select( splitParameter => new[] { splitParameter }.Concat( otherParameters ).ToArray() );
		}

		#endregion
	}
}