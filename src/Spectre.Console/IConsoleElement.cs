namespace Spectre.Console
{
    /// <summary>
    /// Represents an element that can be rendered by the console.
    /// </summary>
    public interface IConsoleElement
    {
        /// <summary>
        /// Gets the width of the element.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Renders the element using the specified console.
        /// </summary>
        /// <param name="console">The console to use.</param>
        void Render(IAnsiConsole console);
    }
}
