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

	public abstract class CommonRestClientBase : IRestClient
	{
		#region members

		protected readonly RestClient _RestClient;

		#endregion

		#region constructors

		protected CommonRestClientBase( RestClient restClient )
		{
			_RestClient = restClient;

			_RestClient.AuthenticationChanged += RestClientOnAuthenticationChanged;
		}

		private void RestClientOnAuthenticationChanged( object sender, EventArgs e )
		{
			AuthenticationChanged?.Invoke( this, EventArgs.Empty );
		}

		#endregion

		#region interface IRestClient

		public void Dispose()
		{
			_RestClient.Dispose();
		}

		public AuthenticationContainer AuthenticationContainer
		{
			get { return _RestClient.AuthenticationContainer; }
			set { _RestClient.AuthenticationContainer = value; }
		}

		public Uri ServiceLocation => _RestClient.ServiceLocation;

	    public int MaxUriLength => _RestClient.MaxUriLength;

		public TimeSpan Timeout
		{
			get { return _RestClient.Timeout; }
			set { _RestClient.Timeout = value; }
		}

		public bool UseDefaultWebProxy
		{
			get { return _RestClient.UseDefaultWebProxy; }
			set { _RestClient.UseDefaultWebProxy = value; }
		}

		public event EventHandler AuthenticationChanged;

		#endregion
	}
}