---
category: rawdataservice
title: Raw Data Service
permalink: /rawdataservice/
sections:
   endpoint: Endpoint information
   add: Add raw data
   fetchInfo: Fetch raw data information
   fetchData: Fetch raw data
   update: Update raw data
   delete: Delete raw data
---

##{{page.sections['endpoint']}}

Raw data as well as information about the raw data can be fetched, added, updated and deleted via the following endpoints. Filters can be set as described in the [URL-Parameter section](General-Information#raw-data).

URL Endpoint | GET | PUT | POST | DELETE
-------------|-----|-----|------|-------
rawData/*entity*/ *uuid*/*key* | Returns the rawData file for the specified *entity* with the committed *uuid* and *key* | Updates the committed rawData file for the *entity* with the *uuid* and *key* | Creates the committed rawData file(s) for the *entity* with the *uuid* | Deletes the rawData file for the specified *entity* with the comitted *uuid* and *key* or all raw data if no *key* is given
rawData/*entity*/ *uuid*/*key*/ thumbnail | Returns a preview image of the raw data object for the specified *entity* with the committed *uuid* and *key* | *Not supported* | *Not supported* | *Not supported*
/rawData/*entity* | Returns a list of raw data information for the *entity* with the uuids commited within the url parameter section | *Not supported* | *Not supported* | *Not supported*

##{{page.sections['add']}}

Raw data can be added to all kinds of entities: part, characteristic, measurement and measured values.
When adding raw data, further information needs to be passed beneath the data itself. The information where the raw data object belongs to is passed within the uri, the data itself is passed within the HTTP body and the additional information is passed within several HTTP header variables:

HTTP header variable | Description                                    | Example Value
---------------------|------------------------------------------------|--------------------------------------
Content-Disposition  | Includes the raw data object's file name       | "MetalPart.meshModel"
Content-Length       | Includes the raw data object's length in bytes | 2090682
Content-MD5          | Includes raw data object's MD5 hash sum        | "bdf6b06ab301a80ae55021085b820393"
Content-Type         | Includes raw data object's MIME type           | "application/x-zeiss-piweb-meshmodel"

When adding raw data, it is possible to pass a key within the uri. If -1 or no key is passed, the next free key will be used on server side. (recommended)

{{site.images['warning']}} If a key is passed which is already in use, the existing raw data object will be replaced.

### Add a raw data object to a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

####Example of a direct webservice call

Request:

{% highlight http %}
PUT /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7 HTTP/1.1
Content-Disposition: "MetalPart.meshModel"
Content-Length: 2090682
Content-MD5: "bdf6b06ab301a80ae55021085b820393"
Content-Type: "application/x-zeiss-piweb-meshmodel"
{% endhighlight %}

Response:
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

####Example of a webservice call via API.dll

Request:

{% highlight csharp %}
var client = new RawDataServiceRestClient( serviceUri );
//Create RawDataInformation object which contains information about the data to be uploaded
var rawDataInformation = new RawDataInformation(RawDataTargetEntity.CreateForPart(new Guid("b8f5d3fe-5bd5-406b-8053-67f647f09dc7"), -1);
rawDataInformation.MD5 = new Guid("bdf6b06ab301a80ae55021085b820393");
rawDataInformation.MimeType = "application/x-zeiss-piweb-meshmodel";
rawDataInformation.FileName = "MetalPart.meshModel";
rawDataInformation.Size = meshModelInBytes.Length;
client.CreateRawData(meshModelInBytes, rawDataInformation);
{% endhighlight %}

##{{page.sections['fetchInfo']}}

The request can be restricted by adding url parameters. For more details see the [URL-Parameter section](General-Information#raw-data).

### Fetch raw data information for the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

#### Example of a direct webservice call

Request:

{% highlight http %}
GET /rawDataServiceRest/rawData/part?uuids={b8f5d3fe-5bd5-406b-8053-67f647f09dc7} HTTP/1.1
{% endhighlight %}

Response:
{% highlight json linenos %}
{
   ...
   "data":
   [
       {
           "target":
           {
               "entity": "Part",
               "uuid": "5129295e-2605-4051-a8d5-7db57fabcab3"
           },
           "key": 0,
           "fileName": "MetalPart.meshModel",
           "description": "PiWeb Mesh Model",
           "mimeType": "application/x-zeiss-piweb-meshmodel",
           "lastModified": "2014-08-15T11:58:03.04Z",
           "created": "2014-08-15T11:58:03.04Z",
           "size": 2090682,
           "md5": "bdf6b06ab301a80ae55021085b820393"
       },
       ...
   ]
}
{% endhighlight %}

####Example of a webservice call via API.dll

{% highlight csharp %}
var client = new RawDataServiceRestClient( serviceUri );
var rawDataInfo = await client.ListRawDataForParts( new Guid[] { b8f5d3fe-5bd5-406b-8053-67f647f09dc7 } );
{% endhighlight %}

##{{page.sections['fetchData']}}

The server caches raw data fetches. When a raw data object is requested for the first time, several HTTP header values are returned beneath the raw data object. This header values include the *ETag* header. It consists of a distinct hash value which identifies the raw data object unambiguously. This hash value is a combination of the MD5 sum and the last modified date. If this *ETag* value is sent within the *If-None-Match* header, the server returns a *304 - Not modified* HTTP header status code on the next request, instead of the raw data object, if the object has not been changed since the last request. If the API.dll is used, the caching is already implemented.

### Fetch raw data with key 0 for a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

#### Example of a direct webservice call

Request:

{% highlight http %}
GET /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/0 HTTP/1.1
{% endhighlight %}

Response: 

The requested raw data file

{% highlight http %}
HTTP/1.1 200 OK
Etag: "6ab0f6bd01b30aa8e55021085b820393635437006830400000"
Last-Modified: Fri, 15 Aug 2014 11:58:03 GMT
...
{% endhighlight %}

Request with If-None-Match header

{% highlight http %}
GET /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/0 HTTP/1.1
If-None-Match: "6ab0f6bd01b30aa8e55021085b820393635437006830400000"
{% endhighlight %}

Response:

{% highlight http %}
HTTP/1.1 304 Not modified
{% endhighlight %}

#### Example of a webservice call via API.dll

{% highlight csharp %}
var client = new RawDataServiceRestClient( serviceUri );
var rawData = await client.GetRawDataForPart( "b8f5d3fe-5bd5-406b-8053-67f647f09dc7", 0 );
{% endhighlight %}

##{{page.sections['update']}}

Updating a raw data object works almost identically like adding raw data objects. The only difference is the key by which the raw data object is identified and which is mandatory.

##{{page.sections['delete']}}

If a key is given within the uri, only the raw data object with the given key will be deleted. Otherwise all raw data objects which belong to the entity will be deleted.

### Delete the raw data object with the key 0 for a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

#### Example of a direct webservice call

Request

{% highlight http %}
DELETE /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/0 HTTP/1.1
{% endhighlight %}

Response

{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

####Example of a webservice call via API.dll

Request:

{% highlight csharp %}
var client = new RawDataServiceRestClient( serviceUri );
client.DeleteRawDataForPart(new Guid("b8f5d3fe-5bd5-406b-8053-67f647f09dc7"),0);
{% endhighlight %}

### Delete all raw data objects for the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

#### Example of a direct webservice call

Request

{% highlight http %}
DELETE /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7  HTTP/1.1
{% endhighlight %}

Response

{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

####Example of a webservice call via API.dll

Request:

{% highlight csharp %}
var client = new RawDataServiceRestClient( serviceUri );
client.DeleteAllRawDataForPart( new Guid("b8f5d3fe-5bd5- 406b-8053-67f647f09dc7") );
{% endhighlight %}
