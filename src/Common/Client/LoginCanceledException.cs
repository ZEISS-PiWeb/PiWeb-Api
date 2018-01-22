#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;

	#endregion

	public class LoginCanceledException : Exception
	{
		#region constructors

		public LoginCanceledException()
		{
		}

		public LoginCanceledException( string message )
			: base( message )
		{
		}

		public LoginCanceledException( Exception exception )
			: base( "Login canceled", exception )
		{
		}

		public LoginCanceledException( string message, Exception exception )
			: base( message, exception )
		{
		}

		#endregion
	}
}