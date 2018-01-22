#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Formatter
{
	using Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree;

	public class UnsupportedCharacterException : FormaterException
	{
		#region constructors

		public UnsupportedCharacterException( char unsupportedCharacter, Token unsupportedToken ) : base( $"The character '{unsupportedCharacter}' may not be in a value." )
		{
			UnsupportedCharacter = unsupportedCharacter;
			UnsupportedToken = unsupportedToken;
		}

		#endregion

		#region properties

		public char UnsupportedCharacter { get; private set; }
		public Token UnsupportedToken { get; private set; }

		#endregion
	}
}
