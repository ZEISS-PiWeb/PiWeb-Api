#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;

	#endregion

	public interface IRestClient : IDisposable
	{
		#region events

		event EventHandler AuthenticationChanged;

		#endregion

		#region properties

		AuthenticationContainer AuthenticationContainer { get; set; }

		Uri ServiceLocation { get; }

		TimeSpan Timeout { get; set; }

		bool UseDefaultWebProxy { get; set; }

		bool CheckCertificateRevocationList { get; set; }

		#endregion
	}
}