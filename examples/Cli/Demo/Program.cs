using Demo.Commands;
using Spectre.Console.Cli;

namespace Demo
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.SetApplicationName("fake-dotnet");
                config.ValidateExamples();
                config.AddExample(new[] { "run", "--no-build" });

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
                    .WithExample(new[] { "serve", "-o", "firefox" })
                    .WithExample(new[] { "serve", "--port", "80", "-o", "firefox" });
            });

            return app.Run(args);
        }
    }
}
