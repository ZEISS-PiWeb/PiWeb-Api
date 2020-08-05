#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// Provides the minimum server version for several features.
	/// </summary>
	public class RawDataServiceFeatureMatrix : FeatureMatrix
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataServiceFeatureMatrix"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		public RawDataServiceFeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{ }

		#endregion

		#region properties

		// If the server supports at least this minor version filtering is possible.
		public static Version RawDataAttributeFilterMinVersion { get; } = new Version( SupportedMajorVersion, 2 );

		public static Version RawDataArchiveLookupMinVersion { get; } = new Version( SupportedMajorVersion, 5 );

		public bool SupportsRawDataAttributeFilter => CurrentInterfaceVersion >= RawDataAttributeFilterMinVersion;

		public bool SupportsArchiveLookup => CurrentInterfaceVersion >= RawDataArchiveLookupMinVersion;

		#endregion
	}
}