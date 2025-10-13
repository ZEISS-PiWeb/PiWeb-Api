#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

using Zeiss.PiWeb.Api.Rest.HttpClient.Data;
using Zeiss.PiWeb.Api.Rest.HttpClient.Health;
using Zeiss.PiWeb.Api.Rest.HttpClient.OAuth;
using Zeiss.PiWeb.Api.Rest.HttpClient.RawData;

/// <summary>
/// Responsible for building PiWeb API rest clients.
/// </summary>
public interface IRestClientBuilder
{
	/// <summary>
	/// Creates a data service rest client using the current setup.
	/// </summary>
	/// <returns>The created data service rest client.</returns>
	public DataServiceRestClient CreateDataServiceRestClient();

	/// <summary>
	/// Creates a raw data service rest client using the current setup.
	/// </summary>
	/// <returns>The created raw data service rest client.</returns>
	public RawDataServiceRestClient CreateRawDataServiceRestClient();

	/// <summary>
	/// Creates an oauth service rest client using the current setup.
	/// </summary>
	/// <returns>The created oauth service rest client.</returns>
	public OAuthServiceRestClient CreateOAuthServiceRestClient();

	/// <summary>
	/// Creates a health service rest client using the current setup.
	/// </summary>
	/// <returns>The created health service rest client.</returns>
	public HealthServiceRestClient CreateHealthServiceRestClient();
}