---
area: rawDataService
level: 0
version: 1.0
title: Rawdata Service
permalink: /rawdataservice/
sections:
  rawdataservice:
    title: Raw Data Service
    iconName: rawdataservice16
    anchor: rs
    secs:
      serviceInformation:
        title: Service Information
        anchor: rs-service-information
      rawDataInformation:
        title: Raw Data Information
        anchor: rs-raw-data-information
      rawDataObjects:
        title: Raw Data Objects
        anchor: rs-raw-data-objects
---

{% include version_combobox.html %}

<h2 id="{{page.sections['rawdataservice']['secs']['serviceInformation'].anchor}}">{{page.sections['rawdataservice']['secs']['serviceInformation'].title}}</h2>

### Endpoints

The service information can be fetched via the following endpoint. This endpoint doesn't provide filter parameters.

{% assign linkId="rawDataEndpointGetServiceInformation" %}
{% assign method="GET" %}
{% assign endpoint="/serviceInformation" %}
{% assign summary="Returns general information about the raw data service" %}
{% assign description="" %}
{% assign exampleCaption="Get service information for a given connection" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawdataServiceRest/serviceInformation HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

 {
  "versionWsdlMajor": "2",
  "versionWsdlMinor": "3",
  "version": "5.8.0.0"
 }

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


<h2 id="{{page.sections['rawdataservice']['secs']['rawDataInformation'].anchor}}">{{page.sections['rawdataservice']['secs']['rawDataInformation'].title}}</h2>


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
`uuids`        | Restricts the query to the entities identified by the given uuids. <br> {{site.images['warning']}} Entites of type 'Value' are identified by a compound key, which consists of the uuid of the measurement, '&#124;' and the characteristics uuid <br><br> *uuids={652ae7a0-d1e1-4ee2-b3a5-d4526f6ba822&#124;78bd15c6-dc70-4ab4-bd3c-8ab2b5780b52}*
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{% endcapture %}

{% assign exampleCaption="Get the information entries for several parts" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part?uuids={05040c4c-f0af-46b8-810e-30c0c00a379e,5441c003-b6db-4217-ac6a-45cdbb805bb3} HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

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

### Object Structure

The returned objects contains the following properties:

{% capture table %}
Property                             | Description
-----------------|-------------------|------------------------
<nobr><code>TargetEntity</code> target</nobr>   | Specifies a concrete entity for a raw data object and consits of the entity's type (Part, Characteristic, Measurement, Value) and guid. If raw data is attached to an entity of type `Value`, the `uuid` contains a compound key in the following format: `{MeasurementUuid}|{CharacteristicUuid}`
<nobr><code>int</code> key</nobr>               | This is a unique key that identifies this specific raw data object for a corresponding entity. An entity can have multiple raw data object that are distinct by this key.
<nobr><code>string</code> fileName</nobr>       | The filename of the raw data object. Please note that this filename is not unique (unlike filenames in traditional file systems).
<nobr><code>string</code> mimeType</nobr>       | The file's mime type. 
<nobr><code>DateTime</code> lastModified</nobr> | The timestamp of the last modification of the corresponding raw data object
<nobr><code>DateTime</code> created</nobr>      | The timestamp of the creation of the corresponding raw data object
<nobr><code>int</code> size</nobr>              | The size of the raw data object in bytes
<nobr><code>string</code> md5</nobr>            | The MD5-Hash of the raw data object
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


<h2 id="{{page.sections['rawdataservice']['secs']['rawDataObjects'].anchor}}">{{page.sections['rawdataservice']['secs']['rawDataObjects'].title}}</h2>

### Endpoints

You can can fetch, create, update and delete raw data objects by using the following endpoints.

{% assign linkId="rawDataEndpointGetFile" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid/:key" %}
{% assign summary="Returns the one specific file identified by :entity, :uuid and :key" %}
{% capture description %}

The server caches raw data fetch requests. When you request a raw data file for the first time the response will contain the file itself and several HTTP headers. One of these headers is the *ETag* header. An ETag is a unique hash value to identify the file. It is a combination of the file's MD5 checksum and the last modification date. If you send the ETag value in the *If-None-Match* header, the server can respond two different ways, depending in whether the file has been modified since the last request:

1. *Not modified*: The server will return a *304 - Not modified* HTTP status code and the response body will be emtpy.
2. *Modified*: The server will return the file.

{% endcapture %}
{% assign exampleCaption="Fetch raw data with key 0 for a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}

{% capture jsonrequest %}
###### Without Caching
{% highlight http %}
GET /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/0 HTTP/1.1
{% endhighlight %}
###### With caching
{% highlight http %}
GET /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/0 HTTP/1.1
If-None-Match: "6ab0f6bd01b30aa8e55021085b820393635437006830400000"
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
###### Modified
{% highlight http %}
HTTP/1.1 200 OK
Etag: "6ab0f6bd01b30aa8e55021085b820393635437006830400000"
Last-Modified: Fri, 15 Aug 2014 11:58:03 GMT
...
{% endhighlight %}
<pre>
The requested raw data file
</pre>

###### Not modified
{% highlight http %}
HTTP/1.1 304 Not modified
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="rawDataEndpointGetThumbnail" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid/:key/thumbnail" %}
{% assign summary="Returns a preview image for the file identified by :entity, :uuid and :key. " %}
{% assign description="" %}
{% assign exampleCaption="Fetch the thumbnail for the raw data object with key 1 which is attached to the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/1/thumbnail HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
<pre>
The requested thumbnail
</pre>
{% endcapture %}

{% include endpointTab.html %}




{% assign linkId="rawDataEndpointAddFile" %}
{% assign method="POST" %}
{% assign endpoint="/rawData/:entity/:uuid/:key" %}
{% assign summary="Adds the transmitted file to the entity with type :entity and id :uuid" %}
{% capture description %}

You can attach files to all entity types: parts, characteristics, measurements and measured values.

An add request consists of 3 mandatory parts:

1. The *URL* specifies which entity the file will be added to.
2. The *request body* contains the file itself.
3. The *HTTP headers* must provide meta information about the file, see below for details.

{% capture table %}
HTTP header variable | Description                  | Example Value
---------------------|------------------------------|--------------------------------------
Content-Disposition  | Includes the file name       | "MetalPart.meshModel"
Content-Length       | Includes the length in bytes | 2090682
Content-MD5          | Includes file's MD5 hash sum | "bdf6b06ab301a<wbr>80ae55021085b820393"
Content-Type         | Includes file's MIME type    | "application/x-zeiss-piweb-meshmodel"
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

When adding a file, you can pass the desired file key as part of the uri. If you pass -1 or no key, the next available key will automatically assigned by the server. (recommended)

{{site.images['warning']}} If you pass a key which is already assigned to another file, this file will be replaced.
{% endcapture %}

{% assign exampleCaption="Add a raw data object to a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}
{% capture jsonrequest %}
{% highlight http %}
POST /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7 HTTP/1.1
Content-Disposition: "MetalPart.meshModel"
Content-Length: 2090682
Content-MD5: "bdf6b06ab301a80ae55021085b820393"
Content-Type: "application/x-zeiss-piweb-meshmodel"
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}



{% assign linkId="rawDataEndpointUpdateFile" %}
{% assign method="PUT" %}
{% assign endpoint="/rawData/:entity/:uuid/:key" %}
{% assign summary="Replaces the file identified by :entity, :uuid and :key with the transmitted one" %}
{% capture description %}

An update request consists of 3 mandatory parts:

1. The *URL* specifies the file to be replaced identified by :entity, :uuid and :key.
2. The *request body* contains the file itself.
3. The *HTTP headers* must provide meta information about the file, see below for details.

{% capture table %}
HTTP header variable | Description                  | Example Value
---------------------|------------------------------|--------------------------------------
Content-Disposition  | Includes the file name       | "MetalPart.meshModel"
Content-Length       | Includes the length in bytes | 2090682
Content-MD5          | Includes file's MD5 hash sum | "bdf6b06ab301a<wbr>80ae55021085b820393"
Content-Type         | Includes file's MIME type    | "application/x-zeiss-piweb-meshmodel"
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{% endcapture %}

{% assign exampleCaption="Replace the raw data object with key 1 of the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/1 HTTP/1.1
Content-Disposition: "MetalPart.meshModel"
Content-Length: 2090682
Content-MD5: "bdf6b06ab301a80ae55021085b820393"
Content-Type: "application/x-zeiss-piweb-meshmodel"
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="rawDataEndpointDeleteFiles" %}
{% assign method="DELETE" %}
{% assign endpoint="/rawData/:entity/:uuid" %}
{% assign summary="Deletes all files for the entity identified by :entity and :uuid. " %}

{% assign description="" %}

{% assign exampleCaption="Delete all raw data objects from the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}



{% assign linkId="rawDataEndpointDeleteFile" %}
{% assign method="DELETE" %}
{% assign endpoint="/rawData/:entity/:uuid/:key" %}
{% assign summary="Deletes the file identified by :entity, :uuid and :key. " %}
{% assign description="" %}

{% assign exampleCaption="Delete the raw data object with key 1 from the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/0 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}
