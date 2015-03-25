---
area: restApi
level: 2
category: rawDataService
subCategory: serviceInformation
title: REST API
subTitle: Raw Data Service
pageTitle: Service Information
permalink: /restapi/rawdataservice/serviceinformation/
---

## {{ page.pageTitle }}

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
   ...
   "data":
   [
       {
        "versionWsdlMajor": "2",
        "versionWsdlMinor": "3",
        "version": "5.8.0.0"
       }
   ]
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}
