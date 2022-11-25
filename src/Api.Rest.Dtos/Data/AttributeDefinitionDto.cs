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

	using System.Text.Json.Serialization;
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters;

	#endregion

	/// <summary>
	/// Defines an entity's attribute.
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( AttributeDefinitionConverter ) )]
	[JsonConverter( typeof( JsonAttributeDefinitionConverter ) )]
	public sealed class AttributeDefinitionDto : AbstractAttributeDefinitionDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeDefinitionDto"/> class.
		/// </summary>
		public AttributeDefinitionDto()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="AttributeDefinitionDto"/> class.
		/// </summary>
		/// <param name="key">The unique key for this attribute</param>
		/// <param name="description">The attribute description</param>
		/// <param name="type">The datatype of this attribute</param>
		/// <param name="length">The length of a string attribute (only valid if <code>type</code> is <code>AttributeType.AlphaNumeric</code></param>
		/// <param name="queryEfficient"><code>true</code> if this attribute is efficient for filtering operations</param>
		public AttributeDefinitionDto( ushort key, string description, AttributeTypeDto type, ushort? length, bool queryEfficient = false )
			: base( key, description, queryEfficient )
		{
			Type = type;
			Length = Type == AttributeTypeDto.AlphaNumeric ? length : null;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the data type of this attribute definition.
		/// </summary>
		public AttributeTypeDto Type { get; set; }

		/// <summary>
		/// Gets or sets the length of this attribute definition (if the definitions data type is <see cref="AttributeTypeDto.AlphaNumeric"/>.
		/// </summary>
		public ushort? Length { get; set; }

		#endregion
	}
}