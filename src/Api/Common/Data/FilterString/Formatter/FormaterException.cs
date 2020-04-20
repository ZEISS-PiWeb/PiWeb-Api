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

	#endregion

	public class FormaterException : Exception
	{
		#region constructors

		public FormaterException( string message ) : base( message ) {}

		#endregion
	}
}
