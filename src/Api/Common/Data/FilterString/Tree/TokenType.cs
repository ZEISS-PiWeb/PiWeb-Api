#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Common.Data.FilterString.Tree
{
	public enum TokenType
	{
		// EOF
		Eof,

		// Texts
		AttributeIdentifier,
		LegacyValue,
		String,

		// Numbers
		Integer,
		Real,

		// Null
		Null,

		// Logic Operators
		And,
		Or,
		Not,
		True,
		False,

		// Comparison Operators
		Equal,
		NotEqual,
		Greater,
		GreaterOrEqual,
		Less,
		LessOrEqual,
		In,
		NotIn,
		Like,

		// Braces
		LPar,
		RPar,
		LBrack,
		RBrack,

		// Separator
		Separator,

		// Pseudo Token -- used in syntax tree nodes but never created by lexer
		LegacyValueList,
		ValueList
	}
}
