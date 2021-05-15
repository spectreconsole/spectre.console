namespace Spectre.Console
{
    /// <summary>
    /// Represents a renderable list item.
    /// </summary>
    /// <typeparam name="T">The list item type.</typeparam>
    public sealed class RenderableListItem<T>
        where T : notnull
    {
        /// <summary>
        /// Gets the data associated with the list item.
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Gets the index associated with the list item.
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderableListItem{T}"/> class.
        /// </summary>
        /// <param name="data">The list item data.</param>
        /// <param name="index">The list item index.</param>
        public RenderableListItem(T data, int index)
        {
            Data = data;
            Index = index;
        }
    }
}
