<h2 id="{{page.sections['basics']['secs']['security'].anchor}}">{{page.sections['basics']['secs']['security'].title}}</h2>

Examples in this section:
+ [Authenticate with basic authentication](#-example--authenticate-with-basic-authentication)
+ [Authenticate using ActiveDirectory (Windows)](#-example--authenticate-using-activedirectory-windows)
+ [Authenticate using a certificate](#-example--authenticate-using-a-certificate)
+ [Authenticate using OpenID connect](#-example--authenticate-using-openid-connect)
<hr>

Access to PiWeb server service might require authentication. Our .NET client supports all authentication modes:

* Basic authentication based on username and password,
* Windows authentication based on Active Directory integration,
* Certificate-based authentication,
* Hardware certificate based authentication,
* OAuth (PiWeb Cloud only)

Authentication mode and credentials (if necessary) can be set via the client's property `AuthenticationContainer`. Remember that your authentication mode has to match the mode set in the server settings.

{{ site.headers['example'] }} Authenticate with basic authentication

{% highlight csharp %}
//Create an AuthenticationContainer
var authContainer = new AuthenticationContainer
(
  AuthenticationMode.NoneOrBasic,
  new NetworkCredential( "API User", "pa55w0rd" )
);

//Set it as the clients AuthenticationContainer
DataServiceClient.AuthenticationContainer = authContainer;
//The client will now use your credentials for requests
{% endhighlight %}
The `AuthenticationContainer` provides information about authentication mode and credentials to use for requests. If authentication is activated accessing the services is only possible with valid credentials. Please note that PiWeb Server has permission management. Users can have different permissions for different functionalities like reading or creating entities.

Trying to fetch data without permission will still work if credentials are correct, but will return an empty result. Actions however, e.g. creating a part will result in an exception (*HTTP 401 Unauthorized* if authentication failed or *HTTP 403 Forbidden* if permissions are missing), so make sure that you have the needed permissions for your requests. <br>

If you don't know your account, credentials or permissions you should contact your PiWeb Server administrator.

{{ site.headers['example'] }} Authenticate using ActiveDirectory (Windows)

{% highlight csharp %}
//Create an AuthenticationContainer with mode Windows (ActiveDirectory)
var authContainer = new AuthenticationContainer
(
  AuthenticationMode.Windows
);

//Set it as the clients AuthenticationContainer
DataServiceClient.AuthenticationContainer = authContainer;
//The client will now negotiate using Windows authentication for requests
{% endhighlight %}
This tells the ServiceClient to use Windows Authentication (ActiveDirectory). From now on the client will use your Windows user account for authentication. You do not have to specify credentials as the client will check and fetch your information from the organization directory. <br>

{{ site.headers['example'] }} Authenticate using a certificate

{% highlight csharp %}
//Get the user certificate (e.g. from the Windows Certificate Store using the thumbprint)
var certificate = CertificateHelper.FindCertificateByThumbprint( "ddfb16cd4931c973a2037d7fc83a4d7d775d05e4" );

//Create an AuthenticationContainer with mode Windows (ActiveDirectory)
var authContainer = new AuthenticationContainer( certificate );

//Set it as the clients AuthenticationContainer
DataServiceClient.AuthenticationContainer = authContainer;
//The client will now send the certificate alongside the requests
{% endhighlight %} 

This tells the ServiceClient to use certificate authentication and the provided certificate of the user.
<br>

{{ site.headers['example'] }} Authenticate using OpenID connect

{% highlight csharp %}
// use OAuthHelper to get the OIDC access token for the service
var oAuthTokenCredential = await OAuthHelper.GetAuthenticationInformationForDatabaseUrlAsync( serverUri.AbsoluteUri, requestCallbackAsync: OAuthRequestCallbackAsync );

//Configure the clients AuthenticationContainer to use the acquired access token
DataServiceClient.AuthenticationContainer = new AuthenticationContainer( oAuthTokenCredential.AccessToken );
{% endhighlight %}

This tells the ServiceClient to use OpenID connect authentication sending the provided access token in the authentication header.

In order for this to work you need to provide a callback method. This callback method gets an `OAuthRequest` as argument.
The callback should open a browser at the start URL and monitor the browser until the callback URL is displayed in the
address bar. The final callback URL with all the parameters or form inputs should be returned and are used by the
`OAuthHelper` to get the access token as well as related information which are returned as `OAuthTokenCredential`.

An example for the required browser interaction using [WebView2](https://www.nuget.org/packages/Microsoft.Web.WebView2/)
is shown in a [C# sample application](https://github.com/ZEISS-PiWeb/PiWeb-Training).
