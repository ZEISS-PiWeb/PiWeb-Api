<h2 id="{{page.sections['basics']['secs']['measurementsValues'].anchor}}">{{page.sections['basics']['secs']['measurementsValues'].title}}</h2>

Examples in this section:
+ [Creating a measurement with values](#-example--creating-a-measurement-with-values)
+ [Writing values using the attribute](#-example--writing-values-using-the-attribute)
+ [Assigning values to measurement attributes](#-example--assigning-values-to-measurement-attributes)
+ [Fetching measurements of a part](#-example--fetching-measurements-of-a-part)
+ [Fetching measurements in a time range](#-example--fetching-measurements-in-a-time-range)
+ [Fetching specified attributes](#-example--fetching-specified-attributes)
+ [Using search conditions](#-example--using-search-conditions)

<hr>

Measurements contain measured values for specific characteristics, and belong to exactly one part. A measurement is represented by the class `SimpleMeasurementDto` and `DataMeasurementDto`, measured values are hold in class `DataValueDto`

>{{ site.images['info'] }} A `SimpleMeasurementDto` is a measurement without values, whilst a `DataMeasurementDto` describes a measurement with measured values.

<img src="/PiWeb-Api/images/v6/measurements_model.png" class="img-responsive center-block">

#### SimpleMeasurementDto
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`AttributeDto[]` Attributes                       | The attributes that belong to that measurement
`DateTime` Created                                | The time of creation, *set by server*
`DateTime` LastModified                           | The time of the last modification, *set by server*
`Guid` PartUuid                                   | The Uuid of the part to which the measurement belongs
`SimpleMeasurementStatusDto` Status               | A status containing information about characteristics in/out tolerance
`DateTime?` Time                                  | The actual time of measuring
`DateTime` TimeOrMinDate                          | Returns the measurement time and in case of no time specified, the minimum time allowed for server backend
`DateTime` TimeOrCreationDate                     | Returns the measurement time and in case of no time specified, the creation date of the measurement.
`Guid` Uuid                                       | The Uuid of the measurement
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
A `SimpleMeasurementDto` contains important information about the measurement itself like the linked part or the measurement attributes. You can use this class to create a measurement without measured values.

#### DataMeasurementDto
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`DataCharacteristicDto[]` Characteristics         | The affected characteristics and their measured values
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
In most cases however you want to create a measurement with actual measured values. The `DataMeasurementDto` class contains an additional list of `DataCharacteristicDto` objects, which store information about a characteristic and the corresponding value.
<hr>
### Creating measurements and measured values

>{{ site.images['info'] }} The `LastModified` property is only relevant for fetching measurements. On creating or updating a measurement it is set by server automatically.

{{ site.headers['example'] }} Creating a measurement with values

{% highlight csharp %}
//Create an attribute that stores the measured value (if it does not exist already)
var valueAttributeDefinition = new AttributeDefinitionDto( WellKnownKeys.Value.MeasuredValue,
"Value", AttributeTypeDto.Float, 0 );

//Create the parts and characteristics you want to measure (details see other examples)
var part = new InspectionPlanPartDto {...};
var characteristic = new InspectionPlanCharacteristicDto {...};
await DataServiceClient.CreateParts( new[] { part } );
await DataServiceClient.CreateCharacteristics( new[] { characteristic } );

//Create value with characteristic
var valueAndCharacteristic = new DataCharacteristicDto
{
  //Always specify both, path and uuid of the characteristic. All other properties are obsolete
  Path = characteristic.Path,
  Uuid = characteristic.Uuid,
  Value = new DataValueDto( 0.5 ) //This is your measured value!
};

//Create a measurement and assign the value-characteristic combination
var measurement = new DataMeasurementDto
{
  Uuid = Guid.NewGuid(),
  PartUuid = part.Uuid,
  Time = DateTime.Now,
  Characteristics = new[] { valueAndCharacteristic }
};

//Create measurement on the server
await DataServiceClient.CreateMeasurementValues( new[] { measurement } );
{% endhighlight %}
The `DataCharacteristicDto` represents a connection between an actual measured value and a characteristic. It is linked to the characteristic via its path and uuid, and contains the value in form of a `DataValueDto` object.
>{{ site.images['info'] }} The method `CreateMeasurementValues` is used to create measurements *with* measured values, not only measured values alone.

>{{ site.images['info'] }} The `Value` property in `DataCharacteristicDto` is a shortcut to the attribute with key *K1*, which is the measured value.

>{{ site.images['warning'] }} *K1* is always associated with the measured value, changing this key is absolutely not advised as it would result in unexpected behavior!

Instead of using the `Value` property you can access the measured value attribute like any other attribute:

{{ site.headers['example'] }} Writing values using the attribute
{% highlight csharp %}
//Surrounding code is skipped here, see above example for details

//Create the DataValueDto and add the attribute with value
valueAndCharacteristic.Value = new DataValueDto
{
  Attributes = new[] { new AttributeDto( WellKnownKeys.Value.MeasuredValue, 0.5 ) }
  //You can set other attributes for the entity Value (if defined) here too
};
{% endhighlight %}
In this example we fill the `Attributes` property of our `DataValueDto` object directly with the attribute and its value. You still use the key *1* (*K1*) because it is associated with the measured value. Other attributes that you may have defined for the entity of type `Value` can be assigned here, too. To assign values to attributes of a measurement, you add it to the measurement definition:

{{ site.headers['example'] }} Assigning values to measurement attributes
{% highlight csharp %}
//Surrounding code is skipped here, see above example for details

//Create the attribute (remember to check if it already exists)
var measurementAttributeDefinition = new AttributeDefinitionDto( WellKnownKeys.Measurement.InspectorName,
"Inspector", AttributeTypeDto.AlphaNumeric, 30 );
await DataServiceClient.CreateAttributeDefinition( EntityDto.Measurement, measurementAttributeDefinition );

//Create the measurement
var measurement = new DataMeasurementDto
{
  Uuid = Guid.NewGuid(),
  PartUuid = part.Uuid,
  Time = DateTime.Now,
  Attributes = new[]
    {
      new AttributeDto( measurementAttributeDefinition.Key, "Ryan, Andrew" )
      //You can again set other attributes for the entity Measurement (if defined) here
    },
  Characteristics = new[] {...}
};

//Create measurement on the server
await DataServiceClient.CreateMeasurementValues( new[] { measurement } );
{% endhighlight %}

>{{ site.headers['bestPractice'] }} Use the property `Time`
This property is a shortcut to the attribute *Time* with key *K4*.

<hr>
### Fetching measurements and measured values

Next to creating or updating measurements, another important functionality is fetching those measurements and measured values according to different criteria.

>{{ site.images['info'] }} To improve performance path information of characteristics in `DataMeasurementDto`s are always blank when fetching data.

{{ site.headers['example'] }} Fetching measurements of a part
{% highlight csharp %}
//Create PathInformation of the desired part that contains measurements
var partPath = PathHelper.RoundtripString2PathInformation( "P:/Measured part/" );

//Fetch all measurements of this part without measured values
var fetchedMeasurements = await DataServiceClient.GetMeasurements( partPath );

//Or fetch all measurements of this part with measured values
var fetchedMeasurementsWithValues = await DataServiceClient.GetMeasurementValues( partPath );
{% endhighlight %}
This is the simplest way to fetch measurements and the associated measured values as the unfiltered method returns all measurements of the specified part. Each measurement then contains a `DataCharacteristicDto` objects linking values to characteristics. Since this can result in a large collection of results you have the possibility to create a filter based on different criteria.
This can be done by using `MeasurementFilterAttributesDto` which is derived from `AbstractMeasurementFilterAttributesDto`:

<img src="/PiWeb-Api/images/v6/measurementFilterAttributes_model.png" class="img-responsive center-block">

#### AbstractMeasurementFilterAttributes
{% capture table %}
Property                                                                              | Description
--------------------------------------------------------------------------------------|------------------------------------------------------------------
<nobr><code>AggregationMeasurementSelectionDto</code> AggregationMeasurements</nobr>  | Specifies what types of measurements will be returned (normal/aggregated measurements or both).
<nobr><code>bool</code> Deep</nobr>                                                   | `false` if measurements for only the given part should be searched, `true` if measurements for the given part and contained subparts should be searched.
<nobr><code>DateTime?</code> FromModificationDate</nobr>                              | Specifies a date to select all measurements that where modified after that date.
<nobr><code>DateTime?</code> ToModificationDate</nobr>                                | Specifies a date to select all measurements that where modified before that date.
<nobr><code>int</code> LimitResult</nobr>                                             | The maximum number of measurements that should be returned, unlimited if set to -1 (default).
<nobr><code>int</code> LimitResultPerPart </nobr>                                     | Restricts the number of result items per part. This parameter only takes effect in combination with either PartPath & Deep or PartUuids:<br> PartPath & Deep: trigger a deep search returning at max LimitResultPerPart measurements for each child part.<br> PartUuids: return at max LimitResultPerPart measurements for each specified part. Unlimited if set to -1 (default).
<nobr><code>Guid[]</code> MeasurementUuids</nobr>                                     | List of uuids of measurements that should be returned.
<nobr><code>OrderDto[]</code> OrderBy</nobr>                                          | The sort order of the resulting measurements.
<nobr><code>Guid[]</code> PartUuids</nobr>                                            | The list of parts that should be used to restrict the measurement search.
<nobr><code>GenericSearchConditionDto</code> SearchCondition</nobr>                   | The search condition that should be used.
<nobr><code>bool?</code> CaseSensitive</nobr>                                         | `true` if SearchCondition string should be compared case sensitive, `false` if SearchCondition string should be compared case insensitive. If left unassigned, comparison is done in a database-default way.
<nobr><code>bool</code> IsUnrestricted</nobr>                                         | Convenience property to check if restrictions are empty (LimitResult, SearchCondition, PartUuids all empty)
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

The class `MeasurementValueFilterAttributesDto` contains further possibilities.
#### MeasurementValueFilterAttributesDto
{% capture table %}
Property                                                                   | Description
---------------------------------------------------------------------------|------------------------------------------------------------------
<nobr><code>Guid[]</code> CharacteristicsUuidList</nobr>                   | The list of characteristics including its measured values that should be returned.
<nobr><code>ushort[]</code> MergeAttributes</nobr>                         | The list of primary measurement keys to be used for joining measurements across multiple parts on the server side.
<nobr><code>MeasurementMergeConditionDto</code> MergeCondition</nobr>         | Specifies the condition that must be met when merging measurements across multiple parts using a primary key. Default value is <code>MeasurementMergeConditionDto.MeasurementsInAllParts</code>.
<nobr><code>Guid</code> MergeMasterPart</nobr>                             | Specifies the part to be used as master part when merging measurements across multiple parts using a primary key.
<nobr><code>AttributeSelector</code> RequestedMeasurementAttributes</nobr> | The selector for the measurement attributes. Default: all
<nobr><code>AttributeSelector</code> RequestedValueAttributes</nobr>       | The selector for the measurement value attributes. Default: all
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Please find some usefull examples for usage of the filter in the following section. The part path is the same as in the first example of this section.

{{ site.headers['example'] }} Fetching measurements in a time range
{% highlight csharp %}
//Fetch all measurements of the part
var fetchedMeasurements = await DataServiceClient.GetMeasurementValues(
  partPath,
  new MeasurementValueFilterAttributesDto
  {
    AggregationMeasurements = AggregationMeasurementSelectionDto.All,
    Deep = true,
    FromModificationDate = DateTime.Now - TimeSpan.FromDays(2),
    ToModificationDate = DateTime.Now
  });
{% endhighlight %}
This returns the measurements of the last 48 hours. You can also use actual dates instead of a time range. It is not required to specify both dates, you could use `FromModificationDate` and `ToModificationDate` independently.

{{ site.headers['example'] }} Fetching specified attributes
{% highlight csharp %}
//Fetch all measurements of the part
var fetchedMeasurements = await DataServiceClient.GetMeasurementValues(
  partPath,
  new MeasurementValueFilterAttributesDto
  {
    AggregationMeasurements = AggregationMeasurementSelectionDto.All,
    Deep = true,
    FromModificationDate = DateTime.Now - TimeSpan.FromDays(2),
    ToModificationDate = DateTime.Now,
    RequestedMeasurementAttributes = new AttributeSelector(){ Attributes = new ushort[]{ WellKnownKeys.Measurement.Time, WellKnownKeys.Measurement.InspectorName, WellKnownKeys.Measurement.Contract } }
  });
{% endhighlight %}
To retrieve only a subset of measurement attributes we set `RequestedMeasurementAttributes`, which requires an `AttributeSelector`. Simply add the keys of the desired attributes to the `Attributes` property, so in this case *time*, *inspector name* and *contract* attributes. The property `RequestedValueAttributes` works the same way for measured value attributes.

>{{ site.images['info'] }} The measured value attribute `K1` is always returned in the `Value` property of a `DataCharacteristicDto`, even when not explicitly requested in `AttributeSelector`.

{{ site.headers['example'] }} Using search conditions
{% highlight csharp %}
//Fetch all measurements of the part
var fetchedMeasurements = await DataServiceClient.GetMeasurementValues(
  partPath,
  new MeasurementValueFilterAttributesDto
  {
    SearchCondition = new GenericSearchAttributeConditionDto
    {
      Attribute = WellKnownKeys.Measurement.Time,
      Operation = OperationDto.GreaterThan,
      Value = XmlConvert.ToString(DateTime.UtcNow - TimeSpan.FromDays(2), XmlDateTimeSerializationMode.Utc)
    }
  });
{% endhighlight %}
In this case we use a `GenericSearchAttributeConditionDto`, a search condition for attributes. You specify the attribute key with the value of interest, an operation and the value you want to check against. This example again returns the measurements of the last 48 hours.
To create more complex filters please use  `GenericSearchAndDto`, `GenericSearchOrDto` and/or `GenericSearchNotDto`. <br>

>{{ site.headers['bestPractice'] }} Consider filtering on client side
Instead of defining complex queries to retrieve filtered results, you should consider requesting data with less restrictions, and filter it on client side according to your criteria. This reduces workload for the server, as filtering requires more resources.
