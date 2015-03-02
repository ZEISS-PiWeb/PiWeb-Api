---
category: general
subCategory: restApi
title: General Information
subTitle: REST API
isSubPage: true
permalink: /general/restapi/
sections:
  addresses: Webservice Adresses
  formats: Formats
  codes: HTTP Status Codes
  parameter: URL Parameter
  envelope: Response Envelope
---

##{{page.sections['addresses']}}

The general addresses of the REST based services are:

####Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/DataServiceRest
{% endhighlight %}

and

####Raw Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/RawDataServiceRest
{% endhighlight %}

The instanceName as well as https is optional and depends on the server settings.

##{{page.sections['formats']}}

The input and output format is defined by the URL parameter format. If there is no URL parameter the format is detected automatically via HTTP-Header field content-type. If this field is not present the default format JSON is used. The JSON format is the most performant and memory efficient format at the moment. The following additional formats are supported as well:

- JSON: Request and response is formatted using JavaScript Object Notation
- XML: Request and response is formatted using XML

##{{page.sections['codes']}}

Method        | Statuscode Ok        | Statuscode Failure                                       | Comment
------------- | :------------------- | -------------------------------------------------------- | -------
GET           | **200** (OK)         | **400** (Bad request) - Request fails <br> **404** (Not found) - Endpoint(s) or item does not exist | --- <br> If not any of the requested items exist status code 404, otherwise status code 200 is responded.
POST           | **201** (Created)    | **400** (Bad request) – Request fails, e.g. due badly formatting <br> **404** (Not found) – Endpoint does not exist <br> **409** (Conflict) – Resource already exists | Status code 400 is responded if creation of at least one item fails. <br> --- <br><br> Status code 409 is responded if at least one item already exists.
PUT          | **200** (Ok)         | **400** (Bad request) –  Request fails, e.g. due badly formatting <br> **404** (Not found) – Endpoint or item(s) does not exist | Status code 400 is responded if update of at least one item fails <br> Status code 404 responded if at least one item does not exist
DELETE        | **200** (Ok) | **400** (Bad request) – Request fails <br><br> **404** (Not found) – Endpoint or item(s) does not exist | Status code 400 is responded if deletion of at least one item fails <br> Status code 404 responded if  not any of the requested items exist

##{{page.sections['parameter']}}

Information about the formatting and restriction of the request and response can be determined by attaching the parameters _**format**_, _**indent**_ and _**filter**_ to the respective endpoint of the webservice URL in the following format:

{% highlight http %}
?parameter=name:value[|name:value] 
{% endhighlight %}

example: 

{% highlight http %}
?filter=deep:true|orderBy:4 asc
{% endhighlight %}

The  _**format**_ and _**indent**_ parameter can have the following name-value pairs. They are accepted by all endpoints and all implemented HTTP methods GET, PUT, POST and DELETE.

Parameter name | Possible values [**default value**] | Description
---------------|------------------------------------|--------------------------------------------------------------
format         | **json**, xml                       | Determines the input and output format.
indent         | true, **false**                     | Determines if the response message should be indented or not.  

The _**filter**_ parameter can have the following name-value pairs. Lists of Ids or UUIDs need to be surrounded by “{“ and “}”, the values within the list are separated by “,”.

###Inspection Plan

Parameter name      | Possible values [**default value**] | Description  <br> ```Example``` | Accepted by endpoint | Accepted by HTTP methods
--------------------|-----------------|-------------|----------------------|--------------------------
depth               | i, i ≥ 0  <br>**1**  | Restricts the query to the specified depth of the inspection plan tree. <br><br>Example:<br>```depth:5``` | parts, characteristics | GET
withHistory         | true, **false**      | Determines if the version history should be fetched or not. Does only effect the query if versioning is server side activated. <br><br>Example:<br>```withHistory:true``` | parts, characteristics | GET
partAttributes      | IDs of the attributes | Restricts the query to the attributes that should be returned for parts. <br><br>Example:<br>```partAttributes:{1001,1008}``` | parts | GET
characteristicAttributes | IDs of the attributes | Restricts the query to the attributes that should be returned for characteristics. <br><br>Example:<br>```characteristicAttributes:{2001,2101}``` | characteristics | GET

###Measurements and Measured Values

Parameter name      | Possible values [**default value**] | Description <br> ```Example```| Accepted by endpoint | Accepted by HTTP methods
--------------------|-----------------|-------------|----------------------|--------------------------
deep                | true, false     | Determines if the query should affect all layers. <br> ```deep:true``` | measurements, values | GET
orderBy             | ID(s) of the attribute(s) and order direction | Determines which attribute key(s) and which direction the key(s) should be ordered by <br> ```orderBy:4 asc, 10 desc``` | measurements, values | GET
searchCondition     | AttribueKey, Operator and Value| Restricts the query to given condition(s). Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> Several restrictions are combined by '+', the format for date/time have to be “yyyy-mm-ddThh:mm:ssZ”. The values needs to be surrounded by [ and ]. <br> ```searchCondition:4>[2012-11-13T00:00:00Z]``` | measurements, values | GET, DELETE
limitResult         | i, i∈N | Restricts the number of result items. <br> ```limitResult:100``` | measurements, values | GET
requestedMeasurementAttributes | IDs of the attributes | Restricts the query to the attributes that should be returned for measurements. <br> ```requestedMeasurementAttributes:{4,8}``` | measurements, values | GET
requestedValueAttributes | IDs of the attributes |List of attributes that should be returned for values. <br> ```requestedValueAttributes:{1,8}```| values | GET
characteristicsUuidList | Uuids of the characteristics | Restricts the query to the characteristics for which values should be returned or deleted. <br> ```characteristicsUuidList:{525d15c6-dc70-4ab4-bd3c-8ab2b5780e6b, 8faae7a0-d1e1-4ee2-b3a5-d4526f6ba822}``` | values | GET, DELETE

###Raw Data

Parameter name      | Possible values | Description <br> ```Example```| Accepted by endpoint | Accepted by HTTP methods
--------------------|-----------------|-------------|----------------------|--------------------------
uuids               | Uuids of the entities | Restricts the query to the entities identified by the given uuids. <br> {{site.images['warning']}} Entites of type 'Value' are identified by a compound key, which consists of the uuid of the Measurement, '&#124;' and the Characteristics uuid <br> <code>uuids:{652ae7a0-d1e1-4ee2-b3a5-d4526f6ba822&#124; 78bd15c6-dc70-4ab4-bd3c-8ab2b5780b52}</code>| rawData | GET

##Response envelope
Every response excluding streamed data responses consits of an response envelope which includes meta data and the data responded from the webservice. A typical response envelope looks as follows:

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success",
   "data":
   {...}
}
{% endhighlight %}

The possible status codes and their meanings can be found in the upper [section](#{{page.sections['codes'] | downcase | replace:' ','-' }}).
