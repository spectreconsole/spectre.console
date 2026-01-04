namespace Spectre.Console.Json;

/// <summary>
/// Contains extension methods for <see cref="JsonText"/>.
/// </summary>
public static class JsonTextExtensions
{
    /// <param name="text">The JSON text instance.</param>
    extension(JsonText text)
    {
        /// <summary>
        /// Sets the style used for braces.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText BracesStyle(Style style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.BracesStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for brackets.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText BracketStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.BracketsStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for member names.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText MemberStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.MemberStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for colons.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText ColonStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.ColonStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for commas.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText CommaStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.CommaStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for string literals.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText StringStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.StringStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for number literals.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText NumberStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.NumberStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for boolean literals.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText BooleanStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.BooleanStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the style used for <c>null</c> literals.
        /// </summary>
        /// <param name="style">The style to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText NullStyle(Style? style)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.NullStyle = style;
            return text;
        }

        /// <summary>
        /// Sets the color used for braces.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText BracesColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.BracesStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for brackets.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText BracketColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.BracketsStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for member names.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText MemberColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.MemberStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for colons.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText ColonColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.ColonStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for commas.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText CommaColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.CommaStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for string literals.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText StringColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.StringStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for number literals.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText NumberColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.NumberStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for boolean literals.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText BooleanColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.BooleanStyle = new Style(color);
            return text;
        }

        /// <summary>
        /// Sets the color used for <c>null</c> literals.
        /// </summary>
        /// <param name="color">The color to set.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public JsonText NullColor(Color color)
        {
            ArgumentNullException.ThrowIfNull(text);

            text.NullStyle = new Style(color);
            return text;
        }
    }
}