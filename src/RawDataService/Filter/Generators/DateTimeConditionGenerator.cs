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
	using PiWebApi.Annotations;
	using Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Conditions;

	#endregion

	public class DateTimeConditionGenerator
	{
		#region members

		private readonly DateTimeAttributes _DateTimeAttribute;

		#endregion

		#region constructors

		public DateTimeConditionGenerator( DateTimeAttributes dateTimeAttribute )
		{
			_DateTimeAttribute = dateTimeAttribute;
		}

		#endregion

		#region methods

		public FilterCondition IsEqual( DateTime? value )
		{
			return new DateTimeCompareFilterCondition( _DateTimeAttribute, CompareOperation.Equal, value );
		}

		public FilterCondition IsNotEqual( DateTime? value )
		{
			return new DateTimeCompareFilterCondition( _DateTimeAttribute, CompareOperation.NotEqual, value );
		}

		public FilterCondition IsGreater( DateTime? value )
		{
			return new DateTimeCompareFilterCondition( _DateTimeAttribute, CompareOperation.Greater, value );
		}

		public FilterCondition IsGreaterOrEqual( DateTime? value )
		{
			return new DateTimeCompareFilterCondition( _DateTimeAttribute, CompareOperation.GreaterOrEqual, value );
		}

		public FilterCondition IsLess( DateTime? value )
		{
			return new DateTimeCompareFilterCondition( _DateTimeAttribute, CompareOperation.Less, value );
		}

		public FilterCondition IsLessOrEqual( DateTime? value )
		{
			return new DateTimeCompareFilterCondition( _DateTimeAttribute, CompareOperation.LessOrEqual, value );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsIn( [NotNull] params DateTime?[] values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new DateTimeListFilterCondition( _DateTimeAttribute, ListOperation.In, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsIn( [NotNull] IEnumerable<DateTime?> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new DateTimeListFilterCondition( _DateTimeAttribute, ListOperation.In, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsNotIn( [NotNull] params DateTime?[] values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new DateTimeListFilterCondition( _DateTimeAttribute, ListOperation.NotIn, values );
		}

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public FilterCondition IsNotIn( [NotNull] IEnumerable<DateTime?> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			return new DateTimeListFilterCondition( _DateTimeAttribute, ListOperation.NotIn, values );
		}

		#endregion
	}
}
