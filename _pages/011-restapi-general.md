---
area: restApi
level: 1
category: general
title: REST API
subTitle: General Information
permalink: /restapi/general/
sections:
   addresses: Addresses
   formats: Formats
   codes: Status Codes
   parameter: Url Parameter
   security: Security
   envelope: Response Envelope
redirect_from: "/"
---

## {{page.sections['addresses']}}

The base addresses for the REST based services are:

### Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/DataServiceRest
{% endhighlight %}

<br/>and

### Raw Data Service

{% highlight http %}
http(s)://serverUri:port/instanceName/RawDataServiceRest

{% endhighlight %}

<br/>
{{ site.images['info'] }} `instanceName` and `https` are optional and depend on the server settings.

## {{page.sections['formats']}}

The input and output format is JSON as it is the most performance and memory efficient format at the moment.

## {{page.sections['codes']}}

{% capture table %}
Method        | Statuscodes
------------- | -----------------------------------------------------------------------------------
GET           | **200** (OK)<br> **400** (Bad request) - Request failed <br> **404** (Not found) - Endpoint or item does not exist 
POST           | **201** (Created)<br> **400** (Bad request) – Creation of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint doesn't exist <br> **409** (Conflict) – An item does already exist
PUT          | **200** (OK)<br> **400** (Bad request) –  Update of at least one item failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint or item(s) doesn't exist
DELETE        | **200** (OK)<br>**400** (Bad request) – Request of at least one item failed <br> **404** (Not found) – Endpoint or items do not exist
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

## {{page.sections['parameter']}}

You can restrict requests by attaching certain parameters to the webservice URL in the following format:

{% highlight http %}
?parameter=value[&parameter=value] 
{% endhighlight %}

<br/>Example: 

{% highlight http %}
?deep=true&orderBy=4 asc
{% endhighlight %}

{{ site.images['info'] }} If the parameter contains lists of ids it needs to be surrounded by `{` and `}`, the values within the list are separated by `,`.

## {{page.sections['security']}}
Access to PiWeb server service might require authentication. Authentication can be either *basic authentication* based on username and password or *Windows authentication* based on Active Directory integration.

If PiWeb Server is secured by basic authentication you have to pass the credentials in the HTTP Authorization header. The authorization header must contain the `Basic` key word followed by base64 encoded `user:password` string:

{% highlight http %}

Authorization: Basic QWRtaW5pc3RyYXRvcjphZG0hbiFzdHJhdDBy

{% endhighlight %}

## {{page.sections['envelope']}}
Every response, excluding streamed data responses, consists of a response envelope which includes meta data and the data returned by the webservice. A typical response envelope looks as follows:

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "OK"
   },
   "category": "Success",
   "data":
   {...}
}
{% endhighlight %}

The possible status codes and their meanings can be found above in [{{page.sections['codes']}}](#{{page.sections['codes'] | downcase | replace:' ','-' }}).
