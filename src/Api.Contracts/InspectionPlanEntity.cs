#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Contracts
{
	/// <summary>
	/// Enumeration of possible inspection plan entities.
	/// </summary>
	public enum InspectionPlanEntityDto : byte
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