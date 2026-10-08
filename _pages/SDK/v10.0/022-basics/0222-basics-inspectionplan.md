<h2 id="{{page.sections['basics']['secs']['inspectionPlan'].anchor}}">{{page.sections['basics']['secs']['inspectionPlan'].title}}</h2>

Examples in this section:
+ [Using the PathHelper class](#-example--using-the-pathhelper-class)
+ [Creating a part with a subpart](#-example--creating-a-part-with-a-subpart)
+ [Fetching different entities](#-example--fetching-different-entities)
+ [Creating characteristics for the part “MetalPart”](#-example--creating-characteristics-for-the-part-metalpart)
+ [Deleting “MetalPart”](#-example--deleting-metalpart)
<hr>

An inspection plan object contains entities of two different types - parts and characteristics. Parts are hold in class `SimplePartDto`, characteristics are hold in class `InspectionPlanCharacteristicDto`. Both are derived from the abstract base class `InspectionPlanDtoBase` and consist of the following properties:

<img src="/PiWeb-Api/images/v6/inspection-plan-schema.png" class="img-responsive center-block">

#### InspectionPlanDtoBase

{% capture table %}
Property                                                                | Description
------------------------------------------------------------------------|----------------------------------------------
<nobr><code>IReadOnlyList&lt;Attribute&gt;</code> Attributes</nobr>     | A set of attributes which describes the entity.
<nobr><code>string</code> Comment</nobr>                                | A comment which describes the last inspection plan change. The comment is only returned if versioning is enabled in the server settings.
<nobr><code>PathInformation</code> Path</nobr>                       | The path of this entity which describes the entity's hierarchical structure.
<nobr><code>string</code> this[ushort key]</nobr>                       | Indexer for accessing entity's attribute value directly with the specified key
<nobr><code>DateTime</code> TimeStamp</nobr>                            | Contains the date and time of when the entity was last updated.
<nobr><code>Guid</code> Uuid</nobr>                                     | Identifies this inspection plan entity uniquely.
<nobr><code>uint</code> Version</nobr>                                  | Contains the entity´s revision number. The revision number starts with `0` and is globally incremented by `1` each time changes are applied to the inspection plan.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

>{{ site.images['info'] }} The version is only updated if versioning is enabled in server settings. Every update creates a new version entry. You should add a value for `Comment` to explain why it was changed and by whom.
The optimal format is "[Username]:Reason of change", e.g. "[John Doe]: Renamed 'Part1' to 'Part2'". This way it will be correctly displayed in PiWeb Planner.

>{{ site.images['info'] }} The version/revision of a part or characteristic is global. This means that the version counter is the same for every entity in the inspection plan. A part with a version of 34 did not necessarily change 34 times but the version indicates that the 34th change in the whole inspection plan was done to this entity.

#### SimplePartDto

{% capture table %}
Property                                          | Description
--------------------------------------------------|----------------------------------------------------------------
<nobr><code>DateTime</code> CharChangeDate</nobr> | The timestamp for the most recent characteristic change on any characteristic below that part (but not below sub parts). This timestamp is updated by the server backend.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Parts as well as characteristic may contain a version history if versioning is enabled in server settings. If enabled, parts are represented by class `InspectionPlanPartDto` which is derived from `SimplePartDto`:

#### InspectionPlanCharacteristicDto, InspectionPlanPartDto

{% capture table %}
Property                                               | Description
-------------------------------------------------------|-----------------------------------------------------
<nobr><code>IReadOnlyList&lt;InspectionPlanBase&gt;</code> History</nobr> | The version history for this inspection plan entity.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### PathInformation
The inspection plan is organized in a tree structure but saved to the database in a flat representation. Path information helps to convert from flat to tree structure.
A `PathInformation` object includes an array of entity's path elements. These path elements contain the following properties:

#### PathElementDto

{% capture table %}
Property                                               | Description
-------------------------------------------------------|-----------------------------------------------------
<nobr><code>InspectionPlanEntityDto</code> Type</nobr> | Type of the path element (Part or Characteristic)
<nobr><code>String</code> Value</nobr>                 | Path elments' name

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

>{{ site.headers['bestPractice'] }} To create a `PathInformation` object you might use the `PathHelper` class which includes several helper methods:

{% capture table %}
Method                                                                                                   | Description
---------------------------------------------------------------------------------------------------------|-----------------------------------------------------
<nobr><code>PathInformation RoundtripString2PathInformation( string path )</code></nobr>              | Creates a path information object based on `path` parameter in roundtrip format ("structure:database path")
<nobr><code>PathInformation String2PartPathInformation( string path )</code></nobr>                   | Creates a path information object based on `path` parameter including plain part structure
<nobr><code>PathInformation String2CharacteristicPathInformation( string path )</code></nobr>         | Creates a path information object based on `path` parameter including plain characteristic structure
<nobr><code>PathInformation DatabaseString2PathInformation( string path, string structure)</code></nobr>| Creates a path information object based on `path` and `structure` parameter

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

#### Examples

>{{ site.images['info'] }} `DataServiceClient` in examples refers to an actual instance of `DataServiceRestClient` pointing to a server.

{{ site.headers['example'] }} Using the `PathHelper` class

{% highlight csharp %}
var characteristicPath = PathHelper.RoundtripString2PathInformation( "PPC:/MetalPart/SubPart/Char1/" );
{% endhighlight %}

In the above example a simple path is created using our PathHelper.RoundtripString2PathInformation method. The method needs the path in [roundtrip format](https://en.wikipedia.org/wiki/Round-trip_format_conversion), consisting of structure and path.

The structure is a list of the letters *P* and *C*, which are the short form of *part* and *characteristic*. The list is ordered according to the occurring types of entities in the following path string. The example structure is `PPC`, standing for `/Part/Part/Characteristic/`, which matches the types of `/MetalPart/SubPart/Char1/` in the exact order. A path needs to end with `/`.

>{{ site.images['info'] }} Please note that a part or characteristic must not contain a backslash `\`.

{{ site.headers['example'] }} Creating a part with a subpart

{% highlight csharp %}
//Create a new part
var parentPart = new InspectionPlanPartDto
{
	Uuid = Guid.NewGuid(),
	Path = PathHelper.RoundtripString2PathInformation("P:/MetalPart/"),
};
//Create a new part that represents a child of the parentPart
var childPart = new InspectionPlanPartDto
{
	Uuid = Guid.NewGuid(),
	Path = PathHelper.RoundtripString2PathInformation("PP:/MetalPart/SubPart/"),
};

//Create parts on the server
await DataServiceClient.CreateParts( new[] { parentPart, childPart } );
{% endhighlight %}
The name of the part is specified within its path. Nesting is easy as you just create the path and structure according to your desired hierarchy. Remember again that characteristics cannot contain parts.

>{{ site.headers['bestPractice'] }} Create or update multiple entities in a single call
To achieve a good performance, it is highly recommended to create or update items in a single call. That is why all create and update methods expect an array parameter.

{{ site.headers['example'] }} Creating characteristics for the part "MetalPart"

{% highlight csharp %}
//Create characteristics for MetalPart
var char1Path = PathHelper.RoundtripString2PathInformation( "PC:/MetalPart/Char1/" );
var char2Path = PathHelper.RoundtripString2PathInformation( "PC:/MetalPart/Char2/" );
//Create characteristic for SubPart
var char3Path = PathHelper.RoundtripString2PathInformation( "PPC:/MetalPart/SubPart/Char3/" );

//Create InspectionPlanCharacteristicDto objects
var char1 = new InspectionPlanCharacteristicDto { Path = char1Path, Uuid = Guid.NewGuid() };
var char2 = new InspectionPlanCharacteristicDto { Path = char2Path, Uuid = Guid.NewGuid() };
var char3 = new InspectionPlanCharacteristicDto { Path = char3Path, Uuid = Guid.NewGuid() };

//Use Client to create characteristics on server
await DataServiceClient.CreateCharacteristics( new[] {char1, char2, char3} );
{% endhighlight %}

{{ site.headers['example'] }} Fetching different entities

{% highlight csharp %}
//Create PathInformation of "MetalPart"
var partPath = PathHelper.String2PartPathInformation( "/MetalPart/" );

//Fetch all parts below "MetalPart"
var parts = await DataServiceClient.GetParts( partPath );

//Fetch all characteristics below "MetalPart"
var characteristics = await DataServiceClient.GetCharacteristics( partPath );
{% endhighlight %}
You can fetch different entities with the corresponding method. The path specifies the entry point from where you want to fetch data. Setting the search depth is also possible, the default value is null, which is equal to an unlimited search and returns all characteristics of the part with their child characteristics. Please note that this does not include characteristics of subparts, to fetch these you have to use the according subpart-path.

If versioning is enabled on server side, you can specify the parameter `withHistory` when fetching entities.
If set to `true` each fetched entity will contain a history, where all previous changes of this entity are stored. Each entry of the history contains the old values of the entity at this time, and a comment with information about
why and by whom the change was made.

>{{ site.headers['knownLimitation'] }} Missing filter possibilities
Not all endpoints provide extensive filter possibilities, as it is much more performant to filter on client side. This reduces additional workload for the server.

{{ site.headers['example'] }} Deleting "MetalPart"

{% highlight csharp %}
//Create PathInformation of "MetalPart"
var partPath = PathHelper.String2PartPathInformation("/MetalPart/");

//Get the part from server, depth 0 to only get the exact part and no children
var parts = await DataServiceClient.GetParts(partPath, depth:0);
//Get the part from the returned array (first entry in our case)
var metalPart = parts.First();

//Delete the part by its Uuid
await DataServiceClient.DeleteParts(new[] { metalPart.Uuid });
{% endhighlight %}

>{{ site.images['info'] }} Deleting a part also deletes its subparts, characteristics and measurements. This is only possible if the server setting *"Parts with measurement data can be deleted"* is activated.
