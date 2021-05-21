using Generator.Commands;
using Spectre.Console.Cli;

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
                config.AddCommand<EmojiGeneratorCommand>("emoji");
                config.AddCommand<SpinnerGeneratorCommand>("spinners");
                config.AddCommand<SampleCommand>("samples");
            });

            return app.Run(args);
        }
    }
}
