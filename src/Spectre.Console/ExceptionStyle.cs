namespace Spectre.Console
{
    /// <summary>
    /// Represent an exception style.
    /// </summary>
    public sealed class ExceptionStyle
    {
        /// <summary>
        /// Gets or sets the message color.
        /// </summary>
        public Style Message { get; set; } = new Style(Color.Red, Color.Default, Decoration.Bold);

        /// <summary>
        /// Gets or sets the exception color.
        /// </summary>
        public Style Exception { get; set; } = new Style(Color.White);

        /// <summary>
        /// Gets or sets the method color.
        /// </summary>
        public Style Method { get; set; } = new Style(Color.Yellow);

        /// <summary>
        /// Gets or sets the parameter type color.
        /// </summary>
        public Style ParameterType { get; set; } = new Style(Color.Blue);

        /// <summary>
        /// Gets or sets the parameter name color.
        /// </summary>
        public Style ParameterName { get; set; } = new Style(Color.Silver);

        /// <summary>
        /// Gets or sets the parenthesis color.
        /// </summary>
        public Style Parenthesis { get; set; } = new Style(Color.Silver);

        /// <summary>
        /// Gets or sets the path color.
        /// </summary>
        public Style Path { get; set; } = new Style(Color.Yellow, Color.Default, Decoration.Bold);

        /// <summary>
        /// Gets or sets the line number color.
        /// </summary>
        public Style LineNumber { get; set; } = new Style(Color.Blue);

        /// <summary>
        /// Gets or sets the color for dimmed text such as "at" or "in".
        /// </summary>
        public Style Dimmed { get; set; } = new Style(Color.Grey);

        /// <summary>
        /// Gets or sets the color for non emphasized items.
        /// </summary>
        public Style NonEmphasized { get; set; } = new Style(Color.Silver);
    }
}
