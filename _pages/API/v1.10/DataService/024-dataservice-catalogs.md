<h2 id="{{page.sections['dataservice']['secs']['catalogs'].anchor}}">{{page.sections['dataservice']['secs']['catalogs'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['catalogs'].anchor}}-endpoints">Endpoints</h3>

Catalogs and catalog entries can be fetched, created, updated and deleted using the following endpoints.

{% assign linkId="catalogEndpointGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Fetches all catalogs including its entries" %}
{% assign description="" %}
{% assign exampleCaption="Fetching all catalogs" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/catalogs HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
[
   {
      "uuid": "d7291afb-0a67-4c1e-8bcc-6fc455bcc0e5",
      "name": "direction catalog",
      "validAttributes": [ 2009 ],
      "catalogEntries":
      [
         {
            "key": 0,
            "attributes": { "2009": "undefined" }
         },
         {
            "key": 1,
            "attributes": { "2009": "right" }
         },
         {
            "key": 2,
            "attributes": { "2009": "left" }
         }
      ]
   },
   {
      "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
      "name": "InspectorCatalog",
      "validAttributes": [ 4092, 4093 ],
      "catalogEntries":
      [
         {
            "key": 0,
            "attributes": { "4092": "n.def.", "4093": "n.def." }
         },
         {
            "key": 1,
            "attributes": { "4092": "21", "4093": "Smith" }
         },
         {
            "key": 2,
            "attributes": { "4092": "20", "4093": "Miller" }
         },
         {
            "key": 3,
            "attributes": { "4092": "23", "4093": "Williams" }
         }
      ]
   },
   ...
]
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="catalogEndpointGetSingle" %}
{% assign method="GET" %}
{% assign endpoint="/catalogs/:catalogUuid" %}
{% assign summary="Fetches the catalog specified by the :catalogUuid including its entries" %}
{% assign description="" %}
{% assign exampleCaption="Fetching the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
    {
        "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
        "name": "InspectorCatalog",
        "validAttributes": [ 4092, 4093 ],
        "catalogEntries":
        [
            {
                "key": 0,
                "attributes": { "4092": "n.def.", "4093": "n.def." }
            },
            {
                "key": 1,
                "attributes": { "4092": "21", "4093": "Smith" }
            },
            {
                "key": 2,
                "attributes": { "4092": "20", "4093": "Miller" }
            },
            {
                "key": 3,
                "attributes": { "4092": "23", "4093": "Williams" }
            }
         ]
     }
]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="catalogEndpointCreate" %}
{% assign method="POST" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Creates catalogs" %}
{% capture description %}
To create a new catalog, the catalog object must be transmitted in the request's body. A valid request contains a unique identifier, the catalog name and the valid attributes. Catalog entries are optional. All valid attributes must be added as catalog attributes beforehand (see <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>).

{{ site.images['info'] }} If no catalog entries are specified, an empty catalog entry with key '0' and attribute value(s) 'not defined' ( in case of alphanumeric attributes ) is created by default.

{{ site.images['info'] }} If catalog entries are specified setting -1 as catalog entry's key leads server to generate a new unique key for that entry.
{% endcapture %}
{% assign exampleCaption="Adding the catalog InspectorCatalog" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/catalogs HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "InspectorCatalog",
           "validAttributes": [ 4092, 4093 ],
           "catalogEntries":
           [
               {
                   "key": 0,
                   "attributes": { "4092": "n.def.", "4093": "n.def." }
               },
               {
                   "key": 1,
                   "attributes": { "4092": "21", "4093": "Smith" }
               },
               {
                   "key": 2,
                   "attributes": { "4092": "20", "4093": "Miller" }
               },
               {
                   "key": -1,  //Server will generate an unique key
                   "attributes": { "4092": "23", "4093": "Williams" }
               }
            ]
        }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="catalogEndpointCreateEntries" %}
{% assign method="POST" %}
{% assign endpoint="/catalogs/:catalogUuid" %}
{% assign summary=" Creates entries for the catalog specified by :catalogUuid" %}
{% assign description="To add entries to an existing catalog they need to be specified in the request body. Each new entry must consist of a unique key. Each entry attribute must be listed as a valid attribute in the catalog definition." %}

{% assign exampleCaption="Adding a catalog entry - add the inspector ‘Clarks’" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad HTTP/1.1
{% endhighlight %}

{% highlight json %}
 [
   {
       "key": 4,
       "attributes": { "4092": "22", "4093": "Clarks" }
   }
 ]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="catalogEndpointUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Updates catalogs" %}
{% capture description %}
Update a catalog when you want to:

* rename the catalog and/or
* update existing catalog entries and/or
* update catalog's valid attributes.

To update a catalog, the whole object, including the valid attributes, needs to be transmitted in the body of the HTTP request.

{% endcapture %}
{% assign exampleCaption="Rename the catalog from 'InspectorCatalog' to 'Inspectors' and change the inspector's name from 'Smith' to  'Clarks'" %}

{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/catalogs HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "Inspectors",
           "validAttributes": [ 4092, 4093 ],
           "catalogEntries":
           [
               {
                   "key": 0,
                   "attributes": { "4092": "n.def.", "4093": "n.def." }
               },
               {
                   "key": 1,
                   "attributes": { "4092": "21", "4093": "Clarks" }
               },
               {
                   "key": 2,
                   "attributes": { "4092": "20", "4093": "Miller" }
               },
               {
                   "key": 3,
                   "attributes": { "4092": "23", "4093": "Williams" }
               }
            ]
        }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="catalogEndpointDeleteCatalogs" %}
{% assign method="DELETE" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Deletes all catalogs" %}
{% assign description="" %}
{% assign exampleCaption="Delete all catalogs" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogs HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="catalogEndpointDeleteCatalog" %}
{% assign method="DELETE" %}
{% assign endpoint="/catalogs/:catalogUuid" %}
{% assign summary="Deletes the catalog specified by :catalogUuid" %}
{% assign description="" %}
{% assign exampleCaption="Delete the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="catalogEndpointDeleteAllEntries" %}
{% assign method="DELETE" %}
{% assign endpoint="/catalogs/:catalogUuid/" %}
{% assign summary="Deletes all entries for the catalog specified by :catalogUuid" %}
{% assign description="" %}

{% assign exampleCaption="Delete all entries from the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad/ HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="catalogEndpointDeleteEntries" %}
{% assign method="DELETE" %}
{% assign endpoint="/catalogs/:catalogUuid/{:catalogEntryKeys}" %}
{% assign summary="Deletes :catalogEntryKeys for the catalog with :catalogUuid" %}
{% assign description="" %}

{% assign exampleCaption="Delete the entries with key 1 and 3 from the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad/{1,3} HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


<h3 id="{{page.sections['dataservice']['secs']['catalogs'].anchor}}-objectstructure">Object Structure</h3>

Each catalog describes a list of entries. All entries have the same defined set of attributes, called *valid attributes*.
All valid attributes must be created as *catalog attributes* beforehand. `Catalog` and `CatalogEntry`have the following structures:

#### Catalog

{% capture table %}
Property                                   | Description
-------------------------------------------|----------------------------------------------
<nobr><code>Guid</code> uuid</nobr>                   | Identifies the catalog uniquely
<nobr><code>string</code> name</nobr>                 | The name of the catalog
<nobr><code>ushort[]</code> validAttributes</nobr>    | A list of attribute keys that are valid for this catalog
<nobr><code>CatalogEntry</code> catalogEntries</nobr> | A list of catalog entries
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### CatalogEntry

{% capture table %}
Property                              | Description
--------------------------------------|----------------------------------------------------
<nobr><code>short</code> key</nobr>              | Specifies the entry's order within the catalog
<nobr><code>Attribute[]</code> attributes</nobr> | A list of attributes which consists of key and value. The keys must be in the `validAttributes` list.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
