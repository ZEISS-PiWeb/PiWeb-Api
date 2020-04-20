#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.DataService.Rest
{
	#region usings

	using System;
	using System.Threading;
	using System.Threading.Tasks;
	using Zeiss.PiWeb.Api.Contracts;
	using Zeiss.PiWeb.Api.Dtos.Data;

	#endregion

	/// <summary>
	/// Client class for communicating with the REST based data service.
	/// </summary>
	public static class DataServiceRestClientExtensions
	{
		#region methods

		/// <summary>
		/// Adds a new attribute definition to the database configuration for the specified <paramref name="entity"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="entity">The entity the attribute definition should be added to.</param>
		/// <param name="definition">The attribute definition to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task CreateAttributeDefinition<T>( this IDataServiceRestClientBase<T> client, Entity entity, AbstractAttributeDefinition definition, CancellationToken cancellationToken = default( CancellationToken ) ) where T : DataServiceFeatureMatrix
		{
			return client.CreateAttributeDefinitions( entity, new[] { definition }, cancellationToken );
		}

		/// <summary> 
		/// Adds the specified catalog entry to the catalog with uuid <paramref name="catalogUuid"/>. If the key <see cref="CatalogEntry.Key"/>
		/// is <code>-1</code>, the server will generate a new unique key for that entry.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="catalogUuid">The uuid of the catalog to add the entry to.</param>
		/// <param name="entry">The catalog entry to add.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task CreateCatalogEntry<T>( this IDataServiceRestClientBase<T> client, Guid catalogUuid, CatalogEntry entry, CancellationToken cancellationToken = default( CancellationToken ) ) where T : DataServiceFeatureMatrix
		{
			return client.CreateCatalogEntries( catalogUuid, new[] { entry }, cancellationToken );
		}

		/// <summary> 
		/// Removes the catalog entry with the specified <paramref name="key"/> from the catalog <paramref name="catalogUuid"/>.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="catalogUuid">The uuid of the catalog to remove the entry from.</param>
		/// <param name="key">The key of the catalog entry to delete.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task DeleteCatalogEntry<T>( this IDataServiceRestClientBase<T> client, Guid catalogUuid, short key, CancellationToken cancellationToken = default( CancellationToken ) ) where T : DataServiceFeatureMatrix
		{
			return client.DeleteCatalogEntries( catalogUuid, new[] { key }, cancellationToken );
		}

		/// <summary> 
		/// Deletes all parts from the database. Since parts act as the parent of characteristics and measurements, this call will 
		/// delete all characteristics and measurements including the measurement values too.
		/// </summary>
		/// <param name="client">The client class to use.</param>
		/// <param name="cancellationToken">A token to cancel the asynchronous operation.</param>
		public static Task DeleteAllParts<T>( this IDataServiceRestClientBase<T> client, CancellationToken cancellationToken = default( CancellationToken ) ) where T : DataServiceFeatureMatrix
		{
			return client.DeleteParts( PathInformation.Root, cancellationToken );
		}


		#endregion
	}
}