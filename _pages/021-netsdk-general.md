---
area: sdk
level: 1
category: general
title: .NET SDK
subTitle: General Information
permalink: /sdk/general/
sections:
  create: Creating the client
  use: Using the client
---
<br/><br/>
The .NET SDK can be used for all .NET languages. The source code examples in this documentation are written in C#.
The two main entry classes of the SDK are the *DataServiceRestClient* and the *RawDataServiceRestClient*.

##{{page.sections['create']}}

 The .NET REST clients provide multiple constructor overloads.

###Data Service

{% capture table %}
Constructor method | Description
-------------------|-------------
```public DataServiceRestClient( string serverUri )``` | Instantiates the client with the server uri passed as string.
```public DataServiceRestClient( Uri serverUri )``` | Instantiates the client with the server uri passed as Uri object.
```public DataServiceRestClient( string scheme, string host, int port, string instance = null )``` | Instantiates the client with the given uri segments.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

###Rawdata Service

{% capture table %}
Constructor method | Description
-------------------|-------------
```public RawDataServiceRestClient( string serverUri )``` | Instantiates the client with the server uri passed as string.
```public RawDataServiceRestClient( Uri serverUri )``` | Instantiates the client with the server uri passed as Uri object.
```public RawDataServiceRestClient( string scheme, string host, int port, string instance = null )``` | Instantiates the client with the given uri segments.
{% endcapture %}
{{ table | markdownify | replace: '<table>', '<table class="table table-hover">' }}

{{ site.headers['example'] }} A data service client for a server using https on host "piwebserver" on port 8080.

{% highlight csharp %}
var dataserviceRestClient = new DataServiceRestClient( "https", "piwebserver", 8080 );
{% endhighlight %}

{{ site.headers['example'] }} A raw data service client pointed to "http:piwebserver:8082"

{% highlight csharp %}
var rawdataserviceRestClient = new RawDataServiceRestClient( "http:piwebserver:8082" );
{% endhighlight %}

##{{page.sections['use']}}

To make a webservice request you can use the corresponding methods provided by the client classes. You can find detailed descriptions and examples in the corresponding documentation section. Each method runs asynchronously and returns an awaitable `Task`. The result will be available once the `Task` has completed. All methods accept a `CancellationToken` which you can use to cancel a request.
