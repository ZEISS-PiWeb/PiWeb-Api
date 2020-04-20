#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Tree
{
	public class LexicalToken
	{
		#region constructors

		public LexicalToken( string value )
		{
			Value = value;
		}

		public LexicalToken( int position, int line, int length, int linePosition, string value ) : this( value )
		{
			Position = position;
			Line = line;
			Length = length;
			LinePosition = linePosition;
		}

		#endregion

		#region properties

		public int Position { get; }
		public int LinePosition { get; }
		public int Line { get; }
		public int Length { get; }

		public string Value { get; }

		#endregion

		/// <summary>
		/// Überschriebene <see cref="System.Object.ToString"/>-Methode.
		/// </summary>
		public override string ToString()
		{
			return $"{Value}";
		}
	}
}