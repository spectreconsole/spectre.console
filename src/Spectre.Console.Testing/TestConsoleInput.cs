namespace Spectre.Console.Testing;

/// <summary>
/// Represents a testable console input mechanism.
/// </summary>
public sealed class TestConsoleInput : IAnsiConsoleInput
{
    private readonly Queue<ConsoleKeyInfo> _input;

    /// <summary>
    /// Initializes a new instance of the <see cref="TestConsoleInput"/> class.
    /// </summary>
    public TestConsoleInput()
    {
        _input = new Queue<ConsoleKeyInfo>();
    }

    /// <summary>
    /// Pushes the specified text to the input queue.
    /// </summary>
    /// <param name="input">The input string.</param>
    public void PushText(string input)
    {
        ArgumentNullException.ThrowIfNull(input);

        foreach (var character in input)
        {
            PushCharacter(character);
        }
    }

    /// <summary>
    /// Pushes the specified text followed by 'Enter' to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushTextWithEnter(string input)
    {
        PushText(input);
        PushKey(ConsoleKey.Enter);
    }

    /// <summary>
    /// Pushes the specified character to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushCharacter(char input)
    {
        var control = char.IsUpper(input);
        _input.Enqueue(new ConsoleKeyInfo(input, (ConsoleKey)input, false, false, control));
    }

    /// <summary>
    /// Pushes the specified key to the input queue.
    /// </summary>
    /// <param name="input">The input.</param>
    public void PushKey(ConsoleKey input)
    {
        _input.Enqueue(new ConsoleKeyInfo(GetKeyChar(input), input, false, false, false));
    }

    /// <summary>
    /// Pushes the specified key to the input queue.
    /// </summary>
    /// <param name="consoleKeyInfo">The input.</param>
    public void PushKey(ConsoleKeyInfo consoleKeyInfo)
    {
        _input.Enqueue(consoleKeyInfo);
    }

    /// <inheritdoc/>
    public bool IsKeyAvailable()
    {
        return _input.Count > 0;
    }

    /// <inheritdoc/>
    public ConsoleKeyInfo? ReadKey(bool intercept)
    {
        if (_input.Count == 0)
        {
            throw new InvalidOperationException("No input available.");
        }

        return _input.Dequeue();
    }

    /// <inheritdoc/>
    public Task<ConsoleKeyInfo?> ReadKeyAsync(bool intercept, CancellationToken cancellationToken)
    {
        return Task.FromResult(ReadKey(intercept));
    }

    private static char GetKeyChar(ConsoleKey key)
    {
        // Navigation and function keys don't produce a character,
        // so casting them to char would result in an unrelated one
        // (e.g. UpArrow would become '&').
        if (key is >= ConsoleKey.PageUp and <= ConsoleKey.Help
            or >= ConsoleKey.F1 and <= ConsoleKey.F24)
        {
            return '\0';
        }

        return (char)key;
    }
}