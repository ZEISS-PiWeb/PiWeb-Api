<h2 id="{{page.sections['dataservice']['secs']['inspectionPlan'].anchor}}">{{page.sections['dataservice']['secs']['inspectionPlan'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['inspectionPlan'].anchor}}-endpoints">Endpoints</h3>

You can fetch, create, update and delete parts and characteristics using the following endpoints:

#### Parts

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
<nobr><code>Condition</code> searchCondition </nobr>                              | The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]` <br>
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

#### Versioning
<hr>

If versioning is enabled, you can fetch the parts history using the parameter `withHistory=true`:

##### Request
{% highlight http %}
GET /dataServiceRest/parts?partPath=/PartWithVersioning&withHistory=true HTTP/1.1
{% endhighlight %}

##### Response
{% highlight json %}

[
    {
        "attributes": {
            "1001": "1758_e3"
        },
        "path": "P:/PartWithVersioning_edit/",
        "history": [
            {
                "attributes": {},
                "path": "P:/PartWithVersioning/",
                "charChangeDate": "2021-01-06T09:18:02.12Z",
                "uuid": "0c2c5bd1-4372-439c-9cdb-726841981daa",
                "comment": "[John Doe]: Part 'PartWithVersioning' created",
                "version": 415,
                "timestamp": "2021-01-06T09:18:02.12Z"
            }
        ],
        "charChangeDate": "2021-01-06T09:18:02.12Z",
        "uuid": "0c2c5bd1-4372-439c-9cdb-726841981daa",
        "comment": "[John Doe]: Part number Changed",
        "version": 421,
        "timestamp": "2021-01-06T09:54:31.583Z"
    }
]

{% endhighlight %}

When using this parameter each part will contain a history, where all previous changes of this part are stored. Each entry of the history contains the old values of the part at this time, and a comment with information about
why and by whom the change was made. In above example we have one history entry, stating that the part 'PartWithVersioning' was created by user John Doe. Attributes where still empty. The current state contains the attribute
1001, and the comment further down tells us that someone changed it (1001 equals attribute 'Part number'). If we add another attribute the history will extend with the current values, the set of history entries will now contain two entries. Please note that the version number is global for the PiWeb Server, so gaps simply mean that another part was edited in the meantime, and this is not the 415th version of the part.

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

{
    "path": "P:/metal part/",
    "charChangeDate": "2014-11-19T10:48:32.917Z",
    "attributes": {},
    "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
    "version": 0,
    "timestamp": "2012-11-19T10:48:32.887Z"
}

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
    "path": "P:/metal part/",
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

If versioning is activated on the server side, every update creates a new version entry. You should add a value for `comment` to explain why it was changed and by whom.
The optimal format is "[Username]:Reason of change", e.g. "[John Doe]: Renamed 'Part1' to 'Part2'". This way it will be correctly displayed in PiWeb Planner.
Please note that the JSON property `comment` can be added to different parts of the PUT request, however only one will be used by the PiWeb Server for the version entry.
The `comment` of the first entry with actual changes will be used.

{{site.images['info']}} If versioning is set to 'Controlled by the client' on server side it can be controlled by the following parameter:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>bool</code> versioningEnabled</nobr><br><i>default:</i> <code>false</code>| Determines whether a version entry should be created or not. This only effects the query if versioning is set to 'Controlled by the client' on server side.
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
     "path": "P:/metal part/",
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

{{ site.images['info'] }} If option "Parts with measurement data can be deleted" is deactivated parts containing measurements will not be deleted.

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

{{ site.images['info'] }} If option "Parts with measurement data can be deleted" is deactivated parts containing measurements will not be deleted.
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


{% assign linkId="inspectionPlanEndpointClearPart" %}
{% assign method="POST" %}
{% assign endpoint="/parts/:partUuid/clear?keep=subParts" %}
{% assign summary="Clears a part" %}
{% capture description %}

To clear a part, you need to specify it by its uuid via uri segment. Clearing a part results in keeping the given part as well as its raw data and deleting all subParts, measurements, values and all other raw data. If optional `keep` parameter is set to value `subParts` subPart (including child items, measurements, values and raw data) are not deleted as well.

{% endcapture %}

{% assign exampleCaption="Clearing the 'metal part' part with the uuid 05040c4c-f0af-46b8-810e-30c0c00a379e but keep all subParts." %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/parts/05040c4c-f0af-46b8-810e-30c0c00a379e/clear?keep=subParts HTTP/1.1
{% endhighlight %}

{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="inspectionPlanEndpointCountParts" %}
{% assign method="GET" %}
{% assign endpoint="/parts/count" %}
{% assign summary="Get the count of parts" %}
{% capture description %}
Get the number of parts matching specified criteria.
The result can be restricted by the following uri parameters:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code> partUuids<br></nobr>                              | Restricts the query to the parts with these uuids.
<nobr><code>Path</code> partPath</nobr>                                        | Restricts the query to the part with this path.
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>1</code>       | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part.
<nobr><code>Condition</code> searchCondition </nobr>                              | The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]` <br>
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Count the parts under '/metal part' to a depth of 50" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/parts/count?partPath=/metal%20part&depth=50 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{
    "count": 4
}

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


<p></p>



#### Characteristics

{% assign linkId="inspectionPlanEndpointGetAllChars" %}
{% assign method="GET" %}
{% assign endpoint="/characteristics" %}
{% assign summary="Fetches characteristics" %}
{% capture description %}

You can fetch all characteristics or only the characteristics described by the uri parameters. Possible filter uri parameters are:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code> charUuids<br></nobr>                              | Restricts the query to the characteristics with these uuids.
<nobr><code>Path</code> partPath</nobr><br><i>default:</i> <code>/</code>      | Restricts the query to the part with this path. The <code>charUuids</code> parameter takes precedence over this parameter.
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>65.535</code>  | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part or characteristic.
<nobr><code>bool</code> withHistory</nobr><br><i>default:</i> <code>false</code>| Determines whether the version history should be fetched or not. This only effects the query if versioning is activated on the server side.
<nobr><code>All, None, ID list</code> requestedCharacteristicAttributes</nobr><br><i>default:</i> <code>All</code>                                                                                  | Restricts the query to the attributes that should be returned for characteristics, for example `requestedCharacteristicAttributes={2001, 2101}`
<nobr><code>Condition</code> searchCondition </nobr>                              | The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]` <br>
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{{ site.images['info'] }} You can only request direct characteristics of the part, characteristics of child parts will be ignored.

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

Versioning works the same way as with parts, described in the first endpoint [GET /parts](#ds-inspection-plan).

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

{
    "path": "PC:/metal part/deviation_3/",
    "attributes": { ... },
    "uuid": "27e23a7c-dbe7-4863-8461-6abf7b03ddd7",
    "version": 0,
    "timestamp": "2012-11-19T10:48:32.887Z"
}

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

If versioning is activated on the server side, every update creates a new version entry. You should add a value for `comment` to explain why it was changed and by whom.
The optimal format is "[Username]:Reason of change", e.g. "[John Doe]: Renamed 'Characteristic1' to 'Characteristic2'". This way it will be correctly displayed in PiWeb Planner.
Please note that the JSON property `comment` can be added to different characteristics of the PUT request, however only one will be used by the PiWeb Server for the version entry.
The `comment` of the first entry with actual changes will be used.

{{site.images['info']}} If versioning is set to 'Controlled by the client' on server side it can be controlled by the following parameter:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>bool</code> versioningEnabled</nobr><br><i>default:</i> <code>false</code>| Determines whether a version entry should be created or not. This only effects the query if versioning is set to 'Controlled by the client' on server side.
{% endcapture %}

{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Change the metal part/deviation_3's attributes" %}
{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/characteristics HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "PC:/metal part/deviation_3/",
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
DELETE /dataServiceRest/characteristics?charPath=/metal%20part/deviation_3 HTTP/1.1
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

{% assign linkId="inspectionPlanEndpointCountChars" %}
{% assign method="GET" %}
{% assign endpoint="/characteristics/count" %}
{% assign summary="Get the count of characteristics" %}
{% capture description %}
Get the number of characteristics matching specified criteria.
The result can be restricted by the following uri parameters:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>Guid list</code> charUuids<br></nobr>                              | Restricts the query to the characteristics with these uuids.
<nobr><code>Path</code> partPath</nobr><br><i>default:</i> <code>/</code>      | Restricts the query to the part with this path. The <code>charUuids</code> parameter takes precedence over this parameter.
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>65.535</code>  | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part or characteristic.
<nobr><code>Condition</code> searchCondition </nobr>                              | The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]` <br>
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Count the characteristics under '/metal part' to a depth of 5" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/characteristics/count?partPath=/metal%20part&depth=5 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{
    "count": 87
}

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

#### Inspection Plan Items

Inspection plan items are not entities by itself. Instead, the following endpoints can be used to fetch a hierarchical structure of parts and characteristics at the same time.

{% assign linkId="inspectionPlanEndpointGetAllItems" %}
{% assign method="GET" %}
{% assign endpoint="/inspectionPlanItems" %}
{% assign summary="Fetches parts and characteristics" %}
{% capture description %}
You can fetch all inspection plan items or only certain ones. Possible filter uri parameters are:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>65.535</code>  | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part or characteristic.
<nobr><code>Path list</code> paths</nobr><br><i>default:</i> <code>/</code>    | Restricts the query to items with the given paths. This parameter is only valid if <code>parentUuids</code> parameter is null or empty.
<nobr><code>Guid list</code> parentUuids<br></nobr>                            | Restricts the query to children of items with these uuids. This parameter is only valid if <code>paths</code> parameter is null or empty.
<nobr><code>Guid list</code> itemUuids<br></nobr>                              | Restricts the query to the inspection plan items (parts or characteristics) with these uuids.
<nobr><code>All, None, ID list</code> requestedPartAttributes</nobr><br><i>default:</i> <code>All</code>            | Restricts the query to the attributes that should be returned for parts, for example `requestedPartAttributes={1001, 1008}`.
<nobr><code>All, None, ID list</code> requestedCharacteristicAttributes</nobr><br><i>default:</i> <code>All</code>  | Restricts the query to the attributes that should be returned for characteristics, for example `requestedCharacteristicAttributes={2001, 2101}`.
<nobr><code>bool</code> withHistory</nobr><br><i>default:</i> <code>false</code>| Determines whether the version history should be fetched or not. This only effects the query if versioning is activated on the server side.
<code>Part, Characteristic, CharacteristicWithoutIntermediateParts, None</code> restrictToEntity<br><i>default:</i> <code>None</code>| Determine whether the inspection plan search should be limited to parts, characteristics, or not.
<nobr><code>Condition</code> searchCondition </nobr>                              | The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]` <br><br>`restrictToEntity` has to be set for the search condition to work. If `restrictToEntity` is not set the value `Part` is assumed. This means you can filter only by one kind of attributes, either `Part` or `Characteristic`. 

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch all inspection plan items beneath the part '/metal part'" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/inspectionPlanItems?paths=/Metal%20part HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
    {
        "charChangeDate": "2025-05-20T12:20:30.167Z",
        "uuid": "9d3a551c-fd0e-4885-8039-9237b5de50fb",
        "comment": "",
        "path": "P:/Metal part/",
        "version": 0,
        "timestamp": "2025-04-22T08:32:40.907Z",
        "attributes": {}
    },
    {
        "uuid": "a90d9ed0-f115-4626-8bf8-7e2bb19b06b3",
        "comment": "",
        "path": "PC:/Metal part/Curve_form1/",
        "version": 0,
        "timestamp": "2025-04-22T08:32:41.243Z",
        "attributes": {
            ...
            "12009": "CharacteristicsCurveForm"
        }
    },
    ...
]

{% endhighlight %}

Versioning works the same way as with parts, described in the first endpoint [GET /parts](#ds-inspection-plan).

{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="inspectionPlanEndpointCountItems" %}
{% assign method="GET" %}
{% assign endpoint="/inspectionPlanItems/count" %}
{% assign summary="Get the count of inspection plan items" %}
{% capture description %}
Get the number of items matching specified criteria.
The result can be restricted by the following uri parameters:

{% capture table %}
Parameter name                                                                 | Description
-------------------------------------------------------------------------------|--------------------------------
<nobr><code>ushort</code> depth</nobr><br><i>default:</i> <code>65.535</code>  | Determines how many levels of the inspection plan tree hierarchy should be fetched. Setting `depth=0` means that only the entity itself should be fetched, `depth=1` means the entity and its direct children should be fetched. Please note that depth is treated relative of the path depth of the provided part or characteristic.
<nobr><code>Path list</code> paths</nobr><br><i>default:</i> <code>/</code>    | Restricts the query to items with the given paths. This parameter is only valid if <code>parentUuids</code> parameter is null or empty.
<nobr><code>Guid list</code> parentUuids<br></nobr>                            | Restricts the query to children of items with these uuids. This parameter is only valid if <code>paths</code> parameter is null or empty.
<nobr><code>Guid list</code> itemUuids<br></nobr>                              | Restricts the query to the inspection plan items (parts or characteristics) with these uuids.
<code>Part, Characteristic, CharacteristicWithoutIntermediateParts, None</code> restrictToEntity<br><i>default:</i> <code>None</code>| Determine whether the inspection plan search should be limited to parts, characteristics, or not.
<nobr><code>Condition</code> searchCondition </nobr>                              | The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]` <br><br>`restrictToEntity` has to be set for the search condition to work. If `restrictToEntity` is not set the value `Part` is assumed. This means you can filter only by one kind of attributes, either `Part` or `Characteristic`.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Count the items under '/Metal part'" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/inspectionPlanItems/count?paths=/Metal%20part HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{
    "count": 42
}

{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

<br/>

<h3 id="{{page.sections['dataservice']['secs']['inspectionPlan'].anchor}}-objectstructure">Object Structure</h3>

Both parts and characteristics are PiWeb inspection plan entities. They have the following properties:

{% capture table %}
Property                                          | Description
--------------------------------------------------|-----------------------
<nobr><code>Guid</code> uuid</nobr>               | Identifies this inspection plan entity uniquely.
<nobr><code>string</code> path</nobr>             | The path of this entity. It consists of the path's hierarchical structure followed by the path itself, e.g. `PCC:/metal part/deviation_3/.X/`. `P` stands for part and `C` for characteristic. The path always ends with `/`.
<nobr><code>Attribute</code> attributes</nobr>    | A set of attributes which describe the entity.
<nobr><code>string</code> comment</nobr>          | A comment which describes the last inspection plan change. The comment is only returned in case versioning is enabled in the server settings.
<nobr><code>int</code> version</nobr>             | Contains the entity´s revision number. The revision number starts with `0` and is incremented by `1` each time changes are applied to the inspection plan.
<nobr><code>dateTime</code> timeStamp</nobr>      | Contains the date and time of when the entity was last updated.
<nobr><code>dateTime</code> charChangeDate</nobr> | *(Parts only)* The timestamp for the most recent characteristic change on any characteristic that belongs to this part
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
