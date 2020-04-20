#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos.Data
{
	#region using

	using Newtonsoft.Json;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// This is the concrete class of a measurement that contains measurement values as well.
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
		[JsonProperty( "characteristics" ), JsonConverter( typeof( Converter.DataCharacteristicConverter ) )]
		public DataCharacteristic[] Characteristics
		{
			[NotNull]
			get { return _Characteristics; }
			set { _Characteristics = value ?? EmptyCharacteristicList; }
		}

		#endregion
	}
}