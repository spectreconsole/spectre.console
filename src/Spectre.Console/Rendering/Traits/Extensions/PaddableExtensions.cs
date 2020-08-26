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
        /// <param name="padding">The left padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T PadLeft<T>(this T obj, int padding)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return SetPadding(obj, new Padding(padding, obj.Padding.Right));
        }

        /// <summary>
        /// Sets the right padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="padding">The right padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T PadRight<T>(this T obj, int padding)
            where T : class, IPaddable
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            return SetPadding(obj, new Padding(obj.Padding.Left, padding));
        }

        /// <summary>
        /// Sets the left and right padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="left">The left padding to apply.</param>
        /// <param name="right">The right padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SetPadding<T>(this T obj, int left, int right)
            where T : class, IPaddable
        {
            return SetPadding(obj, new Padding(left, right));
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
