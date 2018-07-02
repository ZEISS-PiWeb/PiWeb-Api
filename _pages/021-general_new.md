<h2 id="{{page.sections['general']['secs']['create'].anchor}}">{{page.sections['general']['secs']['create'].title}}</h2>


Creating a .NET REST client is quite simple:

### Data Service

{% capture table %}
Constructor method | Description
-------------------|-------------
```public DataServiceRestClient( Uri serverUri, ILoginRequestHandler loginRequestHandler = null, int maxUriLength = RestClient.DefaultMaxUriLength )``` | Instantiates the client with the server uri passed as Uri object.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A data service client pointed to "http://piwebserver:8080.

{% highlight csharp %}
var uri = new Uri("http://piwerbserver:8080");
var dataserviceRestClient = new DataServiceRestClient( uri );
{% endhighlight %}


### Rawdata Service

{% capture table %}
Constructor method | Description
-------------------|-------------
```public RawDataServiceRestClient( Uri serverUri, ILoginRequestHandler loginRequestHandler = null, int maxUriLength = RestClient.DefaultMaxUriLength )``` | Instantiates the client with the server uri passed as Uri object.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A raw data service client pointed to "http://piwebserver:8082"

{% highlight csharp %}
var uri = new Uri("http://piwerbserver:8082");
var rawdataserviceRestClient = new RawDataServiceRestClient( uri );
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
   RestClientHelper.ParseToParameter(requestedPartAttributes:requestedPartAttributes, withHistory:withHistory);
  return
   await Get<InspectionPlanPart>($"parts/{partUuid}", cancellationToken, parameter.ToArray()).ConfigureAwait( false );
}
{% endhighlight %}


{{ site.images['info'] }} Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed. 

{{ site.images['info'] }} All methods accept a `CancellationToken` which you can use to cancel a request.

{{ site.headers['bestPractice'] }} Create or update multiple entities in a single call

To achieve a good performance it is highly recommended to create or update items in a single call. That is why all create and update methods expect an array parameter.

{% highlight csharp %}
var charPath1 = PathHelper.String2PathInformation( "/Part/Char1", "PC");
var charPath2 = PathHelper.String2PathInformation( "/Part/Char2", "PC");
var charPath3 = PathHelper.String2PathInformation( "/Part/Char3", "PC");
var char1 = new InspectionPlanCharacteristic { Path = char1Path, Uuid = Guid.NewGuid() };
var char2 = new InspectionPlanCharacteristic { Path = char2Path, Uuid = Guid.NewGuid() };
var char3 = new InspectionPlanCharacteristic { Path = char3Path, Uuid = Guid.NewGuid() };
var characteristics = new[] { char1, char2, char3 };
await RestDataServiceClient.CreateCharacteristics( characteristics );
{% endhighlight %}