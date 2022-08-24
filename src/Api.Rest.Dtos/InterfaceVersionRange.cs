#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Dtos
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Text.Json.Serialization;

	#endregion

	public class InterfaceVersionRange
	{
		#region properties

		[Newtonsoft.Json.JsonProperty( "supportedVersions" )]
		[JsonPropertyName( "supportedVersions" )]
		public IEnumerable<Version> SupportedVersions { get; set; }

		#endregion
	}
}