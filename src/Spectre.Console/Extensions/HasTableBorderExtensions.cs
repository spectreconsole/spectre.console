using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasTableBorder"/>.
    /// </summary>
    public static class HasTableBorderExtensions
    {
        /// <summary>
        /// Do not display a border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T NoBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.None);
        }

        /// <summary>
        /// Display a square border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SquareBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Square);
        }

        /// <summary>
        /// Display an ASCII border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AsciiBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Ascii);
        }

        /// <summary>
        /// Display another ASCII border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Ascii2Border<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Ascii2);
        }

        /// <summary>
        /// Display an ASCII border with a double header border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AsciiDoubleHeadBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.AsciiDoubleHead);
        }

        /// <summary>
        /// Display a rounded border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T RoundedBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Rounded);
        }

        /// <summary>
        /// Display a minimal border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MinimalBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Minimal);
        }

        /// <summary>
        /// Display a minimal border with a heavy head.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MinimalHeavyHeadBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.MinimalHeavyHead);
        }

        /// <summary>
        /// Display a minimal border with a double header border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MinimalDoubleHeadBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.MinimalDoubleHead);
        }

        /// <summary>
        /// Display a simple border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SimpleBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Simple);
        }

        /// <summary>
        /// Display a simple border with heavy lines.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SimpleHeavyBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.SimpleHeavy);
        }

        /// <summary>
        /// Display a simple border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HorizontalBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Horizontal);
        }

        /// <summary>
        /// Display a heavy border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Heavy);
        }

        /// <summary>
        /// Display a border with a heavy edge.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyEdgeBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.HeavyEdge);
        }

        /// <summary>
        /// Display a border with a heavy header.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyHeadBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.HeavyHead);
        }

        /// <summary>
        /// Display a double border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T DoubleBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Double);
        }

        /// <summary>
        /// Display a border with a double edge.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T DoubleEdgeBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.DoubleEdge);
        }

        /// <summary>
        /// Display a markdown border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MarkdownBorder<T>(this T obj)
            where T : class, IHasTableBorder
        {
            return Border(obj, TableBorder.Markdown);
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Border<T>(this T obj, TableBorder border)
            where T : class, IHasTableBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Border = border;
            return obj;
        }
    }
}
