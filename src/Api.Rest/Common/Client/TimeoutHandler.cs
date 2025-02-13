#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2021                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Client
{
	#region usings

	using System;
	using System.Net.Http;
	using System.Threading;
	using System.Threading.Tasks;

	#endregion

	/// <summary>
	/// Responsible for correctly handling timeouts for HttpClient requests.
	/// </summary>
	/// <remarks>
	/// Unfortunately there is no easy way to recognize hitting a timeout rather than a normal cancellation for HttpClient requests.
	/// In .NET 5.0 Microsoft finally added a way to recognize the timeout by checking the inner exception for being a TimeoutException.
	/// So this http handler mimes that fix by using its own cancellation token for setting up the timeout, so that way the timeout can
	/// easily be recognized and it works also for frameworks before .NET 5.0.
	///
	/// More can be read here:
	/// https://github.com/dotnet/runtime/issues/21965
	/// https://github.com/dotnet/runtime/pull/2281
	/// </remarks>
	public sealed class TimeoutHandler : DelegatingHandler
	{
		#region properties

		/// <summary>
		/// Gets or sets the timespan to wait before the request times out.
		/// </summary>
		public TimeSpan Timeout { get; set; }

		#endregion

		#region methods

		/// <inheritdoc />
		protected override async Task<HttpResponseMessage> SendAsync( HttpRequestMessage request, CancellationToken cancellationToken )
		{
			using( var cts = GetCancellationTokenSource( cancellationToken ) )
			{
				var timeoutTickCount = cts is null
					? long.MaxValue
					: Environment.TickCount + Timeout.Ticks;

				try
				{
					return await base.SendAsync( request, cts?.Token ?? cancellationToken )
						.ConfigureAwait( false );
				}
				catch( OperationCanceledException ex ) when( !cancellationToken.IsCancellationRequested && Environment.TickCount >= timeoutTickCount )
				{
					throw new TimeoutException(
						$"The request was canceled due to the configured timeout of {Timeout.TotalSeconds} seconds elapsing.", ex );
				}
			}
		}

		private CancellationTokenSource GetCancellationTokenSource( CancellationToken cancellationToken )
		{
			if( Timeout == System.Threading.Timeout.InfiniteTimeSpan )
				// No need to create a CTS if there's no timeout
				return null;

			var cts = CancellationTokenSource.CreateLinkedTokenSource( cancellationToken );
			cts.CancelAfter( Timeout );
			return cts;
		}

		#endregion
	}
}