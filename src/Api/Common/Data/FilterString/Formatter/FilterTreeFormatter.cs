#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Common.Data.FilterString.Formatter
{
	#region usings

	using System;
	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Common.Data.FilterString.Tree;

	#endregion

	public class FilterTreeFormatter : IFilterTreeFormatter
	{
		#region methods

		/// <exception cref="ArgumentNullException"><paramref name="tree"/> is <see langword="null" />.</exception>
		public string FormatString( [NotNull] IFilterTree tree )
		{
			if( tree == null )
				throw new ArgumentNullException( nameof( tree ) );

			return FormatExpression( tree, false );
		}

		private string FormatExpression( IFilterTree tree, bool isolate )
		{
			if( TokenMappings.IsLogicalOperation( tree.Token ) )
				return FormatLogicalExpression( tree, isolate );

			if( TokenMappings.IsComparisonOperation( tree.Token ) )
				return FormatComparisonExpression( tree );

			if( TokenMappings.IsBoolean( tree.Token ) )
				return FormatBooleanValue( tree );

			throw new FormaterException( $"Encountered unexpected node of type {tree.Token.Type}." );
		}

		private string FormatLogicalExpression( IFilterTree tree, bool isolate )
		{
			var operatorText = TokenMappings.TokenTypeToDefaultValueMap[ tree.Token.Type ];
			
			string resultExpression;
			if( TokenMappings.IsBinaryLogicalOperation( tree.Token ) )
			{
				var operandTexts = new List<string>();
				foreach( var childItem in tree.GetChildren() )
					operandTexts.Add( FormatExpression( childItem, true ) );

				resultExpression = string.Join( $" {operatorText} ", operandTexts );
			}
			else if( TokenMappings.IsUnaryLogicalOperation( tree.Token ) )
			{
				var childNode = tree.GetChild( 0 );
				var operandText = FormatExpression( childNode, false );

				resultExpression = TokenMappings.IsBinaryLogicalOperation( childNode.Token )
					? $"{operatorText} ({operandText})" 
					: $"{operatorText} {operandText}";
			}
			else
			{
				throw new FormaterException( $"Encountered unexpected node of type {tree.Token.Type}." );
			}
			
			return isolate
				? $"({resultExpression})"
				: resultExpression;
		}

		private string FormatComparisonExpression( IFilterTree tree )
		{
			var attributeText = tree.GetChild( 0 ).Token.Value;
			var operatorText = TokenMappings.TokenTypeToDefaultValueMap[ tree.Token.Type ];
			
			var contentText = TokenMappings.IsListOperation( tree.Token )
				? FormatValueList( tree.GetChild( 1 ) )
				: FormatValue( tree.GetChild( 1 ) );

			return $"{attributeText} {operatorText} {contentText}";
		}

		private string FormatBooleanValue( IFilterTree tree )
		{
			var text = TokenMappings.TokenTypeToDefaultValueMap[ tree.Token.Type ];
			return text;
		}

		private static string FormatValue( IFilterTree tree )
		{
			var text = tree.Token.Value;
			
			switch( tree.Token.Type )
			{
				case TokenType.String:
					return EscapeText( text );
					
				case TokenType.Integer:
				case TokenType.Real:
					return text;
					
				default:
					throw new UnsupportedTokenException( tree.Token );
			}
		}

		private static string FormatValueList( IFilterTree tree )
		{
			if( tree.Token.Type != TokenType.ValueList )
				throw new UnsupportedTokenException( tree.Token );

			var elementTexts = new List<string>();
			foreach( var treeItem in tree.GetChildren() )
			{
				var text = FormatValue( treeItem );
				elementTexts.Add( text );
			}

			var innerList = string.Join( ", ", elementTexts );
			return $"({innerList})";
		}

		private static string EscapeText( string value )
		{
			var result = value.Replace( "'", "''" );
			return $"'{result}'";
		}

		#endregion
	}
}
