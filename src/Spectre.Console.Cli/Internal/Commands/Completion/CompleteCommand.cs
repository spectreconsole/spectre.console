using Spectre.Console.Cli.Completion;
using Spectre.Console.Cli.Internal.Commands.Completion;

namespace Spectre.Console.Cli;

[Description("Generates a list of completion options for the given command.")]
internal sealed class CompleteCommand : AsyncCommand<CompleteCommand.Settings>
{
    private readonly CommandModel _model;
    private readonly ITypeResolver _typeResolver;
    private readonly IAnsiConsole _writer;
    private readonly IConfiguration _configuration;

    public CompleteCommand(
        IConfiguration configuration,
        CommandModel model,
        ITypeResolver typeResolver)
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _typeResolver = typeResolver;
        _writer = configuration.Settings.Console.GetConsole();
        _configuration = configuration;
    }

    public sealed class Settings : CommandSettings
    {
        [CommandArgument(0, "[commandToComplete]")]
        public string? CommandToComplete { get; set; }

        //--position
        [CommandOption("--position|-p")]
        public int? Position { get; set; }
    }

    public override async Task<int> ExecuteAsync([NotNull] CommandContext context, [NotNull] Settings settings)
    {
        HighjackConsoles();

        var commandToComplete = settings.CommandToComplete;
        if (string.IsNullOrEmpty(commandToComplete))
        {
            // No command to complete, so just print the application name.
            _writer.WriteLine(_model.ApplicationName ?? string.Empty, Style.Plain);
            return 0;
        }

        var ctx = new CommandCompletionContextParser(_model, _configuration)
            .Parse(settings.CommandToComplete, settings.Position);

        foreach (var completion in await GetCompletionsAsync(ctx))
        {
            _writer.WriteLine(completion, Style.Plain);
        }

        return 0;
    }

    private async Task<string[]> GetCompletionsAsync(CommandCompletionContext? context)
    {
        if (context?.ShouldReturnEarly ?? true)
        {
            return Array.Empty<string>();
        }

        var commandElements = context.CommandElements;

        var shouldGenerateDefaultSuggestions =
            context.ShouldSuggestMatchingInRoot
            || commandElements?.Length is 0 or null
            || (commandElements.Length == 1 && string.IsNullOrEmpty(commandElements[0]));  // Return early if the only thing we got was "".

        if (shouldGenerateDefaultSuggestions)
        {
            return GetSuggestions(context?.PartialElement, _model.Commands).Suggestions.ToArray();
        }

        if (context.CommandElements.LastOrDefault()?.StartsWith("--") == true)
        {
            var options = GetParameters(context);
            return options.SelectMany(x => x.Suggestions).ToArray();
        }

        var childCommands = GetChildCommands(context.PartialElement, context.Parent);

        childCommands = childCommands.WithGeneratedSuggestions();

        var parameters = GetParameters(context);
        var arguments = await GetCommandArgumentsAsync(context);

        var allResults = parameters.Concat(arguments).Append(childCommands).ToArray();

        if (allResults.Any(x => x.PreventAll))
        {
            return Array.Empty<string>();
        }

        if (allResults.Any(n => n.PreventDefault))
        {
            // Only return non-generated suggestions
            return allResults
                .Where(s => !s.IsGenerated)
                .SelectMany(s => s.Suggestions)
                .Distinct()
                .ToArray();
        }

        // Prefer manual suggestions over generated ones
        return allResults
            .OrderBy(s => s.IsGenerated)
            .SelectMany(s => s.Suggestions)
            .Distinct()
            .ToArray();
    }

    private static CompletionResult GetChildCommands(string partialElement, CommandInfo parent)
    {
        return GetSuggestions(partialElement, parent.Children);
    }

    private static CompletionResult GetSuggestions(string? partialElement, IEnumerable<CommandInfo> commands)
    {
        return commands.Where(cmd => !cmd.IsHidden)
                    .Select(c => c.Name)
                    .Where(n => string.IsNullOrEmpty(partialElement) || n.StartsWith(partialElement, StringComparison.OrdinalIgnoreCase))
                    .ToArray();
    }

    private List<CompletionResult> GetParameters(CommandCompletionContext context)
    {
        var mappedLongNames = context.MappedParameters
                .Select(x => x.Parameter)
                .OfType<CommandOption>()
                .SelectMany(x => x.LongNames)
                .Select(x => $"--{x}")
                .ToArray()
                ;

        var parameters = new List<CompletionResult>();
        foreach (var parameter in context.Parent.Parameters)
        {
            var startsWithDash = context.PartialElement.StartsWith("-");
            var isEmpty = string.IsNullOrEmpty(context.PartialElement);

            if (parameter is CommandOption commandOptionParameter && (startsWithDash || isEmpty))
            {
                // Add all matching long parameter names
                CompletionResult completions = commandOptionParameter.LongNames
                                    .Select(l => "--" + l.ToLowerInvariant())
                                    .Where(p => p.StartsWith(context.PartialElement))
                                    .Where(x => !mappedLongNames.Contains(x, StringComparer.OrdinalIgnoreCase)) // ignore already mapped
                                    .ToArray();

                if (completions.Suggestions.Any())
                {
                    parameters.Add(completions.WithGeneratedSuggestions());
                }
            }
        }

        return parameters;
    }

    private async Task<List<CompletionResult>> GetCommandArgumentsAsync(CommandCompletionContext context)
    {
        if (!string.IsNullOrEmpty(context.PartialElement) && context.PartialElement[0] == '-')
        {
            return new List<CompletionResult>();
        }

        //// Trailing space: The first empty parameter should be completed
        //// No trailing space: The last parameter should be completed
        var hasTrailingSpace = context.CommandElements.LastOrDefault()?.Length == 0;
        var lastMap = context.MappedParameters?.LastOrDefault();

        if (!hasTrailingSpace)
        {
            if (lastMap?.Parameter is null)
            {
                return new List<CompletionResult>();
            }

            var completions = await CompleteCommandOption(context.Parent, lastMap.Parameter, lastMap.Value);
            if (completions == null)
            {
                return new List<CompletionResult>();
            }

            if (completions.Suggestions.Any() || completions.PreventDefault)
            {
                return new List<CompletionResult> { new(completions) };
            }
        }

        var result = new List<CompletionResult>();
        foreach (var parameter in context.MappedParameters)
        {
            if (!string.IsNullOrEmpty(parameter.Value))
            {
                continue;
            }

            if (parameter.Parameter is null)
            {
                continue;
            }

            var completions = await CompleteCommandOption(context.Parent, parameter.Parameter, parameter.Value);
            if (completions == null || !completions.Suggestions.Any())
            {
                return new List<CompletionResult>()
                {
                    new(Array.Empty<string>()) { PreventAll = true, PreventDefault = true, },
                };
            }

            if (completions.Suggestions.Any() || completions.PreventDefault)
            {
                result.Add(new(completions));
                break;
            }
        }

        return result;
    }

    private async Task<CompletionResult?> CompleteCommandOption(CommandInfo parent, CommandParameter parameter, string? partialElement)
    {
        partialElement ??= string.Empty;

        var valuesViaAttributes = parameter.Property.GetCustomAttributes<CompletionSuggestionsAttribute>();
        if (valuesViaAttributes?.Any() == true)
        {
            var values = valuesViaAttributes
                .Where(x => x.Suggestions != null)
                .SelectMany(x => x.Suggestions)
                .Where(x => string.IsNullOrEmpty(partialElement) || x.StartsWith(partialElement, StringComparison.OrdinalIgnoreCase))
                ;

            return new(values, true);
        }

        var commandType = parent.CommandType;
        if (commandType == null)
        {
            return CompletionResult.None();
        }

        var implementsCompleter = commandType.GetInterfaces().Any(x => x == typeof(ICommandCompletable) || x == typeof(IAsyncCommandCompletable));

        if (!implementsCompleter)
        {
            return CompletionResult.None();
        }

        var completer = _typeResolver.Resolve(commandType);
        if (completer is IAsyncCommandCompletable typedAsyncCompleter)
        {
            return await typedAsyncCompleter.GetSuggestionsAsync(parameter, partialElement);
        }

        if (completer is ICommandCompletable typedCompleter)
        {
            return typedCompleter.GetSuggestions(parameter, partialElement);
        }

        return CompletionResult.None();
    }

    /// <summary>
    /// Prevents arbitrary consoles from being used by the completion command. (For example, logging consoles).
    /// </summary>
    private static void HighjackConsoles()
    {
        try
        {
            System.Console.Clear();
        }
        catch
        {
            // Ignored
        }

        AnsiConsole.Console = new HighjackedAnsiConsole(AnsiConsole.Console);
        System.Console.SetOut(new HighjackedTextWriter(System.Console.Out));
    }
}