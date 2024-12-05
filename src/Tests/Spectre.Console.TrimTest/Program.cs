using Spectre.Console.Cli;
using Spectre.Console.TrimTest;
using Spectre.Console.TrimTest.Commands.Add;
using Spectre.Console.TrimTest.Commands.Run;
using Spectre.Console.TrimTest.Commands.Serve;

var app = new CommandApp();
app.Configure(config =>
{
    config.PropagateExceptions();
    config.SetApplicationName("fake-dotnet");
    config.ValidateExamples();
    config.SetInterceptor(new MyInterceptor());
    config.AddExample("run", "--no-build");

    // Run
    config.AddCommand<RunCommand>("run");

    // Add
    config.AddBranch<AddSettings>("add", add =>
    {
        add.SetDescription("Add a package or reference to a .NET project");
        add.AddCommand<AddPackageCommand>("package");
        add.AddCommand<AddReferenceCommand>("reference");
    });

    // Serve
    config.AddCommand<ServeCommand>("serve")
        .WithExample("serve", "-o", "firefox")
        .WithExample("serve", "--port", "80", "-o", "firefox");
});

return app.Run(args);