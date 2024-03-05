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
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

/// <summary>
/// Represents the context of a <see cref="IAuthenticationHandler.InitializeRestClient"/> call. Can be used to get information about the
/// rest client. The context also provides actions that are available to modify the rest client.
/// </summary>
public interface IInitializationContext
{
	#region properties

	/// <summary>
	/// The current authentication container of the rest client.
	/// </summary>
	[NotNull] AuthenticationContainer CurrentAuthenticationContainer { get; }

	/// <summary>
	/// The service location of the rest client.
	/// </summary>
	[NotNull] Uri ServiceLocation { get; }

	#endregion

	#region methods

	/// <summary>
	/// Sets a new authentication container on the rest client.
	/// </summary>
	/// <param name="newAuthenticationContainer">The new authentication container to set.</param>
	void UpdateAuthenticationContainer( [NotNull] AuthenticationContainer newAuthenticationContainer );

	#endregion
}