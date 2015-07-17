#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	/// <summary>
	/// The entities which are allowed in InspectionPlanSearch  are defined in this element.
	/// </summary>
	public enum InspectionPlanEntity : byte
	{
		/// <summary>
		/// Specifies the entity type "Part"
		/// </summary>
		Part = 1,

		/// <summary>
		/// Specifies the entity type "Characteristic"
		/// </summary>
		Characteristic = 2,
	}
}
