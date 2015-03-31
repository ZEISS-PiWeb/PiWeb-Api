---
area: restApi
level: 2
category: dataService
subCategory: catalogs
title: REST API
subTitle: Data Service
pageTitle: Catalogs
permalink: /restapi/dataservice/catalogs/
---

## {{ page.pageTitle }}

### Endpoints

Catalogs and catalog entries can be fetched, created, updated and deleted using the following endpoints.

{% assign linkId="catalogEndpointGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Fetches catalogs" %}
{% assign description="This request can be restricted by the filter parameter `catalogUuids`" %}
{% assign exampleCaption="Fetching all catalogs" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/catalogs HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
      {
         "uuid": "d7291afb-0a67-4c1e-8bcc-6fc455bcc0e5",
         "name": "direction catalog",
         "validAttributes": [ 2009 ],
         "catalogueEntries":
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
         "name": "InspectorCatalogue",
         "validAttributes": [ 4092, 4093 ],
         "catalogueEntries":
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
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="catalogEndpointGetSingle" %}
{% assign method="GET" %}
{% assign endpoint="/catalogs/:catalogUuid" %}
{% assign summary="Returns the catalog specified by the :catalogUuid" %}
{% assign description="" %}
{% assign exampleCaption="Fetching the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
       {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "InspectorCatalogue",
           "validAttributes": [ 4092, 4093 ],
           "catalogueEntries":
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
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="catalogEndpointCreate" %}
{% assign method="POST" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Creates catalogs" %}
{% capture description %}
To create a new catalog the catalog object must be transmitted the request's body. A valid add request must contain a unique identifier, the catalog name and the valid attributes. Catalog entries are optional. All valid attributes must be added as catalog attributes beforehand (see {{ site.links['configuration'] }}).

{{ site.images['info'] }} If no catalog entries are specified an empty catalog entry with key '0' and attribute value(s) 'not defined' ( in case of alphanumeric attributes ) is created by default.
{% endcapture %}
{% assign exampleCaption="Adding the catalog InspectorCatalog" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/catalogues HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "InspectorCatalog",
           "validAttributes": [ 4092, 4093 ],
           "catalogueEntries":
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

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="catalogEndpointCreateEntries" %}
{% assign method="POST" %}
{% assign endpoint="/catalogs/:catalogUuid" %}
{% assign summary=" Creates entries for the catalog specified by the :catalogueUuid" %}
{% assign description="To add new entries to an existing catalog you must specify all new entries in the request body. Each new entry must contain a unique key. Each entry attribute must be listed as a valid attributes in the catalog definition." %}
{% assign exampleCaption="Adding a catalog entry - add the inspector ‘Clarks’" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/catalogues/8c376bee-ffe3-4ee4-abb9-a55b492e69ad/entries
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
Update a catalog if you want to:

* rename the catalog or
* add, update or delete catalog entries.

To update a catalog, the whole object excluding the valid attributes needs to be transmitted in the body of the HTTP request. Updating a catalog essentially replaces the current catalog with the new one (delete followed by an add) in a single transaction.

{{site.images['info']}} To change the valid attributes the catalog needs to be deleted an re-created again.
{% endcapture %}
{% assign exampleCaption="Rename the catalog from 'InspectorCatalog' to 'Inspectors' and add the inspector 'Clarks'" %}

{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/catalogues HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "Inspectors",
           "catalogueEntries":
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
               },
               {
                   "key": 4,
                   "attributes": { "4092": "22", "4093": "Clarks" }
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
{% assign summary="Deletes catalogs" %}
{% capture description %}
There are two different options for deleting catalogs:

* delete all catalogs or
* delete one or more certain catalogs identified by their uuid. Therefore you need to add the filter parameter `catalogUuids` to the request uri.
{% endcapture %}
{% assign exampleCaption="Delete the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogues?catalogUuids=(8c376bee-ffe3-4ee4-abb9-a55b492e69ad) HTTP/1.1
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
{% assign endpoint="/catalogs/:catalogUuid" %}
{% assign summary="Deletes entries for the catalog specified by the :catalogueUuid" %}
{% capture description %}
There are two different options for deleting catalog entries:

* Delete all entries from a certain catalog identified by its uuid or
* Delete one or more specific entries identified by their keys from a certain catalog identified by its uuid. Therefore you need to add the filter parameter `entryIds` to the request uri.
{% endcapture %}

{% assign exampleCaption="Delete the entries with key 1 and 3 from the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogues/8c376bee-ffe3-4ee4-abb9-a55b492e69ad?entryIds=(1,3) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


### Filters

These endpoints  provide the following filter parameters:

{% capture table %}
Parameter name      | Description  <br> `Example` 
--------------------|------------------  ------------------------------
`catalogUuids`      | List of catalogue uuids that restrict the request. <br> `(d7291afb-0a67-4c1e-8bcc-6fc455bcc0e5, 8c376bee-ffe3-4ee4-abb9-a55b492e69ad)`
`entryIds`          | List of catalogue entry ids that restrict the request. <br> `(1,4)`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


### General Information

Each catalog describes a list of entries. All entries have the same defined set of attributes, called *valid attributes*. 
All valid attributes must be created as *catalog attributes* beforehand. ```Catalog``` and ```CatalogEntry```have the following structures:

#### Catalog

{% capture table %}
Property         | Datatype             | Description
-----------------|----------------------|------------------------
uuid             | ```Guid```           | Identifies the catalog uniquely
name             | ```string```         | Name of the catalog
validAttributes  | ```ushort[]```       | A list of attribute keys that are valid for this catalog
catalogEntries   | ```CatalogEntry```   | A list of catalog entries
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### CatalogEntry

{% capture table %}
Property         | Datatype             | Description
-----------------|----------------------|------------------------
key              | ```short```          | Specifies the entry's order within the catalog
attributes       | ```Attribute[]```    | A list of attributes which consists of key and value. The keys must be from the validAttributes.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
