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

Measurements do always belong to a single inspection plan part. Depending on the purpose the measured values are included within a measurement (```SimpleMeasurement```) or not (```DataMeasurement```). Each measurement consists of the following properties:

### SimpleMeasurement

Name         | Type        | Description
-------------|-------------|--------------
uuid         | Guid        | Identifies this measurement uniquely.
partUuid     | Guid        | The uuid of the part the measurement belongs to.
attributes   | Attribute[] | A set of attributes which specifies this measurement.
lastModified | DateTime    | Contains the date and time of the last update applied to this measurement.

### DataMeasurement

Name            | Type                 | Description
----------------|----------------------|--------------
characteristics | DataCharacteristic[] | An array of the characteristics which has been measured within the measurement. Each characteristic within this array consits of the uuid it is identified by and an array of attributes which include at least the measured value attribute.

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Measurements can be fetched, created, updated and deleted via the following endpoints. Filter can be set as described in the [URL-Parameter section]({{site.baseurl }}/general/restapi/#{{ page.subCategory }}).

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|------|-----|-------
/parts/measurements | Returns all measurements without measured values | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | Updates the committed measurements | Deletes all measurements.
/parts/:partPath/measurements | Returns the measurements without measured values which belongs to the part specified by *:partPath*  | *Not supported* | *Not supported* | Deletes the measurements which belongs to the part specified by *:partPath*
/parts/measurements/{:uuidList} | Returns the measurements without measured values which belongs to the parts that uuids are within the *:uuidList* | *Not supported* | *Not supported* |  Deletes all measurements which belongs to the parts that uuid are within the *:uuidList*
/parts/measurements/values | Returns all measurements including measured data | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | *Not supported*
/parts/:partPath/measurements/values | Returns the measurements including measured values which belongs to the part specified by *:partPath* | *Not supported* | *Not supported* | *Not supported*
/parts/measurements/{:uuidList}/values | Returns the measurements including measured values which belongs to the parts that uuids are within the *:uuidList* | *Not supported* | *Not supported* | *Not supported*

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

### Add measurements

Measurements can be created with or without measured values. To create a measurement it is necessary to transfer the measurement object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 

{% assign exampleCaption="Create an measurement without values" %}
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

{{ site.headers['response']  | markdownify }}

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
