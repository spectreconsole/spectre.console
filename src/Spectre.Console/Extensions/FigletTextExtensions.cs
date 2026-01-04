namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="FigletText"/>.
/// </summary>
public static class FigletTextExtensions
{
    /// <param name="text">The text.</param>
    extension(FigletText text)
    {
        /// <summary>
        /// Sets the color of the FIGlet text.
        /// </summary>
        /// <param name="color">The color.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public FigletText Color(Color? color)
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