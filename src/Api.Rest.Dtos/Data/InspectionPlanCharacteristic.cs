#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region using

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class represents an inspection plan characteristics with its attributes and the version history.
	/// </summary>
	public class InspectionPlanCharacteristic : InspectionPlanBase
	{
        #region constructors

	    /// <summary>
	    /// This constructor is required for the JSON deserializer to be able
	    /// to identify concrete classes to use when deserializing <see cref="History"/> property.
	    /// </summary>
	    [ JsonConstructor ]
	    public InspectionPlanCharacteristic( InspectionPlanCharacteristic[] history )
	    {
	        History = history;
	    }

		/// <summary>
		/// Default-Constructor.
		/// </summary>
	    public InspectionPlanCharacteristic() { }

		#endregion

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