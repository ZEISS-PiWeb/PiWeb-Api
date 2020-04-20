#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	/// <summary>
	/// Enumeration that determines which attributes for a specific will be returned.
	/// </summary>
	public enum AllAttributeSelection
	{
		/// <summary>
		/// If this enumeration value is specified in a <see cref="AttributeSelector"/>, all attributes 
		/// will be returned.
		/// </summary>
		True,

		/// <summary>
		/// If this enumeration value is specified in a <see cref="AttributeSelector"/>, only a subset of attributes 
		/// which are specified via <see cref="AttributeSelector.Attributes"/> will be returned.
		/// </summary>
		False,

		/// <summary>
		/// If this enumeration value is specified in a <see cref="AttributeSelector"/>, only attributes which are
		/// marked as <see cref="AbstractAttributeDefinition.QueryEfficient"/> will be returned.
		/// </summary>
		QueryEfficient,
	}
}