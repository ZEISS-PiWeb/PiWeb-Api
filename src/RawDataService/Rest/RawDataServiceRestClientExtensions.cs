#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.IMT.PiWeb.Api.Common.Data;
	using Zeiss.IMT.PiWeb.Api.RawDataService.Filter.Conditions;

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
		public static Task<RawDataInformation[]> ListRawData( this IRawDataServiceRestClient client, RawDataTargetEntity target, CancellationToken cancellationToken = default( CancellationToken ) )
		{
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
		public static async Task<RawDataInformation[]> ListRawData( this IRawDataServiceRestClient client, IEnumerable<RawDataTargetEntity> entities, FilterCondition filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			var infoList = new List<RawDataInformation>();
			foreach( var group in entities.GroupBy( p => p.Entity ) )
			{
				var infos = await client.ListRawData( group.Key, group.Select( p => p.Uuid.ToString() ).ToArray(), filter, cancellationToken ).ConfigureAwait( false );
				infoList.AddRange( infos );
			}
			return infoList.ToArray();
		}

		/// <summary> 
		/// Fetches a list of raw data information for the parts identified by <paramref name="partUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="partUuids">The list of part uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<RawDataInformation[]> ListRawDataForParts( this IRawDataServiceRestClient client, Guid[] partUuids, FilterCondition filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.ListRawData( RawDataEntity.Part, partUuids?.Select( u => u.ToString() ).ToArray(), filter, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the characteristic identified by <paramref name="charateristicUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="charateristicUuids">The list of characteristic uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<RawDataInformation[]> ListRawDataForCharacteristics( this IRawDataServiceRestClient client, Guid[] charateristicUuids, FilterCondition filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.ListRawData( RawDataEntity.Characteristic, charateristicUuids?.Select( u => u.ToString() ).ToArray(), filter, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the measurements identified by <paramref name="measurementUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementUuids">The list of measurement uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<RawDataInformation[]> ListRawDataForMeasurements( this IRawDataServiceRestClient client, Guid[] measurementUuids, FilterCondition filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.ListRawData( RawDataEntity.Measurement, measurementUuids?.Select( u => u.ToString() ).ToArray(), filter, cancellationToken );
		}

		/// <summary> 
		/// Fetches a list of raw data information for the measurement values identified by <paramref name="measurementValueUuids"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementValueUuids">The list of value uuids the raw data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<RawDataInformation[]> ListRawDataForValues( this IRawDataServiceRestClient client, ValueRawDataIdentifier[] measurementValueUuids, FilterCondition filter = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.ListRawData( RawDataEntity.Value, measurementValueUuids?.Select( StringUuidTools.CreateStringUuidPair ).ToArray(), filter, cancellationToken );
		}

		/// <summary>
		/// Fetches raw data as a byte array for the raw data item identified by <paramref name="info"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="info">The <see cref="RawDataInformation"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<byte[]> GetRawData( this IRawDataServiceRestClient client, RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.GetRawData( info.Target, info.Key.GetValueOrDefault(), info.MD5, cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the part with <paramref name="partUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="partUuid">The uuid of the part to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified part.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<byte[]> GetRawDataForPart( this IRawDataServiceRestClient client, Guid partUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.GetRawData( RawDataTargetEntity.CreateForPart( partUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the characteristic with <paramref name="characteristicUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="characteristicUuid">The uuid of the characteristic to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified characteristic.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<byte[]> GetRawDataForCharacteristic( this IRawDataServiceRestClient client, Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.GetRawData( RawDataTargetEntity.CreateForCharacteristic( characteristicUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary> 
		/// Fetches raw data for the measurement with <paramref name="measurementUuid"/> and raw data index <paramref name="rawDataKey"/>. 
		/// </summary>
		/// <param name="client">The client class to use for fetching the raw data.</param>
		/// <param name="measurementUuid">The uuid of the measurement to fetch the raw data object for.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified measurement.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task<byte[]> GetRawDataForMeasurement( this IRawDataServiceRestClient client, Guid measurementUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
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
		public static Task<byte[]> GetRawDataForValue( this IRawDataServiceRestClient client, Guid measurementUuid, Guid characteristicUuid, int rawDataKey, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.GetRawData( RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid ), rawDataKey, cancellationToken: cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the <paramref name="info"/>.
		/// </summary>
		/// <param name="info">The raw data entry to delete.</param>
		/// <param name="client">The client class to use.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task DeleteRawData( this IRawDataServiceRestClient client, RawDataInformation info, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.DeleteRawData( info.Target, info.Key, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> and the part with uuid <paramref name="partUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="partUuid">The uuid of the part the raw data objects belongs to.</param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task DeleteRawDataForPart( this IRawDataServiceRestClient client, Guid partUuid, int? rawDataKey = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.DeleteRawData( RawDataTargetEntity.CreateForPart( partUuid ), rawDataKey, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> for the characteristic with uuid <paramref name="characteristicUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="characteristicUuid">The uuid of the part the raw data objects belongs to. </param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task DeleteRawDataForCharacteristic( this IRawDataServiceRestClient client, Guid characteristicUuid, int? rawDataKey = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.DeleteRawData( RawDataTargetEntity.CreateForCharacteristic( characteristicUuid ), rawDataKey, cancellationToken );
		}

		/// <summary>
		/// Deletes the raw data object identified by the key <paramref name="rawDataKey"/> for the measurement with uuid <paramref name="measurementUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="measurementUuid">The uuid of the part the raw data objects belongs to. </param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task DeleteRawDataForMeasurement( this IRawDataServiceRestClient client, Guid measurementUuid, int? rawDataKey = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
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
		public static Task DeleteRawDataForValue( this IRawDataServiceRestClient client, Guid measurementUuid, Guid characteristicUuid, int? rawDataKey = null, CancellationToken cancellationToken = default( CancellationToken ) )
		{
			return client.DeleteRawData( RawDataTargetEntity.CreateForValue( measurementUuid, characteristicUuid ), rawDataKey, cancellationToken );
		}

		#endregion
	}
}