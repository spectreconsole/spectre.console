namespace Spectre.Console
{
    /// <summary>
    /// Represents a parsed markup node.
    /// </summary>
    public interface IRenderable
    {
        /// <summary>
        /// Gets the width of the element.
        /// </summary>
        int Width { get; }

        /// <summary>
        /// Renders the node using the specified renderer.
        /// </summary>
        /// <param name="renderer">The renderer to use.</param>
        void Render(IAnsiConsole renderer);
    }
}
