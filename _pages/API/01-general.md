---
area: general
level: 0
isCurrentVersion: true
title: General Information
permalink: /general
redirect_from:
  - /
sections:
  general:
    title: General Information
    iconName: generalInformation16
    anchor: gi
    secs:
     model:
        title: Domain Model
        anchor: gi-model
     services:
        title: Services
        anchor: gi-services
     versioning:
        title: Versioning
        anchor: gi-versioning
     formats:
        title: Formats
        anchor: gi-formats
     security:
        title: Security
        anchor: gi-security
     parameter:
        title: URL and Parameter
        anchor: gi-parameter
     codes:
        title: Status Codes
        anchor: gi-codes
     response:
        title: Response
        anchor: gi-response
---

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

### What is PiWeb?
PiWeb is a quality data management system capable of saving, managing, retrieving and analizing measurement data from production. All data is available anytime and anywhere through a web-service, which makes you independent of your current location. Thanks to real-time data analysis, it is possible to react quickly to any problems within the production. Further information can be found here:
[ZEISS PiWeb](https://www.zeiss.com/metrology/products/software/piweb.html "ZEISS Metrology products").

### What is the PiWeb API?
The PiWeb-API provides the public interface of the web service through which various requests can be sent to the PiWeb server. This includes creating, managing and removing data, as well as retrieving relevant files and information from the PiWeb server. The used architecture is based on REpresentational State Transfer (REST), a detailed description can be found [here](https://medium.com/@sagar.mane006/understanding-rest-representational-state-transfer-85256b9424aa "Understanding REST").

**The API basically fulfils two purposes:**

> **Purpose 1**: *Writing new measurement data / connecting measurement software to the system* <br>
The interface allows new measurement data to be written to the database from software other than PiWeb itself. This enables communication between various measuring instruments and the PiWeb server.

> **Purpose 2**: *Customer-specific evaluation of measurement data* <br>
Furthermore, the API offers the possibility to query and retrieve measurement data according to certain criteria, in order to be able to process them in a special way. This means that customer-specific evaluations which are not provided by PiWeb itself can still be executed with custom software. Information about the structure of API requests can be found in this documentation.

<br>
**The overall architecture concerning the API can be split into two components:**

{% capture table %}
REST API        | .NET SDK (C# API)
------------- | -----------------------------------------------------------------------------------
This is the actual interface of the PiWeb-server. It consists of two main services, Data and RawData Service. For more information, see [Services](#gi-services "Services"). Requests are queried using HTTP(S), and data transfered in JSON format, see [Formats](#gi-formats "Formats"). | This development kit implements Data- and RawData Client, which offer different funtionalities of the respective REST endpoints. This way development can be done using .NET objects, methods and available properties provided by these endpoints, instead of pure HTTP(S) and JSON. Further information can be found in the [.NET SDK documentation](http://zeiss-piweb.github.io/PiWeb-Api/sdk/ ".NET SDK documentation")
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Both components are versioned independently.

<h2 id="{{page.sections['general']['secs']['model'].anchor}}">{{page.sections['general']['secs']['model'].title}}</h2>

This section gives you a brief overview of the PiWeb domain model and explains common terminology used for the PiWeb API. The following image represents a simplified version of the PiWeb domain model: <br>
<br>
<img src="/PiWeb-Api/images/domain_model_en.png" class="img-responsive center-block">

### Inspection Plan
Inspection plans specify the structure of parts, characteristics and possible sub-parts and sub-characteristics. In order to get a better understanding of these, they are displayed in a tree structure. The uppermost part forms a root part, and the entries following in further levels subnodes or leaves, as usual in a tree structure. Implicitly, the connected PiWeb server represents the general root node of all objects, from which any arbitrary one can be reached. Inspection plans are created on the basis of the desired parts to be measured and monitored. The inspection plan allows nesting, so that parts and characteristics can again contain such entities. It should be noted that characteristics cannot contain any parts.
<hr>
### Entities
PiWeb's data structure is based on the following four entities: **parts**, **characteristics**, **measurements** and **measured values**. An entity is therefore an object that has certain properties and interaction possibilities. They form the center of the logic that runs within the system.

#### Parts
Parts as well as their production and monitoring are the reason why PiWeb exists at all. They can represent a simple component up to complex assemblies . They describe a part in general, i.e. they specify the type, but not the real part itself. This means that parts that are later measured in real terms do not occur individually in the system, but are assigned to the component as the respective measurement. It is also possible to assign subparts to a part, this can be repeated numerous times.

#### Characteristics
Characteristics can also be described as distinctive or important properties of a part. Examples would be bores, curves or oblong holes, i.e. places where accuracy needs to be checked. A characteristic can again have sub-characteristic, which can be repeated many times. An example would be a hole that still contains diameter and coordinates as sub-characteristics. <br>

*Types of characteristics*:
+ Characteristic <br>
The basic characteristic that can be assigned to parts and other characteristics.

+ Measurement point <br>
A characteristic which represents a point to measure, including coordinates and tolerance. These can only belong to a part, and not to another characteristics.

+ Calculated characteristic <br>
A special type which is based on two other characteristics and a calculation rule to determine the value. Can contain other basic or calculated characteristics, but no measurement points.

+ Calculated measurement point <br>
A special type of measurement point that can be used to calculate distance, symmetry or misalignment of two other points.

#### Measurement
A measurement is a specific measurement process that can affect several characteristics. For example, if an inspector measures a part and records one, several, or all existing characteristics, this procedure and its values describe a measurement. Not all characteristics need to be measured. A measurement always belongs to only one part, deleting the part also removes the measurements.

#### Measured Values
A measured value describes an actual measured or recorded value that is assigned to a characteristic or attribute. They are part of a measurement. Values can be **numerical**, e.g. a simple floating point number, or **attributive**. These are characteristics whose values can be categorized. An example would be the result of a quality inspection, which can either be *passed* or *failed*. These categories can be counted, so in this case the number would be two. **Attributively counting** characteristics are those that consider the number of a certain characteristic. If one examines a painted component for damaged spots, one can also count the occurences. The part then has, for example, two damaged spots, whereby this again represents a category, namely the parts with the exact number of two defects. These characteristics are therefore not "measurable" in the conventional sense.
<hr>
### Configuration
The configuration describes the management of various attributes and catalogs. Both can be created, existing ones edited or removed.

#### Attributes
Characteristic and attribute have a semantically similar meaning, but need to be distinguished in PiWeb. Attributes are divided into four groups according to their affiliation to an entity. Therefore, there are part-, characteristic-, measurement- and measured value attributes. These can be seen as properties of an entity. For example, a characteristic can contain attributes such as characteristic-ID, upper and lower limit, target value, and much more. For parts there are attributes like part-ID, the factory area or a short description. The most important attribute of a measured value is the actual value, but there are no restrictions. Attributes are part of the configuration, and can be set individually. You assign a unique key to each attribute, define description, data type and length. The possible data types are *string*, *integer*, *floating point*, *date* and *catalog*. Catalog entries also have attributes, and form a kind of fifth group, although they are not an explicit entity. Some important attributes are explained further below.

#### Catalogs
As the name suggests, catalogs represent a collection of different values. An entry has a number and can have several other attributes. Catalogs are used to simplify decisions at certain points by specifying the possible values. The user does not have to enter data by hand, and cannot inadvertently cause typing errors. An example would be the machine catalog. This contains the different machines which could be used to measure a part. Additional information for each machine can be noted there too. Now an inspector can select the responsible machine from the catalog when entering a measurement, which makes things a lot easier. Other values than those stored in the catalog are then not possible. Also simple variants, like the direction catalog, which contains directions like left and right, are possible.
<hr>
### Additional Data
Additional data can be seen as a kind of attachment that can be added to any entity in the inspection plan. This can be text, images, log files and much more. Binary files, most often CAD models of the part, can also be attached as additional files. There is no limit to the number of files, and additional data can be edited or removed as desired.
<hr>
#### Important attributes
To get a better understanding of important attributes, you can find some basic information below

+ **Nominal value** <br>
The desired value a characteristic should have, to which the actual measured value should be as close as possible

+ **Measured value** <br>
The attribute of a measured value, which contains the actual value.

+ **Upper/Lower limit** <br>
The limit of acceptable tolerance between the nominal value and actual values.

+ **Upper/Lower warning limit** <br>
A limit that determines when a warning about bad values should be triggered

+ **Upper/Lower contact limit** <br>
A limit that determines when actions regarding production should be taken based on the number of bad parts

<h2 id="{{page.sections['general']['secs']['services'].anchor}}">{{page.sections['general']['secs']['services'].title}}</h2>

#### Data Service
The REST Data Service is the endpoint for everything related to PiWeb data like parts, characteristics, measurements, measured values, attributes and catalogs. Creating, managing and removing data is done using requests queried to this service. <br>
The base address is:
```http
http(s)://serverUri:port/instanceName/DataServiceRest
```

For further information see the [Data Service documentation](http://zeiss-piweb.github.io/PiWeb-Api/dataservice/v1.4/ "Data Service documentation")

#### Raw Data Service
The REST Raw Data Service is responsible for everything regarding additional data attached to certain entities. It is possible to fetch, create, update and delete raw data objects by using this endpoint. <br>
The base address is:
```http
http(s)://serverUri:port/instanceName/RawDataServiceRest
```
For further information see the [RawData Service documentation](http://zeiss-piweb.github.io/PiWeb-Api/rawdataservice/v1.4/ "RawData Service documentation")

><span class="glyphicon glyphicon-info-sign glyphicon-text" aria-hidden="true"></span> `instanceName` and `https` are optional and depend on the server settings.

<h2 id="{{page.sections['general']['secs']['versioning'].anchor}}">{{page.sections['general']['secs']['versioning'].title}}</h2>

#### How is the PiWeb API versioned?
The versioning of the API is based on the principle of **Semantic Versioning** ([SemVer](https://semver.org/ "Semantic Versioning")). The version consists of three components in `X.Y.Z` format, e.g. `2.4.1`. According to this order, the components are *Major*, *Minor* and *Patch*, which have the following meaning:

>**Major**: *Incompatible Change (Breaking Change)* <br>
If this part of the version number increases, a not backwards compatible change was implemented. This means that the changed functionality is only supported as of this major release, and only then delivers the expected results. Examples are changes in the behavior of methods, addition of not optional parameters or changing data types.

>**Minor**: *Compatible Change* <br>
A change to the minor version indicates the addition of a downward compatible feature. This means that there are no discrepancies between an older version and the current version with regards to the expected results. Older clients can still communicate with the newer version, not supported features a marked as such. Due to this compatibility, it is advisable to keep the minor version up to date in order to avoid possible errors in previous versions.

>**Patch**: *Compatible Bugfix* <br>
As the name suggests, the change of this version number indicates a patch, i.e. the fix of internal errors or bugs. This is downward compatible and has no relevance regarding the API interface.

Information about the respective changes can be found in the Release Notes.

#### How do I get the current version of the PiWeb-Server?

There are several ways to get this information.

>**Option 1**: *Access via server information*. <br>
If you have direct access to the server, you can get the server information via the Info button (blue question mark at the top right of the server interface). The current version is displayed there.

>**Option 2**: *Access via server endpoint*. <br>
A simple request via HTTP(S) to the server in form of <br>
```html
http(s)://ServerUri:Port/instanceName/index
```
opens the information page of the corresponding PiWeb server. The current version is displayed there. <br>
The following request also returns the server version in text form:
```html
http(s)://ServerUri:Port/instanceName/serviceInformation
```
Response: The server runs on version 7.4.0.0
```html
7.4.0.0-20190528T152434Z-0351f074c49ddb9a07a6f4a9bb2674d116c37092
```

#### How do I get the API versions supported by the PiWeb server?

A request to the desired service provides the supported versions:
```html
http(s)://ServerUri:Port/instanceName/dataServiceRest

http(s)://ServerUri:Port/instanceName/rawDataServiceRest
```

Response:
```json
{"supportedVersions":["1.4.4"]}
```

<h2 id="{{page.sections['general']['secs']['formats'].anchor}}">{{page.sections['general']['secs']['formats'].title}}</h2>

The input and output format is [JSON](https://www.json.org/ "JSON definition") as it is the most performance and memory efficient format at the moment, and still offers easy readability.

A sample JSON representation of a part in PiWeb:
```json
{
    "path": "P:/metal part/",
    "charChangeDate": "2014-11-19T10:48:32.917Z",
    "attributes": { "1001": "4466", "1003": "mp" },
    "uuid": "05040c4c-f0af-46b8-810e-30c0c00a379e",
    "version": 0,
    "timestamp": "2012-11-19T10:48:32.887Z"
}
```

<h2 id="{{page.sections['general']['secs']['security'].anchor}}">{{page.sections['general']['secs']['security'].title}}</h2>
Access to PiWeb server service might require authentication. Authentication can be either *basic authentication* based on username and password or *Windows authentication* based on Active Directory integration.

If PiWeb Server is secured by basic authentication you have to pass the credentials in the HTTP Authorization header. The authorization header must contain the `Basic` key word followed by base64 encoded `user:password` string:

{% highlight http %}

Authorization: Basic QWRtaW5pc3RyYXRvcjphZG0hbiFzdHJhdDBy

{% endhighlight %}

<h2 id="{{page.sections['general']['secs']['parameter'].anchor}}">{{page.sections['general']['secs']['parameter'].title}}</h2>

You can restrict requests by attaching certain parameters to the webservice URL in the following format:

{% highlight http %}
?parameter=value[&parameter=value]
{% endhighlight %}

<br/>Example:

{% highlight http %}
?deep=true&order=4 asc
{% endhighlight %}

<br/>
><span class="glyphicon glyphicon-info-sign glyphicon-text" aria-hidden="true"></span> If the parameter contains lists of ids it needs to be surrounded by `{` and `}`, the values within the list are separated by `,`.

>{{ site.headers['bestPractice'] }} Encode the URL
As some parameter defintions may contain special characters like brackets, space or plus sign it is highly recommended to encode the URL before sending requests to prevent unexpected behaviors.

<h2 id="{{page.sections['general']['secs']['codes'].anchor}}">{{page.sections['general']['secs']['codes'].title}}</h2>

{% capture table %}
Method        | Statuscodes
------------- | -----------------------------------------------------------------------------------
GET           | **200** (OK)<br> **400** (Bad request) - Request failed <br> **404** (Not found) - Endpoint or item does not exist
POST           | **201** (Created)<br> **400** (Bad request) – Creation of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint doesn't exist <br> **409** (Conflict) – An item does already exist
PUT          | **200** (OK)<br> **400** (Bad request) –  Update of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint or item(s) doesn't exist
DELETE        | **200** (OK)<br>**400** (Bad request) – Request of at least one item failed <br> **404** (Not found) – Endpoint or items do not exist
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


<h2 id="{{page.sections['general']['secs']['response'].anchor}}">{{page.sections['general']['secs']['response'].title}}</h2>
Every response consists either of the requested data or of an error message returned by the webservice. A typical error response looks as follows:

{% highlight json %}
{
   "message": "Unable to insert inspection plan items. An item with path '/metal part/'
               [uuid: 05040c4c-f0af-46b8-810e-30c0c00a379e] does already exist."
}
{% endhighlight %}

<br>
Icons made by Freepik, Smashicons, Dimitry Miroliubov from [flaticon.com](http://www.flaticon.com)
