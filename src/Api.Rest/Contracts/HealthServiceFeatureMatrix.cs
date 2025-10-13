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
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Dtos;

	#endregion

	/// <summary>
	/// Provides the minimum server version for several features.
	/// </summary>
	public class HealthServiceFeatureMatrix : FeatureMatrix
	{
		#region members

		/// <summary>
		/// Checking if server supports at least this version for the healthCheck API.
		/// </summary>
		public static readonly Version HealthCheckMinVersion = new(1, 0);

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of this class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		public HealthServiceFeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{ }

		#endregion

		#region properties

		/// <summary>
		/// Returns <see langword="true"/> if fetching healthCheck API endpoints is possible.
		/// </summary>
		public bool SupportsHealthService => CurrentInterfaceVersion >= HealthCheckMinVersion;

		#endregion
	}
}