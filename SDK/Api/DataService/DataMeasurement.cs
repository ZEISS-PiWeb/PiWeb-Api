#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace DataService
{
	/// <summary>
	/// Each measurement includes characteristic entities, which include the measured data.
	/// </summary>
	public class DataMeasurement : SimpleMeasurement
	{
		#region members

		private static readonly DataCharacteristic[] EmptyCharacteristicList = new DataCharacteristic[0];
		private DataCharacteristic[] _Characteristics = EmptyCharacteristicList;

		#endregion

		#region properties

		/// <summary>
		/// Gets or sets the measurement values.
		/// </summary>
		public DataCharacteristic[] Characteristics
		{
			get { return _Characteristics; }
			set { _Characteristics = value ?? EmptyCharacteristicList; }
		}

		#endregion
	}
}