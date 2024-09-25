#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
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
internal class RequestContext : IRequestContext
{
	#region members

	[NotNull] private readonly RestClientBase _RestClient;

	#endregion

	#region constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="RequestContext"/> class.
	/// </summary>
	/// <param name="restClient">The rest client used to make the rest request.</param>
	/// <param name="request">The rest request.</param>
	/// <param name="session">Identifies the current request session.</param>
	/// <param name="attempt">The current request attempt.</param>
	/// <param name="retryPayload">The payload carried over from the previous attempt.</param>
	/// <param name="cancellationToken">The cancellation token of the operation.</param>
	public RequestContext(
		[NotNull] RestClientBase restClient,
		[NotNull] HttpRequestMessage request,
		long session,
		long attempt,
		[CanBeNull] object retryPayload,
		CancellationToken cancellationToken)
	{
		_RestClient = restClient ?? throw new ArgumentNullException( nameof( restClient ) );

		Session = session;
		Attempt = attempt;
		CurrentRequest = request ?? throw new ArgumentNullException( nameof( request ) );
		RetryPayload = retryPayload;
		CancellationToken = cancellationToken;
	}

	#endregion

	#region interface IResponseContext

	/// <inheritdoc />
	public long Session { get; }

	/// <inheritdoc />
	public long Attempt { get; }

	/// <inheritdoc />
	public HttpRequestMessage CurrentRequest { get; }

	/// <inheritdoc />
	public AuthenticationContainer CurrentAuthenticationContainer => _RestClient.AuthenticationContainer;

	/// <inheritdoc />
	public Uri ServiceLocation => _RestClient.ServiceLocation;

	/// <inheritdoc />
	public object RetryPayload { get; }

	/// <inheritdoc />
	public CancellationToken CancellationToken { get; }

	/// <inheritdoc />
	public void UpdateAuthenticationContainer( AuthenticationContainer newAuthenticationContainer )
	{
		_RestClient.AuthenticationContainer =
			newAuthenticationContainer ?? throw new ArgumentNullException( nameof( newAuthenticationContainer ) );
	}

	#endregion
}