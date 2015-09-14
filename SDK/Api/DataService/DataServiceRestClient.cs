#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Threading;
	using System.Threading.Tasks;
	
	using Common.Client;
	using Common.Data;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based data service.
	/// </summary>
	public class DataServiceRestClient : RestClient
	{
		#region constructor

		/// <summary> 
		/// Constructor. Initialization of the client class for communicating with the DataService via the given <paramref name="serverUri"/>
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		public DataServiceRestClient( string serverUri = "http://127.0.0.1:8080/" )
			: base( new Uri( serverUri ), "DataServiceRest/" ) { }

		/// <summary>
		/// Constructor. Initilization of the client class for communicating with the DataService via the given <paramref name="serverUri"/>
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		public DataServiceRestClient( Uri serverUri )
			: base( serverUri, "DataServiceRest/" ) { }

		/// <summary> 
		/// Constructor. Initialization of the client class for communicating with the DataService via the given parameters.
		/// </summary>
		/// <param name="scheme">PiWeb Server's schema (http/https)</param>
		/// <param name="host">PiWeb Server's host address</param>
		/// <param name="port">PiWeb Server's port</param>
		/// <param name="instance">An additional path wich is added to the uri, e.g. needed for an instance identifier.</param>
		public DataServiceRestClient( string scheme, string host, int port, string instance = null )
			: base( new UriBuilder( scheme, host, port, instance ).Uri, "DataServiceRest/" ) { }

		#endregion

		#region ServiceInformation

		/// <summary> 
		/// Method for fetching the <see cref="ServiceInformation"/>. This method can be used for connection checking. The call returns quickly 
		/// and does not produce any noticeable server load. 
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<ServiceInformation> GetServiceInformation( CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Get<ServiceInformation>( "serviceInformation", cancellationToken );
		}

		#endregion

		#region Configuration

		/// <summary> 
		/// Method for fetching the <see cref="Configuration"/>. The <see cref="Configuration"/> contains the 
		/// attribute definitions for parts, characteristics, measurements and values etc.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<Configuration> GetConfiguration( CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Get<Configuration>( "configuration", cancellationToken );
		}

		/// <summary>
		/// Adds a new attribute definition to the database configuration for the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity the attribute definition should be added to.</param>
		/// <param name="definition">The attribute definition to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateAttributeDefinition( Entity entity, AbstractAttributeDefinition definition, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return CreateAttributeDefinitions( entity, new[] { definition }, cancellationToken );
		}

		/// <summary>
		/// Adds new attribute definitions to the database configuration for the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be added to.</param>
		/// <param name="definitions">The attribute definitions to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateAttributeDefinitions( Entity entity, AbstractAttributeDefinition[] definitions, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( string.Format( "configuration/{0}", entity ), definitions, cancellationToken );
		}

		/// <summary> 
		/// Updates / replaces the attribute definitions for the specified <paramref name="entity"/>. If the definition
		/// does not exist yet, it will be replaced otherwise it will be updated.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be replaced for.</param>
		/// <param name="definitions">The attribute definitions to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateAttributeDefinition( Entity entity, AbstractAttributeDefinition[] definitions, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Put( string.Format( "configuration/{0}", entity ), definitions, cancellationToken );
		}

		/// <summary>
		/// Deletes the attribute definitions from the database configuration for the specified <paramref name="entity"/>. If the key
		/// list is empty, all definitions for the entity are deleted.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be deleted from.</param>
		/// <param name="keys">The keys that specify the definitions.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAttributeDefinitions( Entity entity, ushort[] keys = null, CancellationToken cancellationToken = default(CancellationToken) )
		{
			if( keys != null && keys.Length > 0 )
				return Delete( string.Format( "configuration/{0}/{1}", entity, RestClientHelper.ConvertUInt16ListToString( keys ) ), cancellationToken );

			return Delete( string.Format( "configuration/{0}", entity ), cancellationToken );
		}

		/// <summary>
		/// Deletes all attribute definitions from the database configuration.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllAttributeDefinitions( CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( "configuration", cancellationToken );
		}

		#endregion

		#region Catalogs

		/// <summary> 
		/// Method for fetching the <see cref="Catalog"/>.  The catalogs contain the definition 
		/// and the catalog entries.
		/// </summary>
		/// <param name="catalogUuids">A list of catalog uuids to restrict the result. If this list is empty, all catalogs will be returned</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<Catalog[]> GetCatalogs( Guid[] catalogUuids = null, CancellationToken cancellationToken = default(CancellationToken) )
		{
			if( catalogUuids != null && catalogUuids.Length > 0 )
				return Get<Catalog[]>( string.Format( "catalogs/{0}", RestClientHelper.ConvertGuidListToString( catalogUuids ) ), cancellationToken );

			return Get<Catalog[]>( "catalogs", cancellationToken );
		}

		/// <summary>
		/// Adds the specified catalogs to the database. If the catalog contains entries, the entries will be added too.
		/// </summary>
		/// <param name="catalogs">The catalogs to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateCatalogs( Catalog[] catalogs, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( "catalogs", catalogs, cancellationToken );
		}

		/// <summary> 
		/// Updates the specified catalogs. If the catalog contains entries, the entries will be updated too.
		/// </summary>
		/// <param name="catalogs">The catalogs to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateCatalogs( Catalog[] catalogs, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Put( "catalogs", catalogs, cancellationToken );
		}

		/// <summary> 
		/// Deletes the specified catalogs including their entries from the database. If the parameter <paramref name="catalogUuids"/>
		/// is empty, all catalogs will be deleted.
		/// </summary>
		/// <param name="catalogUuids">The catalog uuids to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteCatalogs( Guid[] catalogUuids = null, CancellationToken cancellationToken = default(CancellationToken) )
		{
			if( catalogUuids != null && catalogUuids.Length > 0 )
				return Delete( string.Format( "catalogs/{0}", RestClientHelper.ConvertGuidListToString( catalogUuids ) ), cancellationToken );

			return Delete( "catalogs", cancellationToken );
		}

		/// <summary> 
		/// Adds the specified catalog entry to the catalog with uuid <paramref name="catalogUuid"/>. If the key <see cref="CatalogEntry.Key"/>
		/// is <code>-1</code>, the server will generate a new unique key for that entry.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to add the entry to.</param>
		/// <param name="entry">The catalog entry to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateCatalogEntry( Guid catalogUuid, CatalogEntry entry, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return CreateCatalogEntries( catalogUuid, new[] { entry }, cancellationToken );
		}

		/// <summary>
		/// Adds the specified catalog entries to the catalog with uuid <paramref name="catalogUuid"/>. If the key <see cref="CatalogEntry.Key"/>
		/// is <code>-1</code>, the server will generate a new unique key for that entry.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to add the entries to.</param>
		/// <param name="entries">The catalog entries to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateCatalogEntries( Guid catalogUuid, CatalogEntry[] entries, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( string.Format( "catalogs/{0}", catalogUuid ), entries, cancellationToken );
		}

		/// <summary> 
		/// Removes the catalog entry with the specified <paramref name="key"/> from the catalog <paramref name="catalogUuid"/>.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to remove the entry from.</param>
		/// <param name="key">The key of the catalog entry to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteCatalogEntry( Guid catalogUuid, ushort key, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return Delete( string.Format( "catalogs/{0}/{1}", catalogUuid, key ), cancellationToken );
		}

		/// <summary> 
		/// Removes the catalog entries with the specified <paramref name="keys"/> from the catalog <paramref name="catalogUuid"/>. If the list of keys
		/// is empty, all catalog entries will be removed.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to remove the entries from.</param>
		/// <param name="keys">The keys of the catalog entries to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteCatalogEntries( Guid catalogUuid, ushort[] keys = null, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( string.Format( "catalogs/{0}/{1}", catalogUuid, RestClientHelper.ConvertUInt16ListToString( keys ) ), cancellationToken );
		}

		#endregion

		#region Parts

		/// <summary>
		/// Fetches a list of parts below <paramref name="partPath"/>. The result list always contains the specified parent part too. If the parent part
		/// is <code>null</code>, a server wide search is performed. If the <see paramref="depth"/> is <code>0</code>, only the specified part will be returned.
		/// </summary>
		/// <param name="partPath">The parent part to fetch the children for.</param> 
		/// <param name="withHistory">Determines whether to return the version history for each part.</param>
		/// <param name="depth">The depth for the inspection plan search.</param>
		/// <param name="partUuids">The list of part uuids to restrict the search to.</param>
		/// <param name="requestedPartAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="streamed">
		/// This controls whether to choose a streamed transfer mode or not. Using streamed mode has the side effect, that the result is transfered 
		/// using http/s when the caller enumerates the result. The caller should be aware of because then enumerating might take longer than expected.
		/// Non streamed transfer mode first reads the whole result inside the task and then returns an enumerator over the buffered result. This is
		/// the preferred way when calling the task from UI code or when enumerating the whole result. The streamed mode would be preferred when the 
		/// result is processed in blocks on non UI code or when not the complete result set is used.
		/// </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<IEnumerable<InspectionPlanPart>> GetParts( PathInformation partPath = null, Guid[] partUuids = null, ushort? depth = null, AttributeSelector requestedPartAttributes = null, bool withHistory = false, bool streamed = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = RestClientHelper.ParseToParameter( partPath, partUuids, depth, requestedPartAttributes, withHistory: withHistory );

			if( streamed )
				return await GetEnumerated<InspectionPlanPart>( "parts", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );

			return await Get<InspectionPlanPart[]>( "parts", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
		}

		/// <summary>
		/// Fetches a single part by its uuid.
		/// </summary>
		/// <param name="partUuid">The part's uuid</param>
		/// <param name="withHistory">Determines whether to return the version history for the part.</param>
		/// <param name="requestedPartAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<InspectionPlanPart> GetPartByUuid( Guid partUuid, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = RestClientHelper.ParseToParameter( requestedPartAttributes: requestedPartAttributes, withHistory: withHistory );
			return await Get<InspectionPlanPart>( String.Format( "parts/{0}", partUuid ), cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
		}

		/// <summary>
		/// Adds the specified parts to the database.
		/// </summary>
		/// <param name="parts">The parts to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateParts( InspectionPlanPart[] parts, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( "parts", parts, cancellationToken );
		}

		/// <summary>
		/// Updates the specified parts in the database.
		/// </summary>
		/// <param name="parts">The parts to update.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateParts( InspectionPlanPart[] parts, bool versioningEnabled = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = ParameterDefinition.Create( "versioningEnabled", versioningEnabled.ToString() );
			return Put( "parts", parts, cancellationToken, parameter );
		}

		/// <summary> 
		/// Deletes all parts from the database. Since parts act as the parent of characteristics and measurements, this call will 
		/// delete all characteristics and measurements including the measurement values too.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllParts( CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( "parts", cancellationToken );
		}
		
		/// <summary> 
		/// Deletes all parts and child parts below <paramref name="partPath"/> from the database. Since parts act as the parent 
		/// of characteristics and measurements, this call will delete all characteristics and measurements including the measurement 
		/// values that are a child of the specified parent part.
		/// </summary>
		/// <param name="partPath">The parent part for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteParts( PathInformation partPath, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( string.Format( "parts?partPath={0}", PathHelper.PathInformation2String( partPath ) ), cancellationToken );
		}

		/// <summary> 
		/// Deletes all parts and child parts below the parts specified by <paramref name="partUuids"/> from the database. Since parts act as the parent 
		/// of characteristics and measurements, this call will delete all characteristics and measurements including the measurement 
		/// values that are a child of the specified parent part.
		/// </summary>
		/// <param name="partUuids">The uuid list of the parent part for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteParts( Guid[] partUuids, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( string.Format( "parts?partUuids={0}", RestClientHelper.ConvertGuidListToString( partUuids ) ), cancellationToken );
		}

		#endregion

		#region Characteristics

		/// <summary>
		/// Fetches a list of characteristics below <paramref name="partPath"/>. If the parent part is <code>null</code> the characteristics below
		/// the root part will be returned. The search can be restricted using the various filter parameters. If the <see paramref="depth"/> is 
		/// <code>0</code>, only the specified part will be returned.
		/// </summary>
		/// <param name="partPath">The parent part to fetch the children for.</param> 
		/// <param name="withHistory">Determines whether to return the version history for each characteristic.</param>
		/// <param name="depth">The depth for the inspection plan search.</param>
		/// <param name="partUuids">The list of part uuids to restrict the search to.</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="streamed">
		/// This controls whether to choose a streamed transfer mode or not. Using streamed mode has the side effect, that the result is transfered 
		/// using http/s when the caller enumerates the result. The caller should be aware of because then enumerating might take longer than expected.
		/// Non streamed transfer mode first reads the whole result inside the task and then returns an enumerator over the buffered result. This is
		/// the preferred way when calling the task from UI code or when enumerating the whole result. The streamed mode would be preferred when the 
		/// result is processed in blocks on non UI code or when not the complete result set is used.
		/// </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<IEnumerable<InspectionPlanCharacteristic>> GetCharacteristics( PathInformation partPath = null, Guid[] partUuids = null, ushort? depth = null, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, bool streamed = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = RestClientHelper.ParseToParameter( partPath, partUuids, depth, requestedCharacteristicAttributes, withHistory: withHistory );

			if( streamed )
				return await GetEnumerated<InspectionPlanCharacteristic>( "characteristics", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );

			return await Get<InspectionPlanCharacteristic[]>( "characteristics", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
		}

		/// <summary>
		/// Fetches a single characteristic by its uuid.
		/// </summary>
		/// <param name="charUuid">The characteristic's uuid</param>
		/// <param name="withHistory">Determines whether to return the version history for the characteristic.</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<InspectionPlanCharacteristic> GetCharacteristicByUuid( Guid charUuid, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = RestClientHelper.ParseToParameter( requestedCharacteristicAttributes: requestedCharacteristicAttributes, withHistory: withHistory );
			return await Get<InspectionPlanCharacteristic>( string.Format( "characteristics/{0}", charUuid ), cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
		}

		/// <summary>
		/// Adds the specified characteristics to the database.
		/// </summary>
		/// <param name="characteristics">The characteristics to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateCharacteristics( InspectionPlanCharacteristic[] characteristics, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( "characteristics", characteristics, cancellationToken );
		}

		/// <summary>
		/// Updates the specified characteristics in the database.
		/// </summary>
		/// <param name="characteristics">characteristics parts to update.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateCharacteristics( InspectionPlanCharacteristic[] characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = ParameterDefinition.Create( "versioningEnabled", versioningEnabled.ToString() );
			return Put( "characteristics", characteristics, cancellationToken, parameter );
		}

		/// <summary> 
		/// Deletes the characteristic <paramref name="charPath"/> and its sub characteristics from the database. 
		/// </summary>
		/// <param name="charPath">The characteristic path for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteCharacteristics( PathInformation charPath, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( string.Format( "characteristics?charPath={0}", PathHelper.PathInformation2String( charPath ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes the characteristics <paramref name="charUuid"/> and their sub characteristics from the database. 
		/// </summary>
		/// <param name="charUuid">The characteristic uuid list for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteCharacteristics( Guid[] charUuid, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( string.Format( "characteristics?charUuids={0}", RestClientHelper.ConvertGuidListToString( charUuid ) ), cancellationToken );
		}

		#endregion

		#region Measurements

		/// <summary>
		/// Fetches a list of measurements for the <paramref name="partPath"/>. The search operation can be parameterized using the specified 
		/// <paramref name="filter"/>. If the filter is empty, all measurements for the specified part will be fetched.
		/// </summary>
		/// <param name="partPath">The part path to fetch the measurements for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="streamed">
		/// This controls whether to choose a streamed transfer mode or not. Using streamed mode has the side effect, that the result is transfered 
		/// using http/s when the caller enumerates the result. The caller should be aware of because then enumerating might take longer than expected.
		/// Non streamed transfer mode first reads the whole result inside the task and then returns an enumerator over the buffered result. This is
		/// the preferred way when calling the task from UI code or when enumerating the whole result. The streamed mode would be preferred when the 
		/// result is processed in blocks on non UI code or when not the complete result set is used.
		/// </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<IEnumerable<SimpleMeasurement>> GetMeasurements( PathInformation partPath = null, MeasurementFilterAttributes filter = null, bool streamed = false, CancellationToken cancellationToken = default(CancellationToken) )
		{
			var parameter = new List<ParameterDefinition>();
			if( filter != null )
				parameter.AddRange( filter.ToParameterDefinition() );

			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2String( partPath ) ) );

			if( streamed )
				return await GetEnumerated<SimpleMeasurement>( "measurements", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );

			return await Get<SimpleMeasurement[]>( "measurements", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
		}

		/// <summary>
		/// Adds the measurements parts to the database.
		/// </summary>
		/// <param name="measurements">The measurements to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateMeasurements( SimpleMeasurement[] measurements, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( "measurements", measurements, cancellationToken );
		}

		/// <summary>
		/// Updates the specified measurements in the database.
		/// </summary>
		/// <param name="measurements">The measurements to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateMeasurements( SimpleMeasurement[] measurements, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Put( "measurements", measurements, cancellationToken );
		}

		/// <summary>
		/// Deletes the measurements including the measurement values for part <paramref name="partPath"/>. The <paramref name="filter"/> can be used 
		/// to restrict the measurements. If the filter is empty, all measurements for the specified part will be deleted. If the partPath is empty,
		/// all measurements from the whole database will be deleted.
		/// </summary>
		/// <param name="partPath">The part path to delete the measurements from.</param>
		/// <param name="filter">A filter to restruct the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteMeasurements( PathInformation partPath = null, GenericSearchCondition filter = null, CancellationToken cancellationToken = default(CancellationToken) )
		{
			if( filter != null )
				return Delete( string.Format( "measurements?partPath={0}&searchCondition={1}", PathHelper.PathInformation2String( partPath ), SearchConditionParser.GenericConditionToString( filter ) ), cancellationToken );

			return Delete( string.Format( "measurements?partPath={0}", PathHelper.PathInformation2String( partPath ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes the measurements that are part of the <paramref name="measurementUuids"/> list.
		/// </summary>
		/// <param name="measurementUuids">The list of uuids of the measurements to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteMeasurements( Guid[] measurementUuids, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Delete( string.Format( "measurements?measurementUuids={0}", RestClientHelper.ConvertGuidListToString( measurementUuids ) ), cancellationToken );
		}

		#endregion

		#region Measurement Values

		/// <summary>
		/// Fetches a list of measurements and measurement values for the <paramref name="partPath"/>. The search operation 
		/// can be parameterized using the specified <paramref name="filter"/>. If the filter is empty, all measurements for 
		/// the specified part will be fetched.
		/// </summary>
		/// <param name="partPath">The part path to fetch the measurements and values for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="streamed">
		/// This controls whether to choose a streamed transfer mode or not. Using streamed mode has the side effect, that the result is transfered 
		/// using http/s when the caller enumerates the result. The caller should be aware of because then enumerating might take longer than expected.
		/// Non streamed transfer mode first reads the whole result inside the task and then returns an enumerator over the buffered result. This is
		/// the preferred way when calling the task from UI code or when enumerating the whole result. The streamed mode would be preferred when the 
		/// result is processed in blocks on non UI code or when not the complete result set is used.
		/// </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<IEnumerable<DataMeasurement>> GetMeasurementValues( PathInformation partPath = null, MeasurementValueFilterAttributes filter = null, bool streamed = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = new List<ParameterDefinition>();
			if( filter != null )
				parameter.AddRange( filter.ToParameterDefinition() );

			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2String( partPath ) ) );

			if( streamed )
				return await GetEnumerated<DataMeasurement>( "values", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );

			return await Get<DataMeasurement[]>( "values", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
		}

		/// <summary>
		/// Adds the measurements and measurement values parts to the database. Please note that no single values can be inserted or updated. Whole
		/// measurements with all values can be created or updated only.
		/// </summary>
		/// <param name="values">The measurements and values to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateMeasurementValues( DataMeasurement[] values, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Post( "values", values, cancellationToken );
		}

		/// <summary>
		/// Updates the measurements and measurement values parts to the database. Please note that no single values can be inserted or updated. Whole
		/// measurements with all values can be created or updated only.
		/// </summary>
		/// <param name="values">The measurements and values to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateMeasurementValues( DataMeasurement[] values, CancellationToken cancellationToken = default(CancellationToken) )
		{
			return Put( "values", values, cancellationToken );
		}

		#endregion
	}
}