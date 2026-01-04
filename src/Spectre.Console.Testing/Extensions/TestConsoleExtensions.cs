namespace Spectre.Console.Testing;

/// <summary>
/// Contains extensions for <see cref="TestConsole"/>.
/// </summary>
public static partial class TestConsoleExtensions
{
    /// <param name="console">The console.</param>
    extension(TestConsole console)
    {
        /// <summary>
        /// Sets the console's color system.
        /// </summary>
        /// <param name="colors">The color system to use.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole Colors(ColorSystem colors)
        {
            console.Profile.Capabilities.ColorSystem = colors;
            return console;
        }

        /// <summary>
        /// Sets whether or not ANSI is supported.
        /// </summary>
        /// <param name="enable">Whether or not VT/ANSI control codes are supported.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole SupportsAnsi(bool enable)
        {
            console.Profile.Capabilities.Ansi = enable;
            return console;
        }

        /// <summary>
        /// Makes the console interactive.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole Interactive()
        {
            console.Profile.Capabilities.Interactive = true;
            return console;
        }

        /// <summary>
        /// Sets the console width.
        /// </summary>
        /// <param name="width">The console width.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole Width(int width)
        {
            console.Profile.Width = width;
            return console;
        }

        /// <summary>
        /// Sets the console height.
        /// </summary>
        /// <param name="width">The console height.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole Height(int width)
        {
            console.Profile.Height = width;
            return console;
        }

        /// <summary>
        /// Sets the console size.
        /// </summary>
        /// <param name="size">The console size.</param>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole Size(Size size)
        {
            console.Profile.Width = size.Width;
            console.Profile.Height = size.Height;
            return console;
        }

        /// <summary>
        /// Turns on emitting of VT/ANSI sequences.
        /// </summary>
        /// <returns>The same instance so that multiple calls can be chained.</returns>
        public TestConsole EmitAnsiSequences()
        {
            console.SetCursor(null);
            console.EmitAnsiSequences = true;
            return console;
        }
    }
}