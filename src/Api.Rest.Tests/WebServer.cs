#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2020                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Tests
{
	#region usings

	using System;
	using System.Collections.Generic;
	using System.Net;
	using System.Text;
	using System.Threading.Tasks;

	#endregion

	internal sealed class WebServer : IDisposable
	{
		#region members

		private readonly HttpListener _Listener = new HttpListener();
		private readonly IDictionary<string, Func<string>> _Responses = new Dictionary<string, Func<string>>();

		#endregion

		#region constructors

		private WebServer( int port )
		{
			var prefix = $"http://+:{port}/";
			_Listener.Prefixes.Add( prefix );
		}

		#endregion

		#region methods

		public static WebServer StartNew( int port )
		{
			var server = new WebServer( port );

			server.Start();

			return server;
		}

		private void Start()
		{
			_Listener.Start();

			Task.Run( async () =>
			{
				while( _Listener.IsListening )
				{
					var ctx = await _Listener.GetContextAsync();

					try
					{
						var rstr = CreateResponse( ctx.Request );
						var buf = Encoding.UTF8.GetBytes( rstr );
						ctx.Response.ContentLength64 = buf.Length;
						await ctx.Response.OutputStream.WriteAsync( buf, 0, buf.Length );
					}
					finally
					{
						ctx.Response.OutputStream.Close();
					}
				}
			} );
		}

		public void RegisterReponse( string uri, Func<string> response )
		{
			_Responses[ uri ] = response;
		}

		public void RegisterReponse( string uri, string response )
		{
			RegisterReponse( uri, () => response );
		}

		private string CreateResponse( HttpListenerRequest request )
		{
			if( _Responses.TryGetValue( request.RawUrl, out var response ) )
				return response();

			return string.Empty;
		}

		#endregion

		#region interface IDisposable

		public void Dispose()
		{
			_Listener.Stop();
			_Listener.Close();
		}

		#endregion
	}
}