using static Spectre.Console.Cli.CommandTreeTokenizer;

namespace Spectre.Console.Cli;

internal sealed class CommandExecutor
{
    private readonly ITypeRegistrar _registrar;

    public CommandExecutor(ITypeRegistrar registrar)
    {
        _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
        _registrar.Register(typeof(DefaultPairDeconstructor), typeof(DefaultPairDeconstructor));
    }

    public async Task<int> Execute(IConfiguration configuration, IEnumerable<string> args)
    {
        CommandTreeParserResult parsedResult;

        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var arguments = args.ToSafeReadOnlyList();

        _registrar.RegisterInstance(typeof(IConfiguration), configuration);
        _registrar.RegisterLazy(typeof(IAnsiConsole), () => configuration.Settings.Console.GetConsole());

        // Create the command model.
        var model = CommandModelBuilder.Build(configuration);
        _registrar.RegisterInstance(typeof(CommandModel), model);
        _registrar.RegisterDependencies(model);

        // Got at least one argument?
        var firstArgument = arguments.FirstOrDefault();
        if (firstArgument != null)
        {
            // Asking for version?
            if (firstArgument.Equals("-v", StringComparison.OrdinalIgnoreCase) ||
                firstArgument.Equals("--version", StringComparison.OrdinalIgnoreCase))
            {
                if (configuration.Settings.ApplicationVersion != null)
                {
                    // We need to check if the command has a version option on its setting class.
                    // Do this by first parsing the command line args and checking the remaining args.
                    try
                    {
                        // Parse and map the model against the arguments.
                        parsedResult = ParseCommandLineArguments(model, configuration.Settings, arguments);
                    }
                    catch (Exception)
                    {
                        // Something went wrong with parsing the command line arguments,
                        // however we know the first argument is a version option.
                        var console = configuration.Settings.Console.GetConsole();
                        console.MarkupLine(configuration.Settings.ApplicationVersion);
                        return 0;
                    }

                    // Check the parsed remaining args for the version options.
                    if ((firstArgument.Equals("-v", StringComparison.OrdinalIgnoreCase) && parsedResult.Remaining.Parsed.Contains("-v")) ||
                        (firstArgument.Equals("--version", StringComparison.OrdinalIgnoreCase) && parsedResult.Remaining.Parsed.Contains("--version")))
                    {
                        // The version option is not a member of the command settings.
                        var console = configuration.Settings.Console.GetConsole();
                        console.MarkupLine(configuration.Settings.ApplicationVersion);
                        return 0;
                    }
                }
            }

            // OpenCLI?
            if (firstArgument.Equals(CliConstants.DumpHelpOpenCliOption, StringComparison.OrdinalIgnoreCase))
            {
                // Replace all arguments with the opencligen command
                arguments = ["cli", "opencli"];
            }
        }

        // Parse and map the model against the arguments.
        parsedResult = ParseCommandLineArguments(model, configuration.Settings, arguments);

        // Register the arguments with the container.
        _registrar.RegisterInstance(typeof(CommandTreeParserResult), parsedResult);
        _registrar.RegisterInstance(typeof(IRemainingArguments), parsedResult.Remaining);

        // Create the resolver.
        using (var resolver = new TypeResolverAdapter(_registrar.Build()))
        {
            // Get the registered help provider, falling back to the default provider
            // if no custom implementations have been registered.
            var helpProviders = resolver.Resolve(typeof(IEnumerable<IHelpProvider>)) as IEnumerable<IHelpProvider>;
            var helpProvider = helpProviders?.LastOrDefault() ?? new HelpProvider(configuration.Settings);

            // Currently the root?
            if (parsedResult?.Tree == null)
            {
                // Display help.
                configuration.Settings.Console.SafeRender(helpProvider.Write(model, null));
                return 0;
            }

            // Get the command to execute.
            var leaf = parsedResult.Tree.GetLeafCommand();
            if (leaf.Command.IsBranch || leaf.ShowHelp)
            {
                // Branches can't be executed. Show help.
                configuration.Settings.Console.SafeRender(helpProvider.Write(model, leaf.Command));
                return leaf.ShowHelp ? 0 : 1;
            }

            // Is this the default and is it called without arguments when there are required arguments?
            if (leaf.Command.IsDefaultCommand && arguments.Count == 0 && leaf.Command.Parameters.Any(p => p.IsRequired))
            {
                // Display help for default command.
                configuration.Settings.Console.SafeRender(helpProvider.Write(model, leaf.Command));
                return 1;
            }

            // Create the content.
            var context = new CommandContext(
                arguments,
                parsedResult.Remaining,
                leaf.Command.Name,
                leaf.Command.Data);

            // Execute the command tree.
            return await Execute(leaf, parsedResult.Tree, context, resolver, configuration).ConfigureAwait(false);
        }
    }

    [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1513:Closing brace should be followed by blank line", Justification = "Improves code readability by grouping together related statements into a block")]
    private CommandTreeParserResult ParseCommandLineArguments(CommandModel model, CommandAppSettings settings, IReadOnlyList<string> args)
    {
        CommandTreeParserResult? parsedResult = null;
        CommandTreeTokenizerResult tokenizerResult;

        try
        {
            (parsedResult, tokenizerResult) = InternalParseCommandLineArguments(model, settings, args);

            var lastParsedLeaf = parsedResult.Tree?.GetLeafCommand();
            var lastParsedCommand = lastParsedLeaf?.Command;

            if (lastParsedLeaf != null && lastParsedCommand != null &&
            lastParsedCommand.IsBranch && !lastParsedLeaf.ShowHelp &&
            lastParsedCommand.DefaultCommand != null)
            {
                // Adjust for any parsed remaining arguments by
                // inserting the the default command ahead of them.
                var position = tokenizerResult.Tokens.Position;
                foreach (var parsedRemaining in parsedResult.Remaining.Parsed)
                {
                    position--;
                    position -= parsedRemaining.Count(value => value != null);
                }
                position = position < 0 ? 0 : position;

                // Insert this branch's default command into the command line
                // arguments and try again to see if it will parse.
                var argsWithDefaultCommand = new List<string>(args);
                argsWithDefaultCommand.Insert(position, lastParsedCommand.DefaultCommand.Name);

                (parsedResult, tokenizerResult) = InternalParseCommandLineArguments(model, settings, argsWithDefaultCommand);
            }
        }
        catch (CommandParseException) when (parsedResult == null && settings.ParsingMode == ParsingMode.Strict)
        {
            // The parsing exception might be resolved by adding in the default command,
            // but we can't know for sure. Take a brute force approach and try this for
            // every position between the arguments.
            for (int i = 0; i < args.Count; i++)
            {
                var argsWithDefaultCommand = new List<string>(args);
                argsWithDefaultCommand.Insert(args.Count - i, "__default_command");

                try
                {
                    (parsedResult, tokenizerResult) = InternalParseCommandLineArguments(model, settings, argsWithDefaultCommand);

                    break;
                }
                catch (CommandParseException)
                {
                    // Continue.
                }
            }

            if (parsedResult == null)
            {
                // Failed to parse having inserted the default command between each argument.
                // Repeat the parsing of the original arguments to throw the correct exception.
                InternalParseCommandLineArguments(model, settings, args);
            }
        }

        if (parsedResult == null)
        {
            // The arguments failed to parse despite everything we tried above.
            // Exceptions should be thrown above before ever getting this far,
            // however the following is the ulimately backstop and avoids
            // the compiler from complaining about returning null.
            throw CommandParseException.UnknownParsingError();
        }

        return parsedResult;
    }

    /// <summary>
    /// Parse the command line arguments using the specified <see cref="CommandModel"/> and <see cref="CommandAppSettings"/>,
    /// returning the parser and tokenizer results.
    /// </summary>
    /// <returns>The parser and tokenizer results as a tuple.</returns>
    private (CommandTreeParserResult ParserResult, CommandTreeTokenizerResult TokenizerResult) InternalParseCommandLineArguments(CommandModel model, CommandAppSettings settings, IReadOnlyList<string> args)
    {
        var parser = new CommandTreeParser(model, settings.CaseSensitivity, settings.ParsingMode, settings.ConvertFlagsToRemainingArguments);

        var parserContext = new CommandTreeParserContext(args, settings.ParsingMode);
        var tokenizerResult = CommandTreeTokenizer.Tokenize(args);
        var parsedResult = parser.Parse(parserContext, tokenizerResult);

        return (parsedResult, tokenizerResult);
    }

    private static async Task<int> Execute(
        CommandTree leaf,
        CommandTree tree,
        CommandContext context,
        ITypeResolver resolver,
        IConfiguration configuration)
    {
        try
        {
            // Bind the command tree against the settings.
            var settings = CommandBinder.Bind(tree, leaf.Command.SettingsType, resolver);
            var interceptors =
                ((IEnumerable<ICommandInterceptor>?)resolver.Resolve(typeof(IEnumerable<ICommandInterceptor>))
                ?? Array.Empty<ICommandInterceptor>()).ToList();
#pragma warning disable CS0618 // Type or member is obsolete
            if (configuration.Settings.Interceptor != null)
            {
                interceptors.Add(configuration.Settings.Interceptor);
            }
#pragma warning restore CS0618 // Type or member is obsolete
            foreach (var interceptor in interceptors)
            {
                interceptor.Intercept(context, settings);
            }

            // Create and validate the command.
            var command = leaf.CreateCommand(resolver);
            var validationResult = command.Validate(context, settings);
            if (!validationResult.Successful)
            {
                throw CommandRuntimeException.ValidationFailed(validationResult);
            }

            // Execute the command.
            var result = await command.Execute(context, settings);
            foreach (var interceptor in interceptors)
            {
                interceptor.InterceptResult(context, settings, ref result);
            }

            return result;
        }
        catch (Exception ex) when (configuration.Settings is { ExceptionHandler: not null, PropagateExceptions: false })
        {
            return configuration.Settings.ExceptionHandler(ex, resolver);
        }
    }
}