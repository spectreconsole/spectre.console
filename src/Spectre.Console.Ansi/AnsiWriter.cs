namespace Spectre.Console;

/// <summary>
/// Represents an ANSI writer, capable of outputting ANSI/VT escape sequences.
/// </summary>
public sealed class AnsiWriter
{
    private readonly TextWriter _output;
    private readonly List<byte> _codes;
    private readonly List<byte> _styleBuffer;
    private int _linkCount;

    /// <summary>
    /// Gets or sets the capabilities for the writer.
    /// </summary>
    public IReadOnlyAnsiCapabilities Capabilities { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiWriter"/> class.
    /// </summary>
    /// <param name="output">The <see cref="TextWriter"/> to write to.</param>
    public AnsiWriter(TextWriter output)
    {
        _output = output ?? throw new ArgumentNullException(nameof(output));
        _codes = [];
        _styleBuffer = [];

        Capabilities = AnsiCapabilities.Create(_output);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="AnsiWriter"/> class.
    /// </summary>
    /// <param name="output">The <see cref="TextWriter"/> to write to.</param>
    /// <param name="capabilities">The capabilities.</param>
    public AnsiWriter(TextWriter output, AnsiCapabilities capabilities)
    {
        _output = output ?? throw new ArgumentNullException(nameof(output));
        _codes = [];
        _styleBuffer = [];

        Capabilities = capabilities ?? throw new ArgumentNullException(nameof(capabilities));
    }

    /// <summary>
    /// Writes the specified text.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Write(string text)
    {
        _output.Write(text);
        return this;
    }

    /// <summary>
    /// Writes an integer.
    /// </summary>
    /// <param name="value">The integer.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Write(int value)
    {
        _output.Write(value);
        return this;
    }

    /// <summary>
    /// Writes the specified text with the specified style.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Write(string text, Style style)
    {
        var shouldClose = false;

        if (Capabilities.Ansi)
        {
            if (style.HasLink)
            {
                var link =
                    style.Link.Equals(Constants.EmptyLink)
                        ? text
                        : style.Link;

                BeginLink(link, style.LinkId);
            }

            _styleBuffer.Clear();
            _styleBuffer.AddRange(AnsiCodeBuilder.Build(style.Decoration));
            _styleBuffer.AddRange(AnsiCodeBuilder.Build(Capabilities.ColorSystem, style.Foreground, true));
            _styleBuffer.AddRange(AnsiCodeBuilder.Build(Capabilities.ColorSystem, style.Background, false));

            shouldClose = WriteSgr(_styleBuffer);
        }

        _output.Write(text);

        if (Capabilities.Ansi)
        {
            if (shouldClose)
            {
                WriteSgr(0);
            }

            if (style.HasLink)
            {
                EndLink();
            }
        }

        return this;
    }

    /// <summary>
    /// Writes an empty line.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter WriteLine()
    {
        _output.Write(Environment.NewLine);
        return this;
    }

    /// <summary>
    /// Writes the specified text, followed by the current line terminator.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter WriteLine(string text)
    {
        _output.Write(text);
        WriteLine();

        return this;
    }

    /// <summary>
    /// Writes the specified text with the specified style, followed by the current line terminator.
    /// </summary>
    /// <param name="text">The text.</param>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter WriteLine(string text, Style style)
    {
        Write(text, style);
        WriteLine();

        return this;
    }

    /// <summary>
    /// Writes a <see cref="Style"/> by emitting <c>SGR</c>.
    /// </summary>
    /// <param name="style">The style.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Style(Style style)
    {
        if (Capabilities.Ansi)
        {
            if (style.HasLink)
            {
                BeginLink(style.Link, style.LinkId);
            }

            _codes.Clear();
            _codes.AddRange(AnsiCodeBuilder.Build(style.Decoration));
            _codes.AddRange(AnsiCodeBuilder.Build(Capabilities.ColorSystem, style.Foreground, true));
            _codes.AddRange(AnsiCodeBuilder.Build(Capabilities.ColorSystem, style.Background, false));

            WriteSgr(_codes);
        }

        return this;
    }

    /// <summary>
    /// Resets any foreground, background, decoration, or style by emitting <c>SGR(0)</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#SGR"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ResetStyle()
    {
        if (Capabilities.Ansi)
        {
            WriteSgr(0);
            EndLink();
        }

        return this;
    }

    /// <summary>
    /// Sets the current decoration by emitting <c>SGR</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#SGR"/>.
    /// </remarks>
    /// <param name="decoration">The decoration.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Decoration(Decoration decoration)
    {
        if (Capabilities.Ansi)
        {
            _codes.Clear();
            _codes.AddRange(AnsiCodeBuilder.Build(decoration));

            WriteSgr(_codes);
        }

        return this;
    }

    /// <summary>
    /// Sets the current background color by emitting <c>SGR</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#SGR"/>.
    /// </remarks>
    /// <param name="color">The background color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Background(Color color)
    {
        if (Capabilities.Ansi)
        {
            _codes.Clear();
            _codes.AddRange(AnsiCodeBuilder.Build(Capabilities.ColorSystem, color, false));

            WriteSgr(_codes);
        }

        return this;
    }

    /// <summary>
    /// Sets the current foreground color by emitting <c>SGR</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#SGR"/>.
    /// </remarks>
    /// <param name="color">The foreground color.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Foreground(Color color)
    {
        if (Capabilities.Ansi)
        {
            _codes.Clear();
            _codes.AddRange(AnsiCodeBuilder.Build(Capabilities.ColorSystem, color, true));

            WriteSgr(_codes);
        }

        return this;
    }

    /// <summary>
    /// Begins a link by emitting <c>OSC 8</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://gist.github.com/egmontkob/eb114294efbcd5adb1944c9f3cb5feda"/>.
    /// </remarks>
    /// <param name="link">The link.</param>
    /// <param name="linkId">The link ID.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter BeginLink(string link, int? linkId = null)
    {
        if (Capabilities is { Ansi: true, Links: true })
        {
            _linkCount++;

            WriteOsc(
                linkId != null
                    ? $"8;id={linkId};{link}\e\\"
                    : $"8;{link}\e\\");
        }

        return this;
    }

    /// <summary>
    /// Ends a link by emitting <c>OSC 8</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://gist.github.com/egmontkob/eb114294efbcd5adb1944c9f3cb5feda"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter EndLink()
    {
        if (Capabilities is { Ansi: true, Links: true } && _linkCount > 0)
        {
            _linkCount--;
            WriteOsc("8;;\e\\");
        }

        return this;
    }

    /// <summary>
    /// This control function moves the cursor to the specified line and column (1-indexed)
    /// by emitting <c>CSI row;column H</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#CUP"/>.
    /// </remarks>
    /// <param name="row">The row.</param>
    /// <param name="column">The column.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorPosition(int row, int column)
    {
        WriteCsi($"{row};{column}", 'H');
        return this;
    }

    /// <summary>
    /// Moves the cursor to position 1,1 (top left corner) by emitting <c>CSI H</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#CUP"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorHome()
    {
        WriteCsi("H");
        return this;
    }

    /// <summary>
    /// Moves the cursor up a specified number of lines in the same column by emitting <c>CSI n A</c>.
    /// The cursor stops at the top margin.
    /// If the cursor is already above the top margin, then the cursor stops at the top line.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#CUU"/>.
    /// </remarks>
    /// <param name="steps">The number of steps to move the cursor up.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorUp(int steps)
    {
        WriteCsi(steps, 'A');
        return this;
    }

    /// <summary>
    /// This control function moves the cursor down a specified number of lines in the same column
    /// by emitting <c>CSI n B</c>.
    /// The cursor stops at the bottom margin.
    /// If the cursor is already below the bottom margin, then the cursor stops at the bottom line.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#CUD"/>.
    /// </remarks>
    /// <param name="steps">The number of steps to move the cursor down.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorDown(int steps)
    {
        WriteCsi(steps, 'B');
        return this;
    }

    /// <summary>
    /// This control function moves the cursor to the right by a specified number of columns
    /// by emitting <c>CSI n C</c>.
    /// The cursor stops at the right border of the page.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#CUF"/>.
    /// </remarks>
    /// <param name="steps">The number of steps to move the cursor right.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorRight(int steps)
    {
        WriteCsi(steps, 'C');
        return this;
    }

    /// <summary>
    /// This control function moves the cursor to the left by a specified number of columns
    /// by emitting <c>CSI n D</c>.
    /// The cursor stops at the left border of the page.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#CUB"/>.
    /// </remarks>
    /// <param name="steps">The number of steps to move the cursor left.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorLeft(int steps)
    {
        WriteCsi(steps, 'D');
        return this;
    }

    /// <summary>
    /// Shows the cursor by emitting <c>CSI ? 25 h</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#SM"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ShowCursor()
    {
        WriteCsi(25, 'h', decPrivateMode: true);
        return this;
    }

    /// <summary>
    /// Hides the cursor by emitting <c>CSI ? 25 l</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#RM"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter HideCursor()
    {
        WriteCsi(25, 'l', decPrivateMode: true);
        return this;
    }

    /// <summary>
    /// Saves current cursor position for SCO console mode
    /// by emitting <c>CSI s</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/SCOSC.html"/>.
    /// </remarks>
    /// <returns></returns>
    public AnsiWriter SaveCursor()
    {
        WriteCsi("s");
        return this;
    }

    /// <summary>
    /// Moves cursor to the position saved by save cursor command in SCO console mode
    /// by emitting <c>CSI u</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/SCORC.html"/>.
    /// </remarks>
    /// <returns></returns>
    public AnsiWriter RestoreCursor()
    {
        WriteCsi("u");
        return this;
    }

    /// <summary>
    /// Moves the active position to the n-th character of the active line
    /// by emitting <c>CSI n G</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/CHA.html"/>.
    /// </remarks>
    /// <param name="position">The horizontal position.</param>
    public AnsiWriter CursorHorizontalAbsolute(int position)
    {
        WriteCsi(position, 'G');
        return this;
    }

    /// <summary>
    /// Enters the alternative screen buffer by emitting <c>CSI ? 1049 h</c>.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter EnterAltScreen()
    {
        WriteCsi(1049, 'h', decPrivateMode: true);
        return this;
    }

    /// <summary>
    /// Exits the alternative screen buffer by emitting <c>CSI ? 1049 l</c>.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ExitAltScreen()
    {
        WriteCsi(1049, 'l', decPrivateMode: true);
        return this;
    }

    /// <summary>
    /// This control function erases characters on the line that has the cursor.
    /// EL clears all character attributes from erased character positions.
    /// EL works inside or outside the scrolling margins.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#EL"/>.
    /// </remarks>
    /// <param name="mode">
    /// The section of the line to erase.
    /// <list type="bullet|number|table">
    ///     <item>
    ///         <term>0</term>
    ///         <description>From the cursor through the end of the line.</description>
    ///     </item>
    ///     <item>
    ///         <term>1</term>
    ///         <description>From the beginning of the line through the cursor.</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>The complete line.</description>
    ///     </item>
    /// </list>
    /// </param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter EraseInLine(int mode = 0)
    {
        WriteCsi(mode, 'K');
        return this;
    }

    /// <summary>
    /// This control function erases characters from part or all of the display.
    /// When you erase complete lines, they become single-height, single-width lines,
    /// with all visual character attributes cleared.
    /// ED works inside or outside the scrolling margins.
    /// </summary>
    /// <param name="mode">
    /// The amount of the display to erase.
    /// <list type="bullet|number|table">
    ///     <item>
    ///         <term>0</term>
    ///         <description>From the cursor through the end of the display.</description>
    ///     </item>
    ///     <item>
    ///         <term>1</term>
    ///         <description>From the beginning of the display through the cursor.</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>The complete display.</description>
    ///     </item>
    /// </list>
    /// </param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter EraseInDisplay(int mode = 0)
    {
        WriteCsi(mode, 'J');
        return this;
    }

    /// <summary>
    /// Clears the scrollback buffer by emitting <c>CSI 3J</c>.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ClearScrollback()
    {
        WriteCsi(3, 'J');
        return this;
    }

    private bool WriteSgr(params List<byte> codes)
    {
        if (!Capabilities.Ansi || codes.Count == 0)
        {
            return false;
        }

        WriteCsi(string.Join(";", codes), 'm');
        return true;
    }

    private bool WriteCsi(int value, char terminator, bool decPrivateMode = false)
    {
        return WriteCsi($"{value}{terminator}", decPrivateMode);
    }

    private bool WriteCsi(string parameters, char terminator, bool decPrivateMode = false)
    {
        return WriteCsi($"{parameters}{terminator}");
    }

    private bool WriteCsi(string parameters, bool decPrivateMode = false)
    {
        if (!Capabilities.Ansi)
        {
            return false;
        }

        Write(decPrivateMode ? $"\e[?{parameters}" : $"\e[{parameters}");
        return true;
    }

    private bool WriteOsc(string parameters)
    {
        if (!Capabilities.Ansi)
        {
            return false;
        }

        Write($"\e]{parameters}");
        return true;
    }
}

file static class AnsiCodeBuilder
{
    public static IEnumerable<byte> Build(Decoration decoration)
    {
        if ((decoration & Decoration.Bold) != 0)
        {
            yield return 1;
        }

        if ((decoration & Decoration.Dim) != 0)
        {
            yield return 2;
        }

        if ((decoration & Decoration.Italic) != 0)
        {
            yield return 3;
        }

        if ((decoration & Decoration.Underline) != 0)
        {
            yield return 4;
        }

        if ((decoration & Decoration.SlowBlink) != 0)
        {
            yield return 5;
        }

        if ((decoration & Decoration.RapidBlink) != 0)
        {
            yield return 6;
        }

        if ((decoration & Decoration.Invert) != 0)
        {
            yield return 7;
        }

        if ((decoration & Decoration.Conceal) != 0)
        {
            yield return 8;
        }

        if ((decoration & Decoration.Strikethrough) != 0)
        {
            yield return 9;
        }
    }

    public static IEnumerable<byte> Build(ColorSystem system, Color color, bool foreground)
    {
        if (color == Color.Default)
        {
            return [];
        }

        return system switch
        {
            ColorSystem.NoColors => [], // No colors
            ColorSystem.TrueColor => GetTrueColor(color, foreground), // 24-bit
            ColorSystem.EightBit => GetEightBit(color, foreground), // 8-bit
            ColorSystem.Standard => GetFourBit(color, foreground), // 4-bit
            ColorSystem.Legacy => GetThreeBit(color, foreground), // 3-bit
            _ => throw new InvalidOperationException("Could not determine ANSI color."),
        };
    }

    private static IEnumerable<byte> GetThreeBit(Color color, bool foreground)
    {
        var number = color.Number;
        if (number == null || color.Number >= 8)
        {
            number = color.ExactOrClosest(ColorSystem.Legacy).Number;
        }

        Debug.Assert(number is >= 0 and < 8, "Invalid range for 4-bit color");

        var mod = foreground ? 30 : 40;
        return [(byte)(number.Value + mod)];
    }

    private static IEnumerable<byte> GetFourBit(Color color, bool foreground)
    {
        var number = color.Number;
        if (number == null || color.Number >= 16)
        {
            number = color.ExactOrClosest(ColorSystem.Standard).Number;
        }

        Debug.Assert(number is >= 0 and < 16, "Invalid range for 4-bit color");

        var mod = number < 8 ? (foreground ? 30 : 40) : (foreground ? 82 : 92);
        return [(byte)(number.Value + mod)];
    }

    private static IEnumerable<byte> GetEightBit(Color color, bool foreground)
    {
        var number = color.Number ?? color.ExactOrClosest(ColorSystem.EightBit).Number;
        Debug.Assert(number is >= 0, "Invalid range for 8-bit color");

        var mod = foreground ? (byte)38 : (byte)48;
        return [mod, 5, (byte)number];
    }

    private static IEnumerable<byte> GetTrueColor(Color color, bool foreground)
    {
        if (color.Number != null)
        {
            return GetEightBit(color, foreground);
        }

        var mod = foreground ? (byte)38 : (byte)48;
        return [mod, 2, color.R, color.G, color.B];
    }
}