namespace Spectre.Console
{
    /// <summary>
    /// Represents console capabilities.
    /// </summary>
    public sealed class Capabilities
    {
        /// <summary>
        /// Gets a value indicating whether or not
        /// the console supports Ansi.
        /// </summary>
        public bool SupportsAnsi { get; }

        /// <summary>
        /// Gets the color system.
        /// </summary>
        public ColorSystem ColorSystem { get; }

        internal Capabilities(bool supportsAnsi, ColorSystem colorSystem)
        {
            SupportsAnsi = supportsAnsi;
            ColorSystem = colorSystem;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var supportsAnsi = SupportsAnsi ? "Yes" : "No";
            var bits = ColorSystem switch
            {
                ColorSystem.NoColors => "1 bit",
                ColorSystem.Legacy => "3 bits",
                ColorSystem.Standard => "4 bits",
                ColorSystem.EightBit => "8 bits",
                ColorSystem.TrueColor => "24 bits",
                _ => "?"
            };

            return $"ANSI={supportsAnsi}, Colors={ColorSystem} ({bits})";
        }
    }
}
