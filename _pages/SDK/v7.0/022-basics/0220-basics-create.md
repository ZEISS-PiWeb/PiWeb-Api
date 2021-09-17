<h2 id="{{page.sections['basics']['secs']['create'].anchor}}">{{page.sections['basics']['secs']['create'].title}}</h2>

Examples in this section:
+ [Creating a DataServiceClient](#-example--creating-a-dataserviceclient)
+ [Creating a RawDataServiceClient](#-example--creating-a-rawdataserviceclient)
<hr>

Creating a .NET REST client is quite simple. You choose the required client based on the use case (DataService for parts, measurements, values and configuration, or RawDataService for additional data), and pass in the `Uri` object pointing to your PiWeb server.

{{ site.headers['example'] }} Creating a DataServiceClient

{% highlight csharp %}
//The Uri of your PiWeb server
var uri = new Uri( "http://piwebserver:8080" );

//Creating the client
var DataServiceClient = new DataServiceRestClient( uri );
{% endhighlight %}
<br>
{{ site.headers['example'] }} Creating a RawDataServiceClient

{% highlight csharp %}
//The Uri of your PiWeb server
var uri = new Uri( "http://piwebserver:8080" );

//Creating the client
var RawDataServiceClient = new RawDataServiceRestClient( uri );
{% endhighlight %}
<br>
>{{ site.images['info'] }} Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed.

>{{ site.images['info'] }} All methods accept a `CancellationToken` which you can use to cancel a request.
