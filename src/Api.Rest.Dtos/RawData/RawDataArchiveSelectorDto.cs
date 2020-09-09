#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.RawData
{
	#region usings

	using System;
	using JetBrains.Annotations;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Selects a number of entries of an archive saved as raw data object.
	/// </summary>
	public class RawDataArchiveSelectorDto
	{
		#region constructors

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveSelectorDto() { }

		/// <summary>
		/// Constructor
		/// </summary>
		public RawDataArchiveSelectorDto( int key, [NotNull] RawDataTargetEntityDto target, [NotNull] string[] entries )
		{
			Entries = entries ?? throw new ArgumentNullException( nameof( entries ) );
			Key = key;
			Target = target ?? throw new ArgumentNullException( nameof( target ) );
		}

		#endregion

		#region properties

		/// <summary>
		/// <see cref="RawDataTargetEntityDto"/> of raw data.
		/// </summary>
		[JsonProperty( "target" )]
		public RawDataTargetEntityDto Target { get; set; }

		/// <summary>
		/// Key to specify raw data of target entity.
		/// </summary>
		[JsonProperty( "key" )]
		public int Key { get; set; }

		/// <summary>
		/// List of requested files in specified archive.
		/// </summary>
		[JsonProperty( "entries" )]
		public string[] Entries { get; set; }

		#endregion
	}
}