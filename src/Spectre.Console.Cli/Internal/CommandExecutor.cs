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

        args ??= new List<string>();

        _registrar.RegisterInstance(typeof(IConfiguration), configuration);
        _registrar.RegisterLazy(typeof(IAnsiConsole), () => configuration.Settings.Console.GetConsole());

        // Create the command model.
        var model = CommandModelBuilder.Build(configuration);
        _registrar.RegisterInstance(typeof(CommandModel), model);
        _registrar.RegisterDependencies(model);

        // Asking for version? Kind of a hack, but it's alright.
        // We should probably make this a bit better in the future.
        if (args.Contains("-v") || args.Contains("--version"))
        {
            var console = configuration.Settings.Console.GetConsole();
            console.WriteLine(ResolveApplicationVersion(configuration));
            return 0;
        }

        // Parse and map the model against the arguments.
        var parsedResult = ParseCommandLineArguments(model, configuration.Settings, args);

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
            if (leaf.Command.IsDefaultCommand && args.Count() == 0 && leaf.Command.Parameters.Any(p => p.Required))
            {
                // Display help for default command.
                configuration.Settings.Console.SafeRender(helpProvider.Write(model, leaf.Command));
                return 1;
            }

            // Create the content.
            var context = new CommandContext(parsedResult.Remaining, leaf.Command.Name, leaf.Command.Data);

            // Execute the command tree.
            return await Execute(leaf, parsedResult.Tree, context, resolver, configuration).ConfigureAwait(false);
        }
    }

#pragma warning disable CS8603 // Possible null reference return.
    private CommandTreeParserResult ParseCommandLineArguments(CommandModel model, CommandAppSettings settings, IEnumerable<string> args)
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
#pragma warning restore CS8603 // Possible null reference return.

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
        try
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
        catch (Exception ex) when (configuration.Settings is { ExceptionHandler: not null, PropagateExceptions: false })
        {
            return Task.FromResult(configuration.Settings.ExceptionHandler(ex, resolver));
        }
    }
}