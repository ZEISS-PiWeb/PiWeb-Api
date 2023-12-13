#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2018                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Core;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

	#endregion

	/// <summary>
	/// This interface represents a rest client for the PiWeb Server 'DataService'.
	/// </summary>
	public interface IDataServiceRestClientBase<T> where T : DataServiceFeatureMatrix
	{
		#region methods

		/// <summary>
		/// Method for fetching the <see cref="ServiceInformationDto"/>. This method can be used for connection checking. The call returns quickly
		/// and does not produce any noticeable server load.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<ServiceInformationDto> GetServiceInformation( CancellationToken cancellationToken = default );

		/// <summary>
		/// Method for fetching the <see cref="InterfaceVersionRange"/>.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<InterfaceVersionRange> GetInterfaceInformation( CancellationToken cancellationToken = default );

		Task<T> GetFeatureMatrix( CancellationToken cancellationToken = default );

		/// <summary>
		/// Method for fetching the <see cref="ConfigurationDto"/>. The <see cref="ConfigurationDto"/> contains the
		/// attribute definitions for parts, characteristics, measurements and values etc.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<ConfigurationDto> GetConfiguration( CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds new attribute definitions to the database configuration for the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be added to.</param>
		/// <param name="definitions">The attribute definitions to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateAttributeDefinitions( EntityDto entity, [NotNull] IReadOnlyCollection<AbstractAttributeDefinitionDto> definitions, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates / replaces the attribute definitions for the specified <paramref name="entity"/>. If the definition
		/// does not exist yet, it will be replaced otherwise it will be updated.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be replaced for.</param>
		/// <param name="definitions">The attribute definitions to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateAttributeDefinitions( EntityDto entity, [NotNull] IReadOnlyCollection<AbstractAttributeDefinitionDto> definitions, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the attribute definitions from the database configuration for the specified <paramref name="entity"/>. If the key
		/// list is empty, all definitions for the entity are deleted.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be deleted from.</param>
		/// <param name="keys">The keys that specify the definitions.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteAttributeDefinitions( EntityDto entity, IReadOnlyCollection<ushort> keys = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes all attribute definitions from the database configuration.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteAllAttributeDefinitions( CancellationToken cancellationToken = default );

		/// <summary>
		/// Method checks if there is any entry for attribute <paramref name="attributeKey"/> with value <paramref name="value
		/// "/>
		/// </summary>
		/// <param name="attributeKey">The attribute key which should be checked.</param>
		/// <param name="value">The value which schould be checked.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		Task<bool> CheckAttributeUsage( ushort attributeKey, string value, CancellationToken cancellationToken = default );

		/// <summary>
		/// Method checks if there is any entry for attribute <paramref name="attributeKey"/> and catalog entry with key (index) <paramref name="catalogEntryKey"/>
		/// </summary>
		/// <param name="attributeKey">The attribute key which should be checked.</param>
		/// <param name="catalogEntryKey">The key (index) of the catalog entry which should be checked.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		Task<bool> CheckCatalogEntryUsage( ushort attributeKey, short catalogEntryKey, CancellationToken cancellationToken = default );

		/// <summary>
		/// Method for fetching all <see cref="CatalogDto"/>s.  The catalogs contain the definition and the catalog entries.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<IReadOnlyList<CatalogDto>> GetAllCatalogs( CancellationToken cancellationToken = default );

		/// <summary>
		/// Method for fetching the <see cref="CatalogDto"/>.  The catalogs contain the definition and the catalog entries.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to be rteurned.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<CatalogDto> GetCatalog( Guid catalogUuid, CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds the specified catalogs to the database. If the catalog contains entries, the entries will be added too.
		/// </summary>
		/// <param name="catalogs">The catalogs to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateCatalogs( [NotNull] IReadOnlyCollection<CatalogDto> catalogs, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates the specified catalogs. If the catalog contains entries, the entries will be updated too.
		/// </summary>
		/// <param name="catalogs">The catalogs to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateCatalogs( [NotNull] IReadOnlyCollection<CatalogDto> catalogs, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the specified catalogs including their entries from the database. If the parameter <paramref name="catalogUuids"/>
		/// is empty, all catalogs will be deleted.
		/// </summary>
		/// <param name="catalogUuids">The catalog uuids to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteCatalogs( IReadOnlyCollection<Guid> catalogUuids = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds the specified catalog entries to the catalog with uuid <paramref name="catalogUuid"/>. If the key <see cref="CatalogEntryDto.Key"/>
		/// is <code>-1</code>, the server will generate a new unique key for that entry.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to add the entries to.</param>
		/// <param name="entries">The catalog entries to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateCatalogEntries( Guid catalogUuid, [NotNull] IReadOnlyCollection<CatalogEntryDto> entries, CancellationToken cancellationToken = default );

		/// <summary>
		/// Removes the catalog entries with the specified <paramref name="keys"/> from the catalog <paramref name="catalogUuid"/>. If the list of keys
		/// is empty, all catalog entries will be removed.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to remove the entries from.</param>
		/// <param name="keys">The keys of the catalog entries to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteCatalogEntries( Guid catalogUuid, IReadOnlyCollection<short> keys = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of parts below <paramref name="partPath"/>. The result list always contains the specified parent part too. If the parent part
		/// is <code>null</code>, a server wide search is performed. If the <see paramref="depth"/> is <code>0</code>, only the specified part will be returned.
		/// </summary>
		/// <param name="partPath">The parent part to fetch the children for.</param>
		/// <param name="withHistory">Determines whether to return the version history for each part.</param>
		/// <param name="depth">The depth for the inspection plan search.</param>
		/// <param name="partUuids">The list of part uuids to restrict the search to.</param>
		/// <param name="requestedPartAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		[NotNull]
		Task<IReadOnlyList<InspectionPlanPartDto>> GetParts( PathInformation partPath = null, IReadOnlyCollection<Guid> partUuids = null, ushort? depth = null, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a single part by its uuid.
		/// </summary>
		/// <param name="partUuid">The part's uuid</param>
		/// <param name="withHistory">Determines whether to return the version history for the part.</param>
		/// <param name="requestedPartAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		[NotNull]
		Task<InspectionPlanPartDto> GetPartByUuid( Guid partUuid, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds the specified parts to the database.
		/// </summary>
		/// <param name="parts">The parts to add.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry. </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateParts( [NotNull] IReadOnlyCollection<InspectionPlanPartDto> parts, bool versioningEnabled = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates the specified parts in the database.
		/// </summary>
		/// <param name="parts">The parts to update.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry. </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateParts( [NotNull] IReadOnlyCollection<InspectionPlanPartDto> parts, bool versioningEnabled = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes all parts and child parts below <paramref name="partPath"/> from the database. Since parts act as the parent
		/// of characteristics and measurements, this call will delete all characteristics and measurements including the measurement
		/// values that are a child of the specified parent part.
		/// </summary>
		/// <param name="partPath">The parent part for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteParts( [NotNull] PathInformation partPath, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes all parts and child parts below the parts specified by <paramref name="partUuids"/> from the database. Since parts act as the parent
		/// of characteristics and measurements, this call will delete all characteristics and measurements including the measurement
		/// values that are a child of the specified parent part.
		/// </summary>
		/// <param name="partUuids">The uuid list of the parent part for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteParts( IReadOnlyCollection<Guid> partUuids, CancellationToken cancellationToken = default );

		/// <summary>
		/// Clears a part by deleting:
		/// - all raw data items belonging the part specified by <paramref name="partUuid"/>,
		/// - all raw data for values of measurements belonging to parts below the part specified by <paramref name="partUuid"/>,
		/// - all values of measurements belonging to parts below the part specified by <paramref name="partUuid"/>,
		/// - all values of measurements belonging to parts the part specified by <paramref name="partUuid"/>,
		/// - all characteristics below the part specified by <paramref name="partUuid"/>,
		/// </summary>
		/// <param name="partUuid">The uuid of the part to be cleared.</param>
		/// <param name="clearPartKeepEntities">Contains entities which should not be affected on clearing part</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task ClearPart( Guid partUuid, IEnumerable<ClearPartKeepEntitiesDto> clearPartKeepEntities = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of characteristics below <paramref name="partPath"/>. Parts below <paramref name="partPath"/> are ignored.
		/// If the parent part is <code>null</code> the characteristics below the root part will be returned.
		/// The search can be restricted using the various filter parameters.
		/// If the <see paramref="depth"/> is <code>0</code>, an empty list will be returned.
		/// </summary>
		/// <param name="partPath">The parent part to fetch the children for.</param>
		/// <param name="withHistory">Determines whether to return the version history for each characteristic.</param>
		/// <param name="depth">The depth for the inspection plan search.</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		[NotNull]
		Task<IReadOnlyList<InspectionPlanCharacteristicDto>> GetCharacteristics( PathInformation partPath = null, ushort? depth = null, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches characteristics based on their <paramref name="charUuids"/>.
		/// </summary>
		/// <param name="charUuids">Uuids of the cherectersitics to be fetched</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="withHistory">Determines whether to return the version history for each characteristic.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		[NotNull]
		Task<IReadOnlyList<InspectionPlanCharacteristicDto>> GetCharacteristicsByUuids( IReadOnlyCollection<Guid> charUuids, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a single characteristic by its uuid.
		/// </summary>
		/// <param name="charUuid">The characteristic's uuid</param>
		/// <param name="withHistory">Determines whether to return the version history for the characteristic.</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		[NotNull]
		Task<InspectionPlanCharacteristicDto> GetCharacteristicByUuid( Guid charUuid, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds the specified characteristics to the database.
		/// </summary>
		/// <param name="characteristics">The characteristics to add.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateCharacteristics( [NotNull] IReadOnlyCollection<InspectionPlanCharacteristicDto> characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates the specified characteristics in the database.
		/// </summary>
		/// <param name="characteristics">characteristics parts to update.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateCharacteristics( [NotNull] IReadOnlyCollection<InspectionPlanCharacteristicDto> characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the characteristic <paramref name="charPath"/> and its sub characteristics from the database.
		/// </summary>
		/// <param name="charPath">The characteristic path for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteCharacteristics( [NotNull] PathInformation charPath, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the characteristics <paramref name="charUuid"/> and their sub characteristics from the database.
		/// </summary>
		/// <param name="charUuid">The characteristic uuid list for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteCharacteristics( IReadOnlyCollection<Guid> charUuid, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of measurements for the <paramref name="partPath"/>. The search operation can be parameterized using the specified
		/// <paramref name="filter"/>. If the filter is empty, all measurements for the specified part will be fetched.
		/// </summary>
		/// <param name="partPath">The part path to fetch the measurements for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		[NotNull]
		Task<IReadOnlyList<SimpleMeasurementDto>> GetMeasurements( PathInformation partPath = null, MeasurementFilterAttributesDto filter = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of measurement attribute values of the attribute <paramref name="key" /> for the <paramref name="partPath" />. The search operation can be parameterized using the specified
		/// <paramref name="filter" />. If the filter is empty, all values for the specified key will be fetched.
		/// </summary>
		/// <param name="key">The key for which to fetch distinct attribute values.</param>
		/// <param name="partPath">The part path to fetch the measurements for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		[NotNull]
		Task<IReadOnlyList<string>> GetDistinctMeasurementAttributeValues( ushort key, PathInformation partPath = null, DistinctMeasurementFilterAttributesDto filter = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds the measurements parts to the database.
		/// </summary>
		/// <param name="measurements">The measurements to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateMeasurements( [NotNull] IReadOnlyCollection<SimpleMeasurementDto> measurements, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates the specified measurements in the database.
		/// </summary>
		/// <param name="measurements">The measurements to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateMeasurements( [NotNull] IReadOnlyCollection<SimpleMeasurementDto> measurements, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the measurements including the measurement values for part <paramref name="partPath"/>. The <paramref name="filter"/> can be used
		/// to restrict the measurements. If the filter is empty, all measurements for the specified part will be deleted. If the partPath is empty,
		/// all measurements from the whole database will be deleted.
		/// </summary>
		/// <param name="partPath">The part path to delete the measurements from.</param>
		/// <param name="filter">A filter to restrict the delete operation.</param>
		/// <param name="aggregation">Specifies what types of measurements will be deleted (normal/aggregated measurements or both).</param>
		/// <param name="deep">Specifies if measurements of <paramref name="partPath"/> only or also measurements of sub parts will be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteMeasurementsByPartPath( PathInformation partPath = null, GenericSearchConditionDto filter = null, AggregationMeasurementSelectionDto aggregation = AggregationMeasurementSelectionDto.Default, MeasurementDeleteBehaviorDto deep = MeasurementDeleteBehaviorDto.DeleteForCurrentPartOnly, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the measurements including the measurement values for parts that uuids are within <paramref name="partUuids"/> list. The <paramref name="filter"/> can be used
		/// to restrict the measurements. If the filter is empty, all measurements for the specified parts will be deleted. If the partPath is empty,
		/// all measurements from the whole database will be deleted.
		/// </summary>
		/// <param name="partUuids">The Uuids of the parts which measurements should be deleted.</param>
		/// <param name="filter">A filter to restrict the delete operation.</param>
		/// <param name="aggregation">Specifies what types of measurements will be deleted (normal/aggregated measurements or both).</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteMeasurementsByPartUuids( IReadOnlyCollection<Guid> partUuids, GenericSearchConditionDto filter = null, AggregationMeasurementSelectionDto aggregation = AggregationMeasurementSelectionDto.Default, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes the measurements that are part of the <paramref name="measurementUuids"/> list.
		/// </summary>
		/// <param name="measurementUuids">The list of uuids of the measurements to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task DeleteMeasurementsByUuid( IReadOnlyCollection<Guid> measurementUuids, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of measurements and measurement values for the <paramref name="partPath"/>. The search operation
		/// can be parameterized using the specified <paramref name="filter"/>. If the filter is empty, all measurements for
		/// the specified part will be fetched.
		/// </summary>
		/// <param name="partPath">The part path to fetch the measurements and values for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		[NotNull]
		Task<IReadOnlyList<DataMeasurementDto>> GetMeasurementValues( PathInformation partPath = null, MeasurementValueFilterAttributesDto filter = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Adds the measurements and measurement values parts to the database. Please note that no single values can be inserted or updated. Whole
		/// measurements with all values can be created or updated only.
		/// </summary>
		/// <param name="values">The measurements and values to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task CreateMeasurementValues( [NotNull] IReadOnlyCollection<DataMeasurementDto> values, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates the measurements and measurement values parts to the database. Please note that no single values can be inserted or updated. Whole
		/// measurements with all values can be created or updated only.
		/// </summary>
		/// <param name="values">The measurements and values to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateMeasurementValues( [NotNull] IReadOnlyCollection<DataMeasurementDto> values, CancellationToken cancellationToken = default );

		#endregion
	}
}