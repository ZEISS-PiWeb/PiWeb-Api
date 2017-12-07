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
	/// This class represents an inspection plan part with its attributes and the version history.
	/// </summary>
	public class InspectionPlanPart : SimplePart
	{
		#region properties

		/// <summary>
		/// Gets or sets the version history for this inspection plan part.
		/// </summary>
		public InspectionPlanBase[] History { get; set; }

		#endregion
	}
}
