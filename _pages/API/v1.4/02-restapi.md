---
area: restApi
level: 0
version: 1.4
title: REST Api
permalink: /restapi/v1.4/
layout: frame1
sections:
  dataservice:
    title: Data Service
    iconName: dataservice16
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
        title: Parts and Characteristics
        anchor: ds-inspection-plan
      measurementsAndValues:
        title: Measurements and Values
        anchor: ds-measurements-and-values
  rawdataservice:
    title: Raw Data Service
    iconName: rawdataservice16
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

{% include version_combobox.html %}

<h1 id="{{page.sections['dataservice'].anchor}}">{{page.sections['dataservice'].title}}</h1>

{% include_relative 012-dataservice/0121-dataservice-serviceinformation.md %}

{% include_relative 012-dataservice/0122-dataservice-configuration.md %}

{% include_relative 012-dataservice/0123-dataservice-catalogs.md %}

{% include_relative 012-dataservice/0124-dataservice-inspectionplan.md %}

{% include_relative 012-dataservice/0125-dataservice-measurementsandvalues.md %}

<p class="dottedline" />

<h1 id="{{page.sections['rawdataservice'].anchor}}">{{page.sections['rawdataservice'].title}}</h1>

{% include_relative 013-rawdataservice/0131-rawdataservice-serviceinformation.md %}

{% include_relative 013-rawdataservice/0132-rawdataservice-rawdatainformation.md %}

{% include_relative 013-rawdataservice/0133-rawdataservice-rawdataobjects.md %}
