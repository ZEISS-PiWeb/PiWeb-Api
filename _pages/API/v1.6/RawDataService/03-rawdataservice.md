---
area: rawDataService
level: 0
version: 16
displayVersion: "1.6"
title: Rawdata Service
permalink: /rawdataservice/v1.6/
sections:
  rawdataservice:
    title: Raw Data Service
    iconName: rawdataservice16
    anchor: rs
    secs:
      releaseNotes:
        title: Release Notes
        anchor: rs-release-notes
      interfaceInformation:
        title: Interface Information
        anchor: rs-interface-information
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

{% include_relative 030-rawdataservice-releasenotes.md %}

{% include_relative 031-rawdataservice-interfaceinformation.md %}

{% include_relative 032-rawdataservice-serviceinformation.md %}

{% include_relative 033-rawdataservice-rawdatainformation.md %}

{% include_relative 034-rawdataservice-rawdataobjects.md %}

{% include_relative 035-rawdataservice-archivelookup.md %}
