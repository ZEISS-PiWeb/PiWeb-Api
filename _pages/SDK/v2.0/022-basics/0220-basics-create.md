<h2 id="{{page.sections['basics']['secs']['create'].anchor}}">{{page.sections['basics']['secs']['create'].title}}</h2>

Creating a .NET REST client is quite simple:

### DataService

{% capture table %}
Constructor method | Description
-------------------|-------------
`public DataServiceRestClient( Uri serverUri, ILoginRequestHandler loginRequestHandler = null, int maxUriLength = RestClient.DefaultMaxUriLength )` | Instantiates the client with the server uri passed as Uri object.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A data service client pointed to "http://piwebserver:8080.

{% highlight csharp %}
var uri = new Uri("http://piwebserver:8080");
var dataserviceRestClient = new DataServiceRestClient( uri );
{% endhighlight %}


### RawDataService

{% capture table %}
Constructor method | Description
-------------------|-------------
`public RawDataServiceRestClient( Uri serverUri, ILoginRequestHandler loginRequestHandler = null, int maxUriLength = RestClient.DefaultMaxUriLength )` | Instantiates the client with the server uri passed as Uri object.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A raw data service client pointed to "http://piwebserver:8082"

{% highlight csharp %}
var uri = new Uri("http://piwebserver:8082");
var rawdataserviceRestClient = new RawDataServiceRestClient( uri );
{% endhighlight %}
<br>
>{{ site.images['info'] }} Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed.

>{{ site.images['info'] }} All methods accept a `CancellationToken` which you can use to cancel a request.
