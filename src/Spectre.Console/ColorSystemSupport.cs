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
        Detect = -1,

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
