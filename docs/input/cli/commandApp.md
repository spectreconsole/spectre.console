Title: CommandApp
Order: 2
Description: "**CommandApp** is the entry point for a *Spectre.Console.Cli* command line application. It is used to configure the settings and commands used for execution of the application."
Reference:
    - T:Spectre.Console.Cli.CommandApp
    - T:Spectre.Console.Cli.CommandApp`1
---

`CommandApp` is the entry point for a `Spectre.Console.Cli` command line application. It is used to configure the settings and commands used for execution of the application. Most `Spectre.Console.Cli` applications will need to specify a custom configuration using the `Configure` method.

For example, the following configuration might be used to change the default behavior indicate that for `DEBUG` configuration's full exception stack traces should be outputted to the screen, and any examples defined for our commands will also be validated.

```csharp
var app = new CommandApp<FileSizeCommand>();
app.Configure(config =>
{
#if DEBUG
    config.PropagateExceptions();
    config.ValidateExamples();
#endif
});
```

## Multiple Commands

In the previous example we have a single command that is configured. For complex command line applications, it is common for them to have multiple commands (or verbs) defined. Examples of applications like this are `git`, `dotnet` and `gh`. For example, git would have a `commit` command and along with other commits like `add` or `rebase`. Each with their own settings and validation. With `Spectre.Console.Cli` we use the `Configure` method to add these commands.

For example, to add three different commands to the application:

```csharp
var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<AddCommand>("add");
    config.AddCommand<CommitCommand>("commit");
    config.AddCommand<RebaseCommand>("rebase");
});
```

This configuration would allow users to run `app.exe add`, `app.exe commit`, or `app.exe rebase` and have the settings routed to the appropriate command.

For more complex command hierarchical configurations, they can also be composed via inheritance and branching. See [Composing Commands](./composing).

## Customizing Command Configurations

The `Configure` method is also used to change how help for the commands is generated. This configuration will give our command an additional alias of `file-size` and a description to be used when displaying the help. Additionally, an example is specified that will be parsed and displayed for users asking for help. Multiple examples can be provided. Commands can also be marked as hidden. With this option they are still executable, but will not be displayed in help screens.

``` csharp
var app = new CommandApp();
app.Configure(config =>
{
    config.AddCommand<FileSizeCommand>("size")
        .IsHidden()
        .WithAlias("file-size")
        .WithDescription("Gets the file size for a directory.")
        .WithExample(new[] {"size", "c:\\windows", "--pattern", "*.dll"});
});
```

## Dependency Injection

`CommandApp` takes care of instantiating commands upon execution. If given a custom type registrar, it will use that to resolve services defined in the command constructor.

```csharp
var registrations = new ServiceCollection();
registrations.AddSingleton<IGreeter, HelloWorldGreeter>();

// Create a type registrar and register any dependencies.
// A type registrar is an adapter for a DI framework.
var registrar = new TypeRegistrar(registrations);

// Create a new command app with the registrar
// and run it with the provided arguments.
var app = new CommandApp<DefaultCommand>(registrar);
return app.Run(args);
```

`TypeRegistrar` is a custom class that must be created by the user. This [example using `Microsoft.Extensions.DependencyInjection` as the container](https://github.com/spectreconsole/spectre.console/tree/main/examples/Cli/Injection) provides an example `TypeRegistrar` and `TypeResolver` that can be added to your application with small adjustments for your DI container.

Hint: If you do write your own implementation of `TypeRegistrar` and `TypeResolver` and you have some form of unit tests in place for your project,
there is a utility `TypeRegistrarBaseTests` available that can be used to ensure your implementations adhere to the required implementation. Simply call `TypeRegistrarBaseTests.RunAllTests()` and expect no `TypeRegistrarBaseTests.TestFailedException` to be thrown.

## Interception

`CommandApp` also provides a `SetInterceptor` configuration. An interceptor is run before all commands are executed. This is typically used for configuring logging or other infrastructure concerns.

All interceptors must implement `ICommandInterceptor`. Upon execution of a command, an instance of your interceptor will be called with the parsed settings. This provides an opportunity for configuring any infrastructure or modifying the settings.

For an example of using the interceptor to configure logging, see the [Serilog demo](https://github.com/spectreconsole/spectre.console/tree/main/examples/Cli/Logging).
