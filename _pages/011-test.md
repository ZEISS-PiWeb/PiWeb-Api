<h2 id="{{page.sections['general']['secs']['addresses'].anchor}}">{{page.sections['general']['secs']['addresses'].title}}</h2>

The base addresses for the REST based services are:

#### Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/DataServiceRest
{% endhighlight %}

#### Raw Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/RawDataServiceRest

{% endhighlight %}

<br/>
{{ site.images['info'] }} `instanceName` and `https` are optional and depend on the server settings.

<h2 id="{{page.sections['formats'].anchor}}">{{page.sections['formats'].title}}</h2>

The input and output format is JSON as it is the most performance and memory efficient format at the moment.

<h2 id="{{page.sections['security'].anchor}}">{{page.sections['security'].title}}</h2>
Access to PiWeb server service might require authentication. Authentication can be either *basic authentication* based on username and password or *Windows authentication* based on Active Directory integration.

If PiWeb Server is secured by basic authentication you have to pass the credentials in the HTTP Authorization header. The authorization header must contain the `Basic` key word followed by base64 encoded `user:password` string:

{% highlight http %}

Authorization: Basic QWRtaW5pc3RyYXRvcjphZG0hbiFzdHJhdDBy

{% endhighlight %}

<h2 id="{{page.sections['parameter'].anchor}}">{{page.sections['parameter'].title}}</h2>

You can restrict requests by attaching certain parameters to the webservice URL in the following format:

{% highlight http %}
?parameter=value[&parameter=value] 
{% endhighlight %}

<br/>Example: 

{% highlight http %}
?deep=true&orderBy=4 asc
{% endhighlight %}

{{ site.images['info'] }} If the parameter contains lists of ids it needs to be surrounded by `{` and `}`, the values within the list are separated by `,`.

<h2 id="{{page.sections['codes'].anchor}}">{{page.sections['codes'].title}}</h2>

{% capture table %}
Method        | Statuscodes
------------- | -----------------------------------------------------------------------------------
GET           | **200** (OK)<br> **400** (Bad request) - Request failed <br> **404** (Not found) - Endpoint or item does not exist 
POST           | **201** (Created)<br> **400** (Bad request) – Creation of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint doesn't exist <br> **409** (Conflict) – An item does already exist
PUT          | **200** (OK)<br> **400** (Bad request) –  Update of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint or item(s) doesn't exist
DELETE        | **200** (OK)<br>**400** (Bad request) – Request of at least one item failed <br> **404** (Not found) – Endpoint or items do not exist
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


<h2 id="{{page.sections['response'].anchor}}">{{page.sections['response'].title}}</h2>
Every response consists either of the requested data or of an error message returned by the webservice. A typical error response looks as follows:

{% highlight json %}
{
   "message": "Unable to insert inspection plan items. An item with path '/metal part/' 
               [uuid: 05040c4c-f0af-46b8-810e-30c0c00a379e] does already exist."
}
{% endhighlight %}
