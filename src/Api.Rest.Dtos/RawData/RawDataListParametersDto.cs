#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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
	public class RawDataListParametersDto
	{
		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="RawDataListParametersDto"/> class.
		/// </summary>
		public RawDataListParametersDto()
		{
			ClientId = ClientIdHelper.ClientId;
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="RawDataListParametersDto"/> class.
		/// </summary>
		public RawDataListParametersDto( [NotNull] RawDataTargetEntityDto entity )
		{
			if( entity == null ) throw new ArgumentNullException( nameof( entity ) );

			ClientId = ClientIdHelper.ClientId;
			Targets = new[] { entity };
		}

		/// <summary>
		///  Initializes a new instance of the <see cref="RawDataListParametersDto"/> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="entities"/> is <see langword="null" />.</exception>
		public RawDataListParametersDto( [NotNull] IEnumerable<RawDataTargetEntityDto> entities )
		{
			if( entities == null ) throw new ArgumentNullException( nameof( entities ) );

			ClientId = ClientIdHelper.ClientId;
			Targets = new List<RawDataTargetEntityDto>( entities ).ToArray();
		}

		#endregion

		#region properties

		public RawDataTargetEntityDto[] Targets { get; set; }

		public string ClientId { get; set; }

		#endregion
	}
}