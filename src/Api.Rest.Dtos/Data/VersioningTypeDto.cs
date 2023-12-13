#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos.Data
{
	#region usings

	using System.Text.Json.Serialization;

	#endregion

	/// <summary>
	/// This enumeration specifies how the server is performing inspection plan versioning.
	/// </summary>
	[Newtonsoft.Json.JsonConverter( typeof( Newtonsoft.Json.Converters.StringEnumConverter ) )]
	[JsonConverter( typeof( JsonStringEnumConverter ) )]
	public enum VersioningTypeDto
	{
		/// <summary>Versioning is disabled by the server. The client cannot control versioning.</summary>
		Off,

		/// <summary>Versioning is enabled by the server. The client cannot control versioning.</summary>
		On,

		/// <summary>
		/// Versioning can be controlled by the client application for each inspection plan update.
		/// </summary>
		Client
	}
}