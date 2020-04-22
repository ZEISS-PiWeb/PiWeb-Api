#tool "nuget:?package=GitVersion.CommandLine"
#addin "nuget:?package=Cake.FileHelpers"
#addin nuget:?package=Cake.VersionReader

//////////////////////////////////////////////////////////////////////
// ARGUMENTS
//////////////////////////////////////////////////////////////////////

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");

//////////////////////////////////////////////////////////////////////
// PREPARATION
//////////////////////////////////////////////////////////////////////

// Define directories.
var srcDir = Directory("./src");
var solutionFile = srcDir + File("Api.sln");
var projectFile_Definitions = srcDir + Directory("Api.Definitions") + File("Api.Definitions.csproj");
var projectFile_Dtos = srcDir + Directory("Api.Rest.Dtos") + File("Api.Rest.Dtos.csproj");
var projectFile_Client = srcDir + Directory("Api.Rest") + File("Api.Rest.csproj");

var buildDir_Definitions = srcDir + Directory("Api.Definitions")+ Directory("bin") + Directory(configuration);
var buildDir_Dtos = srcDir + Directory("Api.Rest.Dtos")+ Directory("bin") + Directory(configuration);
var buildDir_Client = srcDir + Directory("Api.Rest")+ Directory("bin") + Directory(configuration);

var artifactsDir = Directory("./artifacts");

var nugetVersion = "0.0.0";
var isDeveloperBuild = BuildSystem.IsLocalBuild;

var gitVersionInfo = GitVersion(new GitVersionSettings {
    OutputType = GitVersionOutput.Json
});

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Teardown(context =>
{
    Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
// PRIVATE TASKS
//////////////////////////////////////////////////////////////////////

Task("UpdateAssemblyInfo")
    .Does(() =>
{
    gitVersionInfo = GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true,
        OutputType = GitVersionOutput.Json
    });

    nugetVersion = isDeveloperBuild ? "0.0.0" : gitVersionInfo.NuGetVersion;

    Information("AssemblyVersion -> {0}", gitVersionInfo.AssemblySemVer);
    Information("AssemblyFileVersion -> {0}", $"{gitVersionInfo.MajorMinorPatch}.0");
    Information("AssemblyInformationalVersion -> {0}", gitVersionInfo.InformationalVersion);

    if(BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    {
        Information("Nuget Version -> {0}", nugetVersion);
    }
    else
    {
        Warning("Nuget Version -> {0} (developer build)", nugetVersion);
    }
});

Task("AppVeyorSetup")
    .IsDependentOn("UpdateAssemblyInfo")
    .Does(() =>
{
    if(BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    {
        var appVeyorBuildNumber = EnvironmentVariable("APPVEYOR_BUILD_NUMBER");
        var appVeyorBuildVersion = $"{nugetVersion}+{appVeyorBuildNumber}";
        Information("AppVeyor branch name is " + EnvironmentVariable("APPVEYOR_REPO_BRANCH"));
        Information("AppVeyor build version is " + appVeyorBuildVersion);
        BuildSystem.AppVeyor.UpdateBuildVersion(appVeyorBuildVersion);
    }
});

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir_Definitions);
    CleanDirectory(buildDir_Dtos);
    CleanDirectory(buildDir_Client);
    CleanDirectory(artifactsDir);
});

Task("Restore")
    .Does(() =>
{
    NuGetRestore(solutionFile);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .IsDependentOn("UpdateAssemblyInfo")
    .Does(() =>
{
    MSBuild(solutionFile, settings =>
        settings.SetConfiguration(configuration));
});

Task("Pack_Definitions")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nugetVersion_Definitions = GetAssemblyVersion(buildDir_Definitions, "Zeiss.PiWeb.Api.Definitions.dll");
    Information("AssemblyVersion of Zeiss.PiWeb.Api.Definitions -> {0}", nugetVersion_Definitions);

    var releaseNotes = FileReadLines(File("src/Api.Definitions/WHATSNEW_Definitions.txt"));
    var licenseFile = new NuSpecLicense
    {
        Type = "file",
        Value = "License.txt"
    };

    var nuGetPackSettings = new NuGetPackSettings {
        Id                       = "Zeiss.IMT.PiWebApi.Definitions",
        Version                  = nugetVersion_Definitions,
        Title                    = "Zeiss.IMT.PiWebApi.Definitions",
        Authors                  = new[] {"Carl Zeiss Innovationszentrum für Messtechnik GmbH"},
        Owners                   = new[] {"Carl Zeiss Innovationszentrum für Messtechnik GmbH"},
        Description              = "Contains generic data used by the PiWeb API. The ZEISS PiWeb API nuget depends on this package.",
        Summary                  = "Contains generic data used by the PiWeb API.",
        ProjectUrl               = new Uri("https://github.com/ZEISS-PiWeb/PiWeb-Api"),
        IconUrl                  = new Uri("https://raw.githubusercontent.com/ZEISS-PiWeb/PiWeb-Api/master/logo6464.png"),
        License                  = licenseFile,
        Copyright                = string.Format("Copyright © {0} Carl Zeiss Innovationszentrum für Messtechnik GmbH",DateTime.Now.Year),
        ReleaseNotes             = releaseNotes,
        Tags                     = new [] {"ZEISS", "PiWeb", "API", "Definitions"},
        RequireLicenseAcceptance = true,
        Files                    = new [] {
            new NuSpecContent { Source = "netstandard2.0/License.txt", Target = "" },
            new NuSpecContent { Source = "netstandard2.0/Zeiss.PiWeb.Api.Definitions.dll", Target = "lib/netstandard2.0" },
            new NuSpecContent { Source = "netstandard2.0/Zeiss.PiWeb.Api.Definitions.xml", Target = "lib/netstandard2.0" },
        },
        Dependencies             = new [] {
            new NuSpecDependency { Id = "JetBrains.Annotations", Version = "2018.2.1" }
        },
        BasePath                 = buildDir_Definitions,
        OutputDirectory          = artifactsDir
    };
    NuGetPack(nuGetPackSettings);
});

Task("Pack_Dtos")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nugetVersion_Dtos = GetAssemblyVersion(buildDir_Dtos, "Zeiss.PiWeb.Api.Rest.Dtos.dll");
    Information("AssemblyVersion of Zeiss.PiWeb.Api.Rest.Dtos -> {0}", nugetVersion_Dtos);

    var releaseNotes = FileReadLines(File("src/Api.Rest.Dtos/WHATSNEW_Dtos.txt"));
    var licenseFile = new NuSpecLicense
    {
        Type = "file",
        Value = "License.txt"
    };

    var nuGetPackSettings = new NuGetPackSettings {
        Id                       = "Zeiss.IMT.PiWebApi.Dtos",
        Version                  = nugetVersion_Dtos,
        Title                    = "Zeiss.IMT.PiWebApi.Dtos",
        Authors                  = new[] {"Carl Zeiss Innovationszentrum für Messtechnik GmbH"},
        Owners                   = new[] {"Carl Zeiss Innovationszentrum für Messtechnik GmbH"},
        Description              = "Contains the JSON-serializable DataTransferObjects (DTO) that will be exchanged between PiWeb server and client(s). The ZEISS PiWeb API nuget depends on this package.",
        Summary                  = "Contains the JSON-serializable DataTransferObjects (DTO) that will be exchanged between PiWeb server and client(s).",
        ProjectUrl               = new Uri("https://github.com/ZEISS-PiWeb/PiWeb-Api"),
        IconUrl                  = new Uri("https://raw.githubusercontent.com/ZEISS-PiWeb/PiWeb-Api/master/logo6464.png"),
        License                  = licenseFile,
        Copyright                = string.Format("Copyright © {0} Carl Zeiss Innovationszentrum für Messtechnik GmbH",DateTime.Now.Year),
        ReleaseNotes             = releaseNotes,
        Tags                     = new [] {"ZEISS", "PiWeb", "API", "Dtos"},
        RequireLicenseAcceptance = true,
        Files                    = new [] {
            new NuSpecContent { Source = "netstandard2.0/License.txt", Target = "" },
            new NuSpecContent { Source = "netstandard2.0/de/Zeiss.PiWeb.Api.Rest.Dtos.resources.dll", Target = "lib/netstandard2.0/de" },
            new NuSpecContent { Source = "netstandard2.0/Zeiss.PiWeb.Api.Rest.Dtos.dll", Target = "lib/netstandard2.0" },
            new NuSpecContent { Source = "netstandard2.0/Zeiss.PiWeb.Api.Rest.Dtos.xml", Target = "lib/netstandard2.0" },
        },
        Dependencies             = new [] {
            new NuSpecDependency { Id = "Newtonsoft.Json", Version = "12.0.1" },
            new NuSpecDependency { Id = "JetBrains.Annotations", Version = "2018.2.1" }
        },
        BasePath                 = buildDir_Dtos,
        OutputDirectory          = artifactsDir
    };
    NuGetPack(nuGetPackSettings);
});

Task("Pack_Client")
    .IsDependentOn("Build")
    .Does(() =>
{
    var nugetVersion_Client = GetAssemblyVersion(buildDir_Client, "Zeiss.PiWeb.Api.Rest.dll");
    Information("AssemblyVersion of Zeiss.PiWeb.Api.Rest -> {0}", nugetVersion_Client);

    var releaseNotes = FileReadLines(File("WHATSNEW.txt"));
    var licenseFile = new NuSpecLicense
    {
        Type = "file",
        Value = "License.txt"
    };

    var nuGetPackSettings = new NuGetPackSettings {
        Id                       = "Zeiss.IMT.PiWebApi.Client",
        Version                  = nugetVersion,
        Title                    = "ZEISS PiWeb-API .NET Client",
        Authors                  = new[] {"Carl Zeiss Innovationszentrum für Messtechnik GmbH"},
        Owners                   = new[] {"Carl Zeiss Innovationszentrum für Messtechnik GmbH"},
        Description              = "ZEISS PiWeb-API .NET Client provides an extensive set of methods for reading and writing  inspection plan structure as well as measurements and measurement values to and from ZEISS PiWeb server via HTTP(S)/REST based web service endpoints. ZEISS PiWeb server >= version 5.8 is required.",
        Summary                  = "A .NET client for HTTP(S)/REST based communication with the quality data managament system ZEISS PiWeb.",
        ProjectUrl               = new Uri("https://github.com/ZEISS-PiWeb/PiWeb-Api"),
        IconUrl                  = new Uri("https://raw.githubusercontent.com/ZEISS-PiWeb/PiWeb-Api/master/logo6464.png"),
        License                  = licenseFile,
        Copyright                = string.Format("Copyright © {0} Carl Zeiss Innovationszentrum für Messtechnik GmbH",DateTime.Now.Year),
        ReleaseNotes             = releaseNotes,
        Tags                     = new [] {"ZEISS", "PiWeb", "API"},
        RequireLicenseAcceptance = true,
        Files                    = new [] {
            new NuSpecContent { Source = "net48/license.txt", Target = "" },
            new NuSpecContent { Source = "net48/Zeiss.PiWeb.Api.Rest.dll", Target = "lib/net48" },
            new NuSpecContent { Source = "net48/Zeiss.PiWeb.Api.Rest.xml", Target = "lib/net48" },
            new NuSpecContent { Source = "netstandard2.0/Zeiss.PiWeb.Api.Rest.dll", Target = "lib/netstandard2.0" },
            new NuSpecContent { Source = "netstandard2.0/Zeiss.PiWeb.Api.Rest.xml", Target = "lib/netstandard2.0" },
            new NuSpecContent { Source = "netcoreapp3.1/Zeiss.PiWeb.Api.Rest.dll", Target = "lib/netcoreapp3.1" },
            new NuSpecContent { Source = "netcoreapp3.1/Zeiss.PiWeb.Api.Rest.xml", Target = "lib/netcoreapp3.1" },
        },
        Dependencies             = new [] {
            new NuSpecDependency { Id = "Newtonsoft.Json", Version = "12.0.1" },
            new NuSpecDependency { Id = "Newtonsoft.Json.Bson", Version = "1.0.2" },
            new NuSpecDependency { Id = "IdentityModel", Version = "2.0.0" },
            new NuSpecDependency { Id = "Microsoft.IdentityModel.Logging", Version = "5.2.1" },
            new NuSpecDependency { Id = "Microsoft.IdentityModel.Tokens", Version = "5.2.1" },
            new NuSpecDependency { Id = "System.IdentityModel.Tokens.Jwt", Version = "5.2.1" },
            new NuSpecDependency { Id = "JetBrains.Annotations", Version = "2018.2.1" },
            new NuSpecDependency { Id = "System.Security.Cryptography.ProtectedData", Version = "4.7.0" },
            new NuSpecDependency { Id = "Zeiss.IMT.PiWebApi.Definitions", Version = "1.0.0" },
            new NuSpecDependency { Id = "Zeiss.IMT.PiWebApi.Dtos", Version = "1.0.0" }
        },
        BasePath                 = buildDir_Client,
        OutputDirectory          = artifactsDir
    };
    NuGetPack(nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("AppVeyorSetup")
    .IsDependentOn("Pack_Definitions")
    .IsDependentOn("Pack_Dtos")
    .IsDependentOn("Pack_Client");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);

//////////////////////////////////////////////////////////////////////
// CUSTOM METHODS
//////////////////////////////////////////////////////////////////////

string GetAssemblyVersion(DirectoryPath buildDir, string assemblyName)
{
    return GetVersionNumber(new FilePath(GetSubDirectories(buildDir).First() + "/" + assemblyName));
}
