#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class represents an inspection plan characteristics with its attributes and the version history.
	/// </summary>
	public class InspectionPlanCharacteristicDto : InspectionPlanDtoBase
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="InspectionPlanCharacteristicDto"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is required for the JSON deserializer to be able
		/// to identify concrete classes to use when deserializing <see cref="History"/> property.
		/// </remarks>
		[JsonConstructor]
		public InspectionPlanCharacteristicDto( InspectionPlanCharacteristicDto[] history )
		{
			History = history;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="InspectionPlanCharacteristicDto"/> class.
		/// </summary>
		public InspectionPlanCharacteristicDto() { }

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the version history for this inspection plan characteristics. This property will be populated only
		/// when the inspection plan search is performed with the versioning flag set. When creating new parts,
		/// this information will be ignored by the server.
		/// </summary>
		[JsonProperty( "history" )]
		public InspectionPlanDtoBase[] History { get; set; }

		#endregion
	}
}