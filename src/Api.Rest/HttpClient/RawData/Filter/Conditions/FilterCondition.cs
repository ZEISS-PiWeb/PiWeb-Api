#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Conditions
{
	#region usings

	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Generators;

	#endregion

	public abstract class FilterCondition : IFilterCondition
	{
		#region members

		public static readonly AndFilterCondition True = new AndFilterCondition( Array.Empty<FilterCondition>() );
		public static readonly OrFilterCondition False = new OrFilterCondition( Array.Empty<FilterCondition>() );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FilterCondition"/> class.
		/// </summary>
		protected FilterCondition()
		{ }

		#endregion

		#region properties

		public static StringConditionGenerator Filename { get; } = new StringConditionGenerator( StringAttributes.Filename );
		public static StringConditionGenerator Md5 { get; } = new StringConditionGenerator( StringAttributes.Md5 );
		public static StringConditionGenerator Mimetype { get; } = new StringConditionGenerator( StringAttributes.Mimetype );

		public static DateTimeConditionGenerator Created { get; } = new DateTimeConditionGenerator( DateTimeAttributes.Created );
		public static DateTimeConditionGenerator LastModified { get; } = new DateTimeConditionGenerator( DateTimeAttributes.LastModified );

		public static IntegerConditionGenerator Length { get; } = new IntegerConditionGenerator( IntegerAttributes.Length );

		#endregion

		#region methods

		/// <exception cref="ArgumentNullException"><paramref name="conditions"/> is <see langword="null" />.</exception>
		public static FilterCondition And( [NotNull] params FilterCondition[] conditions )
		{
			if( conditions == null )
				throw new ArgumentNullException( nameof( conditions ) );

			return new AndFilterCondition( conditions );
		}

		/// <exception cref="ArgumentNullException"><paramref name="conditions"/> is <see langword="null" />.</exception>
		public static FilterCondition And( [NotNull] IEnumerable<FilterCondition> conditions )
		{
			if( conditions == null )
				throw new ArgumentNullException( nameof( conditions ) );

			return new AndFilterCondition( conditions );
		}

		/// <exception cref="ArgumentNullException"><paramref name="conditions"/> is <see langword="null" />.</exception>
		public static FilterCondition Or( [NotNull] params FilterCondition[] conditions )
		{
			if( conditions == null )
				throw new ArgumentNullException( nameof( conditions ) );

			return new OrFilterCondition( conditions );
		}

		/// <exception cref="ArgumentNullException"><paramref name="conditions"/> is <see langword="null" />.</exception>
		public static FilterCondition Or( [NotNull] IEnumerable<FilterCondition> conditions )
		{
			if( conditions == null )
				throw new ArgumentNullException( nameof( conditions ) );

			return new OrFilterCondition( conditions );
		}

		/// <exception cref="ArgumentNullException"><paramref name="condition"/> is <see langword="null" />.</exception>
		public static FilterCondition Not( [NotNull] FilterCondition condition )
		{
			if( condition == null )
				throw new ArgumentNullException( nameof( condition ) );

			return new NotFilterCondition( condition );
		}

		public abstract IFilterTree BuildFilterTree();

		#endregion
	}
}