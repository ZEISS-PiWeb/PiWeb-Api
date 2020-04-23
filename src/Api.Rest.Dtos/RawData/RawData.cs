#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System;
	using JetBrains.Annotations;

	#endregion

	[Serializable]
	public class RawData : RawDataInformation
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawData"/> class.
		/// </summary>
		public RawData() { }

		/// <summary>
		/// Initializes a new instance of the <see cref="RawData"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="info"/> is <see langword="null" />.</exception>
		public RawData( [NotNull] RawDataInformation info, byte[] data )
			: base( info )
		{
			Data = data;
		}

		#endregion

		#region properties

		public byte[] Data { get; set; }

		#endregion
	}
}