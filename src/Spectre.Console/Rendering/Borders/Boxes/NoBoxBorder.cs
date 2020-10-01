namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents an invisible border.
    /// </summary>
    public sealed class NoBoxBorder : BoxBorder
    {
        /// <inheritdoc/>
        public override string GetPart(BoxBorderPart part)
        {
            return " ";
        }
    }
}
