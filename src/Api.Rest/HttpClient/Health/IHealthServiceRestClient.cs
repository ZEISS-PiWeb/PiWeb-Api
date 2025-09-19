#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Health
{
	#region usings

	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Contracts;

	#endregion

	/// <summary>
	/// Web service interface to communicate with the REST based PiWeb health service.
	/// </summary>
	public interface IHealthServiceRestClient : IRestClient
	{
		#region properties

		/// <summary>
		/// A custom rest client that can be used to execute rest request created by a rest request builder.
		/// </summary>
		public ICustomRestClient CustomRestClient { get; }

		#endregion

		#region methods

		/// <summary>
		/// Readiness indicates if the PiWeb Server is running normally but isn't ready to receive requests.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to cancel the web service call.</param>
		/// <returns>A health check result.</returns>
		Task<HealthCheckResultType> GetReadiness( CancellationToken cancellationToken = default );

		/// <summary>
		/// Liveness indicates if the PiWeb Server has crashed and must be restarted.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to cancel the web service call.</param>
		/// <returns>A health check result.</returns>
		Task<HealthCheckResultType> GetLiveness( CancellationToken cancellationToken = default );

		#endregion
	}
}