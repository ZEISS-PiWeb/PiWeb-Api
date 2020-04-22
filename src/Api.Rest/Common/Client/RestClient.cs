#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// class for communication with REST based web services.
	/// </summary>
	public class RestClient : RestClientBase
	{
		#region constructors

		/// <summary>Constructor.</summary>
		public RestClient( Uri serverUri, string endpointName, TimeSpan? timeout = null, int maxUriLength = DefaultMaxUriLength, bool chunked = true )
        :base(serverUri, endpointName, timeout, maxUriLength, chunked)
		{}

		#endregion
	}
}
