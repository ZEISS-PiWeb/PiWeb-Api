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

	/// <summary>
	/// An <see cref="Exception"/> that is being used for indicating that a request is canceled.
	/// </summary>
	/// <remarks>
	/// This is usually raised by the user at the moment where credentials are requested.
	/// E.g. clicking on cancel in a login window.
	/// </remarks>
	public class LoginCanceledException : Exception
	{
		#region constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public LoginCanceledException()
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The displayed message.</param>
		public LoginCanceledException( string message )
			: base( message )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="exception">An inner exception.</param>
		public LoginCanceledException( Exception exception )
			: base( "Login canceled", exception )
		{
		}

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="message">The displayed message.</param>
		/// <param name="exception">An inner exception.</param>
		public LoginCanceledException( string message, Exception exception )
			: base( message, exception )
		{
		}

		#endregion
	}
}