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

            return Padding(obj, new Padding(left, obj.Padding.GetTopSafe(), obj.Padding.GetRightSafe(), obj.Padding.GetBottomSafe()));
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

            return Padding(obj, new Padding(obj.Padding.GetLeftSafe(), top, obj.Padding.GetRightSafe(), obj.Padding.GetBottomSafe()));
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

            return Padding(obj, new Padding(obj.Padding.GetLeftSafe(), obj.Padding.GetTopSafe(), right, obj.Padding.GetBottomSafe()));
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

            return Padding(obj, new Padding(obj.Padding.GetLeftSafe(), obj.Padding.GetTopSafe(), obj.Padding.GetRightSafe(), bottom));
        }

        /// <summary>
        /// Sets the left, top, right and bottom padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="left">The left padding to apply.</param>
        /// <param name="top">The top padding to apply.</param>
        /// <param name="right">The right padding to apply.</param>
        /// <param name="bottom">The bottom padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Padding<T>(this T obj, int left, int top, int right, int bottom)
            where T : class, IPaddable
        {
            return Padding(obj, new Padding(left, top, right, bottom));
        }

        /// <summary>
        /// Sets the horizontal and vertical padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="horizontal">The left and right padding.</param>
        /// <param name="vertical">The top and bottom padding.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Padding<T>(this T obj, int horizontal, int vertical)
            where T : class, IPaddable
        {
            return Padding(obj, new Padding(horizontal, vertical));
        }

        /// <summary>
        /// Sets the padding.
        /// </summary>
        /// <typeparam name="T">An object implementing <see cref="IPaddable"/>.</typeparam>
        /// <param name="obj">The paddable object instance.</param>
        /// <param name="padding">The padding to apply.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Padding<T>(this T obj, Padding padding)
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
