#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	/// <summary>
	/// This class represents a measurement value together with information about the characteristic this value belongs to.
	/// </summary>
	public class DataCharacteristicDto : InspectionPlanDtoBase
	{
		#region properties

		/// <summary>
		/// Gets or sets the measurement value. Please note that a measurement value can include additional
		/// information when measurement value attributes are defined.
		/// </summary>
		public DataValueDto Value { get; set; }

		#endregion
	}
}