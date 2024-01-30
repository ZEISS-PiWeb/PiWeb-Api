#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion


namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

	#endregion

	/// <summary>
	/// class for communication with REST based web services.
	/// </summary>
	public class RestClient : RestClientBase
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClient" /> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		public RestClient( [NotNull] Uri serverUri, string endpointName, TimeSpan? timeout = null, int maxUriLength = DefaultMaxUriLength, bool chunked = true, [CanBeNull] IObjectSerializer serializer = null )
			: base( serverUri, endpointName, timeout, maxUriLength, chunked, serializer: serializer )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="RestClient" /> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="endpointName"/> is <see langword="null" />.</exception>
		/// <exception cref="ArgumentNullException"><paramref name="settings"/> is <see langword="null" />.</exception>
		internal RestClient( [NotNull] string endpointName, [NotNull] RestClientSettings settings )
			: base( endpointName, settings )
		{ }

		#endregion
	}
}