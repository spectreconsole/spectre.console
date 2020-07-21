namespace Spectre.Console
{
    /// <summary>
    /// Represents a color system.
    /// </summary>
    public enum ColorSystem
    {
        /// <summary>
        /// No colors.
        /// </summary>
        NoColors = 0,

        /// <summary>
        /// Legacy, 3-bit mode.
        /// </summary>
        Legacy = 1,

        /// <summary>
        /// Standard, 4-bit mode.
        /// </summary>
        Standard = 2,

        /// <summary>
        /// 8-bit mode.
        /// </summary>
        EightBit = 3,

        /// <summary>
        /// 24-bit mode.
        /// </summary>
        TrueColor = 4,
    }
}
