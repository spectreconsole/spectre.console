using System;
using DemoAot;
using DemoAot.Commands.Add;
using DemoAot.Commands.Run;
using DemoAot.Commands.Serve;
using DemoAot.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console;
using Spectre.Console.Cli;

try
{
    var services = new ServiceCollection();
    services.AddSingleton<GreetingService>();

// add extra services to the container here
    using var registrar = new DependencyInjectionRegistrar(services);

    var app = new CommandApp(registrar);
    app.Configure(config =>
    {
        config.PropagateExceptions();
        config.SetApplicationName("fake-dotnet");
        config.ValidateExamples();
        config.AddExample("run", "--no-build");

        // Run
        config.AddCommand<RunCommand, RunCommand.Settings>("run");
        config.AddCommand<InfoCommand, InfoCommand.Settings>("info");

        // Add
        config.AddBranch<AddSettings>("add", add =>
        {
            add.SetDescription("Add a package or reference to a .NET project");
            add.AddCommand<AddPackageCommand, AddPackageCommand.Settings>("package");
            add.AddCommand<AddReferenceCommand, AddReferenceCommand.Settings>("reference");
        });

        // Serve
        config.AddCommand<ServeCommand, ServeCommand.Settings>("serve")
            .WithExample("serve", "-o", "firefox")
            .WithExample("serve", "--port", "80", "-o", "firefox");
    });

    app.Run(args);

    return 0;
}
catch (Exception e)
{
    // this will raise a warning because AnsiConsole.WriteException relies on reflection to generate the pretty formatted
    // exception. When executed in a NativeAOT scenario it is the same as calling e.ToString()
    #pragma warning disable IL2026
    AnsiConsole.WriteException(e);
    #pragma warning restore IL2026

    return -1;
}
