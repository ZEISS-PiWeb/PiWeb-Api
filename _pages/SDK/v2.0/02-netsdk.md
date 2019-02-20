---
area: sdk
title: .NET SDK
level: 0
version: 2.0
isCurrentVersion: true
permalink: "/sdk/"
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
---

<h1 id="{{page.sections['general'].anchor}}">{{page.sections['general'].title}}</h1>

The .NET SDK is suitable for all .NET languages. You can include it to your project either by

- including the dll by using our [nuget package](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Client/) (recommended)
- using the source files from our [GitHub repository](https://github.com/ZEISS-PiWeb/PiWeb-Api).

{{ site.headers['bestPractice'] }} Use our sample application for better understanding

On Github you find a [C# sample application](https://github.com/ZEISS-PiWeb/PiWeb-Training) which provides several lessons about the use of the most .NET SDK methods.

{% include_relative 021-general.md %}

<p class="dottedline" />