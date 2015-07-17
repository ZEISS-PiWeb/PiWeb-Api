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

	using System;
	
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// The CatalogAttributeDefinition element (in combination with its base element) is used to
	/// define the possible attributes that an entity (like Part, Characteristic etc.) may have.
	/// In contrast to the element "AttributeDefinition", this element is used for attributes which
	/// reference a Catalog. A Catalog consists of an uuid, a name and a map, which maps from
	/// integer values to arbitrary attributes.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.AttributeDefinitionConverter ) )]
	public class CatalogAttributeDefinition : AbstractAttributeDefinition
	{
		#region properties

		/// <summary>
		/// Gets or sets the uuid of the catalog this attribute definition points to.
		/// </summary>
		public Guid Catalog { get; set; }

		#endregion
	}
}