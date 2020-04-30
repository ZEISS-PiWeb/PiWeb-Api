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

	using System.Collections.Generic;
	using JetBrains.Annotations;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class represents a concrete catalog with a description of possible catalog attributes (<see cref="ValidAttributes"/>)
	/// for each entry (i.e. the colums of a catalog) and the catalog entries (<see cref="CatalogEntries"/>).
	/// </summary>
	public class CatalogDto : SimpleCatalogDto
	{
		#region members

		private readonly Dictionary<short, CatalogEntryDto> _Dictionary = new Dictionary<short, CatalogEntryDto>();
		private CatalogEntryDto[] _CatalogEntries = new CatalogEntryDto[ 0 ];
		private ushort[] _ValidAttributes = new ushort[ 0 ];

		#endregion

		#region properties

		/// <summary>
		/// Returns the catalog entry with index <code>key</code>.
		/// </summary>
		public CatalogEntryDto this[ short key ]
		{
			get
			{
				_Dictionary.TryGetValue( key, out var result );
				return result;
			}
		}

		/// <summary>
		/// Gets or sets a list of possible catalog entry attribute (i.e. columns of a catalog).
		/// </summary>
		[JsonProperty( "validAttributes" )]
		public ushort[] ValidAttributes
		{
			[NotNull] get => _ValidAttributes;
			set => _ValidAttributes = value ?? new ushort[ 0 ];
		}

		/// <summary>
		/// Gets or sets the list of catalog entries that belong to this catalog.
		/// </summary>
		[JsonProperty( "catalogEntries" )]
		public CatalogEntryDto[] CatalogEntries
		{
			[NotNull] get => _CatalogEntries;
			set
			{
				_Dictionary.Clear();

				_CatalogEntries = value ?? new CatalogEntryDto[ 0 ];
				foreach( var entry in _CatalogEntries )
				{
					_Dictionary[ entry.Key ] = entry;
				}
			}
		}

		#endregion
	}
}