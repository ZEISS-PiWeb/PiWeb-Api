#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree
{
	#region usings

	using System.Collections.Generic;

	#endregion

	public static class TokenMappings
	{
		#region constants

		public const string OpEqual = "=";
		public const string OpNotEqual = "<>";
		public const string OpGreater = ">";
		public const string OpGreaterOrEqual = ">=";
		public const string OpLessOrEqual = "<=";
		public const string OpLess = "<";

		public const string Equal = "eq";
		public const string NotEqual = "ne";
		public const string Greater = "gt";
		public const string GreaterOrEqual = "ge";
		public const string LessOrEqual = "le";
		public const string Less = "lt";
		public const string In = "in";
		public const string NotIn = "notIn";
		public const string Like = "like";

		public const string Or = "or";
		public const string And = "and";
		public const string LegacyAnd = "+";
		public const string Not = "not";
		public const string True = "true";
		public const string False = "false";
		public const string Null = "null";

		public const string LPar = "(";
		public const string RPar = ")";
		public const string LBrack = "[";
		public const string RBrack = "]";
		public const string Separator = ",";

		#endregion

		#region members

		public static Dictionary<string, TokenType> KeywordToTokenTypeMap = new Dictionary<string, TokenType>
		{
			{ Equal, TokenType.Equal },
			{ NotEqual, TokenType.NotEqual },
			{ Greater, TokenType.Greater },
			{ GreaterOrEqual, TokenType.GreaterOrEqual },
			{ LessOrEqual, TokenType.LessOrEqual },
			{ Less, TokenType.Less },
			{ In, TokenType.In },
			{ NotIn, TokenType.NotIn },
			{ Like, TokenType.Like },
			{ Or, TokenType.Or },
			{ And, TokenType.And },
			{ Not, TokenType.Not },
			{ True, TokenType.True },
			{ False, TokenType.False },
			{ Null, TokenType.Null }
		};

		public static Dictionary<string, TokenType> OperatorToTokenTypeMap = new Dictionary<string, TokenType>
		{
			{ OpGreaterOrEqual, TokenType.GreaterOrEqual },
			{ OpLessOrEqual, TokenType.LessOrEqual },
			{ OpNotEqual, TokenType.NotEqual },

			{ OpEqual, TokenType.Equal },
			{ OpGreater, TokenType.Greater },
			{ OpLess, TokenType.Less },
			{ LPar, TokenType.LPar },
			{ RPar, TokenType.RPar },
			{ LBrack, TokenType.LBrack },
			{ RBrack, TokenType.RBrack },
			{ Separator, TokenType.Separator },
			{ LegacyAnd, TokenType.And }
		};

		public static Dictionary<TokenType, string> TokenTypeToDefaultValueMap = new Dictionary<TokenType, string>
		{
			{ TokenType.Equal, Equal },
			{ TokenType.NotEqual, NotEqual },
			{ TokenType.Greater, Greater },
			{ TokenType.GreaterOrEqual, GreaterOrEqual },
			{ TokenType.Less, Less },
			{ TokenType.LessOrEqual, LessOrEqual },
			{ TokenType.In, In },
			{ TokenType.NotIn, NotIn },
			{ TokenType.Like, Like },
			{ TokenType.Or, Or },
			{ TokenType.And, And },
			{ TokenType.Not, Not },
			{ TokenType.True, True },
			{ TokenType.False, False }
		};

		public static Dictionary<TokenType, string> TokenTypeToLegacyValueMap = new Dictionary<TokenType, string>
		{
			{ TokenType.Equal, OpEqual },
			{ TokenType.NotEqual, OpNotEqual },
			{ TokenType.Greater, OpGreater },
			{ TokenType.GreaterOrEqual, OpGreaterOrEqual },
			{ TokenType.Less, OpLess },
			{ TokenType.LessOrEqual, OpLessOrEqual },
			{ TokenType.In, In },
			{ TokenType.NotIn, NotIn },
			{ TokenType.Like, Like },
			{ TokenType.And, LegacyAnd }
		};

		public static HashSet<TokenType> SimpleOperators = new HashSet<TokenType>
		{
			TokenType.Equal,
			TokenType.NotEqual,
			TokenType.Greater,
			TokenType.GreaterOrEqual,
			TokenType.Less,
			TokenType.LessOrEqual,
			TokenType.Like
		};

		public static HashSet<TokenType> ListOperators = new HashSet<TokenType>
		{
			TokenType.In,
			TokenType.NotIn
		};

		public static HashSet<TokenType> ComparisonOperators = ComputeComparisonOperators();

		public static HashSet<TokenType> LogicalOperators = new HashSet<TokenType>
		{
			TokenType.Or,
			TokenType.And,
			TokenType.Not
		};

		public static HashSet<TokenType> BooleanValues = new HashSet<TokenType>
		{
			TokenType.True,
			TokenType.False
		};

		public static HashSet<TokenType> Values = new HashSet<TokenType>
		{
			TokenType.String,
			TokenType.Real,
			TokenType.Integer,
			TokenType.Null
		};

		#endregion

		#region methods

		private static HashSet<TokenType> ComputeComparisonOperators()
		{
			var result = new HashSet<TokenType>();
			result.UnionWith( SimpleOperators );
			result.UnionWith( ListOperators );
			return result;
		}

		public static bool IsLogicalOperation( Token token )
		{
			return LogicalOperators.Contains( token.Type );
		}

		public static bool IsBinaryLogicalOperation( Token token )
		{
			return token.Type == TokenType.And || token.Type == TokenType.Or;
		}

		public static bool IsUnaryLogicalOperation( Token token )
		{
			return token.Type == TokenType.Not;
		}

		public static bool IsComparisonOperation( Token token )
		{
			return ComparisonOperators.Contains( token.Type );
		}

		public static bool IsListOperation( Token token )
		{
			return ListOperators.Contains( token.Type );
		}

		public static bool IsBoolean( Token token )
		{
			return BooleanValues.Contains( token.Type );
		}

		public static bool IsValue( Token token )
		{
			return Values.Contains( token.Type );
		}

		#endregion
	}
}
