Title: Exceptions
Order: 12
Description: "Handling exceptions in *Spectre.Console.Cli*"
---

Exceptions happen.

`Spectre.Console.Cli` handles exceptions, writes a user friendly message to the console and sets the exitCode
of the application to `-1`.
While this might be enough for the needs of most applications, there are some options to customize this behavior.

## Propagating exceptions

The most basic way is to set `PropagateExceptions()` during configuration and handle everything.
While this option grants the most freedom, it also requires the most work: Setting `PropagateExceptions`
means that `Spectre.Console.Cli` effectively re-throws exceptions.
This means that `app.Run()` should be wrapped in a `try`-`catch`-block which has to handle the exception
(i.e. outputting something useful) and also provide the exitCode (or `return` value) for the application.

```csharp
using Spectre.Console.Cli;

namespace MyApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp<FileSizeCommand>();

            app.Configure(config =>
            {
                config.PropagateExceptions();
            });

            try
            {
                return app.Run(args);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                return -99;
            }
        }
    }
}
```

## Using a custom ExceptionHandler

Using the `SetExceptionHandler()` during configuration it is possible to handle exceptions in a defined way.
This method comes in two flavours: One that uses the default exitCode (or `return` value) of `-1` and one
where the exitCode needs to be supplied.

The `ITypeResolver?` parameter will be null, when the exception occurs while no `ITypeResolver` is available.
(Basically the `ITypeResolver` will be set, when the exception occurs during a command execution, but not
during the parsing phase and construction of the command.)

### Using `SetExceptionHandler(Func<Exception, ITypeResolver?, int> handler)`

Using this method exceptions can be handled in a custom way. The return value of the handler is used as
the exitCode for the application.

```csharp
using Spectre.Console.Cli;

namespace MyApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp<FileSizeCommand>();

            app.Configure(config =>
            {
                config.SetExceptionHandler((ex, resolver) =>
                {
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                    return -99;
                });
            });

            return app.Run(args);
        }
    }
}
```

### Using `SetExceptionHandler(Action<Exception, ITypeResolver?> handler)`

Using this method exceptions can be handled in a custom way, much the same as with the `SetExceptionHandler(Func<Exception, ITypeResolver?, int> handler)`.
Using the `Action` as the handler however, it is not possible (or required) to supply a return value.
The exitCode for the application will be `-1`.

```csharp
using Spectre.Console.Cli;

namespace MyApp
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp<FileSizeCommand>();

            app.Configure(config =>
            {
                config.SetExceptionHandler((ex, resolver) =>
                {
                    AnsiConsole.WriteException(ex, ExceptionFormats.ShortenEverything);
                });
            });

            return app.Run(args);
        }
    }
}
```