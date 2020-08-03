using Generator.Commands;
using Spectre.Cli;

namespace Generator
{
    public static class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandApp();
            app.Configure(config =>
            {
                config.AddCommand<ColorGeneratorCommand>("colors");
            });

            return app.Run(args);
        }
    }
}
