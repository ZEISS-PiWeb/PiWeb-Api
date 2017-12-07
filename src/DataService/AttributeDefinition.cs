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

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Defines an entity's attribute.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.AttributeDefinitionConverter ) )]
	public class AttributeDefinition : AbstractAttributeDefinition
	{
		#region constructor

		/// <summary>
		/// Constructor.
		/// </summary>
		public AttributeDefinition()
		{ }

		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="key">The unique key for this attribute</param>
		/// <param name="description">The attribute description</param>
		/// <param name="type">The datatype of this attribute</param>
		/// <param name="length">The length of a string attribute (only valid if <code>type</code> is <code>AttributeType.AlphaNumeric</code></param>
		/// <param name="queryEfficient"><code>true</code> if this attribute is efficient for filtering operations</param>
		public AttributeDefinition( ushort key, string description, AttributeType type, ushort length, bool queryEfficient = false )
			: base( key, description, queryEfficient )
		{
			Type = type;
			Length = length;
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the data type of this attribute definition.
		/// </summary>
		[JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
		public AttributeType Type { get; set; }

		/// <summary>
		/// Gets or sets the length of this attribute definition (if the definitions data type is <see cref="AttributeType.AlphaNumeric"/>.
		/// </summary>
		public ushort Length { get; set; }

		#endregion
	}
}