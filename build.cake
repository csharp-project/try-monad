#tool "nuget:?package=Fixie"
#addin "nuget:?package=Cake.Watch"

var target = Argument("target", "Default");
var npi = EnvironmentVariable("npi");

Task("push")
    .IsDependentOn("pack")
    .Description("Push nuget")
    .Does(() => {
        var nupkg = new DirectoryInfo("./Nuget").GetFiles("*.nupkg").LastOrDefault();
        var package = nupkg.FullName;
        NuGetPush(package, new NuGetPushSettings {
            Source = "https://www.nuget.org/api/v2/package",
            ApiKey = npi
        });
    });

Task("pack")
    .IsDependentOn("build")
    .Does(() => {
        CleanDirectory("Nuget");
        var settings = new ProcessSettings {
            Arguments = "pack TryMonad/TryMonad.csproj -OutputDirectory Nuget"
        };
        StartProcess("nuget", settings);
    });

Task("build")
    .Does(() => {
        DotNetBuild("TryMonad.sln", settings => {
            settings.SetConfiguration("Release")
            .WithTarget("Rebuild");
        });
    });

Task("test")
    .Does(() => {
            DotNetBuild("TryMonad.sln");
            Fixie("TryMonad.Tests/bin/Debug/TryMonad.Tests.dll");
    });

Task("watch")
    .Does(() => {
        var settings = new WatchSettings {
            Recursive = true,
            Path = "./",
            Pattern = "*Tests.cs"
        };
        Watch(settings, (changed) => {
            RunTarget("test");
        });
    });

RunTarget(target);