---
category: dataservice
subCategory: serviceInformation
title: Data Service
subTitle: Service Information
isSubPage: true
permalink: /dataservice/serviceinformation/
sections:
  general: General Information
  endpoint: REST API Endpoints
  sdk: .NET SDK Methods
redirect_from: "/dataservice/"
---

## {{ page.sections['general'] }}

Service information requests always have the smallest response time and are therefore well suited for checking the connection. Fetching the service information doesn't require authentication.
The first service information request triggers the calculation of the database statistics. Since the service information call returns immediately the values ```partCount```, ```characteristicsCount```, ```measurementsCount``` and ```valuesCount``` are empty in the first response. These values will be set once the statistics have been calculated; usually on the second call.

The returned ServiceInformation object contains of the following properties:

### ServiceInformation

{% capture table %}
Property | Type | Description
---------|------|-------------
serverName | ```string``` | The name of the PiWeb server as specified in the server settings dialog
version | ```string``` | The version number of the PiWeb server
securityEnabled | ```bool``` | Indicates whether authentication is required by the server.
edition | ```string``` | The database edition. Should generally be PiwebDB.
versionWsdlMajor | ```string``` | The major version number of the interface.
versionWsdlMinor | ```string``` | The minor version number of the interface.
partCount | ```int``` | The number of parts stored on the server
characteristicCount |```int``` | The number of characteristics stored on the server
measurementCount | ```int``` | The number of measurements stored on the server
valueCount | ```int``` | The number of measured values stored on the server
featureList | ```string[]``` | A list of features supported by the server.
inspectionPlanTimestamp | ```DateTime``` | Timestamp of the last inspection plan modification
measurementTimestamp | ```DateTime``` | Timestamp of the last measurement modification
configurationTimestamp | ```DateTime``` | Timestamp of the last configuration timestamp
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

## {{ page.sections['endpoint'] }}

The service information can be fetched via the following endpoint. This endpoint doesn't provide filter parameters.

{% assign linkId="serviceInformationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/serviceInformation" %}
{% assign summary="Returns general information about the PiWeb server" %}
{% assign description="" %}
{% assign exampleCaption="Get Service Information for a given connection" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/serviceInformation HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
       {
          "serverName": "PiWeb Server DEDRSW9KKNVY1",
          "version": "5.6.2.0",
          "securityEnabled": false,
          "edition": "PiWebDB",
          "versionWsdlMajor": "2",
          "versionWsdlMinor": "9",
          "partCount": 4,
          "characteristicCount": 125,
          "measurementCount": 20,
          "valueCount": 900,
          "featureList":
          [
             "MeasurementAggregation",
            "DistinctMeasurementSearch"
          ],
          "inspectionPlanTimestamp": "2014-11-24T16:08:58.812964+01:00",
          "measurementTimestamp": "2014-11-03T10:27:28.3461853+01:00",
          "configurationTimestamp": "2014-11-03T10:27:27.5245116+01:00"
       },
       ...
   ]
}
{% endhighlight %}
{% endcapture %}
{% include endpointTab.html %}

## {{page.sections['sdk'] }}

### Get Service Information

{% assign linkId="serviceInformationMethodGet" %}
{% assign caption="GetServiceInformation" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches the service information. " %}
{% capture parameterTable %}

Name           | Type                                  | Description
---------------|---------------------------------------|--------------------------------------------------
token          | ```CancellationToken```               | Parameter is optional allows to cancel the asyncronous call.
{% endcapture %}

{% assign returnParameter="Task<ServiceInformation>" %}
{% assign exampleCaption="Get the service information" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var serviceInformation = await client.GetServiceInformation();
{% endhighlight %}
{% endcapture %}
{% include methodTab.html %}
