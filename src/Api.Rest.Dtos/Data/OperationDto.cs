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

	#endregion

	/// <summary>
	/// This enumeration specifies the available search operations for measurement searches.
	/// When searching with a list of values using <see cref="In"/> and <see cref="NotIn"/> the following rules apply:
	/// * List values are separated by a comma, i.e. "abc,def"
	/// * Whitespaces at the beginning and end of a values list are ignored, i.e. "  abc,def  " is the same as "abc,def"
	/// * When a value inside the values list should contain a comma, the entry has to be quoted, i.e. "'value,with,two commas', othervalue"
	/// * Dates have to be specified in ISO-8601 format
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
	[JsonConverter( typeof( JsonStringEnumConverter ) )]
	public enum OperationDto
	{
		/// <summary>
		/// Specifies an operation that checks if two values are equal.
		/// </summary>
		Equal,

		/// <summary>
		/// Specifies an operation that checks if two values are not equal.
		/// </summary>
		NotEqual,

		/// <summary>
		/// Specifies an operation that checks if one value is greater than another value.
		/// </summary>
		GreaterThan,

		/// <summary>
		/// Specifies an operation that checks if one value is less than another value.
		/// </summary>
		LessThan,

		/// <summary>
		/// Specifies an operation that checks if one value is greater than or equal to another value.
		/// </summary>
		GreaterThanOrEqual,

		/// <summary>
		/// Specifies an operation that checks if one value is less than or equal to another value.
		/// </summary>
		LessThanOrEqual,

		/// <summary>
		/// Specifies an operation that checks if one value is inside a value list.
		/// </summary>
		In,

		/// <summary>
		/// Specifies an operation that checks if one value is not inside a value list.
		/// </summary>
		NotIn,

		/// <summary>
		/// Specifies an operation that checks if two values are equal. Placeholder "%" (arbitrary string) and "_" (single character) are allowed.
		/// </summary>
		Like
	}
}