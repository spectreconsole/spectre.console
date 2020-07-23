namespace Spectre.Console.Internal
{
    /// <summary>
    /// Represents a parsed markup node.
    /// </summary>
    internal interface IMarkupNode
    {
        /// <summary>
        /// Renders the node using the specified renderer.
        /// </summary>
        /// <param name="renderer">The renderer to use.</param>
        void Render(IAnsiConsole renderer);
    }
}
