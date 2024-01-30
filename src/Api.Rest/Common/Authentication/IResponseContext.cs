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

/// <summary>
/// Represents the context of a <see cref="IAuthenticationHandler.HandleResponse"/> call. Can be used to get information about the current
/// request and response. The context also provides actions that are available as reaction to the given response.
/// </summary>
public interface IResponseContext
{
	#region properties

	/// <summary>
	/// Identifies the current request session. This number will be different for each separate rest request but will be the same while
	/// retrying a rest request. See <see cref="RetryRequest"/>.
	/// </summary>
	long Session { get; }

	/// <summary>
	/// Identifies the current attempt to do a rest request. The original request will always be attempt 1. Every following retry will
	/// increase this value by one.
	/// </summary>
	long Attempt { get; }

	/// <summary>
	/// When set to true, the request will be retried after the authentication handler returns.<br/>
	/// Note: Modifying <see cref="CurrentRequest"/> will have no effect on the retried request because a new request is build for each
	/// retry attempt.
	/// </summary>
	bool RetryRequest { get; set; }

	/// <summary>
	/// Defines payload data that will be carried over to the next request attempt when <see cref="RetryRequest"/> is true.
	/// See <see cref="IRequestContext.RetryPayload"/>.
	/// </summary>
	[CanBeNull] public object RetryPayload { get; set; }

	/// <summary>
	/// The current request.
	/// </summary>
	[NotNull] HttpRequestMessage CurrentRequest { get; }

	/// <summary>
	/// The current response.
	/// </summary>
	[NotNull] HttpResponseMessage CurrentResponse { get; }

	/// <summary>
	/// The current authentication container of the rest client used for the request.
	/// </summary>
	[NotNull] AuthenticationContainer CurrentAuthenticationContainer { get; }

	/// <summary>
	/// The service location of the rest client used for the request.
	/// </summary>
	[NotNull] Uri ServiceLocation { get; }

	/// <summary>
	/// The cancellation token of the current operation.
	/// </summary>
	public CancellationToken CancellationToken { get; }

	#endregion

	#region methods

	/// <summary>
	/// Sets a new authentication container on the rest client used for the request. Can be combined with
	/// <see cref="newAuthenticationContainer"/> to modify authentication data of the retried request.
	/// </summary>
	/// <param name="newAuthenticationContainer">The new authentication container to set.</param>
	void UpdateAuthenticationContainer( [NotNull] AuthenticationContainer newAuthenticationContainer );

	#endregion
}