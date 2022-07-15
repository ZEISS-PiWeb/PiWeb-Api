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

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Enumeration of possible entities.
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
	[JsonConverter( typeof(JsonStringEnumConverter) )]
	public enum EntityDto
	{
		/// <summary>
		/// The entity is a part.
		/// </summary>
		Part,

		/// <summary>
		/// The entity is a characteristic.
		/// </summary>
		Characteristic,

		/// <summary>
		/// The entity is a measurement value.
		/// </summary>
		Value,

		/// <summary>
		/// The entity is a measurement.
		/// </summary>
		Measurement,

		/// <summary>
		/// The entity is a catalog.
		/// </summary>
		Catalog
	}
}