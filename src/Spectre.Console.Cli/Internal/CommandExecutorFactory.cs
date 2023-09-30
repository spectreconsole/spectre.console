namespace Spectre.Console.Cli.Internal;

internal class CommandExecutorFactory
{
    private ITypeRegistrar _registrar;

    public CommandExecutorFactory(ITypeRegistrar registrar)
    {
        _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
    }

    public CommandExecutor CreateCommandExecutor(IConfiguration configuration, IEnumerable<string> args)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        args ??= new List<string>();

        // Asking for version? Kind of a hack, but it's alright.
        // We should probably make this a bit better in the future.
        if (args.Contains("-v") || args.Contains("--version"))
        {
            return new CommandExecutor(
                registrar: _registrar,
                configuration: configuration,
                args: args,
                requiresVersion: true,
                defaultHelpProvider: null,
                parsedResult: null,
                model: null);
        }

        _registrar.Register(typeof(DefaultPairDeconstructor), typeof(DefaultPairDeconstructor));
        _registrar.RegisterInstance(typeof(IConfiguration), configuration);
        _registrar.RegisterLazy(typeof(IAnsiConsole), () => configuration.Settings.Console.GetConsole());

        // Register the help provider
        var defaultHelpProvider = new HelpProvider(configuration.Settings);
        _registrar.RegisterInstance(typeof(IHelpProvider), defaultHelpProvider);

        // Create the command model.
        var model = CommandModelBuilder.Build(configuration);
        _registrar.RegisterInstance(typeof(CommandModel), model);
        _registrar.RegisterDependencies(model);

        // Parse and map the model against the arguments.
        var parsedResult = ParseCommandLineArguments(model, configuration.Settings, args);

        // Register the arguments with the container.
        _registrar.RegisterInstance(typeof(CommandTreeParserResult), parsedResult);
        _registrar.RegisterInstance(typeof(IRemainingArguments), parsedResult.Remaining);

        return new CommandExecutor(
            registrar: _registrar,
            configuration: configuration,
            args: args,
            requiresVersion: false,
            defaultHelpProvider: defaultHelpProvider,
            parsedResult: parsedResult,
            model: model
        );
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
}