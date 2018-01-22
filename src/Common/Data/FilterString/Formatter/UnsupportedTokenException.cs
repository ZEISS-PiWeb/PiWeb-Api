#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Formatter
{
	#region usings

	using Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree;

	#endregion

	public class UnsupportedTokenException : FormaterException
	{
		#region constructors

		public UnsupportedTokenException( Token unsupportedToken ) : base( $"Token type '{unsupportedToken.Type}' is not supported by this formater." )
		{
			UnsupportedToken = unsupportedToken;
		}

		#endregion

		#region properties

		public Token UnsupportedToken { get; private set; }

		#endregion
	}
}
