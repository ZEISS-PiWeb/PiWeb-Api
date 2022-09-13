PiWeb-Api
=========

[![Build on develop](https://github.com/ZEISS-PiWeb/PiWeb-Api/actions/workflows/develop.yml/badge.svg?branch=develop&event=push)](https://github.com/ZEISS-PiWeb/PiWeb-Api/actions/workflows/develop.yml)

<p align="center">
  <img src="https://github.com/ZEISS-PiWeb/PiWeb-Api/blob/master/Logo.png" />
</p>

The PiWeb-API is a communication interface for the quality data management system [ZEISS PiWeb](http://www.zeiss.com/industrial-metrology/en_de/products/software/piweb.html). The interface is based on an HTTP/S web service architecture.

#### What's the PiWeb-API?

The PiWeb-API provides an extensive set of web service endpoints for reading and writing measurement and quality data from and to the PiWeb server. With these HTTP/S endpoints it is very easy to read and write the inspection plan structure as well as measurements and measurement values.

Additional to the HTTP/S based REST API there is also a .NET client library, which is available on NuGet.org: [Zeiss.PiWeb.Api.Rest](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/).
>API .NET SDK v6.0.0 introduced major architectural changes. Updating to version 6.0.0? Please follow our **[migration guide](http://zeiss-piweb.github.io/PiWeb-Api/sdk/v6.0/#migration)** to adapt your application to recent changes!

#### Learn more

* Read the [API documentation](http://zeiss-piweb.github.io/PiWeb-Api)
* Get the [.NET based API NuGet](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/)
* Get the [C# sample project](https://github.com/ZEISS-PiWeb/PiWeb-Training)

#### Questions, bugs or improvement ideas?
Feel free to create an [issue](https://github.com/ZEISS-PiWeb/PiWeb-Api/issues) in this repository and we will try to help you as fast as possible. Please remember to not post sensitive information like server URLs directly from log files, or at least replace them with a placeholder.

### Release Notes

As described [here](http://zeiss-piweb.github.io/PiWeb-Api/general#gi-versioning) the REST API and our .NET SDK NuGet are versioned independently.
Release Notes for our REST API can be found on their particular documentation page:

- [DataService](http://zeiss-piweb.github.io/PiWeb-Api/dataservice/v1.5/)
- [RawDataService](http://zeiss-piweb.github.io/PiWeb-Api/rawdataservice/v1.5/)

You can select the right version with the dropdown in the upper right corner. Release notes are displayed directly at the top of the page.

Release Notes for our .NET SDK NuGet are available on [NuGet.org](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/).

### Contributing

Feel free to contribute to the PiWeb API SDK. For further information about our workflow take a look at the [BUILDING.md](https://github.com/ZEISS-PiWeb/PiWeb-Api/blob/develop/BUILDING.md).


<hr>

### Examples

Detailed examples for both REST API and .NET SDK can be found in the [documentation](https://zeiss-piweb.github.io/PiWeb-Api).

##### Fetching the attribute configuration:

```http
http://your-piweb-server/dataServiceRest/configuration
```

```json
{
  "partAttributes":
  [
    {
      "key": 1001,
      "description": "part number",
      "length": 30,
      "type": "AlphaNumeric",
      "definitionType": "AttributeDefinition"
    }, "..."
  ],
  "characteristicAttributes":
  [
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
    }, "..."
  ],
  "measurementAttributes": [ "..." ],
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
  "catalogAttributes": [ "..." ]
}
```

##### Fetching all parts

```http
http://your-piweb-server/dataServiceRest/parts?depth=10000
```

```json
[
  {
    "path": "/",
    "charChangeDate": "2015-03-26T08:56:51.487Z",
    "attributes": { },
    "uuid": "00000000-0000-0000-0000-000000000000",
    "version": 0,
    "timestamp": "2014-10-07T13:39:34.74Z",
    "current": true
  },  "..."
]
```

##### Fetching the 10 most recent measurements

```http
http://your-piweb-server/dataServiceRest/measurements?limitResult=10
```

```json
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
  }, "..."
]
```
