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
	/// This enumeration specifies the order direction when searching for measurement .
	/// </summary>
	[JsonConverter( typeof( JsonStringEnumConverter ) )]
	public enum OrderDirectionDto
	{
		/// <summary>
		/// Ascending order.
		/// </summary>
		Asc,

		/// <summary>
		/// Descending order.
		/// </summary>
		Desc
	}
}