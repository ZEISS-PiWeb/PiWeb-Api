---
area: sdk
title: .NET SDK
level: 0
version: 2.0.0
permalink: "/sdk/v2.0.0"
sections:
  general:
    title: General Information
    anchor: gi
    secs:
      create:
        title: Creating the client
        anchor: gi-create
      use:
        title: Using the client
        anchor: gi-use
      security:
        title: Security
        anchor: gi-security
  basics:
    title: Basics
    anchor: basics
    secs:
      configuration:
        title: Configuration
        anchor: basics-configuration
      inspectionPlan:
        title: Inspection Plan
        anchor: basics-inspectionPlan
      measurementsValues:
        title: Measurements and Values
        anchor: basics-measurementsValues
      rawData:
        title: Raw Data
        anchor: basics-rawData
---

{% include version_combobox.html %}

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

The .NET SDK is suitable for all .NET languages. You can include it to your project either by

- including the dll by using our [nuget package](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Client/) (recommended)
- using the source files from our [GitHub repository](https://github.com/ZEISS-PiWeb/PiWeb-Api).

{{ site.headers['bestPractice'] }} Use our sample application for better understanding

On nuget you find a [C# demo application](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Sample/) which provides an overview about the use of the most .NET SDK methods.
For proper usage of this sample application:

1. Create a WindowsForms project named 'PiWeb_HelloWorld'
2. Add PiWebApi.Sample nuget package

{% include_relative 021-general.md %}

<p class="dottedline" />

<h1 id="{{page.sections['basics'].anchor}}">{{page.sections['basics'].title}}</h1>

{% include_relative 022-basics/0221-configuration.md %}

{% include_relative 022-basics/0223-basics-inspectionplan.md %}

{% include_relative 022-basics/0224-basics-measurementsandvalues.md %}

{% include_relative 022-basics/0225-basics-rawdata.md %}