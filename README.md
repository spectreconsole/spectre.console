# `Spectre.Console`

_[![Spectre.Console NuGet Version](https://img.shields.io/nuget/v/spectre.console.svg?style=flat&label=NuGet%3A%20Spectre.Console)](https://www.nuget.org/packages/spectre.console)_

A .NET 5/.NET Standard 2.0 library that makes it easier to create beautiful, cross platform, console applications.  
It is heavily inspired by the excellent [Rich library](https://github.com/willmcgugan/rich) 
for Python.

## Table of Contents

1. [Features](#features)
2. [Installing](#installing)
3. [Documentation](#documentation)
4. [Examples](#examples)
5. [Sponsors](#sponsors)
5. [License](#license)

## Features

* Written with unit testing in mind.
* Supports tables, grids, panels, and a [rich](https://github.com/willmcgugan/rich) inspired markup language.
* Supports the most common SRG parameters when it comes to text 
  styling such as bold, dim, italic, underline, strikethrough, 
  and blinking text.
* Supports 3/4/8/24-bit colors in the terminal.  
  The library will detect the capabilities of the current terminal 
  and downgrade colors as needed.  


![Example](resources/gfx/screenshots/example.png)

## Installing

The fastest way of getting started using `Spectre.Console` is to install the NuGet package.

```csharp
dotnet add package Spectre.Console
```

## Documentation

The documentation for `Spectre.Console` can be found at
https://spectresystems.github.io/spectre.console/

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

Sponsors of Spectre.Console are:

* Rodney Littles II (@RLittlesII)
* Martin Björkström (@bjorkstromm)
* Dave Glick (@daveaglick)
* Kim Gunanrsson (@kimgunnarsson)
* Andrew McClenaghan (@andymac4182)
* C. Augusto Proiete (@augustoproiete)
* Viktor Elofsson (@vktr)
* Steven Knox (@stevenknox)
* David Pendray (@dpen2000)

Thank you very much!

## License

Copyright © Spectre Systems.

Spectre.Console is provided as-is under the MIT license. For more information see LICENSE.

* For SixLabors.ImageSharp, see https://github.com/SixLabors/ImageSharp/blob/master/LICENSE