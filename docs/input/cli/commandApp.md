Title: CommandApp
Order: 2
Description: "**CommandApp** is the entry point for a *Spectre.Console.Cli* command line application. It is used to configure the settings and commands used for execution of the application."
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

In the previous example we have a single command that is configured. For complex command line applications, it is common for them to have multiple commands (or verbs) defined. Examples of applications like this are `git`, `dotnet` and `gh`. For example, git would have a `commit` command and along with other commands like `add` or `rebase`. Each with their own settings and validation. With `Spectre.Console.Cli` we use the `Configure` method to add these commands.

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
var registrar = new MyTypeRegistrar(registrations);

// Create a new command app with the registrar
// and run it with the provided arguments.
var app = new CommandApp<DefaultCommand>(registrar);
return app.Run(args);
```

<?# Alert ?>
  `MyTypeRegistrar` is a custom class that implements `ITypeRegistrar` and must be provided by the user.
<?#/ Alert ?>

There is a working [example of dependency injection](https://github.com/spectreconsole/examples/tree/main/examples/Cli/Injection) that uses `Microsoft.Extensions.DependencyInjection` as the container. Example implementations of `ITypeRegistrar` and `ITypeResolver` are provided, which you can copy and paste to your application for dependency injection.

Unit testing your `ITypeRegistrar` and `ITypeResolver` implementations is done using the utility `TypeRegistrarBaseTests` included in `Spectre.Console.Testing`. Simply call `TypeRegistrarBaseTests.RunAllTests()` and expect no `TypeRegistrarBaseTests.TestFailedException` to be thrown.

## Interception
Interceptors can be registered with the `TypeRegistrar` (or with a custom DI-Container). Alternatively, `CommandApp` also provides a `SetInterceptor` configuration.

All interceptors must implement `ICommandInterceptor`. Upon execution of a command, The `Intercept`-Method of an instance of your interceptor will be called with the parsed settings. This provides an opportunity for configuring any infrastructure or modifying the settings.
When the command has been run, the `InterceptResult`-Method of the same instance is called with the result of the command.
This provides an opportunity to modify the result and also to tear down any infrastructure in use.

The `Intercept`-Method of each interceptor is run before the command is executed and the `InterceptResult`-Method is run after it. These are typically used for configuring logging or other infrastructure concerns.

For an example of using the interceptor to configure logging, see the [Serilog demo](https://github.com/spectreconsole/examples/tree/main/examples/Cli/Logging)
