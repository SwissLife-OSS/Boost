# Boost

![BoostLogo](/images/logo_boost.png)

Boost is a [dotnet tool](https://docs.microsoft.com/en-us/dotnet/core/tools/global-tools) to boost your developer experience. 

> Project state: in Pre-Release

[![nuget](https://img.shields.io/nuget/v/Boost.Tool?color=%2300b339)](https://www.nuget.org/packages/Boost.Tool)
[![Downloads](https://img.shields.io/nuget/dt/Boost.Tool?color=%230075c2)](https://www.nuget.org/stats/packages/Boost.Tool?groupby=Version)
[![Build Status](https://dev.azure.com/swisslife-oss/swisslife-oss/_apis/build/status/Release%20-%20Boost?branchName=refs%2Ftags%2F0.2.0-preview.4)](https://dev.azure.com/swisslife-oss/swisslife-oss/_build/latest?definitionId=39&branchName=refs%2Ftags%2F0.2.0)

## Installation

```bash
dotnet tool install --global Boost.Tool
```

> To install to latest pre-release use --version {version} argument.

```bash
dotnet tool install --global Boost.Tool --version 0.3.0-preview.1
```

## Getting started

Use the integrated help to see what boost can do

```
boo --help
```

Or just start the UI 

```
boo ui
```

### Update global tool

```bash
dotnet tool update -g  Boost.Tool
```
### Uninstall global tool

```bash
dotnet tool uninstall -g Boost.Tool
```


## Community

This project has adopted the code of conduct defined by the [Contributor Covenant](https://contributor-covenant.org/)
to clarify expected behavior in our community. For more information, see the [Swiss Life OSS Code of Conduct](https://swisslife-oss.github.io/coc).
