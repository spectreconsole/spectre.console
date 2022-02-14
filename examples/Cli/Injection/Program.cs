using Injection.Commands;
using Injection.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;

namespace Injection;

public class Program
{
    public static int Main(string[] args)
    {
        // Create a type registrar and register any dependencies.
        // A type registrar is an adapter for a DI framework.
        var registrations = new ServiceCollection();
        registrations.AddSingleton<IGreeter, HelloWorldGreeter>();
        var registrar = new TypeRegistrar(registrations);

        // Create a new command app with the registrar
        // and run it with the provided arguments.
        var app = new CommandApp<DefaultCommand>(registrar);
        return app.Run(args);
    }
}
