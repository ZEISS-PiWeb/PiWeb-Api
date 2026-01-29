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
	using Zeiss.PiWeb.Api.Rest.Dtos;

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
		/// Method for fetching the <see cref="InterfaceVersionRange"/> to check for relevant supported features.
		/// </summary>
		/// <remarks>
		/// This method can be used for connection checking. The call returns quickly and does not produce any noticeable server load.
		/// </remarks>
		/// <param name="cancellationToken">A cancellation token to cancel the web service call.</param>
		Task<InterfaceVersionRange> GetInterfaceInformation( CancellationToken cancellationToken = default );

		/// <summary>
		/// Readiness indicates if the PiWeb Server is running normally but isn't ready to receive requests.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to cancel the web service call.</param>
		/// <returns>A health check result.</returns>
		Task<HealthCheckStatus> GetReadiness( CancellationToken cancellationToken = default );

		/// <summary>
		/// Liveness indicates if the PiWeb Server has crashed and must be restarted.
		/// </summary>
		/// <param name="cancellationToken">A cancellation token to cancel the web service call.</param>
		/// <returns>A health check result.</returns>
		Task<HealthCheckStatus> GetLiveness( CancellationToken cancellationToken = default );

		#endregion
	}
}