#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region usings

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class represents an inspection plan characteristics with its attributes and the version history.
	/// </summary>
	public class InspectionPlanCharacteristic : InspectionPlanBase
	{
		#region properties

		/// <summary>
		/// Gets or sets the version history for this inspection plan characteristics. This property will be populated only 
		/// when the inspection plan search is performed with the versioning flag set. When creating new parts,
		/// this information will be ignored by the server.
		/// </summary>
		[JsonProperty( "history" )]
		public InspectionPlanBase[] History { get; set; }

		#endregion
	}
}