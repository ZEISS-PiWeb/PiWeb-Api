---
category: dataservice
subCategory: catalogues
title: Data Service
subTitle: Catalogues
isSubPage: true
permalink: /dataservice/catalogues/
sections:
  general: General Information
  endpoint: REST API Endpoints
  sdk: .NET SDK Methods
---

## {{ page.sections['general'] }}

Every catalogue consists of a range of valid attributes and several catalogue entries.
All attribute keys which are used within the valid attributes or the catalogue entries must have been created as catalogue attributes within the PiWeb configuration. ```Catalogue``` and ```CatalogueEntry```have the following structures:

### Catalogue

Property         | Datatype             | Description
-----------------|----------------------|------------------------
uuid             | ```Guid```           | Identifies the catalogue uniquely
name             | ```string```         | Name of the catalogue
validAttributes  | ```ushort[]```       | A list of attribute keys that are valid for the respective catalogue
catalogueEntries | ```CatalogueEntry``` | A list of catalogue entries

### CatalogueEntry

Property         | Datatype             | Description
-----------------|----------------------|------------------------
key              | ```short```          | Specifies the entry's order within the catalogue
attributes       | ```Attribute[]```    | A list of attributes which consists of key and value. The keys must come from the validAttributes range.

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Catalogues and catalogue entries can be fetched, created, updated and deleted via the following endpoints. There are no filter parameters to restrict the queries.

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|------|-----|-------
/catalogues | Returns all catalogues without their entries | Creates the committed catalogue(s) which is/are transfered in the body of the request | Updates the committed catalogues and their entries | Deletes all catalogues and the catalogue entries
/catalogues/entries | Returns all catalogues including their respective entries | *--* | *--* | *--*
/catalogues/(:catUuid1, :catUuid2,...) | Returns the catalogues of which the uuid is within the catUuid list without their entries | *--* | *--* | Deletes the catalogue(s) which has/have the given catUuid(s)
/catalogues/(:catUuid1, :catUuid2,...)/entries | Returns the catalogues of which the uuid is within the catUuid list including their respective entries | *--* | *--* | *--*
catalogues/:catalogueUuid/entries | *--*| Creates the entries transfered in the body of the request for the catalogue specidied by the *:catalogueUuid* | *--* | Deletes all entries of the catalogue specified by the *:catalogueUuid*
catalogues/:catalogueUuid/entries/{key1, key2...} | *--* | *--* | *--* | Deletes the entries specified by their particular key of the catalogue specified by the *:catalogueUuid* 

### Get Catalogues (GET)

Catalogues can be fetched with or without their respective entries, depending on the endpoint method.

{% assign exampleCaption="Fetching the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad and its entries" %}

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

### Add Catalogues (POST)

To create a catalogue, it is necessary to transfer the catalogue object within the request's body. Beneath a unique identifier and the catalog name, the valid attributes need to be transfered. Catalogue entries are optional. The attribute keys, which are used for the valid attributes, must come from the catalogue attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} If no catalogue entries are transfered, an empty catalogue entry with the key 0 and attribute values 'not defined' ( in case of alphanumeric attributes ) is created by default.

{% assign exampleCaption="Adding the catalogue InspectorCatalogue" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/catalogues HTTP/1.1
{% endhighlight %}

{% highlight json %}
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
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}
{% endcapture %}

{% include exampleFieldset.html %}


### Add Catalogue Entries (POST)

Beneath adding catalogue entries to a catalogue while creating a catalogue, there is also the possibility to add entries to an already existing catalogue.

{% assign exampleCaption="Adding a catalogue entry - add the inspector ‘Clarks’" %}

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


### Update Catalogues (PUT)

Updating a catalogue might regard the following aspects: 

* Rename the catalogue 
* Add, update or delete catalogue entries

To update a catalogue, the whole object excluding the valid attributes needs to be transfered within the body of the HTTP request. On updating catalogues, the existing entries are removed and the passed entries are added to the catalogue.

{{site.images['info']}} To change the valid attributes of a catalogue, it needs to be deleted an re-created again.

{% assign exampleCaption="Rename the catalogue from 'InspectorCatalogue' to 'Inspectors' and add the inspector 'Clarks'" %}

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

### Delete Catalogues (DELETE)

There are two different options for deleting catalogues: 

* Delete all catalogues or
* Delete one or more certain catalogues identified by their uuid

The following examples illustrate these options.

{% assign exampleCaption="Delete all catalogues and their entries" %}

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

{% assign exampleCaption="Delete the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad and its entries" %}

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

### Delete Catalogue Entries

There are two different options for deleting catalogue entries: 

* Delete all entries of a certain catalogue identified by its uuid
* Delete one or more certain entries identified by their keys of a certain catalogue identified by its uuid
 
The following examples illustrate these options.

{% assign exampleCaption="Delete all entries of the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

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

{% assign exampleCaption="Delete the entries with key 1 and 3 of the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

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

### Get Catalogues

{% assign caption="GetCatalogues" %}
{% assign icon=site.images['function-get'] %}
{% assign description="Fetches one or more catalogues identified by their uuids or all catalogues if no uuid is passed." %}
{% capture parameterTable %}

 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuids | ```Guid[]```            | Parameter is optional and may include the uuids of the catalogues to be fetched. If no uuid is given, all catalogues will be fetched.
returnEntries  | ```bool```              | Parameter is optional and indicates whether the catalogue entries should be returned.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Get the catalogue with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
var catalogues = client.GetCatalogues(new Guid[]{new Guid(
        "8c376bee-ffe3-4ee4-abb9-a55b492e69ad")}, new CatalogueFilterAttributes());
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}


### Create Catalogues

{% assign caption="CreateCatalogues" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Creates one or more catalogues." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogues     | ```Catalogue[]```       | Includes the catalogues which should be created.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Create the catalogue InspectorCatalogue" %}
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

### Create Catalogue Entries

{% assign caption="CreateCatalogueEntry" %}
{% assign icon=site.images['function-create'] %}
{% assign description="Adds a catalogue entry to an existing catalogue." %}
{% capture parameterTable %}
Name           | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Uuid[]```            | The Uuid by which the catalogue is identified.
entry          | ```CatalogueEntry```    | The entry which should be added to the catalogue.
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
{% assign description="Adds multiple catalogue entries to an existing catalogue." %}
{% capture parameterTable %}
Name           | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Uuid[]```            | The Uuid by which the catalogue is identified.
entry          | ```CatalogueEntry[]```  | The entries which should be added to the catalogue.
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

### Update Catalogues

{% assign caption="UpdateCatalogues" %}
{% assign icon=site.images['function-update'] %}
{% assign description="Updates one or more catalogues. This might affect renaming the catalogue as well as add, update or delete catalogue entries. On updating catalogues, the existing entries ar removed and the passed entries are added to the catalogue. To change the valid attributes of a catalogue, it must be deleted an re-created again." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogues     | ```Catalogue[]```       | Includes the catalogues which should be updated.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Rename the catalogue InspectorCatalogue to Inspectors and add the inspector Clarks" %}
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

### Delete Catalogues

{% assign caption="DeleteCatalogues" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes the catalogues defined in the ```catalogueUuids``` or all catalogues if the parameter is empty." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuids | ```Guid[]```            | Parameter is optional and may include the uuids of the catalogues which should be deleted. If no uuid is given, all catalogues will be deleted.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Delete the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.DeleteCatalogues( new Guid[]{new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ) );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}

### Delete Catalogue Entries

{% assign caption="DeleteCatalogueEntry" %}
{% assign icon=site.images['function-delete'] %}
{% assign description="Deletes the entry with the ```key```from the catalogue defined by ```catalogueUuid```." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Guid```              | The uuid of the catalogue to which the entry, which should be deleted, belongs to.
key            | ```ushort```            | The key of the entry which should be deleted.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Delete the entry with the key 1 from the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
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
{% assign description="Deletes the entry that keys are included in ```keys```from the catalogue defined by ```catalogueUuid```, or all entries if ```keys```is empty." %}
{% capture parameterTable %}
 Name          | Type                    | Description
---------------|-------------------------|--------------------------------------------------
catalogueUuid  | ```Guid```              | The uuid of the catalogue to which the entries, which should be deleted, belongs to.
keys           | ```ushort[]```          | The keys of the entries which should be deleted. If it's empty, all entries are deleted.
token          | ```CancellationToken``` | Parameter is optional and allows to cancel the asyncronous call.
{% endcapture %}

{% assign exampleCaption="Delete all entries from the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}
{% capture example %}
{% highlight csharp %}
var client = new DataServiceRestClient( "http://piwebserver:8080" );
client.DeleteCatalogueEntries( new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ) );
{% endhighlight %}
{% endcapture %}

{% include sdkFunctionFieldset.html %}
