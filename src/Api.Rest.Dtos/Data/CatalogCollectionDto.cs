#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Globalization;

	#endregion

	/// <summary>
	/// Special class that holds a list of <see cref="CatalogDto"/>.
	/// </summary>
	public class CatalogCollectionDto : IEnumerable<CatalogDto>
	{
		#region members

		private readonly Dictionary<Guid, CatalogDto> _Catalogs = new Dictionary<Guid, CatalogDto>();

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogCollectionDto"/> class.
		/// </summary>
		public CatalogCollectionDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogCollectionDto"/> class.
		/// </summary>
		public CatalogCollectionDto( IEnumerable<CatalogDto> catalogs )
		{
			foreach( var cat in catalogs ?? Array.Empty<CatalogDto>() )
			{
				_Catalogs.Add( cat.Uuid, cat );
			}
		}

		#endregion

		#region properties

		/// <summary>
		/// Returns the number of catalogs in this list.
		/// </summary>
		public int Count => _Catalogs.Count;

		/// <summary>
		/// Returns the catalog with uuid <code>catalogUuid</code>.
		/// </summary>
		public CatalogDto this[ Guid catalogUuid ]
		{
			get
			{
				_Catalogs.TryGetValue( catalogUuid, out var result );
				return result;
			}
		}

		/// <summary>
		/// Returns the catalog entry with index <code>catalogEntryIndex</code> from catalog with uuid <code>catalogUuid</code>.
		/// </summary>
		public CatalogEntryDto this[ Guid catalogUuid, short catalogEntryIndex ]
		{
			get
			{
				var catalog = this[ catalogUuid ];
				return catalog?[ catalogEntryIndex ];
			}
		}

		/// <summary>
		/// Returns the catalog entry with index <code>catalogEntryIndex</code> from catalog with uuid <code>catalogUuid</code>.
		/// </summary>
		/// <remarks>
		/// The index <code>catalogEntryIndex</code> has to be a valid short.
		/// </remarks>
		public CatalogEntryDto this[ Guid catalogUuid, string catalogEntryIndex ]
		{
			get
			{
				return !short.TryParse( catalogEntryIndex, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) ? null : this[ catalogUuid, index ];
			}
		}

		#endregion

		#region interface IEnumerable<CatalogDto>

		/// <inheritdoc />
		public IEnumerator<CatalogDto> GetEnumerator()
		{
			return _Catalogs.Values.GetEnumerator();
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		#endregion
	}
}