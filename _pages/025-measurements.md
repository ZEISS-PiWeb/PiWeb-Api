---
category: dataservice
subCategory: measurements
title: Data Service
subTitle: Measurements and Measured Values
isSubPage: true
permalink: /dataservice/measurements/
sections:
  general: General Information
  endpoint: Endpoint Information
  add: Add Measurements
  get: Get Measurements
  update: Update Measurements
  delete: Delete Measurements
---

## {{ page.sections['general'] }}

Measurements do always belong to a single inspection plan part. Depending on the purpose the measured values are included within a measurement or not. Each measurement consists of the following properties:

Name | Description
-----|-------------
uuid | Identifies this measurement uniquely.
partUuid | The uuid of the part the measurement belongs to.
attributes | A set of attributes which specifies this measurement.
lastModified | Contains the date and time of the last update applied to this measurement.
characteristics | An array of the characteristics which has been measured within the measurement. Each characteristic within this array consits of the uuid it is identified by and an array of attributes which include at least the measured value attribute.
{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['endpoint'] }}

Measurements can be fetched, created, updated and deleted via the following endpoints. Filter can be set as described in the [URL-Parameter section]({{site.baseurl }}/general/#{{ page.subCategory }}).

URL Endpoint | GET | POST | PUT | DELETE
-------------|-----|-----|------|-------
/measurements | Returns all measurements without measured values | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | Updates the committed measurements | Deletes all measurements.
/measurements/:partPath | Returns the measurements without measured values which belongs to the part specified by *:partPath*  | *Not supported* | *Not supported* | Deletes the measurements which belongs to the part specified by *:partPath*
measurements/{:uuidList} | Returns the measurements without measured values which belongs to the parts that uuids are within the *:uuidList* | *Not supported* | *Not supported* |  Deletes all measurements which belongs to the parts that uuid are within the *:uuidList*
/measurements/values | Returns all measurements including measured data | Creates the committed measurements which is/are transfered in the body of the request. These measurements do not contain measured values. | *Not supported*
/measurements/:partPath/values | Returns the measurements including measured values which belongs to the part specified by *:partPath* | *Not supported* | *Not supported* | *Not supported*
/measurements/{:uuidList}/values | Returns the measurements including measured values which belongs to the parts that uuids are within the *:uuidList* | *Not supported* | *Not supported* | *Not supported*

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['add'] }}

Measurements can be created with or without measured values. To create a measurement it is necessary to transfer the measurement object within the request's body. A unique identifier and the path are mandatory, attributes and a comment are optional. The attribute keys which are used for the attributes must come from the parts/characteristics attribute range (specified in the {{ site.links['configuration'] }})

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings.

### {{ site.headers['example'] }} Adding a part with the uuid 05040c4c-f0af-46b8-810e-30c0c00a379e

{{ site.sections['beginExampleWebService'] }}

{% comment %} {% include codeswitcher.html key="add" %} {% endcomment %}

{{ site.headers['request']  | markdownify }}

{% highlight http %}
POST /dataServiceRest/parts HTTP/1.1
{% endhighlight %}

{% include codeStart.html key="add" format="json" %}

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

{% highlight json %}
{
   "status":
   {
       "statusCode": 201,
       "statusDescription": "Created"
   },
   "category": "Success"
}
{% endhighlight %}

{{ site.sections['endCode'] }}

{% comment %}
{% include codeStart.html key="add" format="xml" %}
{% highlight xml %}
<?xml version="1.0" encoding="UTF-8" ?> 
  <InspectionPlanItems>
    <InspectionPlanPart xmlns:q="http://www.daimlerchrysler.com/DataService" uuid="05550c4c-f0af-46b8-810e-30c0c00a379e" >
      <q:attributes key="1001" value="4466" />
      <q:attributes key="1003" value="mp" />
      <q:path>
        <q:node type="Part">metal part</q:node>
      </q:path>
    </InspectionPlanPart>
  </InspectionPlanItems>
{% endhighlight %}
{{ site.sections['endCode'] }}

{{ site.headers['response']  | markdownify }}

{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight xml %}
<Response>
  <status>
    <statusCode>201</statusCode>
    <statusDescription>Created</statusDescription>
  </status>
  <category>Success</category>
</Response>
{% endhighlight %}
{{ site.sections['endCode'] }}

{% endcomment %}

{{ site.sections['endExample'] }}


{{ site.sections['beginExampleAPI'] }}

{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var part = new InspectionPlanPart{ 
  Uuid = new Guid( "05550c4c-f0af-46b8-810e-30c0c00a379e" ),
  Path = PathHelper.String2PartPathInformation( "metal part"),
  Attributes = new[]{ new Attribute( 1001, "4466" ), new Attribute( 1003, "mp" ) }
};
var client = new DataServiceRestClient( serviceUri );
client.CreateParts( new[]{ part } );
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['get'] }}

Fetching inspection plan entites returns the respective parts or characteristics depending on the specified entity constraint and/or filter. 
The most important filter parameter is the *depth* parameter which controls down to which level of the inspection plan the entities should be fetched. Setting *depth:0* means that only the entity itself, *depth:1* means the entity and its direct children should be fetched and so on.

Parts can be fetched several ways:

* fetch a certain part by its path (the filter parameter *depth* must be 0)
* fetch a certain part and its cihldren parts by its path (the filter parameter *depth* must be ≥ 1)
* fetch one or more certain parts by its UUIDs
* fetch all parts (can be restricted by filter parameter *depth*)

There are also several possibilities to fetch characteristics:

* fetch a certain characteristic by its path (the filter parameter *depth* must be 0)
* fetch one or more certain characteristics by its UUIDs
* fetch all characteristics beneath a certain part path (can be restricted by filter parameter *depth*)

### {{ site.headers['example'] }}  Fetching the direct characteristics beneath the part "metal part". Restrict the attributes to the lower and upper tolerance (attribute keys 2110 and 2111).

As the filter parameter *depth* has the default value 1 it can be omitted in this example.

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
GET /dataServiceRest/characteristics/metal%20part?filter=characteristicAttributes:{2110,2111} HTTP/1.1
{% endhighlight %}

{{ site.headers['response'] | markdownify }}
{% highlight json %}
{
   ...
   "data":
   [
       {
           "path": "PC:/metal part/diameter_circle3/",
           "attributes":
           {
               "2110": "-0.2",
               "2111": "0.3",
           },
           "uuid": "1429c5e2-599c-4d3e-b724-4e00ecb0caa7",
           "version": 0,
           "timestamp": "2012-11-19T10:48:32.887Z",
           "current": true
       },
       ...
   ]
}
{% endhighlight %}

{{ site.sections['endExample'] }}

{{ site.sections['beginExampleAPI'] }}
{{ site.headers['request'] | markdownify }}

{% highlight csharp %}
var client = new DataServiceRestClient( serviceUri );
var parentPartPath = PathHelper.String2PartPathInformation("/metal part");
var filter = new InspectionPlanFilterAttributes()
  { RequestedCharacteristicAttributes = new AttributeSelector(
    new[]{ WellKnownKeys.Characteristic.LowerSpecificationLimit, 
    WellKnownKeys.Characteristic.UpperSpecificationLimit} )
  };
var characteristics = client.GetCharacteristicsForPart(
    PathHelper.String2PartPathInformation("/metal part"));
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['update'] }}

Updating inspection plan entities might regard the following aspects: 

* Rename/move inspection plan entities
* Change inspection plan entity's attributes

{{site.images['info']}} If versioning is is server side activated every update of one or more inspection plan entities creates a new version entry.

### {{ site.headers['example'] }}  Rename the characteristic "metal part/diameter_circle3" to "metal part/diameterCircle3"

{{ site.sections['beginExampleWebService'] }}

{{ site.headers['request']  | markdownify }}

{% highlight http %}
PUT /dataServiceRest/characteristics HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
     "path": "PC:/metal part/diameterCircle3/",
     "attributes":
     {
         "2110": "-0.2",
         "2111": "0.3",
     },
     "uuid": "1429c5e2-599c-4d3e-b724-4e00ecb0caa7",
     "version": 0,
     "timestamp": "2012-11-19T10:48:32.887Z",
     "current": true
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

//Get the characteristic
...
var newPath = PathHelper.String2PathInformation( "/metal part/diameterCircle3", "PC" );
characteristic.Path = newPath;
client.UpdateCharacteristics( new InspectionPlanCharacteristic[]{characteristic} );
{% endhighlight %}

{{ site.sections['endExample'] }}

{% comment %}----------------------------------------------------------------------------------------------- {% endcomment %}

## {{ page.sections['delete'] }}

There are two possibilities to delete inspection plan entities either by path or by their uuid. In both cases the entity itself as well as all children are deleted.

### {{ site.headers['example'] }}  Delete the part "metal part"  and all entities below it

{{ site.sections['beginExampleWebService'] }}
{{ site.headers['request'] | markdownify }}

{% highlight http %}
DELETE /dataServiceRest/parts/metal%20part HTTP/1.1
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
client.DeleteParts( PathHelper.String2PartPathInformation( "metal part" ) );
{% endhighlight %}
