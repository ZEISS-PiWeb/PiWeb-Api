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
	using Microsoft.Extensions.Diagnostics.HealthChecks;
	using Newtonsoft.Json;
	using Newtonsoft.Json.Converters;

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
		[Newtonsoft.Json.JsonConverter( typeof( StringEnumConverter ) )]
		[System.Text.Json.Serialization.JsonConverter( typeof( JsonStringEnumConverter ) )]
		public HealthStatus Status { get; set; }

		#endregion
	}
}