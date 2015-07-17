#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System.Collections.Generic;

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This element (in combination with its base element) describes the entity Catalog with its relation to zero or more catalog entries.
	/// When received from the DataService as a result of <code>CatalogueSearch</code>, the element 
	/// <code>validAttributes</code> contains a list of attribute keys, that are valid for the respective catalog.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.CatalogConverter ) )]
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
		/// Gets or sets a list of attribute keys that are valid for the entries of this catalog.
		/// </summary>
		public ushort[] ValidAttributes
		{
			get { return _ValidAttributes; }
			set { _ValidAttributes = value ?? new ushort[ 0 ]; }
		}

		/// <summary>
		/// Gets or sets the list of catalog entries in this catalog.
		/// </summary>
		public CatalogEntry[] CatalogEntries
		{
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
