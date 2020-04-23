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

	public class IntegerListFilterCondition : FilterCondition
	{
		#region members

		private readonly IntegerAttributes _Attribute;
		private readonly ListOperation _Operation;
		private readonly List<int?> _Values;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="IntegerListFilterCondition"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null" />.</exception>
		public IntegerListFilterCondition( IntegerAttributes attribute, ListOperation operation, [NotNull] IEnumerable<int?> values )
		{
			if( values == null )
				throw new ArgumentNullException( nameof( values ) );

			_Attribute = attribute;
			_Operation = operation;
			_Values = new List<int?>( values );
		}

		#endregion

		#region methods

		/// <inheritdoc />
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