#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2024                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.ReportManagement
{

	#region usings

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// Contains further links to endpoints of the report.
	/// </summary>
	public class ReportMetadataLinksDto
	{
		#region properties

		/// <summary>
		/// Gets the link to the ReportMetadata endpoint of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "self" )]
		[JsonPropertyName( "self" )]
		public LinkDto Self { get; set; }

		/// <summary>
		/// Gets the link to the thumbnail endpoint of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "thumbnail" )]
		[JsonPropertyName( "thumbnail" )]
		public LinkDto Thumbnail { get; set; }

		/// <summary>
		/// Gets the link to the content endpoint of the report.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "content" )]
		[JsonPropertyName( "content" )]
		public LinkDto Content { get; set; }

		#endregion
	}
}