namespace Spectre.Console
{
    /// <summary>
    /// This context will be provided to the <see cref="IPromptItemRenderer{T}.Render"/> method.
    /// </summary>
    public sealed class PromptItemContext
    {
        /// <summary>
        /// Gets the state of the current item.
        /// </summary>
        public PromptItemState State { get; internal set; }

        /// <summary>
        /// Gets the Style of the prompt for the item.
        /// </summary>
        public Style? PromptStyle { get; internal set; }
    }
}