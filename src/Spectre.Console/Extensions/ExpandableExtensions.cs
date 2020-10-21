using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IExpandable"/>.
    /// </summary>
    public static class ExpandableExtensions
    {
        /// <summary>
        /// Tells the specified object to not expand to the available area
        /// but take as little space as possible.
        /// </summary>
        /// <typeparam name="T">The expandable object.</typeparam>
        /// <param name="obj">The object to collapse.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Collapse<T>(this T obj)
            where T : class, IExpandable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Expand = false;
            return obj;
        }

        /// <summary>
        /// Tells the specified object to expand to the available area.
        /// </summary>
        /// <typeparam name="T">The expandable object.</typeparam>
        /// <param name="obj">The object to expand.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Expand<T>(this T obj)
            where T : class, IExpandable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Expand = true;
            return obj;
        }
    }
}
