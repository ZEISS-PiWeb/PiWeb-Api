#region copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZM Dresden)                    */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2015                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Dtos
{
	#region usings

	using System;
	using System.Collections.Generic;
	using Newtonsoft.Json;

	# endregion

    public class InterfaceVersionRange
    {
		[ JsonProperty( "supportedVersions" ) ]
		public IEnumerable<Version> SupportedVersions { get; set; }
    }
}