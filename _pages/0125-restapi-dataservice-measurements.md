---
area: restApi
level: 2
category: dataService
subCategory: measurementsAndValues
title: REST API
subTitle: Data Service
pageTitle: Measurements and Values
permalink: /restapi/dataservice/measurementsandvalues/
---

## {{ page.pageTitle }}

### Endpoints

You can fetch, create, update and delete measurements and values via the following endpoints: 
<br/>

#### Measurements
{% assign linkId="measurementsGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/measurements" %}
{% assign summary="Fetches measurements" %}
{% capture description %}
You can fetch all measurements or certain measurements. Possible [filter uri parameters](#filters) are 
<code data-toggle="tooltip" data-placement="bottom auto" data-html="true" title="Restricts the query to this part path <br><br><code>partPath=/metal%20part</code>">partPath</code>
`partPath`, `partUuids`, `measurementUuids`, `deep`, `searchCondition`, `order`, `limitResult`, `requestedMeasurementAttributes`, `statistics` and `aggregation`.

{% endcapture %}
{% assign exampleCaption="Fetch measurements newer than 01.01.2015 for the part with the guid e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/measurements?partUuids=(e42c5327-6258-4c4c-b3e9-6d22c30938b2)&searchCondition=4>[2015-01-01T00:00:00Z] HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
{
  ...
   "data":
   [
     {
       "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
       "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
       "lastModified": "2015-03-09T09:19:38.653Z",
       "attributes":
       {
           "4": "2015-03-09T19:12:00Z",
           "6": "3",
           "7": "0"
       }
      },
      ...
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="measurementsGetOne" %}
{% assign method="GET" %}
{% assign endpoint="/measurements/:measUuid" %}
{% assign summary="Fetches a measurement by its :measUuid" %}
{% assign description="The request can be restricted by [```filter uri parameters```](#filters). Possible filters are `requestedMeasurementAttributes`, `statistics` and `aggregation`." %}

{% assign exampleCaption="Fetch a measurement by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/measurements/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
{
  ...
   "data":
   [
     {
       "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
       "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
       "lastModified": "2015-03-09T09:19:38.653Z",
       "attributes":
       {
           "4": "2015-03-09T19:12:00Z",
           "6": "3",
           "7": "0"
       }
      },
      ...
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsCreate" %}
{% assign method="POST" %}
{% assign endpoint="/measurements" %}
{% assign summary="Creates measurements" %}
{% capture description %}
To create a measurement, it is necessary to transfer the measurement object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 
{% endcapture %}
{% assign exampleCaption="Create a measurement" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "3",
        "7": "0"
      }     
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 201,
       "statusDescription": "Created"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/measurements" %}
{% assign summary="Updates measurements" %}
{% capture description %}
Updating a measurement does always affect the whole measurement. This means that the whole measurement, including attributes and values, needs to be transfered within the body of the request and is deleted and recreated again on server side.
{% endcapture %}

{% assign exampleCaption="Update a measurement - add and change an attribute" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "2",
        "7": "0",
        "8": "1"
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsDelete" %}
{% assign method="DELETE" %}
{% assign endpoint="/measurements" %}
{% assign summary="Deletes measurements" %}
{% capture description %}
There are several possibilities to delete measurements:

* Delete all measurements
* Delete measurements from a part by its path
* Delete measurements from parts by its uuids
* Delete measurements by their uuids

The delete condition may be further restricted by the [filter parameter](#filters) `searchCondition`.
{% endcapture %}

{% assign exampleCaption="Delete measurements newer than 01.01.2015 and older than 31.03.2015 from the part with the uuid e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements?partUuids={4b59cac7-9ecd-403c-aa26-56dd25892421}&searchCondition=4>[2015-01-01T00:00:00Z]+4<[2015-03-31T23:59:59Z] HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsDeleteOne" %}
{% assign method="DELETE" %}
{% assign endpoint="/measurements/:measUuid" %}
{% assign summary="Delete a measurement by its :measUuid" %}
{% assign description="" %}

{% assign exampleCaption="Delete a measurement by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}





#### Values
{% assign linkId="valuesGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/values" %}
{% assign summary="Fetches measurements including measured values" %}
{% capture description %}
You can fetch all measurements including values or certain measurements including values. Possible [filter uri parameters](#filters) are `partPath`, `partUuids`, `measurementUuids`, `deep`, `searchCondition`, `order`, `limitResult`, `characteristicUuids`, `requestedMeasurementAttributes`, `requestedValueAttributes` and `aggregation`.
{% endcapture %}
{% assign exampleCaption="Fetch measurements and values newer than 01.01.2015 restricted to a certain characteristic for a part restricted by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataservicerest/values?partUuids=(05040c4c-f0af-46b8-810e-30c0c00a379e)&searchCondition=4>[2010-11-04T00:00:00Z]&characteristicUuids=(b587d548-8aa6-42b7-b292-0f3e13452c3f) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
{
  ...
   "data":
   [
     {
      "characteristics":
           {
               "b587d548-8aa6-42b7-b292-0f3e13452c3f":
               {
                   "1": "-0.073420455529934786"
               }
           },
           "uuid": "88974561-a449-4a94-8b3e-970822b84406",
           "partUuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
           "lastModified": "2015-01-19T10:48:34.157Z",
           "attributes":
           {
               "4": "2010-11-05T20:30:57.6Z",
               "6": "5",
               "7": "4",
               "8": "7",
               "12": "4"
           }
      },
      ...
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="valuesGetOne" %}
{% assign method="GET" %}
{% assign endpoint="/values/:measUuid" %}
{% assign summary="Fetches a measurement including measured values by its :measUuid" %}
{% assign description="The request can be restricted by [```filter uri parameters```](#filters). Possible filters are `requestedMeasurementAttributes`, `requestedValueAttributes`, `characteristicUuids` and `aggregation`." %}

{% assign exampleCaption="Fetch a measurement including  all values by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/values/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
{
  ...
   "data":
   [
     {
      "characteristics":
       {
           "b587d548-8aa6-42b7-b292-0f3e13452c3f":
           {
               "1": "-0.073420455529934786"
           },
           ...
       },
       "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
       "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
       "lastModified": "2015-03-09T09:19:38.653Z",
       "attributes":
       {
           "4": "2015-03-09T19:12:00Z",
           "6": "3",
           "7": "0"
       }
      },
      ...
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="valuesCreate" %}
{% assign method="POST" %}
{% assign endpoint="/values" %}
{% assign summary="Creates measurements including values" %}
{% capture description %}
To create a measurement including values, it is necessary to transfer the measurement object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 
{% endcapture %}
{% assign exampleCaption="Create a measurement with measured values" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/values HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "3",
        "7": "0"
      },
      "characteristics":
      {
         "360f55e5-77c3-49f9-9a5e-80d0a9040e2d":
         {
             "1": "0.24966522"
         },
         "b5c98235-c75c-41a4-aced-2a38c70a3866":
         {
             "1": "0.4457339"
         },
         "85bbb406-810e-4062-8a9f-c7b636cb61bd":
         {
             "1": "0.24981162"
         }
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 201,
       "statusDescription": "Created"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="valuesUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/values" %}
{% assign summary="Updates measurements and values" %}
{% capture description %}
Updating a measurement does always affect the whole measurement. This means that the whole measurement, including attributes and values, needs to be transfered within the body of the request and is deleted and recreated again on server side.
{% endcapture %}

{% assign exampleCaption="Update a measurement - change a measured value" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "2",
        "7": "0",
        "8": "1"
      }
      "characteristics":
      {
         "360f55e5-77c3-49f9-9a5e-80d0a9040e2d":
         {
             "1": "0.24966522"
         },
         "b5c98235-c75c-41a4-aced-2a38c70a3866":
         {
             "1": "0.4467339"
         },
         "85bbb406-810e-4062-8a9f-c7b636cb61bd":
         {
             "1": "0.25981162"
         }
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}




### Filters
{% capture table %}
Parameter name      | Possible values [**default value**] | Description <br><br> ```Example```
--------------------|---------------------|-------------------------------------------------------------
`measurementUuids`         | Guids of the measurements | Restricts the query to these measurements <br><br> `measurementUuids={5b59cac7-9ecd-403c-aa26-56dd25892421}`
`partUuids`           | Guids of the parts | Restricts the query to these parts guids <br><br> `partUuids={e42c5327-6258-4c4c-b3e9-6d22c30938b2}`
`partPath`            | Path of the part | Restricts the query to this part path <br><br> `partPath=/metal%20part` 
`deep`                | true, **false**     | Determines whether the query should affect all layers. <br> `deep=true` 
`orderBy`             | ID(s) of the attribute(s) and order direction <br> **4 desc** | Determines which attribute key(s) and which direction the key(s) should be ordered by <br><br> `orderBy:4 asc, 10 desc`
`searchCondition`     | AttribueKey, Operator and Value| Restricts the query to given condition(s). Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> Multiple restrictions are combined with '+', the format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. The values need to be surrounded by [ and ]. <br><br> `searchCondition=4>[2012-11-13T00:00:00Z]`
`limitResult`         | i, i∈N | Restricts the number of result items. <br> `limitResult=100`
`requestedMeasurementAttributes` | IDs of the attributes | Restricts the query to the attributes that should be returned for measurements. <br><br> `requestedMeasurementAttributes={4,8}`
`requestedValueAttributes` | IDs of the attributes |List of attributes that should be returned for values. <br><br> `requestedValueAttributes={1,8}`
`characteristicsUuidList` | Uuids of the characteristics | Restricts the query to the characteristics for which values should be returned. <br><br> `characteristicsUuidList={525d15c6-dc70-4ab4-bd3c-8ab2b5780e6b, 8faae7a0-d1e1-4ee2-b3a5-d4526f6ba822}`
`statistics` | **None**, Simple, Detailed | Indicates how statistical information should be returned: <br>*None* = Return no information<br>*Simple* = Return statistical information including numvber of characteristics out of warning limit, number of characteristics out of tolerance and number of characteristics in warning limit and tolerance<br>Detailed = Return statistical information the same way as *Simple* plus the guid for each characteristic <br><br> `statistics=Simple`
`aggregation`          | **Measurements**, AggregationMeasurements, All | Specifies which types of measurements will be fetched. <br><br> `aggregation=All`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

### General Information

Measurements do always belong to a single inspection plan part. Depending on the purpose, the measured values are included within a measurement or not. Each measurement consists of the following properties:

{% capture table %}
Name            | Type                 | Description
----------------|----------------------|------------------------------------------------------------------------------------
uuid            | Guid                 | Identifies the measurement uniquely.
partUuid        | Guid                 | The uuid of the part the measurement belongs to.
attributes      | Attribute[]          | A set of attributes which specifies this measurement.
lastModified    | DateTime             | Contains the date and time of the last update applied to this measurement.
characteristics | DataCharacteristic[] | An array of the characteristics which has been measured within the measurement. Each characteristic within this array consits of the uuid it is identified by and an array of attributes which include at least the measured value attribute.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
