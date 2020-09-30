namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents an invisible border.
    /// </summary>
    public sealed class NoBoxBorder : BoxBorder
    {
        /// <inheritdoc/>
        protected override string GetBorderPart(BoxBorderPart part)
        {
            return " ";
        }
    }
}
