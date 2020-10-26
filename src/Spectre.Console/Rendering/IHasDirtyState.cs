namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents something that can be dirty.
    /// </summary>
    public interface IHasDirtyState
    {
        /// <summary>
        /// Gets a value indicating whether the object is dirty.
        /// </summary>
        bool IsDirty { get; }
    }
}
