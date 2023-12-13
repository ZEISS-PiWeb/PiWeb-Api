#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Core
{
	/// <summary>
	/// Enumeration of possible inspection plan entities.
	/// </summary>
	public enum InspectionPlanEntity : byte
	{
		/// <summary>
		/// The entity is a part.
		/// </summary>
		Part = 1,

		/// <summary>
		/// The entity is a characteristic.
		/// </summary>
		Characteristic = 2
	}
}