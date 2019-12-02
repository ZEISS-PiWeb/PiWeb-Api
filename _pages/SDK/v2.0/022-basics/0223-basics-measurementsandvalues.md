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

Measurements contain measured values for specific characteristics, and belong to exactly one part. A measurement is represented by the class `SimpleMeasurement` and `DataMeasurement`, measured values are hold in class `DataValue`

>{{ site.images['info'] }} A `SimpleMeasurement` is a measurement without values, whilst a `DataMeasurement` describes a measurement with measured values.

<img src="/PiWeb-Api/images/measurements_model.png" class="img-responsive center-block">

#### SimpleMeasurement
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`Attribute[]` Attributes                          | The attributes that belong to that measurement
`DateTime` Created                                | The time of creation, *set by server*
`DateTime` LastModified                           | The time of the last modification, *set by server*
`Guid` PartUuid                                   | The Uuid of the part to which the measurement belongs
`SimpleMeasurementStatus` Status                  | A status containing information about characteristics in/out tolerance
`DateTime` Time                                   | The actual time of measuring
`Guid` Uuid                                       | The Uuid of the measurement
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
A `SimpleMeasurement` contains important information about the measurement itself like the linked part or the measurement attributes. You can use this class to create a measurement without measured values.

#### DataMeasurement
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`DataCharacteristic[]` Characteristics            | The affected characteristics and their measured values
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
In most cases however you want to create a measurement with actual measured values. The `DataMeasurement` class contains an additional list of `DataCharacteristic` objects, which stores information about a characteristic and the corresponding value.
<hr>
### Creating measurements and measured values

>{{ site.images['info'] }} The `LastModified` property is only relevant for fetching measurements. On creating or updating a measurement it is set by server automatically.

{{ site.headers['example'] }} Creating a measurement with values

{% highlight csharp %}
//Create an attribute that stores the measured value (if it not exists already)
var ValueAttributeDefinition = new AttributeDefinition( WellKnownKeys.Value.MeasuredValue,
"Value", AttributeType.Float, 0 );

//Create the parts and characteristics you want to measure (details see other examples)
var Part = new InspectionPlanPart {...};
var Characteristic = new InspectionPlanCharacteristic{...};
await DataServiceClient.CreateParts( new[] { Part } );
await DataServiceClient.CreateCharacteristics( new[] { Characteristic } );

//Create value with characteristic
var ValueAndCharacteristic = new DataCharacteristic
{
  //Always specify both, path and uuid of the characteristic. All other properties are obsolete
  Path = Characteristic.Path,
  Uuid = Characteristic.Uuid,
  Value = new DataValue( 0.5 ) //This is your measured value!
};

//Create a measurement and assign the value-characteristic combination
var Measurement = new DataMeasurement
{
  Uuid = Guid.NewGuid(),
  PartUuid = Part.Uuid,
  Time = DateTime.Now,
  Characteristics = new[] { ValueAndCharacteristic }
};

//Create measurement on the server
await DataServiceClient.CreateMeasurementValues( new[] { Measurement } );
{% endhighlight %}
The `DataCharacteristic` represents a connection between an actual measured value and a characteristic. It is linked to the characteristic via its path and uuid, and contains the value in form of a `DataValue` object.
>{{ site.images['info'] }} The method `CreateMeasurementValues` is used to create measurements *with* measured values, not only measured values alone.

>{{ site.images['info'] }} The `Value` property in `DataCharacteristic` is a shortcut to the attribute with key *K1*, which is the measured value.

>{{ site.images['warning'] }} *K1* is always associated with the measured value, changing this key is absolutely not advised as it would result in unexpected behavior!

Instead of using the `Value` property you can access the measured value attribute like any other attribute:

{{ site.headers['example'] }} Writing values using the attribute
{% highlight csharp %}
//Surrounding code is skipped here, see above example for details

//Create the DataValue and add the attribute with value
ValueAndCharacteristic.Value = new DataValue
{
  Attributes = new[] { new Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute( WellKnownKeys.Value.MeasuredValue,
  0.5 ) } //You can set other attributes for the entity Value (if defined) here
};
{% endhighlight %}
In this example we fill the `Attributes` property of our `DataValue` object directly with the attribute and its value. You still use the key *1* (*K1*) because it is associated with the measured value. Other attributes that you may have defined for the entity of type `Value` can be assigned here, too. To assign values to attributes of a measurement, you add it to the measurement definition:

{{ site.headers['example'] }} Assigning values to measurement attributes
{% highlight csharp %}
//Surrounding code is skipped here, see above example for details

//Create the attribute (remember to check if it already exists)
var MeasurementAttributeDefinition = new AttributeDefinition( WellKnownKeys.Measurement.InspectorName,
"Inspector", AttributeType.AlphaNumeric, 30 );

//Create the measurement
var Measurement = new DataMeasurement
{
  Uuid = Guid.NewGuid(),
  PartUuid = Part.Uuid,
  Time = DateTime.Now,
  Attributes = new[]
    {
      new Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute( MeasurementAttributeDefinition.Key, "Ryan, Andrew" )
      //You can again set other attributes for the entity Measurement (if defined) here
    },
  Characteristics = new[] {...}
};

//Create measurement on the server
await DataServiceClient.CreateMeasurementValues( new[] { Measurement } );
{% endhighlight %}

>{{ site.headers['bestPractice'] }} Use the property `Time`
This property is a shortcut to the attribute *Time* with key *K4*.

>{{ site.images['info'] }} Please use the `Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute` class, not the standard `System.Attribute`!
<hr>
### Fetching measurements and measured values

Next to creating or updating measurements, another important functionality is fetching those measurements and measured values according to different criteria.

>{{ site.images['info'] }} To improve performance path information of characteristics in `DataMeasurement`s are always blank when fetching data.

{{ site.headers['example'] }} Fetching measurements of a part
{% highlight csharp %}
//Create PathInformation of the desired part that contains measurements
var PartPath = PathHelper.RoundtripString2PathInformation("P:/Measured part/"))

//Fetch all measurements of this part without measured values
var FetchedMeasurements = await DataServiceClient.GetMeasurements( PartPath )

//Or fetch all measurements of this part with measured values
var FetchedMeasurements = await DataServiceClient.GetMeasurementValues( PartPath )
{% endhighlight %}
This is the simplest way to fetch measurements and the associated measured values as the unfiltered method returns all measurements of the specified part. Each measurement then contains a `DataCharacteristic` objects linking values to characteristics. Since this can result in a large collection of results you have the possibility to create a filter based on different criteria.
This can be done by using `FilterAttributes` which is derived from `AbstractMeasurementFilterAttributes`:

<img src="/PiWeb-Api/images/measurementFilterAttributes_model.png" class="img-responsive center-block">

#### AbstractMeasurementFilterAttributes
{% capture table %}
Property                                                                          | Description
----------------------------------------------------------------------------------|------------------------------------------------------------------
<nobr><code>AggregationMeasurementSelection</code> AggregationMeasurements</nobr> | Specifies what types of measurements will be returned (normal/aggregated measurements or both).
<nobr><code>bool</code> Deep</nobr>                                               | `false` if measurements for only the given part should be searched, `true` if measurements for the given part and contained subparts should be searched.
<nobr><code>DateTime</code> FromModificationDate</nobr>                           | Specifies a date to select all measurements that where modified after that date.
<nobr><code>DateTime</code> ToModificationDate</nobr>                             | Specifies a date to select all measurements that where modified before that date.
<nobr><code>int</code> LimitResult</nobr>                                         | The maximum number of measurements that should be returned, unlimited if set to -1 (default).
<nobr><code>Guid[]</code> MeasurementUuids</nobr>                                 | List of uuids of measurements that should be returned.
<nobr><code>Order[]</code> OrderBy</nobr>                                         | The sort order of the resulting measurements.
<nobr><code>Guid[]</code> PartUuids</nobr>                                        | The list of parts that should be used to restrict the measurement search.
<nobr><code>GenericSearchCondition</code> SearchCondition</nobr>                  | The search condition that should be used.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

The class `MeasurementValueFilterAttributes` contains further possibilities.
#### MeasurementValueFilterAttributes
{% capture table %}
Property                                                                   | Description
---------------------------------------------------------------------------|------------------------------------------------------------------
<nobr><code>Guid[]</code> CharacteristicsUuidList</nobr>                   | The list of characteristics including its measured values that should be returned.
<nobr><code>ushort[]</code> MergeAttributes</nobr>                         | The list of primary measurement keys to be used for joining measurements across multiple parts on the server side.
<nobr><code>MeasurementMergeCondition</code> MergeCondition</nobr>         | Specifies the condition that must be met when merging measurements across multiple parts using a primary key. Default value is <code>MeasurementMergeCondition.MeasurementsInAllParts</code>.
<nobr><code>Guid</code> MergeMasterPart</nobr>                             | Specifies the part to be used as master part when merging measurements across multiple parts using a primary key.
<nobr><code>AttributeSelector</code> RequestedMeasurementAttributes</nobr> | The selector for the measurement attributes. Default: all
<nobr><code>AttributeSelector</code> RequestedValueAttributes</nobr>       | The selector for the measurement value attributes. Default: all
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Please find some usefull examples for usage of the filter in the following section. The part path is the same as in the first example of this section.

{{ site.headers['example'] }} Fetching measurements in a time range
{% highlight csharp %}
//Fetch all measurements of the part
var FetchedMeasurements = await DataServiceClient.GetMeasurementValues(
  PartPath,
  new MeasurementValueFilterAttributes
  {
    AggregationMeasurements = AggregationMeasurementSelection.All,
    Deep = true,
    FromModificationDate = DateTime.Now - TimeSpan.FromDays(2),
    ToModificationDate = DateTime.Now
  })
{% endhighlight %}
This returns the measurements of the last 48 hours. You can also use actual dates instead of a time range. It is not required to specify both dates, you could use `FromModificationDate` and `ToModificationDate` independently.

{{ site.headers['example'] }} Fetching specified attributes
{% highlight csharp %}
//Fetch all measurements of the part
var FetchedMeasurements = await DataServiceClient.GetMeasurementValues(
  PartPath,
  new MeasurementValueFilterAttributes
  {
    AggregationMeasurements = AggregationMeasurementSelection.All,
    Deep = true,
    FromModificationDate = DateTime.Now - TimeSpan.FromDays(2),
    ToModificationDate = DateTime.Now
    RequestedMeasurementAttributes = new AttributeSelector(){ Attributes = new ushort[]{ WellKnownKeys.Measurement.Time, WellKnownKeys.Measurement.OperatorName, WellKnownKeys.Measurement.Conctract } }  
  })
{% endhighlight %}
To retrieve only a subset of measurement attributes we set `RequestedMeasurementAttributes`, which requires an `AttributeSelector`. Simply add the keys of the desired attributes to the `Attributes` property, so in this case *time*, *operator name* and *contract* attributes. The property `RequestedValueAttributes` works the same way for measured value attributes.

>{{ site.images['info'] }} The measured value attribute `K1` is always returned in the `Value` property of a `DataCharacteristic`, even when not explicitly requested in `AttributeSelector`.

{{ site.headers['example'] }} Using search conditions
{% highlight csharp %}
//Fetch all measurements of the part
var FetchedMeasurements = await DataServiceClient.GetMeasurementValues(
  PartPath,
  new MeasurementValueFilterAttributes
  {
    SearchCondition = new GenericSearchAttributeCondition
    {
      Attribute = WellKnownKeys.Measurement.Time,
      Operation = Operation.GreaterThan,
      Value = XmlConvert.ToString(DateTime.UtcNow - TimeSpan.FromDays(2), XmlDateTimeSerializationMode.Utc)
    }
  })
{% endhighlight %}
In this case we use a `GenericSearchAttributeCondition`, a search condition for attributes. You specify the attribute key with the value of interest, an operation and the value you want to check against. This example again returns the measurements of the last 48 hours.
To create more complex filters please use  `GenericSearchAnd`, `GenericSearchOr` and/or `GenericSearchNot`. <br>

>{{ site.headers['bestPractice'] }} Consider filtering on client side
Instead of defining complex queries to retrieve filtered results, you should consider requesting data with less restrictions, and filter it on client side according to your criteria. This reduces workload for the server, as filtering requires more resources.
