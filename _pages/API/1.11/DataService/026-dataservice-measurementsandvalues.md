<h2 id="{{page.sections['dataservice']['secs']['measurementsAndValues'].anchor}}">{{page.sections['dataservice']['secs']['measurementsAndValues'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['measurementsAndValues'].anchor}}-endpoints">Endpoints</h3>

You can fetch, create, update and delete measurements and values using the following endpoints:
<br/>

>{{ site.images['info'] }} Endpoint `Measurements` creates measurements **without** measured values. Values sent to this endpoint are ignored and won't get saved! Always use the `Values` endpoint when creating or updating measurements **with** values!

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
<nobr><code>Path</code> partPath </nobr> | Restricts the query to this part <br> `partPath=/metal%20part`
<nobr><code>bool</code> deep </nobr><br><i>default:</i> <code>false</code> | Determines whether the query should affect all levels of the inspection plan. <br> `deep=true`
<nobr><code>OrderCriteria</code> order </nobr><br><i>default:</i> <code>4 desc</code>   | Determines which attribute keys and which direction the keys should be ordered by <br> `order=4 asc, 10 desc`
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]`
<nobr><code>bool</code> caseSensitive </nobr> | Determines whether the search condition string comparison should be done case sensitive, case insensitive or (if not selected) by the current database default. <br> `caseSensitive=true` <br> `caseSensitive=false`
<nobr><code>DateTime</code> fromModificationDate </nobr> | Specifies a date to select all measurements that where modified after that date. Please note that the system modification date (lastModified property) is used and not the time attribute (creation date).
<nobr><code>DateTime</code> toModificationDate </nobr> | Specifies a date to select all measurements that where modified before that date. Please note that the system modification date (lastModified property) is used and not the time attribute (creation date).
<nobr><code>int</code> limitResult </nobr>| Restricts the number of result items. <br> `limitResult=100`
<nobr><code>int</code> limitResultPerPart </nobr>| Restricts the number of result items per part. This parameter only takes effect in combination with either partPath & deep or partUuids:<br> PartPath & deep: trigger a deep search returning at max limitResultPerPart measurements for each child part.<br> PartUuids: return at max limitResultPerPart measurements for each specified part.<br> `limitResultPerPart=20`
<nobr><code>UShort list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All attributes</code> | Restricts the query to the attributes that should be returned for measurements. Use an empty list `{}` to return no attributes. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>None, Simple, Detailed</code> statistics </nobr><br><i>default:</i> <code>None</code> | Indicates how statistical informtaion should be returned: <br><code>None</code> = Return no information<br><code>Simple</code> = Return statistical information including number of characteristics out of warning limit, number of characteristics out of tolerance and number of characteristics in warning limit and tolerance<br><code>Detailed</code> = Return statistical information the same way as <code>Simple</code> plus the guid for each characteristic <br> `statistics=Simple`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be fetched. <br> `aggregation=All`
<nobr><code>int List</code> mergeAttributes</nobr> | Specifies the list of primary measurement keys to be used for joining measurements accross multiple parts on the server side. (Please find more detailed information below.) <br> `mergeAttributes=4,6`
<code>None, MeasurementsInAtLeastTwoParts, MeasurementsInAllParts</code> mergeCondition <br><i>default:</i> <code>MeasurementsInAllParts</code> | Specifies the condition that must be adhered to when merging measurements accross multiple parts using a primary key.	(Please find more detailed information below.)
<nobr><code>Guid</code> mergeMasterPart</nobr> | Specifies the part to be used as master part when merging measurements accross multiple parts using a primary key. (Please find more detailed information below.)
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% include_relative 027-dataservice-measurementsandvalues-primarymeasurementkey.md %}

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
<nobr><code>UShort list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All attributes</code> | Restricts the query to the attributes that should be returned for measurements. Use an empty list `{}` to return no attributes. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>None, Simple, Detailed</code> statistics </nobr><br><i>default:</i> <code>None</code> | Indicates how statistical informtaion should be returned: <br><code>None</code> = Return no information<br><code>Simple</code> = Return statistical information including number of characteristics out of warning limit, number of characteristics out of tolerance and number of characteristics in warning limit and tolerance<br><code>Detailed</code> = Return statistical information the same way as <code>Simple</code> plus the guid for each characteristic <br> `statistics=Simple`
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
To create a new measurement, you must send its JSON representation in the request body. A value for `uuid` is required, `attributes` and `comment` are optional. The attribute keys must be valid measurement attributes as specified in the <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>.

{{ site.images['info'] }} The comment is only added if versioning is enabled in server settings.
{% endcapture %}
{% assign exampleCaption="Create a measurement" %}
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
Updating a measurement is about changing measurement attributes. Pass the whole measurement including all attributes in the request body. The server then deletes all attributes and creates the new one in a single transaction.
{% endcapture %}

{% assign exampleCaption="Update a measurement - add and change an attribute" %}
{% assign comment="" %}

{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/measurements HTTP/1.1
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
You have multiple options for deleting measurements:

* Delete all measurements
* Delete measurements by their uuids
* Delete measurements from a single part by its path
* Delete measurements from parts by its uuids

Delete condition for deleting measurements from a single or multiple parts may be further restricted by the filter uri parameter `searchCondition`.

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>Guid list</code> measurementUuids </nobr>         | Restricts the query to these measurements <br> `measurementUuids={5b59cac7-9ecd-403c-aa26-56dd25892421}`
<nobr><code>Guid list</code> partUuids </nobr> | Restricts the query to these parts <br> `partUuids={e42c5327-6258-4c4c-b3e9-6d22c30938b2}`
<nobr><code>Path</code> partPath </nobr> | Restricts the query to this part <br> `partPath=/metal%20part`
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be deleted. <br> `aggregation=All`
<nobr><code>DeleteForCurrentPartOnly</code> or <code>DeleteDeep</code> deep </nobr><br><i>default:</i> <code>DeleteForCurrentPartOnly</code> | Determines whether the query should delete only measurements for the given part(s) specified by either <i>partPath</i> or <i>partUuids.</i><br> `deep=DeleteForCurrentPartOnly`
<nobr><code>bool</code> runAsync </nobr> <br> <i>default:</i> <code>false</code> | Instructs the deletion to run asynchronously. <br> `runAsync=true`
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}

{% assign exampleCaption="Delete measurements newer than 01.01.2015 and older than 31.03.2015 from the part with the uuid e42c5327-6258-4c4c-b3e9-6d22c30938b2" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/measurements?partUuids={4b59cac7-9ecd-403c-aa26-56dd25892421}&searchCondition=4>[2015-01-01T00:00:00Z]+4<[2015-03-31T23:59:59Z] HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}

<hr>
##### {{ site.images['document-info'] }} Example: Asynchronously delete all measurements of the part 'OldPart'.
You can asynchronously delete a large amount of measurements. After initial operation acceptance, the status can be checked using long polling.
##### Request
{% highlight http %}
DELETE /dataServiceRest/measurements?partPath=/OldPart&runAsync=true HTTP/1.1
{% endhighlight %}

##### Response
{% highlight http %}
HTTP/1.1 202 ACCEPTED
{% endhighlight %}
The response contains a `Location`-Header with an URL to track status of the operation:

##### Request
{% highlight http %}
GET /dataServiceRest/pendingOperations/fc2bc95f-b5c5-4060-a3e2-db28c9f75811 HTTP/1.1
{% endhighlight %}

The default polling timeout is 10 seconds. This means you will receive the status after a maximum wait time of 10 seconds but need to check again if the status is still `Running`.
##### Response
{% highlight json %}
{
    "OperationUuid": "fc2bc95f-b5c5-4060-a3e2-db28c9f75811",
    "ExecutionStatus": "Finished",
    "Exception": null
}
{% endhighlight %}
##### Object structure
{% capture table %}
Property                                                       | Description
---------------------------------------------------------------|------------------------------------------------------------------------------------
<nobr><code>Guid</code> OperationUuid</nobr>                   | Identifies the operation uniquely.
<nobr><code>ExecutionStatus</code> ExecutionStatus</nobr>      | The execution status of the operation. Either `Running`, `Finished` or `Exception`.
<nobr><code>Exception</code> Exception</nobr>                  | Contains the occurred exception if status is `Exception`.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

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
DELETE /dataServiceRest/measurements/5b59cac7-9ecd-403c-aa26-56dd25892421 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 OK
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}



{% assign linkId="measurementAttributeValues" %}
{% assign method="GET" %}
{% assign endpoint="/distinctMeasurementAttributeValues" %}
{% assign summary="Fetches distinct values for a certain measurement attribute" %}
{% capture description %}
You can fetch all given attribute values for a measurement attribute. Measurements to be considered are defined by the following parameters:

{% capture table %}
<code>Type</code> Parameter      |  Description <br> <code>Example</code>
--------------------|-----------------------------------------------------------------------------------
<nobr><code>int</code> key </nobr> | The attribute key distincted values should be fetched for
<nobr><code>Guid list</code> measurementUuids </nobr>         | Restricts the query to these measurements <br> `measurementUuids={5b59cac7-9ecd-403c-aa26-56dd25892421}`
<nobr><code>Guid list</code> partUuids </nobr> | Restricts the query to these parts <br> `partUuids={e42c5327-6258-4c4c-b3e9-6d22c30938b2}`
<nobr><code>Path</code> partPath </nobr> | Restricts the query to this part <br><br> `partPath=/metal%20part`
<nobr><code>bool</code> deep </nobr><br><i>default:</i> <code>false</code> | Determines whether the query should affect all levels of the inspection plan. <br> `deep=true`
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`
<nobr><code>DateTime</code> fromModificationDate </nobr> | Specifies a date to select all measurements that where modified after that date. Please note that the system modification date (lastModified property) is used and not the time attribute (creation date).
<nobr><code>DateTime</code> toModificationDate </nobr> | Specifies a date to select all measurements that where modified before that date. Please note that the system modification date (lastModified property) is used and not the time attribute (creation date).
<nobr><code>int</code> limitResult </nobr>| Restricts the number of result items. <br> `limitResult=100`
<nobr><code>int</code> limitResultPerPart </nobr>| Restricts the number of result items per part. This parameter only takes effect in combination with either partPath & deep or partUuids:<br> PartPath & deep: trigger a deep search returning at max limitResultPerPart measurements for each child part.<br> PartUuids: return at max limitResultPerPart measurements for each specified part.<br> `limitResultPerPart=20`
<nobr><code>Measurements, AggregationMeasurements, All</code> aggregation </nobr><br><i>default:</i> <code>Measurements</code> | Specifies which types of measurements will be fetched. <br> `aggregation=All`

{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-inline">' }}

{% endcapture %}
{% assign exampleCaption="Fetch distinct attribute values for attribute key 6 of the last 100 measurements" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataservicerest/distinctMeasurementAttributeValues?key=6&limitResult=100 HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}

{% highlight json %}

 ["5","6","7","8","4","9","","10"]

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
<nobr><code>OrderCriteria</code> order </nobr><br><i>default:</i> <code>4 desc</code>   | Determines which attribute keys and which direction the keys should be ordered by <br> `order=4 asc, 10 desc`
<nobr><code>Condition</code> searchCondition </nobr>| The query will only return items matching all conditions. A simple condition consists of the attribute key, followed by an operator and the value to match enclosed in brackets. Possible operators are: >, <, >=, <=, =, <>, In, NotIn, Like. <br> You can combine multiple conditions with '+'. It can be necessary to encode '+' as '%2B'. The format for date/time has to be “yyyy-mm-ddThh:mm:ssZ”. All values need to be surrounded by [ and ], multiple values are separated by a comma. <br> `searchCondition=4>[2012-11-13T00:00:00Z]`<br>`searchCondition=4>[2012-11-13T00:00:00Z]+852=[1]`<br>`searchCondition=10In[3,21,50]`<br>`searchCondition=6Like[%Example string condition%]`<br>`searchCondition=6=[Example string condition]`
<nobr><code>bool</code> caseSensitive </nobr> | Determines whether the search condition string comparison should be done case sensitive, case insensitive or (if not specified) by the current database default. <br> `caseSensitive=true` <br> `caseSensitive=false`
<nobr><code>DateTime</code> fromModificationDate </nobr> | Specifies a date to select all measurements that where modified after that date. Please note that the system modification date (lastModified property) is used and not the time attribute (creation date).
<nobr><code>DateTime</code> toModificationDate </nobr> | Specifies a date to select all measurements that where modified before that date. Please note that the system modification date (lastModified property) is used and not the time attribute (creation date).
<nobr><code>int</code> limitResult </nobr>| Restricts the number of result items. <br> `limitResult=100`
<nobr><code>int</code> limitResultPerPart </nobr>| Restricts the number of result items per part. This parameter only takes effect in combination with either partPath & deep or partUuids:<br> PartPath & deep: trigger a deep search returning at max limitResultPerPart measurements for each child part.<br> PartUuids: return at max limitResultPerPart measurements for each specified part.<br> `limitResultPerPart=20`
<nobr><code>UShort list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All attributes</code> | Restricts the query to the attributes that should be returned for measurements. Use an empty list `{}` to return no attributes. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>UShort list</code> requestedValueAttributes </nobr><br><i>default:</i> <code>All attributes</code> |List of attributes that should be returned for values. Use an empty list `{}` to return no attributes. <br><br> `requestedValueAttributes={1,8}`
<nobr><code>Guid list</code> characteristicUuids </nobr> | Restricts the query to the characteristics for which values should be returned. <br> `characteristicUuids={525d15c6-dc70-4ab4-bd3c-8ab2b5780e6b, 8faae7a0-d1e1-4ee2-b3a5-d4526f6ba822}`
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
<nobr><code>UShort list</code> requestedMeasurementAttributes </nobr><br><i>default:</i> <code>All attributes</code> | Restricts the query to the attributes that should be returned for measurements.Use an empty list `{}` to return no attributes. <br> `requestedMeasurementAttributes={4,8}`
<nobr><code>UShort list</code> requestedValueAttributes </nobr><br><i>default:</i> <code>All attributes</code> |List of attributes that should be returned for values. Use an empty list `{}` to return no attributes. <br> `requestedValueAttributes={1,8}`
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
To create a measurement with values, you must send its JSON representation in the request body. A value for `uuid` is required, `attributes` and `comment` are optional. The attribute keys must be valid measurement attributes as specified in the <a href="#{{page.sections['dataservice']['secs']['configuration'].anchor}}">{{page.sections['dataservice']['secs']['configuration'].title}}</a>.

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
PUT /dataServiceRest/values HTTP/1.1
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
