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

	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class LikeFilterCondition : FilterCondition
	{
		#region members

		private readonly StringAttributes _Attribute;
		private readonly string _Value;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LikeFilterCondition"/> class.
		/// </summary>
		public LikeFilterCondition( StringAttributes attribute, string value )
		{
			_Attribute = attribute;
			_Value = value;
		}

		#endregion

		#region methods

		/// <inheritdoc />
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