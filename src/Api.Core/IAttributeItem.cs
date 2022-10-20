#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Core
{
	using System.Collections.Generic;

	/// <summary>
	/// This interface represents entities that contain a list of <see cref="Attribute"/>.
	/// </summary>
	public interface IAttributeItem
	{
		#region properties

		/// <summary>
		/// Returns all attributes for this item.
		/// </summary>
		IReadOnlyList<Attribute> Attributes { get; set; }

		#endregion
	}
}