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

	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// This class represents a measurement value together with information about the characteristic this value belongs to.
	/// </summary>
	[JsonConverter( typeof( Common.Data.Converter.DataCharacteristicConverter ) )]
	public class DataCharacteristic : InspectionPlanBase
	{
		#region properties

		/// <summary>
		/// Gets or sets the measurement value. Please note that a measurement value can include additional 
		/// information when measurement value attributes are defined.
		/// </summary>
		public DataValue Value { get; set; }

		#endregion
	}
}
