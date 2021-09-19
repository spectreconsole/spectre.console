Title: Composing Commands
RedirectFrom: introduction
Order: 8
Description: "The underlying philosophy behind *Spectre.Console.Cli* is to rely on the .NET type system to
declare the commands, but tie everything together via composition."
---

The underlying philosophy behind `Spectre.Console.Cli` is to rely on the .NET type system to
declare the commands, but tie everything together via composition.

Imagine the following command structure:

* dotnet *(executable)*
  * add `[PROJECT]`
    * package `<PACKAGE_NAME>` --version `<VERSION>`
    * reference `<PROJECT_REFERENCE>`

For this I would like to implement the commands (the different levels in the tree that
executes something) separately from the settings (the options, flags and arguments),
which I want to be able to inherit from each other.

## Specify the settings

We start by creating some settings that represents the options, flags and arguments
that we want to act upon.

```csharp
public class AddSettings : CommandSettings
{
    [CommandArgument(0, "[PROJECT]")]
    public string Project { get; set; }
}

public class AddPackageSettings : AddSettings
{
    [CommandArgument(0, "<PACKAGE_NAME>")]
    public string PackageName { get; set; }

    [CommandOption("-v|--version <VERSION>")]
    public string Version { get; set; }
}

public class AddReferenceSettings : AddSettings
{
    [CommandArgument(0, "<PROJECT_REFERENCE>")]
    public string ProjectReference { get; set; }
}
```

## Specify the commands

Now it's time to specify the commands that act on the settings we created
in the previous step.

```csharp
public class AddPackageCommand : Command<AddPackageSettings>
{
    public override int Execute(CommandContext context, AddPackageSettings settings)
    {
        // Omitted
        return 0;
    }
}

public class AddReferenceCommand : Command<AddReferenceSettings>
{
    public override int Execute(CommandContext context, AddReferenceSettings settings)
    {
        // Omitted
        return 0;
    }
}
```

You can use `AsyncCommand` if you need async support.

## Let's tie it together

Now when we have our commands and settings implemented, we can compose a command tree
that tells the parser how to interpret user input.

```csharp
using Spectre.Console.Cli;

namespace MyApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();

            app.Configure(config =>
            {
                config.AddBranch<AddSettings>("add", add =>
                {
                    add.AddCommand<AddPackageCommand>("package");
                    add.AddCommand<AddReferenceCommand>("reference");
                });
            });

            return app.Run(args);
        }
    }
}
```

## So why this way?

Now you might wonder, why do things like this? Well, for starters the different parts
of the application are separated, while still having the option to share things like options,
flags and arguments between them.

This makes the resulting code very clean and easy to navigate, not to mention to unit test.
And most importantly at all, the type system guides me to do the right thing. I can't configure 
commands in non-compatible ways, and if I want to add a new top-level `add-package` command 
(or move the command completely), it's just a single line to change. This makes it easy to 
experiment and makes the CLI experience a first class citizen of your application.
