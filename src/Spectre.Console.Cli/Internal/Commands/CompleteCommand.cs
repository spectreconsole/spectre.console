namespace Spectre.Console.Cli;

[Description("Generates a list of completion options for the given command.")]
[SuppressMessage("Performance", "CA1812: Avoid uninstantiated internal classes")]
internal sealed class CompleteCommand : Command<CompleteCommand.Settings>
{
    private readonly CommandModel _model;
    private readonly ITypeResolver _typeResolver;
    private readonly IAnsiConsole _writer;
    private readonly IConfiguration _configuration;

    public CompleteCommand
    (
        IConfiguration configuration,
        CommandModel model
        , ITypeResolver typeResolver
    )
    {
        _model = model ?? throw new ArgumentNullException(nameof(model));
        _typeResolver = typeResolver;
        _writer = configuration.Settings.Console.GetConsole();
        _configuration = configuration;
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

    private string[] GetCompletions(CommandModel model, Settings settings)
    {
        // Get all command elements and skip the application name at the start.
        var commandElements = settings.CommandToComplete?
            .TrimStart('"')
            .TrimEnd('"')
            .Split(' ').Skip(1).ToArray();

        // Return early if the only thing we got was "".
        if (commandElements == null ||
           (commandElements.Count() == 1 &&
            string.IsNullOrEmpty(commandElements.First())))
        {
            return model.Commands.Where(cmd => !cmd.IsHidden)
                                 .Select(c => c.Name)
                                 .ToArray();
        }

        // Parse the command elements to get an abstract syntax tree and some context
        CommandTreeParserResult? parsedResult = null;
        var parser = new CommandTreeParser(model, _configuration.Settings.CaseSensitivity);
        var context = string.Empty;
        var partialElement = string.Empty;
        try
        {
            parsedResult = parser.Parse(commandElements);
            context = commandElements.Last();
            if (string.IsNullOrEmpty(context))
            {
                // Because we support netstandard2.0, we can't use SkipLast, since it's not supported. Also negative indexes are a no go.
                // There probably is a more elegant way to get this result that I just can't see now.
                // context = commandElements.SkipLast(1).Last();
                context = commandElements.ToArray()[commandElements.Count() - 2];
            }
        }
        catch (CommandParseException)
        {
            // Assume that it's because the last commandElement was not complete, and omit that one.
            var strippedCommandElements = commandElements.Take(commandElements.Count() - 1);
            if (strippedCommandElements.Any())
            {
                parsedResult = parser.Parse(strippedCommandElements);
                context = strippedCommandElements.Last();
                partialElement = commandElements.Last().ToLowerInvariant();
            }
        }


        // Return command options based on our current context, filtered on any partial element we found.
        // If partial element = "", the StartsWith will return all options.
        CommandInfo parent;
        if (parsedResult?.Tree == null)
        {
            return model.Commands.Where(cmd => !cmd.IsHidden)
                                 .Select(c => c.Name)
                                 .Where(n => n.StartsWith(partialElement))
                                 .ToArray();
        }
        else
        {
            // The Tree does not natively support walking or visiting, so we need to search it manually.
            parent = FindContextInTree(parsedResult.Tree, context);
        }

        // No idea why this fixes test 7: Parameters
        parent ??= parsedResult.Tree.Command;

        CompletionResult childCommands = parent.Children.Where(cmd => !cmd.IsHidden)
                              .Select(c => c.Name)
                              .Where(n => partialElement == string.Empty || n.StartsWith(partialElement))
                              .ToArray();

        childCommands = childCommands.WithGeneratedSuggestions();

        var parameters = GetParameters(parent, partialElement);
        var arguments = GetCommandArguments(parent, parsedResult.Tree.Mapped, commandElements);

        var allResults = parameters.Concat(arguments).Append(childCommands).ToArray();

        if (allResults.Any(n => n.PreventDefault))
        {
            // Only return non-generated suggestions
            return allResults
                .Where(s => !s.IsGenerated)
                .SelectMany(s => s.Suggestions)
                .Distinct()
                .ToArray();
        }

        return allResults
            //Prefer manual suggestions over generated ones
            .OrderBy(s => s.IsGenerated)
            .SelectMany(s => s.Suggestions)
            .Distinct()
            .ToArray();
    }

    private List<CompletionResult> GetCommandArguments(CommandInfo parent, List<MappedCommandParameter> mapped, string[] args)
    {
        // Trailing space: The first empty parameter should be completed
        // No trailing space: The last parameter should be completed
        var hasTrailingSpace = args.LastOrDefault() == string.Empty;
        var lastIsCommandArgument = mapped.LastOrDefault()?.Parameter is CommandArgument;

        if (!hasTrailingSpace)
        {
            if(!lastIsCommandArgument)
            {
                return new List<CompletionResult>();
            }

            var lastMap = mapped.Last();
            var lastArgument = lastMap.Parameter as CommandArgument;
            var completions = CompleteCommandArgument(lastArgument, lastMap.Value);
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
        foreach (var parameter in mapped)
        {
            if (!string.IsNullOrEmpty(parameter.Value))
            {
                continue;
            }

            if (parameter.Parameter is not CommandArgument commandArgumentParameter)
            {
                continue;
            }

            var completions = CompleteCommandArgument(commandArgumentParameter, parameter.Value);
            if (completions == null)
            {
                continue;
            }

            if (completions.Suggestions.Any() || completions.PreventDefault)
            {
                result.Add(new(completions));
            }
        }

        return result;

        ICompletionResult? CompleteCommandArgument(CommandArgument commandArgumentParameter, string partialElement)
        {
            var commandType = parent.CommandType;
            // check if ICommandParameterCompleter is implemented
            var implementsCompleter = commandType
               .GetInterfaces()
               .Any(i => i == typeof(ICommandParameterCompleter));

            if (!implementsCompleter)
            {
               return CompletionResult.None();
            }

            var completer = _typeResolver.Resolve(commandType);
            var completions = ((ICommandParameterCompleter)completer).GetSuggestions(commandArgumentParameter, partialElement);
            return completions;
        }
    }

    private List<CompletionResult> GetParameters(CommandInfo parent, string partialElement)
    {
        var parameters = new List<CompletionResult>();
        foreach (var parameter in parent.Parameters)
        {
            var startsWithDash = partialElement.StartsWith("-");
            var isEmpty = string.IsNullOrEmpty(partialElement);

            if (parameter is CommandOption commandOptionParameter && (startsWithDash || isEmpty))
            {
                // It doesn't actually make much sense to autocomplete one-char parameters
                // parameters.AddRangeIfNotNull(
                //     commandOptionParameter.ShortNames
                //                           .Select(s => "-" + s.ToLowerInvariant())
                //                           .Where(p => p.StartsWith(partialElement)));
                // Add all matching long parameter names

                CompletionResult completions = commandOptionParameter.LongNames
                                    .Select(l => "--" + l.ToLowerInvariant())
                                    .Where(p => p.StartsWith(partialElement))
                                    .ToArray();
                if (completions.Suggestions.Any())
                {
                    parameters.Add(completions.WithGeneratedSuggestions());
                }
            }
        }

        return parameters;
    }

    private static CommandInfo FindContextInTree(CommandTree tree, string context)
    {
        // This needs to become a recursive function, but for the simpler situations this would work.
        var commandInfo = tree.Command;

        if (commandInfo.Name != context)
        {
            commandInfo = tree.Command.Children.FirstOrDefault(c => c.Name == context);
        }

        return commandInfo;
    }
}