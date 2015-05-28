#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace RawDataService
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

	using Common.Client;
	using Common.Data;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based raw data service.
	/// </summary>
	public class RawDataServiceRestClient : RestClient
	{
		#region constructor

		/// <summary> 
		/// Constructor. Initialization of the client class for communicating with the RawDataService via the given <paramref name="serverUri"/>
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		/// <param name="maxUriLength">The uri length limit</param>
		public RawDataServiceRestClient( string serverUri = "http://127.0.0.1:8080/", int? maxUriLength = null )
			: this( new Uri( serverUri ), maxUriLength )
		{
		}

		/// <summary>
		/// Constructor. Initilization of the client class for communicating with the RawDataService via the given <paramref name="serverUri"/>
		/// </summary>
		/// <param name="serverUri">The PiWeb Server uri, including port and instance</param>
		/// <param name="maxUriLength">The uri length limit</param>
		public RawDataServiceRestClient( Uri serverUri, int? maxUriLength = null )
			: base( serverUri, "RawDataServiceRest/", maxUriLength: maxUriLength )
		{
		}

		/// <summary> 
		/// Constructor. Initialization of the client class for communicating with the RawDataService via the given parameters.
		/// </summary>
		/// <param name="scheme">PiWeb Server's schema (http/https)</param>
		/// <param name="host">PiWeb Server's host address</param>
		/// <param name="port">PiWeb Server's port</param>
		/// <param name="instance">An additional path wich is added to the uri, e.g. needed for an instance identifier.</param>
		/// <param name="maxUriLength">The uri length limit</param>
		public RawDataServiceRestClient( string scheme, string host, int port, string instance = null, int? maxUriLength = null )
			: this( new UriBuilder( scheme, host, port, instance ).Uri, maxUriLength )
		{
		}

		#endregion

		#region General

		/// <summary> 
		/// Method for fetching the <see cref="ServiceInformation"/>. This method can be used for connection checking. The call returns quickly 
		/// and does not produce any noticeable server load. 
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<ServiceInformation> GetServiceInformation( CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return Get<ServiceInformation>( "ServiceInformation", cancellationToken );
		}

		#endregion

		#region Fetching of raw data entry information

		/// <summary> 
		/// Fetches a list of raw data information for the parts identified by <paramref name="partUuids"/>. 
		/// </summary>
		/// <param name="partUuids">The list of part uuids the raw data information should be fetched for.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<RawDataInformation[]> ListRawDataForParts( Guid[] partUuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return ListRawData( RawDataEntity.Part, partUuids.Select( u => u.ToString() ).ToArray(), cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the characteristic identified by <paramref name="charateristicUuids"/>. 
		/// </summary>
		/// <param name="charateristicUuids">The list of characteristic uuids the raw data information should be fetched for.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<RawDataInformation[]> ListRawDataForCharacteristics( Guid[] charateristicUuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return ListRawData( RawDataEntity.Characteristic, charateristicUuids.Select( u => u.ToString() ).ToArray(), cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the measurements identified by <paramref name="measurementUuids"/>. 
		/// </summary>
		/// <param name="measurementUuids">The list of measurement uuids the raw data information should be fetched for.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<RawDataInformation[]> ListRawDataForMeasurements( Guid[] measurementUuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return ListRawData( RawDataEntity.Measurement, measurementUuids.Select( u => u.ToString() ).ToArray(), cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the measurement values identified by <paramref name="measurementValueUuids"/>. 
		/// </summary>
		/// <param name="measurementValueUuids">The list of value uuids the raw data information should be fetched for.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<RawDataInformation[]> ListRawDataForValues( ValueRawDataIdentifier[] measurementValueUuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return ListRawData( RawDataEntity.Value, measurementValueUuids.Select( StringUuidTools.CreateStringUuidPair ).ToArray(), cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the <paramref name="entity"/> identified by <paramref name="uuids"/>. 
		/// </summary>
		/// <param name="entity">The <see cref="RawDataEntity"/> the raw data information should be fetched for.</param>
		/// <param name="uuids">The list of value uuids the data information should be fetched for.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<RawDataInformation[]> ListRawData( RawDataEntity entity, string[] uuids, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( entity == RawDataEntity.Value && !uuids.Count( StringUuidTools.IsStringUuidPair ).Equals( uuids.Length ) )
				throw new ArgumentOutOfRangeException( "uuids", "The uuid string for uploading raw data for measurement value expects two uuids in this format: measurementUuid|characteristicUuid" );

			var result = new List<RawDataInformation>( uuids.Length );
			var targetSize = MaxUriLength - string.Format( "{1}rawData/{0}?uuids={{}}", entity, ServiceLocation ).Length;

			foreach( var uuidList in ArrayHelper.Split( uuids, targetSize, uuid => Uri.EscapeDataString( uuid ).Length + 3 /* Lenght of escaped "," */ ) )
			{
				var listString = RestClientHelper.ToListString( string.Join( ",", uuidList ) );
				var info = await Get<RawDataInformation[]>( string.Format( "rawData/{0}", entity ), cancellationToken, ParameterDefinition.Create( "uuids", listString ) ).ConfigureAwait( false );
				result.AddRange( info );
			}
			return result.ToArray();
		}

		/// <summary> 
		/// Fetches a list of information about the raw data for the <paramref name="entities"/>. 
		/// </summary>
		/// <remarks> Use this method to fetch raw data information for several <see cref="RawDataEntity"/> types in one call.</remarks>
		/// <param name="entities">The entities the raw data information should be fetched for.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<RawDataInformation[]> ListRawData( IEnumerable<RawDataTargetEntity> entities, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var entites = entities.GroupBy( p => p.Entity );
			return Task.Run( async delegate
			{
				var infoList = new List<RawDataInformation>();
				foreach( var entityGroup in entites )
				{
					infoList.AddRange( await ListRawData( entityGroup.Key, entityGroup.Select( p => p.Uuid ).ToArray(), cancellationToken ).ConfigureAwait( false ) );
				}
				return infoList.ToArray();
			}, cancellationToken );
		}

		#endregion

		#region Fetching of raw data entries

		/// <summary> 
		/// Fetches raw data for the part with <paramref name="partUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="partUuid">The uuid of the part to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified part.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<byte[]> GetRawDataForPart( Guid partUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetBytes( string.Format( "rawData/{0}/{1}/{2}", RawDataEntity.Part, partUuid, rawDataKey ),
				cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the characteristic with <paramref name="characteristicUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="characteristicUuid">The uuid of the characteristic to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified characteristic.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<byte[]> GetRawDataForCharacteristic( Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetBytes( string.Format( "rawData/{0}/{1}/{2}", RawDataEntity.Characteristic, characteristicUuid, rawDataKey ),
				cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the measurement with <paramref name="measurementUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="measurementUuid">The uuid of the measurement to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified measurement.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<byte[]> GetRawDataForMeasurement( Guid measurementUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetBytes( string.Format( "rawData/{0}/{1}/{2}", RawDataEntity.Measurement, measurementUuid, rawDataKey ),
				cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the value identified  by the compound key of <paramref name="measurementUuid"/> and <paramref name="characteristicUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="measurementUuid">The uuid of the measurement to fetch the raw data object for.</param>
		/// <param name="characteristicUuid">The uuid of the characteristic to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified measurement value.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<byte[]> GetRawDataForValue( Guid measurementUuid, Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetBytes( string.Format( "rawData/{0}/{1}|{2}/{3}", RawDataEntity.Value, measurementUuid, characteristicUuid, rawDataKey ),
				cancellationToken );
		}
		
		/// <summary>
		/// Fetches raw data as a byte array for the raw data item identified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="info">The <see cref="RawDataInformation"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<byte[]> GetRawData( RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetBytes( string.Format( "rawData/{0}/{1}/{2}", info.Target.Entity, info.Target.Uuid, info.Key ),
				cancellationToken );
		}

		/// <summary>
		/// Fetches raw data as a stream for the raw data item identified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="info">The <see cref="RawDataInformation"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task<Stream> GetRawDataStream( RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return GetStream( string.Format( "rawData/{0}/{1}/{2}", info.Target.Entity, info.Target.Uuid, info.Key ),
				cancellationToken );
		}

		#endregion

		#region Fetching of preview thumbnails for raw data entries

		/// <summary> 
		/// Fetches a preview image for the specified <code>info</code>. 
		/// </summary>
		/// <param name="info">The <see cref="RawDataInformation"/> that identifies the raw data object to fetch the preview image for.</param>
		/// <returns>The preview image as byte array.</returns>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<byte[]> GetRawDataThumbnail( RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			try
			{
				return await GetBytes( string.Format( "rawData/{0}/{1}/{2}/thumbnail", info.Target.Entity, info.Target.Uuid, info.Key ),
					cancellationToken ).ConfigureAwait( false );
			}
			catch( WrappedServerErrorException ex )
			{
				if( ex.StatusCode != HttpStatusCode.NotFound )
					throw;
			}
			return null;
		}

		/// <summary> 
		/// Fetches a preview image for the specified <code>info</code>. 
		/// </summary>
		/// <param name="info">The <see cref="RawDataInformation"/> that identifies the raw data object to fetch the preview image for.</param>
		/// <returns>The preview image as stream.</returns>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public async Task<Stream> GetRawDataThumbnailStream( RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			try
			{
				return await GetStream( string.Format( "rawData/{0}/{1}/{2}/thumbnail", info.Target.Entity, info.Target.Uuid, info.Key ),
					cancellationToken ).ConfigureAwait( false );
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
		/// Creates a new raw data object <paramref name="data"/> for the element identified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformation"/> object containing the <see cref="RawDataEntity"/> type and the uuid of the raw data that should be uploaded.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <remarks>
		/// If key speciefied by <see cref="RawDataInformation.Key"/> is -1, a new key will be chosen by the server automatically. This is the preferred way.
		/// </remarks>
		public Task CreateRawData( RawDataInformation info, Stream data, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return UploadRawData( info, data, HttpMethod.Post, cancellationToken );
		}

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
			return UploadRawData( info, new MemoryStream( data, 0, data.Length, false, true ), HttpMethod.Post, cancellationToken );
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
			return UploadRawData( info, new MemoryStream( data, 0, data.Length, false, true ), HttpMethod.Put, cancellationToken );
		}

		/// <summary>
		/// Updates the raw data object <paramref name="data"/> for the element identified in <paramref name="info"/>.
		/// </summary>
		/// <param name="data">The raw data to be uploaded.</param>
		/// <param name="info">The <see cref="RawDataInformation"/> object containing the <see cref="RawDataEntity"/> type, the uuid and the key of the raw data that should be updated.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task UpdateRawData( RawDataInformation info, Stream data, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( info.Key < 0 )
				throw new InvalidOperationException( "Unable to update raw data object: A key must be specified." );
			return UploadRawData( info, data, HttpMethod.Put, cancellationToken );
		}

		private Task UploadRawData( RawDataInformation info, Stream data, HttpMethod method, CancellationToken cancellationToken )
		{
			if( info.Target.Entity == RawDataEntity.Value && !StringUuidTools.IsStringUuidPair( info.Target.Uuid ) )
				throw new ArgumentOutOfRangeException( "info", "The uuid string for uploading raw data for measurement value needs 2 uuids in the format: {measurementUuid}|{characteristicUuid}" );

			if( String.IsNullOrEmpty( info.FileName ) )
				throw new ArgumentException( "FileName needs to be set.", "info" );

			var requestString = info.Key >= 0
				? string.Format( "rawData/{0}/{1}/{2}", info.Target.Entity, info.Target.Uuid, info.Key )
				: string.Format( "rawData/{0}/{1}", info.Target.Entity, info.Target.Uuid );

			if( method.Equals( HttpMethod.Post ) )
				return Post( requestString, data, cancellationToken, info.Size, info.MimeType, info.MD5, info.FileName );

			if( method.Equals( HttpMethod.Put ) )
				return Put( requestString, data, cancellationToken, info.Size, info.MimeType, info.MD5, info.FileName );

			throw new ArgumentOutOfRangeException( "method" );
		}

		#endregion

		#region Delete of raw data entries

		/// <summary>
		/// Deletes all raw data for the part identified by the uuid <paramref name="partUuid"/>.
		/// </summary>
		/// <param name="partUuid">The uuid of the part the raw data objects belongs to.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllRawDataForPart( Guid partUuid, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForPart( partUuid ), default( int ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes all raw data for the characteristic identified by the uuid <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="characteristicUuid">The uuid of the characteristic the raw data object belongs to. </param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllRawDataForCharacteristic( Guid characteristicUuid, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForCharacteristic( characteristicUuid ), default( int ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes all raw data for the measurement identified by the uuid <paramref name="measurementUuid"/>.
		/// </summary>
		/// <param name="measurementUuid">The uuid of the measurement the raw data object belongs to.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllRawDataForMeasurement( Guid measurementUuid, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForMeasurement( measurementUuid ), default( int ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes all raw data for the value identified by the compound key <paramref name="measurementUuid"/> and the characteristic with uuid <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="measurementUuid">The uuid of the measurement the raw data object belongs to.</param>
		/// <param name="characteristicUuid">The uuid of the characteristic the raw data object belongs to.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteAllRawDataForValue( Guid measurementUuid, Guid characteristicUuid, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid ), default( int ) ), cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> and the part with uuid <paramref name="partUuid"/>.
		/// </summary>
		/// <param name="partUuid">The uuid of the part the raw data objects belongs to.</param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteRawDataForPart( Guid partUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForPart( partUuid ), rawDataKey ), cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> for the characteristic with uuid <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="characteristicUuid">The uuid of the part the raw data objects belongs to. </param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteRawDataForCharacteristic( Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForCharacteristic( characteristicUuid ), rawDataKey ), cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> for the measurement with uuid <paramref name="measurementUuid"/>.
		/// </summary>
		/// <param name="measurementUuid">The uuid of the part the raw data objects belongs to. </param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteRawDataForMeasurement( Guid measurementUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForMeasurement( measurementUuid ), rawDataKey ), cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the compound key <paramref name="measurementUuid"/> and <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="measurementUuid">The uuid of the measurement the raw data objects belongs to. </param>
		/// <param name="characteristicUuid">The uuid of the characteristic the raw data object belongs to.</param>
		/// <param name="rawDataKey">The key of the raw data object that should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteRawDataForValue( Guid measurementUuid, Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return DeleteRawData( new RawDataInformation( RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid ), rawDataKey ), cancellationToken );
		}

		/// <summary>
		/// Deletes raw data for the element identified by <paramref name="info"/>.
		/// </summary>
		/// <param name="info">The <see cref="RawDataInformation"/> object containing the <see cref="RawDataEntity"/> type, the uuid and the key of the raw data that should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public Task DeleteRawData( RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			if( info.Target.Entity == RawDataEntity.Value && !StringUuidTools.IsStringUuidPair( info.Target.Uuid ) )
				throw new ArgumentOutOfRangeException( "info", "The uuid string for uploading raw data for measurement value needs two uuids in the format: {measurementUuid}|{characteristicUuid}" );

			var url = info.Key >= 0
				? string.Format( "rawData/{0}/{{{1}}}/{2}", info.Target.Entity, info.Target.Uuid, info.Key )
				: string.Format( "rawData/{0}/{{{1}}}", info.Target.Entity, info.Target.Uuid );

			return Delete( url, cancellationToken );
		}

		#endregion
	}
}