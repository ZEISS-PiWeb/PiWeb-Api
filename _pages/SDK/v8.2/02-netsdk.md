---
area: sdk
title: .NET SDK
level: 0
version: 8.2
isCurrentVersion: false
permalink: "/sdk/v8.2/"
sections:
  general:
    title: General Information
    anchor: gi  
  basics:
    title: Basics
    anchor: ba
    secs:
      create:
        title: Creating the client
        anchor: ba-create
      configuration:
        title: Configuration
        anchor: ba-configuration
      inspectionPlan:
        title: Inspection plan
        anchor: ba-inspectionPlan
      measurementsValues:
        title: Measurements and values
        anchor: ba-measurementsValues
      rawData:
        title: RawData
        anchor: ba-rawData
      security:
        title: Security
        anchor: ba-security
  migration:
    title: Migration Guide
    anchor: migration
    secs:
      nuget:
        title: NuGet Version 6.0.0
        anchor: mig-nuget
      structure:
        title: New structure
        anchor: mig-structure
      migrate:
        title: Migration
        anchor: mig-migration


---

<div class="alert alert-info fade in">
  <a href="#" class="close" data-dismiss="alert" aria-label="close">&times;</a>
  This page is still a work in progress! It is possible that not all information are up to date!
</div>

{% include version_combobox.html %}

{% include_relative 021-general.md %}

{% include_relative 022-basics.md %}

<p class="dottedline" />

{% include_relative 023-migration.md %}

<p class="dottedline" />
