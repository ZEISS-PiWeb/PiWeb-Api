#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region usings

	using System;
	using System.Linq;
	using Zeiss.IMT.PiWeb.Api.Common.Data;

	#endregion

	/// <summary>
	/// 
	/// </summary>
	public class RawDataValueQuery
	{
		#region constructors

		public RawDataValueQuery()
		{
		}

		public RawDataValueQuery( Guid[] measurementUuids, Guid[] characteristicUuids )
		{
			MeasurementUuids = measurementUuids;
			CharacteristicUuids = characteristicUuids;
		}

		#endregion

		#region properties

		public Guid[] CharacteristicUuids { get; set; } = { };

		public Guid[] MeasurementUuids { get; set; } = { };

		#endregion

		#region methods

		public RawDataTargetEntity[] GetTargetEntities()
		{
			return MeasurementUuids.SelectMany(m => CharacteristicUuids.Select(c => new RawDataTargetEntity(RawDataEntity.Value, StringUuidTools.CreateStringUuidPair(m, c)))).ToArray();
				
		}

		#endregion
	}
}