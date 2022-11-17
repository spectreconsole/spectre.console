# `Spectre.Console`

_[![Spectre.Console NuGet Version](https://img.shields.io/nuget/v/spectre.console.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_ _[![Spectre.Console CLI NuGet Version](https://img.shields.io/nuget/v/spectre.console.cli.svg?style=flat&label=NuGet%3A%20Spectre.Console.Cli)](https://www.nuget.org/packages/spectre.console.cli)_ [![Netlify Status](https://api.netlify.com/api/v1/badges/1eaf215a-eb9c-45e4-8c64-c90b62963149/deploy-status)](https://app.netlify.com/sites/spectreconsole/deploys)

A .NET library that makes it easier to create beautiful, cross platform, console applications.  
It is heavily inspired by the excellent [Rich library](https://github.com/willmcgugan/rich) 
for Python. For detailed usage instructions, [please refer to the documentation at https://spectreconsole.net/.](https://spectreconsole.net/)

## Table of Contents

1. [Features](#features)
1. [Installing](#installing)
1. [Documentation](#documentation)
1. [Examples](#examples)
1. [Sponsors](#sponsors)
1. [Code of Conduct](#code-of-conduct)
1. [.NET Foundation](#net-foundation)
1. [License](#license)

## Features

* Written with unit testing in mind.
* Supports tables, grids, panels, and a [rich](https://github.com/willmcgugan/rich) inspired markup language.
* Supports the most common SRG parameters when it comes to text 
  styling such as bold, dim, italic, underline, strikethrough, 
  and blinking text.
* Supports 3/4/8/24-bit colors in the terminal.  
  The library will detect the capabilities of the current terminal 
  and downgrade colors as needed.  


![Example](docs/input/assets/images/example.png)

## Installing

The fastest way of getting started using `Spectre.Console` is to install the NuGet package.

```csharp
dotnet add package Spectre.Console
```

## Documentation

The documentation for `Spectre.Console` can be found at
https://spectreconsole.net/

## Examples

To see `Spectre.Console` in action, install the 
[dotnet-example](https://github.com/patriksvensson/dotnet-example)
global tool.

```
> dotnet tool restore
```

Now you can list available examples in this repository:

```
> dotnet example
```

And to run an example:

```
> dotnet example tables
```

## Sponsors

The following people are [sponsoring](https://github.com/sponsors/patriksvensson)
Spectre.Console to show their support and to ensure the longevity of the project.

* [Rodney Littles II](https://github.com/RLittlesII)
* [Martin Björkström](https://github.com/bjorkstromm)
* [Dave Glick](https://github.com/daveaglick)
* [Kim Gunnarsson](https://github.com/kimgunnarsson)
* [Andrew McClenaghan](https://github.com/andymac4182)
* [C. Augusto Proiete](https://github.com/augustoproiete)
* [Viktor Elofsson](https://github.com/vktr)
* [Steven Knox](https://github.com/stevenknox)
* [David Pendray](https://github.com/dpen2000)
* [Elmah.io](https://github.com/elmahio)

We really appreciate it.  
**Thank you very much!**

## Code of Conduct

This project has adopted the code of conduct defined by the Contributor Covenant to clarify expected behavior in our community.
For more information see the [.NET Foundation Code of Conduct](https://dotnetfoundation.org/code-of-conduct).

## .NET Foundation

This project is supported by the [.NET Foundation](https://dotnetfoundation.org).

## License

Copyright © Patrik Svensson, Phil Scott, Nils Andresen

Spectre.Console is provided as-is under the MIT license. For more information see LICENSE.

* SixLabors.ImageSharp, a library which Spectre.Console relies upon, is licensed under Apache 2.0 when distributed as part of Spectre.Console. The Six Labors Split License covers all other usage, see: https://github.com/SixLabors/ImageSharp/blob/master/LICENSE 
