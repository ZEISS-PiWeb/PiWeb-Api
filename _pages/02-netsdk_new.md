---
title: .NET SDK
version: 1.0
permalink: "/sdk_new/"
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
  basics:
    title: Basics
    anchor: basics
    secs:
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

<div class="dropdown" style="float:right;">
  <button class="btn btn-default dropdown-toggle" type="button" id="dropdownMenu1" data-toggle="dropdown" aria-haspopup="true" aria-expanded="true">
    {{page.version}}
    <span class="caret"></span>
  </button>
  <ul class="dropdown-menu dropdown-menu-right" aria-labelledby="dropdownMenu1">
    <li><a href="#">2.0</a></li>
    <li role="separator" class="divider"></li>
    <li><a href="#">1.0</a></li>
  </ul>
</div>

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

The .NET SDK is suitable for all .NET languages. You can include it to your project either by

- including the dll by using our [nuget package](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Client/) **recommended**
- using the source files from our [GitHub repository](https://github.com/ZEISS-PiWeb/PiWeb-Api).

{{ site.headers['bestPractice'] }} Use our sample application for better understanding

On nuget you find a [C# demo application](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Sample/) which provides an overview about the use of the most .NET SDK methods.
For proper usage of this sample application:

1. Create a WindowsForms project named 'PiWeb_HelloWorld'
2. Add PiWebApi.Sample nuget package

{% include_relative 021-general_new.md %}

<h1 id="{{page.sections['basics'].anchor}}">{{page.sections['basics'].title}}</h1>

{% include_relative 022-basics.md %}
