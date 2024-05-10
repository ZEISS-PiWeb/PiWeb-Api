#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.ReportManagement
{

	#region usings

	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.ReportManagement;
	using Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based report management service.
	/// </summary>
	public sealed class ReportManagementServiceRestClient : CommonRestClientBase, IReportManagementServiceRestClient
	{
		#region constants

		/// <summary>
		/// The name of the endpoint of this service.
		/// </summary>
		public const string EndpointName = "ReportManagementServiceRest/";

		#endregion

		#region constructors

		/// <summary>
		/// Constructor. Instantiates a new <see cref="ReportManagementServiceRestClient"/> to communicate with the PiWeb-Server ReportManagementServiceRest.
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		/// <param name="maxUriLength">The uri length limit</param>
		/// <param name="restClient">Custom implementation of RestClient</param>
		public ReportManagementServiceRestClient( [NotNull] Uri serverUri, int maxUriLength = RestClientBase.DefaultMaxUriLength, RestClientBase restClient = null )
			: base( restClient ?? new RestClient( serverUri, EndpointName, maxUriLength: maxUriLength, serializer: ObjectSerializer.SystemTextJson ) )
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="ReportManagementServiceRestClient"/> class.
		/// </summary>
		/// <param name="settings">The settings of the rest service.</param>
		internal ReportManagementServiceRestClient( RestClientSettings settings )
			: base( new RestClient( EndpointName, settings ) )
		{ }

		#endregion

		#region interface IReportManagementServiceRestClient

		/// <inheritdoc />
		public ICustomRestClient CustomRestClient => _RestClient;

		/// <inheritdoc />
		public Task<IReadOnlyCollection<ReportMetadataDto>> GetReportMetadataList( bool? deleted, CancellationToken cancellationToken = default )
		{
			return _RestClient.Request<IReadOnlyCollection<ReportMetadataDto>>(
				deleted == null
					? RequestBuilder.CreateGet( "Reports" )
					: RequestBuilder.CreateGet( "Reports", ParameterDefinition.Create( "deleted", deleted.Value.ToString() ) ),
				cancellationToken );
		}

		#endregion
	}
}