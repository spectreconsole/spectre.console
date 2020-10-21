using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasBoxBorder"/>.
    /// </summary>
    public static class HasBoxBorderExtensions
    {
        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Border<T>(this T obj, BoxBorder border)
            where T : class, IHasBoxBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Border = border;
            return obj;
        }

        /// <summary>
        /// Do not display a border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T NoBorder<T>(this T obj)
            where T : class, IHasBoxBorder
        {
            return Border(obj, BoxBorder.None);
        }

        /// <summary>
        /// Display a square border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SquareBorder<T>(this T obj)
            where T : class, IHasBoxBorder
        {
            return Border(obj, BoxBorder.Square);
        }

        /// <summary>
        /// Display an ASCII border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AsciiBorder<T>(this T obj)
            where T : class, IHasBoxBorder
        {
            return Border(obj, BoxBorder.Ascii);
        }

        /// <summary>
        /// Display a rounded border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T RoundedBorder<T>(this T obj)
            where T : class, IHasBoxBorder
        {
            return Border(obj, BoxBorder.Rounded);
        }

        /// <summary>
        /// Display a heavy border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyBorder<T>(this T obj)
            where T : class, IHasBoxBorder
        {
            return Border(obj, BoxBorder.Heavy);
        }

        /// <summary>
        /// Display a double border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T DoubleBorder<T>(this T obj)
            where T : class, IHasBoxBorder
        {
            return Border(obj, BoxBorder.Double);
        }
    }
}
