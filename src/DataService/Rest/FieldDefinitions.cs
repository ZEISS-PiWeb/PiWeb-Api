#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.DataService.Rest
{
	#region usings

	using System.Collections.Generic;
	using System.Linq;

	#endregion

	/// <summary>
	/// Central location where field definitions are defined.
	/// </summary>
	public static class FieldDefinitions
	{
		#region properties

		/// <summary>
		/// Gets the field definitions for measurements.
		/// </summary>
		public static FieldDefinition[] Measurement => new[]
		{
			new FieldDefinition( "LastModified", FieldType.DateTime ),
			new FieldDefinition( "Created", FieldType.DateTime )
		};

		/// <summary>
		/// Gets the field definitions for values.
		/// </summary>
		public static FieldDefinition[] Value => new FieldDefinition[] { };

		#endregion

		#region methods

		/// <summary>
		/// Gets the field definitions.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public static Dictionary<FieldDefinition, Entity> GetAvailableDefinitions( Entity entity )
		{
			switch( entity )
			{
				case Entity.Measurement:
					return Measurement.ToDictionary( f => f, f => entity );
				default:
					return new Dictionary<FieldDefinition, Entity>();
			}
		}

		#endregion
	}
}