namespace Spectre.Console
{
    /// <summary>
    /// Exception settings.
    /// </summary>
    public sealed class ExceptionSettings
    {
        /// <summary>
        /// Gets or sets the exception format.
        /// </summary>
        public ExceptionFormats Format { get; set; }

        /// <summary>
        /// Gets or sets the exception style.
        /// </summary>
        public ExceptionStyle Style { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionSettings"/> class.
        /// </summary>
        public ExceptionSettings()
        {
            Format = ExceptionFormats.Default;
            Style = new ExceptionStyle();
        }
    }
}
