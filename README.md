# `Spectre.Console`

_[![Spectre.Console NuGet Version](https://img.shields.io/nuget/v/spectre.console.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_ [![Netlify Status](https://api.netlify.com/api/v1/badges/1eaf215a-eb9c-45e4-8c64-c90b62963149/deploy-status)](https://app.netlify.com/sites/spectreconsole/deploys)

A .NET library that makes it easier to create beautiful, cross platform, console applications.  
It is heavily inspired by the excellent Python library, [Rich](https://github.com/willmcgugan/rich). Detailed instructions for using `Spectre.Console` are located on the project website, https://spectreconsole.net

## Table of Contents

1. [Features](#features)
1. [Installing](#installing)
1. [Documentation](#documentation)
1. [Examples](#examples)
1. [Code of Conduct](#code-of-conduct)
1. [.NET Foundation](#net-foundation)
1. [License](#license)

## Features

* Supports tables, grids, panels, and a [Rich](https://github.com/willmcgugan/rich) inspired markup language.
* Supports the most common SRG parameters when it comes to text 
  styling such as bold, dim, italic, underline, strikethrough, 
  and blinking text.
* Supports 3/4/8/24-bit colors in the terminal.  
  The library will detect the capabilities of the current terminal 
  and downgrade colors as needed.
* Written with unit testing in mind.

![Example](docs/input/assets/images/example.png)

## Important Notices

> [!IMPORTANT]\
> We use the [Top Issues Dashboard](https://github.com/spectreconsole/spectre.console/issues/1517) for tracking community demand. Please upvote :+1: the issues and pull requests you are interested in.

## Installing

The fastest way of getting started using `Spectre.Console` is to install the NuGet package.

```csharp
dotnet add package Spectre.Console
```

## Documentation

The documentation for `Spectre.Console` can be found at
https://spectreconsole.net

## Examples

To see `Spectre.Console` in action, please see the 
[examples repository](https://github.com/spectreconsole/examples).

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

## License

Copyright © Patrik Svensson, Phil Scott, Nils Andresen, Cédric Luthi, Frank Ray

`Spectre.Console` is provided as-is under the MIT license. For more information see LICENSE.

* SixLabors.ImageSharp, a library which `Spectre.Console` relies upon, is licensed under Apache 2.0 when distributed as part of `Spectre.Console`. The Six Labors Split License covers all other usage, see: https://github.com/SixLabors/ImageSharp/blob/master/LICENSE 
