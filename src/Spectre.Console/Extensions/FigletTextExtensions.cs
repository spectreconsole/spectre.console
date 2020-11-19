using System;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="FigletText"/>.
    /// </summary>
    public static class FigletTextExtensions
    {
        /// <summary>
        /// Sets the color of the FIGlet text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="color">The color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static FigletText Color(this FigletText text, Color? color)
        {
            if (text is null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            text.Color = color ?? Console.Color.Default;
            return text;
        }
    }
}
