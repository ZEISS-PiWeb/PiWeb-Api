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
		#region constants

		protected const int SupportedMajorVersion = 1;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="FeatureMatrix"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		protected FeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange )
		{
			if( interfaceVersionRange == null ) throw new ArgumentNullException( nameof( interfaceVersionRange ) );

			CurrentInterfaceVersion = GetBestKnownVersion( interfaceVersionRange );
		}

		#endregion

		#region properties

		public Version CurrentInterfaceVersion { get; }

		#endregion

		#region methods

		protected static Version GetBestKnownVersion( [NotNull] InterfaceVersionRange interfaceVersionRange )
		{
			if( interfaceVersionRange.SupportedVersions is null )
				throw new ArgumentException( "There must at least one supported version defined", nameof( interfaceVersionRange ) );

			Version bestKnownVersion = null;
			foreach( var versionCandidate in interfaceVersionRange.SupportedVersions )
			{
				var isKnownVersion = versionCandidate >= new Version( SupportedMajorVersion, 0 ) && versionCandidate < new Version( SupportedMajorVersion + 1, 0 );
				var isSuperiorToBestKnownVersion = bestKnownVersion == null || versionCandidate > bestKnownVersion;

				if( isKnownVersion && isSuperiorToBestKnownVersion )
					bestKnownVersion = versionCandidate;
			}

			if( bestKnownVersion == null )
				throw new ServerApiNotSupportedException( interfaceVersionRange );

			return bestKnownVersion;
		}

		#endregion
	}
}