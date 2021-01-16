using Spectre.Console.Cli;

namespace Spectre.Console.Tests
{
    public static class Constants
    {
        public static string[] VersionCommand { get; } =
            new[]
            {
                CliConstants.Commands.Branch,
                CliConstants.Commands.Version,
            };

        public static string[] XmlDocCommand { get; } =
            new[]
            {
                CliConstants.Commands.Branch,
                CliConstants.Commands.XmlDoc,
            };
    }
}
