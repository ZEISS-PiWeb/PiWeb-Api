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
	/// This element describes the entity Part without any relations to other entites.
	/// Like all other entities, the Part entity has an unique identifier, which is used as a primary key,
	/// a type description, and zero or more attributes.
	/// </summary>
	public class SimplePart : InspectionPlanBase
	{
		#region properties

		/// <summary>
		/// Gets or sets the timestamp for characteristic changes. Whenever a characteristic below that part (but not below sub parts) is changed, created or deleted,
		/// this timestamp will be updated by the server backend.
		/// </summary>
		public DateTime CharChangeDate { get; set; }

		#endregion
	}
}
