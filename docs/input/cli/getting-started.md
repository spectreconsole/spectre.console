Title: Getting Started
Order: 1
Description: "How to get started using *Spectre.Console.Cli* to write a modern console application that follows industry conventions for command line parsing."
---

`Spectre.Console.Cli` is a modern library for parsing command line arguments. While it's extremely
opinionated in what it does, it tries to follow established industry conventions, and draws
its inspiration from applications you use everyday.

## How does it work?

A `Spectre.Console.Cli` app will be comprised of Commands and a matching Setting specification. The settings file will be the model for the command parameters. Upon execution, `Spectre.Console.Cli` will parse the `args[]` passed into your application and match them to the appropriate settings file giving you a strongly typed object to work with.

The following example demonstrates these concepts coming together.

```csharp
var app = new CommandApp<FileSizeCommand>();
return app.Run(args);

internal sealed class FileSizeCommand : Command<FileSizeCommand.Settings>
{
    public sealed class Settings : CommandSettings
    {
        [Description("Path to search. Defaults to current directory.")]
        [CommandArgument(0, "[searchPath]")]
        public string? SearchPath { get; init; }

        [CommandOption("-p|--pattern")]
        public string? SearchPattern { get; init; }

        [CommandOption("--hidden")]
        [DefaultValue(true)]
        public bool IncludeHidden { get; init; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        var searchOptions = new EnumerationOptions
        {
            AttributesToSkip = settings.IncludeHidden
                ? FileAttributes.Hidden | FileAttributes.System
                : FileAttributes.System
        };

        var searchPattern = settings.SearchPattern ?? "*.*";
        var searchPath = settings.SearchPath ?? Directory.GetCurrentDirectory();
        var files = new DirectoryInfo(searchPath)
            .GetFiles(searchPattern, searchOptions);

        var totalFileSize = files
            .Sum(fileInfo => fileInfo.Length);

        AnsiConsole.MarkupLine($"Total file size for [green]{searchPattern}[/] files in [green]{searchPath}[/]: [blue]{totalFileSize:N0}[/] bytes");

        return 0;
    }
}
```

In our `Main()` method, an instance of `Spectre.Console.Cli`'s  `CommandApp` is instantiated specifying `FileSizeCommand` will be the app's default and only command. `FileSizeCommand` is defined as inheriting from `Spectre.Console.Cli`'s generic `Command` class specifying the settings for the command are `FileSizeCommand.Settings`. The settings are defined  using a nested class, but they can exist anywhere in the project as long as they inherit from `CommandSettings`.

This command will have three parameters.

* The main parameter for the command will be the path. This is to be passed in as the first argument without needing to specify any command line flags. To configure that setting, use the `CommandArgument` attribute. The `[searchPath]` parameter of `CommandArgument` drives not only how the built in help display will render the help text, but the square brackets tells `Spectre.Console.Cli` that this argument is optional through convention.
* The second will be specified as a parameter option. The `CommandOption` attribute is used to specify this action along with the option command line flag. In the case of `SearchPattern` both `-p` and `--pattern` are valid.
* The third will also be a parameter option. Here `DefaultValue` is used to indicate the default value will be `true`. For boolean parameters these will be interpreted as flags which means the user can just specify `--hidden` rather than `-hidden true`.

When `args` is passed into the `CommandApp`'s run method, `Spectre.Console.Cli` will parse those arguments and populate an instance of your settings. Upon success, it will then pass those settings into an instance of the specified command's `Execute` method.

With this in place, the following commands will all work

```text
app.exe
app.exe c:\windows
app.exe c:\windows --pattern *.dll
app.exe c:\windows --hidden --pattern *.dll
```

Much more is possible. You can have multiple commands per application, settings can be customized and extended and the default behavior of the `CommandApp` can be extended and customized.

* See [CommandApp](./commandapp) for customizing how Spectre.Console.Cli builds the settings.
* See [Create Commands](./commands) for information about different command types and their configurations.
* See [Specifying Settings](./settings) for information about defining the settings.
