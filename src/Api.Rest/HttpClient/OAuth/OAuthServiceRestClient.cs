#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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
	using Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

	#endregion

	/// <summary>
	/// Web service class to communicate with the REST based PiWeb OAuth service.
	/// </summary>
	public class OAuthServiceRestClient : CommonRestClientBase, IOAuthServiceRestClient
	{
		#region constants

		/// <summary>
		/// The name of the endpoint of this service.
		/// </summary>
		public const string EndpointName = "OAuthServiceRest/";

		#endregion

		#region constructors

		/// <summary>
		/// Constructor. Instantiates a new <see cref="OAuthServiceRestClient"/> th communicate with the PiWeb-Server OAuthService.
		/// </summary>
		/// <param name="serverUri">
		/// The base url of the PiWeb-Server. Please note that the required "OAuthServiceRest/" will automatically be appended to this url.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		public OAuthServiceRestClient( [NotNull] Uri serverUri )
			: base( new RestClient( serverUri, EndpointName, serializer: ObjectSerializer.SystemTextJson ) )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthServiceRestClient"/> class.
		/// </summary>
		/// <param name="settings">The settings of the rest service.</param>
		internal OAuthServiceRestClient( RestClientSettings settings )
			: base( new RestClient( EndpointName, settings ) )
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