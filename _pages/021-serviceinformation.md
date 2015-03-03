---
category: dataservice
subCategory: serviceInformation
title: Data Service
subTitle: Service Information
isSubPage: true
permalink: /dataservice/serviceinformation/
sections:
  general: General Information
  endpoint: REST API Endpoint Information
  sdk: .NET SDK Methods
  get: Get Service Information
redirect_from: "/dataservice/"
---

## {{ page.sections['general'] }}

Fetching service information is guaranteed to be very fast and is therefore well suited for checking the connection. This method can always be invoked without having credentials specified. 
The ServiceInformation object which is returned contains of the following properties:

Property | Description
---------|-------------
serverName | The name of the PiWeb server as it is specified in the server settings dialog
version | The version number of the PiWeb server
securityEnabled | Indicates if security is server side enabled or not.
edition | The database edition. Should generally be PiwebDB.
versionWsdlMajor | The major version number of the interface.
versionWsdlMinor | The minor version number of the interface.
partCount | Number of parts stored in the server
characteristicCount | Number of characteristics stored in the server
measurementCount | Number of measurements stored in the server
valueCount |Number of measured values stored in the server
featureList | Includes the erver side supported features.
inspectionPlanTimestamp | Timestamp of the last inspection plan modification
measurementTimestamp | Timestamp of the last measurement modification
configurationTimestamp | Timestamp of the last configuration timestamp

## {{ page.sections['endpoint'] }}

Service information can be fetched via the following endpoint. There are no filter parameter to restrict the query.

URL Endpoint | GET | PUT | POST | DELETE
-------------|-----|-----|------|-------
/serviceInformation | Returns general information about the PiWeb-Server | not supported | not supported | not supported

## {{page.sections['sdk'] }}

### Get Service Information

Method Name | Parmeter<br>*Optional Parameter[default value]* | Parameter Description
------------|-------------------------------------------------|----------------------
GetServiceInformation | *CancellationToken ct [null]* |  The ```CancellationToken ct``` gives the possibility to cancel the asyncronous call.

## {{ page.sections['get'] }}

The first time service information are fetched from the server database statistics values need to be created. As the service information call should return immediately statistics creation is triggered in a separate task. Therefore the statistical values partCount, characteristicsCount, measurementsCount and valuesCount stays empty in the first response but should generally contain values on the second call.

### {{ site.headers['example'] }}  Get Service Information for a given connection

{{ site.sections['beginExampleWebService'] }}

{{ site.headers['request']  | markdownify }}

{% highlight http %}
GET /dataServiceRest/serviceInformation HTTP/1.1
{% endhighlight %}

{{ site.headers['response']  | markdownify }}

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

{{ site.sections['endExample'] }}
{{ site.sections['beginExampleAPI'] }}

{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
ServiceInformation information = client.GetServiceInformation();
{% endhighlight %}

{{ site.sections['endExample'] }}
