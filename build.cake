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
readonly string framework = "net9.0";

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
    DotNetRestore(solutionFile.FullPath);
});

Task("Build")
.IsDependentOn("Clean")
.IsDependentOn("Restore-Packages")
.Does(() =>
{
    var settings = new DotNetBuildSettings
    {
        Configuration = configuration,
        Framework = framework,
    };

    DotNetBuild(projectFile.FullPath, settings);
});

Task("Test")
.IsDependentOn("Clean")
.IsDependentOn("Restore-Packages")
.Does(() =>
{
    var settings = new DotNetTestSettings
    {
        Configuration = configuration,
        Framework = framework,
        Loggers = new List<string>() {"trx"},
        VSTestReportPath = testReport.FullPath,
    };

    settings.ArgumentCustomization =
        args => args.Append("/p:CollectCoverage=true")
        .Append($"/p:CoverletOutput=../../{coverageReport}")
        .Append("/p:CoverletOutputFormat=opencover")
        .Append("/p:Exclude=[xunit.*]*");

    DotNetTest(testProjectFile.FullPath, settings);
});

Task("Package")
.IsDependentOn("Test")
.Does(() =>
{
    var settings = new DotNetPackSettings
    {
        OutputDirectory = packageOutputDirectory,
        Configuration = configuration,
    };

    DotNetPack(projectFile.FullPath, settings);
});

Task("Default")
.IsDependentOn("Test");

RunTarget(target);
