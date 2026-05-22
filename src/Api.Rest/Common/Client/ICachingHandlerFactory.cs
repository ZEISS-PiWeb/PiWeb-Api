#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client;

using System.Net.Http;

/// <summary>
/// Defines the interface for a factory that creates caching handlers to be used in the HTTP request pipeline of a REST client.
/// </summary>
public interface ICachingHandlerFactory
{
	/// <summary>
	/// Creates a <see cref="DelegatingHandler"/> that adds caching capabilities to HTTP requests.
	/// </summary>
	/// <remarks>Use the returned handler as part of an <see cref="HttpMessageHandler"/> pipeline to enable response
	/// caching. The specific caching behavior depends on the implementation of the handler.</remarks>
	/// <returns>A <see cref="DelegatingHandler"/> instance that applies caching logic to outgoing HTTP requests and responses.</returns>
	DelegatingHandler CreateCachingHandler();
}
