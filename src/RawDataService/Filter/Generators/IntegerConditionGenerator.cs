#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Generators
{
	#region usings

	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Conditions;

	#endregion

	public class IntegerConditionGenerator
	{
		#region members

		private readonly IntegerAttributes _IntegerAttribute;

		#endregion

		#region constructors

		public IntegerConditionGenerator( IntegerAttributes integerAttribute )
		{
			_IntegerAttribute = integerAttribute;
		}

		#endregion

		#region methods

		public FilterCondition IsEqual( int? value )
		{
			return new IntegerCompareFilterCondition( _IntegerAttribute, CompareOperation.Equal, value );
		}

		public FilterCondition IsNotEqual( int? value )
		{
			return new IntegerCompareFilterCondition( _IntegerAttribute, CompareOperation.NotEqual, value );
		}

		public FilterCondition IsGreater( int? value )
		{
			return new IntegerCompareFilterCondition( _IntegerAttribute, CompareOperation.Greater, value );
		}

		public FilterCondition IsGreaterOrEqual( int? value )
		{
			return new IntegerCompareFilterCondition( _IntegerAttribute, CompareOperation.GreaterOrEqual, value );
		}

		public FilterCondition IsLess( int? value )
		{
			return new IntegerCompareFilterCondition( _IntegerAttribute, CompareOperation.Less, value );
		}

		public FilterCondition IsLessOrEqual( int? value )
		{
			return new IntegerCompareFilterCondition( _IntegerAttribute, CompareOperation.LessOrEqual, value );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsIn( [NotNull] params int?[] values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new IntegerListFilterCondition( _IntegerAttribute, ListOperation.In, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsIn( [NotNull] IEnumerable<int?> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new IntegerListFilterCondition( _IntegerAttribute, ListOperation.In, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsNotIn( [NotNull] params int?[] values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new IntegerListFilterCondition( _IntegerAttribute, ListOperation.NotIn, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsNotIn( [NotNull] IEnumerable<int?> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new IntegerListFilterCondition( _IntegerAttribute, ListOperation.NotIn, values );
		}

		#endregion
	}
}
