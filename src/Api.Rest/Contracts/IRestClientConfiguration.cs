#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2026                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts;

#region usings

using System;
using CacheCow.Client;
using CacheCow.Common;
using Zeiss.PiWeb.Api.Rest.Common.Client;

#endregion

public interface IRestClientConfiguration
{
	#region properties

	AuthenticationContainer AuthenticationContainer { get; set; }

	TimeSpan Timeout { get; set; }

	bool UseDefaultWebProxy { get; set; }

	bool CheckCertificateRevocationList { get; set; }

	ICacheStore CacheStore { get; set; }

	IVaryHeaderStore VaryHeaderStore { get; set; }

	#endregion

	#region events

	event EventHandler AuthenticationChanged;

	#endregion
}
