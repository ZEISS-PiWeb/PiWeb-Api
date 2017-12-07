#tool "nuget:?package=GitVersion.CommandLine"
#tool "nuget:?package=GitReleaseNotes"

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

var repoBranchName = "master";

var isDeveloperBuild = BuildSystem.IsLocalBuild;

var gitVersionInfo = GitVersion(new GitVersionSettings {
    OutputType = GitVersionOutput.Json
});

var nugetVersion = isDeveloperBuild ? "0.0.0" : gitVersionInfo.NuGetVersion;

///////////////////////////////////////////////////////////////////////////////
// SETUP / TEARDOWN
///////////////////////////////////////////////////////////////////////////////
Setup(context =>
{
    Information("Building v{0}", nugetVersion);    
});

Teardown(context =>
{
    Information("Finished running tasks.");
});

//////////////////////////////////////////////////////////////////////
// PRIVATE TASKS
//////////////////////////////////////////////////////////////////////

Task("AppVeyorSetup")
    .Does(() =>
{
    if(BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    {
        var appVeyorBuildNumber = EnvironmentVariable("APPVEYOR_BUILD_NUMBER");
        var appVeyorBuildVersion = $"{nugetVersion}+{appVeyorBuildNumber}";
        repoBranchName = EnvironmentVariable("APPVEYOR_REPO_BRANCH");
        Information("AppVeyor branch name is " + repoBranchName);
        Information("AppVeyor build version is " + appVeyorBuildVersion);
        BuildSystem.AppVeyor.UpdateBuildVersion(appVeyorBuildVersion);
    }
    else
    {
        Information("Not running on AppVeyor");
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

Task("UpdateAssemblyInfo")
    .Does(() =>
{
    GitVersion(new GitVersionSettings {
        UpdateAssemblyInfo = true
    });

    Information("AssemblyVersion -> {0}", gitVersionInfo.AssemblySemVer);
    Information("AssemblyFileVersion -> {0}", $"{gitVersionInfo.MajorMinorPatch}.0");
    Information("AssemblyInformationalVersion -> {0}", gitVersionInfo.InformationalVersion);
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

Task("ReleaseNotes")
    .IsDependentOn("AppVeyorSetup")
    .Does(() =>
{
    GitReleaseNotes(artifactsDir + File("ReleaseNotes.md"), new GitReleaseNotesSettings {
        WorkingDirectory         = ".",
        Verbose                  = true,       
        RepoBranch               = repoBranchName,    
        Version                  = nugetVersion,
        AllLabels                = true
    });
});

Task("Pack")
    .IsDependentOn("Build")
    .IsDependentOn("ReleaseNotes")
    .Does(() =>
{
    var nuGetPackSettings = new NuGetPackSettings {
        Id                       = "Zeiss.IMT.PiWebApi.Client",
        Version                  = nugetVersion,
        Title                    = "Carl Zeiss PiWeb-API .NET Client",
        Authors                  = new[] {"Carl Zeiss IZfM Dresden"},
        Owners                   = new[] {"Carl Zeiss IZfM Dresden"},
        Description              = "The Carl Zeiss PiWeb-API .NET Client provides an extensive set of methods for reading and writing  inspection plan structure as well as measurements and measurement values from and to the PiWeb server via HTTP(S)/REST based web service endpoints. PiWeb server &gt;= version 5.8 is required.",
        Summary                  = "A .NET client for HTTP(S)/REST based communication with the quality data managament system ZEISS PiWeb.",
        ProjectUrl               = new Uri("https://github.com/ZEISS-PiWeb/PiWeb-Api"),
        IconUrl                  = new Uri("https://raw.githubusercontent.com/ZEISS-PiWeb/PiWeb-Api/master/logo6464.png"),
        LicenseUrl               = new Uri("https://github.com/ZEISS-PiWeb/PiWeb-Api/blob/master/LICENSE.md"),
        Copyright                = "© Carl Zeiss IZfM Dresden",
        ReleaseNotes             = new [] {"Adjusted summaries of classes and properties."},
        Tags                     = new [] {"Carl", "Zeiss", "PiWeb", "API"},
        RequireLicenseAcceptance = true,
        Files                    = new [] { 
		    new NuSpecContent { Source = "PiWeb.Api.dll", Target = "lib" },
		    new NuSpecContent { Source = "PiWeb.Api.xml", Target = "lib" },
		},
		Dependencies             = new [] { new NuSpecDependency { Id = "Newtonsoft.Json", Version = "7.0.1" } },
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
