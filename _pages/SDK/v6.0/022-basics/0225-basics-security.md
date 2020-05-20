<h2 id="{{page.sections['basics']['secs']['security'].anchor}}">{{page.sections['basics']['secs']['security'].title}}</h2>

Examples in this section:
+ [Authenticate with basic authentication](#-example--authenticate-with-basic-authentication)
+ [Authenticate using ActiveDirectory (Windows)](#-example--authenticate-using-activedirectory-windows)
<hr>

Access to PiWeb server service might require authentication. Our .NET client supports all authentication modes:

* Basic authentication based on username and password,
* Windows authentication based on Active Directory integration,
* Certificate-based authentication,
* Hardware certificate based authentication,
* OAuth (PiWeb Cloud only)

Authentication mode and credentials (if necessary) can be set via the client's property `AuthenticationContainer`.

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

Trying to fetch data without permission will still work if credentials are correct, but will return an empty result. Actions however, e.g. creating a part will result in an exception (mostly HTTP 401 Unauthorized), so make sure that you have the needed permissions for your requests. <br>

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
{% endhighlight %}
This tells the ServiceClient to use Windows Authentication (ActiveDirectory). From now on the client will use your Windows user account for authentication. You do not have to specify credentials as the client will check and fetch your information from the organization directory. <br>
Remember that your authentication mode has to match the mode set in the server settings.
