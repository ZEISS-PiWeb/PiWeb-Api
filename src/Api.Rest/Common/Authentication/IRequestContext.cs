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
/// Represents the context of a <see cref="IAuthenticationHandler.HandleRequest"/> call. Can be used to get information about the current
/// request. The context also provides actions that are available to modify the behavior of the request.
/// </summary>
public interface IRequestContext
{
	#region properties

	/// <summary>
	/// Identifies the current request session. This number will be different for each separate rest request but will be the same while
	/// retrying a rest request. See <see cref="ResponseContext.RetryRequest"/>.
	/// </summary>
	long Session { get; }

	/// <summary>
	/// Identifies the current attempt to do a rest request. The original request will always be attempt 1. Every following retry will
	/// increase this value by one. See <see cref="ResponseContext.RetryRequest"/>.
	/// </summary>
	long Attempt { get; }

	/// <summary>
	/// The current request.
	/// </summary>
	[NotNull] HttpRequestMessage CurrentRequest { get; }

	/// <summary>
	/// The current authentication container of the rest client used for the request.
	/// </summary>
	[NotNull] AuthenticationContainer CurrentAuthenticationContainer { get; }

	/// <summary>
	/// The service location of the rest client used for the request.
	/// </summary>
	[NotNull] Uri ServiceLocation { get; }

	/// <summary>
	/// Payload data carried over from the preceding response handler. This field is only set when this is a retry attempt.
	/// See <see cref="Payload"/>.
	/// </summary>
	[CanBeNull] public object RetryPayload { get; }

	/// <summary>
	/// The cancellation token of the current operation.
	/// </summary>
	public CancellationToken CancellationToken { get; }

	#endregion

	#region methods

	/// <summary>
	/// Sets a new authentication container on the rest client used for the request.
	/// </summary>
	/// <param name="newAuthenticationContainer">The new authentication container to set.</param>
	void UpdateAuthenticationContainer( [NotNull] AuthenticationContainer newAuthenticationContainer );

	#endregion
}