#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Utilities
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using PiWebApi.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Client;
	using Zeiss.IMT.PiWeb.Api.Common.Data;

	#endregion

	/// <summary>
	/// Used to split collection parameters into smaller chunks, to prevent errors caused by long URIs.
	/// </summary>
	internal class ParameterSplitter
	{
		#region members

		private readonly Uri _ServiceLocation;
		private readonly int _MaxUriLength;
		[NotNull] private readonly string _RequestPath;

		#endregion

		#region constructors

		public ParameterSplitter( CommonRestClientBase client, string requestPath )
		{
			if( client == null ) throw new ArgumentNullException( nameof(client) );
			if( string.IsNullOrWhiteSpace( requestPath ) )
				throw new ArgumentException( "Value cannot be null or whitespace.", nameof(requestPath) );

			_RequestPath = requestPath;
			_ServiceLocation = client.ServiceLocation;
			_MaxUriLength = client.MaxUriLength;
		}

		#endregion

		#region methods

		/// <summary>
		/// Split the passed parameter collection into smaller chunks and merge each chunk with the rest of the parameters.
		/// </summary>
		/// <param name="collectionParameter">The collection parameter to split.</param>
		/// <param name="otherParameters">All other parameters that are part of the request, e.g. 'filter'.</param>
		/// <returns>A list of all parameter definitions for each chunk of the collection.</returns>
		public IEnumerable<IEnumerable<ParameterDefinition>> SplitAndMerge<T>(
			CollectionParameterDefinition<T> collectionParameter,
			IEnumerable<ParameterDefinition> otherParameters )
		{
			var otherParametersArray = otherParameters.ToArray();

			var maxLength = DetermineMaxLength( collectionParameter, otherParametersArray );
			var splitParameters = collectionParameter.Split( maxLength );

			var result = MergeWithOtherParameters( splitParameters, otherParametersArray );

			return result;
		}

		/// <summary>
		/// Determine the maximum lenght per value string.
		/// </summary>
		private int DetermineMaxLength<T>( 
			CollectionParameterDefinition<T> collectionParameter,
			IEnumerable<ParameterDefinition> otherParameters )
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
		private static IEnumerable<IEnumerable<ParameterDefinition>> MergeWithOtherParameters(
			IEnumerable<ParameterDefinition> splitParameters,
			IEnumerable<ParameterDefinition> otherParameters )
		{
			var setsOfAllParameters = splitParameters
				.Select( splitParameter => new[] { splitParameter }.Concat( otherParameters ) );

			return setsOfAllParameters;
		}

		#endregion
	}
}