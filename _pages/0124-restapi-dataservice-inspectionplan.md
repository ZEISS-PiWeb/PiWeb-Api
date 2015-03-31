---
area: restApi
level: 2
category: dataService
subCategory: inspectionPlan
title: REST API
subTitle: Data Service
pageTitle: Inspection Plan
permalink: /restapi/dataservice/inspectionplan/
---

## {{ page.pageTitle }}

### Endpoints

You can fetch, create, update and delete parts and characteristics via the following endpoints: 

####Parts

{% assign linkId="inspectionPlanEndpointGetAllParts" %}
{% assign method="GET" %}
{% assign endpoint="/parts" %}
{% assign summary="Fetches parts" %}
{% capture description %}
You can fetch all parts or certain parts. Possible [filter uri parameters](#filters) are `partUuids`, `partPath`, `depth`, `withHistory` and `requestedPartAttributes`.
{% endcapture %}
{% assign exampleCaption="Fetch a part by its path '/metal part' without possible child parts restricted to several attributes" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/parts?partPath=/metal%20part&depth=0&requestedPartAttributes={1001,1003} HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
      {
           "path": "P:/metal part/",
           "charChangeDate": "2014-11-19T10:48:32.917Z",
           "attributes": { "1001": "4466", "1003": "mp" },
           "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
           "version": 0,
           "timestamp": "2012-11-19T10:48:32.887Z",
           "current": true
       }
   ]
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointGetPart" %}
{% assign method="GET" %}
{% assign endpoint="/parts/:partUuid" %}
{% assign summary="Fetches a certain part by its :partUuid" %}
{% assign description="" %}
{% assign exampleCaption="Fetch the part '/metal part' by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/parts/05040c4c-f0af-46b8-810e-30c0c00a379e HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
      {
           "path": "P:/metal part/",
           "charChangeDate": "2014-11-19T10:48:32.917Z",
           "attributes": {},
           "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
           "version": 0,
           "timestamp": "2012-11-19T10:48:32.887Z",
           "current": true
       }
   ]
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}



{% assign linkId="inspectionPlanEndpointAddParts" %}
{% assign method="POST" %}
{% assign endpoint="/parts" %}
{% assign summary="Creates parts" %}
{% capture description %}

If you want to create an inspection plan entity it is necessary that you transfer the entity object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which you use for the attributes must come from the parts attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in the server settings.
{% endcapture %}

{% assign exampleCaption="Adding the 'metal part' part with the uuid 05040c4c-f0af-46b8-810e-30c0c00a379e" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
    "path": "/metal part",
    "attributes": { "1001": "4466", "1003": "mp" }       
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


{% assign linkId="inspectionPlanEndpointUpdateParts" %}
{% assign method="PUT" %}
{% assign endpoint="/parts" %}
{% assign summary="Updates parts" %}
{% capture description %}

If you update a part you might want to:

* Rename/move parts
* Change part's attributes

{{site.images['info']}} If versioning is activated on server side, every update of one or more parts creates a new version entry.
{% endcapture %}

{% assign exampleCaption="Change the "metal part"*s attributes" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "/metal part",
     "attributes": { "1001": "4469", "1003": "metalpart" }       
     "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="inspectionPlanEndpointDeleteParts" %}
{% assign method="DELETE" %}
{% assign endpoint="/parts" %}
{% assign summary="Deletes parts" %}
{% capture description %}
There are two possibilities you can delete parts, either by their path or by their uuids. This means that one of the filter parameters `partPath` or `partUuids` has to be set. In both cases the entity itself as well as all children are deleted.
{% endcapture %}

{% assign exampleCaption="Delete the part 'metal part'  and all entities beneath it" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/parts?partPath=/metal%20part HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointDeletePart" %}
{% assign method="DELETE" %}
{% assign endpoint="/parts/:partUuid" %}
{% assign summary="Delete a part by its :partUuid" %}
{% capture description %}
If you delete a part the entity itself as well as all children are deleted.
{% endcapture %}

{% assign exampleCaption="Delete the part 'metal part' and all entities beneath it by its guid" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/parts/05040c4c-f0af-46b8-810e-30c0c00a379e HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


<p></p>



####Characteristics

{% assign linkId="inspectionPlanEndpointGetAllChars" %}
{% assign method="GET" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Fetches characteristics" %}
{% assign description="You can fetch all characteristics or the characteristics described by the uri parameters. Possible [filter uri parameters](#filters) are `partUuids`, `partPath`, `charUuids`, `charPath`, `depth`, `withHistory` and `requestedCharacteristicAttributes`.Only direct characteristics are fetched, characteristics beneath child parts are not considered." %}
{% assign exampleCaption="Fetch all characteristics beneath the part '/metal part' until depth=2" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/characteristics?partPath=/metal%20part&depth=2 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
      {
           "path": "PC:/metal part/deviation_3/",
           "attributes": { ... },
           "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7",
           "version": 0,
           "timestamp": "2012-11-19T10:48:32.887Z",
           "current": true
       },
      {
           "path": "PCC:/metal part/deviation_3/.X/",
           "attributes": { ... },
           "uuid": "51c8568a-9410-465a-a8ed-33063db41dac",
           "version": 0,
           "timestamp": "2015-03-24T08:17:28.03Z",
           "current": true
       },
       {
           "path": "PCC:/metal part/deviation_3/.Y/",
           "attributes": { ... },
           "uuid": "b7a30736-6e89-4dd5-9bc0-e6cb9eb5e2da",
           "version": 0,
           "timestamp": "2015-03-24T08:17:34.61Z",
           "current": true
       },
       {
           "path": "PCC:/metal part/deviation_3/.Z/",
           "attributes": { ... },
           "uuid": "1175919c-5c59-487e-a0fb-deac04510046",
           "version": 0,
           "timestamp": "2015-03-24T08:17:38.423Z",
           "current": true
       }
   ]
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointGetChar" %}
{% assign method="GET" %}
{% assign endpoint="/characteristics/:charUuid" %}
{% assign summary="Fetches a certain characteristics by its :charUuid" %}
{% assign description="" %}
{% assign exampleCaption="Fetch the characteristics '/metal part/deviation_3' by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/characteristics/27e23a7c-dbe7-4863-8461-6abf7b03ddd7 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
      {
           "path": "PC:/metal part/deviation_3/",
           "attributes": { ... },
           "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7",
           "version": 0,
           "timestamp": "2012-11-19T10:48:32.887Z",
           "current": true
       }
   ]
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointAddChars" %}
{% assign method="POST" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Creates characteristics" %}
{% capture description %}

If you create characteristics it is necessary that you transfer the characteristics within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in the server settings.
{% endcapture %}

{% assign exampleCaption="Adding the characteristic 'metal part/deviation_3'" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/characteristics HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "PC:/metal part/deviation_3/",
     "attributes": 
      { 
         "2004": "3",
         "2101": "0",
         "2110": "-0.5",
         "2111": "0.5" 
      },
     "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7"
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


{% assign linkId="inspectionPlanEndpointUpdateChars" %}
{% assign method="PUT" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Updates characteristics" %}
{% capture description %}

If you update characteristics you want to:

* Rename/move characteristics or
* Change characteristic's attributes

{{site.images['info']}} If versioning is activated on server side, every update of one or more parts creates a new version entry.
{% endcapture %}

{% assign exampleCaption="Change the "metal part/deviation_3"*s attributes" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/characteristics HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "/metal part/deviation_3",
     "attributes": { "2110": "-1.0", "2111": "1.0"  }       
     "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="inspectionPlanEndpointDeleteCharacteristics" %}
{% assign method="DELETE" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Deletes characteristics" %}
{% capture description %}
There are two possibilities you can delete characteristics, either by their path or by their uuids. This means that one of the filter parameters `charPath` or `charUuids` has to be set. In both cases the entity itself as well as all children are deleted.
{% endcapture %}

{% assign exampleCaption="Delete the characteristic 'metal part/deviation_3'  and all entities beneath it" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/parts?charPath=/metal%20part/deviation_3 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointDeleteChar" %}
{% assign method="DELETE" %}
{% assign endpoint="/characteristics/:charUuid" %}
{% assign summary="Delete a characteristic by its :charUuid" %}
{% capture description %}
If you delete a characteristic, the entity itself as well as all children are deleted.
{% endcapture %}

{% assign exampleCaption="Delete the characteristic 'metal part/deviation_3' and all entities beneath it by its guid" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/characteristics/27e23a7c-dbe7-4863-8461-6abf7b03ddd7 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

<br/>

### Filters

The described endpoints provide the following filters:

{% capture table %}
Parameter name        | Possible values [**default value**] | Description
----------------------|-------------------------------------|--------------------------------
`partUuids`           | Guids of the parts | Restricts the query to these parts guids 
`partPath`            | Path of the part | Restricts the query to this part path  
`charUuids`           | Guids of the characteristics | Restricts the query to these characteristics guids 
`charPath`            | Path of the characteristic | Restricts the query to this characteristic path 
`depth`               | i, i ≥ 0  <br>**1**  | It controls down to which level of the inspection plan the entities should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched and so on. <br><br>`depth=5` 
`withHistory`         | true, **false**      | Determines whether the version history should be fetched or not. Does only effect the query if versioning is activated on the server side. <br><br>`withHistory=true`
`requestedPartAttributes`      | **All**, None or IDs of the attributes | Restricts the query to the attributes that should be returned for parts. <br><br>`requestedPartAttributes={1001,1008}`
`requestedCharacteristicAttributes` | **All**, None or IDs of the attributes | Restricts the query to the attributes that should be returned for characteristics. <br><br>`requestedCharacteristicAttributes={2001,2101}`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

### General Information

Both parts and characteristics are PiWeb inspection plan entities. They consists of the following properties:

{% capture table %}
Property                  | Description
--------------------------|-----------------------
`Guid` uuid               | Identifies this inspection plan entity uniquely
`PathInformation` path    | The path of this entity
`Attribute` attributes    | A set of attributes which specifies this entity
`string` comment          | A comment which describes the last inspection plan change
`int` version             | Contains the revision number of the entity. The revision number starts with zero and is incremented by one each time when changes are applied to the inspection plan. The version is only returned in case versioning is enabled in the server settings
`bool` current            | Indicates whether the entity is the current version
timeStamp | `dateTime`    | Contains the date and time of the last update applied to this entity
`dateTime` charChangeDate | *(Part only)* The timestamp for the most recent characteristic change on any characteristic that belongs to this part
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
