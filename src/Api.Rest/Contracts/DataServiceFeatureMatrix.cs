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
	public class DataServiceFeatureMatrix : FeatureMatrix
	{
		#region members

		// Deleting of measurements for sub parts is possible if server supports at least this version
		public static readonly Version DeleteMeasurementsForSubPartsMinVersion = new Version( 1, 2 );

		// Creating version entries on creating parts or characteristics is possible if server supports at least this version
		public static readonly Version CreateVersionEntriesOnCreatinPartsOrCharacteristicsMinVersion = new Version( 1, 2 );

		// Checking if an attribute with a given value exists is possible if server supports at least this version
		public static readonly Version CheckAttributeUsageMinVersion = new Version( 1, 2 );

		// Fetching a list of characteristics via uuid restriction is possible if server supports at least this version
		public static readonly Version CharacteristicUuidRestrictionForCharacteristicFetchMinVersion = new Version( 1, 3 );

		// Restrict a measurement search by merge attributes is possible if server supports at least this version
		public static readonly Version RestrictMeasurementSearchByMergeAttributesMinVersion = new Version( 1, 2 );

		// Restrict a measurement search by distinct attributes is possible if server supports at least this version
		public static readonly Version DistinctMeasurementAttributsValuesSearchMinVersion = new Version( 1, 2 );

		// Restrict a measurement search by merge master part if server supports at least this version
		public static readonly Version RestrictMeasurementSearchByMergeMasterPartMinVersion = new Version( 1, 4 );

		// Clearing a part
		public static readonly Version ClearPartMinVersion = new Version( 1, 5 );

		// Request asynchronous deletion of measurements and check status with long polling.
		public static readonly Version AsyncMeasurementDeletionMinVersion = new Version( 1, 7 );

		// Request using a limit per part when fetching measurements.
		public static readonly Version LimitResultPerPartMinVersion = new Version( 1, 8 );

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DataServiceFeatureMatrix"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="interfaceVersionRange"/> is <see langword="null" />.</exception>
		public DataServiceFeatureMatrix( [NotNull] InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{ }

		#endregion

		#region properties

		public bool SupportsDeleteMeasurementsForSubParts => CurrentInterfaceVersion >= DeleteMeasurementsForSubPartsMinVersion;

		public bool SupportsCreateVersionEntriesOnCreatinPartsOrCharacteristics => CurrentInterfaceVersion >= CreateVersionEntriesOnCreatinPartsOrCharacteristicsMinVersion;

		public virtual bool SupportsCheckAttributeUsage => CurrentInterfaceVersion >= CheckAttributeUsageMinVersion;

		public bool SupportsCharacteristicUuidRestrictionForCharacteristicFetch => CurrentInterfaceVersion >= CharacteristicUuidRestrictionForCharacteristicFetchMinVersion;

		public bool SupportsRestrictMeasurementSearchByMergeAttributes => CurrentInterfaceVersion >= RestrictMeasurementSearchByMergeAttributesMinVersion;

		public bool SupportsDistinctMeasurementAttributeValuesSearch => CurrentInterfaceVersion >= DistinctMeasurementAttributsValuesSearchMinVersion;

		public bool SupportRestrictMeasurementSearchByMergeMasterPart => CurrentInterfaceVersion >= RestrictMeasurementSearchByMergeMasterPartMinVersion;

		public bool SupportClearPart => CurrentInterfaceVersion >= ClearPartMinVersion;

		public bool SupportsAsyncMeasurementDeletion => CurrentInterfaceVersion >= AsyncMeasurementDeletionMinVersion;

		public bool SupportsLimitResultPerPart => CurrentInterfaceVersion >= LimitResultPerPartMinVersion;

		#endregion
	}
}