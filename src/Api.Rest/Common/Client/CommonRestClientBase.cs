#region Copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;

	#endregion

	public abstract class CommonRestClientBase : IRestClient
	{
		#region members

		protected readonly RestClientBase _RestClient;
		private bool _IsDisposed;

		#endregion

		#region constructors

		protected CommonRestClientBase( RestClientBase restClient )
		{
			_RestClient = restClient;

			_RestClient.AuthenticationChanged += RestClientOnAuthenticationChanged;
		}

		private void RestClientOnAuthenticationChanged( object sender, EventArgs e )
		{
			AuthenticationChanged?.Invoke( this, EventArgs.Empty );
		}

		/// <summary>
		/// Releases unmanaged and - optionally - managed resources.
		/// </summary>
		/// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
		protected virtual void Dispose( bool disposing )
		{
			if( !_IsDisposed )
			{
				if( disposing )
				{
					_RestClient.Dispose();
				}

				_IsDisposed = true;
			}
		}

		#endregion

		#region interface IRestClient

		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		public virtual AuthenticationContainer AuthenticationContainer
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