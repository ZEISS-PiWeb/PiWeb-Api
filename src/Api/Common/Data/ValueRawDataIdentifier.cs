#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Common.Data
{
	#region usings

	using System;

	#endregion

	/// <summary>
	/// References an identifier for a measured values raw data. Consists of the measurement uuid and the characteristic uuid. 
	/// </summary>
	public class ValueRawDataIdentifier
	{
		#region constructors

		public ValueRawDataIdentifier( Guid measurementUuid, Guid characteristicUuid )
		{
			MeasurementUuid = measurementUuid;
			CharacteristicUuid = characteristicUuid;
		}

		#endregion

		#region properties

		public Guid MeasurementUuid { get; }
		public Guid CharacteristicUuid { get; }

		#endregion
	}
}
