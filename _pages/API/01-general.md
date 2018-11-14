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
     addresses:
        title: Addresses
        anchor: gi-addresses
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

<h2 id="{{page.sections['general']['secs']['model'].anchor}}">{{page.sections['general']['secs']['model'].title}}</h2>

The PiWeb domain model is centered around the idea of a **part**. A part describes a workpiece in general.
Each part has multiple **characteristics** whose **values** can be measured.
Measured **values** can either be numerical (e.g. a diameter) or attributive (e.g. a surface might be _scratched_ or _dented_).
**Measurements** and **measurement values** can also contain **binary data**, e.g. an image or plotted outline of the workpiece. **Binary data** can further be uploaded to parts, most commonly a CAD model.

Parts, characteristics, measurements and measurement values are **PiWeb entities**. Each entity type has a separate configuration of **attributes**. Common attributes for measurements are, for example, the measurement date or the part identifier. The latter identifies the specific workpiece that was manufactured. Attributes can be numbers, dates, text or a predefined set of values, a so-called **catalog**.

The following image represents a simplified version of the PiWeb domain model.
<img src="/PiWeb-Api/images/model.png">

<h2 id="{{page.sections['general']['secs']['addresses'].anchor}}">{{page.sections['general']['secs']['addresses'].title}}</h2>

The base addresses for the REST based services are:

#### Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/DataServiceRest
{% endhighlight %}

#### Raw Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/RawDataServiceRest

{% endhighlight %}

<br/>
<span class="glyphicon glyphicon-info-sign glyphicon-text" aria-hidden="true"></span> `instanceName` and `https` are optional and depend on the server settings.

<h2 id="{{page.sections['general']['secs']['formats'].anchor}}">{{page.sections['general']['secs']['formats'].title}}</h2>

The input and output format is JSON as it is the most performance and memory efficient format at the moment.

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
<span class="glyphicon glyphicon-info-sign glyphicon-text" aria-hidden="true"></span> If the parameter contains lists of ids it needs to be surrounded by `{` and `}`, the values within the list are separated by `,`.

{{ site.headers['bestPractice'] }} Encode the URL
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
