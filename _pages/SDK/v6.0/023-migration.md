<h1 id="{{page.sections['migration'].anchor}}">{{page.sections['migration'].title}}</h1>
<hr>

>{{ site.images['info'] }} Version `6.0.0` of our .NET SDK NuGet introduced major architectural changes. This migration guide will help you migrate from version `5.X.X` to a NuGet version `>6.0.0`

<h2 id="{{page.sections['migration']['secs']['nuget'].anchor}}">.NET SDK NuGet Version 6.0.0</h2>

With Version `6.0.0` of our .NET SDK NuGet we introduced major architectural changes. Another important change affects the PackageID of our NuGet: we renamed it according to our new naming scheme. The new PackageID is [Zeiss.PiWeb.Api.Rest](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/). This NuGet is the official successor of [Zeiss.IMT.PiWebApi.Client](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Client/), which wont get new feature and is therefore discontinued. Critical bugs may be fixed, but migrating is strongly advised for continuous support.

>{{ site.images['info'] }} The new NuGet wont show up as an update, as the PackageID changed. To migrate you need to explicitly deinstall Zeiss.IMT.PiWebApi.Client and install Zeiss.PiWeb.Api.Rest!

<h2 id="{{page.sections['migration']['secs']['structure'].anchor}}">{{page.sections['migration']['secs']['structure'].title}}</h2>

The new NuGet structure aims for clearer architecture and different levels of dependencies. The following image gives you a short impression of the new structure:

<img src="/PiWeb-Api/images/v6/nuget_structure.png" class="img-responsive center-block">

The .NET SDK API Nuget is now split into the three shown parts, and each part is its own NuGet listed on NuGet.org. The blue arrows describe their dependencies between each other. [Zeiss.PiWeb.Api.Rest](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/) needs both NuGets to function correctly, whilst [Zeiss.Piweb.Api.Rest.Dtos](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest.Dtos/) needs [Zeiss.PiWeb.Definitions](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Definitions/). When installing Zeiss.PiWeb.Api.Rest all dependencies are installed automatically in the right version.

The advantages of this splitting are lesser dependencies to client related NuGets like IdentityModel or System.IdentityModel.Tokens.Jwt when only working with API data in backend code. All PiWeb applications are communicating using the API .NET SDK. The older NuGet required all dependencies everywhere it was installed, even in backend code where only data is processed but not sent, which wasn't favorable in an architectural view. <br>
The new NuGets now can partly be used independent: You only need a key of our WellKnownKeys? No problem, just install `Zeiss.PiWeb.Definitions`. Just working with data? Install `Zeiss.Piweb.Api.Rest.Dtos`. At this point the only dependencies are Newtonsoft.Json and JetBrains.Annotations. You finally want to send or recieve data? Then install the client NuGet `Zeiss.PiWeb.Api.Rest`. This way your backend projects will stay free of client dependencies.

<h2 id="{{page.sections['migration']['secs']['migrate'].anchor}}">{{page.sections['migration']['secs']['migrate'].title}}</h2>

The following steps have to be done to migrate from version 5.0 to version 6.0.

<h4>Referencing the new packages</h4>

The very first step is the replacement of the existing Nuget package with the current version. As described above, this step requires to remove the existing reference to [Zeiss.IMT.PiWebApi.Client](https://www.nuget.org/packages/Zeiss.IMT.PiWebApi.Client/) in your projects and add a new reference to the [Zeiss.PiWeb.Api.Rest](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/) package. The default steps of updating a Nuget package does not work since version 6.0 has a different package ID than version 5.0.

If the package reference is only required for getting the data-transfer-objects, it can be useful to reference the Nuget package [Zeiss.PiWeb.Definitions](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Definitions/) instead of [Zeiss.PiWeb.Api.Rest](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest/). This might help reducing dependencies.

<h4>Renaming of Data Transfer Object classes</h4>

A well-designed application should have a domain model. In order to prevent naming collisions between the objects located in the domain model and those used for data transfer (DTO), we decided to add the *Dto*-suffix to our data-transfer-objects. After updating to version 6.0, the naming of the referenced data-transfer-objects in your code needs to be corrected.

<h4>Adapting namespaces</h4>

As part of the breakdown into three Nuget-packages, the namespaces has been changed. 
- First of all, the root namespaces has been changed from `Zeiss.IMT.PiWeb.Api` to `Zeiss.PiWeb.Api`.
- The namespace containing the interface of the REST clients (including supporting types) is `Zeiss.PiWeb.Api.Rest.Contracts`.
- The REST client implementations are located in `Zeiss.PiWeb.Api.Rest.HttpClient` and sub-namespaces.
- The namespace of the well-known-keys is `Zeiss.PiWeb.Api.Definitions`.
- The data transfer objects are located in `Zeiss.PiWeb.Api.Rest.Dtos` and sub-namespaces.
