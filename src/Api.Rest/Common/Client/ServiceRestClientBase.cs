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
	using System.Runtime.CompilerServices;
	using JetBrains.Annotations;
	using Zeiss.PiWeb.Api.Rest.Contracts;

	#endregion

	public abstract class ServiceRestClientBase : IServiceRestClient
	{
		#region members

		protected readonly IRestClient _RestClient;
		private readonly IRestClientConfiguration _Configuration;

		private bool _IsDisposed;

		#endregion

		#region constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceRestClientBase" /> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="restClient"/> is <see langword="null" />.</exception>
		protected ServiceRestClientBase( [NotNull] IRestClient restClient )
			: this( restClient, restClient as IRestClientConfiguration )
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ServiceRestClientBase" /> class.
		/// </summary>
		/// <exception cref="ArgumentNullException"><paramref name="restClient"/> is <see langword="null" />.</exception>
		protected ServiceRestClientBase( [NotNull] IRestClient restClient, IRestClientConfiguration configuration )
		{
			_RestClient = restClient ?? throw new ArgumentNullException( nameof( restClient ) );

			_Configuration = configuration;
			_Configuration?.AuthenticationChanged += RestClientOnAuthenticationChanged;
		}

		#endregion

		#region properties

		public int MaxUriLength => _RestClient.MaxUriLength;

		#endregion

		#region methods

		private IRestClientConfiguration GetConfigurationOrThrow( [CallerMemberName] string propertyName = "" )
		{
			return _Configuration ?? throw new NotSupportedException( $"{propertyName} is not supported for not configured RestClients. Please set the {propertyName} on the RestClient (HttpClient) instance." );
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
					( _RestClient as IDisposable )?.Dispose();
				}

				_IsDisposed = true;
			}
		}

		#endregion

		#region interface IDisposable

		/// <inheritdoc />
		public void Dispose()
		{
			Dispose( true );
			GC.SuppressFinalize( this );
		}

		#endregion

		#region interface IServiceRestClient

		/// <inheritdoc />
		public virtual AuthenticationContainer AuthenticationContainer
		{
			get => _Configuration?.AuthenticationContainer ?? new AuthenticationContainer( AuthenticationMode.NoneOrBasic );
			set => GetConfigurationOrThrow().AuthenticationContainer = value;
		}

		/// <inheritdoc />
		public Uri ServiceLocation => _RestClient.ServiceLocation;

		/// <inheritdoc />
		public TimeSpan Timeout
		{
			get => GetConfigurationOrThrow().Timeout;
			set => GetConfigurationOrThrow().Timeout = value;
		}

		/// <inheritdoc />
		public bool UseDefaultWebProxy
		{
			get => GetConfigurationOrThrow().UseDefaultWebProxy;
			set => GetConfigurationOrThrow().UseDefaultWebProxy = value;
		}

		/// <inheritdoc />
		public bool CheckCertificateRevocationList
		{
			get => GetConfigurationOrThrow().CheckCertificateRevocationList;
			set => GetConfigurationOrThrow().CheckCertificateRevocationList = value;
		}

		public event EventHandler AuthenticationChanged;

		#endregion
	}
}