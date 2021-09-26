using Spectre.Console.Rendering;

namespace Spectre.Console
{
    /// <summary>
    /// Implement this interface to create a renderer for
    /// <see cref="SelectionPrompt{T}"/> or <see cref="MultiSelectionPrompt{T}"/>.
    /// </summary>
    /// <typeparam name="T">The type of the item to render.</typeparam>
    public interface IPromptItemRenderer<in T>
    {
        /// <summary>
        /// Renders one item.
        /// </summary>
        /// <param name="item">The item to render.</param>
        /// <param name="context">The <see cref="PromptItemContext"/> in which to render the item.</param>
        /// <returns>The <see cref="IRenderable"/> to show in the prompt.</returns>
        IRenderable Render(T item, PromptItemContext context);
    }
}