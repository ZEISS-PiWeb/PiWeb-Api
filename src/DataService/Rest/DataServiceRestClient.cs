#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.IMT.PiWeb.Api.Common.Client;
	using Zeiss.IMT.PiWeb.Api.Common.Data;
	using Zeiss.IMT.PiWeb.Api.Common.Utilities;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based data service.
	/// </summary>
	public class DataServiceRestClient : CommonRestClientBase, IDataServiceRestClient
	{
		#region constants

		private const string EndpointName = "DataServiceRest/";

		#endregion

		#region members

		private ServiceInformation _LastValidServiceInformation;
		private DataServiceFeatureMatrix _FeatureMatrix;

		#endregion

		#region constructor

        /// <summary>
        /// Main Constructor. Initilization of the client class for communicating with the DataService via the given <paramref name="serverUri"/>
        /// </summary>
        /// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
        /// <param name="maxUriLength">The uri length limit</param>
        /// <param name="restClient">Custom implementation of RestClient</param>
        public DataServiceRestClient( Uri serverUri, int maxUriLength = RestClientBase.DefaultMaxUriLength, RestClientBase restClient = null )
			: base( restClient ?? new RestClient( serverUri, EndpointName, maxUriLength: maxUriLength ) )
		{
		}

		#endregion

		#region ServiceInformation

		/// <summary> 
		/// Method for fetching the <see cref="ServiceInformation"/>. This method can be used for connection checking. The call returns quickly 
		/// and does not produce any noticeable server load. 
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<ServiceInformation> GetServiceInformation( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var serviceInformation = await GetServiceInformationInternal( FetchBehavior.FetchAlways, cancellationToken ).ConfigureAwait( false );
			_LastValidServiceInformation = serviceInformation;

			return _LastValidServiceInformation;
		}

		/// <summary> 
		/// Method for fetching the <see cref="InterfaceVersionRange"/>.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<InterfaceVersionRange> GetInterfaceInformation( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			try
			{
				return await _RestClient.Request<InterfaceVersionRange>( RequestBuilder.CreateGet( "" ), cancellationToken ).ConfigureAwait( false );
			}
			catch( WrappedServerErrorException ex )
			{
				if( ex.StatusCode != HttpStatusCode.NotFound ) throw;

				// this call didn't exist in Version 1.0.0. We interprete the missing endpoint as Version 1.0.0 
				return new InterfaceVersionRange { SupportedVersions = new[] { new Version( 1, 0, 0 ) } };
			}
		}

		public Task<DataServiceFeatureMatrix> GetFeatureMatrix( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetFeatureMatrixInternal( FetchBehavior.FetchAlways, cancellationToken );
		}

		#endregion

		#region Configuration

		/// <summary> 
		/// Method for fetching the <see cref="Configuration"/>. The <see cref="Configuration"/> contains the 
		/// attribute definitions for parts, characteristics, measurements and values etc.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<Configuration> GetConfiguration( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request<Configuration>( RequestBuilder.CreateGet( "configuration" ), cancellationToken );
		}

		/// <summary>
		/// Adds new attribute definitions to the database configuration for the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be added to.</param>
		/// <param name="definitions">The attribute definitions to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateAttributeDefinitions( Entity entity, AbstractAttributeDefinition[] definitions, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePost( $"configuration/{entity}", Payload.Create( definitions ) ), cancellationToken );
		}

		/// <summary> 
		/// Updates / replaces the attribute definitions for the specified <paramref name="entity"/>. If the definition
		/// does not exist yet, it will be replaced otherwise it will be updated.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be replaced for.</param>
		/// <param name="definitions">The attribute definitions to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateAttributeDefinitions( Entity entity, AbstractAttributeDefinition[] definitions, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePut( $"configuration/{entity}", Payload.Create( definitions ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes the attribute definitions from the database configuration for the specified <paramref name="entity"/>. If the key
		/// list is empty, all definitions for the entity are deleted.
		/// </summary>
		/// <param name="entity">The entity the attribute definitions should be deleted from.</param>
		/// <param name="keys">The keys that specify the definitions.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteAttributeDefinitions( Entity entity, ushort[] keys = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var requestBasePath = $"configuration/{entity}";

			if( keys == null || keys.Length == 0 )
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( requestBasePath ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				//As the keys are passed as path segment within the uri target size is set to maximum length of path segemnts: 255
				foreach( var keyList in ArrayHelper.Split( keys, RestClient.MaximumPathSegmentLength, RestClientHelper.LengthOfListElementInUri ) )
				{
					var requestRestriction = $"/{RestClientHelper.ConvertUshortArrayToString( keyList )}";
					await _RestClient.Request( RequestBuilder.CreateDelete( string.Concat( requestBasePath, requestRestriction ) ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <summary>
		/// Deletes all attribute definitions from the database configuration.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllAttributeDefinitions( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreateDelete( "configuration" ), cancellationToken );
		}

		#endregion

		#region Attribute Usage

		/// <summary>
		/// Method checks if there is any entry for attribute <paramref name="attributeKey"/> with value <paramref name="value
		/// "/>
		/// </summary>
		/// <param name="attributeKey">The attribute key which should be checked.</param>
		/// <param name="value">The value which schould be checked.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		public Task<bool> CheckAttributeUsage( ushort attributeKey, string value, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return CheckAttributeUsageInternal( attributeKey, value, cancellationToken );
		}

		/// <summary>
		/// Method checks if there is any entry for attribute <paramref name="attributeKey"/> and catalog entry with index <paramref name="catalogEntryIndex"/>
		/// </summary>
		/// <param name="attributeKey">The attribute key which should be checked.</param>
		/// <param name="catalogEntryIndex">The index of the catalog entry which should be checked.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		public Task<bool> CheckCatalogEntryUsage( ushort attributeKey, int catalogEntryIndex, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( catalogEntryIndex < 0 )
				throw new InvalidOperationException( $"Unable to check catalogue entry usage. {nameof(catalogEntryIndex)} must be equal or greater than 0." );

			return CheckAttributeUsageInternal( attributeKey, catalogEntryIndex.ToString(), cancellationToken );
		}

		private async Task<bool> CheckAttributeUsageInternal( ushort attributeKey, string value, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsCheckAttributeUsage )
			{
				throw new OperationNotSupportedOnServerException(
					"Checking attribute usage is not supported by this server.",
					DataServiceFeatureMatrix.CheckAttributeUsageMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}

			try
			{
				await _RestClient.Request( RequestBuilder.CreateGet( $"attributes/{attributeKey}/\"{value}\"" ), cancellationToken ).ConfigureAwait( false );
				return true;
			}
			catch( WrappedServerErrorException e )
			{
				if( e.StatusCode == HttpStatusCode.NotFound )
					return false;

				throw;
			}
		}

		#endregion

		#region Catalogs

		/// <summary> 
		/// Method for fetching all <see cref="Catalog"/>s.  The catalogs contain the definition and the catalog entries.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<Catalog[]> GetAllCatalogs( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request<Catalog[]>( RequestBuilder.CreateGet( "catalogs" ), cancellationToken );
		}

		/// <summary> 
		/// Method for fetching the <see cref="Catalog"/>.  The catalogs contain the definition and the catalog entries.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to be rteurned.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<Catalog> GetCatalog( Guid catalogUuid, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var catalog = await _RestClient.Request<Catalog[]>( RequestBuilder.CreateGet( $"catalogs/{catalogUuid}" ), cancellationToken ).ConfigureAwait( false );
			return catalog.FirstOrDefault();
		}

		/// <summary>
		/// Adds the specified catalogs to the database. If the catalog contains entries, the entries will be added too.
		/// </summary>
		/// <param name="catalogs">The catalogs to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateCatalogs( Catalog[] catalogs, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePost( "catalogs", Payload.Create( catalogs ) ), cancellationToken );
		}

		/// <summary> 
		/// Updates the specified catalogs. If the catalog contains entries, the entries will be updated too.
		/// </summary>
		/// <param name="catalogs">The catalogs to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateCatalogs( Catalog[] catalogs, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePut( "catalogs", Payload.Create( catalogs ) ), cancellationToken );
		}

		/// <summary> 
		/// Deletes the specified catalogs including their entries from the database. If the parameter <paramref name="catalogUuids"/>
		/// is empty, all catalogs will be deleted.
		/// </summary>
		/// <param name="catalogUuids">The catalog uuids to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteCatalogs( Guid[] catalogUuids = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			const string uri = "catalogs";

			if( catalogUuids == null || catalogUuids.Length == 0 )
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( uri ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				//As the keys are passed as path segment within the uri target size is set to maximum length of path segemnts: 255
				foreach( var catUuids in ArrayHelper.Split( catalogUuids, RestClient.MaximumPathSegmentLength, RestClientHelper.LengthOfListElementInUri ) )
				{
					var restriction = $"/{RestClientHelper.ConvertGuidListToString( catUuids )}";
					await _RestClient.Request( RequestBuilder.CreateDelete( string.Concat( uri, restriction ) ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <summary>
		/// Adds the specified catalog entries to the catalog with uuid <paramref name="catalogUuid"/>. If the key <see cref="CatalogEntry.Key"/>
		/// is <code>-1</code>, the server will generate a new unique key for that entry.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to add the entries to.</param>
		/// <param name="entries">The catalog entries to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateCatalogEntries( Guid catalogUuid, CatalogEntry[] entries, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePost( $"catalogs/{catalogUuid}", Payload.Create( entries ) ), cancellationToken );
		}

		/// <summary> 
		/// Removes the catalog entries with the specified <paramref name="keys"/> from the catalog <paramref name="catalogUuid"/>. If the list of keys
		/// is empty, all catalog entries will be removed.
		/// </summary>
		/// <param name="catalogUuid">The uuid of the catalog to remove the entries from.</param>
		/// <param name="keys">The keys of the catalog entries to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteCatalogEntries( Guid catalogUuid, int[] keys = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var uri = $"catalogs/{catalogUuid}";

			if( keys == null || keys.Length == 0 )
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( uri ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				foreach( var keyList in ArrayHelper.Split( keys, RestClient.MaximumPathSegmentLength, RestClientHelper.LengthOfListElementInUri ) )
				{
					var restriction = $"/{RestClientHelper.ConvertIntArrayToString( keyList )}";
					await _RestClient.Request( RequestBuilder.CreateDelete( string.Concat( uri, restriction ) ), cancellationToken ).ConfigureAwait( false );
				}
			}
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
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<IEnumerable<InspectionPlanPart>> GetParts( PathInformation partPath = null, Guid[] partUuids = null, ushort? depth = null, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( partUuids != null && partUuids.Length > 0 )
			{
				var result = new List<InspectionPlanPart>( partUuids.Length );
				foreach( var uuid in partUuids )
				{
					var inspectionPlanPart = await GetPartByUuid( uuid, requestedPartAttributes, withHistory, cancellationToken ).ConfigureAwait( false );
					if( inspectionPlanPart != null )
						result.Add( inspectionPlanPart );
				}
				return result;
			}

			var parameter = RestClientHelper.ParseToParameter( partPath, partUuids, null, depth, requestedPartAttributes, withHistory: withHistory );
			return await _RestClient.Request<InspectionPlanPart[]>( RequestBuilder.CreateGet( "parts", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		/// Fetches a single part by its uuid.
		/// </summary>
		/// <param name="partUuid">The part's uuid</param>
		/// <param name="withHistory">Determines whether to return the version history for the part.</param>
		/// <param name="requestedPartAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<InspectionPlanPart> GetPartByUuid( Guid partUuid, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = RestClientHelper.ParseToParameter( requestedPartAttributes: requestedPartAttributes, withHistory: withHistory );
			return await _RestClient.Request<InspectionPlanPart>( RequestBuilder.CreateGet( $"parts/{partUuid}", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );

		}

		/// <summary>
		/// Adds the specified parts to the database.
		/// </summary>
		/// <param name="parts">The parts to add.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry. </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task CreateParts( InspectionPlanPart[] parts, bool versioningEnabled = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( versioningEnabled )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsCreateVersionEntriesOnCreatinPartsOrCharacteristics )
				{
					throw new OperationNotSupportedOnServerException(
						"Creating a new inspection plan version entry is not supported by this server.",
						DataServiceFeatureMatrix.DeleteMeasurementsForSubPartsMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
				await _RestClient.Request( RequestBuilder.CreatePost( "parts", Payload.Create( parts ), ParameterDefinition.Create( "versioningEnabled", bool.TrueString ) ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				await _RestClient.Request( RequestBuilder.CreatePost( "parts", Payload.Create( parts ) ), cancellationToken ).ConfigureAwait( false );
			}
		}

		/// <summary>
		/// Updates the specified parts in the database.
		/// </summary>
		/// <param name="parts">The parts to update.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry. </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateParts( InspectionPlanPart[] parts, bool versioningEnabled = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = ParameterDefinition.Create( "versioningEnabled", versioningEnabled.ToString() );
			return _RestClient.Request( RequestBuilder.CreatePut( "parts", Payload.Create( parts ), parameter ), cancellationToken );
		}

		/// <summary> 
		/// Deletes all parts and child parts below <paramref name="partPath"/> from the database. Since parts act as the parent 
		/// of characteristics and measurements, this call will delete all characteristics and measurements including the measurement 
		/// values that are a child of the specified parent part.
		/// </summary>
		/// <param name="partPath">The parent part for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteParts( PathInformation partPath, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = ParameterDefinition.Create( "partPath", PathHelper.PathInformation2DatabaseString( partPath ) );
			return _RestClient.Request( RequestBuilder.CreateDelete( "parts", parameter ), cancellationToken );
		}

		/// <summary> 
		/// Deletes all parts and child parts below the parts specified by <paramref name="partUuids"/> from the database. Since parts act as the parent 
		/// of characteristics and measurements, this call will delete all characteristics and measurements including the measurement 
		/// values that are a child of the specified parent part.
		/// </summary>
		/// <param name="partUuids">The uuid list of the parent part for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteParts( Guid[] partUuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			const string requestPath = "parts";
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestPath, MaxUriLength, ParameterDefinition.Create( "partUuids", "{}" ) );

			foreach( var uuidList in ArrayHelper.Split( partUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				var uuidListParameter = ParameterDefinition.Create( "partUuids", RestClientHelper.ConvertGuidListToString( uuidList ) );
				await _RestClient.Request( RequestBuilder.CreateDelete( requestPath, uuidListParameter ), cancellationToken ).ConfigureAwait( false );
			}
		}

		#endregion

		#region Characteristics

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
		public async Task<IEnumerable<InspectionPlanCharacteristic>> GetCharacteristics( PathInformation partPath = null, ushort? depth = null, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = RestClientHelper.ParseToParameter( partPath, null, null, depth, null, requestedCharacteristicAttributes, withHistory );
			return await _RestClient.Request<InspectionPlanCharacteristic[]>( RequestBuilder.CreateGet( "characteristics", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		/// Fetches characteristics based on their <paramref name="charUuids"/>.
		/// </summary>
		/// <param name="charUuids">Uuids of the cherectersitics to be fetched</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="withHistory">Determines whether to return the version history for each characteristic.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		public async Task<IEnumerable<InspectionPlanCharacteristic>> GetCharacteristicsByUuids( Guid[] charUuids, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if(charUuids == null || charUuids.Length == 0) return new InspectionPlanCharacteristic[0];

			var result = new List<InspectionPlanCharacteristic>( charUuids.Length );

			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsCharacteristicUuidRestrictionForCharacteristicFetch )
			{
				foreach( var uuid in charUuids )
				{
					var inspectionPlanCharacteristic = await GetCharacteristicByUuid( uuid, requestedCharacteristicAttributes, withHistory, cancellationToken ).ConfigureAwait( false );
					if(inspectionPlanCharacteristic != null)
						result.Add( inspectionPlanCharacteristic );
				}
			}
			else
			{
				const string requestPath = "characteristics";
				var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestPath, MaxUriLength, ParameterDefinition.Create( "charUuids", "{}" ) );

				foreach( var uuidList in ArrayHelper.Split( charUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
				{
					var parameter = RestClientHelper.ParseToParameter( null, null, uuidList, null, null, requestedCharacteristicAttributes, withHistory );
					var characteristics = await _RestClient.Request<InspectionPlanCharacteristic[]>( RequestBuilder.CreateGet( "characteristics", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
					result.AddRange( characteristics );
				}
			}

			return result;
		}

		/// <summary>
		/// Fetches a single characteristic by its uuid.
		/// </summary>
		/// <param name="charUuid">The characteristic's uuid</param>
		/// <param name="withHistory">Determines whether to return the version history for the characteristic.</param>
		/// <param name="requestedCharacteristicAttributes">The attribute selector to determine which attributes to return.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<InspectionPlanCharacteristic> GetCharacteristicByUuid( Guid charUuid, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = RestClientHelper.ParseToParameter( requestedCharacteristicAttributes: requestedCharacteristicAttributes, withHistory: withHistory );
			return await _RestClient.Request<InspectionPlanCharacteristic>( RequestBuilder.CreateGet( $"characteristics/{charUuid}", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		/// Adds the specified characteristics to the database.
		/// </summary>
		/// <param name="characteristics">The characteristics to add.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task CreateCharacteristics( InspectionPlanCharacteristic[] characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( versioningEnabled )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsCreateVersionEntriesOnCreatinPartsOrCharacteristics )
				{
					throw new OperationNotSupportedOnServerException(
						"Creating a new inspection plan version entry is not supported by this server.",
						DataServiceFeatureMatrix.DeleteMeasurementsForSubPartsMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
				await _RestClient.Request( RequestBuilder.CreatePost( "characteristics", Payload.Create( characteristics ), ParameterDefinition.Create( "versioningEnabled", bool.TrueString ) ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				await _RestClient.Request( RequestBuilder.CreatePost( "characteristics", Payload.Create( characteristics ) ), cancellationToken ).ConfigureAwait( false );
			}
		}

		/// <summary>
		/// Updates the specified characteristics in the database.
		/// </summary>
		/// <param name="characteristics">characteristics parts to update.</param>
		/// <param name="versioningEnabled">Specifies whether to create a new inspection plan version entry.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateCharacteristics( InspectionPlanCharacteristic[] characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = ParameterDefinition.Create( "versioningEnabled", versioningEnabled.ToString() );
			return _RestClient.Request( RequestBuilder.CreatePut( "characteristics", Payload.Create( characteristics ), parameter ), cancellationToken );
		}

		/// <summary> 
		/// Deletes the characteristic <paramref name="charPath"/> and its sub characteristics from the database. 
		/// </summary>
		/// <param name="charPath">The characteristic path for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteCharacteristics( PathInformation charPath, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = ParameterDefinition.Create( "charPath", PathHelper.PathInformation2DatabaseString( charPath ) );
			return _RestClient.Request( RequestBuilder.CreateDelete( "characteristics", parameter ), cancellationToken );
		}

		/// <summary>
		/// Deletes the characteristics <paramref name="charUuid"/> and their sub characteristics from the database. 
		/// </summary>
		/// <param name="charUuid">The characteristic uuid list for the delete operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteCharacteristics( Guid[] charUuid, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			const string requestPath = "characteristics";
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestPath, MaxUriLength, ParameterDefinition.Create( "charUuids", "{}" ) );

			foreach( var uuidList in ArrayHelper.Split( charUuid, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				var uuidListParameter = ParameterDefinition.Create( "charUuids", RestClientHelper.ConvertGuidListToString( uuidList ) );
				await _RestClient.Request( RequestBuilder.CreateDelete( requestPath, uuidListParameter ), cancellationToken ).ConfigureAwait( false );
			}
		}

		#endregion

		#region Measurements

		/// <summary>
		/// Fetches a list of measurements for the <paramref name="partPath"/>. The search operation can be parameterized using the specified 
		/// <paramref name="filter"/>. If the filter is empty, all measurements for the specified part will be fetched.
		/// </summary>
		/// <param name="partPath">The part path to fetch the measurements for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<SimpleMeasurement[]> GetMeasurements( PathInformation partPath = null, MeasurementFilterAttributes filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( filter?.MergeAttributes?.Length > 0 )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsRestrictMeasurementSearchByMergeAttributes )
				{
					throw new OperationNotSupportedOnServerException(
						"Restricting measurement search by merge attributes is not supported by this server.",
						DataServiceFeatureMatrix.RestrictMeasurementSearchByMergeAttributesMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
			}

			if( filter?.MergeMasterPart != null )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportRestrictMeasurementSearchByMergeMasterPart )
				{
					throw new OperationNotSupportedOnServerException(
						"Restricting measurement search by merge master part is not supported by this server.",
						DataServiceFeatureMatrix.RestrictMeasurementSearchByMergeAttributesMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
			}

			const string requestPath = "measurements";

			// split multiple measurement uuids into chunks of uuids using multiple requests to avoid "Request-URI Too Long" exception
			if( filter?.MeasurementUuids?.Length > 0 )
			{
				var newFilter = filter.Clone();
				newFilter.MeasurementUuids = null;

				var parameterName = AbstractMeasurementFilterAttributes.MeasurementUuidsParamName;
				var parameterDefinitions = CreateParameterDefinitions( partPath, newFilter );

				//Split into multiple parameter sets to limit uuid parameter lenght
				var splitter = new ParameterSplitter( this, requestPath );
				var collectionParameter = CollectionParameterFactory.Create( parameterName, filter.MeasurementUuids );
				var parameterSets = splitter.SplitAndMerge( collectionParameter, parameterDefinitions );

				//Execute requests in parallel
				var requests = parameterSets
					.Select( set => RequestBuilder.CreateGet( requestPath, set.ToArray() ) )
					.Select( request => _RestClient.Request<SimpleMeasurement[]>( request, cancellationToken ) );
				var result = await Task.WhenAll( requests ).ConfigureAwait( false );

				return result.SelectMany( r => r ).ToArray();
			}

			// split multiple part uuids into chunks of uuids using multiple requests to avoid "Request-URI Too Long" exception
			if( filter?.PartUuids?.Length > 0 )
			{
				var newFilter = filter.Clone();
				newFilter.PartUuids = null;

				var parameterName = AbstractMeasurementFilterAttributes.PartUuidsParamName;
				var parameterDefinitions = CreateParameterDefinitions( partPath, newFilter );

				//Split into multiple parameter sets to limit uuid parameter lenght
				var splitter = new ParameterSplitter( this, requestPath );
				var collectionParameter = CollectionParameterFactory.Create( parameterName, filter.PartUuids );
				var parameterSets = splitter.SplitAndMerge( collectionParameter, parameterDefinitions );

				//Execute requests in parallel
				var requests = parameterSets
					.Select( set => RequestBuilder.CreateGet( requestPath, set.ToArray() ) )
					.Select( request => _RestClient.Request<SimpleMeasurement[]>( request, cancellationToken ) );
				var result = await Task.WhenAll( requests ).ConfigureAwait( false );

				return result.SelectMany( r => r ).ToArray();
			}

			{
				var parameterDefinitions = CreateParameterDefinitions( partPath, filter ).ToArray();
				var requestUrl = RequestBuilder.CreateGet( requestPath, parameterDefinitions );
				return await _RestClient.Request<SimpleMeasurement[]>( requestUrl, cancellationToken ).ConfigureAwait( false );
			}

		}

		/// <summary>
		/// Fetches a list of measurement attribute values of the attribute <paramref name="key" /> for the <paramref name="partPath" />. The search operation can be parameterized using the specified
		/// <paramref name="filter" />. If the filter is empty, all values for the specified key will be fetched.
		/// </summary>
		/// <param name="key">The key for which to fetch distinct attribute values.</param>
		/// <param name="partPath">The part path to fetch the measurements for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		public async Task<string[]> GetDistinctMeasurementAttributeValues( ushort key, PathInformation partPath = null, DistinctMeasurementFilterAttributes filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsDistinctMeasurementAttributeValuesSearch )
			{
				throw new OperationNotSupportedOnServerException(
					"Fetching distinct measurement values is not supported by this server.",
					DataServiceFeatureMatrix.DistinctMeasurementAttributsValuesSearchMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}

			if( filter?.MeasurementUuids?.Length > 0 )
			{
				var newFilter = filter.Clone();
				newFilter.MeasurementUuids = null;

				var parameter = CreateParameterDefinitions( partPath, newFilter, key );
				parameter.Add( ParameterDefinition.Create( AbstractMeasurementFilterAttributes.MeasurementUuidsParamName, "" ) );

				var requestRestriction = RequestBuilder.AppendParameters( "values", parameter );
				var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

				var result = new List<string>( filter.MeasurementUuids.Length );
				foreach( var uuids in ArrayHelper.Split( filter.MeasurementUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
				{
					newFilter.MeasurementUuids = uuids;

					var attributes = await _RestClient.Request<string[]>( RequestBuilder.CreateGet( "distinctMeasurementAttributeValues", CreateParameterDefinitions( partPath, newFilter, key ).ToArray() ), cancellationToken ).ConfigureAwait( false );
					result.AddRange( attributes );
				}
				return result.ToArray();
			}

			return await _RestClient.Request<string[]>( RequestBuilder.CreateGet( "distinctMeasurementAttributeValues", CreateParameterDefinitions( partPath, filter, key ).ToArray() ), cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		/// Adds the measurements parts to the database.
		/// </summary>
		/// <param name="measurements">The measurements to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateMeasurements( SimpleMeasurement[] measurements, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request<SimpleMeasurement[]>( RequestBuilder.CreatePost( "measurements", Payload.Create( measurements ) ), cancellationToken );
		}

		/// <summary>
		/// Updates the specified measurements in the database.
		/// </summary>
		/// <param name="measurements">The measurements to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateMeasurements( SimpleMeasurement[] measurements, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request<SimpleMeasurement[]>( RequestBuilder.CreatePut( "measurements", Payload.Create( measurements ) ), cancellationToken );
		}

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
		public async Task DeleteMeasurementsByPartPath( PathInformation partPath = null, GenericSearchCondition filter = null, AggregationMeasurementSelection aggregation = AggregationMeasurementSelection.Default, MeasurementDeleteBehavior deep = MeasurementDeleteBehavior.DeleteForCurrentPartOnly, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var parameter = new List<ParameterDefinition>();

			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2DatabaseString( partPath ) ) );

			if( filter != null )
				parameter.Add( ParameterDefinition.Create( "searchCondition", SearchConditionParser.GenericConditionToString( filter ) ) );

			if( aggregation != AggregationMeasurementSelection.Default )
				parameter.Add( ParameterDefinition.Create( "aggregation", aggregation.ToString() ) );

			if( deep == MeasurementDeleteBehavior.DeleteDeep )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsDeleteMeasurementsForSubParts )
				{
					throw new OperationNotSupportedOnServerException(
						"Deleting measurements for sub parts is not supported by this server.",
						DataServiceFeatureMatrix.DeleteMeasurementsForSubPartsMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
				parameter.Add( ParameterDefinition.Create( "deep", deep.ToString() ) );
			}

			await _RestClient.Request( RequestBuilder.CreateDelete( "measurements", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		/// Deletes the measurements including the measurement values for parts that uuids are within <paramref name="partUuids"/> list. The <paramref name="filter"/> can be used 
		/// to restrict the measurements. If the filter is empty, all measurements for the specified parts will be deleted. If the partPath is empty,
		/// all measurements from the whole database will be deleted. 
		/// </summary>
		/// <param name="partUuids">The Uuids of the parts which measurements should be deleted.</param>
		/// <param name="filter">A filter to restrict the delete operation.</param>
		/// <param name="aggregation">Specifies what types of measurements will be deleted (normal/aggregated measurements or both).</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteMeasurementsByPartUuids( Guid[] partUuids, GenericSearchCondition filter = null, AggregationMeasurementSelection aggregation = AggregationMeasurementSelection.Default, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( partUuids == null )
			{
				var parameter = new List<ParameterDefinition>();

				if( filter != null )
					parameter.Add( ParameterDefinition.Create( "searchCondition", SearchConditionParser.GenericConditionToString( filter ) ) );

				if( aggregation != AggregationMeasurementSelection.Default )
					parameter.Add( ParameterDefinition.Create( "aggregation", aggregation.ToString() ) );

				await _RestClient.Request( RequestBuilder.CreateDelete( "measurements", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				foreach( var partUuid in partUuids )
				{
					var parameter = new List<ParameterDefinition> { ParameterDefinition.Create( "partUuids", RestClientHelper.ConvertGuidListToString( new[] { partUuid } ) ) };

					if( filter != null )
						parameter.Add( ParameterDefinition.Create( "searchCondition", SearchConditionParser.GenericConditionToString( filter ) ) );

					if( aggregation != AggregationMeasurementSelection.Default )
						parameter.Add( ParameterDefinition.Create( "aggregation", aggregation.ToString() ) );

					await _RestClient.Request( RequestBuilder.CreateDelete( "measurements", parameter.ToArray() ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <summary>
		/// Deletes the measurements that are part of the <paramref name="measurementUuids"/> list.
		/// </summary>
		/// <param name="measurementUuids">The list of uuids of the measurements to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task DeleteMeasurementsByUuid( Guid[] measurementUuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( measurementUuids.Any() )
			{
				var emptyParameter = new[] { ParameterDefinition.Create( "measurementUuids", "" ) };
				var requestRestriction = RequestBuilder.AppendParameters( "measurements", emptyParameter );

				var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

				foreach( var uuids in ArrayHelper.Split( measurementUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
				{
					var parameter = ParameterDefinition.Create( "measurementUuids", RestClientHelper.ConvertGuidListToString( uuids ) );
					await _RestClient.Request( RequestBuilder.CreateDelete( "measurements", parameter ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		#endregion

		#region Measurement Values

		/// <summary>
		/// Fetches a list of measurements and measurement values for the <paramref name="partPath"/>. The search operation 
		/// can be parameterized using the specified <paramref name="filter"/>. If the filter is empty, all measurements for 
		/// the specified part will be fetched.
		/// <remarks>The <see cref="DataCharacteristic"/> objects within the returned measurements does not include the characteristics' paths due to perform issues.</remarks>
		/// </summary>
		/// <param name="partPath">The part path to fetch the measurements and values for.</param>
		/// <param name="filter">A filter that can be used to further restrict the search operation.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<DataMeasurement[]> GetMeasurementValues( PathInformation partPath = null, MeasurementValueFilterAttributes filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( filter?.MergeAttributes?.Length > 0 )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsRestrictMeasurementSearchByMergeAttributes )
				{
					throw new OperationNotSupportedOnServerException(
						"Restricting measurement search by merge attributes is not supported by this server.",
						DataServiceFeatureMatrix.RestrictMeasurementSearchByMergeAttributesMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
			}

			if( filter?.MeasurementUuids?.Length > 0 )
				return await GetMeasurementValuesSplitByMeasurement( partPath, filter, cancellationToken ).ConfigureAwait( false );

			if( filter?.CharacteristicsUuidList?.Length > 0 )
				return await GetMeasurementValuesSplitByCharacteristics( partPath, filter, cancellationToken ).ConfigureAwait( false );

			return await _RestClient.Request<DataMeasurement[]>( RequestBuilder.CreateGet( "values", CreateParameterDefinitions( partPath, filter ).ToArray() ), cancellationToken ).ConfigureAwait( false );
		}

		private async Task<DataMeasurement[]> GetMeasurementValuesSplitByMeasurement( PathInformation partPath, MeasurementValueFilterAttributes filter, CancellationToken cancellationToken )
		{
			var newFilter = filter.Clone();
			newFilter.MeasurementUuids = null;

			var parameter = CreateParameterDefinitions( partPath, newFilter );
			parameter.Add( ParameterDefinition.Create( AbstractMeasurementFilterAttributes.MeasurementUuidsParamName, "" ) );

			var requestRestriction = RequestBuilder.AppendParameters( "values", parameter );
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

			var result = new List<DataMeasurement>( filter.MeasurementUuids.Length );
			foreach( var uuids in ArrayHelper.Split( filter.MeasurementUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				newFilter.MeasurementUuids = uuids;
				if( newFilter.CharacteristicsUuidList?.Length > 0 )
				{
					result.AddRange( await GetMeasurementValuesSplitByCharacteristics( partPath, newFilter, cancellationToken ).ConfigureAwait( false ) );
				}
				else
				{
					result.AddRange( await _RestClient.Request<DataMeasurement[]>( RequestBuilder.CreateGet( "values", CreateParameterDefinitions( partPath, newFilter ).ToArray() ), cancellationToken ).ConfigureAwait( false ) );
				}
			}
			return result.ToArray();
		}

		private async Task<DataMeasurement[]> GetMeasurementValuesSplitByCharacteristics( PathInformation partPath, MeasurementValueFilterAttributes filter, CancellationToken cancellationToken )
		{
			var newFilter = filter.Clone();
			newFilter.CharacteristicsUuidList = null;

			var parameter = CreateParameterDefinitions( partPath, newFilter );
			parameter.Add( ParameterDefinition.Create( MeasurementValueFilterAttributes.CharacteristicsUuidListParamName, "" ) );

			var requestRestriction = RequestBuilder.AppendParameters( "values", parameter );
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

			var result = new List<DataMeasurement>( filter.MeasurementUuids?.Length ?? 0 );
			var allMeasurements = new Dictionary<Guid, DataMeasurement>();
			foreach( var uuids in ArrayHelper.Split( filter.CharacteristicsUuidList, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				newFilter.CharacteristicsUuidList = uuids;

				var measurements = await _RestClient.Request<DataMeasurement[]>( RequestBuilder.CreateGet( "values", CreateParameterDefinitions( partPath, newFilter ).ToArray() ), cancellationToken ).ConfigureAwait( false );
				foreach( var measurement in measurements )
				{
					DataMeasurement existingMeasurement;
					if( allMeasurements.TryGetValue( measurement.Uuid, out existingMeasurement ) )
					{
						existingMeasurement.Characteristics = Combine( existingMeasurement.Characteristics, measurement.Characteristics );
					}
					else
					{
						result.Add( measurement );
						allMeasurements.Add( measurement.Uuid, measurement );
					}
				}
			}
			return result.ToArray();
		}

		/// <summary>
		/// Adds the measurements and measurement values parts to the database. Please note that no single values can be inserted or updated. Whole
		/// measurements with all values can be created or updated only.
		/// </summary>
		/// <param name="values">The measurements and values to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task CreateMeasurementValues( DataMeasurement[] values, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePost( "values", Payload.Create( values ) ), cancellationToken );
		}

		/// <summary>
		/// Updates the measurements and measurement values parts to the database. Please note that no single values can be inserted or updated. Whole
		/// measurements with all values can be created or updated only.
		/// </summary>
		/// <param name="values">The measurements and values to update.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateMeasurementValues( DataMeasurement[] values, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return _RestClient.Request( RequestBuilder.CreatePut( "values", Payload.Create( values ) ), cancellationToken );
		}

		#endregion

		#region Helper methods

		private static DataCharacteristic[] Combine( DataCharacteristic[] list1, DataCharacteristic[] list2 )
		{
			if( list1 == null )
				return list2;
			if( list2 == null )
				return list1;

			return list1.Concat( list2 ).ToArray();
		}

		private static List<ParameterDefinition> CreateParameterDefinitions<T>( PathInformation partPath, T filter, int? key = null ) where T : AbstractMeasurementFilterAttributes
		{
			var parameter = new List<ParameterDefinition>();

			if( filter != null )
				parameter.AddRange( filter.ToParameterDefinition() );

			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2DatabaseString( partPath ) ) );

			if( key.HasValue )
				parameter.Add( ParameterDefinition.Create( "key", key.ToString() ) );

			return parameter;
		}

		private async Task<ServiceInformation> GetServiceInformationInternal( FetchBehavior behavior, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			// This is an intentional race condition. Calling this method from multiple threads may lead to multiple calls to Get<ServiceInformation>().
			// However, this would be rare and harmless, since it should always return the same result. It would be a lot more difficult to make this work without any races or blocking.
			// It is important to never set _LastValidServiceInformation to null anywhere to avoid possible null returns here due to the race condition.
			if( behavior == FetchBehavior.FetchAlways || _LastValidServiceInformation == null )
			{
				var serviceInformation = await _RestClient.Request<ServiceInformation>( RequestBuilder.CreateGet( "ServiceInformation" ), cancellationToken ).ConfigureAwait( false );
				_LastValidServiceInformation = serviceInformation;
			}

			return _LastValidServiceInformation;
		}

		private async Task<DataServiceFeatureMatrix> GetFeatureMatrixInternal( FetchBehavior behavior, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			// This is an intentional race condition. Calling this method from multiple threads may lead to multiple calls to Get<InterfaceInformation>().
			// However, this would be rare and harmless, since it should always return the same result. It would be a lot more difficult to make this work without any races or blocking.
			// It is important to never set _LastValidServiceInformation to null anywhere to avoid possible null returns here due to the race condition.
			if( behavior == FetchBehavior.FetchAlways || _FeatureMatrix == null )
			{
				var interfaceVersionRange = await GetInterfaceInformation( cancellationToken ).ConfigureAwait( false );
				_FeatureMatrix = new DataServiceFeatureMatrix( interfaceVersionRange );
			}

			return _FeatureMatrix;
		}

		#endregion
	}
}
