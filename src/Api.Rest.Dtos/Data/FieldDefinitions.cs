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

	using System;
	using System.Collections.Generic;

	#endregion

	/// <summary>
	/// Central location where field definitions are defined.
	/// </summary>
	public static class FieldDefinitions
	{
		#region members

		private static readonly IReadOnlyDictionary<FieldDefinitionDto, EntityDto> MeasurementFieldDefinitions = new Dictionary<FieldDefinitionDto, EntityDto>
		{
			{ new FieldDefinitionDto( "LastModified", FieldTypeDto.DateTime ), EntityDto.Measurement },
			{ new FieldDefinitionDto( "Created", FieldTypeDto.DateTime ), EntityDto.Measurement }
		};

		private static readonly IReadOnlyDictionary<FieldDefinitionDto, EntityDto> EmptyFieldDefinitions = new Dictionary<FieldDefinitionDto, EntityDto>();

		#endregion

		#region properties

		/// <summary>
		/// Gets the field definitions for measurements.
		/// </summary>
		public static IReadOnlyCollection<FieldDefinitionDto> Measurement => new[]
		{
			new FieldDefinitionDto( "LastModified", FieldTypeDto.DateTime ),
			new FieldDefinitionDto( "Created", FieldTypeDto.DateTime )
		};

		/// <summary>
		/// Gets the field definitions for values.
		/// </summary>
		public static IReadOnlyCollection<FieldDefinitionDto> Value => Array.Empty<FieldDefinitionDto>();

		#endregion

		#region methods

		/// <summary>
		/// Gets the field definitions.
		/// </summary>
		/// <param name="entity">The entity.</param>
		public static IReadOnlyDictionary<FieldDefinitionDto, EntityDto> GetAvailableDefinitions( EntityDto entity )
		{
			return entity switch
			{
				EntityDto.Measurement => MeasurementFieldDefinitions,
				_ => EmptyFieldDefinitions
			};
		}

		#endregion
	}
}