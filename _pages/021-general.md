<h2 id="{{page.sections['general']['secs']['create'].anchor}}">{{page.sections['general']['secs']['create'].title}}</h2>


The .NET REST clients provide multiple constructor overloads.

###Data Service

{% capture table %}
Constructor method | Description
-------------------|-------------
```public DataServiceRestClient( string serverUri )``` | Instantiates the client with the server uri passed as string.
```public DataServiceRestClient( Uri serverUri )``` | Instantiates the client with the server uri passed as Uri object.
```public DataServiceRestClient( string scheme, string host, int port, string instance = null )``` | Instantiates the client with the given uri segments.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A data service client for a server using https on host "piwebserver" on port 8080.

{% highlight csharp %}
var dataserviceRestClient = new DataServiceRestClient( "https", "piwebserver", 8080 );
{% endhighlight %}


###Rawdata Service

{% capture table %}
Constructor method | Description
-------------------|-------------
```public RawDataServiceRestClient( string serverUri )``` | Instantiates the client with the server uri passed as string.
```public RawDataServiceRestClient( Uri serverUri )``` | Instantiates the client with the server uri passed as Uri object.
```public RawDataServiceRestClient( string scheme, string host, int port, string instance = null )``` | Instantiates the client with the given uri segments.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A raw data service client pointed to "http://piwebserver:8082"

{% highlight csharp %}
var rawdataserviceRestClient = new RawDataServiceRestClient( "http://piwebserver:8082" );
{% endhighlight %}

<h2 id="{{page.sections['general']['secs']['use'].anchor}}">{{page.sections['general']['secs']['use'].title}}</h2>


To make a webservice request you can use the corresponding methods provided by the client classes. Please find detailed descriptions in each method's summary section (see example below).

{% highlight csharp %}
///<summary>
///Fetches a single part by its uuid.
///</summary>
///<param name="partUuid">The part's uuid</param>
///<param name="withHistory">Determines whether to return the version history for the part.</param>
///<param name="requestedPartAttributes">The attribute selector to determine which attributes to return.</param>
///<param name="cancellationToken">A token to cancel the asynchronous operation.</param>
public async Task<InspectionPlanPart> GetPartByUuid( Guid partUuid, AttributeSelector requestedPartAttributes = null, bool withHistory = false, CancellationToken cancellationToken = default(CancellationToken) )
{
var parameter =
 RestClientHelper.ParseToParameter( requestedPartAttributes: requestedPartAttributes, withHistory: withHistory );
return await Get<InspectionPlanPart>( $"parts/{partUuid}", cancellationToken, parameter.ToArray() ).ConfigureAwait( false );
}
{% endhighlight %}

Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed. All methods accept a `CancellationToken` which you can use to cancel a request.
Useful hints can be found in the following Best practices section.
