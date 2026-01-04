namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasBorder"/>.
/// </summary>
public static class HasBorderExtensions
{
    /// <param name="obj">The object to enable the safe border for.</param>
    /// <typeparam name="T">An object type with a border.</typeparam>
    extension<T>(T obj) where T : class, IHasBorder
    {
        /// <summary>
        /// Enables the safe border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T SafeBorder()
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
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T NoSafeBorder()
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
        /// <param name="style">The border style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T BorderStyle(Style style)
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
        /// <param name="color">The border color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T BorderColor(Color color)
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