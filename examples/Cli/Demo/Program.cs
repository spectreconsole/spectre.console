using System;
using System.Diagnostics.CodeAnalysis;
using Demo.Commands;
using Demo.Commands.Add;
using Demo.Commands.Run;
using Demo.Commands.Serve;
using Spectre.Console.Cli;

namespace Demo;


public static class Program
{
    public static int Main(string[] args)
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
}
