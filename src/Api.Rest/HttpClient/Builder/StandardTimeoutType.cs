#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Builder;

/// <summary>
/// Enumerates standard timeout values uses for rest requests.
/// </summary>
public enum StandardTimeoutType
{
	/// <summary>
	/// Timeout for standard operations: 5 minutes.
	/// </summary>
	Default,

	/// <summary>
	/// Timeout for connection checks: 5 seconds.
	/// </summary>
	ConnectionCheck,

	/// <summary>
	/// Timeout for short operations: 15 seconds.
	/// </summary>
	ShortOperation,

	/// <summary>
	/// Infinite timeout.
	/// </summary>
	Infinite
}