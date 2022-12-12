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
	using CacheCow.Client;
	using CacheCow.Common;
	using JetBrains.Annotations;

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
		public RestClient(
			[NotNull] Uri serverUri,
			string endpointName,
			TimeSpan? timeout = null,
			int maxUriLength = DefaultMaxUriLength,
			bool chunked = true,
			[CanBeNull] IObjectSerializer serializer = null,
			[CanBeNull] ICacheStore cacheStore = null,
			[CanBeNull] IVaryHeaderStore varyHeaderStore = null )
			: base( serverUri, endpointName, timeout, maxUriLength, chunked, serializer: serializer, cacheStore: cacheStore, varyHeaderStore: varyHeaderStore )
		{ }

		#endregion
	}
}