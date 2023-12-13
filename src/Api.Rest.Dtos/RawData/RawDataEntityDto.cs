#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Enumeration that specifies the entity to which a raw data object is attached to.
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
	[JsonConverter( typeof( JsonStringEnumConverter ) )]
	public enum RawDataEntityDto
	{
		/// <summary>
		/// The raw data object belongs to a part.
		/// </summary>
		Part,

		/// <summary>
		/// The raw data object belongs to a characteristic.
		/// </summary>
		Characteristic,

		/// <summary>
		/// The raw data object belongs to a measurement.
		/// </summary>
		Measurement,

		/// <summary>
		/// The raw data object belongs to a measurement value.
		/// </summary>
		Value,
	}
}