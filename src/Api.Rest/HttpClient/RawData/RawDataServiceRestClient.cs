#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Common.Client;
	using Zeiss.PiWeb.Api.Rest.Common.Data;
	using Zeiss.PiWeb.Api.Rest.Common.Data.FilterString.Formatter;
	using Zeiss.PiWeb.Api.Rest.Common.Utilities;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.RawData;
	using Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Conditions;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based raw data service.
	/// </summary>
	public class RawDataServiceRestClient : CommonRestClientBase, IRawDataServiceRestClient
	{
		#region constants

		private const string EndpointName = "RawDataServiceRest/";

		#endregion

		#region members

		private ServiceInformationDto _LastValidServiceInformation;
		private RawDataServiceFeatureMatrix _FeatureMatrix;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataServiceRestClient"/> class.
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		/// <param name="maxUriLength">The uri length limit</param>
		/// <param name="restClient">Custom implementation of RestClient</param>
		public RawDataServiceRestClient( [NotNull] Uri serverUri, int maxUriLength = RestClientBase.DefaultMaxUriLength, RestClientBase restClient = null )
			: base( restClient ?? new RestClient( serverUri, EndpointName, maxUriLength: maxUriLength ) )
		{ }

		#endregion

		#region methods

		private Task<RawDataInformationDto[]> ListRawDataForAllEntities( RawDataEntityDto entity, [NotNull] IFilterCondition filter, CancellationToken cancellationToken = default )
		{
			if( filter == null )
				throw new ArgumentNullException( nameof( filter ) );

			var requestPath = $"rawData/{entity}";

			var parameterDefinitionList = new List<ParameterDefinition>();
			var filterTree = ( (FilterCondition)filter ).BuildFilterTree();
			var filterString = new FilterTreeFormatter().FormatString( filterTree );
			var filterParameter = ParameterDefinition.Create( "filter", filterString );
			parameterDefinitionList.Add( filterParameter );

			return _RestClient.Request<RawDataInformationDto[]>( RequestBuilder.CreateGet( requestPath, parameterDefinitionList.ToArray() ), cancellationToken );
		}

		private async Task<RawDataInformationDto[]> ListRawDataByUuidList( RawDataEntityDto entity, string[] uuids, IFilterCondition filter, CancellationToken cancellationToken = default )
		{
			StringUuidTools.CheckUuids( entity, uuids );

			var requestPath = $"rawData/{entity}";
			var parameterDefinitions = new List<ParameterDefinition>();

			if( filter != null )
			{
				var filterTree = ( (FilterCondition)filter ).BuildFilterTree();
				var filterString = new FilterTreeFormatter().FormatString( filterTree );
				var filterParameter = ParameterDefinition.Create( "filter", filterString );
				parameterDefinitions.Add( filterParameter );
			}

			//Split into multiple parameter sets to limit uuid parameter lenght
			var splitter = new ParameterSplitter( this, requestPath );
			var collectionParameter = CollectionParameterFactory.Create( "uuids", uuids );
			var parameterSets = splitter.SplitAndMerge( collectionParameter, parameterDefinitions );

			//Execute requests in parallel
			var requests = parameterSets
				.Select( set => RequestBuilder.CreateGet( requestPath, set.ToArray() ) )
				.Select( request => _RestClient.Request<RawDataInformationDto[]>( request, cancellationToken ) );
			var result = await Task.WhenAll( requests ).ConfigureAwait( false );

			return result.SelectMany( r => r ).ToArray();
		}

		private Task UploadRawData( RawDataInformationDto info, byte[] data, HttpMethod method, CancellationToken cancellationToken )
		{
			StringUuidTools.CheckUuid( info.Target.Entity, info.Target.Uuid );

			if( string.IsNullOrEmpty( info.FileName ) )
				throw new ArgumentException( "FileName needs to be set.", nameof( info ) );

			var requestString = info.Key.HasValue && info.Key >= 0
				? $"rawData/{info.Target.Entity}/{info.Target.Uuid}/{info.Key}"
				: $"rawData/{info.Target.Entity}/{info.Target.Uuid}";

			var stream = new MemoryStream( data, 0, data.Length, false, true );
			return _RestClient.Request( RequestBuilder.CreateWithAttachment( method, requestString, stream, info.MimeType, info.Size, info.MD5, info.FileName ), cancellationToken );
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

		private async Task<RawDataServiceFeatureMatrix> GetFeatureMatrixInternal( FetchBehavior behavior, CancellationToken cancellationToken = default )
		{
			// This is an intentional race condition. Calling this method from multiple threads may lead to multiple calls to Get<InterfaceInformation>().
			// However, this would be rare and harmless, since it should always return the same result. It would be a lot more difficult to make this work without any races or blocking.
			// It is important to never set _LastValidServiceInformation to null anywhere to avoid possible null returns here due to the race condition.
			if( behavior == FetchBehavior.FetchAlways || _FeatureMatrix == null )
			{
				var interfaceVersionRange = await GetInterfaceInformation( cancellationToken ).ConfigureAwait( false );
				_FeatureMatrix = new RawDataServiceFeatureMatrix( interfaceVersionRange );
			}

			return _FeatureMatrix;
		}

		#endregion

		#region interface IRawDataServiceRestClient

		/// <summary>
		/// Method for fetching the <see cref="ServiceInformationDto"/>. This method can be used for connection checking. The call returns quickly
		/// and does not produce any noticeable server load.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<ServiceInformationDto> GetServiceInformation( CancellationToken cancellationToken = default )
		{
			var serviceInformation = await GetServiceInformationInternal( FetchBehavior.FetchAlways, cancellationToken ).ConfigureAwait( false );
			_LastValidServiceInformation = serviceInformation;

			return _LastValidServiceInformation;
		}

		/// <summary>
		/// Method for fetching the <see cref="InterfaceVersionRange"/>.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
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

		/// <summary>
		/// Method for fetching the <see cref="RawDataServiceFeatureMatrix"/>
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		public Task<RawDataServiceFeatureMatrix> GetFeatureMatrix( CancellationToken cancellationToken = default )
		{
			return GetFeatureMatrixInternal( FetchBehavior.FetchAlways, cancellationToken );
		}

		/// <summary>
		/// Fetches a list of raw data information for the <paramref name="entity"/> identified by <paramref name="uuids"/> and filtered by <paramref name="filter"/>.
		/// Either <paramref name="uuids" /> or <paramref name="filter"/> must have a value.
		/// </summary>
		/// <param name="entity">The <see cref="RawDataEntityDto"/> the raw data information should be fetched for.</param>
		/// <param name="uuids">The list of value uuids the data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="InvalidOperationException">No uuids and no filter was specified.</exception>
		/// <exception cref="OperationNotSupportedOnServerException">An attribute filter for raw data is not supported by this server.</exception>
		public async Task<RawDataInformationDto[]> ListRawData( RawDataEntityDto entity, string[] uuids, IFilterCondition filter = null, CancellationToken cancellationToken = default )
		{
			if( uuids == null && filter == null )
				throw new InvalidOperationException( "Either a filter or at least one uuid must be specified." );

			if( filter != null )
			{
				var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
				if( !featureMatrix.SupportsRawDataAttributeFilter )
				{
					throw new OperationNotSupportedOnServerException(
						"An attribute filter for raw data is not supported by this server.",
						RawDataServiceFeatureMatrix.RawDataAttributeFilterMinVersion,
						featureMatrix.CurrentInterfaceVersion );
				}
			}

			return uuids != null
				? await ListRawDataByUuidList( entity, uuids, filter, cancellationToken ).ConfigureAwait( false )
				: await ListRawDataForAllEntities( entity, filter, cancellationToken ).ConfigureAwait( false );
		}

		/// <summary>
		/// Fetches raw data as a byte array for the raw data item identified by <paramref name="target"/> and <paramref name="rawDataKey"/>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntityDto"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <param name="expectedMd5">The md5 check sum that is expected for the result object. If this value is set, performance is better because server side round trips are reduced.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null" />.</exception>
		public Task<byte[]> GetRawData( RawDataTargetEntityDto target, int rawDataKey, Guid? expectedMd5 = null, CancellationToken cancellationToken = default )
		{
			if( target == null ) throw new ArgumentNullException( nameof( target ) );

			if( expectedMd5.HasValue )
				return _RestClient.RequestBytes( RequestBuilder.CreateGet( $"rawData/{target.Entity}/{target.Uuid}/{rawDataKey}?expectedMd5={expectedMd5}" ), cancellationToken );

			return _RestClient.RequestBytes( RequestBuilder.CreateGet( $"rawData/{target.Entity}/{target.Uuid}/{rawDataKey}" ), cancellationToken );
		}

		/// <summary>
		/// Fetches a preview image for the specified <code>info</code>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntityDto"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <returns>The preview image as byte array.</returns>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null" />.</exception>
		public Task<byte[]> GetRawDataThumbnail( RawDataTargetEntityDto target, int rawDataKey, CancellationToken cancellationToken = default )
		{
			if( target == null ) throw new ArgumentNullException( nameof( target ) );

			async Task<byte[]> GetRawDataThumbnail()
			{
				try
				{
					return await _RestClient
						.RequestBytes(
							RequestBuilder.CreateGet( $"rawData/{target.Entity}/{target.Uuid}/{rawDataKey}/thumbnail" ),
							cancellationToken )
						.ConfigureAwait( false );
				}
				catch( WrappedServerErrorException ex )
				{
					if( ex.StatusCode != HttpStatusCode.NotFound )
						throw;
				}

				return null;
			}

			return GetRawDataThumbnail();
		}

		/// <inheritdoc />
		public async Task<RawDataArchiveEntriesDto> ListRawDataArchiveEntries( RawDataTargetEntityDto targetEntity, int targetKey, CancellationToken cancellationToken = default )
		{
			if( targetEntity == null ) throw new ArgumentNullException( nameof( targetEntity ) );

			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsArchiveLookup )
			{
				throw new OperationNotSupportedOnServerException(
					"Archive lookup is not supported by this server.",
					RawDataServiceFeatureMatrix.RawDataArchiveLookupMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}

			var requestPath = $"rawData/{targetEntity.Entity}/{targetEntity.Uuid}/{targetKey}/archiveEntries";

			return ( await _RestClient.Request<RawDataArchiveEntriesDto[]>( RequestBuilder.CreateGet( requestPath ), cancellationToken ) ).First();
		}

		/// <inheritdoc />
		public async Task<byte[]> GetRawDataArchiveContent( RawDataTargetEntityDto targetEntity, int targetKey, string fileName, Guid? expectedArchiveMd5 = null, CancellationToken cancellationToken = default )
		{
			if( targetEntity == null ) throw new ArgumentNullException( nameof( targetEntity ) );
			if( fileName == null ) throw new ArgumentNullException( nameof( fileName ) );

			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsArchiveLookup )
			{
				throw new OperationNotSupportedOnServerException(
					"Archive lookup is not supported by this server.",
					RawDataServiceFeatureMatrix.RawDataArchiveLookupMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}

			var requestPath = expectedArchiveMd5.HasValue
				? $"rawData/{targetEntity.Entity}/{targetEntity.Uuid}/{targetKey}/archiveContent/{fileName}?expectedArchiveMd5={expectedArchiveMd5}"
				: $"rawData/{targetEntity.Entity}/{targetEntity.Uuid}/{targetKey}/archiveContent/{fileName}";

			return await _RestClient.RequestBytes( RequestBuilder.CreateGet( requestPath ), cancellationToken );
		}

		/// <inheritdoc />
		public async Task<RawDataArchiveEntriesDto[]> RawDataArchiveEntryQuery( RawDataBulkQueryDto query, CancellationToken cancellationToken = default )
		{
			if( query == null ) throw new ArgumentNullException( nameof( query ) );

			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsArchiveLookup )
			{
				throw new OperationNotSupportedOnServerException(
					"Archive lookup is not supported by this server.",
					RawDataServiceFeatureMatrix.RawDataArchiveLookupMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}

			return await _RestClient.Request<RawDataArchiveEntriesDto[]>( RequestBuilder.CreatePost(
					"rawData/archiveEntryQuery",
					Payload.Create( query ) ),
				cancellationToken );
		}

		/// <inheritdoc />
		public async Task<RawDataArchiveEntriesDto[]> RawDataArchiveEntryQuery( RawDataInformationDto[] rawDataInfo, CancellationToken cancellationToken = default )
		{
			if( rawDataInfo == null ) throw new ArgumentNullException( nameof( rawDataInfo ) );

			var selectors = new List<RawDataSelectorDto>();
			foreach( var info in rawDataInfo )
			{
				if( !info.Key.HasValue ) throw new ArgumentNullException( nameof( info.Key ) );
				selectors.Add( new RawDataSelectorDto( info.Key.Value, info.Target ) );
			}
			var query = new RawDataBulkQueryDto( selectors.ToArray() );

			return await RawDataArchiveEntryQuery( query, cancellationToken );
		}

		/// <inheritdoc />
		public async Task<RawDataArchiveContentDto[]> RawDataArchiveContentQuery( RawDataArchiveBulkQueryDto query, CancellationToken cancellationToken = default )
		{
			if( query == null ) throw new ArgumentNullException( nameof( query ) );

			var featureMatrix = await GetFeatureMatrixInternal( FetchBehavior.FetchIfNotCached, cancellationToken ).ConfigureAwait( false );
			if( !featureMatrix.SupportsArchiveLookup )
			{
				throw new OperationNotSupportedOnServerException(
					"Archive lookup is not supported by this server.",
					RawDataServiceFeatureMatrix.RawDataArchiveLookupMinVersion,
					featureMatrix.CurrentInterfaceVersion );
			}

			using var stream = await _RestClient.RequestStream( RequestBuilder.CreatePost(
					"rawData/archiveContentQuery",
					Payload.Create( query ) ),
				cancellationToken );

			return RestClientHelper.DeserializeBinaryObject<RawDataArchiveContentDto[]>( stream );
		}

		/// <summary>
		/// Creates a new raw data object <paramref name="data"/> for the element specified by <paramref name="info"/>.
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformationDto"/> object containing the <see cref="RawDataEntityDto"/> type and the uuid of the raw data that should be uploaded.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <remarks>
		/// If key speciefied by <see cref="RawDataInformationDto.Key"/> is -1, a new key will be chosen by the server automatically. This is the preferred way.
		/// </remarks>
		/// <exception cref="ArgumentNullException"><paramref name="info"/> or <paramref name="data"/> is <see langword="null" />.</exception>
		public Task CreateRawData( RawDataInformationDto info, byte[] data, CancellationToken cancellationToken = default )
		{
			if( info == null ) throw new ArgumentNullException( nameof( info ) );
			if( data == null ) throw new ArgumentNullException( nameof( data ) );
			return UploadRawData( info, data, HttpMethod.Post, cancellationToken );
		}

		/// <summary>
		/// Updates the raw data object <paramref name="data"/> for the element identified by <paramref name="info"/>.
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformationDto"/> object containing the <see cref="RawDataEntityDto"/> type, the uuid and the key of the raw data that should be updated.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="info"/> is <see langword="null" />.</exception>
		public Task UpdateRawData( RawDataInformationDto info, byte[] data, CancellationToken cancellationToken = default )
		{
			if( info == null ) throw new ArgumentNullException( nameof( info ) );

			if( info.Key < 0 )
				throw new InvalidOperationException( "Unable to update raw data object: A key must be specified." );
			if( data == null )
				throw new ArgumentNullException( nameof( data ), "Unable to update raw data object: Data object is null." );

			return UploadRawData( info, data, HttpMethod.Put, cancellationToken );
		}

		/// <summary>
		/// Deletes raw data for the element identified by <paramref name="target"/> and <paramref name="rawDataKey"/>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntityDto"/> object containing the <see cref="RawDataEntityDto"/> type and the uuid of the raw data that should be deleted.</param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="target"/> is <see langword="null" />.</exception>
		public Task DeleteRawData( RawDataTargetEntityDto target, int? rawDataKey = null, CancellationToken cancellationToken = default )
		{
			if( target == null ) throw new ArgumentNullException( nameof( target ) );

			StringUuidTools.CheckUuid( target.Entity, target.Uuid );

			var url = rawDataKey.HasValue
				? $"rawData/{target.Entity}/{{{target.Uuid}}}/{rawDataKey}"
				: $"rawData/{target.Entity}/{{{target.Uuid}}}";

			return _RestClient.Request( RequestBuilder.CreateDelete( url ), cancellationToken );
		}

		#endregion
	}
}
