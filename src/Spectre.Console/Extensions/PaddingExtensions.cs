namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Padding"/>.
/// </summary>
public static class PaddingExtensions
{
    /// <param name="padding">The padding.</param>
    extension(Padding? padding)
    {
        /// <summary>
        /// Gets the left padding.
        /// </summary>
        /// <returns>The left padding or zero if <c>padding</c> is null.</returns>
        public int GetLeftSafe()
        {
            return padding?.Left ?? 0;
        }

        /// <summary>
        /// Gets the right padding.
        /// </summary>
        /// <returns>The right padding or zero if <c>padding</c> is null.</returns>
        public int GetRightSafe()
        {
            return padding?.Right ?? 0;
        }

        /// <summary>
        /// Gets the top padding.
        /// </summary>
        /// <returns>The top padding or zero if <c>padding</c> is null.</returns>
        public int GetTopSafe()
        {
            return padding?.Top ?? 0;
        }

        /// <summary>
        /// Gets the bottom padding.
        /// </summary>
        /// <returns>The bottom padding or zero if <c>padding</c> is null.</returns>
        public int GetBottomSafe()
        {
            return padding?.Bottom ?? 0;
        }
    }
}