#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
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
		public static FieldDefinitionDto[] Measurement => new[]
		{
			new FieldDefinitionDto( "LastModified", FieldTypeDto.DateTime ),
			new FieldDefinitionDto( "Created", FieldTypeDto.DateTime )
		};

		/// <summary>
		/// Gets the field definitions for values.
		/// </summary>
		public static FieldDefinitionDto[] Value => new FieldDefinitionDto[] { };

		#endregion

		#region methods

		/// <summary>
		/// Gets the field definitions.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public static Dictionary<FieldDefinitionDto, EntityDto> GetAvailableDefinitions( EntityDto entity )
		{
			switch( entity )
			{
				case EntityDto.Measurement:
					return Measurement.ToDictionary( f => f, f => entity );
				default:
					return new Dictionary<FieldDefinitionDto, EntityDto>();
			}
		}

		#endregion
	}
}