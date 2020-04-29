#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.RawData
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Contracts;
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.RawData;
	using Zeiss.PiWeb.Api.Rest.HttpClient.RawData.Filter.Conditions;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based raw data service.
	/// </summary>
	public static class RawDataServiceRestClientExtensions
	{
		#region methods

		/// <summary> 
		/// Fetches a list of raw data information. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="target">The target entity to fetch the raw data information.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> or <paramref name="target"/> is <see langword="null" />.</exception>
		public static Task<RawDataInformation[]> ListRawData( [NotNull] this IRawDataServiceRestClient client, [NotNull] RawDataTargetEntity target, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			if( target == null ) throw new ArgumentNullException( nameof( target ) );

			return client.ListRawData( target.Entity, new[] { target.Uuid }, null, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of information about the raw data for the <paramref name="entities"/>. 
		/// </summary>
		/// <remarks> Use this method to fetch raw data information for several <see cref="RawDataEntity"/> types in one call.</remarks>
		/// <param name="client">The client class to use.</param>
		/// <param name="entities">The entities the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> or <paramref name="entities"/> is <see langword="null" />.</exception>
		public static Task<RawDataInformation[]> ListRawData( [NotNull] this IRawDataServiceRestClient client, [NotNull] IEnumerable<RawDataTargetEntity> entities, FilterCondition filter = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			if( entities == null ) throw new ArgumentNullException( nameof( entities ) );

			async Task<RawDataInformation[]> ListRawData()
			{
				var infoList = new List<RawDataInformation>();
				foreach( var group in entities.GroupBy( p => p.Entity ) )
				{
					var infos = await client.ListRawData( group.Key, group.Select( p => p.Uuid.ToString() ).ToArray(), filter, cancellationToken ).ConfigureAwait( false );
					infoList.AddRange( infos );
				}

				return infoList.ToArray();
			}

			return ListRawData();
		}

		/// <summary> 
		/// Fetches a list of raw data information for the parts identified by <paramref name="partUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="partUuids">The list of part uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<RawDataInformation[]> ListRawDataForParts( [NotNull] this IRawDataServiceRestClient client, [CanBeNull] Guid[] partUuids, FilterCondition filter = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.ListRawData( RawDataEntity.Part, partUuids?.Select( u => u.ToString() ).ToArray(), filter, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the characteristic identified by <paramref name="charateristicUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="charateristicUuids">The list of characteristic uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<RawDataInformation[]> ListRawDataForCharacteristics( [NotNull] this IRawDataServiceRestClient client, [CanBeNull] Guid[] charateristicUuids, FilterCondition filter = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.ListRawData( RawDataEntity.Characteristic, charateristicUuids?.Select( u => u.ToString() ).ToArray(), filter, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the measurements identified by <paramref name="measurementUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementUuids">The list of measurement uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<RawDataInformation[]> ListRawDataForMeasurements( [NotNull] this IRawDataServiceRestClient client, [CanBeNull] Guid[] measurementUuids, FilterCondition filter = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.ListRawData( RawDataEntity.Measurement, measurementUuids?.Select( u => u.ToString() ).ToArray(), filter, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the measurement values identified by <paramref name="measurementValueUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementValueUuids">The list of value uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<RawDataInformation[]> ListRawDataForValues( [NotNull] this IRawDataServiceRestClient client, [CanBeNull] ValueRawDataIdentifier[] measurementValueUuids, FilterCondition filter = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.ListRawData( RawDataEntity.Value, measurementValueUuids?.Select( StringUuidTools.CreateStringUuidPair ).ToArray(), filter, cancellationToken );
		}

		/// <summary>
		/// Fetches raw data as a byte array for the raw data item identified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="info">The <see cref="RawDataInformation"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> or <paramref name="info"/> is <see langword="null" />.</exception>
		public static Task<byte[]> GetRawData( [NotNull] this IRawDataServiceRestClient client, [NotNull] RawDataInformation info, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			if( info == null ) throw new ArgumentNullException( nameof( info ) );

			return client.GetRawData( info.Target, info.Key.GetValueOrDefault(), info.MD5, cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the part with <paramref name="partUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="partUuid">The uuid of the part to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified part.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<byte[]> GetRawDataForPart( [NotNull] this IRawDataServiceRestClient client, Guid partUuid, int rawDataKey, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.GetRawData( RawDataTargetEntity.CreateForPart( partUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the characteristic with <paramref name="characteristicUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="characteristicUuid">The uuid of the characteristic to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified characteristic.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<byte[]> GetRawDataForCharacteristic( [NotNull] this IRawDataServiceRestClient client, Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.GetRawData( RawDataTargetEntity.CreateForCharacteristic( characteristicUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the measurement with <paramref name="measurementUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use for fetching the raw data.</param>
		/// <param name="measurementUuid">The uuid of the measurement to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified measurement.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<byte[]> GetRawDataForMeasurement( [NotNull] this IRawDataServiceRestClient client, Guid measurementUuid, int rawDataKey, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.GetRawData( RawDataTargetEntity.CreateForMeasurement( measurementUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the value identified  by the compound key of <paramref name="measurementUuid"/> and <paramref name="characteristicUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementUuid">The uuid of the measurement to fetch the raw data object for.</param>
		/// <param name="characteristicUuid">The uuid of the characteristic to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified measurement value.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task<byte[]> GetRawDataForValue( [NotNull] this IRawDataServiceRestClient client, Guid measurementUuid, Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.GetRawData( RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the <paramref name="info"/>.
		/// </summary>
		/// <param name="info">The raw data entry to delete.</param>
		/// <param name="client">The client class to use.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> or <paramref name="info"/> is <see langword="null" />.</exception>
		public static Task DeleteRawData( [NotNull] this IRawDataServiceRestClient client, [NotNull] RawDataInformation info, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			if( info == null ) throw new ArgumentNullException( nameof( info ) );

			return client.DeleteRawData( info.Target, info.Key, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> and the part with uuid <paramref name="partUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="partUuid">The uuid of the part the raw data objects belongs to.</param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task DeleteRawDataForPart( [NotNull] this IRawDataServiceRestClient client, Guid partUuid, int? rawDataKey = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.DeleteRawData( RawDataTargetEntity.CreateForPart( partUuid ), rawDataKey, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> for the characteristic with uuid <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="characteristicUuid">The uuid of the part the raw data objects belongs to. </param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task DeleteRawDataForCharacteristic( [NotNull] this IRawDataServiceRestClient client, Guid characteristicUuid, int? rawDataKey = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.DeleteRawData( RawDataTargetEntity.CreateForCharacteristic( characteristicUuid ), rawDataKey, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> for the measurement with uuid <paramref name="measurementUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementUuid">The uuid of the part the raw data objects belongs to. </param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task DeleteRawDataForMeasurement( [NotNull] this IRawDataServiceRestClient client, Guid measurementUuid, int? rawDataKey = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.DeleteRawData( RawDataTargetEntity.CreateForMeasurement( measurementUuid ), rawDataKey, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the compound key <paramref name="measurementUuid"/> and <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementUuid">The uuid of the measurement the raw data objects belongs to. </param>
		/// <param name="characteristicUuid">The uuid of the characteristic the raw data object belongs to.</param>
		/// <param name="rawDataKey">The key of the raw data object that should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <exception cref="ArgumentNullException"><paramref name="client"/> is <see langword="null" />.</exception>
		public static Task DeleteRawDataForValue( [NotNull] this IRawDataServiceRestClient client, Guid measurementUuid, Guid characteristicUuid, int? rawDataKey = null, CancellationToken cancellationToken = default )
		{
			if( client == null ) throw new ArgumentNullException( nameof( client ) );
			return client.DeleteRawData( RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid ), rawDataKey, cancellationToken );
		}

		#endregion
	}
}