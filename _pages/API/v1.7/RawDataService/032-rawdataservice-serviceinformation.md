<h2 id="{{page.sections['rawdataservice']['secs']['serviceInformation'].anchor}}">{{page.sections['rawdataservice']['secs']['serviceInformation'].title}}</h2>

### Endpoints

The service information can be fetched via the following endpoint. This endpoint doesn't provide filter parameters.

{% assign linkId="rawDataEndpointGetServiceInformation" %}
{% assign method="GET" %}
{% assign endpoint="/serviceInformation" %}
{% assign summary="Returns general information about the raw data service" %}
{% assign description="" %}
{% assign exampleCaption="Get service information for a given connection" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawdataServiceRest/serviceInformation HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

 {
  "version": "7.8.0.0"
 }

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}
