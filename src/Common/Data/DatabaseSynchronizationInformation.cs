#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Data
{
	#region usings

	using System.Collections.Generic;
	using Zeiss.IMT.PiWeb.Api.DataService.Rest;

	#endregion

	public class DatabaseSynchronizationInformation
	{
		#region properties

		public DatabaseSynchronizationResult Result { get; set; }
		public string SourceName { get; set; }
		public Error Error { get; set; }
		public long ElapsedMilliseconds { get; set; }
		public PathInformation DestinationPath { get; set; }
		public int MeasurementRawDataCount { get; set; }
		public int MeasurementValueRawDataCount { get; set; }
		public int InspectionPlanRawDataCount { get; set; }
		public int InsertMeasurementValueCount { get; set; }
		public int UpdateMeasurementValueCount { get; set; }
		public int InsertMeasurementCount { get; set; }
		public int UpdateMeasurementCount { get; set; }
		public int InsertCharacteristicCount { get; set; }
		public int UpdateCharacteristicCount { get; set; }
		public int InsertPartCount { get; set; }
		public int UpdatePartCount { get; set; }
		public List<string> Warnings { get; } = new List<string>();

		#endregion
	}
}