#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	/// <summary>
	/// Client class for communicating with the REST based raw data service.
	/// </summary>
	public interface IRawDataServiceRestClient : IRawDataServiceRestClientBase<RawDataServiceFeatureMatrix>
	{}
}