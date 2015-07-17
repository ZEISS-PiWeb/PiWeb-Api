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
	/// This element describes the entity Characteristic in the context of measured data.
	/// It extends its base element with a relation to the measured data.
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
