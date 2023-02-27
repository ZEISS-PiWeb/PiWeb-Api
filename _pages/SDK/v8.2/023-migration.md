<h1 id="{{page.sections['migration'].anchor}}">{{page.sections['migration'].title}}</h1>
<hr>

>{{ site.images['info'] }} Version `8.0.0` of our .NET SDK NuGet introduced breaking changes. This migration guide will help you migrate from version `7.X.X` to a NuGet version `>=8.0.0`.

<h2 id="{{page.sections['migration']['secs']['nuget'].anchor}}">.NET SDK NuGet Version 8.0.0</h2>

With Version `8.0.0` of our .NET SDK NuGet we introduced more architectural changes. We added an additional NuGet: [Zeiss.Piweb.Api.Core](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Core/). This Nuget contains core classes of the API which were former DTOs, but contain a certain amount of logic. This classes were moved and renamed to not contain the *Dto*-suffix. 

<h2 id="{{page.sections['migration']['secs']['structure'].anchor}}">{{page.sections['migration']['secs']['structure'].title}}</h2>

The new NuGet structure aims for clearer architecture and different levels of dependencies. The following image gives you a short impression of the new structure:

<img src="/PiWeb-Api/images/v8/nuget_structure.png" class="img-responsive center-block">

<h2 id="{{page.sections['migration']['secs']['migrate'].anchor}}">{{page.sections['migration']['secs']['migrate'].title}}</h2>

The following steps have to be done to migrate from version 7.2 to version 8.0.

<h4>Referencing the new package</h4>

The new package will automatically be referenced when updating either the package `Zeiss.PiWeb.Api.Rest` or `Zeiss.PiWeb.Api.Rest.Dtos`, since it is a transitive dependency. You do not need to do anything when your are just using `Zeiss.PiWeb.Api.Definitions`.

<h4>Renaming of Data Transfer Object classes</h4>

You need to remove the *Dto*-suffix from the following classes or interfaces if your are referencing them in your application:
- Attribute
- AttributeItemExtensions
- IAttributeItem
- PathInformation
- PathInformationExtension
- PathElement
- InspectionPlanEntity
- PathHelper

It can be neccessary to add an explicit using, e.g. `using Attribute = Zeiss.PiWeb.Api.Core.Attribute;` when encountering ambiguous references.

<h4>Other changes</h4>

Many return types and method parameters which were former Arrays are now ReadOnlyList or ReadOnlyCollections. This removes many `.ToArrays` when using methods but also prevents certain operations on the fetched result lists.
You can still edit entries, e.g. fetched parts, but you cannot append new entries. It can be needed to maintain your own list instead of working directly with the result set or property of a DTO.

The class `DataCharacteristicDto` was removed, since it had no real benefit (only a simple container for the characteristic uuid and the value). It was replaced by a dictionary of uuid and `DataValueDto`, see `DataMeasurement`: the property
*Characteristics* was formerly an array of `DataCharacteristicDto` and is now an `IReadOnlyDictionary<Guid, DataValueDto>`.
