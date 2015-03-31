PiWeb-Api
=========

<img align="center" src="Logo.png" alt="PiWeb Logo" width="200"/>

PiWeb-API is a communication interface for the quality data managament system [ZEISS PiWeb](http://www.zeiss.com/industrial-metrology/en_de/products/software/piweb.html). The interface is based on a HTTP/S web service architecture.

#### What is PiWeb-API?

PiWeb-API provides an extensive set of web service endpoints for reading and writing measurement and quality data into the PiWeb server. It is very easy with these HTTP/S based endpoints to read and write the inspection plan structure as well as measurement and measurement values.

#####Fetching the attribute configuration:

```http
http://your-piweb-server/dataServiceRest/configuration
```

```json
{

    "status": { ... } ,
    "category": "Success",
    "data":
    {
        "partAttributes": 
        [
            {
                "key": 1001,
                "description": "part number",
                "length": 30,
                "type": "AlphaNumeric",
                "definitionType": "AttributeDefinition"
            }, ...
        ],
        "characteristicAttributes":
        [
            {
                "key": 2001,
                "description": "characteristic number",
                "length": 20,
                "type": "AlphaNumeric",
                "definitionType": "AttributeDefinition"
            },
            {

                "key": 2101,
                "description": "Nominal",
                "length": 0,
                "type": "Float",
                "definitionType": "AttributeDefinition"
            },
            {

                "key": 2110,
                "description": "Lower tolerance",
                "length": 0,
                "type": "Float",
                "definitionType": "AttributeDefinition"
            },
            {
                "key": 2111,
                "description": "Upper tolerance",
                "length": 0,
                "type": "Float",
                "definitionType": "AttributeDefinition"
            }, ...
        ],
        "measurementAttributes": [ ... ],
        "valueAttributes":
        [
            {
                "key": 1,
                "description": "measured value",
                "length": 0,
                "type": "Float",
                "definitionType": "AttributeDefinition"
            }
        ],
        "catalogAttributes": [ ... ]
    }
}
```

#####Fetching all parts

```http
http://your-piweb-server/dataServiceRest/parts?depth=10000
```

```json
{
    "status": { ... },
    "category": "Success",
    "data":
    [
        {
            "path": "/",
            "charChangeDate": "2015-03-26T08:56:51.487Z",
            "attributes": { },
            "uuid": "00000000-0000-0000-0000-000000000000",
            "version": 0,
            "timestamp": "2014-10-07T13:39:34.74Z",
            "current": true
        },  ...
   ]
}
```

#####Fetching the last 10 measurements

```http
http://your-piweb-server/dataServiceRest/measurements?limitResult=10
```

```json
{
    "status": 
    { ... },
    "category": "Success",
    "data":
    [
       {
		    "uuid": "64a47361-9b5b-43e3-9774-45e2862e65ab",
		    "partUuid": "4ce9ba9a-794f-4e57-beb2-c84612065179",
		    "lastModified": "2015-03-26T08:51:29.343Z",
		    "attributes": 
		    {
		        "4": "2014-12-05T14:25:55Z"
		    }
		},
		{
		    "uuid": "c0b784f1-d85b-4c46-8f39-7824567004aa",
		    "partUuid": "4ce9ba9a-794f-4e57-beb2-c84612065179",
		    "lastModified": "2015-03-26T08:48:35.14Z",
		    "attributes": 
		    {
		        "4": "2014-12-05T13:25:55Z"
		    }
		},        
		...
   ]
}
```

#### Learn more

* Read the [API documentation guide](http://zeiss-piweb.github.io/PiWeb-Api)
* Get the [C# sample project](https://github.com/zeiss-piweb/zeiss-piweb/tree/master/sdk/samples)