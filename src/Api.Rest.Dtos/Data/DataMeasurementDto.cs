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
	using Zeiss.PiWeb.Api.Rest.Dtos.Converter;
	using Zeiss.PiWeb.Api.Rest.Dtos.JsonConverters;

	#endregion

	/// <summary>
	/// This is the concrete class of a measurement that contains measurement values as well.
	/// </summary>
	public class DataMeasurementDto : SimpleMeasurementDto
	{
		#region members

		private IReadOnlyCollection<DataCharacteristicDto> _Characteristics = Array.Empty<DataCharacteristicDto>();

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the measurement values.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "characteristics" ), Newtonsoft.Json.JsonConverter( typeof( DataCharacteristicConverter ) )]
		[JsonPropertyName( "characteristics" ), JsonConverter( typeof( JsonDataCharacteristicArrayConverter ) )]
		public IReadOnlyCollection<DataCharacteristicDto> Characteristics
		{
			[NotNull] get => _Characteristics;
			set => _Characteristics = value ?? Array.Empty<DataCharacteristicDto>();
		}

		#endregion
	}
}