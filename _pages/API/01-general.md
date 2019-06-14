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
     details:
        title: Details
        anchor: gi-details
---

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

### What is PiWeb?
PiWeb is a quality data management system capable of saving, managing, retrieving and analyzing quality inspection data. All data is available at anytime and from anywhere through web-services. Thanks to real-time data analysis, it is possible to react quickly to any problems within production. Further information can be found at
[ZEISS PiWeb](https://www.zeiss.com/metrology/products/software/piweb.html "ZEISS Metrology products").

### What is the PiWeb API?
The PiWeb API provides the public interface of the web service through which various requests can be sent to the PiWeb server. You can create, manage and remove data, or retrieve relevant files and information from the PiWeb server. The used architecture is based on REpresentational State Transfer (REST), a detailed description can be found [here](https://medium.com/@sagar.mane006/understanding-rest-representational-state-transfer-85256b9424aa "Understanding REST").

**The API fulfils two purposes:**

+ **Purpose 1**: *Writing new measurement data / connecting measurement software to the system* <br>
The interface allows you to write new measurement data to the database from software other than PiWeb itself. This enables communication between various measuring instruments and the PiWeb server.

+ **Purpose 2**: *Customer-specific evaluation of measurement data* <br>
Furthermore, the API offers the possibility to query and retrieve measurement data according to certain criteria, in order to be able to process them in a special way. This means that customer-specific evaluations which are not provided by PiWeb itself can still be executed with custom software. You can find information about the structure of API requests in this documentation.

<br>
**The PiWeb API actually consists of two APIs:**

{% capture table %}
REST API        | .NET SDK (C# API)
------------- | -----------------------------------------------------------------------------------
This is the interface provided directly by PiWeb server. It consists of two main services, DataService and RawDataService. For more information, see [Services](#gi-services "Services"). Requests are made using HTTP(S), and data is transferred in JSON format, see [Formats](#gi-formats "Formats"). | The .NET SDK offers C# client implementations for DataService and RawDataService. This way you can develop software using C# objects, methods and available properties instead of pure HTTP(S) and JSON. You can find further information in the [.NET SDK documentation](http://zeiss-piweb.github.io/PiWeb-Api/sdk/ ".NET SDK documentation")
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Both APIs are versioned independently.

<h2 id="{{page.sections['general']['secs']['model'].anchor}}">{{page.sections['general']['secs']['model'].title}}</h2>

This section gives you a brief overview of the PiWeb domain model and explains common terminology used for the PiWeb API. The following image shows a simplified version of the PiWeb domain model: <br>
<br>
<img src="/PiWeb-Api/images/domain_model_en.png" class="img-responsive center-block">

A detailed model can be found in the section [Details](#gi-details "Details").

### Inspection Plan
Inspection plans specify the structure of parts, characteristics and possible sub-parts and sub-characteristics. In order to get a better understanding of these, they are displayed in a tree structure. The uppermost part forms a root part, and the entries following in further levels form subnodes or leaves. The connected PiWeb server always contains a root node for all objects, from which any arbitrary part or characteristic can be reached. <br>
You can create inspection plans on the basis of the desired parts you want to measure and monitor. The inspection plan allows nesting, so that parts and characteristics can again contain such entities. Characteristics cannot contain any parts.

The following image shows an example of a simple inspection plan:

<img src="/PiWeb-Api/images/example_tree.png" class="img-responsive center-block">

<hr>
### Entities
PiWeb's data structure is based on the following four entities: **parts**, **characteristics**, **measurements** and **measured values**. An entity is therefore an object that has certain properties and interaction possibilities. They form the center of the logic that runs within the system.

#### Parts
Parts can represent a simple component up to complex assemblies. They describe a workpiece in general, i.e. the workpiece type. This means that real parts that are later measured do not occur individually in the system, but are assigned to the workpiece in form of a measurement. It is also possible to assign subparts to a part, this can be repeated numerous times.

#### Characteristics
Characteristics can be described as distinctive or important properties of a part. Examples would be bores, curves or oblong holes, i.e. places where accuracy needs to be checked. A characteristic can again have characteristics, which can be repeated many times. An example would be a hole that contains diameter and coordinates as sub-characteristics. <br>

Types of characteristics:
+ *Characteristic* <br>
The basic characteristic that can be assigned to parts and other characteristics.

+ *Measurement point* <br>
A characteristic which represents a point to measure, including coordinates and tolerance. These can only belong to a part and not to another characteristics.

+ *Calculated characteristic* <br>
A special type which is based on two other characteristics and a calculation rule to determine the value. It can contain other basic or calculated characteristics, but no measurement points.

+ *Calculated measurement point* <br>
A special type of measurement point that can be used to calculate distance, symmetry or offset of two other points.

#### Measurement
A measurement is a specific measurement process that can affect several characteristics. For example, if an inspector measures a part and records one, several, or all existing characteristics, this procedure and its values describe a measurement. Not all characteristics need to be measured. A measurement always belongs to only one part, deleting the part also removes the measurements.

#### Measured Values
A measured value describes an actual measured or recorded value that is assigned to a characteristic or attribute. They are part of a measurement. Values can be **numerical**, e.g. a simple floating point number, or **attributive**. Values of attributive characteristics can be categorized. An example would be the result of a quality inspection, which can either be *passed* or *failed*. These categories can be counted, so in this case the number would be two. <br>

 **Attributively counting** characteristics are those that consider the number of a certain characteristic. If you examine a painted component for damaged spots, you can also count the occurrences. If the part has two damaged spots, this again represents a category, namely the parts with the exact number of two defects. These characteristics are therefore not "measurable" in the conventional sense.
<hr>
### Configuration
The configuration defines attributes and catalogs. Both can be created, edited or removed.

#### Attributes
Characteristic and attribute have a similar meaning, but need to be distinguished in PiWeb. Attributes are divided into four groups according to their affiliation to an entity. Therefore, there are part-, characteristic-, measurement- and measured value attributes. These can be seen as properties of an entity. For example, a characteristic can contain attributes such as characteristic-ID, upper and lower limit, target value, and much more. For parts there are attributes like part-ID, the factory area or a short description. The most important attribute of a measured value is the actual value, but there are no restrictions.

Attributes are part of the configuration, and can be set individually. You assign a unique key to each attribute, define description, data type and length. The available data types are `string`, `integer`, `floating-point`, `date` and `catalog`. Catalog entries also have attributes and form a fifth group, although they are not an explicit entity.

The unique key is important, as it has the purpose of an identifier for PiWeb to identify different attributes. The measurement date as an example has the key K4. PiWeb knows different important attributes and their keys, changing them is not advised.

#### Important attributes
To get a better understanding of important attributes, you can find some basic information below:

+ *Measured value: K1* <br>
The attribute of a measured value, which contains the actual value.

+ *Measurement date: K4* <br>
The date of measurement.

+ *Nominal value: K2100* <br>
The desired value a characteristic should have, to which the actual measured value should be as close as possible

+ *Lower/Upper limit: K2110/K2111*<br>
The limit of acceptable tolerance between the nominal value and actual values.

+ *Lower/Upper contact limit: K8012/K8013*<br>
A limit that determines when actions regarding production should be taken based on the number of bad parts

+ *Lower/Upper warning limit: K8014/K8015*<br>
A limit that determines when a warning about bad values should be triggered

#### Catalogs
Catalogs represent a collection of different values. An entry has a number and can have several other attributes. Catalogs are used to simplify decisions at certain points by specifying the available values. The user does not have to enter data by hand, and cannot cause unintended typing errors.

 An example would be the machine catalog. This contains the different machines which could be used to measure a part. Additional information for each machine can be noted there too. Now an inspector selects the responsible machine from the catalog when entering a measurement, which makes things a lot easier. Other values than those stored in the catalog are not possible. Also simple variants, like the direction catalog, which contains directions like left and right, are possible.
<hr>
### Additional Data
Additional data are attachments that can be added to any entity in the inspection plan. This can be text, images, log files, CAD models or any binary file. There is no limit to the number of files and you can edit or remove additional data as desired.

<h2 id="{{page.sections['general']['secs']['services'].anchor}}">{{page.sections['general']['secs']['services'].title}}</h2>

#### DataService
The REST DataService is the endpoint for everything related to PiWeb data like parts, characteristics, measurements, measured values, attributes and catalogs. Creating, managing and removing data is done using requests sent to this service. <br>

The base address is:
```http
http(s)://serverUri:port/instanceName/DataServiceRest
```

For further information see the [DataService documentation](http://zeiss-piweb.github.io/PiWeb-Api/dataservice/v1.4/ "Data Service documentation")

#### Raw Data Service
Use the RawDataService to fetch, create, update and delete additional data, i.e. raw data. <br>

The base address is:
```http
http(s)://serverUri:port/instanceName/RawDataServiceRest
```
For further information see the [RawDataService documentation](http://zeiss-piweb.github.io/PiWeb-Api/rawdataservice/v1.4/ "RawData Service documentation")

><span class="glyphicon glyphicon-info-sign glyphicon-text" aria-hidden="true"></span> `instanceName` and `https` are optional and depend on the server settings.

<h2 id="{{page.sections['general']['secs']['versioning'].anchor}}">{{page.sections['general']['secs']['versioning'].title}}</h2>

#### How is the PiWeb API versioned?
PiWeb uses **Semantic Versioning** ([SemVer](https://semver.org/ "Semantic Versioning")) for its APIs. The REST API, its services and the .NET SDK are versioned independently. The version consists of three components in `X.Y.Z` format, e.g. `2.4.1`. According to this order, the components are *Major*, *Minor* and *Patch*, which have the following meaning:

+ **Major**: *Incompatible Change (Breaking Change)* <br>
If the major version changes, we implemented a change that is not compatible with older API versions. This means that the changed functionality is only supported as of this major release. Examples are changes in the behavior of methods, addition of non-optional parameters or changing data types.

+ **Minor**: *Compatible Change* <br>
A change to the minor version indicates the addition of a backwards compatible feature. This means that existing features and their usages still return the expected results. Software with an older version can communicate with the newer version, unsupported features are marked as such.

+ **Patch**: *Compatible Bugfix* <br>
The change of this version number indicates a patch, i.e. fixing of internal errors or bugs. This is downward compatible and has no relevance regarding the API interface.

A list of changes can be found in the Release Notes.

#### How do I get the API versions supported by PiWeb server?
<br>
The .NET SDK has a global version, depending on your chosen NuGet package or GitHub source files. <br>
The REST API instead doesn't have a version as a whole, each service has its own version. <br>
You can find out the supported service versions by making a GET request to its base URL:
```html
http(s)://ServerUri:Port/instanceName/dataServiceRest
or
http(s)://ServerUri:Port/instanceName/rawDataServiceRest
```

Response:
```json
{"supportedVersions":["1.4.4"]}
```

<h2 id="{{page.sections['general']['secs']['formats'].anchor}}">{{page.sections['general']['secs']['formats'].title}}</h2>

The input and output format is [JSON](https://www.json.org/ "JSON definition").

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
Access to PiWeb server service might require authentication. Authentication can be either *basic authentication*, based on username and password, or *Windows authentication* based on Active Directory integration.

If PiWeb Server is secured by basic authentication you have to pass the credentials in the HTTP Authorization header. The authorization header must contain the `Basic` key word followed by base64 encoded `user:password` string:

```http
Authorization: Basic QWRtaW5pc3RyYXRvcjphZG0hbiFzdHJhdDBy
```

<h2 id="{{page.sections['general']['secs']['parameter'].anchor}}">{{page.sections['general']['secs']['parameter'].title}}</h2>

You can restrict requests by attaching certain parameters to the webservice URL in the following format:

```http
?parameter=value[&parameter=value]
```

<br/>Example:

```http
?deep=true&order=4 asc
```

<br/>
><span class="glyphicon glyphicon-info-sign glyphicon-text" aria-hidden="true"></span> If the parameter contains lists of ids it needs to be surrounded by `{` and `}`, the values within the list are separated by `,`.

>{{ site.headers['bestPractice'] }} Encode the URL
As some parameter definitions may contain special characters like brackets, space or plus sign it is highly recommended to encode the URL before sending requests to prevent unexpected behaviors.

<h2 id="{{page.sections['general']['secs']['codes'].anchor}}">{{page.sections['general']['secs']['codes'].title}}</h2>

The following table shows different HTTP status codes and the possible reason that caused it.

{% capture table %}
Method        | Status codes
------------- | -----------------------------------------------------------------------------------
GET           | **200** (OK)<br> **400** (Bad request) - Request failed <br> **404** (Not found) - Endpoint or item does not exist
POST           | **201** (Created)<br> **400** (Bad request) – Creation of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint doesn't exist <br> **409** (Conflict) – An item does already exist
PUT          | **200** (OK)<br> **400** (Bad request) –  Update of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint or item(s) doesn't exist
DELETE        | **200** (OK)<br>**400** (Bad request) – Request of at least one item failed <br> **404** (Not found) – Endpoint or items do not exist
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}


<h2 id="{{page.sections['general']['secs']['response'].anchor}}">{{page.sections['general']['secs']['response'].title}}</h2>
Every response consists either of the requested data or of an error message returned by the webservice. A typical error response looks as follows:

```json
{
   "message": "Unable to insert inspection plan items. An item with path '/metal part/'
               [uuid: 05040c4c-f0af-46b8-810e-30c0c00a379e] does already exist."
}
```

<h2 id="{{page.sections['general']['secs']['details'].anchor}}">{{page.sections['general']['secs']['details'].title}}</h2>

This section contains detailed information about the domain model.

Complete image of the domain model:
<img src="/PiWeb-Api/images/domain_model_full.png" class="img-responsive center-block">

Relations of additional data:
<img src="/PiWeb-Api/images/domain_model_additional_data.png" class="img-responsive center-block">
