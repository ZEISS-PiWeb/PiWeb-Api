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
		/// <summary>Versioning functionality is completly disabled.</summary>
		Off,

		/// <summary>Versioning is enabled every time.</summary>
		On,

		/// <summary>Client decides if versioning is enabled.</summary> 
		/// <comment>This means that versioning is generally enabled and the client 
		/// has the possibility to switch versioning on/off for certain changes.</comment>
		Client
	}
}