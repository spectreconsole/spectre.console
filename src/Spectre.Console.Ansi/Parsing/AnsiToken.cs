namespace Spectre.Console.Ansi;

/// <summary>
/// Represents a parsed ANSI/VT token.
/// </summary>
public abstract record AnsiToken
{
    /// <summary>
    /// Prints a Unicode scalar value (codepoint) to the screen.
    /// </summary>
    /// <param name="Codepoint">
    /// The Unicode codepoint to print. Astral codepoints, encoded as UTF-16
    /// surrogate pairs in the input, are combined into a single scalar value.
    /// </param>
    public record Print(int Codepoint) : AnsiToken
    {
        /// <summary>
        /// Converts the <see cref="Codepoint"/> to its UTF-16 representation, suitable
        /// for writing to a <see cref="System.IO.TextWriter"/> or a <see cref="string"/>.
        /// </summary>
        /// <returns>
        /// A string of one or two <see cref="char"/> values; an astral codepoint is
        /// returned as a UTF-16 surrogate pair. The parser only ever emits valid Unicode
        /// scalar values, so this never throws.
        /// </returns>
        public string ToUtf16()
        {
            return char.ConvertFromUtf32(Codepoint);
        }
    }

    /// <summary>
    /// Executes the specified function.
    /// </summary>
    /// <param name="Function">The C0/C1 function to execute.</param>
    public record Execute(char Function) : AnsiToken;

    /// <summary>
    /// Execute an ESC command.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Final">The final byte identifying the ESC command.</param>
    public record Esc(IReadOnlyList<char> Intermediates, char Final) : AnsiToken
    {
        /// <inheritdoc/>
        public virtual bool Equals(Esc? other)
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
    }

    /// <summary>
    /// Executes a CSI command.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Params">The parameters.</param>
    /// <param name="Final">The final byte identifying the CSI command.</param>
    /// <param name="ParamsRaw">The raw parameters.</param>
    public record Csi(
        IReadOnlyList<char> Intermediates,
        IReadOnlyList<int> Params,
        char Final,
        string ParamsRaw) : AnsiToken
    {
        /// <inheritdoc/>
        public virtual bool Equals(Csi? other)
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
                    hash = (hash * 397) ^ parameter;
                }

                return hash;
            }
        }
    }

    /// <summary>
    /// Executes an OSC command.
    /// </summary>
    /// <param name="Command">The command to execute.</param>
    public record Osc(OscCommand Command) : AnsiToken;

    /// <summary>
    /// Sets up a DCS handler.
    /// </summary>
    /// <param name="Intermediates">Intermediate bytes.</param>
    /// <param name="Params">The parameters.</param>
    /// <param name="Final">The final byte identifying the DCS command.</param>
    /// <param name="ParamsRaw">The raw parameters.</param>
    public record DcsHook(
        IReadOnlyList<char> Intermediates,
        IReadOnlyList<int> Params,
        char Final,
        string ParamsRaw) : AnsiToken
    {
        /// <inheritdoc/>
        public virtual bool Equals(DcsHook? other)
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
                    hash = (hash * 397) ^ parameter;
                }

                return hash;
            }
        }
    }

    /// <summary>
    /// Puts a byte into the selected handler.
    /// </summary>
    /// <param name="Code">The byte to put.</param>
    public record DcsPut(char Code) : AnsiToken;

    /// <summary>
    /// Unselects the current specified DCS handler.
    /// </summary>
    public record DcsUnhook : AnsiToken;
}