#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2025                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Health
{
	#region usings

	using System;
	using System.Net;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;
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
		public const string EndpointName = "HealthServiceRest";

		#endregion

		#region members

		private HealthServiceFeatureMatrix _FeatureMatrix;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="HealthServiceRestClient"/> class.
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

		#region methods

		private async Task<HealthCheckStatus> ExecuteHealthCheck( string relativeUri, CancellationToken cancellationToken = default )
		{
			try
			{
				await ThrowOnUnsupported( cancellationToken );
				return await _RestClient.Request<HealthCheckStatus>( RequestBuilder.CreateGet( relativeUri ), cancellationToken ).ConfigureAwait( false );
			}
			catch( WrappedServerErrorException exception )
			{
				if( exception.StatusCode == HttpStatusCode.ServiceUnavailable )
					return new HealthCheckStatus { Status = HealthCheckResultType.Unhealthy };

				throw;
			}
		}

		/// <summary>
		/// Throws an exception if the API endpoint is not available (404) or the interface version is below the minimum required version.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="OperationNotSupportedOnServerException">The server does not support this API operation.</exception>
		private async Task ThrowOnUnsupported( CancellationToken cancellationToken )
		{
			try
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				// in case of the supported version is below the minimum required version
				if( !featureMatrix.SupportsHealthService )
				{
					throw new OperationNotSupportedOnServerException( "Healthcheck API version is below the minimum required version.", HealthServiceFeatureMatrix.HealthCheckMinVersion, featureMatrix.CurrentInterfaceVersion );
				}
			}
			catch( WrappedServerErrorException exception )
			{
				// in case of 404
				if( exception.StatusCode == HttpStatusCode.NotFound )
				{
					throw new OperationNotSupportedOnServerException( "Healthcheck API is not supported by this server.", HealthServiceFeatureMatrix.HealthCheckMinVersion, new Version( 0, 0, 0 ) );
				}
			}
		}

		private async Task<HealthServiceFeatureMatrix> GetFeatureMatrixInternal( FetchBehavior behavior, CancellationToken cancellationToken = default )
		{
			// This is an intentional race condition. Calling this method from multiple threads may lead to multiple calls to Get<InterfaceInformation>().
			// However, this would be rare and harmless, since it should always return the same result. It would be a lot more difficult to make this work without any races or blocking.
			// It is important to never set _FeatureMatrix to null anywhere to avoid possible null returns here due to the race condition.
			if( behavior == FetchBehavior.FetchAlways || _FeatureMatrix == null )
			{
				var interfaceVersionRange = await GetInterfaceInformation( cancellationToken ).ConfigureAwait( false );
				_FeatureMatrix = new HealthServiceFeatureMatrix( interfaceVersionRange );
			}

			return _FeatureMatrix;
		}

		#endregion

		#region interface IHealthServiceRestClient

		/// <inheritdoc />
		public ICustomRestClient CustomRestClient => _RestClient;

		/// <inheritdoc />
		public async Task<InterfaceVersionRange> GetInterfaceInformation( CancellationToken cancellationToken = default )
		{
			return await _RestClient.Request<InterfaceVersionRange>( RequestBuilder.CreateGet( EndpointName ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		public async Task<HealthCheckStatus> GetReadiness( CancellationToken cancellationToken = default )
		{
			return await ExecuteHealthCheck( $"{EndpointName}/ready", cancellationToken );
		}

		/// <inheritdoc />
		public async Task<HealthCheckStatus> GetLiveness( CancellationToken cancellationToken = default )
		{
			return await ExecuteHealthCheck( $"{EndpointName}/live", cancellationToken );
		}

		#endregion
	}
}