---
area: piwebapi
title: PiWeb API
permalink: "/piwebapi/"
sections:
  general:
    title: General Information
    anchor: gi
    secs:
     whatis:
        title: What is PiWeb API?
        anchor: gi-api
     requirements:
        title: Requirements
        anchor: gi-requirements
---

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

<h2 id="{{page.sections['general']['secs']['whatis'].anchor}}">{{page.sections['general']['secs']['whatis'].title}}</h2>

PiWeb API is a HTTP/S web service based interface for the quality data management system ZEISS PiWeb. It provides a [REST webservice based Api](/PiWeb-API/restapi) as well as a [.NET SDK](/PiWeb-API/sdk) and consists of an extensive set of web service endpoints for reading and writing measurement and quality data directly from and to PiWeb server. With these interfaces it is very easy to read, write, update or delete inspection plan structure as well as measurements, measurement values and raw data.

<h2 id="{{page.sections['general']['secs']['requirements'].anchor}}">{{page.sections['general']['secs']['requirements'].title}}</h2>

In order to use PiWeb API you need to have PiWeb server 5.8 or higher running.
