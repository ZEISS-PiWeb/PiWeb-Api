#tool "nuget:?package=GitVersion.CommandLine"
#addin "nuget:?package=Cake.FileHelpers"

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
var buildDir = srcDir + Directory("bin") + Directory(configuration);
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
    CleanDirectory(buildDir);
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

Task("Pack")
    .IsDependentOn("Build")
    .Does(() =>
{
    var releaseNotes = FileReadLines(File("WHATSNEW.txt"));

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
        LicenseUrl               = new Uri("https://github.com/ZEISS-PiWeb/PiWeb-Api/blob/master/LICENSE.md"),
        Copyright                = string.Format("Copyright © {0} Carl Zeiss Innovationszentrum für Messtechnik GmbH",DateTime.Now.Year),
        ReleaseNotes             = releaseNotes,
        Tags                     = new [] {"ZEISS", "PiWeb", "API"},
        RequireLicenseAcceptance = true,
        Files                    = new [] { 
            new NuSpecContent { Source = "net48/PiWeb.Api.dll", Target = "lib/net48" },
            new NuSpecContent { Source = "net48/PiWeb.Api.xml", Target = "lib/net48" },
            new NuSpecContent { Source = "netstandard2.0/PiWeb.Api.dll", Target = "lib/netstandard2.0" },
            new NuSpecContent { Source = "netstandard2.0/PiWeb.Api.xml", Target = "lib/netstandard2.0" },
            new NuSpecContent { Source = "netcoreapp3.0/PiWeb.Api.dll", Target = "lib/netcoreapp3.0" },
            new NuSpecContent { Source = "netcoreapp3.0/PiWeb.Api.xml", Target = "lib/netcoreapp3.0" },
        },
        Dependencies             = new [] {
            new NuSpecDependency { Id = "Newtonsoft.Json", Version = "12.0.1" },
            new NuSpecDependency { Id = "Newtonsoft.Json.Bson", Version = "1.0.2" },
            new NuSpecDependency { Id = "IdentityModel", Version = "2.0.0" },
            new NuSpecDependency { Id = "Microsoft.IdentityModel.Logging", Version = "5.2.1" },
            new NuSpecDependency { Id = "Microsoft.IdentityModel.Tokens", Version = "5.2.1" },
            new NuSpecDependency { Id = "System.IdentityModel.Tokens.Jwt", Version = "5.2.1" },
            new NuSpecDependency { Id = "JetBrains.Annotations", Version = "2018.2.1" },
        },
        BasePath                 = buildDir,
        OutputDirectory          = artifactsDir
    };
    NuGetPack(nuGetPackSettings);
});

//////////////////////////////////////////////////////////////////////
// TASK TARGETS
//////////////////////////////////////////////////////////////////////

Task("Default")
    .IsDependentOn("AppVeyorSetup")
    .IsDependentOn("Pack");

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);
