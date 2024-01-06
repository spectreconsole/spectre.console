Title: Command Help
Order: 13
Description: "Console applications built with *Spectre.Console.Cli* include automatically generated help command line help."
---

Console applications built with `Spectre.Console.Cli` include automatically generated help which is displayed when `-h` or `--help` has been specified on the command line.

The automatically generated help is derived from the configured commands and their command settings.

The help is also context aware and tailored depending on what has been specified on the command line before it. For example,

1. When `-h` or `--help` appears immediately after the application name (eg. `application.exe --help`), then the help displayed is a high-level summary of the application, including any command line examples and a listing of all possible commands the user can execute. 

2. When `-h` or `--help` appears immediately after a command has been specified (eg. `application.exe command --help`), then the help displayed is specific to the command and includes information about command specific switches and any default values. 

`HelpProvider` is the `Spectre.Console` class responsible for determining context and preparing the help text to write to the console. It is an implementation of the public interface `IHelpProvider`.

## Styling the help text

Basic styling is applied to the generated help text by default, however this is configurable.

There are three different themes shipped with Spectre.Console, namely `Default`, `BoldHeadings`, `None`. 

You can explicitly set the theme when configuring a CommandApp, for example:

```csharp
config.Settings.HelpProviderStyles = HelpProviderStyle.BoldHeadings;
```

Implied by the name, `HelpProviderStyle.None` doesn't include any styles and is a good choice for accessibility.

A custom theme can also be defined and applied, in a similar way how the HelpProviderStyle factories do it.

```csharp
HelpProviderStyle styles = default(HelpProviderStyle);

styles.Description.Header.Markup = "yellow";
styles.Usage.Header.Markup = "yellow";
styles.Usage.CurrentCommand.Markup = "underline";
styles.Usage.Command.Markup = "aqua";

config.Settings.HelpProviderStyles = styles;

```

See [Markup](../markup) for information about the use of markup in Spectre.Console, and [Styles](xref:styles) for a listing of supported styles.

## Custom help providers

Whilst it shouldn't be common place to implement your own help provider, it is however possible. 

You are able to implement your own `IHelpProvider` and configure a `CommandApp` to use that instead of the Spectre.Console help provider. 

```csharp
using Spectre.Console.Cli;

namespace Help;

public static class Program
{
    public static int Main(string[] args)
    {
        var app = new CommandApp<DefaultCommand>();

        app.Configure(config =>
        {
            // Register the custom help provider
            config.SetHelpProvider(new CustomHelpProvider(config.Settings));
        });

        return app.Run(args);
    }
}
```

There is a working [example of a custom help provider](https://github.com/spectreconsole/spectre.console/tree/main/examples/Cli/Help) demonstrating this.

