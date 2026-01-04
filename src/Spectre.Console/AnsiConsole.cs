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