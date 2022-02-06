namespace Spectre.Console.Cli;

[Description("Generates a list of completion options for the given command.")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class CompleteCommand : Command<CompleteCommand.Settings>
{
    private readonly CommandModel _model;
    private readonly IAnsiConsole _writer;

    public CompleteCommand(IConfiguration configuration, CommandModel model)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _writer = configuration.Settings.Console.GetConsole();
    }

    public sealed class Settings : CommandSettings
    {
        public Settings(string? commandToComplete)
        {
            CommandToComplete = commandToComplete;
        }

        [CommandArgument(0, "[commandToComplete]")]
        public string? CommandToComplete { get; }
    }

    public override int Execute([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        _writer.Write(GetCompletions(_model, settings), Style.Plain);
        return 0;
    }

    public static string GetCompletions(CommandModel model, Settings settings)
    { 
        throw new NotImplementedException();
    }
}