namespace Spectre.Console.Cli;

[Description("Displays the CLI library version")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class VersionCommand : Command, IBuiltInCommand
{
    private readonly IAnsiConsole _writer;

    public VersionCommand(IConfiguration configuration)
    {
        _writer = configuration.Settings.Console.GetConsole();
    }

    public override int Execute(CommandContext context, CancellationToken cancellationToken)
    {
        _writer.MarkupLine(
            "[yellow]Spectre.Cli[/] version [aqua]{0}[/]",
            VersionHelper.GetVersion(typeof(VersionCommand)?.Assembly));

        _writer.MarkupLine(
            "[yellow]Spectre.Console[/] version [aqua]{0}[/]",
            VersionHelper.GetVersion(typeof(IAnsiConsole)?.Assembly));

        return 0;
    }
}