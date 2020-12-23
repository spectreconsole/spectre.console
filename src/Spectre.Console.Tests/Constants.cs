namespace Spectre.Console.Tests
{
    public static class Constants
    {
        public static string[] VersionCommand { get; } =
            new[]
            {
                Spectre.Console.Cli.Internal.Constants.Commands.Branch,
                Spectre.Console.Cli.Internal.Constants.Commands.Version,
            };

        public static string[] XmlDocCommand { get; } =
            new[]
            {
                Spectre.Console.Cli.Internal.Constants.Commands.Branch,
                Spectre.Console.Cli.Internal.Constants.Commands.XmlDoc,
            };
    }
}
