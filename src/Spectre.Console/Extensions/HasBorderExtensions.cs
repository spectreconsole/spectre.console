using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasBorder"/>.
    /// </summary>
    public static class HasBorderExtensions
    {
        /// <summary>
        /// Do not display a border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T NoBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.None);
        }

        /// <summary>
        /// Display a square border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SquareBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Square);
        }

        /// <summary>
        /// Display an ASCII border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AsciiBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Ascii);
        }

        /// <summary>
        /// Display another ASCII border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T Ascii2Border<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Ascii2);
        }

        /// <summary>
        /// Display an ASCII border with a double header border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T AsciiDoubleHeadBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.AsciiDoubleHead);
        }

        /// <summary>
        /// Display a rounded border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T RoundedBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Rounded);
        }

        /// <summary>
        /// Display a minimal border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MinimalBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Minimal);
        }

        /// <summary>
        /// Display a minimal border with a heavy head.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MinimalHeavyHeadBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.MinimalHeavyHead);
        }

        /// <summary>
        /// Display a minimal border with a double header border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T MinimalDoubleHeadBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.MinimalDoubleHead);
        }

        /// <summary>
        /// Display a simple border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SimpleBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Simple);
        }

        /// <summary>
        /// Display a simple border with heavy lines.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SimpleHeavyBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.SimpleHeavy);
        }

        /// <summary>
        /// Display a simple border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HorizontalBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Horizontal);
        }

        /// <summary>
        /// Display a heavy border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Heavy);
        }

        /// <summary>
        /// Display a border with a heavy edge.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyEdgeBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.HeavyEdge);
        }

        /// <summary>
        /// Display a border with a heavy header.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T HeavyHeadBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.HeavyHead);
        }

        /// <summary>
        /// Display a double border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T DoubleBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.Double);
        }

        /// <summary>
        /// Display a border with a double edge.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T DoubleEdgeBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            return SetBorder(obj, Border.DoubleEdge);
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SetBorder<T>(this T obj, Border border)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.Border = border;
            return obj;
        }

        /// <summary>
        /// Disables the safe border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T NoSafeBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.UseSafeBorder = false;
            return obj;
        }

        /// <summary>
        /// Sets the border style.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border color for.</param>
        /// <param name="style">The border style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SetBorderStyle<T>(this T obj, Style style)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.BorderStyle = style;
            return obj;
        }

        /// <summary>
        /// Sets the border color.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to set the border color for.</param>
        /// <param name="color">The border color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SetBorderColor<T>(this T obj, Color color)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.BorderStyle = (obj.BorderStyle ?? Style.Plain).WithForeground(color);
            return obj;
        }
    }
}
