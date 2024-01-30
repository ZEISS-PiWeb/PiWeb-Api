#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Authentication;

#region usings

using System;
using System.Net.Http;
using System.Threading;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

/// <inheritdoc />
internal class ResponseContext : IResponseContext
{
	#region members

	[NotNull] private readonly RestClientBase _RestClient;

	#endregion

	#region constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="ResponseContext"/> class.
	/// </summary>
	/// <param name="restClient">The rest client used to make the rest request.</param>
	/// <param name="request">The rest request.</param>
	/// <param name="response">The rest response.</param>
	/// <param name="session">Identifies the current request session.</param>
	/// <param name="attempt">The current request attempt.</param>
	/// <param name="cancellationToken">The cancellation token of the operation.</param>
	public ResponseContext(
		[NotNull] RestClientBase restClient,
		[NotNull] HttpRequestMessage request,
		[NotNull] HttpResponseMessage response,
		long session,
		long attempt,
		CancellationToken cancellationToken)
	{
		_RestClient = restClient ?? throw new ArgumentNullException( nameof( restClient ) );

		Session = session;
		Attempt = attempt;
		CancellationToken = cancellationToken;
		CurrentRequest = request ?? throw new ArgumentNullException( nameof( request ) );
		CurrentResponse = response ?? throw new ArgumentNullException( nameof( response ) );
	}

	#endregion

	#region interface IResponseContext

	/// <inheritdoc />
	public long Session { get; }

	/// <inheritdoc />
	public long Attempt { get; }

	/// <inheritdoc />
	public bool RetryRequest { get; set; }

	/// <inheritdoc />
	[CanBeNull] public object RetryPayload { get; set; }

	/// <inheritdoc />
	[NotNull]
	public HttpRequestMessage CurrentRequest { get; }

	/// <inheritdoc />
	[NotNull]
	public HttpResponseMessage CurrentResponse { get; }

	/// <inheritdoc />
	[NotNull]
	public AuthenticationContainer CurrentAuthenticationContainer => _RestClient.AuthenticationContainer;

	/// <inheritdoc />
	public Uri ServiceLocation => _RestClient.ServiceLocation;

	/// <inheritdoc />
	public CancellationToken CancellationToken { get; }

	/// <inheritdoc />
	public void UpdateAuthenticationContainer( [NotNull] AuthenticationContainer newAuthenticationContainer )
	{
		_RestClient.AuthenticationContainer =
			newAuthenticationContainer ?? throw new ArgumentNullException( nameof( newAuthenticationContainer ) );
	}

	#endregion
}