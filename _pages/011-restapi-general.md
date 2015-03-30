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
{{ site.images['info'] }} The instanceName and https are optional and depend on the server settings.

## {{page.sections['formats']}}

The input and output format is JSON as it is the most performance and memory efficient format at the moment.

## {{page.sections['codes']}}

{% capture table %}
Method        | Statuscode Ok        | Statuscode Failure                                       | Comment
------------- | :------------------- | -------------------------------------------------------- | -------
GET           | **200** (OK)         | **400** (Bad request) - Request failed <br> **404** (Not found) - Endpoint(s) or item does not exist | --- <br> If none of the requested items exist, status code 404 is returned. Otherwise status code 200 is returned.
POST           | **201** (Created)    | **400** (Bad request) – Request failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint doesn't exist <br> **409** (Conflict) – Resource already exists | Status code 400 is returned if the creation of at least one item failed. <br> --- <br><br> Status code 409 is returned if one or more items already existed.
PUT          | **200** (Ok)         | **400** (Bad request) –  Request failed, e.g. due to bad formatting <br> **404** (Not found) – Endpoint or item(s) doesn't exist | Status code 400 is returned if the update of at least one item failed. <br> Status code 404 is returned if one or more items didn't exist.
DELETE        | **200** (Ok) | **400** (Bad request) – Request failed <br><br> **404** (Not found) – Endpoint or item(s) doesn't exist | Status code 400 is returned if deletion of one or more items failed. <br> Status code 404 is returned if none of the requested items exist.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

## {{page.sections['parameter']}}

You can restrict requests by attaching several parameters to the endpoint of the webservice URL in the following format:

{% highlight http %}
?parameter=value[&parameter=value] 
{% endhighlight %}

<br/>Example: 

{% highlight http %}
?deep=true&orderBy=4 asc
{% endhighlight %}

{{ site.images['info'] }} If the parameter contains lists of ids guids it needs to be surrounded by “{“ and “}”, the values within the list are separated by “,”.

## {{page.sections['security']}}
The access to PiWeb database might be secured by access control settings. 

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
