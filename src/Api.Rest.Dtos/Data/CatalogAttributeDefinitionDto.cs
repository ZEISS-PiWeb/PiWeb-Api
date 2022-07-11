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
	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converters;

	#endregion

	/// <summary>
	/// Defines an entity's attribute which is based on a <see cref="Catalog"/>.
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( AttributeDefinitionConverter ) )]
	[JsonConverter( typeof( AttributeDefinitionJsonConverter ) )]
	public sealed class CatalogAttributeDefinitionDto : AbstractAttributeDefinitionDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogAttributeDefinitionDto"/> class.
		/// </summary>
		public CatalogAttributeDefinitionDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="CatalogAttributeDefinitionDto"/> class.
		/// </summary>
		/// <param name="key">The unique key for this attribute</param>
		/// <param name="description">The attribute description</param>
		/// <param name="catalogUuid">The uuid of the catalog this attribute definition points to.</param>
		/// <param name="queryEfficient"><code>true</code> if this attribute is efficient for filtering operations</param>
		public CatalogAttributeDefinitionDto( ushort key, string description, Guid catalogUuid, bool queryEfficient = false )
			: base( key, description, queryEfficient )
		{
			Catalog = catalogUuid;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the uuid of the catalog this attribute definition points to.
		/// </summary>
		public Guid Catalog { get; set; }

		#endregion
	}
}