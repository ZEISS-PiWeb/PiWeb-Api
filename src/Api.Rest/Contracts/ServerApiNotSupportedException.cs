#region Copyright
/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss IMT (IZfM Dresden)                   */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */
#endregion

namespace Zeiss.PiWeb.Api.Rest.Contracts
{
	public class ServerApiNotSupportedException : RestClientException
	{
		public ServerApiNotSupportedException() : base( "The server supports no known interface version." )
		{

		}
	}
}