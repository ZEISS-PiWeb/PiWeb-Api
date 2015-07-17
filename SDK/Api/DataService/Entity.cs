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
	/// This enumeration specifies all possible entities that are accessible via DataService.
	/// </summary>
	public enum Entity 
	{
		/// <summary>Part</summary>
		Part,

		/// <summary>Characteristic</summary>
		Characteristic,

		/// <summary>Measurement value</summary>
		Value,

		/// <summary>Measurement</summary>
		Measurement,

		/// <summary>Catalog</summary>
		Catalog
	}
}