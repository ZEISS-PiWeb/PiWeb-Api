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
	/// Defines an entity's attribute which is based on a <see cref="Catalog"/>.
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