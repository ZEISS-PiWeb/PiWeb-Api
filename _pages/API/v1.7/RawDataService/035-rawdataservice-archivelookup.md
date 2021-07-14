### Archive lookup

The RawDataService offers special endpoints to work with archives, including the possibility to list contents of an archive which is saved as raw data on any target entity,
or retrieving only specified files of an archive instead of requesting the whole archive. This can save time and data if you only need a small portion of an archive.

>{{ site.images['info'] }} Archive lookup currently works with archives of filetype `.zip`

{% assign linkId="rawDataEndpointListArchiveEntries" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid/:key/ArchiveEntries" %}
{% assign summary="Returns information about the archive and a list of its contents " %}
{% assign exampleCaption="Get information and a list of entries of an archive which is raw data with key 1 of the part with uuid db42f331-1a22-491b-b20f-1dc52a40cec7" %}
{% capture description %}

You can fetch information about the archive and included files. In this example the ExampleArchive.zip contains three files as listed in "entries":
_information.txt_, _metal_part.meshModel_ and _thumbnail.jpg_.

{% endcapture %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part/db42f331-1a22-491b-b20f-1dc52a40cec7/1/ArchiveEntries HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

 {% highlight json %}

 [
     {
         "archiveInfo": {
             "target": {
                 "entity": "Part",
                 "uuid": "db42f331-1a22-491b-b20f-1dc52a40cec7"
             },
             "key": 1,
             "fileName": "ExampleArchive.zip",
             "mimeType": "application/x-zip-compressed",
             "lastModified": "2020-12-17T10:35:49.017Z",
             "created": "2020-12-17T10:35:49.017Z",
             "size": 2206767,
             "md5": "9c5574b3-88f6-4e39-c4cd-4319db285c9c"
         },
         "entries": [
             "information.txt",
             "metal_part.meshModel",
             "thumbnail.jpg"
         ]
     }
 ]
{% endhighlight %}    

#### Object Structure

The returned object contains the following properties:

{% capture table %}
Property                             | Description
-----------------|-------------------|------------------------
<nobr><code>RawDataInformation</code> archiveInfo</nobr>    | Information about the raw data which is an archive, the same properties as [Raw Data Information](#rs-raw-data-information).
<nobr><code>string[]</code> entries</nobr>                  | A list of all files inside the archive, including subfolders.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### Subfolders
As archives can contain subfolders, the structure is present in the filename.

 {% highlight json %}
 [
     {
         "archiveInfo": { . . . },
         "entries": [
             "metal_part.meshModel",
             "Subfolder/",
             "Subfolder/information.txt",
             "thumbnail.jpg"
         ]
     }
 ]   
{% endhighlight %}

Since folders are also entries from the point of view of the archive, they are part of the response like *Subfolder/* in above example.
Please note that these entries do not contain data, so requesting the entry *Subfolder/* will result in HTTP status code `200 - OK`, but without data in the response body.
You will not receive the folders content. For this you need to explicitly fetch the files located in the subfolder with their full name, including folder structure, as shown in the next endpoint.
{% endcapture %}
{% include endpointTab.html %}

{% assign linkId="rawDataEndpointGetArchiveContent" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid/:key/ArchiveContent/:filename" %}
{% assign summary="Fetch the file identified by :filename from the archive" %}
{% assign exampleCaption="Fetch the file 'information.txt' of an archive with raw data key 1 of the part with uuid db42f331-1a22-491b-b20f-1dc52a40cec7" %}
{% capture description %}

Once you know the contents of specified archive you can request single files with a GET request similiar to fetching normal raw data.
You specify the raw data archive with *:entity*, *:uuid* and *:key*, and the requested file with its *:filename*.
This endpoint offers the same caching mechanisms as the normal request for raw data.

>{{ site.images['info'] }} The filename is *case insensitive*. Archive lookup will return the first occurence of specified file if two files with the same name exist in the same folder.

{% capture table %}
Parameter name | Description  <br> *Example*
---------------|---------------------------
`filename`     | The filename of a file which is part of specified archive, e.g. *information.txt*. The filename includes folder structure if the file is located in some subfolder, e.g. *Subfolder/information.txt*.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{% endcapture %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part/db42f331-1a22-491b-b20f-1dc52a40cec7/1/ArchiveContent/information.txt HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
The requested file
{% endhighlight %}    

{% endcapture %}
{% include endpointTab.html %}

{% assign linkId="rawDataEndpointArchiveEntryQuery" %}
{% assign method="POST" %}
{% assign endpoint="/rawData/ArchiveEntryQuery" %}
{% assign summary="Fetch information about multiple archives" %}
{% assign exampleCaption="Fetch information and file lists of two raw data archives" %}
{% capture description %}

You can request information and file lists for more than one archive at a time using this query endpoint.
The property `selectors` can contain any number of raw data targets and the key of the raw data archive.

{% endcapture %}

{% capture jsonrequest %}
{% highlight http %}
POST /rawDataServiceRest/rawData/ArchiveEntryQuery HTTP/1.1
{% endhighlight %}
{% highlight json %}
{
    "selectors": [
        {
            "target": {
                "entity": "Part",
                "uuid": "db42f331-1a22-491b-b20f-1dc52a40cec7"
            },
            "key": 1
        },
        {
            "target": {
                "entity": "Characteristic",
                "uuid": "b8f5d3fe-5bd5-406b-8053-67f647f09dc7"
            },
            "key": 4
        },
    ]
}
{% endhighlight %}  
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}
[
    {
        "archiveInfo": {
            "target": {
                "entity": "Part",
                "uuid": "db42f331-1a22-491b-b20f-1dc52a40cec7"
            },
            "key": 1,
            "fileName": "ExampleArchive.zip",
            "mimeType": "application/x-zip-compressed",
            "lastModified": "2020-12-17T10:35:49.017Z",
            "created": "2020-12-17T10:35:49.017Z",
            "size": 2206767,
            "md5": "9c5574b3-88f6-4e39-c4cd-4319db285c9c"
        },
        "entries": [
            "information.txt",
            "metal_part.meshModel",
            "thumbnail.jpg"
        ]
    },
    {
        "archiveInfo": {
            "target": {
                "entity": "Characteristic",
                "uuid": "b8f5d3fe-5bd5-406b-8053-67f647f09dc7"
            },
            "key": 4,
            "fileName": "ExampleArchive_Subfolder.zip",
            "mimeType": "application/x-zip-compressed",
            "lastModified": "2020-12-17T14:17:53.587Z",
            "created": "2020-12-17T14:17:53.587Z",
            "size": 2206919,
            "md5": "8e51ba7a-b63d-5066-e2a0-c03abc73261a"
        },
        "entries": [
            "metal_part.meshModel",
            "Subfolder/",
            "Subfolder/information.txt",
            "thumbnail.jpg"
        ]
    }
]
{% endhighlight %}    

{% endcapture %}
{% include endpointTab.html %}

{% assign linkId="rawDataEndpointArchiveContentQuery" %}
{% assign method="POST" %}
{% assign endpoint="/rawData/ArchiveContentQuery" %}
{% assign summary="Fetch multiple files of raw data archives" %}
{% assign exampleCaption="Fetch the files *information.txt* and *thumbnail.jpg* in one request" %}
{% capture description %}

You can fetch multiple files of the same or different archives using the `ArchiveContentQuery` endpoint.
The property `selectors` contains the same information as the `ArchiveEntryQuery` above, with an additional list of requested files per raw data archive.

{% endcapture %}

{% capture jsonrequest %}
{% highlight http %}
POST /rawDataServiceRest/rawData/ArchiveContentQuery HTTP/1.1
{% endhighlight %}
{% highlight json %}
{
    "selectors": [
        {
            "target": {
                "entity": "Part",
                "uuid": "db42f331-1a22-491b-b20f-1dc52a40cec7"
            },
            "key": 1,
            "entries": [
                "information.txt",
                "thumbnail.jpg"
            ]
        }
    ]
}
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

The requested files in [BSON](http://bsonspec.org/n) format.

<img src="/PiWeb-Api/images/archive_bson.png" class="img-responsive center-block">

The BSON contains one entry for each requested file, with no separation between files of different raw data archives. Each entry contains
the property `archiveInfo`, which again has the same structure as [Raw Data Information](#rs-raw-data-information). The entry can be identified by its
`fileName` and is completed by the actual `data`, the `size` and an `md5` checksum.

You can add more targets to `selectors` to fetch files from different archives:

{% highlight json %}
{
    "selectors": [
        {
            "target": { . . . },
            "key": 1,               //Archive 1
            "entries": [ . . . ]
        },
        {
            "target": { . . . },
            "key": 4,               //Archive 2
            "entries": [ . . . ]
        },
        {
            "target": { . . . },
            "key": 325,             //Archive 3
            "entries": [ . . . ]
        }
    ]
}
{% endhighlight %}
{% endcapture %}
{% include endpointTab.html %}

#### Exceptions

Archive lookup only works with supported archives, currently the .zip format. If you try to list or fetch contents of raw data in any other format you will receive HTTP status code `400 - BadRequest`
and the following response:

{% highlight json %}
{
    "Message": "Specified RawData is no archive of supported format!",
    "ExceptionType": "System.InvalidOperationException",
    "StackTrace": " . . . "
}
{% endhighlight %}
