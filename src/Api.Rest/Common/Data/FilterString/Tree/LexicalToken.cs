#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Tree
{
	public class LexicalToken
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="LexicalToken"/> class.
		/// </summary>
		public LexicalToken( string value )
		{
			Value = value;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LexicalToken"/> class.
		/// </summary>
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

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return $"{Value}";
		}

		#endregion
	}
}