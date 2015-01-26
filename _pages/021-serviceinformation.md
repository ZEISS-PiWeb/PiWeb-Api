---
category: dataservice
subCategory: serviceInformation
title: Data Service
subTitle: Service Information
isSubPage: true
permalink: /dataservice/serviceinformation/
sections:
  endpoint: Endpoint Information
  get: Get Service Information
---

## {{ page.sections['endpoint'] }}

Service information can be fetched via the following endpoint. There are no filter parameter to restrict the query.

URL Endpoint | GET | PUT | POST | DELETE
-------------|-----|-----|------|-------
/serviceInformation | Returns general information about the PiWeb-Server | not supported | not supported | not supported

## {{ page.sections['get'] }}

Fetching service information is guaranteed to be very fast and is therefore well suited for checking the connection. This method can always be invoked without having credentials specified. 

### Get Service Information for a given connection

####Example for direct webservice call

Request:

{% highlight http %}
GET /dataServiceRest/serviceInformation HTTP/1.1
{% endhighlight %}

Response:
{% highlight json linenos %}
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
          "queryTimeout": 0,
          "queryRowLimit": 0,
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

####Example for webservice call via API.dll

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
ServiceInformation information = client.GetServiceInformation();
{% endhighlight %}
