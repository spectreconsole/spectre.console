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

        _registrar.RegisterInstance(typeof(IConfiguration), configuration);
        _registrar.RegisterLazy(typeof(IAnsiConsole), () => configuration.Settings.Console.GetConsole());

        // Create the command model.
        var model = CommandModelBuilder.Build(configuration);
        _registrar.RegisterInstance(typeof(CommandModel), model);
        _registrar.RegisterDependencies(model);

        var applicationVersion = ResolveApplicationVersion(configuration);

        // No default command?
        if (model.DefaultCommand == null)
        {
            // Got at least one argument?
            var firstArgument = args.FirstOrDefault();
            if (firstArgument != null)
            {
                // Asking for version? Kind of a hack, but it's alright.
                // We should probably make this a bit better in the future.
                if (firstArgument.Equals("--version", StringComparison.OrdinalIgnoreCase) ||
                    firstArgument.Equals("-v", StringComparison.OrdinalIgnoreCase))
                {
                    var console = configuration.Settings.Console.GetConsole();
                    console.WriteLine(applicationVersion);
                    return 0;
                }
            }
        }

        // Parse and map the model against the arguments.
        var parser = new CommandTreeParser(model, configuration.Settings.CaseSensitivity);
        var parsedResult = parser.Parse(args);
        _registrar.RegisterInstance(typeof(CommandTreeParserResult), parsedResult);

        var helpProvider = new HelpWriter(model, parsedResult, configuration.Settings.ShowOptionDefaultValues) as IHelpProvider;

        // Currently the root?
        if (parsedResult?.Tree == null)
        {
            // Display help.
            configuration.Settings.Console.SafeRender(helpProvider.Help);
            return 0;
        }

        // Get the command to execute.
        var leaf = parsedResult.Tree.GetLeafCommand();
        if (leaf.Command.IsBranch || leaf.ShowHelp)
        {
            // Branches can't be executed. Show help.
            configuration.Settings.Console.SafeRender(helpProvider.Help);
            return leaf.ShowHelp ? 0 : 1;
        }

        // Is this the default and is it called without arguments when there are required arguments?
        if (leaf.Command.IsDefaultCommand && args.Count() == 0 && leaf.Command.Parameters.Any(p => p.Required))
        {
            // Display help for default command.
            configuration.Settings.Console.SafeRender(helpProvider.Help);
            return 1;
        }

        // Register the arguments with the container.
        _registrar.RegisterInstance(typeof(CommandTreeParserResult), parsedResult);
        _registrar.RegisterInstance(typeof(IRemainingArguments), parsedResult.Remaining);

        // Create the resolver and the context.
        using (var resolver = new TypeResolverAdapter(_registrar.Build()))
        {
            var context = new CommandContext(parsedResult.Remaining, leaf.Command.Name, leaf.Command.Data, helpProvider.Help, applicationVersion);

            // Execute the command tree.
            return await Execute(leaf, parsedResult.Tree, context, resolver, configuration).ConfigureAwait(false);
        }
    }

    private CommandTreeParserResult? ParseCommandLineArguments(CommandModel model, CommandAppSettings settings, IEnumerable<string> args)
    {
        var parser = new CommandTreeParser(model, settings.CaseSensitivity, settings.ParsingMode, settings.ConvertFlagsToRemainingArguments);

        var parserContext = new CommandTreeParserContext(args, settings.ParsingMode);
        var tokenizerResult = CommandTreeTokenizer.Tokenize(args);
        var parsedResult = parser.Parse(parserContext, tokenizerResult);

        var lastParsedLeaf = parsedResult?.Tree?.GetLeafCommand();
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

    private static string ResolveApplicationVersion(IConfiguration configuration)
    {
        return
            configuration.Settings.ApplicationVersion ?? // potential override
            VersionHelper.GetVersion(Assembly.GetEntryAssembly());
    }

    private static Task<int> Execute(
        CommandTree leaf,
        CommandTree tree,
        CommandContext context,
        ITypeResolver resolver,
        IConfiguration configuration)
    {
        // Bind the command tree against the settings.
        var settings = CommandBinder.Bind(tree, leaf.Command.SettingsType, resolver);
        configuration.Settings.Interceptor?.Intercept(context, settings);

        // Create and validate the command.
        var command = leaf.CreateCommand(resolver);
        var validationResult = command.Validate(context, settings);
        if (!validationResult.Successful)
        {
            throw CommandRuntimeException.ValidationFailed(validationResult);
        }

        // Execute the command.
        return command.Execute(context, settings);
    }
}