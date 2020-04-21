#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Conditions
{
	#region usings

	using System;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class NotFilterCondition : FilterCondition
	{
		#region members

		readonly FilterCondition _ChildCondition;

		#endregion

		#region constructors

		/// <exception cref="ArgumentNullException"><paramref name="childCondition"/> is <see langword="null" />.</exception>
		public NotFilterCondition( [NotNull] FilterCondition childCondition )
		{
			if( childCondition == null )
				throw new ArgumentNullException( nameof( childCondition ) );

			_ChildCondition = childCondition;
		}

		#endregion

		#region methods

		public override IFilterTree BuildFilterTree()
		{
			var subTree = _ChildCondition.BuildFilterTree(); 
			return FilterTree.MakeNot( subTree );
		}

		#endregion
	}
}
