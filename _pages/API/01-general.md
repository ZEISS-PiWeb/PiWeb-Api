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
PiWeb is a quality data management system capable of saving, managing, retrieving and analyzing quality inspection data. All data is available at anytime and from anywhere through web-services. Thanks to real-time data analysis, it is possible to react quickly to any problems within production. Further information can be found at
[ZEISS PiWeb](https://www.zeiss.com/metrology/products/software/piweb.html "ZEISS Metrology products").

### What is the PiWeb API?
The PiWeb API provides the public interface of the web service through which various requests can be sent to PiWeb Server. You can create, manage and remove data, or retrieve relevant files and information from PiWeb Server. The used architecture is based on REpresentational State Transfer (REST).

**The API fulfils two purposes:**

+ **Purpose 1**: *Writing new measurement data / connecting measurement software to the system* <br>
The interface allows you to write new measurement data to the database from software other than PiWeb itself. This enables communication between various measuring instruments and PiWeb Server.

+ **Purpose 2**: *Custom evaluation of measurement data* <br>
Furthermore, the API offers the possibility to query and retrieve measurement data according to certain criteria, in order to be able to process them in a special way. This means that customer-specific evaluations which are not provided by PiWeb itself can still be executed with custom software. You can find information about the structure of API requests in this documentation.

<br>
**The PiWeb API actually consists of two APIs:**

{% capture table %}
REST API        | .NET SDK (C# API)
------------- | -----------------------------------------------------------------------------------
This is the interface provided directly by PiWeb Server. It consists of two main services, DataService and RawDataService, explained in the section [Services](#gi-services "Services"). Requests are made using HTTP(S), and data is transferred in [JSON format](#gi-formats "Formats"). | The .NET SDK offers C# client implementations for DataService and RawDataService. This way you can develop software using C# objects, methods and available properties instead of pure HTTP(S) and JSON. You can find further information in the [.NET SDK documentation](http://zeiss-piweb.github.io/PiWeb-Api/sdk/v6.0/ ".NET SDK documentation").
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

Both APIs are versioned independently.

<h2 id="{{page.sections['general']['secs']['model'].anchor}}">{{page.sections['general']['secs']['model'].title}}</h2>
This section gives you an overview of the PiWeb domain model and explains common terminology used for the PiWeb API.

### Inspection plan and measurements
The following image provides an overview to the inspection plan and its relations to measurements and measured values.<br>
<br>
<img src="/PiWeb-Api/images/inspectionPlanAndMeasurements.png" class="img-responsive center-block">

#### Inspection plan
The inspection plan specifies the structure of parts, characteristics and possible sub-parts and sub-characteristics. Because the included elements are related hierarchically they are normally displayed in a tree structure.

#### Part and root part
A part can represent a simple component up to a complex assembly. It describes a workpiece in general, i. e. the workpiece type. This means that real parts that are later measured do not occur individually in the system, but are assigned to the workpiece in form of a measurement. It is also possible to assign sub-parts to a part which can be repeated numerous times.

The uppermost part is the root part which has no parent. It typically contains sub-nodes like parts and characteristics which again may contain sub-nodes. The connected PiWeb Server always provides a root node, from which any arbitrary part or characteristic can be reached.

#### Characteristic
A characteristic can be described as a distinctive or important property of a part. Examples are bores, curves or oblong holes, i. e. places where accuracy needs to be checked. A characteristic can itself contain sub-characteristics, which can be repeated many times. An example would be a hole that contains diameter and coordinates.

Types of characteristics:
- *Characteristic*<br>
  The basic characteristic that can be assigned to parts and other characteristics.
- *Measurement point*<br>
  A characteristic which represents a point to measure, including coordinates and tolerance. These can only belong to a part and not to another characteristic.
- *Calculated characteristic*<br>
  A special type which is based on at least two other characteristics and a calculation rule to determine the value. It can contain other basic or calculated characteristics, but no measurement points.
- *Calculated measurement point*<br>
  A special type of measurement point that can be used to calculate distance, symmetry or offset of two other points.
  
#### Measurement
A measurement is a specific measurement process that can affect several characteristics. For example, if an inspector measures a part and records one, several, or all existing characteristics, this procedure and its values describe a measurement. Not all characteristics need to be measured. A measurement always belongs to only one part, deleting the part also removes the measurements.

#### Measured value
A measured value describes an actual measured or recorded value that is assigned to a characteristic or attribute. Multiple measured values are collected in a measurement. Values can be **numerical**, e. g. a simple floating point number, **attributive** or **enumerated**. 

Values of **attributive** characteristics can be categorized. An example would be the result of a quality inspection, which can either be *passed* or *failed*. These categories can be counted, so in this case the number would be two.

**Enumerated** characteristics are those that consider the number of a certain characteristic. If you examine a painted component for damaged spots, you can also count the occurrences. If the part has two damaged spots, this again represents a category, namely the parts with the exact number of two defects. These characteristics are therefore not “measurable” in the conventional sense.

#### Example of inspection plan with measurements
The following picture shows an example of an inspection plan and related measurements containing measured values. The inspection plan contains the target value while the measurements provide the actual measured values.<br>
<br>
<img src="/PiWeb-Api/images/inspectionPlanAndMeasurementsExample.png" class="img-responsive center-block">

### Entity details
In the following diagram the already explained domain terms *part*, *characteristic*, *measurement* and *measured value* are displayed with the more general term *Entity*. An entity is an object that has certain properties and interaction possibilities. Entities form the center of the logic that runs within the system.<br>
<br>
<img src="/PiWeb-Api/images/entityDetails.png" class="img-responsive center-block">

#### Additional data
Additional data are files that can be attached to any entity in the inspection plan. This can be text files, images, log files, CAD models or any binary file. There is no limit to the number of files. They can be edited or removed as necessary.

#### Attribute value and catalog entry
Each entity can have several attribute values which can be seen as their properties. Such a value can have the data type `string`, `integer`, `floating point`, `data` and `catalog`. The last data type belongs to the specific attribute value, the catalog entry whose possible values are defined in a catalog.

#### Attribute
An attribute defines an attribute value including its data type and key. Each specific attribute defines where they can be used. For each of the four entities a separate type of attribute exists: *part attribute*, *characteristic attribute*, *measurement attribute* and *measured value attribute*. Examples for part attributes are: part ID, factory area and short description. Examples for characteristic attributes are: characteristic ID, upper limit and target value.
Additionally it is also possible to specify attributes for a catalog entry which is an *catalog attribute*.

The unique key of an attribute is important, as it has the purpose of an identifier for PiWeb attributes. The time/date attribute as an example has the key K4. PiWeb already knows a lot of important attributes and their keys by default, which is why changing them is not advised.

Here are some examples for common attributes known by default:

- *Measured value: K1*<br>
  The attribute of a measured value, which contains the actual value.
- *Time/Date: K4*<br>
  The date of measurement.
- *Target value: K2100*<br>
  The desired value a characteristic should have, to which the actual measured value should be as close as possible
- *Lower/Upper limit: K2110/K2111*<br>
  The limit of acceptable tolerance between the target value and actual values.
- *Lower/Upper control limit: K8012/K8013*<br>
  A limit that determines when actions regarding production should be taken based on the number of bad parts
- *Lower/Upper warning limit: K8014/K8015*<br>
  A limit that determines when a warning about bad values should be triggered

### Configuration
In the PiWeb configuration attributes can be defined ready to be used at parts, characteristics, measurements, measured values and catalog entries. The available catalogs and their contained catalog entries are specified as well.<br>
<br>
<img src="/PiWeb-Api/images/configuration.png" class="img-responsive center-block">

#### Catalogs
Catalogs represent a collection of different values. An entry has a number and can have several other attributes. Catalogs are used to simplify decisions at certain points by specifying the available values. The user does not have to enter data by hand, and cannot cause unintended typing errors.

An example would be the machine catalog. This contains the different machines which could be used to measure a part. Additional information for each machine can be noted there too. Now an inspector selects the responsible machine from the catalog when entering a measurement, which makes things a lot easier. Other values than those stored in the catalog are not possible. Also simple variants, like the direction catalog, which contains directions like left and right, are possible.

<h2 id="{{page.sections['general']['secs']['services'].anchor}}">{{page.sections['general']['secs']['services'].title}}</h2>

#### DataService
The REST DataService is the endpoint for everything related to PiWeb data like parts, characteristics, measurements, measured values, attributes and catalogs. Creating, managing and removing data is done using requests sent to this service. <br>

The base address is:
```http
http(s)://serverHost:port/instanceName/DataServiceRest
```

For further information see the [DataService documentation](http://zeiss-piweb.github.io/PiWeb-Api/dataservice/v1.4/ "Data Service documentation")

#### Raw Data Service
Use the RawDataService to fetch, create, update and delete additional data, i.e. raw data. <br>

The base address is:
```http
http(s)://serverHost:port/instanceName/RawDataServiceRest
```
For further information see the [RawDataService documentation](http://zeiss-piweb.github.io/PiWeb-Api/rawdataservice/v1.4/ "RawData Service documentation")

>{{ site.images['info'] }} `instanceName` and `https` are optional and depend on the server settings.

<h2 id="{{page.sections['general']['secs']['versioning'].anchor}}">{{page.sections['general']['secs']['versioning'].title}}</h2>

#### How is the PiWeb API versioned?
PiWeb uses **Semantic Versioning** ([SemVer](https://semver.org/ "Semantic Versioning")) for its APIs. The REST API, its services and the .NET SDK are versioned independently. The version consists of three components in `X.Y.Z` format, e.g. `2.4.1`. According to this order, the components are *Major*, *Minor* and *Patch*, which have the following meaning:

+ **Major**: *Incompatible Change (Breaking Change)* <br>
If the major version changes, we implemented a change that is not compatible with older API versions. This means that the changed functionality is only supported as of this major release. Examples are changes in the behavior of methods, addition of non-optional parameters or changing data types.

+ **Minor**: *Compatible Change* <br>
A change to the minor version indicates the addition of a backwards compatible feature. This means that existing features and their usages still return the expected results. Software with an older version can communicate with the newer version, unsupported features are marked as such.

+ **Patch**: *Compatible Bugfix* <br>
The change of this version number indicates a patch, i.e. fixing of internal errors or bugs. This is downward compatible and has no relevance regarding the API interface.

A list of changes can be found in the release notes.

#### How do I get the API versions supported by PiWeb Server?
<br>
The .NET SDK has a global version, depending on your chosen NuGet package or GitHub source files. <br>
The REST API instead doesn't have a version as a whole, each service has its own version. <br>
You can find out the supported service versions by making a GET request to its base URL:
```html
http(s)://ServerHost:Port/instanceName/dataServiceRest
or
http(s)://ServerHost:Port/instanceName/rawDataServiceRest
```

Response:
```json
{"supportedVersions":["1.4.4"]}
```

<h2 id="{{page.sections['general']['secs']['formats'].anchor}}">{{page.sections['general']['secs']['formats'].title}}</h2>

**Input and output format**

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
<br/>
**Encoding**

Since PiWeb 7.8 the default encoding for strings like the part path is `UTF-8`.
The Content-Type header of incoming messages indicates this as well:

```http
Content-Type: application/json; charset=utf-8
```

>{{ site.images['info'] }} For PiWeb versions before 7.8 the default encoding depends on the machine where the PiWeb Server is running, so it is necessary to use the correct encoding conversion according to your environment,
if your chosen tool or script language does not work directly with present encoding.

<h2 id="{{page.sections['general']['secs']['security'].anchor}}">{{page.sections['general']['secs']['security'].title}}</h2>
Access to PiWeb Server service might require authentication. Authentication can be either *basic authentication* based on username and password, *Windows authentication* based on Active Directory integration, *certificate authentication*
or *OpenID connect authentication*. Our .NET SDK (C# API) offers convenient functionality to use all possible authentication methods, corresponding documentation can be found in the
[security section](http://zeiss-piweb.github.io/PiWeb-Api/sdk/v8.2/#ba-security ".NET SDK security documentation") of the .NET SDK. When accessing the PiWeb API via REST using different clients (e.g. Python), it can be necessary to use 3rd party 
libraries or to implement authentication methods yourself. We do not offer detailed documentation on how to do this, instead you can find further information following provided links.

**Basic authentication**

If PiWeb Server is secured by basic authentication you have to pass the credentials in the HTTP authorization header. The authorization header must contain the `Basic` key word followed by base64 encoded `user:password` string:

```http
Authorization: Basic QWRtaW5pc3RyYXRvcjphZG0hbiFzdHJhdDBy
```
<br/>
**Windows authentication**

If PiWeb Server is secured by Windows authentication you have to follow the [Kerberos protocol](https://learn.microsoft.com/en-us/windows-server/security/kerberos/kerberos-authentication-overview) to authenticate your client. This is easy using our .NET SDK (C# API). For different clients (e.g. Python) this can be more complex, we suggest to use available libraries.

<br/>
**Certificate authentication**

If PiWeb Server is secured by certificate authentication you have to provide a valid X.509 certificate of an authorized user along with the REST request.

<br/>
**OpenID connect authentication**

If PiWeb Server is secured by OpenID connect authentication you have to obtain an access token and pass it in the HTTP authorization header. The access token can be obtained from an OpenID Issuer.
The OpenID issuer URL is provided by a helper endpoint. Invoke a GET request on this endpoint without any authorization header.

```http
http(s)://serverHost:port/instanceName/OAuthServiceRest/oauthTokenInformation
```

A JSON response with the following format will be returned.

```json
{"openIdAuthority":"https://issuerHost:port/basePath"}
```

The OpenID issuer URL can be used together with the [OpenID connect](https://openid.net/connect/) specifications to obtain an access token. You can use
a OpenID connect client library to help dealing with all the details. Following a short description together with the PiWeb specifics.

First use [OpenID connect discovery](https://openid.net/specs/openid-connect-discovery-1_0.html#ProviderConfig) to get the URL of the authorization and
token endpoints. Then you can use the [OpenID connect](https://openid.net/specs/openid-connect-core-1_0.html) specification to get an access token.

The authorization flow used by PiWeb is the Authorization Code Flow + PKCE. You need to specify the client id "f1ddf74a-7ed1-4963-ab60-a1138a089791" and the
scope "piweb". Additionally the scopes "profile", "email" and "offline_access" are supported as well. The redirect_uri is "urn:ietf:wg:oauth:2.0:oob".

The obtained authorization code, the PKCE code verifier and the client secret "d2940022-7469-4790-9498-776e3adac79f" are sent to the token endpoint
which returns an access token.

The access token can be used to authenticate requests to PiWeb services. PiWeb server uses the authentication scheme "Bearer" for OpenID connect
authentication. The HTTP authorization header must contain the scheme "Bearer" as well as the obtained access token.

```http
GET /DataServiceRest/serviceInformation HTTP/1.1
Authorization: Bearer access_token
```

The access token is valid for 1 hour. When the access token has expired you need to obtain a new access token. The expiration date can be obtained
by client from the access token itself. The access token is a JSON Web Token as defined in [RFC7519](https://www.rfc-editor.org/rfc/rfc7519)
and contains the claim "exp" defining its expiration date.

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
>{{ site.images['info'] }} If the parameter contains lists of ids it needs to be surrounded by `{` and `}`, the values within the list are separated by `,`.

>{{ site.headers['bestPractice'] }} Encode the URL
As some parameter definitions may contain special characters like brackets, space or plus sign it is highly recommended to encode the URL before sending requests to prevent unexpected behaviors.

<h2 id="{{page.sections['general']['secs']['codes'].anchor}}">{{page.sections['general']['secs']['codes'].title}}</h2>

The following table shows different HTTP status codes and the possible reason that caused it.

{% capture table %}
Method        | Status codes
------------- | -----------------------------------------------------------------------------------
GET           | **200** (OK) <br> **400** (Bad request) – Request failed <br> **401** (Unauthorized) – Authentication failed <br> **403** (Forbidden) – Permission refused <br> **404** (Not found) – Endpoint or item does not exist <br> **405** (Method not allowed) – Endpoint exists but GET is not allowed
POST          | **201** (Created) <br> **400** (Bad request) – Creation of at least one item failed, e.g. due to bad formatting <br> **401** (Unauthorized) – Authentication failed <br> **403** (Forbidden) – Permission refused <br> **404** (Not found) – Endpoint doesn't exist <br> **405** (Method not allowed) – Endpoint exists but POST is not allowed <br> **409** (Conflict) – An item does already exist <br> **415** (Unsupported media type) – In most cases `application/json` is expected as `Content-Type`
PUT           | **200** (OK) <br> **400** (Bad request) –  Update of at least one item failed, e.g. due to bad formatting <br> **401** (Unauthorized) – Authentication failed <br> **403** (Forbidden) – Permission refused <br> **404** (Not found) – Endpoint or item(s) doesn't exist <br> **405** (Method not allowed) – Endpoint exists but PUT is not allowed <br> **415** (Unsupported media type) – In most cases `application/json` is expected as `Content-Type`
DELETE        | **200** (OK) <br> **400** (Bad request) – Request of at least one item failed <br> **401** (Unauthorized) – Authentication failed <br> **403** (Forbidden) – Permission refused <br> **404** (Not found) – Endpoint or items do not exist <br> **405** (Method not allowed) – Endpoint exists but DELETE is not allowed
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
