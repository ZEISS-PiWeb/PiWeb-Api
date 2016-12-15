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
	/// Enumeration of possible entities.
	/// </summary>
	public enum Entity 
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