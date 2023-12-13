#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Linq;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// This class represents a concrete catalog with a description of possible catalog attributes (<see cref="ValidAttributes"/>)
	/// for each entry (i.e. the colums of a catalog) and the catalog entries (<see cref="CatalogEntries"/>).
	/// </summary>
	public class CatalogDto : SimpleCatalogDto
	{
		#region members

		private Dictionary<short, CatalogEntryDto> _Dictionary;
		private IReadOnlyList<CatalogEntryDto> _CatalogEntries = Array.Empty<CatalogEntryDto>();
		private IReadOnlyList<ushort> _ValidAttributes = Array.Empty<ushort>();

		#endregion

		#region properties

		/// <summary>
		/// Returns the catalog entry with index <code>key</code>.
		/// </summary>
		public CatalogEntryDto this[ short key ]
		{
			get
			{
				_Dictionary ??= _CatalogEntries.ToDictionary( k => k.Key, v => v );
				_Dictionary.TryGetValue( key, out var result );

				return result;
			}
		}

		/// <summary>
		/// Gets or sets a list of possible catalog entry attribute (i.e. columns of a catalog).
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "validAttributes" )]
		[JsonPropertyName( "validAttributes" )]
		public IReadOnlyList<ushort> ValidAttributes
		{
			[NotNull] get => _ValidAttributes;
			set => _ValidAttributes = value ?? Array.Empty<ushort>();
		}

		/// <summary>
		/// Gets or sets the list of catalog entries that belong to this catalog.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "catalogEntries" )]
		[JsonPropertyName( "catalogEntries" )]
		public IReadOnlyList<CatalogEntryDto> CatalogEntries
		{
			[NotNull] get => _CatalogEntries;
			set
			{
				_Dictionary = null;
				_CatalogEntries = value ?? Array.Empty<CatalogEntryDto>();
			}
		}

		#endregion
	}
}