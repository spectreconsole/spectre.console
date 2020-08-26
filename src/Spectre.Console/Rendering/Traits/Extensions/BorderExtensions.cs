using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Contains extension methods for <see cref="IHasBorder"/>.
    /// </summary>
    public static class BorderExtensions
    {
        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <typeparam name="T">The object that has a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SetBorder<T>(this T obj, BorderKind border)
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
        /// <typeparam name="T">The object that has a border.</typeparam>
        /// <param name="obj">The object to set the border for.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T NoSafeBorder<T>(this T obj)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.SafeBorder = false;
            return obj;
        }

        /// <summary>
        /// Sets the border color.
        /// </summary>
        /// <typeparam name="T">The object that has a border.</typeparam>
        /// <param name="obj">The object to set the border color for.</param>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static T SetBorderColor<T>(this T obj, Color color)
            where T : class, IHasBorder
        {
            if (obj is null)
            {
                throw new ArgumentNullException(nameof(obj));
            }

            obj.BorderColor = color;
            return obj;
        }
    }
}
