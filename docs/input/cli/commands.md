Title: Creating Commands
Order: 6
Description: "How to create commands for *Spectre.Console.Cli*"
Reference:
    - T:Spectre.Console.Cli.AsyncCommand`1
    - T:Spectre.Console.Cli.Command`1
---

Commands in `Spectre.Console.Cli` are defined by creating a class that inherits from either `Command<TSettings>` or `AsyncCommand<TSettings>`. `Command<TSettings>` must implement an `Execute` method that returns an int where as `AsyncCommand<TSettings>` must implement `ExecuteAsync`  returning `Task<int>`.

```csharp
public class HelloCommand : Command<HelloCommand.Settings>
{
    public class Settings : CommandSettings
    {
        [CommandArgument(0, "[Name]")]
        public string Name { get; set; }
    }


    public override int Execute(CommandContext context, Settings settings)
    {
        AnsiConsole.MarkupLine($"Hello, [blue]{settings.Name}[/]");
        return 0;
    }
}
```

## Configuring

Commands are configured via the [`CommandApp`](commandapp)'s `Configure` method.

```csharp
var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<HelloCommand>("hello")
        .WithAlias("hola")
        .WithDescription("Say hello")
        .WithExample("hello", "Phil")
        .WithExample("hello", "Phil", "--count", "4");
});
```

* `WithAlias` allows commands to have multiple names.
* `WithDescription` is used by the help renderer to give commands a description when displaying help.
* `WithExample` is used by the help renderer to provide examples to the user for running the commands. The parameters is a string array that matches the values passed in `Main(string[] args)`.

## Dependency Injection

Constructor injection is supported on commands. See the [`CommandApp`](commandapp) documentation for further information on configuring `Spectre.Console` for your container.

## Validation

While the settings can validate themselves, the command also provides a validation. For example, `IFileSystem` might be injected into the command which we want to use to validate that a path passed in exists.

```csharp
public override ValidationResult Validate(CommandContext context, Settings settings)
{
    if (_fileSystem.IO.File.Exists(settings.Path))
    {
        return ValidationResult.Error($"Path not found - {settings.Path}");
    }

    return base.Validate(context, settings);
}
```
