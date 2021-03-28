#tool nuget:?package=coveralls.io&version=1.4.2

#addin nuget:?package=Cake.Coveralls&version=1.0.0

var target = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var packageOutputDirectory = Argument("packageOutputDirectory", "dist");

readonly FilePath solutionFile = "src/Cake.Wget.sln";
readonly FilePath projectFile = "src/Cake.Wget/Cake.Wget.csproj";
readonly FilePath testProjectFile = "src/Cake.Wget.Tests/Cake.Wget.Tests.csproj";
readonly FilePath testReportDirectory = "TestsOutput";
readonly FilePath testReport = $"{testReportDirectory}/report.trx";
readonly FilePath coverageReportDirectory = "CoverageResults";
readonly FilePath coverageReport = $"{coverageReportDirectory}/coverage.xml";

Task("Clean")
.Does(() =>
{
    CleanDirectory(packageOutputDirectory);
    CleanDirectory(testReportDirectory.FullPath);
    CleanDirectory(coverageReportDirectory.FullPath);
    CleanDirectories("**/bin");
    CleanDirectories("**/obj");
});

Task("Restore-Packages")
.Does(() =>
{
    DotNetCoreRestore(solutionFile.FullPath);
});

Task("Build")
.IsDependentOn("Clean")
.IsDependentOn("Restore-Packages")
.Does(() =>
{
    var settings = new DotNetCoreBuildSettings
    {
        Configuration = configuration,
        Framework = "netstandard2.0",
    };

    DotNetCoreBuild(projectFile.FullPath, settings);
});

Task("Test")
.IsDependentOn("Clean")
.IsDependentOn("Restore-Packages")
.Does(() =>
{
    var settings = new DotNetCoreTestSettings
    {
        Configuration = configuration,
        Framework = "netcoreapp2.1",
        Loggers = new List<string>() {"trx"},
        VSTestReportPath = testReport.FullPath,
    };

    settings.ArgumentCustomization = 
        args => args.Append("/p:CollectCoverage=true")
        .Append($"/p:CoverletOutput=../../{coverageReport}")
        .Append("/p:CoverletOutputFormat=opencover")
        .Append("/p:Exclude=[xunit.*]*");

    DotNetCoreTest(testProjectFile.FullPath, settings);
});

Task("Package")
.IsDependentOn("Test")
.Does(() =>
{
    var settings = new DotNetCorePackSettings
    {
        OutputDirectory = packageOutputDirectory,
        Configuration = configuration,
    };
    
    DotNetCorePack(projectFile.FullPath, settings);
});

Task("Coverage-Report")
    .IsDependentOn("Test")
    .WithCriteria(BuildSystem.IsRunningOnAppVeyor)
    .WithCriteria(() => FileExists(coverageReport.FullPath))
    .Does(() =>
{
    var settings = new CoverallsIoSettings
    {
        RepoToken = EnvironmentVariable("CoverallsRepoToken"),
    };

    CoverallsIo(coverageReport, settings);
});

Task("Default")
.IsDependentOn("Test");

RunTarget(target);
