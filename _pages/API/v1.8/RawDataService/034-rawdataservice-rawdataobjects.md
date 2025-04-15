<h2 id="{{page.sections['rawdataservice']['secs']['rawDataObjects'].anchor}}">{{page.sections['rawdataservice']['secs']['rawDataObjects'].title}}</h2>

### Endpoints

You can can fetch, create, update and delete raw data objects by using the following endpoints.

{% assign linkId="rawDataEndpointGetFile" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid/:key" %}
{% assign summary="Returns the one specific file identified by :entity, :uuid and :key" %}
{% capture description %}

The server caches raw data fetch requests. When you request a raw data file for the first time the response will contain the file itself and several HTTP headers. One of these headers is the *ETag* header. An ETag is a unique hash value to identify the file. It is a combination of the file's MD5 checksum and the last modification date. If you send the ETag value in the *If-None-Match* header, the server can respond two different ways, depending on whether the file has been modified since the last request:

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
GET /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/1/thumbnail HTTP/1.1
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
HTTP header variable | Description                                                | Example Value
---------------------|------------------------------------------------------------|--------------------------------------
Content-Disposition  | Includes the file name                                     | "MetalPart.meshModel"
Content-Length       | Includes the length in bytes                               | 2090682
Content-Type         | Includes file's MIME type                                  | "application/x-zeiss-piweb-meshmodel"
Content-MD5          | (*optional*) Includes file's MD5 hash sum in Base64 format | "vfawarMBqArlUCEIW4IDkw=="
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

>{{site.images['info']}} The allowed raw data file names (see `Content-Disposition`) can be restricted. See the option `Restrict filenames by a whitelist` in the advanced options of the PiWeb Server.

>{{site.images['info']}} When adding a file, you can pass the desired file key as part of the uri. If you pass -1 or no key, the next available key will automatically be assigned by the server (recommended) and can be read from the result.

>{{site.images['warning']}} If you pass a key which is already assigned to another file, this file will be replaced.

>{{site.images['info']}} Remember, when adding a file for a measured value, you need to specify the target uuid in a compound format of *measurementUuid&#124;characteristicUuid*, instead of one single uuid.

{% endcapture %}

{% assign exampleCaption="Add a raw data object to a part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}
{% capture jsonrequest %}
{% highlight http %}
POST /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7 HTTP/1.1
Content-Disposition: "MetalPart.meshModel"
Content-Length: 2090682
Content-MD5: "vfawarMBqArlUCEIW4IDkw=="
Content-Type: "application/x-zeiss-piweb-meshmodel"
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight json %}

{
    "target":
    {
        "entity": "Part",
        "uuid": "b8f5d3fe-5bd5-406b-8053-67f647f09dc7"
    },
    "key": 0,
    "fileName": "MetalPart.meshModel",
    "mimeType": "application/x-zip-compressed",
    "lastModified": "2012-11-19T10:48:34.327Z",
    "created": "2012-11-19T10:48:34.327Z",
    "size": 2090682,
    "md5": "6ab0f6bd-01b3-0aa8-e550-21085b820393"
}
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
HTTP header variable | Description                                                | Example Value
---------------------|------------------------------------------------------------|--------------------------------------
Content-Disposition  | Includes the file name                                     | "MetalPart.meshModel"
Content-Length       | Includes the length in bytes                               | 2090682
Content-Type         | Includes file's MIME type                                  | "application/x-zeiss-piweb-meshmodel"
Content-MD5          | (*optional*) Includes file's MD5 hash sum in Base64 format | "vfawarMBqArlUCEIW4IDkw=="

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

>{{site.images['info']}} The allowed raw data file names (see `Content-Disposition`) can be restricted on the server.

{% endcapture %}

{% assign exampleCaption="Replace the raw data object with key 1 of the part with the uuid b8f5d3fe-5bd5-406b-8053-67f647f09dc7" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /rawDataServiceRest/rawData/part/b8f5d3fe-5bd5-406b-8053-67f647f09dc7/1 HTTP/1.1
Content-Disposition: "MetalPart.meshModel"
Content-Length: 2090682
Content-MD5: "vfawarMBqArlUCEIW4IDkw=="
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

<h4>Encoding filenames</h4>

Per definition, HTTP headers are ASCII-only, making special characters in the filename sent via Content-Disposition header slightly more complex. When using our .NET SDK NuGet, the framework does all encoding when it encounters non-ASCII-characters. For this, it is using the [Encoded-Word](https://en.wikipedia.org/wiki/MIME#Encoded-Word) syntax. An example header value would look like this:

`attachment; filename="=?utf-8?B?ZsO2w7Ziw6RyLnR4dA==?="`

Where `utf-8` ist the charset, `B` is the encoding (Base64), and the following `ZsO2w7Ziw6RyLnR4dA==` is the actual filename in Base64 encoding, e.g. `fööbär.txt` in this example. All separated by `?`, 
with a leading `=?` and trailing `?=`. Other charsets and encodings are possible, you can find more information about this in the documentation of used Encoded-Word syntax. When using the RawDataService directly via REST, you may need to implement this yourself.