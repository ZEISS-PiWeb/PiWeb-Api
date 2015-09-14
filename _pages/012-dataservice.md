<h2 id="{{page.sections['dataservice']['secs']['serviceInformation'].anchor}}">{{page.sections['dataservice']['secs']['serviceInformation'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['serviceInformation'].anchor}}-endpoints">Endpoints</h3>

The service information can be fetched using the following endpoint. This endpoint doesn't provide filter parameters.

{% assign linkId="serviceInformationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/serviceInformation" %}
{% assign summary="Returns general information about PiWeb server and data service" %}
{% assign description="" %}
{% assign exampleCaption="Get Service Information for a given connection" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/serviceInformation HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{
   "serverName": "PiWeb Server",
   "version": "5.6.2.0",
   "securityEnabled": false,
   "edition": "PiWebDB",
   "versionWsdlMajor": "2",
   "versionWsdlMinor": "9",
   "partCount": 4,
   "characteristicCount": 125,
   "measurementCount": 20,
   "valueCount": 900,
   "featureList":
   [
      "MeasurementAggregation",
      "DistinctMeasurementSearch"
   ],
   "inspectionPlanTimestamp": "2014-11-24T16:08:58.812964+01:00",
   "measurementTimestamp": "2014-11-03T10:27:28.3461853+01:00",
   "configurationTimestamp": "2014-11-03T10:27:27.5245116+01:00"
}

{% endhighlight %}
{% endcapture %}
{% include endpointTab.html %}

<h3 id="{{page.sections['dataservice']['secs']['serviceInformation'].anchor}}-objectstructure">Object Structure</h3>

Service information requests always have the smallest response time and are therefore well suited for checking the connection. Fetching the service information doesn't require authentication.

The returned ServiceInformation object has the following properties:

{% capture table %}
Property                                                   | Description
-----------------------------------------------------------|---------------------------------------------------------------
<nobr><code>string</code> serverName</nobr>                | The name of the PiWeb server as specified in the server settings
<nobr><code>string</code> version</nobr>                   | The version number of the PiWeb server
<nobr><code>bool</code> securityEnabled</nobr>             | Indicates whether authentication is required by the server
<nobr><code>string</code> edition</nobr>                   | The database edition. Usually this is "PiWebDB"
<nobr><code>string</code> versionWsdlMajor</nobr>          | The major version number of the interface
<nobr><code>string</code> versionWsdlMinor</nobr>          | The minor version number of the interface
<nobr><code>int</code> partCount</nobr>                    | The estimated number of parts stored on the server
<nobr><code>int</code> characteristicCount</nobr>          | The estimated number of characteristics stored on the server
<nobr><code>int</code> measurementCount</nobr>             | The estimated number of measurements stored on the server
<nobr><code>int</code> valueCount</nobr>                   | The estimated number of measured values stored on the server
<nobr><code>Features</code> featureList</nobr>             | A list of features supported by the server. This can be *MeasurementAggregation* or *DistinctMeasurementSearch*
<nobr><code>DateTime</code> inspectionPlanTimestamp</nobr> | Timestamp of the last inspection plan modification
<nobr><code>DateTime</code> measurementTimestamp</nobr>    | Timestamp of the last measurement modification
<nobr><code>DateTime</code> configurationTimestamp</nobr>  | Timestamp of the last configuration modification
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

<h2 id="{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['configuration'].anchor}}-endpoints">Endpoints</h3>

The configuration can be fetched, created, updated and deleted using the following endpoints. These endpoints do not provide filter parameters.

{% assign linkId="configurationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/configuration" %}
{% assign summary="Returns the attribute definitions for all entity types" %}
{% assign description="" %}
{% assign exampleCaption="Fetching the configuration including all attributes" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/configuration HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{
       "partAttributes":
       [
        [
            "key":1001,
            "description":"partNumber",
            "length":30,
            "type":"AlphaNumeric",
            "definitionType":"AttributeDefinition"
        ],
        ...
       ],
       "characteristicAttributes":
       [
        [
            "key":2001,
            "description":"characteristicNumber",
            "length":20,
            "type":"AlphaNumeric",
            "definitionType":"AttributeDefinition"
        ],
        ...
       ],
       "measurementAttributes":
       [
             "key": 8,
             "description": "inspector",
             "catalog": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
             "definitionType": "CatalogAttributeDefinition"
       ...
       ],
       "valueAttributes":
       [
       ...
       ],
       "catalogAttributes":
       [
       ...
       ]
}

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="configurationEndpointAdd" %}
{% assign method="POST" %}
{% assign endpoint="/configuration/:entityType" %}
{% assign summary="Creates the attributes for :entityType" %}
{% assign description="Creates the attribute definitions transfered within the body of the request for the given `:entityType` which must be part of the uri" %}
{% assign exampleCaption="Adding a part attribute with the key 1001 to the configuration" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/configuration/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "key":1001,
    "description":"partNumber",
    "length":30,
    "type":"AlphaNumeric",
    "definitionType":"AttributeDefinition"
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


{% assign linkId="configurationEndpointUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/configuration/:entityType" %}
{% assign summary="Updates the attributes for :entityType" %}
{% assign description="Updates the attribute definitions transfered within the body of the request for the given `:entityType` which must be part of the uri" %}
{% assign exampleCaption="Updating the part attribute with key 1001 - change length from 30 to 50" %}

{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/configuration/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "key":1001,
    "description":"partNumber",
    "length":50,
    "type":"AlphaNumeric",
    "definitionType":"AttributeDefinition"
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


{% assign linkId="configurationEndpointDelete1" %}
{% assign method="DELETE" %}
{% assign endpoint="/configuration" %}
{% assign summary="Deletes all attribute definitions" %}
{% assign description="" %}
{% assign exampleCaption="Delete all attributes of the current configuration" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/configuration HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="configurationEndpointDelete2" %}
{% assign method="DELETE" %}
{% assign endpoint="/configuration/:entityType/:attributesIdList" %}
{% assign summary="Deletes the attributes in :attributedIdList for :entityType" %}
{% assign description="Deletes all attribute definitions of which the id is in the `:attributeIdList` for the specified `:entityType`. If `:attributeIdList` is not specified, the request deletes all attributes of the `:entityType`." %}
{% assign exampleCaption="Delete the part attributes with the keys *1001* and *1002*" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/configuration/part/{1001, 1002} HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

<h3 id="{{page.sections['dataservice']['secs']['configuration'].anchor}}-objectstructure">Object Structure</h3>

The PiWeb configuration consists of a list of attributes for each entity type. 
The different entity types are: 

* *parts*, 
* *characteristics*, 
* *measurements*, 
* *values* and 
* *catalogs*.

The attributes are either `AttributeDefinition` or `CatalogAttributeDefinition`.
{% capture table %}
####AttributeDefinition

Property                             | Description
-------------------------------------|--------------------------------------------------------------
<nobr><code>ushort</code> key</nobr>            | The attribute's key, which serves as a unique id
<nobr><code>string</code> description</nobr>    | The attribute's name or a short description 
<nobr><code>AttributeType</code> type</nobr>    | The attribute's type. *AlphaNumeric*, *Integer*, *Float* or *DateTime*
<nobr><code>ushort</code> length</nobr>         | The attribute's maximum length. Only set if the type is *AlphaNumeric*
<nobr><code>string</code> definitionType</nobr> | Always has the value 'AttributeDefinition' and is used to differentiate between  `AttributeDefinition` and `CatalogAttributeDefinition`

####CatalogAttributeDefinition

Property                              | Description
--------------------------------------|------------------------------------------------------------
<nobr><code>ushort</code> key</nobr>             | The attribute's key, which serves as a unique id
<nobr><code>string</code> description</nobr>     | The attribute's name or a short description 
<nobr><code>Guid</code> catalog</nobr>         | The id of the catalog that contains the possible attribute values
<nobr><code>string</code> definitionType</nobr>  | Always has the value 'CatalogAttributeDefinition' and is used to differentiate between  `AttributeDefinition` and `CatalogAttributeDefinition`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

<h2 id="{{page.sections['dataservice']['secs']['catalogs'].anchor}}">{{page.sections['dataservice']['secs']['catalogs'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['catalogs'].anchor}}-endpoints">Endpoints</h3>

Catalogs and catalog entries can be fetched, created, updated and deleted using the following endpoints.

{% assign linkId="catalogEndpointGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/catalogs" %}
{% assign summary="Fetches catalogs" %}
{% assign description="This request can be restricted by the filter parameter `catalogUuids`, a list of catalog uuids to target only specific catalogs. " %}
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
To create a new catalog, the catalog object must be transmitted in the request's body. A valid add request must contain a unique identifier, the catalog name and the valid attributes. Catalog entries are optional. All valid attributes must be added as catalog attributes beforehand (see <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>).

{{ site.images['info'] }} If no catalog entries are specified, an empty catalog entry with key '0' and attribute value(s) 'not defined' ( in case of alphanumeric attributes ) is created by default.
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
{% assign summary=" Creates entries for the catalog specified by the :catalogUuid" %}
{% assign description="To add new entries to an existing catalog, you must specify all new entries in the request body. Each new entry must contain a unique key. Each entry attribute must be listed as a valid attribute in the catalog definition." %}
{% assign exampleCaption="Adding a catalog entry - add the inspector ‘Clarks’" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad/entries
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

* rename the catalog or
* add, update or delete catalog entries.

To update a catalog, the whole object, excluding the valid attributes, needs to be transmitted in the body of the HTTP request. Updating a catalog essentially replaces the current catalog with the new one (*delete* followed by an *add*) in a single transaction.

{{site.images['info']}} To change the valid attributes, the catalog needs to be deleted an re-created again.
{% endcapture %}
{% assign exampleCaption="Rename the catalog from 'InspectorCatalog' to 'Inspectors' and add the inspector 'Clarks'" %}

{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/catalogs HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
           "uuid": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
           "name": "Inspectors",
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
* delete one or more specific catalogs identified by their uuid. For this request you need to add the filter parameter `catalogUuids` to the request uri.
* 
{% endcapture %}
{% assign exampleCaption="Delete the catalog with the uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogs?catalogUuids={8c376bee-ffe3-4ee4-abb9-a55b492e69ad} HTTP/1.1
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
{% assign summary="Deletes entries for the catalog specified by the :catalogUuid" %}
{% capture description %}
There are two different options for deleting catalog entries:

* Delete all entries from a certain catalog identified by its uuid or
* Delete one or more specific entries identified by their keys from a certain catalog identified by its uuid. For this request you need to add the filter parameter `entryIds` to the request uri.
{% endcapture %}

{% assign exampleCaption="Delete the entries with key 1 and 3 from the catalog with uuid 8c376bee-ffe3-4ee4-abb9-a55b492e69ad" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/catalogs/8c376bee-ffe3-4ee4-abb9-a55b492e69ad?entryIds={1,3} HTTP/1.1
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

<h2 id="{{page.sections['dataservice']['secs']['inspectionPlan'].anchor}}">{{page.sections['dataservice']['secs']['inspectionPlan'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['inspectionPlan'].anchor}}-endpoints">Endpoints</h3>

You can fetch, create, update and delete parts and characteristics using the following endpoints: 

####Parts

{% assign linkId="inspectionPlanEndpointGetAllParts" %}
{% assign method="GET" %}
{% assign endpoint="/parts" %}
{% assign summary="Fetches parts" %}
{% capture description %}
You can fetch all parts or certain parts. Possible filter uri parameters are: 

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code> partUuids<br></nobr>                               | Restricts the query to the parts with these uuids.
<nobr><code>Path</code> partPath</nobr>                                        | Restricts the query to the part with this path.
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>1</code>       | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part.
<nobr><code>bool</code> withHistory</nobr><br><i>default:</i> <code>false</code>| Determines whether the version history should be fetched or not. This only effects the query if versioning is activated on the server side.
<nobr><code>All, None, ID list</code> requestedPartAttributes</nobr><br><i>default:</i> <code>All</code>                                                                                            | Restricts the query to the attributes that should be returned for parts, for example `requestedPartAttributes={1001, 1008}`.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch the part at path `/metal part` without child parts and only get the values for attributes `1001` and `1003`" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/parts?partPath=/metal%20part&depth=0&requestedPartAttributes={1001,1003} HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
   {
        "path": "P:/metal part/",
        "charChangeDate": "2014-11-19T10:48:32.917Z",
        "attributes": { "1001": "4466", "1003": "mp" },
        "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
        "version": 0,
        "timestamp": "2012-11-19T10:48:32.887Z"
    }
]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointGetPart" %}
{% assign method="GET" %}
{% assign endpoint="/parts/:partUuid" %}
{% assign summary="Fetches a certain part by its :partUuid" %}
{% capture description %}

You can fetch  a certain part by its :partUuid. The result can be restricted by the following uri parameters:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>bool</code> withHistory</nobr><br><i>default:</i> <code>false</code>| Determines whether the version history should be fetched or not. This only effects the query if versioning is activated on the server side.
<nobr><code>All, None, ID list</code> requestedPartAttributes</nobr><br><i>default:</i> <code>All</code>                                                                                            | Restricts the query to the attributes that should be returned for parts, for example `requestedPartAttributes={1001, 1008}`.
{% endcapture %}

{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch the part '/metal part' by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/parts/05040c4c-f0af-46b8-810e-30c0c00a379e HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
   {
        "path": "P:/metal part/",
        "charChangeDate": "2014-11-19T10:48:32.917Z",
        "attributes": {},
        "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
        "version": 0,
        "timestamp": "2012-11-19T10:48:32.887Z"
    }
]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}



{% assign linkId="inspectionPlanEndpointAddParts" %}
{% assign method="POST" %}
{% assign endpoint="/parts" %}
{% assign summary="Creates parts" %}
{% capture description %}

To create a new part, you must send its JSON representation in the request body. Values for `uuid` and `path` are required, `attributes` and `comment` are optional. The attribute keys must be valid part attributes as specified in the <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>.

{{ site.images['info'] }} The comment is only added if versioning is enabled in the server settings.
{% endcapture %}

{% assign exampleCaption="Adding the 'metal part' part with the uuid 05040c4c-f0af-46b8-810e-30c0c00a379e" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
    "path": "/metal part",
    "attributes": { "1001": "4466", "1003": "mp" }       
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


{% assign linkId="inspectionPlanEndpointUpdateParts" %}
{% assign method="PUT" %}
{% assign endpoint="/parts" %}
{% assign summary="Updates parts" %}
{% capture description %}

If you update a part you might want to:

* Rename/move parts or
* change attributes of parts.

{{site.images['info']}} If versioning is activated on the server side, every update creates a new version entry.

If versioning is set to 'Controlled by the client' on server side it can be done by adding the following parameter:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>bool</code> versioningEnabled</nobr><br><i>default:</i> <code>false</code>| Determines whether a version entry should be created or not. This only effects the query if versioning is set to 'Controlled by the client' on the server side.
{% endcapture %}

{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Change the metal part's attributes" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "/metal part",
     "attributes": { "1001": "4469", "1003": "metalpart" }       
     "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
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

{% assign linkId="inspectionPlanEndpointDeleteParts" %}
{% assign method="DELETE" %}
{% assign endpoint="/parts" %}
{% assign summary="Deletes parts" %}
{% capture description %}
There are two ways to delete parts, either by their path or by their uuids. This means that either the filter parameter `partPath` or `partUuids` has to be set: 

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code> partUuids<br></nobr>                              | Restricts the query to the parts with these uuids.
<nobr><code>Path</code> partPath</nobr>                                        | Restricts the query to the part with this path.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

In both cases the request deletes the part itself as well as all its child parts and child characteristics. If both parameters are set only the `partUuids` parameter will be considered.

{% endcapture %}

{% assign exampleCaption="Delete the part 'metal part' and its children." %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/parts?partPath=/metal%20part HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointDeletePart" %}
{% assign method="DELETE" %}
{% assign endpoint="/parts/:partUuid" %}
{% assign summary="Delete a part by its :partUuid" %}
{% capture description %}
Deleting a part also deletes all its children.
{% endcapture %}

{% assign exampleCaption="Delete the part 'metal part' and all entities beneath it by the part's guid" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/parts/05040c4c-f0af-46b8-810e-30c0c00a379e HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


<p></p>



####Characteristics

{% assign linkId="inspectionPlanEndpointGetAllChars" %}
{% assign method="GET" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Fetches characteristics" %}
{% capture description %}

You can fetch all characteristics or only the characteristics described by the uri parameters. Possible filter uri parameters are:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code>partUuids<br></nobr>                               | Restricts the query to the parts with these uuids.
<nobr><code>Path</code> partPath</nobr>                                        | Restricts the query to the part with this path.
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>65.536</code>  | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part or characteristic.
<nobr><code>bool</code> withHistory</nobr><br><i>default:</i> <code>false</code>| Determines whether the version history should be fetched or not. This only effects the query if versioning is activated on the server side.
<nobr><code>All, None, ID list</code> requestedCharacteristicAttributes</nobr><br><i>default:</i> <code>All</code>                                                                                  | Restricts the query to the attributes that should be returned for characteristics, for example `requestedCharacteristicAttributes={2001, 2101}`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{{ site.images['info'] }}You can only request direct characteristics of the part, characteristics of child parts will be ignored. 

{% endcapture %}
{% assign exampleCaption="Fetch all characteristics beneath the part '/metal part' until depth=2" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/characteristics?partPath=/metal%20part&depth=2 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
   {
        "path": "PC:/metal part/deviation_3/",
        "attributes": { ... },
        "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7",
        "version": 0,
        "timestamp": "2012-11-19T10:48:32.887Z"
    },
   {
        "path": "PCC:/metal part/deviation_3/.X/",
        "attributes": { ... },
        "uuid": "51c8568a-9410-465a-a8ed-33063db41dac",
        "version": 0,
        "timestamp": "2015-03-24T08:17:28.03Z"
    },
    {
        "path": "PCC:/metal part/deviation_3/.Y/",
        "attributes": { ... },
        "uuid": "b7a30736-6e89-4dd5-9bc0-e6cb9eb5e2da",
        "version": 0,
        "timestamp": "2015-03-24T08:17:34.61Z"
    },
    {
        "path": "PCC:/metal part/deviation_3/.Z/",
        "attributes": { ... },
        "uuid": "1175919c-5c59-487e-a0fb-deac04510046",
        "version": 0,
        "timestamp": "2015-03-24T08:17:38.423Z"
    }
]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointGetChar" %}
{% assign method="GET" %}
{% assign endpoint="/characteristics/:charUuid" %}
{% assign summary="Fetches a certain characteristic by its :charUuid" %}
{% capture description %}
The result of fetching a certain characteristic by its :charUuid can be restricted by the following uri parameters:

{% capture table %}
Parameter name                                                                  | Description
--------------------------------------------------------------------------------|--------------------------------
<nobr><code>bool</code> withHistory</nobr><br><i>default:</i> <code>false</code>| Determines whether the version history should be fetched or not. This only effects the query if versioning is activated on the server side.
<nobr><code>All, None, ID list</code> requestedCharacteristicAttributes</nobr><br><i>default:</i> <code>All</code>                                                                                  | Restricts the query to the attributes that should be returned for characteristics, for example `requestedCharacteristicAttributes={2001, 2101}`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}
{% endcapture %}

{% assign exampleCaption="Fetch the characteristic '/metal part/deviation_3' by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/characteristics/27e23a7c-dbe7-4863-8461-6abf7b03ddd7 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
   {
        "path": "PC:/metal part/deviation_3/",
        "attributes": { ... },
        "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7",
        "version": 0,
        "timestamp": "2012-11-19T10:48:32.887Z"
    }
]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointAddChars" %}
{% assign method="POST" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Creates characteristics" %}
{% capture description %}

To create characteristics, you must send a JSON representation of the characteristics in the request body. Values for `uuid` and `path` are required, `attributes` and `comment` are optional. The attribute keys must be valid characteristic attributes as specified in the <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>.

{{ site.images['info'] }} The comment is only added if versioning is enabled in the server settings.
{% endcapture %}

{% assign exampleCaption="Adding the characteristic 'metal part/deviation_3'" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/characteristics HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "PC:/metal part/deviation_3/",
     "attributes": 
      { 
         "2004": "3",
         "2101": "0",
         "2110": "-0.5",
         "2111": "0.5" 
      },
     "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7"
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


{% assign linkId="inspectionPlanEndpointUpdateChars" %}
{% assign method="PUT" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Updates characteristics" %}
{% capture description %}

If you update characteristics you want to:

* Rename/move characteristics or
* change attributes of characteristics.

{{site.images['info']}} If versioning is activated on server side, every update of one or more parts creates a new version entry.
{% endcapture %}

{% assign exampleCaption="Change the "metal part/deviation_3"*s attributes" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/characteristics HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "/metal part/deviation_3",
     "attributes": { "2110": "-1.0", "2111": "1.0"  }       
     "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
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

{% assign linkId="inspectionPlanEndpointDeleteCharacteristics" %}
{% assign method="DELETE" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Deletes characteristics" %}
{% capture description %}
You have two options to delete characteristics, either by their paths or by their uuids. This means that either the filter parameter `charPath` or `charUuids` has to be set: 

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code>charUuids<br></nobr>                               | Restricts the query to characteristics with these uuids.
<nobr><code>Path</code> charPath</nobr>                                        | Restricts the query to the part with this characteristics.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

In both cases the request deletes the characteristic itself as well as all its children. If both parameters are set only the `charUuids` parameter will be considered.
{% endcapture %}

{% assign exampleCaption="Delete the characteristic 'metal part/deviation_3' and all entities beneath it" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/parts?charPath=/metal%20part/deviation_3 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="inspectionPlanEndpointDeleteChar" %}
{% assign method="DELETE" %}
{% assign endpoint="/characteristics/:charUuid" %}
{% assign summary="Delete a characteristic by its :charUuid" %}
{% capture description %}
Deleting a characteristic also deletes all its children.
{% endcapture %}

{% assign exampleCaption="Delete the characteristic 'metal part/deviation_3' and all entities beneath it by its guid" %}
{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/characteristics/27e23a7c-dbe7-4863-8461-6abf7b03ddd7 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

<br/>

<h3 id="{{page.sections['dataservice']['secs']['inspectionPlan'].anchor}}-objectstructure">Object Structure</h3>

Both parts and characteristics are PiWeb inspection plan entities. They have the following properties:

{% capture table %}
Property                                          | Description
--------------------------------------------------|-----------------------
<nobr><code>Guid</code> uuid</nobr>               | Identifies this inspection plan entity uniquely
<nobr><code>string</code> path</nobr>             | The path of this entity. It consists of the path's hierarchical structure followed by the path itself, e.g. `PCC:/metal part/deviation_3/.X/`. `P` stands for part and `C` for characteristic.
<nobr><code>Attribute</code> attributes</nobr>    | A set of attributes which describe the entity
<nobr><code>string</code> comment</nobr>          | A comment which describes the last inspection plan change
<nobr><code>int</code> version</nobr>             | Contains the entity´s revision number. The revision number starts with `0` and is incremented by `1` each time changes are applied to the inspection plan. The version is only returned in case versioning is enabled in the server settings.
<nobr><code>dateTime</code> timeStamp</nobr>      | Contains the date and time of when the entity was last updated
<nobr><code>dateTime</code> charChangeDate</nobr> | *(Parts only)* The timestamp for the most recent characteristic change on any characteristic that belongs to this part
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

<h2 id="{{page.sections['dataservice']['secs']['measurementsAndValues'].anchor}}">{{page.sections['dataservice']['secs']['measurementsAndValues'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['measurementsAndValues'].anchor}}-endpoints">Endpoints</h3>

You can fetch, create, update and delete measurements and values using the following endpoints: 
<br/>

#### Measurements
{% assign linkId="measurementsGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/measurements" %}
{% assign summary="Fetches measurements" %}
{% capture description %}
You can fetch all measurements or certain measurements only. Possible filter uri parameters are: 

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>Guid list</code> measurementUuids </nobr>         | Restricts the query to these measurements <br> `measurementUuids={5b59cac7-9ecd-403c-aa26-56dd25892421}`
<nobr><code>Guid list</code> partUuids </nobr> | Restricts the query to these parts <br> `partUuids={e42c5327-6258-4c4c-b3e9-6d22c30938b2}`
<nobr><code>Path</code> partPath </nobr> | Restricts the query to this part <br><br> `partPath=/metal%20part` 
<nobr><code>bool</code> deep </nobr><br><i>default:</i> <code>false</code> | Determines whether the query should affect all levels of the inspection plan. <br> `deep=true` 
<nobr><code>OrderCriteria</code> orderBy </nobr><br><i>default:</i> <code>4 desc</code>   | Determines which attribute keys and which direction the keys should be ordered by <br> `orderBy:4 asc, 10 desc`
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ]. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`
<nobr><code>int</code> limitResult </nobr>| Restricts the number of result items. <br> `limitResult=100`
<nobr><code>All, None, Id list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All</code> | Restricts the query to the attributes that should be returned for measurements. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>None, Simple, Detailed</code> statistics </nobr><br><i>default:</i> <code>None</code> | Indicates how statistical informtaion should be returned: <br><code>None</code> = Return no information<br><code>Simple</code> = Return statistical information including numvber of characteristics out of warning limit, number of characteristics out of tolerance and number of characteristics in warning limit and tolerance<br><code>Detailed</code> = Return statistical information the same way as <code>Simple</code> plus the guid for each characteristic <br> `statistics=Simple`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be fetched. <br> `aggregation=All`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch measurements newer than 01.01.2015 for the part with the guid e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/measurements?partUuids={e42c5327-6258-4c4c-b3e9-6d22c30938b2}&searchCondition=4>[2015-01-01T00:00:00Z] HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}

 [
   {
     "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
     "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
     "lastModified": "2015-03-09T09:19:38.653Z",
     "attributes":
     {
         "4": "2015-03-09T19:12:00Z",
         "6": "3",
         "7": "0"
     }
    },
    ...
  ]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="measurementsGetOne" %}
{% assign method="GET" %}
{% assign endpoint="/measurements/:measUuid" %}
{% assign summary="Fetches a measurement by its :measUuid" %}
{% capture description %}

The request can be restricted by the following filter uri parameters: 

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>All, None, Id list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All</code> | Restricts the query to the attributes that should be returned for measurements. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>None, Simple, Detailed</code> statistics </nobr><br><i>default:</i> <code>None</code> | Indicates how statistical informtaion should be returned: <br><code>None</code> = Return no information<br><code>Simple</code> = Return statistical information including numvber of characteristics out of warning limit, number of characteristics out of tolerance and number of characteristics in warning limit and tolerance<br><code>Detailed</code> = Return statistical information the same way as <code>Simple</code> plus the guid for each characteristic <br> `statistics=Simple`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be fetched. <br> `aggregation=All`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch a measurement by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/measurements/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}

 [
   {
     "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
     "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
     "lastModified": "2015-03-09T09:19:38.653Z",
     "attributes":
     {
         "4": "2015-03-09T19:12:00Z",
         "6": "3",
         "7": "0"
     }
    }
  ]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsCreate" %}
{% assign method="POST" %}
{% assign endpoint="/measurements" %}
{% assign summary="Creates measurements" %}
{% capture description %}
To create a new measurement, you must send its JSON representation in the request body. Values for `uuid` and `path` are required, `attributes` and `comment` are optional. The attribute keys must be valid measurement attributes as specified in the <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>.

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 
{% endcapture %}
{% assign exampleCaption="Create a measurement" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "3",
        "7": "0"
      }     
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


{% assign linkId="measurementsUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/measurements" %}
{% assign summary="Updates measurements" %}
{% capture description %}
Updating a measurement does always affect the whole measurement. This means that you must send the whole measurement, including attributes and values, in the request body. The server then deletes the old measurement and creates the new one in a single transaction.
{% endcapture %}

{% assign exampleCaption="Update a measurement - add and change an attribute" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "2",
        "7": "0",
        "8": "1"
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsDelete" %}
{% assign method="DELETE" %}
{% assign endpoint="/measurements" %}
{% assign summary="Deletes measurements" %}
{% capture description %}
You have multiple options for deleting measurements measurements:

* Delete all measurements
* Delete measurements from a single part by its path
* Delete measurements from parts by its uuids
* Delete measurements by their uuids

The delete condition may be further restricted by the filter uri parameter `searchCondition`:

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ]. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Delete measurements newer than 01.01.2015 and older than 31.03.2015 from the part with the uuid e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements?partUuids={4b59cac7-9ecd-403c-aa26-56dd25892421}&searchCondition=4>[2015-01-01T00:00:00Z]+4<[2015-03-31T23:59:59Z] HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="measurementsDeleteOne" %}
{% assign method="DELETE" %}
{% assign endpoint="/measurements/:measUuid" %}
{% assign summary="Delete a measurement by its :measUuid" %}
{% assign description="" %}

{% assign exampleCaption="Delete a measurement by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}





#### Values
{% assign linkId="valuesGetAll" %}
{% assign method="GET" %}
{% assign endpoint="/values" %}
{% assign summary="Fetches measurements including measured values" %}
{% capture description %}
You can fetch all measurements with values or only certain measurements with values. Possible filter uri parameters are:

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>Guid list</code> measurementUuids </nobr>         | Restricts the query to these measurements <br> `measurementUuids={5b59cac7-9ecd-403c-aa26-56dd25892421}`
<nobr><code>Guid list</code> partUuids </nobr> | Restricts the query to these parts <br> `partUuids={e42c5327-6258-4c4c-b3e9-6d22c30938b2}`
<nobr><code>Path</code> partPath </nobr> | Restricts the query to this part <br><br> `partPath=/metal%20part` 
<nobr><code>bool</code> deep </nobr><br><i>default:</i> <code>false</code> | Determines whether the query should affect all levels of the inspection plan. <br> `deep=true` 
<nobr><code>OrderCriteria</code> orderBy </nobr><br><i>default:</i> <code>4 desc</code>   | Determines which attribute keys and which direction the keys should be ordered by <br> `orderBy:4 asc, 10 desc`
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ]. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`
<nobr><code>int</code> limitResult </nobr>| Restricts the number of result items. <br> `limitResult=100`
<nobr><code>All, None, Id list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All</code> | Restricts the query to the attributes that should be returned for measurements. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>All, None, Id list</code> requestedValueAttributes </nobr><br><i>default:</i> <code>All</code> |List of attributes that should be returned for values. <br><br> `requestedValueAttributes={1,8}`
<nobr><code>Guid list</code> characteristicUuids </nobr> | Restricts the query to the characteristics for which values should be returned. <br> `characteristicsUuidList={525d15c6-dc70-4ab4-bd3c-8ab2b5780e6b, 8faae7a0-d1e1-4ee2-b3a5-d4526f6ba822}`
<nobr><code>None, Simple, Detailed</code> statistics </nobr><br><i>default:</i> <code>None</code> | Indicates how statistical informtaion should be returned: <br><code>None</code> = Return no information<br><code>Simple</code> = Return statistical information including numvber of characteristics out of warning limit, number of characteristics out of tolerance and number of characteristics in warning limit and tolerance<br><code>Detailed</code> = Return statistical information the same way as <code>Simple</code> plus the guid for each characteristic <br> `statistics=Simple`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be fetched. <br> `aggregation=All`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch measurements and values newer than 01.01.2015 restricted to a certain characteristic for a part restricted by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataservicerest/values?partUuids={05040c4c-f0af-46b8-810e-30c0c00a379e}&searchCondition=4>[2010-11-04T00:00:00Z]&characteristicUuids={b587d548-8aa6-42b7-b292-0f3e13452c3f} HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}

 [
   {
    "characteristics":
         {
             "b587d548-8aa6-42b7-b292-0f3e13452c3f":
             {
                 "1": "-0.073420455529934786"
             }
         },
         "uuid": "88974561-a449-4a94-8b3e-970822b84406",
         "partUuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
         "lastModified": "2015-01-19T10:48:34.157Z",
         "attributes":
         {
             "4": "2010-11-05T20:30:57.6Z",
             "6": "5",
             "7": "4",
             "8": "7",
             "12": "4"
         }
    },
    ...
  ]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="valuesGetOne" %}
{% assign method="GET" %}
{% assign endpoint="/values/:measUuid" %}
{% assign summary="Fetches a measurement including measured values by its :measUuid" %}
{% capture description %}
The request can be restricted by the following uri filter parameters: Possible filters are: 

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>All, None, Id list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All</code> | Restricts the query to the attributes that should be returned for measurements. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>All, None, Id list</code> requestedValueAttributes </nobr><br><i>default:</i> <code>All</code> |List of attributes that should be returned for values. <br><br> `requestedValueAttributes={1,8}`
<nobr><code>Guid list</code> characteristicUuids </nobr> | Restricts the query to the characteristics for which values should be returned. <br> `characteristicsUuidList={525d15c6-dc70-4ab4-bd3c-8ab2b5780e6b, 8faae7a0-d1e1-4ee2-b3a5-d4526f6ba822}`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be fetched. <br> `aggregation=All`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Fetch a measurement including  all values by its guid" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/values/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}

 [
   {
    "characteristics":
     {
         "b587d548-8aa6-42b7-b292-0f3e13452c3f":
         {
             "1": "-0.073420455529934786"
         },
         ...
     },
     "uuid": "5b59cac7-9ecd-403c-aa26-56dd25892421",
     "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
     "lastModified": "2015-03-09T09:19:38.653Z",
     "attributes":
     {
         "4": "2015-03-09T19:12:00Z",
         "6": "3",
         "7": "0"
     }
    },
    ...
  ]

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="valuesCreate" %}
{% assign method="POST" %}
{% assign endpoint="/values" %}
{% assign summary="Creates measurements including values" %}
{% capture description %}
To create a measurement with values, you must send its JSON representation in the request body. Values for `uuid` and `path` are required, `attributes` and `comment` are optional. The attribute keys must be valid measurement attributes as specified in the <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>.

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings. 
{% endcapture %}
{% assign exampleCaption="Create a measurement with measured values" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/values HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "3",
        "7": "0"
      },
      "characteristics":
      {
         "360f55e5-77c3-49f9-9a5e-80d0a9040e2d":
         {
             "1": "0.24966522"
         },
         "b5c98235-c75c-41a4-aced-2a38c70a3866":
         {
             "1": "0.4457339"
         },
         "85bbb406-810e-4062-8a9f-c7b636cb61bd":
         {
             "1": "0.24981162"
         }
      }
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


{% assign linkId="valuesUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/values" %}
{% assign summary="Updates measurements and values" %}
{% capture description %}
Updating a measurement does always affect the whole measurement. This means that you must send the whole measurement, including attributes and values, in the request body. The server then deletes the old measurement and creates the new one in a single transaction.
{% endcapture %}

{% assign exampleCaption="Update a measurement - change a measured value" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/measurements HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "uuid": "4b59cac7-9ecd-403c-aa26-56dd25892421",
      "partUuid": "e42c5327-6258-4c4c-b3e9-6d22c30938b2",
      "attributes": {
        "4": "2015-03-09T19:12:00Z",
        "6": "2",
        "7": "0",
        "8": "1"
      }
      "characteristics":
      {
         "360f55e5-77c3-49f9-9a5e-80d0a9040e2d":
         {
             "1": "0.24966522"
         },
         "b5c98235-c75c-41a4-aced-2a38c70a3866":
         {
             "1": "0.4467339"
         },
         "85bbb406-810e-4062-8a9f-c7b636cb61bd":
         {
             "1": "0.25981162"
         }
      }
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


<h3 id="{{page.sections['dataservice']['secs']['measurementsAndValues'].anchor}}-objectStructure">Object Structure</h3>

Measurements do always belong to a single inspection plan part. Depending on the purpose, the measured values are included within a measurement or not. Each measurement has the following properties:

{% capture table %}
Property                                                       | Description
---------------------------------------------------------------|------------------------------------------------------------------------------------
<nobr><code>Guid</code> uuid</nobr>                            | Identifies the measurement uniquely.
<nobr><code>Guid</code> partUuid</nobr>                        | The uuid of the part the measurement belongs to.
<nobr><code>Attribute[]</code> attributes</nobr>               | A set of attributes which specifies this measurement.
<nobr><code>DateTime</code> lastModified</nobr>                | Contains the date and time of the last update applied to this measurement.
<nobr><code>DataCharacteristic[]</code> characteristics</nobr> | An array of the characteristics which has been measured within the measurement. Each characteristic within this array consits of the uuid it is identified by and an array of attributes which include at least the measured value attribute.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
