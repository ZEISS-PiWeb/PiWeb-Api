#region copyright

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

		/// <summary>
		/// Initializes a new instance of the <see cref="NotFilterCondition"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="childCondition"/> is <see langword="null" />.</exception>
		public NotFilterCondition( [NotNull] FilterCondition childCondition )
		{
			_ChildCondition = childCondition ?? throw new ArgumentNullException( nameof( childCondition ) );
		}

		#endregion

		#region methods

		/// <inheritdoc />
		public override IFilterTree BuildFilterTree()
		{
			var subTree = _ChildCondition.BuildFilterTree();
			return FilterTree.MakeNot( subTree );
		}

		#endregion
	}
}