<h1 id="{{page.sections['migration'].anchor}}">{{page.sections['migration'].title}}</h1>
<hr>

>{{ site.images['info'] }} Version `10.0` of our .NET SDK NuGet introduced a separate caching-related NuGet package. This migration guide will help you migrate from version `9.X` to NuGet version `10.0`.

<h2 id="{{page.sections['migration']['secs']['nuget'].anchor}}">.NET SDK NuGet Version 10.0</h2>

In version `10.0` of our .NET SDK NuGet package we introduced architectural changes. We added an additional NuGet: [Zeiss.Piweb.Api.Rest.Caching](https://www.nuget.org/packages/Zeiss.PiWeb.Api.Rest.Caching). This Nuget contains classes related to caching of HTTP and HTTPS requests.

<h2 id="{{page.sections['migration']['secs']['structure'].anchor}}">{{page.sections['migration']['secs']['structure'].title}}</h2>

The new NuGet structure aims to provide a clearer architecture and distinguish dependency levels. The following image provides an overview of the new structure:

<img src="/PiWeb-Api/images/v10/nuget_structure.png" class="img-responsive center-block">

<h2 id="{{page.sections['migration']['secs']['migrate'].anchor}}">{{page.sections['migration']['secs']['migrate'].title}}</h2>

The following steps describe how to migrate from version 9.X to version 10.0.

<h4>Using the new package</h4>

The new package is needed only if you enable caching for the DataServiceRestClient by providing implementations of `ICacheStore` and `IVaryHeaderStore` for the relevant properties of the wrapped `RestClient` (named CacheStore and VaryHeaderStore). These two properties have been removed and replaced with the property `CachingHandlerFactory`. This moves the dependencies for these implementations into the new package. You can either provide your own implementation for the interface `ICachingHandlerFactory` or use the provided implementation from our package, which simply requires implementations of `ICacheStore` and `IVaryHeaderStore` in the constructor, as before.

The available implementations are unchanged.

ICacheStore:
- `InMemoryCacheStore` (provided by the external CacheCow library)
- `FilesystemCacheStore` (from Zeiss.Piweb.Api.Rest.Caching)

IVaryHeaderStore:
- `InMemoryVaryHeaderStore` (provided by the external CacheCow library)
- `FilesystemVaryHeaderStore` (from Zeiss.Piweb.Api.Rest.Caching)

Please note that the filesystem-based implementations are basic and may not be suitable for production use; for example, they do not impose a maximum cache size or provide a cleanup mechanism. Assess whether they meet your needs, or provide more sophisticated implementations of `ICacheStore` and `IVaryHeaderStore`.

<h4>Other changes</h4>

Due to the extraction of caching into the separate NuGet package, the `RestClientBuilder` no longer keeps an internal cache. This means that the builder no longer implements `IDisposable`, existing `usings` need to be removed.