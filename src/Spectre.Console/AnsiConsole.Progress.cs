namespace Spectre.Console
{
    /// <summary>
    /// A console capable of writing ANSI escape sequences.
    /// </summary>
    public static partial class AnsiConsole
    {
        /// <summary>
        /// Creates a new <see cref="Progress"/> instance.
        /// </summary>
        /// <returns>A <see cref="Progress"/> instance.</returns>
        public static Progress Progress()
        {
            return Console.Progress();
        }

        /// <summary>
        /// Creates a new <see cref="Status"/> instance.
        /// </summary>
        /// <returns>A <see cref="Status"/> instance.</returns>
        public static Status Status()
        {
            return Console.Status();
        }
    }
}
