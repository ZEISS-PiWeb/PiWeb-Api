#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2017                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System;
	using System.Text.Json.Serialization;
	using JetBrains.Annotations;

	#endregion

	/// <summary>
	/// Selects a specific raw data entry on a target.
	/// </summary>
	[Serializable]
	public class RawDataSelectorDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataSelectorDto()
		{ }

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataSelectorDto( int key, [NotNull] RawDataTargetEntityDto target )
		{
			Key = key;
			Target = target ?? throw new ArgumentNullException( nameof( target ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// <see cref="RawDataTargetEntityDto"/> of raw data.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "target" )]
		[JsonPropertyName( "target" )]
		public RawDataTargetEntityDto Target { get; set; }

		/// <summary>
		/// Key to specify raw data of target entity.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "key" )]
		[JsonPropertyName( "key" )]
		public int Key { get; set; }

		#endregion
	}
}