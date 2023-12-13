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
	using JetBrains.Annotations;

	#endregion

	public abstract class CommonRestClientBase : IRestClient
	{
		#region members

		protected readonly RestClientBase _RestClient;
		private bool _IsDisposed;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="CommonRestClientBase" /> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="restClient"/> is <see langword="null" />.</exception>
		protected CommonRestClientBase( [NotNull] RestClientBase restClient )
		{
			_RestClient = restClient ?? throw new ArgumentNullException( nameof( restClient ) );
			_RestClient.AuthenticationChanged += RestClientOnAuthenticationChanged;
		}

		#endregion

		#region properties

		public int MaxUriLength => _RestClient.MaxUriLength;

		#endregion

		#region methods

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

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		/// <inheritdoc />
		public virtual AuthenticationContainer AuthenticationContainer
		{
			get => _RestClient.AuthenticationContainer;
			set => _RestClient.AuthenticationContainer = value;
		}

		/// <inheritdoc />
		public Uri ServiceLocation => _RestClient.ServiceLocation;

		/// <inheritdoc />
		public TimeSpan Timeout
		{
			get => _RestClient.Timeout;
			set => _RestClient.Timeout = value;
		}

		/// <inheritdoc />
		public bool UseDefaultWebProxy
		{
			get => _RestClient.UseDefaultWebProxy;
			set => _RestClient.UseDefaultWebProxy = value;
		}

		public event EventHandler AuthenticationChanged;

		#endregion
	}
}