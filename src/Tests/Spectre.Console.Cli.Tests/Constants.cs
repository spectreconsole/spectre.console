namespace Spectre.Console.Tests;

public static class Constants
{
    public static string[] VersionCommand { get; } =
    [
        CliConstants.Commands.Branch,
        CliConstants.Commands.Version
    ];

    public static string[] XmlDocCommand { get; } =
    [
        CliConstants.Commands.Branch,
        CliConstants.Commands.XmlDoc
    ];

    public static string[] OpenCliOption { get; } =
        [CliConstants.DumpHelpOpenCliOption];
}
