#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.HttpClient.Health
{
	#region usings

	using System.Text.Json.Serialization;
	using Newtonsoft.Json;

	#endregion

	/// <summary>
	/// Holds information about the health check status.
	/// </summary>
	public class HealthCheckStatus
	{
		#region properties

		/// <summary>
		/// The overall status of the health check response.
		/// </summary>
		[JsonProperty( "status" )]
		[JsonPropertyName( "status" )]
		public HealthCheckResultType Status { get; set; }

		#endregion
	}
}