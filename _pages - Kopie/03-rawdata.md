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

You can fetch, add, update and delete raw data files and the raw data file information using the following endpoints. Applicable filters are described in the [URL Parameters section](General-Information#raw-data).

URL Endpoint | GET | PUT | POST | DELETE
-------------|-----|-----|------|-------
rawData/*entity*/ *uuid*/*key* | Returns the one specific file identified by *entity*, *uuid* and *key*| Replaces the file identified by *entity*, *uuid* and *key* with the one in the request body| Adds the transmitted file to the entity with type *entity* and id *uuid* | Deletes the file identified by *entity*, *uuid* and *key*. Deletes all files if no *key* is identified.
rawData/*entity*/ *uuid*/*key*/ thumbnail | Returns a preview image for the file identified by *entity*, *uuid* and *key* | *Not supported* | *Not supported* | *Not supported*
/rawData/*entity* | Returns a list of raw data file information entries for all entities with type *entity* and a matching id in the *uuids* parameter | *Not supported* | *Not supported* | *Not supported*

##{{page.sections['add']}}

You can attach files to all entity types: parts, characteristics, measurements and measured values.

An add request consists of 3 mandatory parts:
1. The *URL* specifies which entity the file will be added to.
2. The *request body* contains the file itself.
3. The *HTTP headers* must provide meta information about the file, see below for details.

HTTP header variable | Description                                    | Example Value
---------------------|------------------------------------------------|--------------------------------------
Content-Disposition  | Includes the file name       | "MetalPart.meshModel"
Content-Length       | Includes the length in bytes | 2090682
Content-MD5          | Includes file's MD5 hash sum        | "bdf6b06ab301a80ae55021085b820393"
Content-Type         | Includes file's MIME type           | "application/x-zeiss-piweb-meshmodel"

When adding a file, you can pass the desired file key as part of the uri. If you pass -1 or no key, the next available key will automatically assigned by the server. (recommended)

{{site.images['warning']}} If you pass a key which is already assigned to another file, this file will be replaced.

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

You can restrict the request by adding url parameters. For more details see the [URL-Parameter section](General-Information#raw-data).

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

The server caches raw data fetch requests. When you request a raw data file for the first time the response will contain the file itself and several HTTP headers. One of these headers is the *ETag* header. An ETag is a unique hash value to identify the file. It is a combination of the file's MD5 checksum and the last modification date. If you send an ETag in the *If-None-Match* header, the server can respond two different ways, depending in whether the file has been modified since the last request:
1. *Not modified*: The server will return a *304 - Not modified* HTTP status code and the response body will be emtpy.
2. *Modified*: The server will return the file.

If you use the PiWeb .NET SDK, caching is already built in.

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

Updating a file works just like adding a file. The only difference is that you must specify the key for the file you want to update.

##{{page.sections['delete']}}

If you provide a key in the URI, the server will only delete the file identified by the key. Otherwise the server will delete all files attached to the entity.

### Delete the raw data object with the key 0 from a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

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

### Delete all raw data objects from the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7

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
