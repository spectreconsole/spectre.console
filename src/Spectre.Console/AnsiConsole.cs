namespace Spectre.Console;

/// <summary>
/// A console capable of writing ANSI escape sequences.
/// </summary>
public static partial class AnsiConsole
{
#pragma warning disable CS0618 // 'AnsiConsoleFactory' is obsolete
    private static readonly AnsiConsoleFactory _factory = new AnsiConsoleFactory();
#pragma warning restore CS0618

    internal static Style CurrentStyle { get; set; } = Style.Plain;
    internal static Recorder? Recorder { get; set; }

    internal static bool Created { get; private set; }

    private static Lazy<IAnsiConsole> _console = new Lazy<IAnsiConsole>(
        () =>
        {
            var console = Create(new AnsiConsoleSettings
            {
                Ansi = AnsiSupport.Detect,
                ColorSystem = ColorSystemSupport.Detect,
                Out = new AnsiConsoleOutput(System.Console.Out),
            });

            Created = true;
            return console;
        });

    /// <summary>
    /// Gets or sets the underlying <see cref="IAnsiConsole"/>.
    /// </summary>
    public static IAnsiConsole Console
    {
        get
        {
            return Recorder ?? _console.Value;
        }
        set
        {
            _console = new Lazy<IAnsiConsole>(() => value);
            Recorder = Recorder?.Clone(value); // Recreate the recorder
            Created = true;
        }
    }

    /// <summary>
    /// Gets the <see cref="IAnsiConsoleCursor"/>.
    /// </summary>
    public static IAnsiConsoleCursor Cursor => Recorder?.Cursor ?? _console.Value.Cursor;

    /// <summary>
    /// Gets the console profile.
    /// </summary>
    public static Profile Profile => Console.Profile;

    /// <summary>
    /// Creates a new <see cref="IAnsiConsole"/> instance
    /// from the provided settings.
    /// </summary>
    /// <param name="settings">The settings to use.</param>
    /// <returns>An <see cref="IAnsiConsole"/> instance.</returns>
    public static IAnsiConsole Create(AnsiConsoleSettings settings)
    {
        return _factory.Create(settings);
    }
}

// TODO: This is here temporary due to a bug in the .NET SDK
// See issue: https://github.com/dotnet/roslyn/issues/80024
public static partial class AnsiConsole
{
    /// <summary>
    /// Writes the text representation of the specified array of objects,
    /// to the console using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void Write(string format, params object[] args)
    {
        Write(CultureInfo.CurrentCulture, format, args);
    }

    /// <summary>
    /// Writes the text representation of the specified array of objects,
    /// to the console using the specified format information.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void Write(IFormatProvider provider, string format, params object[] args)
    {
        Console.Write(string.Format(provider, format, args), CurrentStyle);
    }

    /// <summary>
    /// Writes the text representation of the specified array of objects,
    /// followed by the current line terminator, to the console
    /// using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void WriteLine(string format, params object[] args)
    {
        WriteLine(CultureInfo.CurrentCulture, format, args);
    }

    /// <summary>
    /// Writes the text representation of the specified array of objects,
    /// followed by the current line terminator, to the console
    /// using the specified format information.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void WriteLine(IFormatProvider provider, string format, params object[] args)
    {
        Console.WriteLine(string.Format(provider, format, args), CurrentStyle);
    }

    /// <summary>
    /// Writes the specified markup to the console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void Markup(string format, params object[] args)
    {
        Console.Markup(format, args);
    }

    /// <summary>
    /// Writes the specified markup to the console.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void Markup(IFormatProvider provider, string format, params object[] args)
    {
        Console.Markup(provider, format, args);
    }

    /// <summary>
    /// Writes the specified markup, followed by the current line terminator, to the console.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void MarkupLine(string format, params object[] args)
    {
        Console.MarkupLine(format, args);
    }

    /// <summary>
    /// Writes the specified markup, followed by the current line terminator, to the console.
    /// </summary>
    /// <param name="provider">An object that supplies culture-specific formatting information.</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write.</param>
    public static void MarkupLine(IFormatProvider provider, string format, params object[] args)
    {
        Console.MarkupLine(provider, format, args);
    }
}