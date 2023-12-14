#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.OAuth
{
	#region usings

	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.PiWeb.Api.Rest.Common.Client;

	#endregion

	/// <summary>
	/// Web service interface to communicate with the REST based PiWeb OAuth service.
	/// </summary>
	public interface IOAuthServiceRestClient : IRestClient
	{
		#region methods

		/// <summary>
		/// Method to query the <see cref="OAuthTokenInformation"/>.
		/// <remarks>
		/// This method can also be used for quick connection check to test if the service is alive. It is
		/// quaranteed that this method returns quickly and does perform a lot of work server side.
		/// </remarks>
		/// </summary>
		Task<OAuthTokenInformation> GetOAuthTokenInformation( CancellationToken cancellationToken = default );

		#endregion
	}
}