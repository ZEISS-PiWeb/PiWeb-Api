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

	using System;
	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// This class represents the base class for parts inside the inspection plan structure.
	/// The concrete class for parts that also contains the part attributes is <see cref="InspectionPlanPartDto"/>.
	/// </summary>
	public class SimplePartDto : InspectionPlanDtoBase
	{
		#region properties

		/// <summary>
		/// Gets or sets the timestamp for characteristic changes.
		/// Whenever a characteristic below that part (but not below sub parts) is changed, created or deleted,
		/// this timestamp will be updated by the server.
		/// </summary>
		[JsonPropertyName( "charChangeDate" )]
		public DateTime CharChangeDate { get; set; }

		#endregion
	}
}