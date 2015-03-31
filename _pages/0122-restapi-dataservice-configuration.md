---
area: restApi
level: 2
category: dataService
subCategory: configuration
title: REST API
subTitle: Data Service
pageTitle: Configuration
permalink: /restapi/dataservice/configuration/
---

## {{ page.pageTitle }}

### Endpoints

The configuration can be fetched, created, updated and deleted using the following endpoints. These endpoints do not provide filter parameters.

{% assign linkId="configurationEndpointGet" %}
{% assign method="GET" %}
{% assign endpoint="/configuration" %}
{% assign summary="Returns the attribute definitions for all entity types" %}
{% assign description="" %}
{% assign exampleCaption="Fetching the configuration including all attributes" %}

{% capture jsonrequest %}
{% highlight http %}
GET /dataServiceRest/configuration HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight json %}
{
   ...
   "data":
   [
       {
          "partAttributes":
          [
           [
               "key":1001,
               "description":"partNumber",
               "length":30,
               "type":"AlphaNumeric",
               "definitionType":"AttributeDefinition"
           ],
           ...
          ],
          "characteristicAttributes":
          [
           [
               "key":2001,
               "description":"characteristicNumber",
               "length":20,
               "type":"AlphaNumeric",
               "definitionType":"AttributeDefinition"
           ],
           ...
          ],
          "measurementAttributes":
          [
                "key": 8,
                "description": "inspector",
                "catalogue": "8c376bee-ffe3-4ee4-abb9-a55b492e69ad",
                "definitionType": "CatalogueAttributeDefinition"
          ...
          ],
          "valueAttributes":
          [
          ...
          ],
          "catalogueAttributes":
          [
          ...
          ]
       }
   ]
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="configurationEndpointAdd" %}
{% assign method="POST" %}
{% assign endpoint="/configuration/:entityType" %}
{% assign summary="Creates the attribute(s) for :entityType" %}
{% assign description="Creates the attribute definition(s) transfered within the body of the request for the given `:entityType` which must be part of the uri" %}
{% assign exampleCaption="Adding a part attribute with the key 1001 to the configuration" %}

{% capture jsonrequest %}
{% highlight http %}
POST /dataServiceRest/configuration/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "key":1001,
    "description":"partNumber",
    "length":30,
    "type":"AlphaNumeric",
    "definitionType":"AttributeDefinition"
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 201 Created
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 201,
       "statusDescription": "Created"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="configurationEndpointUpdate" %}
{% assign method="PUT" %}
{% assign endpoint="/configuration/:entityType" %}
{% assign summary="Updates the attribute(s) for :entityType" %}
{% assign description="Updates the attribute definition(s) transfered within the body of the request for the given `:entityType` which must be part of the uri" %}
{% assign exampleCaption="Updating the part attribute with key 1001 - change length from 30 to 50" %}

{% capture jsonrequest %}
{% highlight http %}
PUT /dataServiceRest/configuration/parts HTTP/1.1
{% endhighlight %}

{% highlight json %}
[
  {
    "key":1001,
    "description":"partNumber",
    "length":50,
    "type":"AlphaNumeric",
    "definitionType":"AttributeDefinition"
  }
]
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="configurationEndpointDelete1" %}
{% assign method="DELETE" %}
{% assign endpoint="/configuration" %}
{% assign summary="Deletes all attribute definitions" %}
{% assign description="" %}
{% assign exampleCaption="Delete all attributes of the current configuration" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/configuration HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}


{% assign linkId="configurationEndpointDelete2" %}
{% assign method="DELETE" %}
{% assign endpoint="/configuration/:entityType/:attributesIdList" %}
{% assign summary="Deletes the attribute(s) in :attributedIdList for :entityType" %}
{% assign description="Deletes the attribute definitions identified by the list of attribute definition ids for the given :entityType. If the list of attribute definition ids is empty, all attributes for the given :entityType are deleted." %}
{% assign exampleCaption="Delete the part attributes with the keys 1001 and 1002" %}

{% capture jsonrequest %}
{% highlight http %}
DELETE /dataServiceRest/configuration/part/(1001, 1002) HTTP/1.1
{% endhighlight %}
{% endcapture %}

{% capture jsonresponse %}
{% highlight http %}
HTTP/1.1 200 Ok
{% endhighlight %}

{% highlight json %}
{
   "status":
   {
       "statusCode": 200,
       "statusDescription": "Ok"
   },
   "category": "Success"
}
{% endhighlight %}
{% endcapture %}

{% include endpointTab.html %}

### General Information

The PiWeb configuration consists of a list of attributes for all types of entities. 
The different types of entites are: 

* *parts*, 
* *characteristics*, 
* *measurements*, 
* *values* and 
* *catalogues*.

The attributes are either ```AttributeDefinition``` or ```CatalogueAttributeDefinition```.
{% capture table %}
####AttributeDefinition

Property      | Type                | Description
--------------|---------------------|--------------------------------------------------------------
key           | ```ushort```        | The attribute's key, by which the attribute can be uniquely identified
description   | ```string```        | The attribute's name or a short description 
type          | ```AttributeType``` | The attribute's type. *AlphaNumeric*, *Integer*, *Float* or *DateTime*
length        | ```ushort```        | The attribute's maximum length. Only set if the type is *AlphaNumeric*
definitionType| ```string```        | Always has the value 'AttributeDefinition' and is used to differentiate between  ```AttributeDefinition``` and ```CatalogueAttributeDefinition```

####CatalogueAttributeDefinition

Property      | Type         | Description
--------------|--------------|------------------------------------------------------------
key           | ```ushort``` | The attribute's key, by which the attribute can be uniquely identified
description   | ```string``` | The attribute's name or a short description 
catalogue     | ```Guid```   | The uuid of the catalogue that contains the attribute's values
definitionType| ```string``` | Always has the value 'CatalogueAttributeDefinition' and is used to differentiate between  ```AttributeDefinition``` and ```CatalogueAttributeDefinition```
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}
