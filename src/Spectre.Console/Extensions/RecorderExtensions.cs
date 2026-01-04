namespace Spectre.Console;

/// <summary>
/// Contains extension methods for <see cref="Recorder"/>.
/// </summary>
public static class RecorderExtensions
{
    private static readonly TextEncoder _textEncoder = new TextEncoder();
    private static readonly HtmlEncoder _htmlEncoder = new HtmlEncoder();

    /// <param name="recorder">The recorder.</param>
    extension(Recorder recorder)
    {
        /// <summary>
        /// Exports the recorded content as text.
        /// </summary>
        /// <returns>The recorded content as text.</returns>
        public string ExportText()
        {
            if (recorder is null)
            {
                throw new ArgumentNullException(nameof(recorder));
            }

            return recorder.Export(_textEncoder);
        }

        /// <summary>
        /// Exports the recorded content as HTML.
        /// </summary>
        /// <returns>The recorded content as HTML.</returns>
        public string ExportHtml()
        {
            if (recorder is null)
            {
                throw new ArgumentNullException(nameof(recorder));
            }

            return recorder.Export(_htmlEncoder);
        }
    }
}