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
	using System.Collections.Generic;
	using JetBrains.Annotations;

	#endregion

	[Serializable]
	public class RawDataListParameters
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataListParameters"/> class.
		/// </summary>
		public RawDataListParameters()
		{
			ClientId = ClientIdHelper.ClientId;
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="RawDataListParameters"/> class.
		/// </summary>
		public RawDataListParameters( [NotNull] RawDataTargetEntity entity )
		{
			if( entity == null ) throw new ArgumentNullException( nameof( entity ) );

			ClientId = ClientIdHelper.ClientId;
			Targets = new[] { entity };
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="RawDataListParameters"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="entities"/> is <see langword="null" />.</exception>
		public RawDataListParameters( [NotNull] IEnumerable<RawDataTargetEntity> entities )
		{
			if( entities == null ) throw new ArgumentNullException( nameof( entities ) );

			ClientId = ClientIdHelper.ClientId;
			Targets = new List<RawDataTargetEntity>( entities ).ToArray();
		}

		#endregion

		#region properties

		public RawDataTargetEntity[] Targets { get; set; }

		public string ClientId { get; set; }

		#endregion
	}
}