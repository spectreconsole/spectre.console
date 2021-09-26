namespace Spectre.Console
{
    /// <summary>
    /// Defines the states an item can have.
    /// </summary>
    public enum PromptItemState
    {
        /// <summary>
        /// An item that is not the current item and not disabled.
        /// </summary>
        Normal,

        /// <summary>
        /// The current item under the cursor. (If the item is not disabled.)
        /// </summary>
        Current,

        /// <summary>
        /// Disabled item.
        /// </summary>
        Disabled,
    }
}