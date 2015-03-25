---
area: restApi
level: 2
category: dataService
subCategory: serviceInformation
title: REST API
subTitle: Data Service
pageTitle: Service Information
permalink: /restapi/dataservice/serviceInformation/
---

## {{ page.pageTitle }}

### Endpoints

The service information can be fetched via the following endpoint. This endpoint doesn't provide filter parameters.

{% assign linkId="serviceInformationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/serviceInformation" %}
{% assign summary="Returns general information about PiWeb server and data service" %}
{% assign description="" %}
{% assign exampleCaption="Get Service Information for a given connection" %}

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

### General Information

Service information requests always have the smallest response time and are therefore well suited for checking the connection. Fetching the service information doesn't require authentication.
The first service information request triggers the calculation of the database statistics. Since the service information call returns immediately the values ```partCount```, ```characteristicsCount```, ```measurementsCount``` and ```valuesCount``` are empty in the first response. These values will be set once the statistics have been calculated; usually on the second call.

The returned ServiceInformation object contains of the following properties:

{% capture table %}
Property | Description
---------|-------------
serverName | The name of the PiWeb server as specified in the server settings dialog
version | The version number of the PiWeb server
securityEnabled | Indicates whether authentication is required by the server.
edition | The database edition. Should generally be PiwebDB.
versionWsdlMajor | The major version number of the interface.
versionWsdlMinor | The minor version number of the interface.
partCount | The number of parts stored on the server
characteristicCount | The number of characteristics stored on the server
measurementCount | The number of measurements stored on the server
valueCount | The number of measured values stored on the server
featureList | A list of features supported by the server.
inspectionPlanTimestamp | Timestamp of the last inspection plan modification
measurementTimestamp | Timestamp of the last measurement modification
configurationTimestamp | Timestamp of the last configuration timestamp
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
