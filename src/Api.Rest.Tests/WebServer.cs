#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
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

		private readonly HttpListener _Listener = new();
		private readonly IDictionary<string, Tuple<Func<string>, HttpStatusCode>> _Responses = new Dictionary<string, Tuple<Func<string>, HttpStatusCode>>();
		private readonly List<string> _ReceivedRequests;
		private string _DefaultResponse;
		private HttpStatusCode _DefaultResponseStatusCode;

		#endregion

		#region constructors

		private WebServer( int port )
		{
			var prefix = $"http://+:{port}/";
			_Listener.Prefixes.Add( prefix );
			_ReceivedRequests = new List<string>();
			_DefaultResponse = "";
			_DefaultResponseStatusCode = HttpStatusCode.OK;
		}

		#endregion

		#region properties

		public IEnumerable<string> ReceivedRequests => _ReceivedRequests;

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
					var context = await _Listener.GetContextAsync();

					if( context.Request.Url is not null )
						_ReceivedRequests.Add( context.Request.Url.ToString() );

					try
					{
						var response = CreateResponse( context.Request );
						var buf = Encoding.UTF8.GetBytes( response.Item1 );
						context.Response.ContentLength64 = buf.Length;
						context.Response.StatusCode = (int)response.Item2;
						await context.Response.OutputStream.WriteAsync( buf, 0, buf.Length );
					}
					finally
					{
						context.Response.OutputStream.Close();
					}
				}
			} );
		}

		public void RegisterResponse( string uri, Func<string> response, HttpStatusCode responseStatusCode = HttpStatusCode.OK )
		{
			_Responses[ uri ] = new Tuple<Func<string>, HttpStatusCode>( response, responseStatusCode );
		}

		public void RegisterResponse( string uri, string response, HttpStatusCode responseStatusCode = HttpStatusCode.OK )
		{
			RegisterResponse( uri, () => response );
		}

		public void RegisterDefaultResponse( string response, HttpStatusCode responseStatusCode = HttpStatusCode.OK )
		{
			_DefaultResponse = response;
			_DefaultResponseStatusCode = responseStatusCode;
		}

		private (string, HttpStatusCode) CreateResponse( HttpListenerRequest request )
		{
			if( request.RawUrl != null && _Responses.TryGetValue( request.RawUrl, out var response ) )
				return ( response.Item1(), response.Item2 );

			return ( _DefaultResponse, _DefaultResponseStatusCode );
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