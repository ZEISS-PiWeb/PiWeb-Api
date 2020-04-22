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

	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class IntegerCompareFilterCondition : FilterCondition
	{
		#region members

		private readonly IntegerAttributes _Attribute;
		private readonly CompareOperation _Operation;
		private readonly int? _Value;

		#endregion

		#region constructors

		public IntegerCompareFilterCondition( IntegerAttributes attribute, CompareOperation operation, int? value )
		{
			_Attribute = attribute;
			_Operation = operation;
			_Value = value;
		}

		#endregion

		#region methods

		public override IFilterTree BuildFilterTree()
		{
			var attributeName = FilterHelper.GetAttributeName( _Attribute );
			var operatorTokenType = FilterHelper.GetOperatorTokenType( _Operation );

			var valueTree = FilterTree.MakeValue( _Value );
			return FilterHelper.MakeComparison( operatorTokenType, attributeName, valueTree );
		}

		#endregion
	}
}
