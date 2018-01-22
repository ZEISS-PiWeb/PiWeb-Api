#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data
{
	#region usings

	using System;

	#endregion

	[Flags]
	public enum DatabaseSynchronizationResult
	{
		/// <summary>
		/// Success
		/// </summary>
		Success = 0x00000000,

		/// <summary>
		/// The destination server has thrown an exception
		/// </summary>
		ServerError = 0x00000001,

		/// <summary>
		/// The server version is not supported
		/// </summary>
		InvalidServerVersion = 0x00000002,

		/// <summary>
		/// The synchronization settings are not set
		/// </summary>
		MissingSynchronizationSettings = 0x00000004,

		/// <summary>
		/// The destination url is empty or invalid
		/// </summary>
		EmptyDestinationUrl = 0x00000008,

		/// <summary>
		/// One or more path rules are empty or invalid
		/// </summary>
		InvalidPathRules = 0x00000010,

		/// <summary>
		/// The destination part path is empty or invalid
		/// </summary>
		InvalidPath = 0x00000020,

		/// <summary>
		/// One or more attributes are mapped twice
		/// </summary>
		InvalidAttributeMapping = 0x00000040,

		/// <summary>
		/// The destination part path was not available, and creating was disabled.
		/// </summary>
		UnavailablePath = 0x00000080,

		/// <summary>
		/// The synchronization settings are invalid
		/// </summary>
		InvalidSynchronizationSettings = EmptyDestinationUrl | InvalidPathRules | InvalidPath | InvalidAttributeMapping,

		/// <summary>
		/// The synchronization settings are not set or invalid
		/// </summary>
		SynchronizationSettingsError = InvalidSynchronizationSettings | MissingSynchronizationSettings
	}
}