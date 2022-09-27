#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Net;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Contracts;
	using Zeiss.PiWeb.Api.Definitions;
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.Data;

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

		private ServiceInformationDto _LastValidServiceInformation;
		private DataServiceFeatureMatrix _FeatureMatrix;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="DataServiceRestClient"/> class.
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		/// <param name="maxUriLength">The uri length limit</param>
		/// <param name="restClient">Custom implementation of RestClient</param>
		public DataServiceRestClient( [NotNull] Uri serverUri, int maxUriLength = RestClientBase.DefaultMaxUriLength, RestClientBase restClient = null )
			: base( restClient ?? new RestClient( serverUri, EndpointName, maxUriLength: maxUriLength, serializer: ObjectSerializer.SystemTextJson ) )
		{ }

		#endregion

		#region methods

		private async Task<bool> CheckAttributeUsageInternal( ushort attributeKey, string value, CancellationToken cancellationToken = default )
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

		private async Task<IReadOnlyList<DataMeasurementDto>> GetMeasurementValuesSplitByMeasurement( PathInformation partPath, MeasurementValueFilterAttributesDto filter, CancellationToken cancellationToken )
		{
			var newFilter = filter.Clone();
			newFilter.MeasurementUuids = null;

			var parameter = CreateParameterDefinitions( partPath, newFilter );
			parameter.Add( ParameterDefinition.Create( AbstractMeasurementFilterAttributesDto.MeasurementUuidsParamName, "" ) );

			var requestRestriction = RequestBuilder.AppendParameters( "values", parameter );
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

			var result = new List<DataMeasurementDto>( filter.MeasurementUuids.Count );
			var resultSets = 0;
			foreach( var uuids in ArrayHelper.Split( filter.MeasurementUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				newFilter.MeasurementUuids = uuids;
				if( newFilter.CharacteristicsUuidList?.Count > 0 )
				{
					result.AddRange( await GetMeasurementValuesSplitByCharacteristics( partPath, newFilter, cancellationToken ).ConfigureAwait( false ) );
				}
				else
				{
					result.AddRange( await _RestClient.Request<IReadOnlyList<DataMeasurementDto>>( RequestBuilder.CreateGet( "values", CreateParameterDefinitions( partPath, newFilter ) ), cancellationToken ).ConfigureAwait( false ) );
				}

				resultSets++;
			}

			return resultSets > 1 ? LimitAndSortResult( result, filter, resultSets ) : result;
		}

		private async Task<IReadOnlyList<DataMeasurementDto>> GetMeasurementValuesSplitByCharacteristics( PathInformation partPath, MeasurementValueFilterAttributesDto filter, CancellationToken cancellationToken )
		{
			var newFilter = filter.Clone();
			newFilter.CharacteristicsUuidList = null;

			var parameter = CreateParameterDefinitions( partPath, newFilter );
			parameter.Add( ParameterDefinition.Create( MeasurementValueFilterAttributesDto.CharacteristicsUuidListParamName, "" ) );

			var requestRestriction = RequestBuilder.AppendParameters( "values", parameter );
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

			var result = new List<DataMeasurementDto>( filter.MeasurementUuids?.Count ?? 0 );
			var allMeasurements = new Dictionary<Guid, DataMeasurementDto>();
			foreach( var uuids in ArrayHelper.Split( filter.CharacteristicsUuidList, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				newFilter.CharacteristicsUuidList = uuids;

				var measurements = await _RestClient.Request<IEnumerable<DataMeasurementDto>>( RequestBuilder.CreateGet( "values", CreateParameterDefinitions( partPath, newFilter ) ), cancellationToken ).ConfigureAwait( false );
				foreach( var measurement in measurements )
				{
					if( allMeasurements.TryGetValue( measurement.Uuid, out var existingMeasurement ) )
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

			return result;
		}

		private static IReadOnlyDictionary<Guid, DataValueDto> Combine( IReadOnlyDictionary<Guid, DataValueDto> list1, IReadOnlyDictionary<Guid, DataValueDto> list2 )
		{
			if( list1 == null )
				return list2;
			if( list2 == null )
				return list1;

			var result = new Dictionary<Guid, DataValueDto>( list1.Count + list2.Count );
			foreach( var item in list1 )
			{
				result.Add( item.Key, item.Value );
			}
			foreach( var item in list2 )
			{
				result.Add( item.Key, item.Value );
			}

			return result;
		}

		private static List<ParameterDefinition> CreateParameterDefinitions<T>( PathInformation partPath, T filter, int? key = null ) where T : AbstractMeasurementFilterAttributesDto
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

		private async Task<ServiceInformationDto> GetServiceInformationInternal( FetchBehavior behavior, CancellationToken cancellationToken = default )
		{
			// This is an intentional race condition. Calling this method from multiple threads may lead to multiple calls to Get<ServiceInformation>().
			// However, this would be rare and harmless, since it should always return the same result. It would be a lot more difficult to make this work without any races or blocking.
			// It is important to never set _LastValidServiceInformation to null anywhere to avoid possible null returns here due to the race condition.
			if( behavior == FetchBehavior.FetchAlways || _LastValidServiceInformation == null )
			{
				var serviceInformation = await _RestClient.Request<ServiceInformationDto>( RequestBuilder.CreateGet( "ServiceInformation" ), cancellationToken ).ConfigureAwait( false );
				_LastValidServiceInformation = serviceInformation;
			}

			return _LastValidServiceInformation;
		}

		private async Task<DataServiceFeatureMatrix> GetFeatureMatrixInternal( FetchBehavior behavior, CancellationToken cancellationToken = default )
		{
			// This is an intentional race condition. Calling this method from multiple threads may lead to multiple calls to Get<InterfaceInformation>().
			// However, this would be rare and harmless, since it should always return the same result. It would be a lot more difficult to make this work without any races or blocking.
			// It is important to never set _FeatureMatrix to null anywhere to avoid possible null returns here due to the race condition.
			if( behavior == FetchBehavior.FetchAlways || _FeatureMatrix == null )
			{
				var interfaceVersionRange = await GetInterfaceInformation( cancellationToken ).ConfigureAwait( false );
				_FeatureMatrix = new DataServiceFeatureMatrix( interfaceVersionRange );
			}

			return _FeatureMatrix;
		}

		/// <summary>
		/// Limit and sort received measurements if the initial request was split into multiple requests due to uri length.
		/// </summary>
		/// <param name="source">Fetched measurements</param>
		/// <param name="filter">Set filter to check for LimitResult and OrderBy</param>
		/// <param name="resultSets">Number of requests after splitting the initial request</param>
		/// <returns>Array of measurements in correct order and size.</returns>
		private static IReadOnlyList<T> LimitAndSortResult<T>( IEnumerable<T> source, AbstractMeasurementFilterAttributesDto filter, int resultSets ) where T: SimpleMeasurementDto
		{
			if( resultSets <= 1 )
				return ConvertToList( source );

			if( filter.OrderBy == null || !filter.OrderBy.Any() )
			{
				return filter.LimitResult < 0
					? ConvertToList( source )
					: source.Take( filter.LimitResult ).ToArray();
			}

			var orderedResult = StartAttributeOrderChain( filter.OrderBy.First(), source );

			//Evaluate each OrderBy
			foreach( var orderDto in filter.OrderBy.Skip( 1 ) )
			{
				orderedResult = AppendAttributeOrderChain( orderDto, orderedResult );
			}

			return filter.LimitResult < 0
				? orderedResult.ToArray()
				: orderedResult.Take( filter.LimitResult ).ToArray();
		}

		private static IReadOnlyList<T> ConvertToList<T>( IEnumerable<T> source ) where T : SimpleMeasurementDto
		{
			if( source is IReadOnlyList<T> list )
				return list;

			return source.ToArray();
		}

		/// <summary>
		/// Sort the measurements according to the order criteria. Start a new OrderBy-chain.
		/// </summary>
		private static IOrderedEnumerable<T> StartAttributeOrderChain<T>( OrderDto order, IEnumerable<T> source ) where T: SimpleMeasurementDto
		{
			return order.Direction switch
			{
				OrderDirectionDto.Asc => source.OrderBy( SelectAttributeValues<T>( order ) ),
				OrderDirectionDto.Desc => source.OrderByDescending( SelectAttributeValues<T>( order ) ),
				_ => throw new ArgumentOutOfRangeException( nameof( order.Direction ) )
			};
		}

		/// <summary>
		/// Sort the measurements according to the order criteria. Chain it to a previous result.
		/// </summary>
		private static IOrderedEnumerable<T> AppendAttributeOrderChain<T>( OrderDto order, IOrderedEnumerable<T> orderedSource ) where T: SimpleMeasurementDto
		{
			return order.Direction switch
			{
				OrderDirectionDto.Asc => orderedSource.ThenBy( SelectAttributeValues<T>( order ) ),
				OrderDirectionDto.Desc => orderedSource.ThenByDescending( SelectAttributeValues<T>( order ) ),
				_ => throw new ArgumentOutOfRangeException( nameof( order.Direction ) )
			};
		}

		private static Func<T, object> SelectAttributeValues<T>( OrderDto order ) where  T: SimpleMeasurementDto
		{
			if( order.Attribute == WellKnownKeys.Measurement.Time )
				return measurement => measurement.Time;

			return measurement => measurement.GetAttributeValue( order.Attribute );
		}

		private async Task PollOperationStatus( string statusId, CancellationToken cancellationToken = default )
		{
			while( !cancellationToken.IsCancellationRequested )
			{
				var operationStatus = await _RestClient.Request<OperationStatusDto>( RequestBuilder.CreateGet( $"PendingOperations/{statusId}" ), cancellationToken ).ConfigureAwait( false );
				if( operationStatus.ExecutionStatus == OperationExecutionStatusDto.Finished )
					break;
				if( operationStatus.ExecutionStatus == OperationExecutionStatusDto.Exception )
					throw new WrappedServerErrorException( operationStatus.Exception );
			}
		}

		private async Task DeleteMeasurementsInternal( List<ParameterDefinition> parameter, CancellationToken cancellationToken )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );

			if( featureMatrix.SupportsAsyncMeasurementDeletion )
			{
				parameter.Add( ParameterDefinition.Create( "runAsync", "true" ) );
				var pollingUri = await _RestClient.RequestAsyncOperation( RequestBuilder.CreateDelete( "measurements", parameter ), cancellationToken ).ConfigureAwait( false );

				if( pollingUri != null )
				{
					var statusId = pollingUri.Segments.Last();
					await PollOperationStatus( statusId, cancellationToken ).ConfigureAwait( false );
				}
			}
			else
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( "measurements", parameter ), cancellationToken ).ConfigureAwait( false );
			}
		}

		private async Task ThrowOnUnsupportedLimitResultPerPart( AbstractMeasurementFilterAttributesDto filter, CancellationToken cancellationToken )
		{
			if( filter?.LimitResultPerPart > -1 )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsLimitResultPerPart )
				{
					throw new OperationNotSupportedOnServerException(
						"Restricting measurement search with a limit per part is not supported by this server.",
						DataServiceFeatureMatrix.LimitResultPerPartMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
			}
		}

		private async Task ThrowOnUnsupportedMergeAttributes( IReadOnlyCollection<ushort> mergeAttributes, CancellationToken cancellationToken )
		{
			if( mergeAttributes?.Count > 0 )
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
		}

		private async Task ThrowOnUnsupportedMergeMasterPart( MeasurementFilterAttributesDto filter, CancellationToken cancellationToken )
		{
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
		}

		private async Task ThrowOnUnsupportedDeleteMeasurementsForSubParts( CancellationToken cancellationToken )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsDeleteMeasurementsForSubParts )
			{
				throw new OperationNotSupportedOnServerException(
					"Deleting measurements for sub parts is not supported by this server.",
					DataServiceFeatureMatrix.DeleteMeasurementsForSubPartsMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}
		}

		private async Task ThrowOnUnsupportedDistinctMeasurementValueSearch( CancellationToken cancellationToken )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsDistinctMeasurementAttributeValuesSearch )
			{
				throw new OperationNotSupportedOnServerException(
					"Fetching distinct measurement values is not supported by this server.",
					DataServiceFeatureMatrix.DistinctMeasurementAttributsValuesSearchMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}
		}

		private async Task ThrowOnUnsupportedCreateVersionEntries( CancellationToken cancellationToken )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsCreateVersionEntriesOnCreatinPartsOrCharacteristics )
			{
				throw new OperationNotSupportedOnServerException(
					"Creating a new inspection plan version entry is not supported by this server.",
					DataServiceFeatureMatrix.CreateVersionEntriesOnCreatinPartsOrCharacteristicsMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}
		}

		private async Task ThrowOnUnsupportedClearPart( CancellationToken cancellationToken )
		{
			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportClearPart )
			{
				throw new OperationNotSupportedOnServerException(
					"Clearing a part is not supported by this server.",
					DataServiceFeatureMatrix.ClearPartMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}
		}

		#endregion

		#region interface IDataServiceRestClient

		/// <inheritdoc />
		public async Task<ServiceInformationDto> GetServiceInformation( CancellationToken cancellationToken = default )
		{
			var serviceInformation = await GetServiceInformationInternal( FetchBehavior.FetchAlways, cancellationToken ).ConfigureAwait( false );
			_LastValidServiceInformation = serviceInformation;

			return _LastValidServiceInformation;
		}

		/// <inheritdoc />
		public async Task<InterfaceVersionRange> GetInterfaceInformation( CancellationToken cancellationToken = default )
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

		/// <inheritdoc />
		public Task<DataServiceFeatureMatrix> GetFeatureMatrix( CancellationToken cancellationToken = default )
		{
			return GetFeatureMatrixInternal( FetchBehavior.FetchAlways, cancellationToken );
		}

		/// <inheritdoc />
		public Task<ConfigurationDto> GetConfiguration( CancellationToken cancellationToken = default )
		{
			return _RestClient.Request<ConfigurationDto>( RequestBuilder.CreateGet( "configuration" ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="definitions"/> is <see langword="null"/>.</exception>
		public Task CreateAttributeDefinitions( EntityDto entity, IReadOnlyCollection<AbstractAttributeDefinitionDto> definitions, CancellationToken cancellationToken = default )
		{
			if( definitions == null ) throw new ArgumentNullException( nameof( definitions ) );
			return _RestClient.Request( RequestBuilder.CreatePost( $"configuration/{entity}", Payload.Create( definitions ) ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="definitions"/> is <see langword="null"/>.</exception>
		public Task UpdateAttributeDefinitions( EntityDto entity, IReadOnlyCollection<AbstractAttributeDefinitionDto> definitions, CancellationToken cancellationToken = default )
		{
			if( definitions == null ) throw new ArgumentNullException( nameof( definitions ) );
			return _RestClient.Request( RequestBuilder.CreatePut( $"configuration/{entity}", Payload.Create( definitions ) ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task DeleteAttributeDefinitions( EntityDto entity, IReadOnlyCollection<ushort> keys = null, CancellationToken cancellationToken = default )
		{
			var requestBasePath = $"configuration/{entity}";

			if( keys == null || keys.Count == 0 )
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( requestBasePath ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				//As the keys are passed as path segment within the uri target size is set to maximum length of path segemnts: 255
				foreach( var keyList in ArrayHelper.Split( keys, RestClientBase.MaximumPathSegmentLength, RestClientHelper.LengthOfListElementInUri ) )
				{
					var requestRestriction = $"/{RestClientHelper.ConvertUshortArrayToString( keyList )}";
					await _RestClient.Request( RequestBuilder.CreateDelete( string.Concat( requestBasePath, requestRestriction ) ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <inheritdoc />
		public Task DeleteAllAttributeDefinitions( CancellationToken cancellationToken = default )
		{
			return _RestClient.Request( RequestBuilder.CreateDelete( "configuration" ), cancellationToken );
		}

		/// <inheritdoc />
		public Task<bool> CheckAttributeUsage( ushort attributeKey, string value, CancellationToken cancellationToken = default )
		{
			return CheckAttributeUsageInternal( attributeKey, value, cancellationToken );
		}

		/// <inheritdoc />
		public Task<bool> CheckCatalogEntryUsage( ushort attributeKey, short catalogEntryKey, CancellationToken cancellationToken = default )
		{
			if( catalogEntryKey < 0 )
				throw new InvalidOperationException( $"Unable to check catalogue entry usage. {nameof( catalogEntryKey )} must be equal or greater than 0." );

			return CheckAttributeUsageInternal( attributeKey, catalogEntryKey.ToString(), cancellationToken );
		}

		/// <inheritdoc />
		public Task<IReadOnlyList<CatalogDto>> GetAllCatalogs( CancellationToken cancellationToken = default )
		{
			return _RestClient.Request<IReadOnlyList<CatalogDto>>( RequestBuilder.CreateGet( "catalogs" ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task<CatalogDto> GetCatalog( Guid catalogUuid, CancellationToken cancellationToken = default )
		{
			var catalog = await _RestClient.Request<IEnumerable<CatalogDto>>( RequestBuilder.CreateGet( $"catalogs/{catalogUuid}" ), cancellationToken ).ConfigureAwait( false );
			return catalog.FirstOrDefault();
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="catalogs"/> is <see langword="null"/>.</exception>
		public Task CreateCatalogs( IReadOnlyCollection<CatalogDto> catalogs, CancellationToken cancellationToken = default )
		{
			if( catalogs == null ) throw new ArgumentNullException( nameof( catalogs ) );
			return _RestClient.Request( RequestBuilder.CreatePost( "catalogs", Payload.Create( catalogs ) ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="catalogs"/> is <see langword="null"/>.</exception>
		public Task UpdateCatalogs( IReadOnlyCollection<CatalogDto> catalogs, CancellationToken cancellationToken = default )
		{
			if( catalogs == null ) throw new ArgumentNullException( nameof( catalogs ) );
			return _RestClient.Request( RequestBuilder.CreatePut( "catalogs", Payload.Create( catalogs ) ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task DeleteCatalogs( IReadOnlyCollection<Guid> catalogUuids = null, CancellationToken cancellationToken = default )
		{
			const string uri = "catalogs";

			if( catalogUuids == null || catalogUuids.Count == 0 )
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( uri ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				//As the keys are passed as path segment within the uri target size is set to maximum length of path segemnts: 255
				foreach( var catUuids in ArrayHelper.Split( catalogUuids, RestClientBase.MaximumPathSegmentLength, RestClientHelper.LengthOfListElementInUri ) )
				{
					var restriction = $"/{RestClientHelper.ConvertGuidListToString( catUuids )}";
					await _RestClient.Request( RequestBuilder.CreateDelete( string.Concat( uri, restriction ) ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="entries"/> is <see langword="null"/>.</exception>
		public Task CreateCatalogEntries( Guid catalogUuid, IReadOnlyCollection<CatalogEntryDto> entries, CancellationToken cancellationToken = default )
		{
			if( entries == null ) throw new ArgumentNullException( nameof( entries ) );
			return _RestClient.Request( RequestBuilder.CreatePost( $"catalogs/{catalogUuid}", Payload.Create( entries ) ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task DeleteCatalogEntries( Guid catalogUuid, IReadOnlyCollection<short> keys = null, CancellationToken cancellationToken = default )
		{
			var uri = $"catalogs/{catalogUuid}";

			if( keys == null || keys.Count == 0 )
			{
				await _RestClient.Request( RequestBuilder.CreateDelete( uri ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				foreach( var keyList in ArrayHelper.Split( keys, RestClientBase.MaximumPathSegmentLength, RestClientHelper.LengthOfListElementInUri ) )
				{
					var restriction = $"/{RestClientHelper.ConvertShortArrayToString( keyList )}";
					await _RestClient.Request( RequestBuilder.CreateDelete( string.Concat( uri, restriction ) ), cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyList<InspectionPlanPartDto>> GetParts( PathInformation partPath = null, IReadOnlyCollection<Guid> partUuids = null, ushort? depth = null, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default )
		{
			if( partUuids != null && partUuids.Count > 0 )
			{
				var result = new List<InspectionPlanPartDto>( partUuids.Count );
				foreach( var uuid in partUuids )
				{
					var inspectionPlanPart = await GetPartByUuid( uuid, requestedPartAttributes, withHistory, cancellationToken ).ConfigureAwait( false );
					if( inspectionPlanPart != null )
						result.Add( inspectionPlanPart );
				}

				return result;
			}

			var parameter = RestClientHelper.ParseToParameter( partPath, partUuids, null, depth, requestedPartAttributes, withHistory: withHistory );
			return await _RestClient.Request<IReadOnlyList<InspectionPlanPartDto>>( RequestBuilder.CreateGet( "parts", parameter ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		public async Task<InspectionPlanPartDto> GetPartByUuid( Guid partUuid, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default )
		{
			var parameter = RestClientHelper.ParseToParameter( requestedPartAttributes: requestedPartAttributes, withHistory: withHistory );
			return await _RestClient.Request<InspectionPlanPartDto>( RequestBuilder.CreateGet( $"parts/{partUuid}", parameter ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="parts"/> is <see langword="null"/>.</exception>
		public Task CreateParts( IReadOnlyCollection<InspectionPlanPartDto> parts, bool versioningEnabled = false, CancellationToken cancellationToken = default )
		{
			if( parts == null ) throw new ArgumentNullException( nameof( parts ) );

			async Task CreateParts()
			{
				if( versioningEnabled )
				{
					await ThrowOnUnsupportedCreateVersionEntries( cancellationToken );

					await _RestClient.Request( RequestBuilder.CreatePost( "parts", Payload.Create( parts ), ParameterDefinition.Create( "versioningEnabled", bool.TrueString ) ), cancellationToken ).ConfigureAwait( false );
				}
				else
				{
					await _RestClient.Request( RequestBuilder.CreatePost( "parts", Payload.Create( parts ) ), cancellationToken ).ConfigureAwait( false );
				}
			}

			return CreateParts();
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="parts"/> is <see langword="null"/>.</exception>
		public Task UpdateParts( IReadOnlyCollection<InspectionPlanPartDto> parts, bool versioningEnabled = false, CancellationToken cancellationToken = default )
		{
			if( parts == null ) throw new ArgumentNullException( nameof( parts ) );
			var parameter = ParameterDefinition.Create( "versioningEnabled", versioningEnabled.ToString() );
			return _RestClient.Request( RequestBuilder.CreatePut( "parts", Payload.Create( parts ), parameter ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="partPath"/> is <see langword="null"/>.</exception>
		public Task DeleteParts( PathInformation partPath, CancellationToken cancellationToken = default )
		{
			if( partPath == null ) throw new ArgumentNullException( nameof( partPath ) );
			var parameter = ParameterDefinition.Create( "partPath", PathHelper.PathInformation2DatabaseString( partPath ) );
			return _RestClient.Request( RequestBuilder.CreateDelete( "parts", parameter ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task DeleteParts( IReadOnlyCollection<Guid> partUuids, CancellationToken cancellationToken = default )
		{
			const string requestPath = "parts";
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestPath, MaxUriLength, ParameterDefinition.Create( "partUuids", "{}" ) );

			foreach( var uuidList in ArrayHelper.Split( partUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				var uuidListParameter = ParameterDefinition.Create( "partUuids", RestClientHelper.ConvertGuidListToString( uuidList ) );
				await _RestClient.Request( RequestBuilder.CreateDelete( requestPath, uuidListParameter ), cancellationToken ).ConfigureAwait( false );
			}
		}

		/// <inheritdoc/>
		public async Task ClearPart( Guid partUuid, IEnumerable<ClearPartKeepEntitiesDto> clearPartKeepEntities = null, CancellationToken cancellationToken = default )
		{
			await ThrowOnUnsupportedClearPart( cancellationToken );

			if( clearPartKeepEntities != null )
			{
				var clearPartListString = clearPartKeepEntities.Select( p => p.ToString() );
				var keepRestriction = RestClientHelper.ToListString( clearPartListString );
				var keepListParameter = ParameterDefinition.Create( "keep", keepRestriction );

				await _RestClient.Request( RequestBuilder.CreatePost( $"parts/{partUuid}/clear", Payload.Empty, keepListParameter ), cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				await _RestClient.Request( RequestBuilder.CreatePost( $"parts/{partUuid}/clear", Payload.Empty ), cancellationToken ).ConfigureAwait( false );
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyList<InspectionPlanCharacteristicDto>> GetCharacteristics( PathInformation partPath = null, ushort? depth = null, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default )
		{
			var parameter = RestClientHelper.ParseToParameter( partPath, null, null, depth, null, requestedCharacteristicAttributes, withHistory );
			return await _RestClient.Request<IReadOnlyList<InspectionPlanCharacteristicDto>>( RequestBuilder.CreateGet( "characteristics", parameter ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		public async Task<IReadOnlyList<InspectionPlanCharacteristicDto>> GetCharacteristicsByUuids( IReadOnlyCollection<Guid> charUuids, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default )
		{
			if( charUuids == null || charUuids.Count == 0 )
				return Array.Empty<InspectionPlanCharacteristicDto>();

			var result = new List<InspectionPlanCharacteristicDto>( charUuids.Count );

			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsCharacteristicUuidRestrictionForCharacteristicFetch )
			{
				foreach( var uuid in charUuids )
				{
					var inspectionPlanCharacteristic = await GetCharacteristicByUuid( uuid, requestedCharacteristicAttributes, withHistory, cancellationToken ).ConfigureAwait( false );
					if( inspectionPlanCharacteristic != null )
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
					var characteristics = await _RestClient.Request<IReadOnlyList<InspectionPlanCharacteristicDto>>( RequestBuilder.CreateGet( "characteristics", parameter ), cancellationToken ).ConfigureAwait( false );

					result.AddRange( characteristics );
				}
			}

			return result;
		}

		/// <inheritdoc />
		public async Task<InspectionPlanCharacteristicDto> GetCharacteristicByUuid( Guid charUuid, AttributeSelector requestedCharacteristicAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default )
		{
			var parameter = RestClientHelper.ParseToParameter( requestedCharacteristicAttributes: requestedCharacteristicAttributes, withHistory: withHistory );
			return await _RestClient.Request<InspectionPlanCharacteristicDto>( RequestBuilder.CreateGet( $"characteristics/{charUuid}", parameter ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="characteristics"/> is <see langword="null"/>.</exception>
		public Task CreateCharacteristics( IReadOnlyCollection<InspectionPlanCharacteristicDto> characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default )
		{
			if( characteristics == null ) throw new ArgumentNullException( nameof( characteristics ) );

			async Task CreateCharacteristics()
			{
				if( versioningEnabled )
				{
					await ThrowOnUnsupportedCreateVersionEntries( cancellationToken );

					await _RestClient.Request( RequestBuilder.CreatePost( "characteristics", Payload.Create( characteristics ), ParameterDefinition.Create( "versioningEnabled", bool.TrueString ) ), cancellationToken ).ConfigureAwait( false );
				}
				else
				{
					await _RestClient.Request( RequestBuilder.CreatePost( "characteristics", Payload.Create( characteristics ) ), cancellationToken ).ConfigureAwait( false );
				}
			}

			return CreateCharacteristics();
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="characteristics"/> is <see langword="null"/>.</exception>
		public Task UpdateCharacteristics( IReadOnlyCollection<InspectionPlanCharacteristicDto> characteristics, bool versioningEnabled = false, CancellationToken cancellationToken = default )
		{
			if( characteristics == null ) throw new ArgumentNullException( nameof( characteristics ) );
			var parameter = ParameterDefinition.Create( "versioningEnabled", versioningEnabled.ToString() );
			return _RestClient.Request( RequestBuilder.CreatePut( "characteristics", Payload.Create( characteristics ), parameter ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="charPath"/> is <see langword="null"/>.</exception>
		public Task DeleteCharacteristics( PathInformation charPath, CancellationToken cancellationToken = default )
		{
			if( charPath == null ) throw new ArgumentNullException( nameof( charPath ) );
			var parameter = ParameterDefinition.Create( "charPath", PathHelper.PathInformation2DatabaseString( charPath ) );
			return _RestClient.Request( RequestBuilder.CreateDelete( "characteristics", parameter ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task DeleteCharacteristics( IReadOnlyCollection<Guid> charUuid, CancellationToken cancellationToken = default )
		{
			const string requestPath = "characteristics";
			var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestPath, MaxUriLength, ParameterDefinition.Create( "charUuids", "{}" ) );

			foreach( var uuidList in ArrayHelper.Split( charUuid, targetSize, RestClientHelper.LengthOfListElementInUri ) )
			{
				var uuidListParameter = ParameterDefinition.Create( "charUuids", RestClientHelper.ConvertGuidListToString( uuidList ) );
				await _RestClient.Request( RequestBuilder.CreateDelete( requestPath, uuidListParameter ), cancellationToken ).ConfigureAwait( false );
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyList<SimpleMeasurementDto>> GetMeasurements( PathInformation partPath = null, MeasurementFilterAttributesDto filter = null, CancellationToken cancellationToken = default )
		{
			await ThrowOnUnsupportedMergeAttributes( filter?.MergeAttributes, cancellationToken );
			await ThrowOnUnsupportedMergeMasterPart( filter, cancellationToken );
			await ThrowOnUnsupportedLimitResultPerPart( filter, cancellationToken );

			const string requestPath = "measurements";

			// split multiple measurement uuids into chunks of uuids using multiple requests to avoid "Request-URI Too Long" exception
			if( filter?.MeasurementUuids?.Count > 0 )
			{
				var newFilter = filter.Clone();
				newFilter.MeasurementUuids = null;

				var parameterName = AbstractMeasurementFilterAttributesDto.MeasurementUuidsParamName;
				var parameterDefinitions = CreateParameterDefinitions( partPath, newFilter );

				//Split into multiple parameter sets to limit uuid parameter lenght
				var parameterSets = RestClientHelper.SplitAndMergeParameters( ServiceLocation, requestPath, MaxUriLength, parameterName, filter.MeasurementUuids, parameterDefinitions );

				//Execute requests in parallel
				var requests = parameterSets
					.Select( set => RequestBuilder.CreateGet( requestPath, set ) )
					.Select( request => _RestClient.Request<IReadOnlyCollection<SimpleMeasurementDto>>( request, cancellationToken ) );
				var result = await Task.WhenAll( requests ).ConfigureAwait( false );

				return LimitAndSortResult( result.SelectMany( r => r ), filter, result.Length );
			}

			// split multiple part uuids into chunks of uuids using multiple requests to avoid "Request-URI Too Long" exception
			if( filter?.PartUuids?.Count > 0 )
			{
				var newFilter = filter.Clone();
				newFilter.PartUuids = null;

				const string parameterName = AbstractMeasurementFilterAttributesDto.PartUuidsParamName;
				var parameterDefinitions = CreateParameterDefinitions( partPath, newFilter );

				//Split into multiple parameter sets to limit uuid parameter lenght
				var parameterSets = RestClientHelper.SplitAndMergeParameters( ServiceLocation, requestPath, MaxUriLength, parameterName, filter.PartUuids, parameterDefinitions );

				//Execute requests in parallel
				var requests = parameterSets
					.Select( set => RequestBuilder.CreateGet( requestPath, set ) )
					.Select( request => _RestClient.Request<IReadOnlyList<SimpleMeasurementDto>>( request, cancellationToken ) );
				var result = await Task.WhenAll( requests ).ConfigureAwait( false );

				return LimitAndSortResult( result.SelectMany( r => r ), filter, result.Length );
			}

			{
				var parameterDefinitions = CreateParameterDefinitions( partPath, filter );
				var requestUrl = RequestBuilder.CreateGet( requestPath, parameterDefinitions );
				return await _RestClient.Request<IReadOnlyList<SimpleMeasurementDto>>( requestUrl, cancellationToken ).ConfigureAwait( false );
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyList<string>> GetDistinctMeasurementAttributeValues( ushort key, PathInformation partPath = null, DistinctMeasurementFilterAttributesDto filter = null, CancellationToken cancellationToken = default )
		{
			await ThrowOnUnsupportedDistinctMeasurementValueSearch( cancellationToken );
			await ThrowOnUnsupportedLimitResultPerPart( filter, cancellationToken );

			if( filter?.MeasurementUuids?.Count > 0 )
			{
				var newFilter = filter.Clone();
				newFilter.MeasurementUuids = null;

				var parameter = CreateParameterDefinitions( partPath, newFilter, key );
				parameter.Add( ParameterDefinition.Create( AbstractMeasurementFilterAttributesDto.MeasurementUuidsParamName, "" ) );

				var requestRestriction = RequestBuilder.AppendParameters( "values", parameter );
				var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

				var result = new List<string>( filter.MeasurementUuids.Count );

				foreach( var uuids in ArrayHelper.Split( filter.MeasurementUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
				{
					newFilter.MeasurementUuids = uuids;

					var attributes = await _RestClient.Request<IReadOnlyCollection<string>>( RequestBuilder.CreateGet( "distinctMeasurementAttributeValues", CreateParameterDefinitions( partPath, newFilter, key ) ), cancellationToken ).ConfigureAwait( false );
					result.AddRange( attributes );
				}

				return result;
			}

			return await _RestClient.Request<IReadOnlyList<string>>( RequestBuilder.CreateGet( "distinctMeasurementAttributeValues", CreateParameterDefinitions( partPath, filter, key ) ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="measurements"/> is <see langword="null"/>.</exception>
		public Task CreateMeasurements( IReadOnlyCollection<SimpleMeasurementDto> measurements, CancellationToken cancellationToken = default )
		{
			if( measurements == null ) throw new ArgumentNullException( nameof( measurements ) );
			return _RestClient.Request( RequestBuilder.CreatePost( "measurements", Payload.Create( measurements ) ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="measurements"/> is <see langword="null"/>.</exception>
		public Task UpdateMeasurements( IReadOnlyCollection<SimpleMeasurementDto> measurements, CancellationToken cancellationToken = default )
		{
			if( measurements == null ) throw new ArgumentNullException( nameof( measurements ) );
			return _RestClient.Request( RequestBuilder.CreatePut( "measurements", Payload.Create( measurements ) ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task DeleteMeasurementsByPartPath( PathInformation partPath = null, GenericSearchConditionDto filter = null, AggregationMeasurementSelectionDto aggregation = AggregationMeasurementSelectionDto.Default, MeasurementDeleteBehaviorDto deep = MeasurementDeleteBehaviorDto.DeleteForCurrentPartOnly, CancellationToken cancellationToken = default )
		{
			var parameter = new List<ParameterDefinition>();

			if( partPath != null )
				parameter.Add( ParameterDefinition.Create( "partPath", PathHelper.PathInformation2DatabaseString( partPath ) ) );

			if( filter != null )
				parameter.Add( ParameterDefinition.Create( "searchCondition", SearchConditionParser.GenericConditionToString( filter ) ) );

			if( aggregation != AggregationMeasurementSelectionDto.Default )
				parameter.Add( ParameterDefinition.Create( "aggregation", aggregation.ToString() ) );

			if( deep == MeasurementDeleteBehaviorDto.DeleteDeep )
			{
				await ThrowOnUnsupportedDeleteMeasurementsForSubParts( cancellationToken );

				parameter.Add( ParameterDefinition.Create( "deep", deep.ToString() ) );
			}

			await DeleteMeasurementsInternal( parameter, cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		public async Task DeleteMeasurementsByPartUuids( IReadOnlyCollection<Guid> partUuids, GenericSearchConditionDto filter = null, AggregationMeasurementSelectionDto aggregation = AggregationMeasurementSelectionDto.Default, CancellationToken cancellationToken = default )
		{
			if( partUuids == null )
			{
				var parameter = new List<ParameterDefinition>();

				if( filter != null )
					parameter.Add( ParameterDefinition.Create( "searchCondition", SearchConditionParser.GenericConditionToString( filter ) ) );

				if( aggregation != AggregationMeasurementSelectionDto.Default )
					parameter.Add( ParameterDefinition.Create( "aggregation", aggregation.ToString() ) );

				await DeleteMeasurementsInternal( parameter, cancellationToken ).ConfigureAwait( false );
			}
			else
			{
				foreach( var partUuid in partUuids )
				{
					var parameter = new List<ParameterDefinition> { ParameterDefinition.Create( "partUuids", RestClientHelper.ConvertGuidListToString( new[] { partUuid } ) ) };

					if( filter != null )
						parameter.Add( ParameterDefinition.Create( "searchCondition", SearchConditionParser.GenericConditionToString( filter ) ) );

					if( aggregation != AggregationMeasurementSelectionDto.Default )
						parameter.Add( ParameterDefinition.Create( "aggregation", aggregation.ToString() ) );

					await DeleteMeasurementsInternal( parameter, cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <inheritdoc />
		public async Task DeleteMeasurementsByUuid( IReadOnlyCollection<Guid> measurementUuids, CancellationToken cancellationToken = default )
		{
			if( measurementUuids.Any() )
			{
				var emptyParameter = new[] { ParameterDefinition.Create( "measurementUuids", "" ) };
				var requestRestriction = RequestBuilder.AppendParameters( "measurements", emptyParameter );

				var targetSize = RestClientHelper.GetUriTargetSize( ServiceLocation, requestRestriction, MaxUriLength );

				foreach( var uuids in ArrayHelper.Split( measurementUuids, targetSize, RestClientHelper.LengthOfListElementInUri ) )
				{
					var parameter = new List<ParameterDefinition> { ParameterDefinition.Create( "measurementUuids", RestClientHelper.ConvertGuidListToString( uuids ) ) };
					await DeleteMeasurementsInternal( parameter, cancellationToken ).ConfigureAwait( false );
				}
			}
		}

		/// <inheritdoc />
		public async Task<IReadOnlyList<DataMeasurementDto>> GetMeasurementValues( PathInformation partPath = null, MeasurementValueFilterAttributesDto filter = null, CancellationToken cancellationToken = default )
		{
			await ThrowOnUnsupportedMergeAttributes( filter?.MergeAttributes, cancellationToken );
			await ThrowOnUnsupportedLimitResultPerPart( filter, cancellationToken );

			if( filter?.MeasurementUuids?.Count > 0 )
				return await GetMeasurementValuesSplitByMeasurement( partPath, filter, cancellationToken ).ConfigureAwait( false );

			if( filter?.CharacteristicsUuidList?.Count > 0 )
				return await GetMeasurementValuesSplitByCharacteristics( partPath, filter, cancellationToken ).ConfigureAwait( false );

			return await _RestClient.Request<IReadOnlyList<DataMeasurementDto>>( RequestBuilder.CreateGet( "values", CreateParameterDefinitions( partPath, filter ) ), cancellationToken ).ConfigureAwait( false );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
		public Task CreateMeasurementValues( IReadOnlyCollection<DataMeasurementDto> values, CancellationToken cancellationToken = default )
		{
			if( values == null ) throw new ArgumentNullException( nameof( values ) );
			return _RestClient.Request( RequestBuilder.CreatePost( "values", Payload.Create( values ) ), cancellationToken );
		}

		/// <inheritdoc />
		/// <exception cref="ArgumentNullException"><paramref name="values"/> is <see langword="null"/>.</exception>
		public Task UpdateMeasurementValues( IReadOnlyCollection<DataMeasurementDto> values, CancellationToken cancellationToken = default )
		{
			if( values == null ) throw new ArgumentNullException( nameof( values ) );
			return _RestClient.Request( RequestBuilder.CreatePut( "values", Payload.Create( values ) ), cancellationToken );
		}

		#endregion
	}
}
