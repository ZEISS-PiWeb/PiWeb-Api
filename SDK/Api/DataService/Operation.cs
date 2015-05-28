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

	#endregion

	/// <summary>
	/// This element contains the possible operations which can be used in search criteria expressions.
	/// The different values of <code>In</code> and <code>NotIn</code> have to be separated by a
	/// comma. If a value itself contains a comma, the value has to be quoted in single quotes. If a value
	/// is quoted and it should contain a single quote, the quote has to be escaped by a second quoute.
	/// Valid examples for <code>In</code>/<code>NotIn</code> are: <code>1,2</code>, <code>1,'2,0'</code>
	/// and <code>1,'2,0''5'</code>.
	/// </summary>
	public enum Operation
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