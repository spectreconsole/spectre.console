namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents an invisible border.
    /// </summary>
    public sealed class NoBorder : Border
    {
        /// <inheritdoc/>
        public override bool Visible => false;

        /// <inheritdoc/>
        protected override string GetBoxPart(BorderPart part)
        {
            return " ";
        }
    }
}
