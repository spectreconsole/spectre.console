using System;
using System.ComponentModel;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IPaddable"/>.
    /// </summary>
    public static class ObsoletePaddableExtensions
    {
        /// <summary>
        /// Sets the left and right padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="left">The left padding to apply.</param>
        /// <param name="top">The top padding to apply.</param>
        /// <param name="right">The right padding to apply.</param>
        /// <param name="bottom">The bottom padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Padding(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetPadding<T>(this T obj, int left, int top, int right, int bottom)
            where T : class, IPaddable
        {
            return SetPadding(obj, new Padding(left, top, right, bottom));
        }

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="padding">The padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        [Obsolete("Use Padding(..) instead.")]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public static T SetPadding<T>(this T obj, Padding padding)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Padding = padding;
            return obj;
        }
    }
}
