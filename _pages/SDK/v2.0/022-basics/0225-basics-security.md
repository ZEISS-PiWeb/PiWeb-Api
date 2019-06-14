<h2 id="{{page.sections['basics']['secs']['security'].anchor}}">{{page.sections['basics']['secs']['security'].title}}</h2>

TODO: redo and examples

Access to PiWeb server service might require authentication. Our .NET client supports all authentication modes:

* Basic authentication based on username and password,
* Windows authentication based on Active Directory integration,
* Certificate based authentication,
* Hardware certificate based authentication,
* OAuth (PiWeb Cloud only)

Authentication mode and credentilas (if neccessary) can be set via REST client's property `AuthenticationContainer`.

{{ site.headers['example'] }} Create a DataServiceRest client instance and set authentication mode to Windows Authentication

{% highlight csharp %}
var uri = new Uri("http://piwerbserver:8080");
var dataserviceRestClient = new DataServiceRestClient( uri );
dataServiceRestClient.AuthenticationContainer = new AuthenticationContainer(AuthenticationMode.Windows);
{% endhighlight %}
