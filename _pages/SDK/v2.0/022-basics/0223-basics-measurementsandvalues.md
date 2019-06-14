<h2 id="{{page.sections['basics']['secs']['measurementsValues'].anchor}}">{{page.sections['basics']['secs']['measurementsValues'].title}}</h2>

[Test](#creating-measurements-and-measured-values)

Measurements contain measured values for specific characteristics, and are assigned to exactly one part. A measurement is represented by the class `SimpleMeasurement` and `DataMeasurement`, and measured values are hold in class `DataValue`

>{{ site.images['info'] }} A `SimpleMeasurement` is a measurement without values, whilst a `DataMeasurement` describes a measurement with measured values.

<img src="/PiWeb-Api/images/measurements_model.png" class="img-responsive center-block">

#### SimpleMeasurement
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`Attribute[]` Attributes | The attributes that belong to that measurement
`DateTime` Created | The time of creation, *set by server*
`DateTime` LastModfified | The time of the last modification, *set by server*
`Guid` PartUuid | The Uuid of the part to which the measurement belongs
`SimpleMeasurementStauts` Status | A status containing information about characteristics in/out tolerance
`DateTime` Time | The actual time of measuring
`Guid` Uuid | The Uuid of the measurement
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
A `SimpleMeasurement` contains important information about the measurement itself like the linked part or the measurement attributes. You can use this class to create a measurement without measured values.

#### DataMeasurement
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
`DataCharacteristic[]` Characteristics | The affected characteristics and their measured values
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
In most cases however you want to create a measurement with actual measured values. The `DataMeasurement` class contains an additional list of `DataCharacteristic` objects, which stores information about a characteristic and the corresponding value.
<hr>
### Creating measurements and measured values

>{{ site.images['info'] }} The `LastModfified` property is only relevant for fetching measurements. On creating or updating a measurement it is set by server automatically.

{{ site.headers['example'] }} Create a measurement with values

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
private static readonly DataMeasurement Measurement = new DataMeasurement
{
  Uuid = Guid.NewGuid(),
  PartUuid = Part.Uuid,
  Attributes = new[]
    {
      new Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute( MeasurementAttributeDefinition.Key, "Ryan, Andrew" )
      //You can again set other attributes for the entity Measurement (if defined) here
    },
  Characteristics = new[] {...}
};
{% endhighlight %}

>{{ site.images['info'] }} Please use the `Zeiss.IMT.PiWeb.Api.DataService.Rest.Attribute` class, not the standard `System.Attribute`!
<hr>
### Fetching measurements and measured values

Next to creating or updating measurements, another important functionality is fetching those measurements and measurde values according to different criterias.

{{ site.headers['example'] }} Fetching measurements of a part
{% highlight csharp %}
//Create PathInformation of the desired part that contains measurements
var PartPath = PathHelper.RoundtripString2PathInformation("P:/Measured part/"))

//Fetch all measurements of this part
var FetchedMeasurements = await DataServiceClient.GetMeasurementValues( PartPath )
{% endhighlight %}
This is the simplest way to fetch measurements and the associated measured values, it returns all measurements of the specified part. Since this can result in a large collection of results, you have the possibility to filter based on different criterias.
For this you can use `FilterAttributes`. The base class is `AbstractMeasurementFilterAttributes`:

<img src="/PiWeb-Api/images/measurementFilterAttributes_model.png" class="img-responsive center-block">

#### AbstractMeasurementFilterAttributes
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
<nobr><code>AggregationMeasurementSelection</code> AggregationMeasurements</nobr> | Specifies what types of measurements will be returned (normal/aggregated measurements or both).
<nobr><code>bool</code> Deep</nobr> | `false` if only the part should be searched, `true` if the part and contained sub-parts should be searched.
<nobr><code>DateTime</code> FromModificationDate</nobr> | Specifies a date to select all measurements that where modified after that date. Uses the attribute `LastModfified`, and not `Time`.
<nobr><code>int</code> LimitResult</nobr> | The maximum number of measurements that should be returned, unlimited if set to -1 (default).
<nobr><code>Guid[]</code> MeasurementUuids</nobr> | The list of measurements that should be returned.
<nobr><code>Order[]</code> OrderBy</nobr> | The sort order of the resulting measurements.
<nobr><code>Guid[]</code> PartUuids</nobr> | The list of parts that should be used to restrict the measurement search.
<nobr><code>GenericSearchCondition</code> SearchCondition</nobr> | The search condition that should be used.
<nobr><code>DateTime</code> ToModificationDate</nobr> | Specifies a date to select all measurements that where modified before that date. Uses the attribute `LastModfified`, and not `Time`.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

The class `MeasurementValueFilterAttributes` contains further possibilities.
#### MeasurementValueFilterAttributes
{% capture table %}
Property                                          | Description
--------------------------------------------------|------------------------------------------------------------------
<nobr><code>Guid[]</code> CharacteristicsUuidList</nobr> | The list of characteristics that should be returned.
<nobr><code>ushort[]</code> MergeAttributes</nobr> | The list of primary measurement keys to be used for joining measurements across multiple parts on the server side.
<nobr><code>MeasurementMergeCondition</code> MergeCondition</nobr> | Specifies the condition that must be met to when merging measurements across multiple parts using a primary key. Default value is <code>MeasurementMergeCondition.MeasurementsInAllParts</code>.
<nobr><code>Guid</code> MergeMasterPart</nobr> | Specifies the part to be used as master part when merging measurements across multiple parts using a primary key.
<nobr><code>AttributeSelector</code> RequestedMeasurementAttributes</nobr> | The selector for the measurement attributes.
<nobr><code>AttributeSelector</code> RequestedValueAttributes</nobr> | The selector for the measurement value attributes.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


TODO: Was passiert bei Messungsabruf wenn alle drei Parameter angegeben? Measurement Uuid>part Uuid>path
