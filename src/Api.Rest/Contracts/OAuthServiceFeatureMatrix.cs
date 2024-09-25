#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
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
	/// Provides the minimum server version for features of the OAuth REST service.
	/// </summary>
	public class OAuthServiceFeatureMatrix : FeatureMatrix
	{
		#region members

		/// <summary>
		/// Fetching extended OAuth token information is possible if server supports at least this version
		/// </summary>
		public static readonly Version OAuthConfigurationMinVersion = new Version( 1, 2 );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="OAuthServiceFeatureMatrix"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		public OAuthServiceFeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{ }

		#endregion

		#region properties

		/// <summary>
		/// Returns <see langword="true"/> if fetching extended OAuth token information is possible.
		/// </summary>
		public bool SupportsOAuthConfiguration => CurrentInterfaceVersion >= OAuthConfigurationMinVersion;

		#endregion
	}
}