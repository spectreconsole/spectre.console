namespace Spectre.Console.Composition
{
    /// <summary>
    /// Represents an invisible border.
    /// </summary>
    public sealed class NoBorder : Border
    {
        /// <inheritdoc/>
        protected override string GetBoxPart(BorderPart part)
        {
            return " ";
        }
    }
}
