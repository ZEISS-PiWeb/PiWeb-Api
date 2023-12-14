#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree
{
	#region usings

	using System;
	using System.Text;
	using JetBrains.Annotations;

	#endregion

	public class Token
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class.
		/// </summary>
		public Token( TokenType type )
		{
			Type = type;
			Value = string.Empty;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null" />.</exception>
		public Token( TokenType type, [NotNull] string value )
		{
			Type = type;
			Value = value ?? throw new ArgumentNullException( nameof( value ) );
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="lexicalToken"/> is <see langword="null" />.</exception>
		public Token( TokenType type, [NotNull] LexicalToken lexicalToken )
		{
			if( lexicalToken == null )
				throw new ArgumentNullException( nameof( lexicalToken ) );

			Type = type;
			Value = lexicalToken.Value;
			LexicalToken = lexicalToken;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Token"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="lexicalToken"/> is <see langword="null" />.</exception>
		public Token( TokenType type, [NotNull] string value, [NotNull] LexicalToken lexicalToken )
		{
			Type = type;
			Value = value ?? throw new ArgumentNullException( nameof( value ) );
			LexicalToken = lexicalToken ?? throw new ArgumentNullException( nameof( lexicalToken ) );
		}

		#endregion

		#region properties

		public TokenType Type { get; }
		public string Value { get; }

		[CanBeNull]
		public LexicalToken LexicalToken { get; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			var builder = new StringBuilder();
			builder.Append( Type );

			if( LexicalToken != null )
			{
				builder.Append( '(' );
				builder.Append( '\'' );
				builder.Append( LexicalToken.Value );
				builder.Append( '\'' );
				builder.Append( '[' );
				builder.Append( LexicalToken.Position );
				builder.Append( "->" );
				builder.Append( LexicalToken.Position + LexicalToken.Length );
				builder.Append( ']' );
				if( LexicalToken.Value != Value )
				{
					builder.Append( "=>" );
					builder.Append( '\'' );
					builder.Append( Value );
					builder.Append( '\'' );
				}

				builder.Append( ')' );
			}
			else
			{
				builder.Append( '(' );
				builder.Append( '\'' );
				builder.Append( Value );
				builder.Append( '\'' );
				builder.Append( ')' );
			}

			return builder.ToString();
		}

		#endregion
	}
}