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

        // No default command?
        if (model.DefaultCommand == null)
        {
            // Got at least one argument?
            var firstArgument = arguments.FirstOrDefault();
            if (firstArgument != null)
            {
                // Asking for version? Kind of a hack, but it's alright.
                // We should probably make this a bit better in the future.
                if (firstArgument.Equals("--version", StringComparison.OrdinalIgnoreCase) ||
                    firstArgument.Equals("-v", StringComparison.OrdinalIgnoreCase))
                {
                    if (configuration.Settings.ApplicationVersion != null)
                    {
                        var console = configuration.Settings.Console.GetConsole();
                        console.MarkupLine(configuration.Settings.ApplicationVersion);
                        return 0;
                    }
                }
            }
        }

        // Parse and map the model against the arguments.
        var parsedResult = ParseCommandLineArguments(model, configuration.Settings, arguments);

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
            if (leaf.Command.IsDefaultCommand && arguments.Count == 0 && leaf.Command.Parameters.Any(p => p.Required))
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

    private CommandTreeParserResult ParseCommandLineArguments(CommandModel model, CommandAppSettings settings, IReadOnlyList<string> args)
    {
        var parser = new CommandTreeParser(model, settings.CaseSensitivity, settings.ParsingMode, settings.ConvertFlagsToRemainingArguments);

        var parserContext = new CommandTreeParserContext(args, settings.ParsingMode);
        var tokenizerResult = CommandTreeTokenizer.Tokenize(args);
        var parsedResult = parser.Parse(parserContext, tokenizerResult);

        var lastParsedLeaf = parsedResult.Tree?.GetLeafCommand();
        var lastParsedCommand = lastParsedLeaf?.Command;
        if (lastParsedLeaf != null && lastParsedCommand != null &&
            lastParsedCommand.IsBranch && !lastParsedLeaf.ShowHelp &&
            lastParsedCommand.DefaultCommand != null)
        {
            // Insert this branch's default command into the command line
            // arguments and try again to see if it will parse.
            var argsWithDefaultCommand = new List<string>(args);

            argsWithDefaultCommand.Insert(tokenizerResult.Tokens.Position, lastParsedCommand.DefaultCommand.Name);

            parserContext = new CommandTreeParserContext(argsWithDefaultCommand, settings.ParsingMode);
            tokenizerResult = CommandTreeTokenizer.Tokenize(argsWithDefaultCommand);
            parsedResult = parser.Parse(parserContext, tokenizerResult);
        }

        return parsedResult;
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