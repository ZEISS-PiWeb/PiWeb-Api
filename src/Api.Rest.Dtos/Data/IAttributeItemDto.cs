#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	using System.Collections.Generic;

	/// <summary>
	/// This interface represents entities that contain a list of <see cref="AttributeDto"/>.
	/// </summary>
	public interface IAttributeItemDto
	{
		#region properties

		/// <summary>
		/// Returns all attributes for this item.
		/// </summary>
		IReadOnlyList<AttributeDto> Attributes { get; set; }

		#endregion
	}
}