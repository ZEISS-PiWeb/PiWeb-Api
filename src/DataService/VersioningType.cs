#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;

	#endregion

	/// <summary>
	/// This enumeration specifies how the server is performing inspection plan versioning.
	/// </summary>
	public enum VersioningType
	{
		/// <summary>Versioning is disabled by the server. The client cannot control versioning.</summary>
		Off,

		/// <summary>Versioning is enabled by the server. The client cannot control versioning.</summary>
		On,

		/// <summary>
		/// Versioning can be controlled by the client application for each inspection plan update.
		/// </summary>
		Client
	}
}