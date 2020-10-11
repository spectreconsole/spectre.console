namespace Spectre.Console
{
    /// <summary>
    /// Represents table overflow.
    /// </summary>
    public enum TableOverflow
    {
        /// <summary>
        /// Nothing is done to prevent overflow.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Drop columns until everything fit.
        /// </summary>
        Drop = 1,
    }
}
