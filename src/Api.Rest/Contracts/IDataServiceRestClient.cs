#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	/// <summary>
	/// Client interface for communicating with the REST based data service.
	/// </summary>
	public interface IDataServiceRestClient : IDataServiceRestClientBase<DataServiceFeatureMatrix>
	{ }
}