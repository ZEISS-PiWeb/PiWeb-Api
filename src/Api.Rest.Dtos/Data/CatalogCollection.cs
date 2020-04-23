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
	/// Special class that holds a list of <see cref="Catalog"/>.
	/// </summary>
	public class CatalogCollection : IEnumerable<Catalog>
	{
		#region members

		private readonly Dictionary<Guid, Catalog> _Catalogs = new Dictionary<Guid, Catalog>();

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogCollection"/> class.
		/// </summary>
		public CatalogCollection()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogCollection"/> class.
		/// </summary>
		public CatalogCollection( IEnumerable<Catalog> catalogs )
		{
			foreach( var cat in catalogs ?? new Catalog[ 0 ] )
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
		public Catalog this[ Guid catalogUuid ]
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
		public CatalogEntry this[ Guid catalogUuid, short catalogEntryIndex ]
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
		public CatalogEntry this[ Guid catalogUuid, string catalogEntryIndex ]
		{
			get
			{
				return !short.TryParse( catalogEntryIndex, NumberStyles.Integer, CultureInfo.InvariantCulture, out var index ) ? null : this[ catalogUuid, index ];
			}
		}

		#endregion

		#region interface IEnumerable<Catalog>

		/// <inheritdoc />
		public IEnumerator<Catalog> GetEnumerator()
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