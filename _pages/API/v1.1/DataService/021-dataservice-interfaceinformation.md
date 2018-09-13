<h2 id="{{page.sections['dataservice']['secs']['interfaceInformation'].anchor}}">{{page.sections['dataservice']['secs']['interfaceInformation'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['interfaceInformation'].anchor}}-endpoints">Endpoints</h3>

Information about supported interface version can be fetched calling the root endpoint of DataService:

{% assign linkId="interfaceInformationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/" %}
{% assign summary="Returns supported interface version for data service" %}
{% assign description="" %}
{% assign exampleCaption="Get interface version for Data Service" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/ HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{"supportedVersions":["1.4.2"]}

{% endhighlight %}
{% endcapture %}
{% include endpointTab.html %}

<h3 id="{{page.sections['interfaceInformation']['secs']['interfaceinformation'].anchor}}-objectstructure">Object Structure</h3>

Interface information returns the highest available minor version of each major version. This lets the user decide which features can be used.
As a minor version is backwards compatible to all previous minor versions, there is no point in enumerating them all. An user want to know which revision of each major version is available as that means they can work around possible bugs of older revisions.