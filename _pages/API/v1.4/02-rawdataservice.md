---
area: restApi
level: 0
version: 1.4
title: Rawdata Service
permalink: /rawdataservice/v1.4/
layout: frame1
sections:
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

<h1 id="{{page.sections['rawdataservice'].anchor}}">{{page.sections['rawdataservice'].title}}</h1>

{% include_relative 013-rawdataservice/0131-rawdataservice-serviceinformation.md %}

{% include_relative 013-rawdataservice/0132-rawdataservice-rawdatainformation.md %}

{% include_relative 013-rawdataservice/0133-rawdataservice-rawdataobjects.md %}
