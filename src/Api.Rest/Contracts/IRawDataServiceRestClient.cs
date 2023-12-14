#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	/// <summary>
	/// Client class for communicating with the REST based raw data service.
	/// </summary>
	public interface IRawDataServiceRestClient : IRawDataServiceRestClientBase<RawDataServiceFeatureMatrix>
	{ }
}