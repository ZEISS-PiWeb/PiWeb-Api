#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{

	/// <summary>
	/// Web service interface to communicate with the REST based PiWeb report management service.
	/// </summary>
	public interface IReportManagementServiceRestClient : IReportManagementServiceRestClientBase<ReportManagementServiceFeatureMatrix>
	{ }

}