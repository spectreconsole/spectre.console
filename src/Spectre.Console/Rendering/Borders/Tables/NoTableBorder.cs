namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents an invisible border.
    /// </summary>
    public sealed class NoTableBorder : TableBorder
    {
        /// <inheritdoc/>
        public override bool Visible => false;

        /// <inheritdoc/>
        public override string GetPart(TableBorderPart part)
        {
            return " ";
        }
    }
}
