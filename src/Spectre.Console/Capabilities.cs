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
        /// Gets a value indicating whether or not
        /// the console support links.
        /// </summary>
        /// <remarks>
        /// There is probably a lot of room for improvement here
        /// once we have more information about the terminal
        /// we're running inside.
        /// </remarks>
        public bool SupportLinks => SupportsAnsi && !LegacyConsole;

        /// <summary>
        /// Gets the color system.
        /// </summary>
        public ColorSystem ColorSystem { get; }

        /// <summary>
        /// Gets a value indicating whether or not
        /// this is a legacy console (cmd.exe).
        /// </summary>
        /// <remarks>
        /// Only relevant when running on Microsoft Windows.
        /// </remarks>
        public bool LegacyConsole { get; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the console supports interaction.
        /// </summary>
        public bool SupportsInteraction { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Capabilities"/> class.
        /// </summary>
        /// <param name="supportsAnsi">Whether or not ANSI escape sequences are supported.</param>
        /// <param name="colorSystem">The color system that is supported.</param>
        /// <param name="legacyConsole">Whether or not this is a legacy console.</param>
        /// <param name="supportsInteraction">Whether or not the console supports interaction.</param>
        public Capabilities(bool supportsAnsi, ColorSystem colorSystem, bool legacyConsole, bool supportsInteraction)
        {
            SupportsAnsi = supportsAnsi;
            ColorSystem = colorSystem;
            LegacyConsole = legacyConsole;
            SupportsInteraction = supportsInteraction;
        }

        /// <summary>
        /// Checks whether the current capabilities supports
        /// the specified color system.
        /// </summary>
        /// <param name="colorSystem">The color system to check.</param>
        /// <returns><c>true</c> if the color system is supported, otherwise <c>false</c>.</returns>
        public bool Supports(ColorSystem colorSystem)
        {
            return (int)colorSystem <= (int)ColorSystem;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var supportsAnsi = SupportsAnsi ? "Yes" : "No";
            var legacyConsole = LegacyConsole ? "Legacy" : "Modern";
            var bits = ColorSystem switch
            {
                ColorSystem.NoColors => "1 bit",
                ColorSystem.Legacy => "3 bits",
                ColorSystem.Standard => "4 bits",
                ColorSystem.EightBit => "8 bits",
                ColorSystem.TrueColor => "24 bits",
                _ => "?",
            };

            return $"ANSI={supportsAnsi}, Colors={ColorSystem}, Kind={legacyConsole} ({bits})";
        }
    }
}
