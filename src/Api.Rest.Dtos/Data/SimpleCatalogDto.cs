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
	using Newtonsoft.Json;

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
		[JsonProperty( "uuid" )]
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the name of the catalog.
		/// </summary>
		[JsonProperty( "name" )]
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