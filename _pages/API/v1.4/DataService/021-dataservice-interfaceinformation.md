<h2 id="{{page.sections['dataservice']['secs']['interfaceInformation'].anchor}}">{{page.sections['dataservice']['secs']['interfaceInformation'].title}}</h2>

<h3 id="{{page.sections['dataservice']['secs']['interfaceInformation'].anchor}}-endpoints">Endpoints</h3>

Information about supported interface version can be fetched calling the root endpoint of DataService:

{% assign linkId="interfaceInformationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/" %}
{% assign summary="Returns supported interface version for data service" %}
{% assign description="" %}
{% assign exampleCaption="Get interface version for Data Service" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/ HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}

{"supportedVersions":["1.4.2"]}

{% endhighlight %}
{% endcapture %}
{% include endpointTab.html %}

<h3 id="{{page.sections['interfaceInformation']['secs']['interfaceinformation'].anchor}}-objectstructure">Object Structure</h3>

Interface information returns the highest available minor version of each major version. This lets the user decide which features can be used.
As a minor version is backwards compatible to all previous minor versions, there is no point in enumerating them all. An user want to know which revision of each major version is available as that means they can work around possible bugs of older revisions.

>{{ site.headers['bestPractice'] }} Encode the URL
As some parameter definitions may contain special characters like spaces or plus signs, it is highly recommended to encode the URL before sending requests to prevent unexpected behaviors.

{% assign linkId="details-encoding" %}
{% assign title="Encoding URLs" %}
{% capture content %}

Most characters are recognized correctly, but some special characters in different places can cause errors or unexpected behavior. To avoid this, you should encode your request URL.
Encoding here means *percent-encoding*, replacing special characters with an encoded representation in the form of % and a trailing number.

The most important characters to encode are **spaces**, e.g. in a part path, or the **plus sign** when combining search conditions.

A request like: <br>
`/dataServiceRest/characteristics?partPath=Measured part&searchCondition=4>[sampleValue]+12NotIn[sampleList]` <br>
would result in this simple encoding: <br>
`/dataServiceRest/characteristics?partPath=Measured%20part&searchCondition=4>[sampleValue]%2B12NotIn[sampleList]`

The following table contains the most used special character and their encoding. A larger collection can be found [here](https://www.w3schools.com/tags/ref_urlencode.asp).
<br>

#### Encoding table

{% include_relative encoding_table.html %}

<hr>

#### Encoding date and time

When specifying date and time to filter requests, the expected syntax is XML Date/Time notation: <br>
`YYYY-MM-DDThh:mm:ssZ` <br>

+ *YYYY* indicates the year
+ *MM* indicates the month
+ *DD* indicates the day
+ `T` indicates the start of the required time section
+ *hh* indicates the hour
+ *mm* indicates the minute
+ *ss* indicates the second
+ `Z` indicates the time zone UTC

Example:<br>
`2019-06-21T13:45:00Z` encodes the 21th June 2019, 13:45 UTC.

Please note that this format uses [Coordinated Universal Time (UTC)](https://en.wikipedia.org/wiki/Coordinated_Universal_Time). Depending on your systems time zone you have to take the time difference in account when filtering for dates in UTC format. You can simply add an offset to the time: `2019-06-21T13:45:00+02:00` is UTC+2. Minus is also possible. Remember to encode the plus or minus signs!

{% endcapture %}
{% include detailTab.html %}
