#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.RawDataService.Filter.Conditions
{
	#region usings

	using Zeiss.PiWeb.Api.Common.Data.FilterString.Tree;

	#endregion

	public class LikeFilterCondition : FilterCondition
	{
		#region members

		private readonly StringAttributes _Attribute;
		private readonly string _Value;

		#endregion

		#region constructors

		public LikeFilterCondition( StringAttributes attribute, string value )
		{
			_Attribute = attribute;
			_Value = value;
		}

		#endregion

		#region methods

		public override IFilterTree BuildFilterTree()
		{
			var attributeName = FilterHelper.GetAttributeName( _Attribute );
			var operatorTokenType = TokenType.Like;

			var valueTree = _Value == null
				? FilterTree.MakeNull()
				: FilterTree.MakeValue( _Value );

			return FilterHelper.MakeComparison( operatorTokenType, attributeName, valueTree );
		}

		#endregion
	}
}
