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
                config.AddCommand<EmojiGeneratorCommand>("emoji");
                config.AddCommand<SpinnerGeneratorCommand>("spinners");
            });

            return app.Run(args);
        }
    }
}
