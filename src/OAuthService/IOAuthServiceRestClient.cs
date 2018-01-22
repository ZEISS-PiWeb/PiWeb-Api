#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.OAuthService
{
	#region usings

	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.IMT.PiWeb.Api.Common.Client;

	#endregion

	/// <summary>
	/// Web service interface to communicate with the REST based PiWeb OAuth service.
	/// </summary>
	public interface IOAuthServiceRestClient : IRestClient
	{
		#region methods

		/// <summary>
		/// Method to query the <see cref="ServiceInformation"/>. 
		/// <remarks>
		/// This method can also be used for quick connection check to test if the service is alive. It is 
		/// quaranteed that this method returns quickly and does perform a lot of work server side.
		/// </remarks>
		/// </summary>
		Task<OAuthTokenInformation> GetOAuthTokenInformation( CancellationToken cancellationToken = default( CancellationToken ) );

		/// <summary>
		/// Get information about valid OAuth issues authorities and resource ids.
		/// </summary>
		/// <param name="cancellationToken">A cancelation token to cancel the web service call.</param>
		Task<ServiceInformation> GetServiceInformation();

		#endregion
	}
}