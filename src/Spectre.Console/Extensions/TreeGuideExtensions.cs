using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Contains extension methods for <see cref="TreeGuide"/>.
    /// </summary>
    public static class TreeGuideExtensions
    {
        /// <summary>
        /// Gets the safe border for a border.
        /// </summary>
        /// <param name="guide">The tree guide to get the safe version for.</param>
        /// <param name="safe">Whether or not to return the safe border.</param>
        /// <returns>The safe border if one exist, otherwise the original border.</returns>
        public static TreeGuide GetSafeTreeGuide(this TreeGuide guide, bool safe)
        {
            if (guide is null)
            {
                throw new ArgumentNullException(nameof(guide));
            }

            if (safe && guide.SafeTreeGuide != null)
            {
                return guide.SafeTreeGuide;
            }

            return guide;
        }
    }
}
