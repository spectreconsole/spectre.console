using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        /// <summary>
        /// Starts recording the console output.
        /// </summary>
        public static void Record()
        {
            if (_recorder == null)
            {
                _recorder = new Recorder(Console);
            }
        }

        /// <summary>
        /// Exports all recorded console output as text.
        /// </summary>
        /// <returns>The recorded output as text.</returns>
        public static string ExportText()
        {
            if (_recorder == null)
            {
                throw new InvalidOperationException("Cannot export text since a recording hasn't been started.");
            }

            return _recorder.ExportText();
        }

        /// <summary>
        /// Exports all recorded console output as HTML text.
        /// </summary>
        /// <returns>The recorded output as HTML text.</returns>
        public static string ExportHtml()
        {
            if (_recorder == null)
            {
                throw new InvalidOperationException("Cannot export HTML since a recording hasn't been started.");
            }

            return _recorder.ExportHtml();
        }

        /// <summary>
        /// Exports all recorded console output using a custom encoder.
        /// </summary>
        /// <param name="encoder">The encoder to use.</param>
        /// <returns>The recorded output.</returns>
        public static string ExportCustom(IAnsiConsoleEncoder encoder)
        {
            if (_recorder == null)
            {
                throw new InvalidOperationException("Cannot export HTML since a recording hasn't been started.");
            }

            if (encoder is null)
            {
                throw new ArgumentNullException(nameof(encoder));
            }

            return _recorder.Export(encoder);
        }
    }
}
