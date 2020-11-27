namespace Spectre.Console
{
    /// <summary>
    /// Determines what color system should be used.
    /// </summary>
    public enum ColorSystemSupport
    {
        /// <summary>
        /// Try to detect the color system.
        /// </summary>
        Detect = 0,

        /// <summary>
        /// No colors.
        /// </summary>
        NoColors = 1,

        /// <summary>
        /// Legacy, 3-bit mode.
        /// </summary>
        Legacy = 2,

        /// <summary>
        /// Standard, 4-bit mode.
        /// </summary>
        Standard = 3,

        /// <summary>
        /// 8-bit mode.
        /// </summary>
        EightBit = 4,

        /// <summary>
        /// 24-bit mode.
        /// </summary>
        TrueColor = 5,
    }
}
