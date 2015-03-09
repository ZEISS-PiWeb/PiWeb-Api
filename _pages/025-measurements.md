---
category: dataservice
subCategory: measurements
title: Data Service
subTitle: Measurements and Measured Values
isSubPage: true
permalink: /dataservice/measurements/
sections:
  general: General Information
  endpoint: REST API Endpoints
  sdk: .NET SDK Methods
---

## {{ page.sections['general'] }}

Measurements do always belong to a single inspection plan part. Depending on the purpose the measured values are included within a measurement (```DataMeasurement```) or not (```SimpleMeasurement```). Each measurement consists of the following properties:

### SimpleMeasurement

Name         | Type        | Description
-------------|-------------|--------------
uuid         | Guid        | Identifies this measurement uniquely.
partUuid     | Guid        | The uuid of the part the measurement belongs to.
attributes   | Attribute[] | A set of attributes which specifies this measurement.
lastModified | DateTime    | Contains the date and time of the last update applied to this measurement.

### DataMeasurement : SimpleMeasurement

Name            | Type                 | Description
----------------|----------------------|--------------
characteristics | DataCharacteristic[] | An array of the characteristics which has been measured within the measurement. Each characteristic within this array consits of the uuid it is identified by and an array of attributes which include at least the measured value attribute.

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Measurements can be fetched, created, updated and deleted via the following endpoints.

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|------|-----|-------
/parts/measurements | Returns all measurements without measured values | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | Updates the committed measurements | Deletes all measurements.
/parts/:partPath/measurements | Returns the measurements without measured values which belongs to the part specified by *:partPath*  | *--* | *--* | Deletes the measurements which belongs to the part specified by *:partPath*
/parts/(:partUuids)/measurements | Returns the measurements without measured values which belongs to the parts specified by *:partUuids*  | *--* | *--* | Deletes the measurements which belongs to the parts specified by *:partUuids*
/parts/measurements/{:uuidList} | Returns the measurements without measured values which belongs to the parts that uuids are within the *:uuidList* | *--* | *--* |  Deletes all measurements which belongs to the parts that uuid are within the *:uuidList*
/parts/measurements/values | Returns all measurements including measured data | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | *--*
/parts/:partPath/measurements/values | Returns the measurements including measured values which belongs to the part specified by *:partPath* | *--* | *--* | *--*
/parts/(:partUuids)/measurements/values | Returns the measurements including measured values which belongs to the parts specified by *:partUuids* | *--* | *--* | *--*
/parts/measurements/{:uuidList}/values | Returns the measurements including measured values which belongs to the parts that uuids are within the *:uuidList* | *--* | *--* | *--*

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

### Get measurements

Measurements can be fetched in several ways and for several purposes:

* fetch all measurements - with or without values
* fetch measurements for a particular part by its path - with or without values
* fetch measurements for particular parts by their uuids - with or without values
* fetch measurements by their uuids - with or without values

Each request can be restricted by the respective filter values as described in the [URL-Parameter section]({{site.baseurl }}/general/restapi/#{{ page.subCategory }})

{% assign exampleCaption="Fetch all measurements for the 'metal part' including measured values - restrict to measurements newer than 01.01.2015" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/|metal%20part/measurements/values?filter=searchCondition:4>[2015-01-01T00:00:00Z] HTTP/1.1
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

{% include exampleFieldset.html %}

### Add measurements

Measurements can be created with or without measured values. To create a measurement it is necessary to transfer the measurement object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 

{% assign exampleCaption="Create a measurement without values" %}
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
      "lastModified": "2014-11-03T09:27:28.253Z",
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

{% include exampleFieldset.html %}

{% assign exampleCaption="Create a measurement with measured values" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements/values HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "lastModified": "2014-11-03T09:27:28.253Z",
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

{% include exampleFieldset.html %}
