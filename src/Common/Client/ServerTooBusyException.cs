#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	/// <summary>
	/// Thrown if the server is currently too busy to perform your request (e.g. high load resulting in an exception).
	/// </summary>
	public class ServerTooBusyException : RestClientException
	{
		#region constructors

		/// <summary>
		/// Constructor.
		/// </summary>
		public ServerTooBusyException() : base( "The server is currently too busy and your request could not be completed. Please try again." )
		{ }

		#endregion
	}
}