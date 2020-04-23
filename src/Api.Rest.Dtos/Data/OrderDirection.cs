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
	using Newtonsoft.Json.Converters;

	#endregion

	/// <summary>
	/// This enumeration specifies the order direction when searching for measurement .
	/// </summary>
	[JsonConverter( typeof( StringEnumConverter ) )]
	public enum OrderDirection
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