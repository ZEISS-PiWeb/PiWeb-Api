<hr />

<h4>Using the primary measurement key</h4>

The primary measurement key can be used to filter and fetch multiple measurements of the same physical part. This is a common use case when parts are measured multiple times on different machines or assembly stages.

PiWeb allows you to filter your measurement query on server side, so the returned result set contains exactly the data you need. Three new parameters are introduced to the query:

{% capture table %}
| **Parameter** | Description |
|------------------|--------------------------------------------------------------------|
| **mergeAttributes** | A list of attribute keys, by which the measurements are grouped. Currently, you can only specify one attribute. | 
| **mergeCondition** | Specifies whether the primary measurement key has to appear in only one or multiple parts| 
| **mergeMasterPart** | Specifies the part on which PiWeb searches for distinct values of the primary key attribute
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

In this documentation, we are going to work with the following little measurement table:


<img src="/PiWeb-Api/images/default.png" class="img-responsive center-block">

Please note that these are measurements from three different inspection plan parts which are identified by attribute `14`, which has the name `Key`. In the following examples, all returned measurements will be highlighted. The number in the row headers indicates pivot rows, while the matching color highlights measurements, that are returned because they match a primary measurement key of a pivot row.

**1. Fetching measurements without primary measurement key**

 A very basic query to this measurement table would look like the following:

| **Full query:** | `/measurements?&orderBy=4&limitResult=3` |
|------------------|--------------------------------------------------------------------|
| **orderBy** | `4` | 
| **limitResult** | `3` | 


<img src="/PiWeb-Api/images/limitResult.png" class="img-responsive center-block">

This query will fetch the last 3 measurements of all parts. The result set contains two different physical parts with the Key `Y` and `O`, on two different inspection plan parts `Assembly` and `Cmm`.


**2. Adding the paramter `mergeAttributes`**

In our example, we are using attribute `14` as our primary measurement key. PiWeb will only return measurements, that have a value for the primary measurement key that exists in all specified parts. You can change this behavior later.

{{ site.images['info'] }} PiWeb needs the parameter `partUuids` to be specified when applying the primary measurement keys. Querys with the parameter `partPath` specified and the parameter `deep` set to `true` are **not allowed**.


| **Full query:** | `/measurements?&orderBy=4&limitResult=3&partUuids={uuid1,uuid2,uuid3}&mergeAttributes={14}` |
|------------------|--------------------------------------------------------------------|
| **mergeAttributes** | `{14}` | 
| **mergeCondition** | `MeasurementsInAllParts` **(default)** |
| **mergeMasterPart** | `null` **(default)** |


<img src="/PiWeb-Api/images/measurementsInAllParts.png" class="img-responsive center-block">

The query returns three measurements with the Key `O` since it's the only key that appears in all three parts, `Assembly`, `Cmm` and `Inline`. 

**3. Adding the parameter `mergeCondition` with value `MeasurementsInAtLeastTwoParts`**

Compared to the last query, the result will also include measurements with keys, that appear in only two parts.

| **Full query:** | `/measurements?&orderBy=4&limitResult=3&partUuids={uuid1,uuid2,uuid3}&mergeAttributes={14}&mergeCondition=MeasurementsInAtLeastTwoParts` |
|------------------|--------------------------------------------------------------------|
| **mergeAttributes** | `{14}` | 
| **mergeCondition** | `MeasurementsInAtLeastTwoParts` | 
| **mergeMasterPart** | `null` **(default)** |


<img src="/PiWeb-Api/images/measurementsInAtLeastTwoParts.png" class="img-responsive center-block">

The result set contains measurements with the keys `O` and `J` because the measurements with unique keys `Y` and `S` are filtered out.

**4. Adding the parameter `mergeCondition` with value `None`**

Setting the `mergeCondition` to `None` will result in single, non-matched measurements to be returned. 

| **Full query:** | `/measurements?&orderBy=4&limitResult=3&partUuids={uuid1,uuid2,uuid3}&mergeAttributes={14}&mergeCondition=None` |
|------------------|--------------------------------------------------------------------|
| **mergeAttributes** | `{14}` | 
| **mergeCondition** | `None` | 
| **mergeMasterPart** | `null` **(default)** |


<img src="/PiWeb-Api/images/mergeAttributes.png" class="img-responsive center-block">

All measurements will be returned, only limited by the parameter `limitResult` which is set to `3`. Therefore, all measurements of the three most recently measured keys will be returned.

**5. Adding the parameter `mergeMasterPart` with the uuid of `Inline`**

When the parameter `mergeMasterPart` is set, PiWeb will search for unique values of the primary measurement key only on measurements of the specified part. Measurements that are not measured on the specified part will therefore be filtered.

| **Full query:** | `/measurements?&orderBy=4&limitResult=3&partUuids={uuid1,uuid2,uuid3}&mergeAttributes={14}&mergeCondition=None&mergeMasterPart=uuid` |
|------------------|--------------------------------------------------------------------|
| **mergeAttributes** | `{14}` | 
| **mergeCondition** | `None` | 
| **mergeMasterPart** | `uuid of part 'Inline'` |



<img src="/PiWeb-Api/images/mergeMasterPart.png" class="img-responsive center-block">

As you noticed, the measurements with the keys `Y` and `J` are not included in the result set, because none of them is measured on the master part `Inline`.

<hr />