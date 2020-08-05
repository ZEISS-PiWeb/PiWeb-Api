#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
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
	using Zeiss.PiWeb.Api.Rest.Dtos;
	using Zeiss.PiWeb.Api.Rest.Dtos.RawData;

	#endregion

	public interface IRawDataServiceRestClientBase<T> where T : RawDataServiceFeatureMatrix
	{
		#region methods

		/// <summary>
		/// Method for fetching the <see cref="ServiceInformationDto"/>. This method can be used for connection checking. The call returns quickly
		/// and does not produce any noticeable server load.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		Task<ServiceInformationDto> GetServiceInformation( CancellationToken cancellationToken = default );

		/// <summary>
		/// Method for fetching the <see cref="InterfaceVersionRange"/>.
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		Task<InterfaceVersionRange> GetInterfaceInformation( CancellationToken cancellationToken = default );

		/// <summary>
		/// Method for fetching the <see cref="RawDataServiceFeatureMatrix"/>
		/// </summary>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		/// <returns></returns>
		Task<T> GetFeatureMatrix( CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of raw data information for the <paramref name="entity"/> identified by <paramref name="uuids"/> and filtered by <paramref name="filter"/>.
		/// Either <paramref name="uuids" /> or <paramref name="filter"/> must have a value.
		/// </summary>
		/// <param name="entity">The <see cref="RawDataEntityDto"/> the raw data information should be fetched for.</param>
		/// <param name="uuids">The list of value uuids the data information should be fetched for.</param>
		/// <param name="filter">A condition used to filter the result.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		/// <exception cref="InvalidOperationException">No uuids and no filter was specified.</exception>
		/// <exception cref="OperationNotSupportedOnServerException">An attribute filter for raw data is not supported by this server.</exception>
		Task<RawDataInformationDto[]> ListRawData( RawDataEntityDto entity, string[] uuids, IFilterCondition filter = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches raw data as a byte array for the raw data item identified by <paramref name="target"/> and <paramref name="rawDataKey"/>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntityDto"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <param name="expectedMd5">The md5 check sum that is expected for the result object. If this value is set, performance is better because server side round trips are reduced.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		Task<byte[]> GetRawData( [NotNull] RawDataTargetEntityDto target, int rawDataKey, Guid? expectedMd5 = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a preview image for the specified <code>info</code>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntityDto"/> that specifies the raw data object that should be fetched.</param>
		/// <param name="rawDataKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <returns>The preview image as byte array.</returns>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		Task<byte[]> GetRawDataThumbnail( [NotNull] RawDataTargetEntityDto target, int rawDataKey, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches a list of entries if specified raw data is an archive of known format.
		/// </summary>
		/// <param name="targetEntity">The <see cref="RawDataTargetEntityDto"/> that specifies the archive</param>
		/// <param name="targetKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<RawDataArchiveIndexDto> GetRawDataArchiveEntries( RawDataTargetEntityDto targetEntity, int targetKey, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches multiple lists of entries for queried raw data objects if they are archives of known format.
		/// </summary>
		/// <param name="query"><see cref="RawDataBulkQueryDto"/> with selectors containing targets of requested archives.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		/// <returns></returns>
		Task<IEnumerable<RawDataArchiveIndexDto>> RawDataArchiveEntryQuery( RawDataBulkQueryDto query, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches file as byte array of specified raw data if it is part of an archive of known format.
		/// </summary>
		/// <param name="targetEntity">The <see cref="RawDataTargetEntityDto"/> that specifies the archive</param>
		/// <param name="targetKey">The unique key that identifies the raw data object for the specified target.</param>
		/// <param name="fileName">The requested file.</param>
		/// <param name="expectedMd5">The md5 check sum that is expected for the result object, used for caching.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<byte[]> GetRawDataArchiveContent( RawDataTargetEntityDto targetEntity, int targetKey, string fileName, Guid? expectedMd5 = null, CancellationToken cancellationToken = default );

		/// <summary>
		/// Fetches multiple files of queried raw data objects if they are part of selected archives of known format.
		/// </summary>
		/// <param name="query"><see cref="RawDataArchiveBulkQueryDto"/> with selectors containing targets of requested archives as well as requested files.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task<IEnumerable<RawDataArchiveContentDto>> RawDataArchiveContentQuery( RawDataArchiveBulkQueryDto query, CancellationToken cancellationToken = default );

		/// <summary>
		/// Creates a new raw data object <paramref name="data"/> for the element specified by <paramref name="info"/>.
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformationDto"/> object containing the <see cref="RawDataEntityDto"/> type and the uuid of the raw data that should be uploaded.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		/// <remarks>
		/// If key speciefied by <see cref="RawDataInformationDto.Key"/> is -1, a new key will be chosen by the server automatically. This is the preferred way.
		/// </remarks>
		Task CreateRawData( [NotNull] RawDataInformationDto info, [NotNull] byte[] data, CancellationToken cancellationToken = default );

		/// <summary>
		/// Updates the raw data object <paramref name="data"/> for the element identified by <paramref name="info"/>.
		/// </summary>
		/// <param name="data">The raw data to upload.</param>
		/// <param name="info">The <see cref="RawDataInformationDto"/> object containing the <see cref="RawDataEntityDto"/> type, the uuid and the key of the raw data that should be updated.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		Task UpdateRawData( [NotNull] RawDataInformationDto info, byte[] data, CancellationToken cancellationToken = default );

		/// <summary>
		/// Deletes raw data for the element identified by <paramref name="target"/> and <paramref name="rawDataKey"/>.
		/// </summary>
		/// <param name="target">The <see cref="RawDataTargetEntityDto"/> object containing the <see cref="RawDataEntityDto"/> type and the uuid of the raw data that should be deleted.</param>
		/// <param name="rawDataKey">The key of the raw data object which should be deleted.</param>
		/// <param name="cancellationToken">A token to cancel the hronous operation.</param>
		Task DeleteRawData( [NotNull] RawDataTargetEntityDto target, int? rawDataKey = null, CancellationToken cancellationToken = default );

		#endregion
	}
}