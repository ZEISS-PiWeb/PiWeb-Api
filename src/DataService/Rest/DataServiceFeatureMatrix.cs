#region copyright
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
// /* Carl Zeiss IMT (IZM Dresden)                    */
// /* Softwaresystem PiWeb                            */
// /* (c) Carl Zeiss 2016                             */
// /* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	#region usings

	using System;
	using Zeiss.IMT.PiWeb.Api.Common.Client;
	using Zeiss.IMT.PiWeb.Api.Common.Data;

	#endregion

	/// <summary>
	/// Provides the minimum server version for several features.
	/// </summary>
	public class DataServiceFeatureMatrix : FeatureMatrix
	{
		#region constants

		// Deleting of measuremnts for sub parts is possible if server supports at least this minor version
		public static readonly Version DeleteMeasurementsForSubPartsMinVersion = new Version( SupportedMajorVersion, 2 );

		// Creating version entries on creating parts or characteristics is possible if server supports at least this minor version
		public static readonly Version CreateVersionEntriesOnCreatinPartsOrCharacteristicsMinVersion = new Version( SupportedMajorVersion, 2 );

		// Checking if an attribute with a given value exists is possible if server supports at least this minor version
		public static readonly Version CheckAttributeUsageMinVersion = new Version( SupportedMajorVersion, 2 );

		// Fetching a list of characteristics via uuid restriction is possible if server supports at least this minor version
		public static readonly Version CharacteristicUuidRestrictionForCharacteristicFetchMinVersion = new Version( SupportedMajorVersion, 3 );

		// Restrict a measurement search by merge attributes is possible if server supports at least this minor version
		public static readonly Version RestrictMeasurementSearchByMergeAttributesMinVersion = new Version( SupportedMajorVersion, 2 );

		// Restrict a measurement search by distinct attributes is possible if server supports at least this minor version
		public static readonly Version DistinctMeasurementAttributsValuesSearchMinVersion = new Version( SupportedMajorVersion, 2 );

		//Restrict a measurement search by merge master part if server supports at least this minor version
		public static readonly Version RestrictMeasurementSearchByMergeMasterPartMinVersion = new Version( SupportedMajorVersion, 4 );

		#endregion

		#region constructor

		public DataServiceFeatureMatrix( InterfaceVersionRange interfaceVersionRange ) : base( interfaceVersionRange )
		{}

		#endregion

		#region properties

		public bool SupportsDeleteMeasurementsForSubParts => CurrentInterfaceVersion >= DeleteMeasurementsForSubPartsMinVersion;

		public bool SupportsCreateVersionEntriesOnCreatinPartsOrCharacteristics => CurrentInterfaceVersion >= CreateVersionEntriesOnCreatinPartsOrCharacteristicsMinVersion;

		public virtual bool SupportsCheckAttributeUsage => CurrentInterfaceVersion >= CheckAttributeUsageMinVersion;

		public bool SupportsCharacteristicUuidRestrictionForCharacteristicFetch => CurrentInterfaceVersion >= CharacteristicUuidRestrictionForCharacteristicFetchMinVersion;

		public bool SupportsRestrictMeasurementSearchByMergeAttributes => CurrentInterfaceVersion >= RestrictMeasurementSearchByMergeAttributesMinVersion;

		public bool SupportsDistinctMeasurementAttributeValuesSearch => CurrentInterfaceVersion >= DistinctMeasurementAttributsValuesSearchMinVersion;

		public bool SupportRestrictMeasurementSearchByMergeMasterPart => CurrentInterfaceVersion >= RestrictMeasurementSearchByMergeMasterPartMinVersion;


		#endregion
	}
}