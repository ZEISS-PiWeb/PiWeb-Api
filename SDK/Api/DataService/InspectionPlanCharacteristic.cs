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
	/// This element extends the entity Characteristic with an optional version history of old values.
	/// </summary>
	public class InspectionPlanCharacteristic : InspectionPlanBase
	{
		#region properties

		/// <summary>
		/// Gets or sets the version history for this inspection plan characteristic.
		/// </summary>
		public InspectionPlanBase[] History { get; set; }

		#endregion
	}
}