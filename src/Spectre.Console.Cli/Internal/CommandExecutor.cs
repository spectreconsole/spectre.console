namespace Spectre.Console.Cli;

internal sealed class CommandExecutor
{
    private readonly ITypeRegistrar _registrar;

    public IConfiguration Configuration { get; }
    public IEnumerable<string> Args { get; }
    public bool RequiresVersion { get; internal set; }
    public HelpProvider DefaultHelpProvider { get; internal set; }
    public CommandTreeParserResult ParsedResult { get; internal set; }
    public CommandModel Model { get; internal set; }

    public CommandExecutor(
        ITypeRegistrar registrar,
        IConfiguration configuration,
        IEnumerable<string> args,
        bool requiresVersion,
        HelpProvider? defaultHelpProvider,
        CommandTreeParserResult? parsedResult,
        CommandModel? model)
    {
        _registrar = registrar ?? throw new ArgumentNullException(nameof(registrar));
        Configuration = configuration;
        Args = args;
        _registrar.Register(typeof(DefaultPairDeconstructor), typeof(DefaultPairDeconstructor));
        RequiresVersion = requiresVersion;

        if (requiresVersion)
        {
            return;
        }

        DefaultHelpProvider = defaultHelpProvider ?? throw new ArgumentNullException(nameof(defaultHelpProvider));
        ParsedResult = parsedResult ?? throw new ArgumentNullException(nameof(parsedResult));
        Model = model ?? throw new ArgumentNullException(nameof(model));
    }

    public async Task<int> Execute()
    {
        // Asking for version? Kind of a hack, but it's alright.
        // We should probably make this a bit better in the future.
        if (RequiresVersion)
        {
            var console = Configuration.Settings.Console.GetConsole();
            console.WriteLine(ResolveApplicationVersion(Configuration));
            return 0;
        }

        // Create the resolver.
        using var resolver = new TypeResolverAdapter(_registrar.Build());

        // Get the registered help provider, falling back to the default provider
        // registered above if no custom implementations have been registered.
        var helpProvider = resolver.Resolve(typeof(IHelpProvider)) as IHelpProvider ?? DefaultHelpProvider;

        // Currently the root?
        if (ParsedResult?.Tree == null)
        {
            // Display help.
            Configuration.Settings.Console.SafeRender(helpProvider.Write(Model, null));
            return 0;
        }

        // Get the command to execute.
        var leaf = ParsedResult.Tree.GetLeafCommand();
        if (leaf.Command.IsBranch || leaf.ShowHelp)
        {
            // Branches can't be executed. Show help.
            Configuration.Settings.Console.SafeRender(helpProvider.Write(Model, leaf.Command));
            return leaf.ShowHelp ? 0 : 1;
        }

        // Is this the default and is it called without arguments when there are required arguments?
        if (leaf.Command.IsDefaultCommand && Args.Count() == 0 && leaf.Command.Parameters.Any(p => p.Required))
        {
            // Display help for default command.
            Configuration.Settings.Console.SafeRender(helpProvider.Write(Model, leaf.Command));
            return 1;
        }

        // Create the content.
        var context = new CommandContext(ParsedResult.Remaining, leaf.Command.Name, leaf.Command.Data);

        // Execute the command tree.
        return await Execute(leaf, ParsedResult.Tree, context, resolver, Configuration).ConfigureAwait(false);
    }

    public TypeResolverAdapter BuildTypeResolver()
    {
        return new TypeResolverAdapter(_registrar.Build());
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