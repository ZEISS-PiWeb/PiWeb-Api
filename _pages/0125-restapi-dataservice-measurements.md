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

{% assign linkId="measurementsGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/measurements" %}
{% assign summary="Fetches measurements" %}
{% capture description %}
You can fetch all measurements or certain measurements if you restrict the query. Possible filter parameters are `deep`, `searchCondition`, `orderBy`, `limitResult` or `requestedMeasurementAttributes` by [```filter uri parameters```](#filters).
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

The delete condition may be further restricted by the filter parameter `searchCondition`.

{% assign exampleCaption="Delete measurements newer than 01.01.2015 and older than 31.03.2015 from the part with the uuid e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}
{% assign comment="" %}
{% endcapture %}

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
