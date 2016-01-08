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
   RestClientHelper.ParseToParameter(requestedPartAttributes:requestedPartAttributes, withHistory:withHistory);
  return
   await Get<InspectionPlanPart>($"parts/{partUuid}", cancellationToken, parameter.ToArray()).ConfigureAwait( false );
}
{% endhighlight %}

Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed. All methods accept a `CancellationToken` which you can use to cancel a request.
Useful hints can be found in the following Best practices section.

<h2 id="{{page.sections['general']['secs']['basics'].anchor}}">{{page.sections['general']['secs']['basics'].title}}</h2>

### Inspection Plan

In PiWeb the inspection plan consists of two different entity types - parts and characteristics. Parts are hold in class `SimplePart`, characteristics are hold in class `InspectionPlanCharacteristic`. Both are derived from the abstract base class `InspectionPlanBase` and consists of the following properties:

#### `InspectionPlanBase`

{% capture table %}
Property                                          | Description
--------------------------------------------------|--------------------------------------------------------------------
<nobr><code>Guid</code> Uuid</nobr>               | Identifies this inspection plan entity uniquely.
<nobr><code>PathInformation</code> Path</nobr>    | The path of this entity which describes the entity's hierarchical structure.
<nobr><code>Attribute[]</code> Attributes</nobr>  | A set of attributes which describe the entity.
<nobr><code>string</code> Comment</nobr>          | A comment which describes the last inspection plan change.
<nobr><code>uint</code> Version</nobr>            | Contains the entity´s revision number. The revision number starts. with `0` and is incremented by `1` each time changes are applied to the inspection plan. The version is only returned in case versioning is enabled in the server settings.
<nobr><code>DateTime</code> TimeStamp</nobr>      | Contains the date and time of when the entity was last updated.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

A `SimplePart` does additionally consist of the timestamp for the most recent characteristic change:

#### `SimplePart` : `InspectionPlanBase`

{% capture table %}
Property                                          | Description
--------------------------------------------------|----------------------------------------------------------------
<nobr><code>dateTime</code> CharChangeDate</nobr> | The timestamp for the most recent characteristic change on any characteristic below that part (but not below sub parts). This timestamp is updated by the server backend.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Parts as well as characteristic may contain a version history. For parts class `InspectionPlanPart` which is derived from `SimplePart` is used.

#### `InspectionPlanCharacteristic` : `InspectionPlanBase`

#### `InspectionPlanPart` : `SimplePart`

{% capture table %}
Property                                               | Description
-------------------------------------------------------|-----------------------------------------------------
<nobr><code>InspectionPlanBase[]</code> History</nobr> | The version history for this inspection plan entity.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
