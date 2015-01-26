---
category: dataservice
subCategory: inspection-plan
title: Data Service
subTitle: Inspection Plan
isSubPage: true
permalink: /dataservice/inspection-plan/
sections:
  general: General Information
  endpoint: Endpoint Information
  add: Add Entities
  get: Get Entities
  update: Update Entities
  delete: Delete Entities
---

## {{ page.sections['general'] }}

Both parts and characteristics are PiWeb inspeaction plan entities. Each entity consits of the following properties:

Name | Description
-----|-------------
uuid | Identifies this inspection plan entity uniquely.
path | The path of this entity.
attributes | A set of attributes which specifies this entity.
comment | A comment which describes the last inspection plan change.
version | Contains the revision number of the entity. The revision number starts with zero and is incremented by one each time when changes are applied to the inspection plan. The version is only returned if versioning is enabled in server settings.
current | Indicates wheter the entity is the current version.
timeStamp | Contains the date and time of the last update applied to this entity.
charChangeDate (only for parts) | The timestamp for the most recent characteristic change on any characteristic that belongs to this part

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Parts and characteristics can be fetched, created, updated and deleted via the following endpoints. Filter can be set as described in the [URL-Parameter section]({{site.baseurl }}/general/#{{ page.subCategory }}).

###Parts

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|-----|------|-------
/parts | Returns all parts | Creates the committed part(s) which is/are transfered in the body of the request | Updates the committed parts | Deletes all parts
/parts/:partsPath | Returns the part specified by *:partsPath* as well as the parts beneath this part | *Not supported* | *Not supported* | Deletes the part specified by *:partsPath* as well as the parts and characteristics beneath this part
parts/{:uuidList} | Returns all parts that uuid are within the *:uuidList* | *Not supported* | *Not supported* |  Deletes all parts that uuid are within the *:uuidList* as well as the parts and characteristics beneath the particular part

### Characteristics

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|-----|------|-------
/characteristics | *Not supported* | Creates the committed characteristic(s) which is/are transfered in the body of the request | Updates the committed characteristics | *Not supported*
/characteristics/:partsPath | Returns the characteristics beneath the part specified by *:partPath* | *Not supported* | *Not supported* | Deletes the characteristics beneath the part specified by *:partsPath*
characteristics/{:uuidList} | Returns all characteristics that uuid are within the *:uuidList* | *Not supported* | *Not supported* |  Deletes all characteristics that uuid are within the *:uuidList*

## {{ page.sections['add'] }}

To create a inspection plan entity it is necessary to transfer the entity object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the [configuration]({{site.baseurl }}/{{page.category}}/parts/)

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings.

### {{ site.headers['example'] }} Adding a part with the uuid 05040c4c-f0af-46b8-810e-30c0c00a379e

{{ site.sections['beginExampleWebService'] }}
{{ site.sections['formatBtns'] }}

<div class="dropdown">

		<button class="btn btn-danger" type="button" id="myButton">Show Menue</button>

		<ul class="dropdown-menu">

			<li><a href="#">Schuhe</a></li>

			<li><a href="#">Pferd</a></li>

			<li><a href="#">Fahrrad</a></li>

			<li><a href="#">Moped</a></li>

		</ul>

</div>

<script>

$('#myButton').dropdown();

</script>

{{ site.headers['request']  | markdownify }}

{% highlight http %}
POST /dataServiceRest/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
    "path": "P:/metal part",
    "attributes": 
    {
      "1001": "4466",
      "1003": "mp"
    }       
  }
]
{% endhighlight %}

{{ site.headers['response']  | markdownify }}

{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{{ site.sections['endExample'] }}
{{ site.sections['beginExampleAPI'] }}

{{ site.headers['request'] | markdownify }}

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
var client = new DataServiceRestClient( serviceUri );
client.CreateCatalogues( new[]{ catalogue } );
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['addEntries'] }}

Beneath adding catalogue entries to a catalogue while creating a catalogue there is also the possibility to add entries to an already existing catalogue.

### {{ site.headers['example'] }}  Adding a catalogue entry - add the inspector ‘Clarks’

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
POST /dataServiceRest/catalogues/{8c376bee-ffe3-4ee4-abb9-a55b492e69ad}/entries
{% endhighlight %}

{% highlight json %}
 [
   {
       "key": 4,
       "attributes":
       {
           "4092": "22",
           "4093": "Clarks"
       }
   }
 ]
{% endhighlight %}

{{ site.headers['response'] | markdownify }}

{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{{ site.sections['endExample'] }}
{{ site.sections['beginExampleAPI'] }}

{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var entry = new CatalogueEntry(){ Key = 4, 
        Attributes = new[]{ new Attribute( 4092, "22" ), new Attribute( 4093, "Clarks" ) };
var client = new DataServiceRestClient( serviceUri );
client.CreateCatalogueEntry( new Guid("8c376bee-ffe3-4ee4-abb9-a55b492e69ad"), entry);
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['get'] }}

Fetching the catalogues returns the catalogue an depending on the filter specified or not the catalogue entries. If no filter is specified the entries are returned by default.

### {{ site.headers['example'] }}  Fetching the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad and its entries

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
GET /dataServiceRest/catalogues/{8c376bee-ffe3-4ee4-abb9-a55b492e69ad}?filter=withCatalogueEntries:true HTTP/1.1
{% endhighlight %}

{{ site.headers['response'] | markdownify }}
{% highlight json %}
{
   ...
   "data":
   [
       {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "InspectorCatalogue",
           "validAttributes":
           [
               4092,
               4093
           ],
           "catalogueEntries":
           [
               {
                   "key": 0,
                   "attributes":
                   {
                       "4092": "n.def.",
                       "4093": "n.def."
                   }
               },
               {
                   "key": 1,
                   "attributes":
                   {
                       "4092": "21",
                       "4093": "Smith"
                   }
               },
               {
                   "key": 2,
                   "attributes":
                   {
                       "4092": "20",
                       "4093": "Miller"
                   }
               },
               {
                   "key": 3,
                   "attributes":
                   {
                       "4092": "23",
                       "4093": "Williams"
                   }
               }
            ]
        }
   ]
}
{% endhighlight %}

{{ site.sections['endExample'] }}

{{ site.sections['beginExampleAPI'] }}
{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
var catalogues = client.GetCatalogues(new Guid[]{new Guid(
        "8c376bee-ffe3-4ee4-abb9-a55b492e69ad")}, new CatalogueFilterAttributes());
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['update'] }}

Updating a catalogue might regard the following aspects: 

* Rename the catalogue 
* Add, update or delete catalogue entries

{{site.images['info']}} To change the valid attributes of a catalogue it needs to be deleted an re-created again.

### {{ site.headers['example'] }}  Updating the catalogue with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad - rename it from 'InspectorCatalogue' to 'Inspectors' and add the inspector 'Clarks'

{{ site.sections['beginExampleWebService'] }}

{{ site.headers['request']  | markdownify }}

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
                   "attributes":
                   {
                       "4092": "n.def.",
                       "4093": "n.def."
                   }
               },
               {
                   "key": 1,
                   "attributes":
                   {
                       "4092": "21",
                       "4093": "Smith"
                   }
               },
               {
                   "key": 2,
                   "attributes":
                   {
                       "4092": "20",
                       "4093": "Miller"
                   }
               },
               {
                   "key": 3,
                   "attributes":
                   {
                       "4092": "23",
                       "4093": "Williams"
                   }
               },
               {
                   "key": 4,
                   "attributes":
                   {
                       "4092": "22",
                       "4093": "Clarks"
                   }
               }
            ]
        }
]
{% endhighlight %}

{{ site.headers['response']  | markdownify }}

{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{{ site.sections['endExample'] }}
{{ site.sections['beginExampleAPI'] }}

{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );

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

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['delete'] }}

There are two different options of deleting catalogues: 

* Delete all catalogues or
* Delete one or more certain catalogues identified by its uuid
 
The following examples illustrate these options.

### {{ site.headers['example'] }}  Delete all catalogues

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
DELETE /dataServiceRest/catalogues HTTP/1.1
{% endhighlight %}

{{ site.headers['response'] | markdownify }}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{{ site.sections['endExample'] }}

{{ site.sections['beginExampleAPI'] }}
{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
client.DeleteCatalogues();
{% endhighlight %}

{{ site.sections['endExample'] }}

### {{ site.headers['example'] }}  Delete the catalogues with the uuid "8c376bee-ffe3-4ee4-abb9-a55b492e69ad"

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
DELETE /dataServiceRest/catalogues/{8c376bee-ffe3-4ee4-abb9-a55b492e69ad} HTTP/1.1
{% endhighlight %}

{{ site.headers['response'] | markdownify }}

{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{{ site.sections['endExample'] }}

{{ site.sections['beginExampleAPI'] }}
{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
client.DeleteCatalogues( new Guid[]{new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ) );
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['deleteEntries'] }}

There are two different options of deleting catalogue entries: 

* Delete all entries of a certain catalogue identified by its uuid
* Delete one or more certain entries identified by its keys of a certain catalogue identified by its uuid
 
The following examples illustrate these options.

### {{ site.headers['example'] }}  Delete all entries of the catalogue with the uuid "8c376bee-ffe3-4ee4-abb9-a55b492e69ad"

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
DELETE /dataServiceRest/catalogues/{8c376bee-ffe3-4ee4-abb9-a55b492e69ad}/entries HTTP/1.1
{% endhighlight %}

{{ site.headers['response'] | markdownify }}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{{ site.sections['endExample'] }}

{{ site.sections['beginExampleAPI'] }}
{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
client.DeleteCatalogueEntries( new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad" ) );
{% endhighlight %}

{{ site.sections['endExample'] }}

### {{ site.headers['example'] }}  Delete the entries with key 1 and 3 of the catalogue with the uuid "8c376bee-ffe3-4ee4-abb9-a55b492e69ad"

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
DELETE /dataServiceRest/catalogues/{8c376bee-ffe3-4ee4-abb9-a55b492e69ad}/entries/{1,3} HTTP/1.1
{% endhighlight %}

{{ site.headers['response'] | markdownify }}

{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{{ site.sections['endExample'] }}

{{ site.sections['beginExampleAPI'] }}
{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
client.DeleteCatalogueEntries( 
  new Guid( "8c376bee-ffe3-4ee4-abb9-a55b492e69ad", new []{ (ushort)1, (ushort(3) } );
{% endhighlight %}

{{ site.sections['endExample'] }}

