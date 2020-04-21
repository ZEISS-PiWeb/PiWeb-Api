#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region using

	using System;

	#endregion

	/// <summary>
	/// Base class for client side exceptions that are thrown by the REST clients
	/// (<see cref="IRawDataServiceRestClient"/>, <see cref="IDataServiceRestClient"/>) in case of errors.
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
