namespace Spectre.Console.Testing;

/// <summary>
/// A <see cref="CommandApp"/> test harness.
/// </summary>
public sealed class CommandAppTester
{
    private Action<CommandApp>? _appConfiguration;
    private Action<IConfigurator>? _configuration;

    /// <summary>
    /// Gets the test console used by both the CommandAppTester and CommandApp.
    /// </summary>
    public TestConsole Console { get; }

    /// <summary>
    /// Gets or sets the Registrar to use in the CommandApp.
    /// </summary>
    public ITypeRegistrar? Registrar { get; set; }

    /// <summary>
    /// Gets or sets the settings for the <see cref="CommandAppTester"/>.
    /// </summary>
    public CommandAppTesterSettings TestSettings { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAppTester"/> class.
    /// </summary>
    /// <param name="registrar">The registrar.</param>
    /// <param name="settings">The settings.</param>
    /// <param name="console">The test console that overrides the default one.</param>
    public CommandAppTester(
        ITypeRegistrar? registrar = null,
        CommandAppTesterSettings? settings = null,
        TestConsole? console = null)
    {
        Registrar = registrar;
        TestSettings = settings ?? new CommandAppTesterSettings();
        Console = console ?? new TestConsole().Width(int.MaxValue);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandAppTester"/> class.
    /// </summary>
    /// <param name="settings">The settings.</param>
    public CommandAppTester(CommandAppTesterSettings settings)
    {
        TestSettings = settings;
        Console = new TestConsole().Width(int.MaxValue);
    }

    /// <summary>
    /// Sets the default command.
    /// </summary>
    /// <param name="description">The optional default command description.</param>
    /// <param name="data">The optional default command data.</param>
    /// <typeparam name="T">The default command type.</typeparam>
    public void SetDefaultCommand<T>(string? description = null, object? data = null)
        where T : class, ICommand
    {
        _appConfiguration = (app) =>
        {
            var defaultCommandBuilder = app.SetDefaultCommand<T>();
            if (description != null)
            {
                defaultCommandBuilder.WithDescription(description);
            }

            if (data != null)
            {
                defaultCommandBuilder.WithData(data);
            }
        };
    }

    /// <summary>
    /// Configures the command application.
    /// </summary>
    /// <param name="action">The configuration action.</param>
    public void Configure(Action<IConfigurator> action)
    {
        if (_configuration != null)
        {
            throw new InvalidOperationException("The command app harnest have already been configured.");
        }

        _configuration = action;
    }

    /// <summary>
    /// Runs the command application and expects an exception of a specific type to be thrown.
    /// </summary>
    /// <typeparam name="T">The expected exception type.</typeparam>
    /// <param name="args">The arguments.</param>
    /// <returns>The information about the failure.</returns>
    public CommandAppFailure RunAndCatch<T>(params string[] args)
        where T : Exception
    {
        try
        {
            RunAsync(args, Console, c => c.PropagateExceptions()).GetAwaiter().GetResult();
            throw new InvalidOperationException("Expected an exception to be thrown, but there was none.");
        }
        catch (T ex)
        {
            if (ex is CommandAppException commandAppException && commandAppException.Pretty != null)
            {
                Console.Write(commandAppException.Pretty);
            }
            else
            {
                Console.WriteLine(ex.Message);
            }

            return new CommandAppFailure(ex, Console.Output);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(
                $"Expected an exception of type '{typeof(T).FullName}' to be thrown, "
                + $"but received {ex.GetType().FullName}.");
        }
    }

    /// <summary>
    /// Runs the command application.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <returns>The result.</returns>
    public CommandAppResult Run(params string[] args)
    {
        return RunAsync(args, Console).GetAwaiter().GetResult();
    }

    /// <summary>
    /// Runs the command application asynchronously.
    /// </summary>
    /// <param name="args">The arguments.</param>
    /// <param name="cancellationToken">The token to monitor for cancellation requests.</param>
    /// <returns>The result.</returns>
    public async Task<CommandAppResult> RunAsync(string[]? args = null, CancellationToken cancellationToken = default)
    {
        return await RunAsync(args ?? [], Console, cancellationToken: cancellationToken);
    }

    private async Task<CommandAppResult> RunAsync(string[] args, TestConsole console, Action<IConfigurator>? config = null, CancellationToken cancellationToken = default)
    {
        CommandContext? context = null;
        CommandSettings? settings = null;

        var app = new CommandApp(Registrar);
        _appConfiguration?.Invoke(app);

        if (_configuration != null)
        {
            app.Configure(_configuration);
        }

        if (config != null)
        {
            app.Configure(config);
        }

        app.Configure(c => c.ConfigureConsole(console));
        app.Configure(c => c.SetInterceptor(new CallbackCommandInterceptor((ctx, s) =>
        {
            context = ctx;
            settings = s;
        })));

        var result = await app.RunAsync(args, cancellationToken);

        var output = console.Output.NormalizeLineEndings();
        output = TestSettings.TrimConsoleOutput ? output.TrimLines().Trim() : output;

        return new CommandAppResult(result, output, context, settings);
    }
}