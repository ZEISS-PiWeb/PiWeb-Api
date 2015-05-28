#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	#region using

	using System;

	#endregion

	/// <summary>
	/// This element specifies the direction of an order operation.
	/// </summary>
	public enum OrderDirection 
	{
		/// <summary>
		/// Ascending order.
		/// </summary>
		Asc,

		/// <summary>
		/// Descending order.
		/// </summary>
		Desc,
	}
}
