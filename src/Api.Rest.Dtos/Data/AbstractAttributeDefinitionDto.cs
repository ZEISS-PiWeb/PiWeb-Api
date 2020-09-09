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

	using Newtonsoft.Json;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;

	#endregion

	/// <summary>
	/// Abstract base class of <see cref="CatalogAttributeDefinitionDto"/> ans <see cref="AttributeDefinitionDto"/>.
	/// It holds attribute's base properties key and description.
	/// </summary>
	[JsonConverter( typeof( AttributeDefinitionConverter ) )]
	public abstract class AbstractAttributeDefinitionDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractAttributeDefinitionDto"/> class.
		/// </summary>
		protected AbstractAttributeDefinitionDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="AbstractAttributeDefinitionDto"/> class by using the specified key and value.
		/// </summary>
		/// <param name="key">The unique key for this attribute</param>
		/// <param name="description">The attribute description</param>
		/// <param name="queryEfficient"><code>true</code> if this attribute is efficient for filtering operations</param>
		protected AbstractAttributeDefinitionDto( ushort key, string description, bool queryEfficient )
		{
			Key = key;
			QueryEfficient = queryEfficient;
			Description = description;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the key for this attribute definition.
		/// The key is the attribute's unique identifier.
		/// </summary>
		public ushort Key { get; set; }

		/// <summary>
		/// Gets or sets the name of this attribute definition.
		/// The attribute's name which is displayed in UI.
		/// </summary>
		public string Description { get; set; }

		/// <summary>
		/// Determines whether this attribute is efficient for filtering operations.
		/// </summary>
		/// <remarks>
		/// This flag is currently unused. This may be used in future web service versions.
		/// </remarks>
		public bool QueryEfficient { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		public override string ToString()
		{
			return Description;
		}

		#endregion
	}
}