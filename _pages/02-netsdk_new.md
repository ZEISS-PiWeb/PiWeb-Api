---
title: .NET SDK
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
---

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

The .NET SDK is suitable for all .NET languages. You can include it to your project either by

- using the source files from our [GitHub repository](https://github.com/ZEISS-PiWeb/PiWeb-Api) or
- including the dll by using our [nuget package](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Client/).

On nuget you can also find a [C# demo application](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Sample/) which provides an overview about the use of the most .NET SDK methods.

{% include_relative 021-general_new.md %}

<h1 id="{{page.sections['basics'].anchor}}">{{page.sections['basics'].title}}</h1>

{% include_relative 022-basics.md %}
