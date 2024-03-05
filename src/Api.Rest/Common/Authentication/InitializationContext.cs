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

/// <inheritdoc />
internal class InitializationContext : IInitializationContext
{
	#region members

	[NotNull] private readonly RestClientBase _RestClient;

	#endregion

	#region constructors

	/// <summary>
	/// Initializes a new instance of the <see cref="InitializationContext"/> class.
	/// </summary>
	/// <param name="restClient">The rest client.</param>
	public InitializationContext( [NotNull] RestClientBase restClient )
	{
		_RestClient = restClient ?? throw new ArgumentNullException( nameof( restClient ) );
	}

	#endregion

	#region interface IResponseContext

	/// <inheritdoc />
	[NotNull]
	public AuthenticationContainer CurrentAuthenticationContainer => _RestClient.AuthenticationContainer;

	/// <inheritdoc />
	[NotNull]
	public Uri ServiceLocation => _RestClient.ServiceLocation;

	/// <inheritdoc />
	public void UpdateAuthenticationContainer( [NotNull] AuthenticationContainer newAuthenticationContainer )
	{
		_RestClient.AuthenticationContainer =
			newAuthenticationContainer ?? throw new ArgumentNullException( nameof( newAuthenticationContainer ) );
	}

	#endregion
}