#region copyright

/* * * * * * * * * * * * * * * * * * * * * * * * * */
/* Carl Zeiss Industrielle Messtechnik GmbH        */
/* Softwaresystem PiWeb                            */
/* (c) Carl Zeiss 2016                             */
/* * * * * * * * * * * * * * * * * * * * * * * * * */

#endregion

namespace Zeiss.PiWeb.Api.Rest.Common.Authentication;

/// <summary>
/// Responsible for handling the authentication of a rest client.
/// </summary>
public interface IAuthenticationHandler
{
	#region methods

	/// <summary>
	/// This method is called when the authentication handler is set on a rest client.
	/// </summary>
	/// <param name="context">Contains information and possible actions on the rest client.</param>
	public void InitializeRestClient( IInitializationContext context );

	/// <summary>
	/// This method is called before a rest request is send.
	/// </summary>
	/// <param name="context">Contains information and possible actions on current request and the rest client.</param>
	public void HandleRequest( IRequestContext context );

	/// <summary>
	/// This method is called after a rest response is received. To retry the request, set the RetryRequest property of the context to
	/// <c>true</c>.
	/// </summary>
	/// <param name="context">Contains information and possible actions on current request, response and the rest client.</param>
	public void HandleResponse( IResponseContext context );

	#endregion
}