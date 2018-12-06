#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.RawDataService.Rest
{
	#region using

	using System;

	#endregion

	[Serializable]
	public class RawData : RawDataInformation
	{
		#region Constructor

		public RawData() { }

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