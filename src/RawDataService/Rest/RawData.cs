namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region using

	using System;

	#endregion

	/// <remarks/>
	[Serializable]
	public class RawData : RawDataInformation
	{
		#region Constructor

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RawData() { }

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RawData( RawDataInformation info, byte[] data )
			: base( info )
		{
			Data = data;
		}

		#endregion

		#region Properties

		public byte[] Data { get; set; }

		#endregion
	}
}