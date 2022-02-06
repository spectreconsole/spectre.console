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
        foreach (var completion in GetCompletions(_model, settings))
        {
            _writer.WriteLine(completion, Style.Plain);
        }

        return 0;
    }

    public static string[] GetCompletions(CommandModel model, Settings settings)
    {
        var completions = new string[] { };

        if (string.IsNullOrEmpty(settings.CommandToComplete))
        {
            return completions;
        }

        // We should actually follow the branches of the command and handle parameters, but for a first very naive implementation this is ok.
        string lastSegment = settings.CommandToComplete.Split(" ").LastOrDefault();

        completions = model.Commands
            .TakeWhile(c => c.Name.StartsWith(lastSegment))
                .Select(c => c.Name)
                .ToArray();

        return completions;
    }
}