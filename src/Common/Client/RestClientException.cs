#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region using

	using System;

	#endregion

	/// <summary>
	/// Base class for client side exceptions that are thrown by the <see cref="RestClient"/> in case of errors.
	/// </summary>
	public class RestClientException : Exception
	{
		#region constructor
		
		/// <summary>
		/// Constructor.
		/// </summary>
		public RestClientException()
		{}

		/// <summary>
		/// Constructor.
		/// </summary>
		public RestClientException( string message )
			: base( message )
		{}

		/// <summary>
		/// Constructor.
		/// </summary>
		public RestClientException( string message, Exception innerException )
			: base( message, innerException )
		{ }

		#endregion
	}
}
