#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	public abstract class FeatureMatrix
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureMatrix"/> class.
		/// </summary>
		/// <param name="interfaceVersionRange">The interface versions offered by the server.</param>
		/// <param name="supportedMajorVersions">The supported major versions. If no major version is defined, "1" will be used. </param>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		protected FeatureMatrix(
			[NotNull] InterfaceVersionRange interfaceVersionRange,
			params int[] supportedMajorVersions )
		{
			if( interfaceVersionRange == null ) throw new ArgumentNullException( nameof( interfaceVersionRange ) );

			if ( supportedMajorVersions.Length == 0 )
				supportedMajorVersions = new[] { 1 };

			SupportedMajorVersions = supportedMajorVersions;

			CurrentInterfaceVersion = GetBestKnownVersion( interfaceVersionRange, supportedMajorVersions );
		}

		#endregion

		#region properties

		public Version CurrentInterfaceVersion { get; }

		public IEnumerable<int> SupportedMajorVersions { get; }

		#endregion

		#region methods

		internal static Version GetBestKnownVersion(
			[NotNull] InterfaceVersionRange interfaceVersionRange,
			[NotNull] IReadOnlyCollection<int> supportedMajorVersions )
		{
			if( !interfaceVersionRange.SupportedVersions.Any() || !supportedMajorVersions.Any() )
				throw new ServerApiNotSupportedException( interfaceVersionRange, supportedMajorVersions );

			var bestKnownVersion = interfaceVersionRange.SupportedVersions
				.Where( version => supportedMajorVersions.Contains( version.Major ) )
				.Max();

			if ( bestKnownVersion != null )
				return bestKnownVersion;

			var versionsTooNew = interfaceVersionRange.SupportedVersions.All( interfaceVersion =>
				supportedMajorVersions.All( supportedMajorVersion => interfaceVersion.Major > supportedMajorVersion ) );
			if ( versionsTooNew )
				throw new ServerApiNotSupportedException( interfaceVersionRange, supportedMajorVersions, ServerApiNotSupportedReason.VersionsTooHigh );

			var versionsTooOld = interfaceVersionRange.SupportedVersions.All( interfaceVersion =>
				supportedMajorVersions.All( supportedMajorVersion => interfaceVersion.Major < supportedMajorVersion ) );
			if ( versionsTooOld )
				throw new ServerApiNotSupportedException( interfaceVersionRange, supportedMajorVersions, ServerApiNotSupportedReason.VersionsTooLow );

			throw new ServerApiNotSupportedException( interfaceVersionRange, supportedMajorVersions );
		}

		#endregion
	}
}