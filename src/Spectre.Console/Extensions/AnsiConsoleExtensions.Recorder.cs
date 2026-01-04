namespace Spectre.Console;

public static partial class AnsiConsoleExtensions
{
    extension(AnsiConsole)
    {
        /// <summary>
        /// Creates a recorder for the specified console.
        /// </summary>
        /// <returns>A recorder for the specified console.</returns>
        public static Recorder CreateRecorder()
        {
            return new Recorder(AnsiConsole.Console);
        }

        /// <summary>
        /// Starts recording the console output.
        /// </summary>
        public static void Record()
        {
            if (AnsiConsole.Recorder == null)
            {
                AnsiConsole.Recorder = new Recorder(AnsiConsole.Console);
            }
        }

        /// <summary>
        /// Exports all recorded console output as text.
        /// </summary>
        /// <returns>The recorded output as text.</returns>
        public static string ExportText()
        {
            if (AnsiConsole.Recorder == null)
            {
                throw new InvalidOperationException("Cannot export text since a recording hasn't been started.");
            }

            return AnsiConsole.Recorder.ExportText();
        }

        /// <summary>
        /// Exports all recorded console output as HTML text.
        /// </summary>
        /// <returns>The recorded output as HTML text.</returns>
        public static string ExportHtml()
        {
            if (AnsiConsole.Recorder == null)
            {
                throw new InvalidOperationException("Cannot export HTML since a recording hasn't been started.");
            }

            return AnsiConsole.Recorder.ExportHtml();
        }

        /// <summary>
        /// Exports all recorded console output using a custom encoder.
        /// </summary>
        /// <param name="encoder">The encoder to use.</param>
        /// <returns>The recorded output.</returns>
        public static string ExportCustom(IAnsiConsoleEncoder encoder)
        {
            if (AnsiConsole.Recorder == null)
            {
                throw new InvalidOperationException("Cannot export HTML since a recording hasn't been started.");
            }

            ArgumentNullException.ThrowIfNull(encoder);

            return AnsiConsole.Recorder.Export(encoder);
        }
    }
}