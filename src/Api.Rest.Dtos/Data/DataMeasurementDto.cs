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

	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// This is the concrete class of a measurement that contains measurement values as well.
	/// </summary>
	public class DataMeasurementDto : SimpleMeasurementDto
	{
		#region members

		[NotNull] private IReadOnlyDictionary<Guid, DataValueDto> _Characteristics = new Dictionary<Guid, DataValueDto>();

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the measurement values.
		/// </summary>
		[JsonPropertyName( "characteristics" )]
		public IReadOnlyDictionary<Guid, DataValueDto> Characteristics
		{
			[NotNull] get => _Characteristics;
			set => _Characteristics = value ?? new Dictionary<Guid, DataValueDto>();
		}

		#endregion
	}
}