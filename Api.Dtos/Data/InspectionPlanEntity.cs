#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	#region using

	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	#endregion

	/// <summary>
	/// Enumeration of possible inspection plan entities.
	/// </summary>
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum InspectionPlanEntity : byte
	{
		/// <summary>
		/// The entity is a part.
		/// </summary>
		Part = 1,

		/// <summary>
		/// The entity is a characteristic.
		/// </summary>
		Characteristic = 2,
	}
}
