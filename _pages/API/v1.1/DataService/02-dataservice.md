---
area: dataService
level: 0
version: 1.1
title: Data Service
permalink: /dataservice/v1.1/
sections:
  dataservice:
    title: Data Service
    iconName: dataservice16
    anchor: ds
    secs:
      releaseNotes:
        title: Release Notes
        anchor: ds-release-notes
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
---

{% include version_combobox.html %}

<h1 id="{{page.sections['dataservice'].anchor}}">{{page.sections['dataservice'].title}}</h1>

{% include_relative 020-dataservice-releasenotes.md %}

{% include_relative 021-dataservice-serviceinformation.md %}

{% include_relative 022-dataservice-configuration.md %}

{% include_relative 023-dataservice-catalogs.md %}

{% include_relative 024-dataservice-inspectionplan.md %}

{% include_relative 025-dataservice-measurementsandvalues.md %}
