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
	/// Contains the description of a link.
	/// </summary>
	public class LinkDto
	{
		#region properties

		/// <summary>
		/// Gets the Url of the link.
		/// </summary>
		[Newtonsoft.Json.JsonProperty( "href" )]
		[JsonPropertyName( "href" )]
		public string Href { get; set; }

		#endregion
	}
}