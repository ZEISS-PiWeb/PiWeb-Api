#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Conditions
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using PiWebApi.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree;

	#endregion

	public class AndFilterCondition : FilterCondition
	{
		#region members

		readonly List<FilterCondition> _ChildConditions;

		#endregion

		#region constructors

		/// <exception cref="ArgumentNullException"><paramref name="childConditions"/> is <see langword="null" />.</exception>
		public AndFilterCondition( [NotNull] IEnumerable<FilterCondition> childConditions )
		{
			if( childConditions == null )
				throw new ArgumentNullException( nameof( childConditions ) );

			_ChildConditions = new List<FilterCondition>( childConditions );
		}

		#endregion

		#region methods

		public override IFilterTree BuildFilterTree()
		{
			var subTrees = _ChildConditions.Select( condition => condition.BuildFilterTree() );
			return FilterTree.MakeAnd( subTrees.ToArray() );
		}

		#endregion
	}
}
