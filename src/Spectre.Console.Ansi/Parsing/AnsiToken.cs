namespace Spectre.Console.Ansi;

/// <summary>
/// Represents a parsed ANSI/VT token.
/// </summary>
[Experimental(Constants.ExperimentalParserId)]
public abstract record AnsiToken
{
    // Only the nested token types derive from this
    private AnsiToken()
    {
    }

    // A record prints a list as its type name, so print the items instead
    private static string FormatList<T>(IEnumerable<T> items)
    {
        return "[" + string.Join(", ", items) + "]";
    }

    /// <summary>
    /// Prints a Unicode scalar value (codepoint) to the screen.
    /// </summary>
    /// <param name="Codepoint">
    /// The Unicode codepoint to print. Astral codepoints, encoded as UTF-16
    /// surrogate pairs in the input, are combined into a single scalar value.
    /// </param>
    public sealed record Print(int Codepoint) : AnsiToken
    {
        /// <summary>
        /// Converts the <see cref="Codepoint"/> to its UTF-16 representation, suitable
        /// for writing to a <see cref="System.IO.TextWriter"/> or a <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// A string of one or two <see cref="char"/> values; an astral codepoint is
        /// returned as a UTF-16 surrogate pair.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <see cref="Codepoint"/> isn't a valid Unicode scalar value. Tokens from the parser
        /// always hold a valid one.
        /// </exception>
        public string ToUtf16()
        {
            return char.ConvertFromUtf32(Codepoint);
        }
    }

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <param name="Function">
    /// The C0/C1 function to execute. An ESC (0x1B) that doesn't start a sequence, such as a
    /// single press of the Escape key, is reported this way too, and so is DEL (0x7F), the
    /// Backspace key, in <see cref="AnsiParserMode.Input"/>.
    /// </param>
    public sealed record Execute(char Function) : AnsiToken;

    /// <summary>
    /// Execute an ESC command.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Final">
    /// The final character identifying the ESC command. A key pressed with Alt arrives as ESC
    /// and the key, so in terminal input this can be any character, such as ä for Alt+ä, or
    /// CR for Alt+Enter in <see cref="AnsiParserMode.Input"/>.
    /// </param>
    public sealed record Esc(IReadOnlyList<char> Intermediates, char Final) : AnsiToken
    {
        /// <inheritdoc/>
        public bool Equals(Esc? other)
        {
            return other is not null
                && base.Equals(other)
                && Intermediates.SequenceEqual(other.Intermediates)
                && Final == other.Final;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();
                hash = (hash * 397) ^ Final.GetHashCode();
                foreach (var intermediate in Intermediates)
                {
                    hash = (hash * 397) ^ intermediate.GetHashCode();
                }

                return hash;
            }
        }

        /// <inheritdoc/>
        protected override bool PrintMembers(StringBuilder builder)
        {
            builder.Append("Intermediates = ").Append(FormatList(Intermediates))
                .Append(", Final = ").Append(Final);

            return true;
        }
    }

    /// <summary>
    /// Executes a CSI command.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Params">The parameters, each with its colon-separated subparameters.</param>
    /// <param name="Final">The final byte identifying the CSI command.</param>
    /// <param name="ParamsRaw">The raw parameters.</param>
    public sealed record Csi(
        IReadOnlyList<char> Intermediates,
        IReadOnlyList<AnsiParameter> Params,
        char Final,
        string ParamsRaw) : AnsiToken
    {
        /// <inheritdoc/>
        public bool Equals(Csi? other)
        {
            return other is not null
                && base.Equals(other)
                && Intermediates.SequenceEqual(other.Intermediates)
                && Params.SequenceEqual(other.Params)
                && Final == other.Final
                && ParamsRaw == other.ParamsRaw;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();
                hash = (hash * 397) ^ Final.GetHashCode();
                hash = (hash * 397) ^ ParamsRaw.GetHashCode();
                foreach (var intermediate in Intermediates)
                {
                    hash = (hash * 397) ^ intermediate.GetHashCode();
                }

                foreach (var parameter in Params)
                {
                    hash = (hash * 397) ^ parameter.GetHashCode();
                }

                return hash;
            }
        }

        /// <inheritdoc/>
        protected override bool PrintMembers(StringBuilder builder)
        {
            builder.Append("Intermediates = ").Append(FormatList(Intermediates))
                .Append(", Params = ").Append(FormatList(Params))
                .Append(", Final = ").Append(Final)
                .Append(", ParamsRaw = ").Append(ParamsRaw);

            return true;
        }
    }

    /// <summary>
    /// Executes an OSC command.
    /// </summary>
    /// <param name="Command">The command to execute.</param>
    public sealed record Osc(OscCommand Command) : AnsiToken;

    /// <summary>
    /// Sets up a DCS handler.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Params">The parameters, each with its colon-separated subparameters.</param>
    /// <param name="Final">The final byte identifying the DCS command.</param>
    /// <param name="ParamsRaw">The raw parameters.</param>
    public sealed record DcsHook(
        IReadOnlyList<char> Intermediates,
        IReadOnlyList<AnsiParameter> Params,
        char Final,
        string ParamsRaw) : AnsiToken
    {
        /// <inheritdoc/>
        public bool Equals(DcsHook? other)
        {
            return other is not null
                && base.Equals(other)
                && Intermediates.SequenceEqual(other.Intermediates)
                && Params.SequenceEqual(other.Params)
                && Final == other.Final
                && ParamsRaw == other.ParamsRaw;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = base.GetHashCode();
                hash = (hash * 397) ^ Final.GetHashCode();
                hash = (hash * 397) ^ ParamsRaw.GetHashCode();
                foreach (var intermediate in Intermediates)
                {
                    hash = (hash * 397) ^ intermediate.GetHashCode();
                }

                foreach (var parameter in Params)
                {
                    hash = (hash * 397) ^ parameter.GetHashCode();
                }

                return hash;
            }
        }

        /// <inheritdoc/>
        protected override bool PrintMembers(StringBuilder builder)
        {
            builder.Append("Intermediates = ").Append(FormatList(Intermediates))
                .Append(", Params = ").Append(FormatList(Params))
                .Append(", Final = ").Append(Final)
                .Append(", ParamsRaw = ").Append(ParamsRaw);

            return true;
        }
    }

    /// <summary>
    /// Puts a byte into the selected handler.
    /// </summary>
    /// <param name="Code">The byte to put.</param>
    public sealed record DcsPut(char Code) : AnsiToken;

    /// <summary>
    /// Unselects the current specified DCS handler.
    /// </summary>
    public sealed record DcsUnhook : AnsiToken;

    /// <summary>
    /// A key that the terminal sent as SS3 (ESC O), such as an arrow key or F1 to F4. Only
    /// <see cref="AnsiParserMode.Input"/> reports these.
    /// </summary>
    /// <param name="Final">The character identifying the key, such as A for the up arrow.</param>
    public sealed record Ss3(char Final) : AnsiToken;

    /// <summary>
    /// Text that was pasted with bracketed paste turned on. Only
    /// <see cref="AnsiParserMode.Input"/> reports these.
    /// </summary>
    /// <param name="Text">The pasted text, without the CSI 200 ~ and CSI 201 ~ markers.</param>
    public sealed record Paste(string Text) : AnsiToken;
}