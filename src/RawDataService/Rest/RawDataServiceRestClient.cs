#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region using

	using System;
	using System.Collections.Generic;
	using System.IO;
	using System.Linq;
	using System.Net;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.IMT.PiWeb.Api.Common.Client;
	using Zeiss.IMT.PiWeb.Api.Common.Data;
	using Zeiss.IMT.PiWeb.Api.Common.Data.FilterString.Formatter;
	using Zeiss.IMT.PiWeb.Api.Common.Utilities;
	using Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Conditions;

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

		private ServiceInformation _LastValidServiceInformation;
		private RawDataServiceFeatureMatrix _FeatureMatrix;

		#endregion

		#region constructors

        /// <summary>
        /// Main Constructor. Initilization of the client class for communicating with the RawDataService via the given <paramref name="serverUri"/>
        /// </summary>
        /// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
        /// <param name="maxUriLength">The uri length limit</param>
        /// <param name="restClient">Custom implementation of RestClient</param>
        public RawDataServiceRestClient( Uri serverUri, int maxUriLength = RestClientBase.DefaultMaxUriLength, RestClientBase restClient = null )
			: base( restClient ?? new RestClient( serverUri, EndpointName, maxUriLength: maxUriLength ) )
		{
		}

		#endregion

		#region General

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

		/// <summary>
		/// Method for fetching the <see cref="RawDataServiceFeatureMatrix"/>
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		public Task<RawDataServiceFeatureMatrix> GetFeatureMatrix( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetFeatureMatrixInternal( FetchBehavior.FetchAlways, cancellationToken );
		}

		#endregion

		#region Fetching of raw data entry information

		/// <summary> 
		/// Fetches a list of raw data information for the <paramref name="entity"/> identified by <paramref name="uuids"/> and filtered by <paramref name="filter"/>.
		/// Either <paramref name="uuids" /> or <paramref name="filter"/> must have a value.
		/// </summary>
		/// <param name="entity">The <see cref="RawDataEntity"/> the raw data information should be fetched for.</param>
		/// <param name="uuids">The list of value uuids the data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="InvalidOperationException">No uuids and no filter was specified.</exception>
		/// <exception cref="OperationNotSupportedOnServerException">An attribute filter for raw data is not supported by this server.</exception>
		public async Task<RawDataInformation[]> ListRawData( RawDataEntity entity, string[] uuids, FilterCondition filter, CancellationToken cancellationToken = default( CancellationToken ) )
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

		private Task<RawDataInformation[]> ListRawDataForAllEntities( RawDataEntity entity, [NotNull] FilterCondition filter, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( filter == null )
				throw new ArgumentNullException( nameof(filter) );

			var requestPath = $"rawData/{entity}";

			var parameterDefinitionList = new List<ParameterDefinition>();
			var filterTree = filter.BuildFilterTree();
			var filterString = new FilterTreeFormatter().FormatString( filterTree );
			var filterParameter = ParameterDefinition.Create( "filter", filterString );
			parameterDefinitionList.Add( filterParameter );

			return _RestClient.Request<RawDataInformation[]>( RequestBuilder.CreateGet( requestPath, parameterDefinitionList.ToArray() ), cancellationToken );
		}

		private async Task<RawDataInformation[]> ListRawDataByUuidList( RawDataEntity entity, string[] uuids, FilterCondition filter, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			StringUuidTools.CheckUuids( entity, uuids );

			var requestPath = $"rawData/{entity}";
			var parameterDefinitions = new List<ParameterDefinition>();

			if( filter != null )
			{
				var filterTree = filter.BuildFilterTree();
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
				.Select( request => _RestClient.Request<RawDataInformation[]>( request, cancellationToken ) );
			var result = await Task.WhenAll( requests ).ConfigureAwait( false );

			return result.SelectMany( r => r ).ToArray();
		}

		#endregion

		#region Fetching of raw data entries

		/// <summary>
		/// Fetches raw data as a byte array for the raw data item identified by <paramref name="target"/> and <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntity"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <param name="expectedMd5">The md5 check sum that is expected for the result object. If this value is set, performance is better because server side round trips are reduced.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		public Task<byte[]> GetRawData( RawDataTargetEntity target, int rawDataKey, Guid? expectedMd5 = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( expectedMd5.HasValue )
				return _RestClient.RequestBytes( RequestBuilder.CreateGet( $"rawData/{target.Entity}/{target.Uuid}/{rawDataKey}?expectedMd5={expectedMd5}" ), cancellationToken );

			return _RestClient.RequestBytes( RequestBuilder.CreateGet( $"rawData/{target.Entity}/{target.Uuid}/{rawDataKey}" ), cancellationToken );
		}

		#endregion

		#region Fetching of preview thumbnails for raw data entries

		/// <summary> 
		/// Fetches a preview image for the specified <code>info</code>. 
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntity"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <returns>The preview image as byte array.</returns>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<byte[]> GetRawDataThumbnail( RawDataTargetEntity target, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			try
			{
				return await _RestClient.RequestBytes( RequestBuilder.CreateGet( $"rawData/{target.Entity}/{target.Uuid}/{rawDataKey}/thumbnail" ), cancellationToken ).ConfigureAwait( false );
			}
			catch( WrappedServerErrorException ex )
			{
				if( ex.StatusCode != HttpStatusCode.NotFound )
					throw;
			}
			return null;
		}

		#endregion

		#region Creation of raw data entries

		/// <summary> 
		/// Creates a new raw data object <paramref name="data"/> for the element specified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformation"/> object containing the <see cref="RawDataEntity"/> type and the uuid of the raw data that should be uploaded.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <remarks>
		/// If key speciefied by <see cref="RawDataInformation.Key"/> is -1, a new key will be chosen by the server automatically. This is the preferred way.
		/// </remarks>
		public Task CreateRawData( RawDataInformation info, byte[] data, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return UploadRawData( info, data, HttpMethod.Post, cancellationToken );
		}

		#endregion

		#region Update of raw data entries

		/// <summary> 
		/// Updates the raw data object <paramref name="data"/> for the element identified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformation"/> object containing the <see cref="RawDataEntity"/> type, the uuid and the key of the raw data that should be updated.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateRawData( RawDataInformation info, byte[] data, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( info.Key < 0 )
				throw new InvalidOperationException( "Unable to update raw data object: A key must be specified." );
			if( data == null )
				throw new ArgumentNullException( nameof(data), "Unable to update raw data object: Data object is null." );

			return UploadRawData( info, data, HttpMethod.Put, cancellationToken );
		}

		private Task UploadRawData( RawDataInformation info, byte[] data, HttpMethod method, CancellationToken cancellationToken )
		{
			StringUuidTools.CheckUuid( info.Target.Entity, info.Target.Uuid );

			if( string.IsNullOrEmpty( info.FileName ) )
				throw new ArgumentException( "FileName needs to be set.", nameof(info) );

			var requestString = info.KeySpecified && info.Key >= 0
				? $"rawData/{info.Target.Entity}/{info.Target.Uuid}/{info.Key}"
				: $"rawData/{info.Target.Entity}/{info.Target.Uuid}";

			var stream = new MemoryStream( data, 0, data.Length, false, true );
			return _RestClient.Request( RequestBuilder.CreateWithAttachment( method, requestString, stream, info.MimeType, info.Size, info.MD5, info.FileName ), cancellationToken );
		}

		#endregion

		#region Delete of raw data entries

		/// <summary>
		/// Deletes raw data for the element identified by <paramref name="target"/> and <paramref name="rawDataKey"/>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntity"/> object containing the <see cref="RawDataEntity"/> type and the uuid of the raw data that should be deleted.</param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		public Task DeleteRawData( RawDataTargetEntity target, int? rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			StringUuidTools.CheckUuid( target.Entity, target.Uuid );

			var url = rawDataKey.HasValue
				? $"rawData/{target.Entity}/{{{target.Uuid}}}/{rawDataKey}"
				: $"rawData/{target.Entity}/{{{target.Uuid}}}";

			return _RestClient.Request( RequestBuilder.CreateDelete( url ), cancellationToken );
		}

		#endregion

		#region helper

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

		private async Task<RawDataServiceFeatureMatrix> GetFeatureMatrixInternal( FetchBehavior behavior, CancellationToken cancellationToken = default( CancellationToken ) )
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
	}
}