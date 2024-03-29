﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Generators
{
	#region usings

	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Conditions;

	#endregion

	public class StringConditionGenerator
	{
		#region members

		private readonly StringAttributes _StringAttribute;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="StringConditionGenerator"/> class.
		/// </summary>
		public StringConditionGenerator( StringAttributes stringAttribute )
		{
			_StringAttribute = stringAttribute;
		}

		#endregion

		#region methods

		public FilterCondition IsEqual( string value )
		{
			return new StringCompareFilterCondition( _StringAttribute, CompareOperation.Equal, value );
		}

		public FilterCondition IsNotEqual( string value )
		{
			return new StringCompareFilterCondition( _StringAttribute, CompareOperation.NotEqual, value );
		}

		public FilterCondition IsGreater( string value )
		{
			return new StringCompareFilterCondition( _StringAttribute, CompareOperation.Greater, value );
		}

		public FilterCondition IsGreaterOrEqual( string value )
		{
			return new StringCompareFilterCondition( _StringAttribute, CompareOperation.GreaterOrEqual, value );
		}

		public FilterCondition IsLess( string value )
		{
			return new StringCompareFilterCondition( _StringAttribute, CompareOperation.Less, value );
		}

		public FilterCondition IsLessOrEqual( string value )
		{
			return new StringCompareFilterCondition( _StringAttribute, CompareOperation.LessOrEqual, value );
		}

		public FilterCondition IsLike( string value )
		{
			return new LikeFilterCondition( _StringAttribute, value );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsIn( [NotNull] params string[] values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new StringListFilterCondition( _StringAttribute, ListOperation.In, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsIn( [NotNull] IEnumerable<string> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new StringListFilterCondition( _StringAttribute, ListOperation.In, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsNotIn( [NotNull] params string[] values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new StringListFilterCondition( _StringAttribute, ListOperation.NotIn, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsNotIn( [NotNull] IEnumerable<string> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new StringListFilterCondition( _StringAttribute, ListOperation.NotIn, values );
		}

		#endregion
	}
}