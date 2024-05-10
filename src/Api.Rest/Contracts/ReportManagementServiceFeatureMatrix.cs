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
	/// Provides the minimum server version for several features.
	/// </summary>
	public class ReportManagementServiceFeatureMatrix : FeatureMatrix
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ReportManagementServiceFeatureMatrix"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		public ReportManagementServiceFeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{ }

		#endregion

	}
}