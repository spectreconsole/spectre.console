using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasBorder"/>.
    /// </summary>
    public static class HasBorderExtensions
    {
        /// <summary>
        /// Enables the safe border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to enable the safe border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SafeBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.UseSafeBorder = true;
            return obj;
        }

        /// <summary>
        /// Disables the safe border.
        /// </summary>
        /// <typeparam name="T">An object type with a border.</typeparam>
        /// <param name="obj">The object to disable the safe border for.</param>
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
        /// <param name="obj">The object to set the border style for.</param>
        /// <param name="style">The border style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T BorderStyle<T>(this T obj, Style style)
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
        public static T BorderColor<T>(this T obj, Color color)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.BorderStyle = (obj.BorderStyle ?? Style.Plain).Foreground(color);
            return obj;
        }
    }
}
