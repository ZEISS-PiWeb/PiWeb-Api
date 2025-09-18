#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Health
{
	#region usings

	using System;
	using System.IO;
	using System.Net;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

	#endregion

	/// <summary>
	/// Web service class to communicate with the REST based PiWeb health service.
	/// </summary>
	public class HealthServiceRestClient : CommonRestClientBase, IHealthServiceRestClient
	{
		#region constants

		/// <summary>
		/// The name of the endpoint of this service.
		/// </summary>
		public const string EndpointName = "health/";

		#endregion

		#region constructors

		/// <summary>
		/// Constructor. Instantiates a new <see cref="HealthServiceRestClient"/> th communicate with the PiWeb-Server OAuthService.
		/// </summary>
		/// <param name="serverUri">
		/// The base url of the PiWeb-Server. Please note that the required "OAuthServiceRest/" will automatically be appended to this url.
		/// </param>
		/// <exception cref="ArgumentNullException"><paramref name="serverUri"/> is <see langword="null" />.</exception>
		public HealthServiceRestClient( [NotNull] Uri serverUri )
			: base( new RestClient( serverUri, EndpointName, serializer: ObjectSerializer.SystemTextJson ) )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthServiceRestClient"/> class.
		/// </summary>
		/// <param name="settings">The settings of the rest service.</param>
		internal HealthServiceRestClient( RestClientSettings settings )
			: base( new RestClient( EndpointName, settings ) )
		{ }

		#endregion

		#region interface IHealthServiceRestClient

		/// <inheritdoc />
		public ICustomRestClient CustomRestClient => _RestClient;

		/// <inheritdoc />
		public async Task<HealthCheckResultType> GetReadiness( CancellationToken cancellationToken = default )
		{
			try
			{
				using var resultStream = await _RestClient.RequestStream( RequestBuilder.CreateGet( "ready" ), cancellationToken ).ConfigureAwait( false );
				using var reader = new StreamReader( resultStream );
				var result = await reader.ReadLineAsync() ?? "";

				if( result.Equals( "healthy", StringComparison.OrdinalIgnoreCase ) )
					return HealthCheckResultType.Healthy;

				if( result.Equals( "degraded", StringComparison.OrdinalIgnoreCase ) )
					return HealthCheckResultType.Degraded;

				throw new InvalidOperationException( $"Unexpected response from readiness check: {result}" );
			}
			catch( WrappedServerErrorException exception )
			{
				if( exception.StatusCode == HttpStatusCode.ServiceUnavailable )
					return HealthCheckResultType.Unhealthy;

				throw;
			}
		}

		/// <inheritdoc />
		public async Task<HealthCheckResultType> GetLiveness( CancellationToken cancellationToken = default )
		{
			try
			{
				using var resultStream = await _RestClient.RequestStream( RequestBuilder.CreateGet( "live" ), cancellationToken ).ConfigureAwait( false );
				using var reader = new StreamReader( resultStream );
				var result = await reader.ReadLineAsync() ?? "";

				if( result.Equals( "healthy", StringComparison.OrdinalIgnoreCase ) )
					return HealthCheckResultType.Healthy;

				if( result.Equals( "degraded", StringComparison.OrdinalIgnoreCase ) )
					return HealthCheckResultType.Degraded;

				throw new InvalidOperationException( $"Unexpected response from liveness check: {result}" );
			}
			catch( WrappedServerErrorException exception )
			{
				if( exception.StatusCode == HttpStatusCode.ServiceUnavailable )
					return HealthCheckResultType.Unhealthy;

				throw;
			}
		}

		#endregion
	}
}