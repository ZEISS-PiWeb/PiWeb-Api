<h2 id="{{page.sections['rawdataservice']['secs']['rawDataInformation'].anchor}}">{{page.sections['rawdataservice']['secs']['rawDataInformation'].title}}</h2>


### Endpoints

You can can fetch information about raw data objects by using the following endpoints.

{% assign linkId="rawDataEndpointGetListMulti" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity" %}
{% assign summary="Returns a list of information entries for entities with type :entity" %}
{% capture description %}
Returns a list of raw data file information entries for all entities with type :entity. You further have to and restrict the request to the entities' uuids by adding the following uri parameter:

{% capture table %}
Parameter name | Description  <br> *Example* 
---------------|---------------------------
`uuids`        | Restricts the query to the entities identified by the given uuids. <br/> {{site.images['warning']}} Entites of type 'Value' are identified by a compound key, which consists of the uuid of the measurement, '&#124;' and the characteristics uuid <br/><br/> *uuids={652ae7a0-d1e1-4ee2-b3a5-d4526f6ba822&#124;78bd15c6-dc70-4ab4-bd3c-8ab2b5780b52}*
`filter`       | Contains one or multiple attribute conditions to restrict the query. Details can be found below.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

**Filter syntax**

Basic module is expression `<attribute> <operator> <value>` or `<attribute> <list operator> (<value1>, <value2>, ...)`

{% capture table %}
Possible &lt;attribute&gt; values | Description 
----------------------------------|---------------------------
`MimeType` | MimeType of raw data object
`FileName` | File name of raw data object
`LastModified` | Date of last change made to raw data object
`Created` | Date of creation of raw data object
`Length` | Size of raw data object in bytes
`MD5` | Check sum of raw data object
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{% capture table %}
Possible &lt;operator&gt; values | Description 
---------------------------------|---------------------------
`eq` | Checks if attribute equals &lt;value&gt;
`ne` | Checks if attribute is not equal to &lt;value&gt;
`lt` | Checks if attribute is lower than &lt;value&gt;
`le` | Checks if attribute is equal or lower than &lt;value&gt;
`gt` | Checks if attribute is greater than &lt;value&gt;
`ge` | Checks if attribute is equal or greater than &lt;value&gt;
`like` | Compares attribute with a wildcard string. Character '*' represents any number of any characters. Character '?' represents exactly one of any characters. If you want to use one of these wildcard characters within a filter expression use '\' to mask it. Use '\' to mask '\' as well. Use of like operator is only available for string attributes.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{% capture table %}
Possible &lt;List operator&gt; values | Description 
--------------------------------------|---------------------------
`in` | Checks if attribute exists in a list of values
`notin` | Checks if attribute does not exist in a list of values
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

**&lt;Value&gt;** can be formatted differently and needs to match type of attribute:
- *Strings* need to be surrounded by a single quote. If value contains a single quote it needs to be masked by another single quote:
`'Some Value'` or `'This string contains a single '' character.'`.
- Decimal seperator for *Numbers* is a point.
- *Date specifications* may be absolute or relative to current server time.
  - Absolute date specifications are ISO formatted strings and have to explicitly contain a time zone
  `'2018-01-21T16:12:30+01:00'`
  - Relative data specifications are strings in following format: `'-<Offset>[mhdwMy]'` where m = minutes, h = hours, d = days, w = weeks, M = months, y = years, e.g. '-1w' or '-5d'

Value list **(&lt;Value1&gt;, &lt;Value2&gt;, ...)** contains any number of elements which need to meet the conditions described in above <Value> section.

Attribute conditions can be combined by logical operators `and`, `or` or `not`:
`<Condition1> and <Condition2>`
`<Condition1> or <Condition2>`
`not <Condition1>`


Examples:
`Filename like '*.txt' or MimeType eq 'text/plain'`
`Length lt 5000`
`(MimeType like 'text/* or Filename like '*.txt') and Length lt 5000`
`not MimeType like 'text/*'`
`Filename in ('some_file.txt', 'some_other_file.html')`

{% endcapture %}

{% assign exampleCaption="Get the information entries for several parts" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part?uuids={05040c4c-f0af-46b8-810e-30c0c00a379e,5441c003-b6db-4217-ac6a-45cdbb805bb3}&fileName%20like%20'*meshModel*' HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

[
 {
      "target":
      {
          "entity": "Part",
          "uuid": " 05040c4c-f0af-46b8-810e-30c0c00a379e"
      },
      "key": 0,
      "fileName": "section view.meshModel",
      "mimeType": "application/x-zip-compressed",
      "lastModified": "2012-11-19T10:48:34.327Z",
      "created": "2012-11-19T10:48:34.327Z",
      "size": 147376,
      "md5": "02f9c86143ea176c06e24524385b5907"
  },
  {
      "target":
      {
          "entity": "Part",
          "uuid": "5441c003-b6db-4217-ac6a-45cdbb805bb3"
      },
      "key": 0,
      "fileName": "cad_22.meshModel",
      "mimeType": "application/x-zeiss-piweb-meshmodel",
      "lastModified": "2015-03-20T14:37:02.943Z",
      "created": "2015-03-20T14:37:02.943Z",
      "size": 837245,
      "md5": "cbde88e2ed754c70860b3e6d4313551a"
 }
]
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

{% assign linkId="rawDataEndpointGetListSingle" %}
{% assign method="GET" %}
{% assign endpoint="/rawData/:entity/:uuid" %}
{% assign summary="Returns a list of information entries for the entity with type :entity and guid :uuid " %}
{% assign description="" %}
{% assign exampleCaption="Get the information entries for a certain part" %}

{% capture jsonrequest %}
{% highlight http %}
GET /rawDataServiceRest/rawData/part/05040c4c-f0af-46b8-810e-30c0c00a379e HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
 {% highlight json %}
 
[
 {
      "target":
      {
          "entity": "Part",
          "uuid": " 05040c4c-f0af-46b8-810e-30c0c00a379e"
      },
      "key": 0,
      "fileName": "section view.meshModel",
      "mimeType": "application/x-zip-compressed",
      "lastModified": "2012-11-19T10:48:34.327Z",
      "created": "2012-11-19T10:48:34.327Z",
      "size": 147376,
      "md5": "02f9c86143ea176c06e24524385b5907"
  }
 ]
{% endhighlight %}    
{% endcapture %}

{% include endpointTab.html %}

### Object Structure

The returned objects contains the following properties:

{% capture table %}
Property                             | Description
-----------------|-------------------|------------------------
<nobr><code>TargetEntity</code> target</nobr>   | Specifies a concrete entity for a raw data object and consits of the entity's type (Part, Characteristic, Measurement, Value) and guid. If raw data is attached to an entity of type `Value`, the `uuid` contains a compound key in the following format: `{MeasurementUuid}|{CharacteristicUuid}`
<nobr><code>int</code> key</nobr>               | This is a unique key that identifies this specific raw data object for a corresponding entity. An entity can have multiple raw data object that are distinct by this key.
<nobr><code>string</code> fileName</nobr>       | The filename of the raw data object. Please note that this filename is not unique (unlike filenames in traditional file systems).
<nobr><code>string</code> mimeType</nobr>       | The file's mime type. 
<nobr><code>DateTime</code> lastModified</nobr> | The timestamp of the last modification of the corresponding raw data object
<nobr><code>DateTime</code> created</nobr>      | The timestamp of the creation of the corresponding raw data object
<nobr><code>int</code> size</nobr>              | The size of the raw data object in bytes
<nobr><code>string</code> md5</nobr>            | The MD5-Hash of the raw data object
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
