#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region usings

	using System;
	using Zeiss.IMT.PiWeb.Api.Common.Client;
	using Zeiss.PiWeb.Api.Dtos;

	#endregion

	/// <summary>
	/// Provides the minimum server version for several features.
	/// </summary>
	public class RawDataServiceFeatureMatrix : FeatureMatrix
	{
		#region constructors

		public RawDataServiceFeatureMatrix( InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{
		}

		#endregion

		#region properties

		// If the server supports at least this minor version filtering is possible.
		public static Version RawDataAttributeFilterMinVersion { get; } = new Version( SupportedMajorVersion, 2 );

		public bool SupportsRawDataAttributeFilter => CurrentInterfaceVersion >= RawDataAttributeFilterMinVersion;

		#endregion
	}
}