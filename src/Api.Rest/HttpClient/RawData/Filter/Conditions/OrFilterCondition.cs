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
	using System.Linq;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class OrFilterCondition : FilterCondition
	{
		#region members

		private readonly List<FilterCondition> _ChildConditions;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OrFilterCondition"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="childConditions"/> is <see langword="null" />.</exception>
		public OrFilterCondition( [NotNull] IEnumerable<FilterCondition> childConditions )
		{
			if( childConditions == null )
				throw new ArgumentNullException( nameof( childConditions ) );

			_ChildConditions = new List<FilterCondition>( childConditions );
		}

		#endregion

		#region methods

		/// <inheritdoc />
		public override IFilterTree BuildFilterTree()
		{
			var subTrees = _ChildConditions.Select( condition => condition.BuildFilterTree() );
			return FilterTree.MakeOr( subTrees.ToArray() );
		}

		#endregion
	}
}