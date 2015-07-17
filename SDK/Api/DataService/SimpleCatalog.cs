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
	/// This element describes the entity Catalog without any relations to other entites.
	/// The entity Catalog has an unique identifier, which is used as primary key, and a name,
	/// which is used by an user to distinguish between different Catalogs.
	/// </summary>
	public class SimpleCatalog
	{
		#region constructor

		/// <summary>
		/// Constructor
		/// </summary>
		public SimpleCatalog()
		{
			Name = "";
		}

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the uuid of this catalog.
		/// </summary>
		public Guid Uuid { get; set; }

		/// <summary>
		/// Gets or sets the name of the catalog.
		/// </summary>
		public string Name { get; set; }

		#endregion

		#region methods

		/// <summary>
		/// Overridden <see cref="System.Object.ToString"/> method.
		/// </summary>
		public override string ToString()
		{
			return Name;
		}

		#endregion
	}
}
