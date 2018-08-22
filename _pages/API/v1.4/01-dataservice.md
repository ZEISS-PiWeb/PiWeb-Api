---
area: restApi
level: 0
version: 1.4
title: Data Service
permalink: /dataservice/v1.4/
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
---

{% include version_combobox.html %}

<h1 id="{{page.sections['dataservice'].anchor}}">{{page.sections['dataservice'].title}}</h1>

{% include_relative 012-dataservice/0121-dataservice-serviceinformation.md %}

{% include_relative 012-dataservice/0122-dataservice-configuration.md %}

{% include_relative 012-dataservice/0123-dataservice-catalogs.md %}

{% include_relative 012-dataservice/0124-dataservice-inspectionplan.md %}

{% include_relative 012-dataservice/0125-dataservice-measurementsandvalues.md %}
