---
area: restApi
level: 0
title: REST Api
redirect_from: "/"
permalink: "/restapi/"
sections:
  general:
    title: General Information
    anchor: gi
    secs:
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
  dataservice:
    title: Data Service
    anchor: ds
    secs:
      serviceInformation:
        title: Service Information
        anchor: ds-service-information
      configuration:
        title: Configuration
        anchor: ds-configuration
      catalogs:
        title: Catalogs
        anchor: ds-catalogs
      inspectionPlan:
        title: Inspection Plan
        anchor: ds-inspection-plan
      measurementsAndValues:
        title: Measurements And Values
        anchor: ds-measurements-and-values
  rawdataservice:
    title: Raw Data Service
    anchor: rs
    secs:
      serviceInformation:
        title: Service Information
        anchor: rs-service-information
      rawDataInformation:
        title: Raw Data Information
        anchor: rs-raw-data-information
      rawDataObjects:
        title: Raw Data Objects
        anchor: rs-raw-data-objects
---

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

{% include_relative 011-general.md %}

<p class="dottedline" />

<img src="/PiWeb-Api/images/database-table.png">
<h1 id="{{page.sections['dataservice'].anchor}}">{{page.sections['dataservice'].title}}</h1>

{% include_relative 012-dataservice.md %}

<p class="dottedline" />

<h1 id="{{page.sections['rawdataservice'].anchor}}">{{page.sections['rawdataservice'].title}}</h1>

{% include_relative 013-rawdataservice.md %}
