---
area: restApi
level: 2
category: rawDataService
subCategory: rawDataInformation
title: REST API
subTitle: Raw Data Service
pageTitle: Raw Data Information
permalink: /restapi/rawdataservice/rawdatainformation/
---

## {{ page.pageTitle }}

### Endpoints

You can can fetch information about raw data objects by using the following endpoints.

{% assign linkId="rawDataEndpointGetListMulti" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity" %}
{% assign summary="Returns a list of information entries for entities with type :entity" %}
{% capture description %}
Returns a list of raw data file information entries for all entities with type :entity. You further have to and restrict the request to the entities' uuids by adding the following uri parameter:

{% capture table %}
Parameter name | Description  <br> *Example* 
---------------|---------------------------
`uuids`        | Restricts the query to the entities identified by the given uuids. <br> {{site.images['warning']}} Entites of type 'Value' are identified by a compound key, which consists of the uuid of the measurement, '&#124;' and the characteristics uuid <br><br> *uuids=(652ae7a0-d1e1-4ee2-b3a5-d4526f6ba822&#124; 78bd15c6-dc70-4ab4-bd3c-8ab2b5780b52)*
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{% endcapture %}

{% assign exampleCaption="Get the information entries for several parts" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part?uuids=(05040c4c-f0af-46b8-810e-30c0c00a379e,5441c003-b6db-4217-ac6a-45cdbb805bb3) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
 ...
   "data":
   [
    {
         "target":
         {
             "entity": "Part",
             "uuid": " 05040c4c-f0af-46b8-810e-30c0c00a379e"
         },
         "key": 0,
         "fileName": "section view.meshModel",
         "mimeType": "application/x-zip-compressed",
         "lastModified": "2012-11-19T10:48:34.327Z",
         "created": "2012-11-19T10:48:34.327Z",
         "size": 147376,
         "md5": "02f9c86143ea176c06e24524385b5907"
     },
     {
         "target":
         {
             "entity": "Part",
             "uuid": "5441c003-b6db-4217-ac6a-45cdbb805bb3"
         },
         "key": 0,
         "fileName": "cad_22.meshModel",
         "mimeType": "application/x-zeiss-piweb-meshmodel",
         "lastModified": "2015-03-20T14:37:02.943Z",
         "created": "2015-03-20T14:37:02.943Z",
         "size": 837245,
         "md5": "cbde88e2ed754c70860b3e6d4313551a"
     }
    ]
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="rawDataEndpointGetListSingle" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid" %}
{% assign summary="Returns a list of information entries for the entity with type :entity and guid :uuid " %}
{% assign description="" %}
{% assign exampleCaption="Get the information entries for a certain part" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part/05040c4c-f0af-46b8-810e-30c0c00a379e HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
 {% highlight json %}
 ...
   "data":
   [
    {
         "target":
         {
             "entity": "Part",
             "uuid": " 05040c4c-f0af-46b8-810e-30c0c00a379e"
         },
         "key": 0,
         "fileName": "section view.meshModel",
         "mimeType": "application/x-zip-compressed",
         "lastModified": "2012-11-19T10:48:34.327Z",
         "created": "2012-11-19T10:48:34.327Z",
         "size": 147376,
         "md5": "02f9c86143ea176c06e24524385b5907"
     }
    ]
{% endhighlight %}    
{% endcapture %}

{% include endpointTab.html %}

### General Information

The returned objects contains the following properties:

{% capture table %}
Property                             | Description
-----------------|-------------------|------------------------
<nobr>target `TargetEntity`</nobr>   | Specifies a concrete entity for a raw data object and consits of the entity's type (Part, Characteristic, Measurement, Value) and guid. If raw data is attached to an entity of type `Value`, the `uuid` contains a compound key in the following format: `{MeasurementUuid}|{CharacteristicUuid}`
<nobr>key `int`</nobr>               | This is a unique key that identifies this specific raw data object for a corresponding entity. An entity can have multiple raw data object that are distinct by this key.
<nobr>fileName `string`</nobr>       | The filename of the raw data object. Please note that this filename is not unique (unlike filenames in traditional file systems).
<nobr>mimeType `string`</nobr>       | The file's mime type. 
<nobr>lastModified `DateTime`</nobr> | The timestamp of the last modification of the corresponding raw data object
<nobr>created `DateTime`</nobr>      | The timestamp of the creation of the corresponding raw data object
<nobr>size `int`</nobr>              | The size of the raw data object in bytes
<nobr>md5 `string`</nobr>            | The MD5-Hash of the raw data object
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
