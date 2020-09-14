using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IPaddable"/>.
    /// </summary>
    public static class PaddableExtensions
    {
        /// <summary>
        /// Sets the left padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="left">The left padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T PadLeft<T>(this T obj, int left)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return SetPadding(obj, new Padding(left, obj.Padding.Top, obj.Padding.Right, obj.Padding.Bottom));
        }

        /// <summary>
        /// Sets the top padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="top">The top padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T PadTop<T>(this T obj, int top)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return SetPadding(obj, new Padding(obj.Padding.Left, top, obj.Padding.Right, obj.Padding.Bottom));
        }

        /// <summary>
        /// Sets the right padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="right">The right padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T PadRight<T>(this T obj, int right)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return SetPadding(obj, new Padding(obj.Padding.Left, obj.Padding.Top, right, obj.Padding.Bottom));
        }

        /// <summary>
        /// Sets the bottom padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="bottom">The bottom padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T PadBottom<T>(this T obj, int bottom)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return SetPadding(obj, new Padding(obj.Padding.Left, obj.Padding.Top, obj.Padding.Right, bottom));
        }

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
