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
	/// This element (in combination with its base element) is used to define the possible attributes
	/// that an entity (like Part, Characteristic etc.) may have. In addition to its base element, this
	/// element contains the type of an attribute and its size (or length).
	/// "length" is ignored for Integer, Float and DateTime attributes, as they have a fixed format.
	/// Integer is a signed 32-bit type, Float and DateTime are adopted from the XML Schema definition.
	/// XML Schema defines them as follows:
	/// Float corresponds to the IEEE single-precision 32-bit floating point type. The following
	/// examples are legal literals for Float: <code>-1E4, 1267.43233E12, 12.78e-2, 12</code>.
	/// Details can be found in the XML Schema definition, Part 2: Datatypes, Section "Primitive
	/// Datatypes".
	/// DateTime represents a specific instant of time. The lexical representation has to be
	/// in the format "CCYY-MM-DDThh:mm:ss". The letter "T" is the date/time separator. "ss" may
	/// contain fractional seconds (like "ss.ssss...") to increase the precision.
	/// Details can be found in the XML Schema definition, Part 2: Datatypes, Section "Primitive Datatypes".
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