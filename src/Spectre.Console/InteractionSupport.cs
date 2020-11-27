namespace Spectre.Console
{
    /// <summary>
    /// Determines interactivity support.
    /// </summary>
    public enum InteractionSupport
    {
        /// <summary>
        /// Interaction support should be
        /// detected by the system.
        /// </summary>
        Detect = 0,

        /// <summary>
        /// Interactivity is supported.
        /// </summary>
        Yes = 1,

        /// <summary>
        /// Interactivity is not supported.
        /// </summary>
        No = 2,
    }
}
