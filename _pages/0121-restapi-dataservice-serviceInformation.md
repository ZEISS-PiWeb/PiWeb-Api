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

The returned ServiceInformation object has the following properties:

{% capture table %}
Property                                                   | Description
-----------------------------------------------------------|---------------------------------------------------------------
<nobr><code>string</code> serverName</nobr>                | The name of the PiWeb server as specified in the server settings dialog
<nobr><code>string</code> version</nobr>                   | The version number of the PiWeb server
<nobr><code>bool</code> securityEnabled</nobr>             | Indicates whether authentication is required by the server
<nobr><code>string</code> edition</nobr>                   | The database edition. Usually this is "PiWebDB"
<nobr><code>string</code> versionWsdlMajor</nobr>          | The major version number of the interface
<nobr><code>string</code> versionWsdlMinor</nobr>          | The minor version number of the interface
<nobr><code>int</code> partCount</nobr>                    | The estimated number of parts stored on the server
<nobr><code>int</code> characteristicCount</nobr>          | The estimated number of characteristics stored on the server
<nobr><code>int</code> measurementCount</nobr>             | The estimated number of measurements stored on the server
<nobr><code>int</code> valueCount</nobr>                   | The estimated number of measured values stored on the server
<nobr><code>Features</code> featureList</nobr>             | A list of features supported by the server. This can be *MeasurementAggregation* or *DistinctMeasurementSearch*
<nobr><code>DateTime</code> inspectionPlanTimestamp</nobr> | Timestamp of the last inspection plan modification
<nobr><code>DateTime</code> measurementTimestamp</nobr>    | Timestamp of the last measurement modification
<nobr><code>DateTime</code> configurationTimestamp</nobr>  | Timestamp of the last configuration modification
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
