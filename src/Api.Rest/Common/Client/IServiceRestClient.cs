#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// Defines the interface for a service REST client that can be used to send HTTP requests to a REST service.
	/// </summary>
	public interface IServiceRestClient : IDisposable
	{
		#region events

		/// <summary>
		/// Occurs when the authentication state changes.
		/// </summary>
		event EventHandler AuthenticationChanged;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the authentication container used to manage authentication state and credentials.
		/// </summary>
		AuthenticationContainer AuthenticationContainer { get; set; }

		/// <summary>
		/// Gets the URI endpoint of the service used for network communication.
		/// </summary>
		Uri ServiceLocation { get; }

		/// <summary>
		/// Gets or sets the maximum duration to wait before the operation times out.
		/// </summary>
		TimeSpan Timeout { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the default system web proxy is used for network requests.
		/// </summary>
		bool UseDefaultWebProxy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the certificate revocation list is checked during certificate validation.
		/// </summary>
		bool CheckCertificateRevocationList { get; set; }

		#endregion
	}
}