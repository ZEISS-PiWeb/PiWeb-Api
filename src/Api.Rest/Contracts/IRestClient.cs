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
public interface IRestClient
{
	#region properties

	/// <summary>
	/// Gets the URI endpoint of the service.
	/// </summary>
	Uri ServiceLocation { get; }

	/// <summary>
	/// Gets the maximum allowed length of the request URI. Requests exceeding this length may be split or handled differently.
	/// </summary>
	int MaxUriLength { get; }

	#endregion

	#region methods

	/// <summary>
	/// Sends an HTTP request created by the specified handler.
	/// </summary>
	/// <param name="requestCreationHandler">A delegate that creates and returns the <see cref="HttpRequestMessage"/> to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation of sending the HTTP request.</returns>
	Task Request( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request using a user-provided handler to create the request message.
	/// </summary>
	/// <param name="requestCreationHandler">A delegate that creates and returns the <see cref="HttpRequestMessage"/> to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation of sending the HTTP request.</returns>
	Task Request( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and returns the response deserialized as the specified type.
	/// </summary>
	/// <typeparam name="T">The type to which the HTTP response content is deserialized.</typeparam>
	/// <param name="requestCreationHandler">>A delegate that creates and returns the <see cref="HttpRequestMessage"/> to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type T.</returns>
	Task<T> Request<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and asynchronously deserializes the response to the specified type.
	/// </summary>
	/// <typeparam name="T">The type to which the HTTP response content will be deserialized.</typeparam>
	/// <param name="requestCreationHandler">>A delegate that creates and returns the <see cref="HttpRequestMessage"/> to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the deserialized response of type T.</returns>
	Task<T> Request<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and returns the response content as a <see cref="Stream"/>.
	/// </summary>
	/// <param name="requestCreationHandler">A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a  <see cref="Stream"/> for reading the response content.</returns>
	Task<Stream> RequestStream( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and returns the response content as a <see cref="Stream"/>.
	/// </summary>
	/// <param name="requestCreationHandler">A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains a  <see cref="Stream"/> for reading the response content.</returns>
	Task<Stream> RequestStream( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends HTTP requests created by the specified handler and asynchronously returns the responses as an enumerated sequence of type T.
	/// </summary>
	/// <typeparam name="T">The type of the elements returned in the asynchronous sequence.</typeparam>
	/// <param name="requestCreationHandler">A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>An asynchronous sequence of elements of type T representing the results of each HTTP request.</returns>
	IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken );

	/// <summary>
	/// Sends HTTP requests created by the specified handler and asynchronously returns the responses as an enumerated sequence of type T.
	/// </summary>
	/// <typeparam name="T">The type of the elements returned in the asynchronous sequence.</typeparam>
	/// <param name="requestCreationHandler">A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the asynchronous operation.</param>
	/// <returns>An asynchronous sequence of elements of type T representing the results of each HTTP request.</returns>
	IAsyncEnumerable<T> RequestEnumerated<T>( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, [EnumeratorCancellation] CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and asynchronously retrieves the response content as a <see langword="byte"/> array.
	/// </summary>
	/// <param name="requestCreationHandler">>A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the response content as a <see langword="byte"/> array.</returns>
	Task<byte[]> RequestBytes( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and asynchronously retrieves the response content as a <see langword="byte"/> array.
	/// </summary>
	/// <param name="requestCreationHandler">>A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the response content as a <see langword="byte"/> array.</returns>
	Task<byte[]> RequestBytes( [NotNull] Func<IObjectSerializer, CancellationToken, HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and asynchronously retrieves the response content as binary serialized data of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to which the binary response content is deserialized or converted.</typeparam>
	/// <param name="requestCreationHandler">A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the response content as an instance of type T.</returns>
	Task<T> RequestBinary<T>( [NotNull] Func<HttpRequestMessage> requestCreationHandler, CancellationToken cancellationToken );

	/// <summary>
	/// Sends an HTTP request created by the specified handler and asynchronously retrieves the response content as binary serialized data of the specified type.
	/// </summary>
	/// <typeparam name="T">The type to which the binary response content is deserialized or converted.</typeparam>
	/// <param name="requestCreationHandler">A delegate that creates and returns the HTTP request message to be sent.</param>
	/// <param name="cancellationToken">A cancellation token that can be used to cancel the request operation.</param>
	/// <returns>A task that represents the asynchronous operation. The task result contains the response content as an instance of type T.</returns>
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
