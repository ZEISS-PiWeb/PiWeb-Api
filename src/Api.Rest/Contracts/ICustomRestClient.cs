/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

namespace Zeiss.PiWeb.Api.Rest.Contracts;

#region usings

using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

/// <summary>
/// Represent a custom rest client that can be used to execute generic rest requests created by a request builder.
/// </summary>
public interface ICustomRestClient
{
	#region methods

	Task Request( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task Request( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<T> Request<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<T> Request<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<Stream> RequestStream( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<Stream> RequestStream( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken );
	IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken );
	Task<byte[]> RequestBytes( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<byte[]> RequestBytes( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<T> RequestBinary<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );
	Task<T> RequestBinary<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Start an async operation. Returns an URL to poll for status updates if the operation is accepted, otherwise the operation is done synchronously.
	/// </summary>
	/// <returns>Returns a Task that represents the duration of the initial REST request. The result of the task contains
	/// the URI for polling the operation result or null in case the server already finished the request synchronously.</returns>
	/// <exception cref="RestClientException">The response indicated status Accepted, but did not contain polling information.</exception>
	Task<Uri> RequestAsyncOperation( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Start an async operation. Returns an URL to poll for status updates if the operation is accepted, otherwise the operation is done synchronously.
	/// </summary>
	/// <returns>Returns a Task that represents the duration of the initial REST request. The result of the task contains
	/// the URI for polling the operation result or null in case the server already finished the request synchronously.</returns>
	/// <exception cref="RestClientException">The response indicated status Accepted, but did not contain polling information.</exception>
	Task<Uri> RequestAsyncOperation( [NotNull] Func<IObjectSerializer, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	#endregion
}