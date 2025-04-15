<h2 id="{{page.sections['basics']['secs']['create'].anchor}}">{{page.sections['basics']['secs']['create'].title}}</h2>

Examples in this section:
+ [Creating a DataServiceClient](#-example--creating-a-dataserviceclient)
+ [Creating a RawDataServiceClient](#-example--creating-a-rawdataserviceclient)
+ [Creating REST clients using the RestClientBuilder](#-example--creating-rest-clients-using-the-restclientbuilder)
<hr>

Creating a .NET REST client is quite simple. You choose the required client based on the use case (`DataServiceRestClient` for parts, measurements, values and configuration, or `RawDataServiceRestClient` for additional data), and pass in the `Uri` object pointing to your PiWeb server or your PiWeb cloud database. The SDK also offers a `RestClientBuilder` for easy creation and configuration of necessary REST clients with a fluent syntax. 

The `Uri` for your PiWeb server can be found in the PiWeb server UI. The `Uri` for PiWeb cloud is the base URI https://piwebcloud-service.metrology.zeiss.com with the database id appended. The database id can be found
in PiWeb cloud portal.

>{{ site.images['info'] }} All REST clients implement the `IDisposable` interface, keep in mind to dispose the client during shutdown of your application.

{{ site.headers['example'] }} Creating a DataServiceClient

{% highlight csharp %}
//The Uri of your PiWeb server or PiWeb cloud database
var uri = new Uri( "http://piwebserver:8080" );

//Creating the client
using var DataServiceClient = new DataServiceRestClient( uri );
{% endhighlight %}
<br>
{{ site.headers['example'] }} Creating a RawDataServiceClient

{% highlight csharp %}
//The Uri of your PiWeb server or PiWeb cloud database
var uri = new Uri( "https://piwebcloud-service.metrology.zeiss.com/xxxxxxxx-xxxx-xxxx-xxxx-xxxxxxxxxxxx" );

//Creating the client
using var RawDataServiceClient = new RawDataServiceRestClient( uri );
{% endhighlight %}
<br>
>{{ site.images['info'] }} Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed.

>{{ site.images['info'] }} All methods accept a `CancellationToken` which you can use to cancel a request.
<br>
<h4>Using the RestClientBuilder</h4>
Our .NET SDK offers a `RestClientBuilder` for easy creation and configuration of necessary REST clients with a fluent syntax. You configure this builder once, and can then create identically configured REST clients.
While REST clients should be reused and have a longer lifespan, using the builder is a convenient way of efficiently creating REST clients where reusing them is not feasable. A number of settings can optionally be configured before actually creating the REST clients. Default values are assumed for any setting that is not configured. The only required setting is the PiWeb Server uri which has no sensible default value. For all available settings, check the offered methods of the `RestClientBuilder` in your IDE.

{{ site.headers['example'] }} Creating REST clients using the RestClientBuilder

{% highlight csharp %}
//The Uri of your PiWeb server or PiWeb cloud database
var uri = new Uri( "http://piwebserver:8080" );

//Creating the builder
using var restClientBuilder = new RestClientBuilder( uri );

//Configure the builder
restClientBuilder
    .SetTimeout( TimeSpan.FromMinutes( 5 ) )
    .SetMaxUriLength( 8192 )
    .SetMaxRequestsInParallel( 8 );

//Create the REST clients using the builder
using var DataServiceClient = restClientBuilder.CreateDataServiceRestClient();
using var RawDataServiceClient = restClientBuilder.CreateRawDataServiceRestClient();    
{% endhighlight %}
