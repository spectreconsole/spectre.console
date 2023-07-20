using Spectre.Console.Cli.Completion;

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
        var commandToComplete = settings.CommandToComplete;
        if (string.IsNullOrEmpty(commandToComplete))
        {
            // No command to complete, so just print the application name.
            _writer.WriteLine(_model.ApplicationName ?? string.Empty, Style.Plain);
            return 0;
        }

        // Get all command elements and skip the application name at the start.
        var normalizedCommand = commandToComplete
            .TrimStart('"')
            .TrimEnd('"');

        if (!string.IsNullOrEmpty(normalizedCommand)
            && settings.Position != null
            && settings.Position > normalizedCommand.Length)
        {
            // extend the command to the position with whitespace
            var requiredWhitespace = (settings.Position - normalizedCommand.Length) ?? 1;
            normalizedCommand += new string(' ', requiredWhitespace);
        }

        foreach (var completion in await GetCompletionsAsync(_model, normalizedCommand))
        {
            _writer.WriteLine(completion, Style.Plain);
        }

        return 0;
    }

    private async Task<string[]> GetCompletionsAsync(CommandModel model, string command)
    {
        var commandElements = command
            .Split(' ').Skip(1).ToArray();

        if (commandElements?.Length is 0 or null)
        {
            return GetSuggestions(string.Empty, model.Commands).Suggestions.ToArray();
        }

        // Return early if the only thing we got was "".
        if (commandElements == null ||
           (commandElements.Length == 1 &&
            string.IsNullOrEmpty(commandElements[0])))
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
            context = commandElements.LastOrDefault() ?? string.Empty;
            if (string.IsNullOrEmpty(context))
            {
                // Because we support netstandard2.0, we can't use SkipLast, since it's not supported. Also negative indexes are a no go.
                // There probably is a more elegant way to get this result that I just can't see now.
                // context = commandElements.SkipLast(1).Last();
                context = commandElements.ToArray()[commandElements.Length - 2];
            }

            // Early return when "myapp feline"
            // but show completions for feline if "myapp feline "
            // spacing matters
            if (parsedResult.Tree?.Command.Name == context)
            {
                return Array.Empty<string>();
            }
        }
        catch (CommandParseException)
        {
            // Assume that it's because the last commandElement was not complete, and omit that one.
            var strippedCommandElements = commandElements.Take(commandElements.Length - 1);
            if (strippedCommandElements.Any())
            {
                parsedResult = parser.Parse(strippedCommandElements);
                context = strippedCommandElements.Last();
            }

            partialElement = commandElements.LastOrDefault()?.ToLowerInvariant() ?? string.Empty;
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
            parent = FindContextInTree(parsedResult.Tree, context)
                ?? parsedResult.Tree.Command;
        }

        // No idea why this fixes test 7: Parameters
        parent ??= parsedResult.Tree.Command;

        var childCommands = GetChildCommands(partialElement, parent);

        childCommands = childCommands.WithGeneratedSuggestions();

        var parameters = GetParameters(parent, partialElement);
        var arguments = await GetCommandArgumentsAsync(parent, parsedResult.Tree.Mapped, commandElements, partialElement);

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

        // return parent.Children.Where(cmd => !cmd.IsHidden)
        //            .Select(c => c.Name)
        //            .Where(n => string.IsNullOrEmpty(partialElement) || n.StartsWith(partialElement))
        //            .ToArray();
    }

    private static CompletionResult GetSuggestions(string partialElement, IEnumerable<CommandInfo> commands)
    {
        return commands.Where(cmd => !cmd.IsHidden)
                    .Select(c => c.Name)
                    .Where(n => string.IsNullOrEmpty(partialElement) || n.StartsWith(partialElement))
                    .ToArray();
    }

    private async Task<List<CompletionResult>> GetCommandArgumentsAsync(CommandInfo parent, List<MappedCommandParameter> mapped, string[] args, string partialElement)
    {
        if (!string.IsNullOrEmpty(partialElement) && partialElement[0] == '-')
        {
            return new List<CompletionResult>();
        }

        // Trailing space: The first empty parameter should be completed
        // No trailing space: The last parameter should be completed
        var hasTrailingSpace = args.LastOrDefault()?.Length == 0;
        var lastIsCommandArgument = mapped.LastOrDefault()?.Parameter is CommandArgument;

        if (!hasTrailingSpace)
        {
            if (!lastIsCommandArgument)
            {
                return new List<CompletionResult>();
            }

            var lastMap = mapped.Last();
            if (lastMap.Parameter is not CommandArgument lastArgument)
            {
                return new List<CompletionResult>();
            }

            var completions = await CompleteCommandOption(parent, lastArgument, lastMap.Value);
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

            // if (parameter.Parameter is CommandArgument commandArgumentParameter)
            // {
            //     var completions = CompleteCommandOption(parent, commandArgumentParameter, parameter.Value);
            //     if (completions == null)
            //     {
            //         continue;
            //     }

            // if (completions.Suggestions.Any() || completions.PreventDefault)
            //     {
            //         result.Add(new(completions));
            //     }
            // }
            // else if (parameter.Parameter is CommandOption option)
            // {
            //     // arrive on
            //     // "\"myapp lion 2 4 --name \""
            //     //
            //     // Debugger.Break();
            //     var completions = CompleteCommandOption(parent, option, parameter.Value);
            //     if (completions == null)
            //     {
            //         continue;
            //     }

            // if (completions.Suggestions.Any() || completions.PreventDefault)
            //     {
            //         result.Add(new(completions));
            //     }
            // }
            if (parameter.Parameter is ICommandParameterInfo commandArgumentParameter)
            {
                var completions = await CompleteCommandOption(parent, commandArgumentParameter, parameter.Value);
                if (completions == null)
                {
                    continue;
                }

                if (completions.Suggestions.Any() || completions.PreventDefault)
                {
                    result.Add(new(completions));
                }
            }
            else
            {
                // Should not happen.
#if DEBUG
                Debugger.Break();
#endif
            }
        }

        return result;
    }

    private async Task<CompletionResult?> CompleteCommandOption(CommandInfo parent, ICommandParameterInfo commandArgumentParameter, string? partialElement)
    {
        partialElement ??= string.Empty;

        var commandType = parent.CommandType;
        if (commandType == null)
        {
            return CompletionResult.None();
        }

        var implementsCompleter = false;
        var implementsAsyncCompleter = false;
        foreach (var item in commandType.GetInterfaces())
        {
            if (item == typeof(ICommandCompletable))
            {
                implementsCompleter = true;
            }
            else if (item == typeof(IAsyncCommandCompletable))
            {
                implementsAsyncCompleter = true;
            }

            if (implementsCompleter && implementsAsyncCompleter)
            {
                break;
            }
        }

        if (!(implementsCompleter || implementsAsyncCompleter))
        {
            return CompletionResult.None();
        }

        var completer = _typeResolver.Resolve(commandType);
        if (completer is IAsyncCommandCompletable typedAsyncCompleter)
        {
            return await typedAsyncCompleter.GetSuggestionsAsync(commandArgumentParameter, partialElement);
        }

        if (completer is ICommandCompletable typedCompleter)
        {
            return typedCompleter.GetSuggestions(commandArgumentParameter, partialElement);
        }

        return CompletionResult.None();
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

    private static CommandInfo? FindContextInTree(CommandTree tree, string context)
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