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
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// This class represents the base class for cataalogs. The concrete class for catalogs that also contains the catalog entries is <see cref="CatalogDto"/>.
	/// A catalog is identified by an <see cref="Uuid"/> and a catalog has a <see cref="Name"/>.
	/// </summary>
	public class SimpleCatalogDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="SimpleCatalogDto"/> class.
		/// </summary>
		public SimpleCatalogDto()
		{
			Name = "";
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the uuid of this catalog.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "uuid" )]
		[JsonPropertyName( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the name of the catalog.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "name" )]
		[JsonPropertyName( "name" )]
		public string Name { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return Name;
		}

		#endregion
	}
}