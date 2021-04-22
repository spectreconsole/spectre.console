namespace Spectre.Console.Testing
{
    /// <summary>
    /// Contains extensions for <see cref="TestConsole"/>.
    /// </summary>
    public static class TestConsoleExtensions
    {
        /// <summary>
        /// Sets the console's color system.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="colors">The color system to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TestConsole Colors(this TestConsole console, ColorSystem colors)
        {
            console.Profile.Capabilities.ColorSystem = colors;
            return console;
        }

        /// <summary>
        /// Sets whether or not ANSI is supported.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="enable">Whether or not VT/ANSI control codes are supported.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TestConsole SupportsAnsi(this TestConsole console, bool enable)
        {
            console.Profile.Capabilities.Ansi = enable;
            return console;
        }

        /// <summary>
        /// Makes the console interactive.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TestConsole Interactive(this TestConsole console)
        {
            console.Profile.Capabilities.Interactive = true;
            return console;
        }

        /// <summary>
        /// Sets the console width.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="width">The console width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TestConsole Width(this TestConsole console, int width)
        {
            console.Profile.Width = width;
            return console;
        }

        /// <summary>
        /// Turns on emitting of VT/ANSI sequences.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public static TestConsole EmitAnsiSequences(this TestConsole console)
        {
            console.SetCursor(null);
            console.EmitAnsiSequences = true;
            return console;
        }
    }
}
