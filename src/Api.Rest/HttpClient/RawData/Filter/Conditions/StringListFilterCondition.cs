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
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class StringListFilterCondition : FilterCondition
	{
		#region members

		private readonly StringAttributes _Attribute;
		private readonly ListOperation _Operation;
		private readonly List<string> _Values;

		#endregion

		#region constructors

		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public StringListFilterCondition( StringAttributes attribute, ListOperation operation, [NotNull] IEnumerable<string> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			_Attribute = attribute;
			_Operation = operation;
			_Values = new List<string>( values );
		}

		#endregion

		#region methods

		public override IFilterTree BuildFilterTree()
		{
			var attributeName = FilterHelper.GetAttributeName( _Attribute );
			var operatorTokenType = FilterHelper.GetOperatorTokenType( _Operation );

			var valueTree = FilterTree.MakeValueList( _Values.ToArray() );
			return FilterHelper.MakeComparison( operatorTokenType, attributeName, valueTree );
		}

		#endregion
	}
}
