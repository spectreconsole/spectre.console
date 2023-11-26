namespace Spectre.Console.Cli;

/// <summary>
/// Contains extensions for <see cref="IConfigurator"/>
/// and <see cref="IConfigurator{TSettings}"/>.
/// </summary>
public static class ConfiguratorExtensions
{
    /// <summary>
    /// Sets the help provider for the application.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="helpProvider">The help provider to use.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetHelpProvider(this IConfigurator configurator, IHelpProvider helpProvider)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.SetHelpProvider(helpProvider);
        return configurator;
    }

    /// <summary>
    /// Sets the help provider for the application.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <typeparam name="T">The type of the help provider to instantiate at runtime and use.</typeparam>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetHelpProvider<T>(this IConfigurator configurator)
        where T : IHelpProvider
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.SetHelpProvider<T>();
        return configurator;
    }

    /// <summary>
    /// Sets the culture for the application.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="culture">The culture.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    /// <remarks>
    /// Text displayed by <see cref="Help.HelpProvider"/> can be localised, but defaults to English.
    /// Setting the application culture informs the resource manager which culture to use when fetching strings.
    /// English will be used when a culture has not been specified
    /// or a string has not been localised for the specified culture.
    /// </remarks>
    public static IConfigurator SetApplicationCulture(this IConfigurator configurator, CultureInfo? culture)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.Culture = culture;
        return configurator;
    }

    /// <summary>
    /// Sets the name of the application.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the application.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetApplicationName(this IConfigurator configurator, string name)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.ApplicationName = name;
        return configurator;
    }

    /// <summary>
    /// Overrides the auto-detected version of the application.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="version">The version of application.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetApplicationVersion(this IConfigurator configurator, string version)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.ApplicationVersion = version;
        return configurator;
    }

    /// <summary>
    /// Hides the <c>DEFAULT</c> column that lists default values coming from the
    /// <see cref="DefaultValueAttribute"/> in the options help text.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator HideOptionDefaultValues(this IConfigurator configurator)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.ShowOptionDefaultValues = false;
        return configurator;
    }

    /// <summary>
    /// Configures the console.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="console">The console.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator ConfigureConsole(this IConfigurator configurator, IAnsiConsole console)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.Console = console;
        return configurator;
    }

    /// <summary>
    /// Sets the parsing mode to strict.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator UseStrictParsing(this IConfigurator configurator)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.StrictParsing = true;
        return configurator;
    }

    /// <summary>
    /// Tells the help writer whether or not to trim trailing period.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="trimTrailingPeriods">True to trim trailing period (default), false to not.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator TrimTrailingPeriods(this IConfigurator configurator, bool trimTrailingPeriods)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.TrimTrailingPeriod = trimTrailingPeriods;
        return configurator;
    }

    /// <summary>
    /// Tells the command line application to propagate all
    /// exceptions to the user.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator PropagateExceptions(this IConfigurator configurator)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.PropagateExceptions = true;
        return configurator;
    }

    /// <summary>
    /// Configures case sensitivity.
    /// </summary>
    /// <param name="configurator">The configuration.</param>
    /// <param name="sensitivity">The case sensitivity.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator CaseSensitivity(this IConfigurator configurator, CaseSensitivity sensitivity)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.CaseSensitivity = sensitivity;
        return configurator;
    }

    /// <summary>
    /// Tells the command line application to validate all
    /// examples before running the application.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator ValidateExamples(this IConfigurator configurator)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.ValidateExamples = true;
        return configurator;
    }

    /// <summary>
    /// Sets the command interceptor to be used.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="interceptor">A <see cref="ICommandInterceptor"/>.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetInterceptor(this IConfigurator configurator, ICommandInterceptor interceptor)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.Interceptor = interceptor;
        return configurator;
    }

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="action">The command branch configuration.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    public static IBranchConfigurator AddBranch(
        this IConfigurator configurator,
        string name,
        Action<IConfigurator<CommandSettings>> action)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return configurator.AddBranch(name, action);
    }

    /// <summary>
    /// Adds a command branch.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command branch.</param>
    /// <param name="action">The command branch configuration.</param>
    /// <returns>A branch configurator that can be used to configure the branch further.</returns>
    public static IBranchConfigurator AddBranch<TSettings>(
        this IConfigurator<TSettings> configurator,
        string name,
        Action<IConfigurator<TSettings>> action)
            where TSettings : CommandSettings
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return configurator.AddBranch(name, action);
    }

    /// <summary>
    /// Adds a command without settings that executes a delegate.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    public static ICommandConfigurator AddDelegate(
        this IConfigurator configurator,
        string name,
        Func<CommandContext, int> func)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return configurator.AddDelegate<EmptyCommandSettings>(name, (c, _) => func(c));
    }

    /// <summary>
    /// Adds a command without settings that executes an async delegate.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    public static ICommandConfigurator AddAsyncDelegate(
        this IConfigurator configurator,
        string name,
        Func<CommandContext, Task<int>> func)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return configurator.AddAsyncDelegate<EmptyCommandSettings>(name, (c, _) => func(c));
    }

    /// <summary>
    /// Adds a command without settings that executes a delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    public static ICommandConfigurator AddDelegate<TSettings>(
        this IConfigurator<TSettings> configurator,
        string name,
        Func<CommandContext, int> func)
            where TSettings : CommandSettings
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return configurator.AddDelegate<TSettings>(name, (c, _) => func(c));
    }

    /// <summary>
    /// Adds a command without settings that executes an async delegate.
    /// </summary>
    /// <typeparam name="TSettings">The command setting type.</typeparam>
    /// <param name="configurator">The configurator.</param>
    /// <param name="name">The name of the command.</param>
    /// <param name="func">The delegate to execute as part of command execution.</param>
    /// <returns>A command configurator that can be used to configure the command further.</returns>
    public static ICommandConfigurator AddAsyncDelegate<TSettings>(
        this IConfigurator<TSettings> configurator,
        string name,
        Func<CommandContext, Task<int>> func)
        where TSettings : CommandSettings
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        return configurator.AddAsyncDelegate<TSettings>(name, (c, _) => func(c));
    }

    /// <summary>
    /// Sets the ExceptionsHandler.
    /// <para>Setting <see cref="ICommandAppSettings.ExceptionHandler"/> this way will use the
    /// default exit code of -1.</para>
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="exceptionHandler">The Action that handles the exception.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetExceptionHandler(this IConfigurator configurator, Action<Exception, ITypeResolver?> exceptionHandler)
    {
        return configurator.SetExceptionHandler((ex, resolver) =>
        {
            exceptionHandler(ex, resolver);
            return -1;
        });
    }

    /// <summary>
    /// Sets the ExceptionsHandler.
    /// </summary>
    /// <param name="configurator">The configurator.</param>
    /// <param name="exceptionHandler">The Action that handles the exception.</param>
    /// <returns>A configurator that can be used to configure the application further.</returns>
    public static IConfigurator SetExceptionHandler(this IConfigurator configurator, Func<Exception, ITypeResolver?, int>? exceptionHandler)
    {
        if (configurator == null)
        {
            throw new ArgumentNullException(nameof(configurator));
        }

        configurator.Settings.ExceptionHandler = exceptionHandler;
        return configurator;
    }
}