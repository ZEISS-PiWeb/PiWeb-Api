#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{

	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.ReportManagement;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based report management service.
	/// </summary>
	public interface IReportManagementServiceRestClientBase<T> where T : ReportManagementServiceFeatureMatrix
	{
		#region properties

		/// <summary>
		/// A custom rest client that can be used to execute rest request created by a rest request builder.
		/// </summary>
		public ICustomRestClient CustomRestClient { get; }

		#endregion

		#region methods

		/// <summary>
		/// Retrieves a list of all reports.
		/// </summary>
		/// <param name="deleted">
		/// <see langword="true" /> if the result should be restricted to deleted reports,
		/// <see langword="false" /> if the result should be restricted to non-deleted reports,
		/// <see langword="null" /> if the result should not be restricted regarding deletion.
		/// </param>
		/// <param name="cancellationToken">Propagates notification that operations should be canceled.</param>
		Task<IReadOnlyCollection<ReportMetadataDto>> GetReportMetadataList( bool? deleted, CancellationToken cancellationToken = default );

		#endregion
	}
}