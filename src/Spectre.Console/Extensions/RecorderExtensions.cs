using System;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Contains extension methods for <see cref="Recorder"/>.
    /// </summary>
    public static class RecorderExtensions
    {
        private static readonly TextEncoder _textEncoder = new TextEncoder();
        private static readonly HtmlEncoder _htmlEncoder = new HtmlEncoder();

        /// <summary>
        /// Exports the recorded content as text.
        /// </summary>
        /// <param name="recorder">The recorder.</param>
        /// <returns>The recorded content as text.</returns>
        public static string ExportText(this Recorder recorder)
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
        /// <param name="recorder">The recorder.</param>
        /// <returns>The recorded content as HTML.</returns>
        public static string ExportHtml(this Recorder recorder)
        {
            if (recorder is null)
            {
                throw new ArgumentNullException(nameof(recorder));
            }

            return recorder.Export(_htmlEncoder);
        }
    }
}
