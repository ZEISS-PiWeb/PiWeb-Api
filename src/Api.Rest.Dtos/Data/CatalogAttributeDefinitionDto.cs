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
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// Defines an entity's attribute which is based on a <see cref="Catalog"/>.
	/// </summary>
	[JsonConverter( typeof( AttributeDefinitionConverter ) )]
	public class CatalogAttributeDefinitionDto : AbstractAttributeDefinitionDto
	{
		#region properties

		/// <summary>
		/// Gets or sets the uuid of the catalog this attribute definition points to.
		/// </summary>
		public Guid Catalog { get; set; }

		#endregion
	}
}