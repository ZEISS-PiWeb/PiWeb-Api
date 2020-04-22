#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Formatter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree;

	#endregion

	public class LegacyFilterTreeFormatter : IFilterTreeFormatter
	{
		#region methods

		private string FormatExpression( IFilterTree tree )
		{
			if( TokenMappings.IsLogicalOperation( tree.Token ) )
				return FormatLogicalExpression( tree );

			if( TokenMappings.IsComparisonOperation( tree.Token ) )
				return FormatComparisonExpression( tree );

			throw new FormaterException( $"Encountered unexpected node of type {tree.Token.Type}." );
		}

		private string FormatLogicalExpression( IFilterTree tree )
		{
			if( tree.Token.Type != TokenType.And )
				throw new UnsupportedTokenException( tree.Token );

			var operatorText = TokenMappings.TokenTypeToLegacyValueMap[ tree.Token.Type ];

			var operandTexts = new List<string>();
			foreach( var childItem in tree.GetChildren() )
				operandTexts.Add( FormatExpression( childItem ) );

			var resultExpression = string.Join( $" {operatorText} ", operandTexts );

			return resultExpression;
		}

		private string FormatComparisonExpression( IFilterTree tree )
		{
			var attributeText = tree.GetChild( 0 ).Token.Value;
			var operatorText = TokenMappings.TokenTypeToLegacyValueMap[ tree.Token.Type ];

			var contentText = TokenMappings.IsListOperation( tree.Token )
				? FormatValueList( tree.GetChild( 1 ) )
				: FormatValue( tree.GetChild( 1 ) );

			return $"{attributeText} {operatorText} [{contentText}]";
		}

		private static string FormatValue( IFilterTree tree )
		{
			if( tree.Token.Type != TokenType.LegacyValue )
				throw new UnsupportedTokenException( tree.Token );

			if( tree.Token.Value.IndexOf( ']' ) != -1 )
				throw new UnsupportedCharacterException( ']', tree.Token );

			return tree.Token.Value;
		}

		private static string FormatValueList( IFilterTree tree )
		{
			var elementTexts = new List<string>();
			foreach( var treeItem in tree.GetChildren() )
			{
				var text = FormatValue( treeItem );
				if( NeedsEscaping( text ) )
					text = EscapeText( text );

				elementTexts.Add( text );
			}

			return string.Join( ", ", elementTexts );
		}

		private static string EscapeText( string value )
		{
			var result = value.Replace( "'", "''" );
			return $"'{result}'";
		}

		private static bool NeedsEscaping( string value )
		{
			var isEmpty = value.Length == 0;
			var hasLeadingWhitspace = value.Length > 0 && char.IsWhiteSpace( value[ 0 ] );
			var hasTrailingWhitspace = value.Length > 0 && char.IsWhiteSpace( value[ value.Length - 1 ] );
			var containsComma = value.IndexOf( ',' ) != -1;
			var containsQuote = value.IndexOf( '\'' ) != -1;
			var containsClosingBracket = value.IndexOf( ']' ) != -1;

			return isEmpty || hasLeadingWhitspace || hasTrailingWhitspace || containsComma || containsQuote || containsClosingBracket;
		}

		#endregion

		#region interface IFilterTreeFormatter

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="tree"/> is <see langword="null" />.</exception>
		public string FormatString( IFilterTree tree )
		{
			if( tree == null )
				throw new ArgumentNullException( nameof( tree ) );

			return FormatExpression( tree );
		}

		#endregion
	}
}