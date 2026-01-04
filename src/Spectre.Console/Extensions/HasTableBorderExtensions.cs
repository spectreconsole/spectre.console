namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="IHasTableBorder"/>.
/// </summary>
public static class HasTableBorderExtensions
{
    /// <param name="obj">The object to set the border for.</param>
    /// <typeparam name="T">An object type with a border.</typeparam>
    extension<T>(T obj) where T : class, IHasTableBorder
    {
        /// <summary>
        /// Do not display a border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T NoBorder()
        {
            return Border(obj, TableBorder.None);
        }

        /// <summary>
        /// Display a square border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T SquareBorder()
        {
            return Border(obj, TableBorder.Square);
        }

        /// <summary>
        /// Display an ASCII border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T AsciiBorder()
        {
            return Border(obj, TableBorder.Ascii);
        }

        /// <summary>
        /// Display another ASCII border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Ascii2Border()
        {
            return Border(obj, TableBorder.Ascii2);
        }

        /// <summary>
        /// Display an ASCII border with a double header border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T AsciiDoubleHeadBorder()
        {
            return Border(obj, TableBorder.AsciiDoubleHead);
        }

        /// <summary>
        /// Display a rounded border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T RoundedBorder()
        {
            return Border(obj, TableBorder.Rounded);
        }

        /// <summary>
        /// Display a minimal border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T MinimalBorder()
        {
            return Border(obj, TableBorder.Minimal);
        }

        /// <summary>
        /// Display a minimal border with a heavy head.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T MinimalHeavyHeadBorder()
        {
            return Border(obj, TableBorder.MinimalHeavyHead);
        }

        /// <summary>
        /// Display a minimal border with a double header border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T MinimalDoubleHeadBorder()
        {
            return Border(obj, TableBorder.MinimalDoubleHead);
        }

        /// <summary>
        /// Display a simple border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T SimpleBorder()
        {
            return Border(obj, TableBorder.Simple);
        }

        /// <summary>
        /// Display a simple border with heavy lines.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T SimpleHeavyBorder()
        {
            return Border(obj, TableBorder.SimpleHeavy);
        }

        /// <summary>
        /// Display a simple border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T HorizontalBorder()
        {
            return Border(obj, TableBorder.Horizontal);
        }

        /// <summary>
        /// Display a heavy border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T HeavyBorder()
        {
            return Border(obj, TableBorder.Heavy);
        }

        /// <summary>
        /// Display a border with a heavy edge.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T HeavyEdgeBorder()
        {
            return Border(obj, TableBorder.HeavyEdge);
        }

        /// <summary>
        /// Display a border with a heavy header.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T HeavyHeadBorder()
        {
            return Border(obj, TableBorder.HeavyHead);
        }

        /// <summary>
        /// Display a double border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T DoubleBorder()
        {
            return Border(obj, TableBorder.Double);
        }

        /// <summary>
        /// Display a border with a double edge.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T DoubleEdgeBorder()
        {
            return Border(obj, TableBorder.DoubleEdge);
        }

        /// <summary>
        /// Display a markdown border.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T MarkdownBorder()
        {
            return Border(obj, TableBorder.Markdown);
        }

        /// <summary>
        /// Sets the border.
        /// </summary>
        /// <param name="border">The border to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public T Border(TableBorder border)
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