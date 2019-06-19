<h2 id="{{page.sections['basics']['secs']['security'].anchor}}">{{page.sections['basics']['secs']['security'].title}}</h2>

Examples in this section:
+ [Authenticate yourself with basic authentication](#-example--authenticate-yourself-with-basic-authentication)
<hr>

Access to PiWeb server service might require authentication. Our .NET client supports all authentication modes:

* Basic authentication based on username and password,
* Windows authentication based on Active Directory integration,
* Certificate-based authentication,
* Hardware certificate based authentication,
* OAuth (PiWeb Cloud only)

Authentication mode and credentials (if necessary) can be set via the client's property `AuthenticationContainer`.

{{ site.headers['example'] }} Authenticate yourself with basic authentication

{% highlight csharp %}
//Create an AuthenticationContainer
var authContainer = new AuthenticationContainer
(
  AuthenticationMode.NoneOrBasic,
  new NetworkCredential( "API User", "pa55w0rd" )
);

//Set it as the clients AuthenticationContainer
RawDataServiceClient.AuthenticationContainer = authContainer;
//The client will now use your credentials for requests
{% endhighlight %}
The `AuthenticationContainer` provides information about authentication mode and credentials to use for requests. Accessing the services is only possible with valid credentials if authentication is activated. Please note that PiWeb Server has permission management. Users can have different permissions for different functionalities, like reading or creating entities.

Trying to fetch data without permission will still work if credentials are correct, but will return an empty result. Actions however, e.g. creating a part will result in an exception (mostly HTTP 401 Unauthorized), so make sure that you have the needed permissions for your requests. <br>

If you don't know your account, credentials or permissions you should contact your PiWeb Server administrator.
