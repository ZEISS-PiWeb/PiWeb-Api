#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.IO;
	using System.Net;
	using System.Net.Cache;
	using System.Net.Http;
	using System.Net.Http.Headers;
	using System.Text;
	using System.Threading;
	using System.Threading.Tasks;

	using Newtonsoft.Json;
	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Data;

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
