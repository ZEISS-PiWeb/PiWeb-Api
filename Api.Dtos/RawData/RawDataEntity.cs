#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.RawData
{
	#region using

	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

	#endregion

	/// <summary>
	/// Enumeration that specifies the entity to which a raw data object is attached to. 
	/// </summary>
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum RawDataEntity
	{
		/// <summary>The raw data object belongs to a part.</summary>
		Part,

		/// <summary>The raw data object belongs to a characteristic.</summary>
		Characteristic,

		/// <summary>The raw data object belongs to a measurement.</summary>
		Measurement,

		/// <summary>The raw data object belongs to a measurement value.</summary>
		Value,
	}
}