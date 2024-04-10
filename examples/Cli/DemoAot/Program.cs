using System;
using DemoAot.Commands.Add;
using DemoAot.Commands.Run;
using DemoAot.Commands.Serve;
using Spectre.Console;
using Spectre.Console.Cli;

try
{
    var app = new CommandApp();
    app.Configure(config =>
    {
        config.PropagateExceptions();
        config.SetApplicationName("fake-dotnet");
        config.ValidateExamples();
        config.AddExample("run", "--no-build");

        // Run
        config.AddCommand<RunCommand, RunCommand.Settings>("run");

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
