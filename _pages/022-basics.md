<h2 id="{{page.sections['basics']['secs']['inspectionPlan'].anchor}}">{{page.sections['basics']['secs']['inspectionPlan'].title}}</h2>

An inspection plan object contains entities of two different types - parts and characteristics. Parts are hold in class `SimplePart`, characteristics are hold in class `InspectionPlanCharacteristic`. Both are derived from the abstract base class `InspectionPlanBase` and consists of the following properties:

#### `InspectionPlanBase`

{% capture table %}
Property                                          | Description
--------------------------------------------------|--------------------------------------------------------------------
<nobr><code>Guid</code> Uuid</nobr>               | Identifies this inspection plan entity uniquely.
<nobr><code>PathInformation</code> Path</nobr>    | The path of this entity which describes the entity's hierarchical structure.
<nobr><code>Attribute[]</code> Attributes</nobr>  | A set of attributes which describe the entity.
<nobr><code>string</code> Comment</nobr>          | A comment which describes the last inspection plan change. The comment is only returned in case versioning is enabled in the server settings.
<nobr><code>uint</code> Version</nobr>            | Contains the entity´s revision number. The revision number starts with `0` and is incremented by `1` each time changes are applied to the inspection plan.
<nobr><code>DateTime</code> TimeStamp</nobr>      | Contains the date and time of when the entity was last updated.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

A `SimplePart` does additionally consist of the timestamp for the most recent characteristic change:

#### `SimplePart : InspectionPlanBase`

{% capture table %}
Property                                          | Description
--------------------------------------------------|----------------------------------------------------------------
<nobr><code>dateTime</code> CharChangeDate</nobr> | The timestamp for the most recent characteristic change on any characteristic below that part (but not below sub parts). This timestamp is updated by the server backend.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Parts as well as characteristic may contain a version history if versioning is enabled in server settings. If so parts are represented by class `InspectionPlanPart` which is derived from `SimplePart`.

#### `InspectionPlanCharacteristic : InspectionPlanBase` , `InspectionPlanPart : SimplePart`

{% capture table %}
Property                                               | Description
-------------------------------------------------------|-----------------------------------------------------
<nobr><code>InspectionPlanBase[]</code> History</nobr> | The version history for this inspection plan entity.

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


{{ site.headers['bestPractice'] }} Create a  `PathInformation` object for a part entity
As pointed in the upper section a `PathInformation` object includes an entity's path as well as its structure. To easily create a `PathInformation` use `PathHelper` class:

{% highlight csharp %}
//Fetch all characteristics for the part "MetalPart"
var partPath = PathHelper.String2PartPathInformation("/MetalPart");
var characteristics = await RestDataServiceClient.GetCharacteristics( partPath );
{% endhighlight %}

{{ site.headers['bestPractice'] }} Create or update multiple entities in one step

<h2 id="{{page.sections['basics']['secs']['measurementsValues'].anchor}}">{{page.sections['basics']['secs']['measurementsValues'].title}}</h2>

{{ site.images['info'] }} The `LastModfified` property is only relevant for fetching measurements. On creating or updating a measurement it is set by server automatically.

<h2 id="{{page.sections['basics']['secs']['rawData'].anchor}}">{{page.sections['basics']['secs']['rawData'].title}}</h2>
