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
    /// <param name="link">The link.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Write(string text, Style style, Link? link = null)
    {
        var shouldClose = false;

        if (Capabilities.Ansi)
        {
            if (link != null)
            {
                var url = link.Url.Equals(Constants.EmptyLink) ? text : link.Url;
                BeginLink(url, link.Id);
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

            if (link != null)
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
    /// <param name="link">The link.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter WriteLine(string text, Style style, Link? link = null)
    {
        Write(text, style, link);
        WriteLine();

        return this;
    }

    /// <summary>
    /// Writes a <see cref="Style"/> by emitting <c>SGR</c>.
    /// </summary>
    /// <param name="style">The style.</param>
    /// <param name="link">The link.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Style(Style style, Link? link = null)
    {
        if (Capabilities.Ansi)
        {
            if (link != null)
            {
                BeginLink(link);
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
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter BeginLink(Link link)
    {
        ArgumentNullException.ThrowIfNull(link);
        return BeginLink(link.Url, link.Id);
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
        ArgumentNullException.ThrowIfNull(link);

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
        return WriteCsi($"{row};{column}", 'H');
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
        return WriteCsi("H");
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
        return WriteCsi(steps, 'A');
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
        return WriteCsi(steps, 'B');
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
        return CursorForward(steps);
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
    public AnsiWriter CursorForward(int steps)
    {
        return WriteCsi(steps, 'C');
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
        return CursorBackward(steps);
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
    public AnsiWriter CursorBackward(int steps)
    {
        return WriteCsi(steps, 'D');
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
        return WriteCsi(25, 'h', decPrivateMode: true);
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
        return WriteCsi(25, 'l', decPrivateMode: true);
    }

    /// <summary>
    /// Saves current cursor position for SCO console mode by emitting <c>CSI s</c> (SCOSC)
    /// if staying on page, otherwise <c>ESC 7</c> (DECSC).
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/SCOSC.html"/> and
    /// <see href="https://vt100.net/docs/vt510-rm/DECSC.html"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter SaveCursor(bool stayOnPage = true)
    {
        if (stayOnPage)
        {
            // SCOSC
            return WriteCsi("s");
        }

        // DECSC
        return WriteEsc("7");
    }

    /// <summary>
    /// Moves cursor to the position saved by save cursor command in SCO console mode
    /// by emitting <c>CSI u</c> (SCORC) if staying on page, otherwise <c>ESC 8</c> (DECRC).
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/SCORC.html"/> and
    /// <see href="https://vt100.net/docs/vt510-rm/DECRC.html"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter RestoreCursor(bool stayOnPage = true)
    {
        if (stayOnPage)
        {
            // SCORC
            return WriteCsi("u");
        }

        // DECRC
        return WriteEsc("8");
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
        return WriteCsi(position, 'G');
    }

    /// <summary>
    /// Enters the alternative screen buffer by emitting <c>CSI ? 1049 h</c>.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter EnterAltScreen()
    {
        return WriteCsi(1049, 'h', decPrivateMode: true);
    }

    /// <summary>
    /// Exits the alternative screen buffer by emitting <c>CSI ? 1049 l</c>.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ExitAltScreen()
    {
        return WriteCsi(1049, 'l', decPrivateMode: true);
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
        return WriteCsi(mode, 'K');
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
        return WriteCsi(mode, 'J');
    }

    /// <summary>
    /// Clears the scrollback buffer by emitting <c>CSI 3J</c>.
    /// </summary>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ClearScrollback()
    {
        return EraseInDisplay(3);
    }

    /// <summary>
    /// Move the active position n tabs backward
    /// by emitting <c>CSI n Z</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/CBT.html"/>.
    /// </remarks>
    /// <param name="tabs">The number of tabs to move backwards</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorBackwardTabulation(int tabs = 1)
    {
        return WriteCsi(tabs, 'Z');
    }

    /// <summary>
    /// Move the active position n tabs forward
    /// by emitting <c>CSI n I</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/CHT.html"/>.
    /// </remarks>
    /// <param name="tabs">The number of tabs to move forward</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorHorizontalTabulation(int tabs = 1)
    {
        return WriteCsi(tabs, 'I');
    }

    /// <summary>
    /// Move the cursor to the next line
    /// by emitting <c>CSI n E</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/CNL.html"/>.
    /// </remarks>
    /// <param name="lines">The number of lines to move</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorNextLine(int lines = 1)
    {
        return WriteCsi(lines, 'E');
    }

    /// <summary>
    /// Move the cursor to the preceding line
    /// by emitting <c>CSI n F</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/CPL.html"/>.
    /// </remarks>
    /// <param name="lines">The number of lines to move</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter CursorPreviousLine(int lines = 1)
    {
        return WriteCsi(lines, 'F');
    }

    /// <summary>
    /// Moves the cursor down one line in the same column by emitting <c>ESC D</c>.
    /// If the cursor is at the bottom margin, then the screen performs a scroll-up.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/IND.html"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter Index()
    {
        return WriteEsc("D");
    }

    /// <summary>
    /// Moves the cursor up one line in the same column by emitting <c>ESC M</c>.
    /// If the cursor is at the top margin, then the screen performs a scroll-down.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt100-ug/chapter3.html#RI"/>.
    /// </remarks>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ReverseIndex()
    {
        return WriteEsc("M");
    }

    /// <summary>
    /// This control function deletes one or more characters from the cursor position to the right
    /// by emitting <c>CSI n P</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/DCH.html"/>.
    /// </remarks>
    /// <param name="characters">
    /// The number of characters to delete. If <paramref name="characters"/> is greater than the number of characters between the cursor and the right margin, then DCH only deletes the remaining characters.
    /// </param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter DeleteCharacter(int characters = 1)
    {
        return WriteCsi(characters, 'P');
    }

    /// <summary>
    /// Select the style of the cursor on the screen by emitting <c>CSI n SP q</c>
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/DECSCUSR.html"/>.
    /// </remarks>
    /// <param name="style">
    /// The style of the cursor
    /// <list type="bullet|number|table">
    ///     <item>
    ///         <term>0</term>
    ///         <description>Terminal default.</description>
    ///     </item>
    ///     <item>
    ///         <term>1</term>
    ///         <description>Blinking block.</description>
    ///     </item>
    ///     <item>
    ///         <term>2</term>
    ///         <description>Steady block.</description>
    ///     </item>
    ///     <item>
    ///         <term>3</term>
    ///         <description>Blinking underline.</description>
    ///     </item>
    ///     <item>
    ///         <term>5</term>
    ///         <description>Steady underline.</description>
    ///     </item>
    ///     <item>
    ///         <term>6</term>
    ///         <description>Steady vertical bar.</description>
    ///     </item>
    /// </list>
    /// </param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter SetCursorStyle(int style = 0)
    {
        return WriteCsi($"{style} ", 'q');
    }

    /// <summary>
    /// This control function deletes one or more lines in the scrolling region
    /// by emitting <c>CSI n M</c>, starting with the line that has the cursor.
    /// As lines are deleted, lines below the cursor and in the scrolling region move up.
    /// The terminal adds blank lines with no visual character attributes at the bottom of the scrolling region.
    /// If <c>lines</c> is greater than the number of lines remaining on the page, DL deletes only the remaining lines.
    /// DL has no effect outside the scrolling margins.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/DL.html"/>.
    /// </remarks>
    /// <param name="lines">The number of lines to delete.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter DeleteLine(int lines = 0)
    {
        return WriteCsi(lines, 'M');
    }

    /// <summary>
    /// Erases one or more characters from the cursor position to the right by emitting <c>CSI n X</c> (ECH).
    /// ECH clears character attributes from erased character positions.
    /// ECH works inside or outside the scrolling margins.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/ECH.html"/>.
    /// </remarks>
    /// <param name="characters">The number of characters to erase. A value of 0 or 1 erases one character.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter EraseCharacter(int characters = 1)
    {
        return WriteCsi(characters, 'X');
    }

    /// <summary>
    /// inserts one or more space (SP) characters starting at the cursor position
    /// by emitting <c>CSI n @</c> (ICH).
    /// The ICH sequence inserts blank characters with the normal character attribute.
    /// The cursor remains at the beginning of the blank characters.
    /// Text between the cursor and right margin moves to the right.
    /// Characters scrolled past the right margin are lost. ICH has no effect outside the scrolling margins.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/ICH.html"/>.
    /// </remarks>
    /// <param name="characters">The number of characters to insert.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter InsertCharacter(int characters = 1)
    {
        return WriteCsi(characters, '@');
    }

    /// <summary>
    /// Inserts one or more blank lines, starting at the cursor by emitting <c>CSI n L</c> (IL).
    /// As lines are inserted, lines below the cursor and in the scrolling region move down.
    /// Lines scrolled off the page are lost. IL has no effect outside the page margins.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/IL.html"/>.
    /// </remarks>
    /// <param name="lines">The number of lines to insert.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter InsertLine(int lines = 1)
    {
        return WriteCsi(lines, 'L');
    }

    /// <summary>
    /// Moves the user window up a specified number of lines in page memory
    /// by emitting <c>CSI n T</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/SD.html"/>.
    /// </remarks>
    /// <param name="lines">
    /// The number of lines to move the user window up in page memory.
    /// <c>lines</c> new lines appear at the top of the display.
    /// <c>lines</c> old lines disappear at the bottom of the display.
    /// You cannot pan past the top margin of the current page.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ScrollDown(int lines = 1)
    {
        return WriteCsi(lines, 'T');
    }

    /// <summary>
    /// Moves the user window down a specified number of lines in page memory
    /// by emitting <c>CSI n S</c>.
    /// </summary>
    /// <remarks>
    /// See <see href="https://vt100.net/docs/vt510-rm/SU.html"/>.
    /// </remarks>
    /// <param name="lines">
    /// The number of lines to move the user window down in page memory.
    /// <c>lines</c> new lines appear at the bottom of the display.
    /// <c>lines</c> old lines disappear at the top of the display.
    /// You cannot pan past the bottom margin of the current page.</param>
    /// <returns>The same instance so that multiple calls can be chained.</returns>
    public AnsiWriter ScrollUp(int lines = 1)
    {
        return WriteCsi(lines, 'S');
    }

    private AnsiWriter WriteCsi(int value, char terminator, bool decPrivateMode = false)
    {
        return WriteCsi($"{value}{terminator}", decPrivateMode);
    }

    private AnsiWriter WriteCsi(string parameters, char terminator, bool decPrivateMode = false)
    {
        return WriteCsi($"{parameters}{terminator}", decPrivateMode);
    }

    private AnsiWriter WriteCsi(string parameters, bool decPrivateMode = false)
    {
        if (Capabilities.Ansi)
        {
            Write(decPrivateMode ? $"\e[?{parameters}" : $"\e[{parameters}");
        }

        return this;
    }

    private AnsiWriter WriteOsc(string parameters)
    {
        if (Capabilities.Ansi)
        {
            Write($"\e]{parameters}");
        }

        return this;
    }

    private AnsiWriter WriteEsc(string value)
    {
        return Write($"\e{value}");
    }

    private bool WriteSgr(params List<byte> codes)
    {
        if (!Capabilities.Ansi)
        {
            return false;
        }

        var parameters = string.Join(";", codes);
        if (string.IsNullOrWhiteSpace(parameters))
        {
            return false;
        }

        WriteCsi(parameters, 'm');
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