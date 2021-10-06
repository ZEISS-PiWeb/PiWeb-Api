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
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		protected FeatureMatrix(
			[NotNull] InterfaceVersionRange interfaceVersionRange,
			params int[] supportedMajorVersions )
		{
			if( interfaceVersionRange == null ) throw new ArgumentNullException( nameof( interfaceVersionRange ) );

			if ( supportedMajorVersions.Length == 0 )
				supportedMajorVersions = new[] { 1 };

			SupportedMajorVersions = supportedMajorVersions;

			CurrentInterfaceVersion = GetBestKnownVersion( interfaceVersionRange, SupportedMajorVersions );
		}

		#endregion

		#region properties

		public Version CurrentInterfaceVersion { get; }

		public IEnumerable<int> SupportedMajorVersions { get; }

		#endregion

		#region methods

		internal static Version GetBestKnownVersion(
			[NotNull] InterfaceVersionRange interfaceVersionRange,
			[NotNull] IEnumerable<int> supportedMajorVersions )
		{
			if( interfaceVersionRange.SupportedVersions is null )
				throw new ArgumentException( "There must at least one supported version defined.", nameof( interfaceVersionRange ) );

			var bestKnownVersion = interfaceVersionRange.SupportedVersions
				.Where( version => supportedMajorVersions.Contains( version.Major ) )
				.Max();

			if( bestKnownVersion == null )
				throw new ServerApiNotSupportedException( interfaceVersionRange );

			return bestKnownVersion;
		}

		#endregion
	}
}