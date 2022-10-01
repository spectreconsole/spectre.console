Title: Quick Start
Order: 20
Description: Getting started with *Spectre.Console*
---

The fastest way of getting started using Spectre.Console is
to install the NuGet package.

```text
> dotnet add package Spectre.Console
```

After that you will need to reference the `Spectre.Console` namespace.
Once that is done, you can start using all the available features.

```csharp
using Spectre.Console;

public static class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.Markup("[underline red]Hello[/] World!");
    }
}
```
