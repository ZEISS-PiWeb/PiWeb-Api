﻿#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth
{
	#region usings

	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Client;

	#endregion

	/// <summary>
	/// Web service class to communicate with the REST based PiWeb OAuth service.
	/// </summary>
	public class OAuthServiceRestClient : CommonRestClientBase, IOAuthServiceRestClient
	{
		#region constructors

		/// <summary>
		/// Constructor. Instantiates a new <see cref="OAuthServiceRestClient"/> th communicate with the PiWeb-Server OAuthService.
		/// </summary>
		/// <param name="serverUri">
		/// The base url of the PiWeb-Server. Please note that the required "OAuthServiceRest/" will automatically be appended to this url.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		public OAuthServiceRestClient( [NotNull] Uri serverUri )
			: base( new RestClient( serverUri, "OAuthServiceRest/", serializer: ObjectSerializer.Default ) )
		{ }

		#endregion

		#region interface IOAuthServiceRestClient

		/// <summary>
		/// Get information about valid OAuth issues authorities and resource ids.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to cancel the web service call.</param>
		public Task<OAuthTokenInformation> GetOAuthTokenInformation( CancellationToken cancellationToken = default )
		{
			return _RestClient.Request<OAuthTokenInformation>( RequestBuilder.CreateGet( "oauthTokenInformation" ), default );
		}

		#endregion
	}
}