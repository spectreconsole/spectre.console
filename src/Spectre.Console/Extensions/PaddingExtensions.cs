namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Padding"/>.
    /// </summary>
    public static class PaddingExtensions
    {
        /// <summary>
        /// Gets the left padding.
        /// </summary>
        /// <param name="padding">The padding.</param>
        /// <returns>The left padding or zero if <c>padding</c> is null.</returns>
        public static int GetLeftSafe(this Padding? padding)
        {
            return padding?.Left ?? 0;
        }

        /// <summary>
        /// Gets the right padding.
        /// </summary>
        /// <param name="padding">The padding.</param>
        /// <returns>The right padding or zero if <c>padding</c> is null.</returns>
        public static int GetRightSafe(this Padding? padding)
        {
            return padding?.Right ?? 0;
        }

        /// <summary>
        /// Gets the top padding.
        /// </summary>
        /// <param name="padding">The padding.</param>
        /// <returns>The top padding or zero if <c>padding</c> is null.</returns>
        public static int GetTopSafe(this Padding? padding)
        {
            return padding?.Top ?? 0;
        }

        /// <summary>
        /// Gets the bottom padding.
        /// </summary>
        /// <param name="padding">The padding.</param>
        /// <returns>The bottom padding or zero if <c>padding</c> is null.</returns>
        public static int GetBottomSafe(this Padding? padding)
        {
            return padding?.Bottom ?? 0;
        }
    }
}
