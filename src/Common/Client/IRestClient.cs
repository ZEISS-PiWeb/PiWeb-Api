#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.IMT.PiWeb.Api.Common.Client
{
	#region usings

	using System;

	#endregion

	public interface IRestClient : IDisposable
	{
		#region properties

		AuthenticationContainer AuthenticationContainer { get; set; }

		Uri ServiceLocation { get; }

		TimeSpan Timeout { get; set; }

		bool UseDefaultWebProxy { get; set; }

		event EventHandler AuthenticationChanged;

		#endregion
	}
}