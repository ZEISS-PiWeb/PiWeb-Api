---
category: dataservice
subCategory: catalogs
title: Data Service
subTitle: Catalogs
isSubPage: true
permalink: /dataservice/catalogues/
sections:
  general: General Information
  endpoint: REST API Endpoints
  sdk: .NET SDK Methods
---

## {{ page.sections['general'] }}

Each catalog describes a list of entries. All entries have the same defined set of attributes, called *valid attributes*. 
All valid attributes must be created as *catalog attributes* beforehand. ```Catalogue``` and ```CatalogueEntry```have the following structures:

### Catalogue

Property         | Datatype             | Description
-----------------|----------------------|------------------------
uuid             | ```Guid```           | Identifies the catalog uniquely
name             | ```string```         | Name of the catalog
validAttributes  | ```ushort[]```       | A list of attribute keys that are valid for this catalog
catalogueEntries | ```CatalogueEntry``` | A list of catalog entries

### CatalogueEntry

Property         | Datatype             | Description
-----------------|----------------------|------------------------
key              | ```short```          | Specifies the entry's order within the catalog
attributes       | ```Attribute[]```    | A list of attributes which consists of key and value. The keys must be from the validAttributes.

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Catalogs and catalog entries can be fetched, created, updated and deleted using the following endpoints. These endpoints don't provide filter parameters.

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|------|-----|-------
/catalogues | Returns all catalogs without their entries | Creates the committed catalog(s) which is/are transfered in the body of the request | Updates the committed catalogs and their entries | Deletes all catalogs and the catalog entries
/catalogues/entries | Returns all catalogs *with entries* | *--* | *--* | *--*
/catalogues/(:catUuid1, :catUuid2,...) | Returns all catalogs from the catUuid list *without entries* | *--* | *--* | Deletes the catalogs with the provided catUuids
/catalogues/(:catUuid1, :catUuid2,...)/entries | Returns the catalogs of which the uuid is within the catUuid list including their entries | *--* | *--* | *--*
catalogues/:catalogueUuid/entries | *--*| Creates the entries in the request body for the catalog specified by the *:catalogueUuid* | *--* | Deletes all entries from the catalogue specified by the *:catalogueUuid*
catalogues/:catalogueUuid/entries/{key1, key2...} | *--* | *--* | *--* | Deletes the entries specified by their particular key from the catalog specified by the *:catalogueUuid* 

### Get Catalogs (GET)

Catalogs can be fetched with or without their entries, depending on the endpoint method.

{% assign exampleCaption="Fetching the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad and its entries" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/catalogues/(8c376bee-ffe3-4ee4-abb9-a55b492e69ad)/entries HTTP/1.1
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

{% include exampleFieldset.html %}

### Add Catalogs (POST)

To create a new catalog the catalog object must be transmitted the request's body. A valid add request must contain a unique identifier, the catalog name and the valid attributes. Catalog entries are optional. All valid attributes must be added as catalog attributes beforehand (see {{ site.links['configuration'] }}).

{{ site.images['info'] }} If no catalog entries are specified an empty catalog entry with key '0' and attribute value(s) 'not defined' ( in case of alphanumeric attributes ) is created by default.

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

{% include exampleFieldset.html %}


### Add Catalog Entries (POST)

To add new entries to an existing catalog you must specify all new entries in the request body. Each new entry must contain a unique key.
Each entry attribute must be listed as a valid attributes in the catalog definition.

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

{% include exampleFieldset.html %}


### Update Catalogs (PUT)

Update a catalog if you want to:

* rename the catalog or
* add, update or delete catalog entries.

To update a catalog, the whole object excluding the valid attributes needs to be transmitted in the body of the HTTP request. Updating a catalog essentially replaces the current catalog with the new one (delete followed by an add) in a single transaction.

{{site.images['info']}} To change the valid attributes the catalog needs to be deleted an re-created again.

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

{% include exampleFieldset.html %}

### Delete Catalogs (DELETE)

There are two different options for deleting catalogs: 

* delete all catalogs or
* delete one or more certain catalogs identified by their uuid.

The following examples demonstrates these options.

{% assign exampleCaption="Delete all catalogs and their entries" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogues HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

{% assign exampleCaption="Delete the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad and its entries" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogues/(8c376bee-ffe3-4ee4-abb9-a55b492e69ad) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

### Delete Catalog Entries

There are two different options for deleting catalog entries: 

* delete all entries from a certain catalog identified by its uuid or
* delete one or more specific entries identified by their keys from a certain catalog identified by its uuid
 
The following examples demonstrate these options.

{% assign exampleCaption="Delete all entries from the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogues/8c376bee-ffe3-4ee4-abb9-a55b492e69ad/entries HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

{% assign exampleCaption="Delete the entries with key 1 and 3 from the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogues/8c376bee-ffe3-4ee4-abb9-a55b492e69ad/entries/(1,3) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['sdk'] }}

### Get Catalogs

{% assign caption="GetCatalogues" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches one or more catalogs identified by their uuids or all catalogs if no uuid is set." %}
{% capture parameterTable %}

 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuids | ```Guid[]```            | Parameter is optional and may include the uuids of the catalogs to be fetched. If no uuid is given, all catalogs will be fetched.
returnEntries  | ```bool```              | Parameter is optional and indicates whether the catalog entries shall be returned.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Get the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var catalogues = client.GetCatalogues(new Guid[]{new Guid(
        "8c376bee-ffe3-4ee4-abb9-a55b492e69ad")}, new CatalogueFilterAttributes());
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}


### Create Catalogs

{% assign caption="CreateCatalogues" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Creates one or more catalogs." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogues     | ```Catalogue[]```       | Includes the catalogs which shall be created.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Create the catalog InspectorCatalogue" %}
{% capture example %}
{% highlight csharp %}
var catalogue = new Catalogue(){ 
  Uuid = new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ),
  Name = "InspectorCatalogue",
  ValidAttributes = new ushort[]{ 4092, 4093 },
  CatalogueEntries = new[]{
    new CatalogueEntry(){ Key = 0, 
    Attributes = new[]{ new Attribute( 4092, "n.def." ), new Attribute( 4093, "n.def." ) },
    new CatalogueEntry(){ Key = 1, 
    Attributes = new[]{ new Attribute( 4092, "21" ), new Attribute( 4093, "Smith" ) },
    new CatalogueEntry(){ Key = 2, 
    Attributes = new[]{ new Attribute( 4092, "20" ), new Attribute( 4093, "Miller" ) },
    new CatalogueEntry(){ Key = 3, 
    Attributes = new[]{ new Attribute( 4092, "23" ), new Attribute( 4093, "Williams" ) }
  }
};
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.CreateCatalogues( new[]{ catalogue } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Create Catalog Entries

{% assign caption="CreateCatalogueEntry" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Adds a catalog entry to an existing catalog." %}
{% capture parameterTable %}
Name           | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Uuid[]```            | The Uuid by which the catalog is identified.
entry          | ```CatalogueEntry```    | The entry which should be added to the catalog.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Add a entry for the inspector Clarks to the InspectorCatalogue" %}
{% capture example %}
{% highlight csharp %}
var entry = new CatalogueEntry(){ Key = 4, 
        Attributes = new[]{ new Attribute( 4092, "22" ), new Attribute( 4093, "Clarks" ) };
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.CreateCatalogueEntry( new Guid("8c376bee-ffe3-4ee4-abb9-a55b492e69ad"), entry);
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="CreateCatalogueEntries" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Adds multiple catalog entries to an existing catalog." %}
{% capture parameterTable %}
Name           | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Uuid[]```            | The Uuid by which the catalog is identified.
entry          | ```CatalogueEntry[]```  | The entries which shall be added to the catalog.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Add entries for the inspectors Adams and Watson" %}
{% capture example %}
{% highlight csharp %}
var entryAdams = new CatalogueEntry(){ Key = 5, 
        Attributes = new[]{ new Attribute( 4092, "23" ), new Attribute( 4093, "Adams" ) };
var entryWatson = new CatalogueEntry(){ Key = 6, 
        Attributes = new[]{ new Attribute( 4092, "24" ), new Attribute( 4093, "Watson" ) };
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.CreateCatalogueEntry( new Guid("8c376bee-ffe3-4ee4-abb9-a55b492e69ad"), new[]{ entryAdams, entryWatson);
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Update Catalogs

{% assign caption="UpdateCatalogues" %}
{% assign icon=site.images['function-update'] %}
{% assign description="Updates one or more catalogs. Use to rename the catalog or add, update or delete catalog entries. Updating a catalog removes the existing entries and adds the new ones. To change the valid attributes of a catalog, it must be deleted an re-created again." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogues     | ```Catalogue[]```       | Includes the catalogs which shall be updated.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Rename the catalog InspectorCatalogue to Inspectors and add the inspector Clarks" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );

//Get the catalogue
...

catalogue.Name = "Inspectors";
var entries = new List< CatalogueEntry >();
entries = catalogue.CatalogueEntries;
entries.Add( new CatalogueEntry()
      { Key = 4, 
        Attributes = new[]{ new Attribute( 4092, "22" ), new Attribute( 4093, "Clarks" ) };
cataloge.CatalogueEntries = entries.ToArray();
client.UpdateCatalogues( catalogue );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Delete Catalogs

{% assign caption="DeleteCatalogues" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes the catalogs defined in the ```catalogueUuids``` or all catalogs if the parameter is empty." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuids | ```Guid[]```            | Parameter is optional can be used to delete specific catalogs only. If no uuid is set, all catalogs will be deleted.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Delete the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.DeleteCatalogues( new Guid[]{new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ) );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Delete Catalog Entries

{% assign caption="DeleteCatalogueEntry" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes the entry with the ```key```from the catalog defined by ```catalogueUuid```." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Guid```              | The uuid of the catalog which the entry belongs to.
key            | ```ushort```            | The key of the entry which should be deleted.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Delete the entry with the key 1 from the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.DeleteCatalogueEntries( 
  new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad", new []{ (ushort)1 } );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

{% assign caption="DeleteCatalogueEntries" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes the entries of which the key is included in ```keys```from the catalog defined by ```catalogueUuid```, or all entries if ```keys```is empty." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Guid```              | The uuid of the catalog which the entries belong to.
keys           | ```ushort[]```          | The keys of the entries which shall be deleted. If it's empty, all entries are deleted.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Delete all entries from the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.DeleteCatalogueEntries( new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ) );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}
