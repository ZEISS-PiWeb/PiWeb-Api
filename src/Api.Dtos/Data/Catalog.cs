#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	#region using

	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class represents a concrete catalog with a description of possible catalog attributes (<see cref="ValidAttributes"/>) for each entry (i.e. the colums of a catalog) and
	/// the catalog entries (<see cref="CatalogEntries"/>).
	/// </summary>
	public class Catalog : SimpleCatalog
	{
		#region members

		private readonly Dictionary<short, CatalogEntry> _Dictionary = new Dictionary<short, CatalogEntry>();
		private CatalogEntry[] _CatalogEntries = new CatalogEntry[ 0 ];
		private ushort[] _ValidAttributes = new ushort[ 0 ];

		#endregion

		#region properties

		/// <summary>
		/// Returns the catalog entry with index <code>key</code>.
		/// </summary>
		public CatalogEntry this[ short key ]
		{
			get
			{
				CatalogEntry result;
				_Dictionary.TryGetValue( key, out result );

				return result;
			}
		}

		/// <summary>
		/// Gets or sets a list of possible catalog entry attribute (i.e. columns of a catalog).
		/// </summary>
		[JsonProperty( "validAttributes" )]
		public ushort[] ValidAttributes
		{
			[NotNull]
			get { return _ValidAttributes; }
			set { _ValidAttributes = value ?? new ushort[ 0 ]; }
		}

		/// <summary>
		/// Gets or sets the list of catalog entries that belong to this catalog.
		/// </summary>
		[JsonProperty( "catalogEntries" )]
		public CatalogEntry[] CatalogEntries
		{
			[NotNull]
			get { return _CatalogEntries; }
			set
			{
				_Dictionary.Clear();

				_CatalogEntries = value ?? new CatalogEntry[0];
				foreach( var entry in _CatalogEntries )
				{
					_Dictionary[ entry.Key ] = entry;
				}
			}
		}

		#endregion
	}
}
