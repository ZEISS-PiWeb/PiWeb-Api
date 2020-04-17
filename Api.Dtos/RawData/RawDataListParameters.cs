#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Dtos.RawData
{
	#region using

	using System;
	using System.Collections.Generic;

	#endregion

	/// <remarks/>
	[Serializable]
	public class RawDataListParameters
	{
		#region Membervariablen

		#endregion

		#region Constructor

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RawDataListParameters()
		{
			ClientId = ClientIdHelper.ClientId;
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RawDataListParameters( RawDataTargetEntity entity )
		{
			ClientId = ClientIdHelper.ClientId;
			Targets = new [] { entity };
		}

		/// <summary>
		/// Konstruktor
		/// </summary>
		public RawDataListParameters( IEnumerable<RawDataTargetEntity> entities )
		{
			ClientId = ClientIdHelper.ClientId;
			Targets = new List<RawDataTargetEntity>( entities ).ToArray();
		}

		#endregion

		#region Properties

		public RawDataTargetEntity[] Targets { get; set; }

		public string ClientId { get; set; }

		#endregion
	}
}