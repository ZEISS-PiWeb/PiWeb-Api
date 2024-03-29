﻿#region copyright

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
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// Represents a HTTP GET parameter that has a collection of values.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	internal class CollectionParameterDefinition<T>
	{
		#region constants

		private const string EmptyCollectionString = RestClientHelper.QueryListStart + RestClientHelper.QueryListStop;

		#endregion

		#region members

		private readonly Func<IEnumerable<T>, string> _ToStringConverter;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CollectionParameterDefinition{T}"/> class.
		/// </summary>
		/// <param name="name">Name of the collection parameter, e.g. 'uuids'</param>
		/// <param name="values">The separate values as string.</param>
		/// <param name="toStringConverter">A function to convert the parameter list to a string</param>
		internal CollectionParameterDefinition( string name, IEnumerable<T> values, Func<IEnumerable<T>, string> toStringConverter )
		{
			Name = name;
			Values = values;
			_ToStringConverter = toStringConverter;

			Empty = ParameterDefinition.Create( Name, EmptyCollectionString );
		}

		#endregion

		#region properties

		public ParameterDefinition Empty { get; }

		public string Name { get; }

		public IEnumerable<T> Values { get; }

		#endregion

		#region methods

		/// <summary>
		/// Split the values into smaller chunks, e.g. to prevent URIs that are too long.
		/// </summary>
		/// <param name="maxLength">The maximum length of the resulting value string.</param>
		/// <returns>
		/// A collection of parameter definitions that all have the same name, but whose
		/// value is always shorter than <paramref name="maxLength"/>.
		/// </returns>
		public IEnumerable<ParameterDefinition> Split( int maxLength )
		{
			return ArrayHelper
				.Split( Values, maxLength, RestClientHelper.LengthOfListElementInUri )
				.Select( chunk => ParameterDefinition.Create( Name, _ToStringConverter( chunk ) ) );
		}

		public ParameterDefinition ToParameterDefinition()
		{
			return ParameterDefinition.Create( Name, _ToStringConverter( Values ) );
		}

		#endregion
	}
}