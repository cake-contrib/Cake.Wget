# Cake.Wget

Cake.Wget is a cross-platform add-in for [Cake](http://cakebuild.net/) which encapsulates downloading files via [Wget](https://www.gnu.org/software/wget/) tool. Cake.Wget targets the [.NET Standard 2.0](https://docs.microsoft.com/en-us/dotnet/standard/net-standard) which means that will run on Windows, Linux and macOS.

[![Nuget](https://img.shields.io/nuget/v/Cake.Wget.svg)](https://www.nuget.org/packages/Cake.Wget)

## Continuous integration

Build server | Platform | Build status
--- | --- | ---
Travis CI | Ubuntu | [![Build Status](https://travis-ci.org/cake-contrib/Cake.Wget.svg?branch=master)](https://travis-ci.org/cake-contrib/Cake.Wget)
AppVeyor | Windows | [![AppVeyor branch](https://img.shields.io/appveyor/ci/cakecontrib/cake-wget/master.svg)](https://ci.appveyor.com/project/cakecontrib/cake-wget)
Azure Pipelines | Windows | [![Build Status](https://dev.azure.com/cake-contrib/Cake.Wget/_apis/build/status/cake-contrib.Cake.Wget%20-%20Windows?branchName=master)](https://dev.azure.com/cake-contrib/Cake.Wget/_build/latest?definitionId=20&branchName=master)
Azure Pipelines | Ubuntu | [![Build Status](https://dev.azure.com/cake-contrib/Cake.Wget/_apis/build/status/cake-contrib.Cake.Wget%20-%20Ubuntu?branchName=master)](https://dev.azure.com/cake-contrib/Cake.Wget/_build/latest?definitionId=21&branchName=master)
Azure Pipelines | macOS | [![Build Status](https://dev.azure.com/cake-contrib/Cake.Wget/_apis/build/status/cake-contrib.Cake.Wget%20-%20macOS?branchName=master)](https://dev.azure.com/cake-contrib/Cake.Wget/_build/latest?definitionId=22&branchName=master)

Test Coverage |
--- |
[![Coverage Status](https://coveralls.io/repos/github/cake-contrib/Cake.Wget/badge.svg?branch=master)](https://coveralls.io/github/cake-contrib/Cake.Wget?branch=master) |

## Prerequisites

You will need to have a copy of the [Wget executable for your OS](https://www.gnu.org/software/wget/faq.html#download). Put location of Wget executable in your `PATH` environment variable and [Cake will find it](http://cakebuild.net/docs/tools/tool-resolution).

## Usage

Cake.Wget is wrapper for command line [Wget](https://www.gnu.org/software/wget/) tool. To use `Cake.Wget`, you must import addin in your cake script:

```csharp
#addin nuget:?package=Cake.Wget
```

You can also specify version which should be used:

```csharp
#addin nuget:?package=Cake.Wget&version=1.0.0
```

Command line switches are passed to the tool by `WgetSettings` object properties. Most `WgetSettings` properties names can be derived from Wget command line switches (long version) and vice versa. For example command line switch `--input-file` is translated to `WgetSettings` property name `InputFile`.

If command line switch does not have corresponding property, it can be passed to [Wget](https://www.gnu.org/software/wget/) tool by `ArgumentCustomization`:

```csharp
var settings = new WgetSettings {
    ArgumentCustomization = args=>args.Append("--debug")
       .Append("--dns-timeout=10")
};
```

Detailed documentation of all `Wget` command line switches can be found in official [Wget manual pages](https://www.gnu.org/software/wget/manual/wget.html).

### Download a single file

```csharp
var settings = new WgetSettings {
    Url = new Uri("https://wordpress.org/latest.zip")
};

Wget(settings);
```

### Download a file and save it under a different name

```csharp
var settings = new WgetSettings {
    Url = new Uri("https://wordpress.org/latest.zip"),
    OutputDocument = "wordpress.zip"
};

Wget(settings);
```

### Download a file and save it in a specific directory

```csharp
var settings = new WgetSettings {
    Url = new Uri("https://wordpress.org/latest.zip"),
    DirectoryPrefix = "/opt/wordpress"
};

Wget(settings);
```

### Set the download speed to 300KB/s

```csharp
var settings = new WgetSettings {
    Url = new Uri("https://wordpress.org/latest.zip"),
    LimitRate = new WgetLimitRateArgument(
        300,
        LimitRateUnitEnum.Kilobytes)
};

Wget(settings);
```

Note: If you do not specify limit rate units (it is optional parameter), then the number means _bytes per second_.

### Continue interrupted download

```csharp
var settings = new WgetSettings {
    Url = new Uri("https://wordpress.org/latest.zip"),
    Continue = true
};

Wget(settings);
```

### Download multiple files

If you want to download multiple files with one command, create a text file (for example `download.txt`) and write the URLs each on separate line.

```csharp
var settings = new WgetSettings {
    InputFile = "download.txt"
};

Wget(settings);
```

## License

[MIT License](https://github.com/cake-contrib/Cake.Wget/blob/master/LICENSE) &copy; deqenq

## Addendum

There is another interesting [Cake](http://cakebuild.net/) add-in [Cake.Curl](https://github.com/cake-contrib/Cake.Curl). [Curl](https://curl.haxx.se) is command line tool and library for transferring data with URLs.

General comparison between [curl](https://curl.haxx.se) and [Wget](https://www.gnu.org/software/wget/) is [described here](https://daniel.haxx.se/docs/curl-vs-wget.html).
