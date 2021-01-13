using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// An ASCII tree guide.
    /// </summary>
    public sealed class AsciiTreeGuide : TreeGuide
    {
        /// <inheritdoc/>
        public override string GetPart(TreeGuidePart part)
        {
            return part switch
            {
                TreeGuidePart.Space => "    ",
                TreeGuidePart.Continue => "|   ",
                TreeGuidePart.Fork => "|-- ",
                TreeGuidePart.End => "`-- ",
                _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Unknown tree part."),
            };
        }
    }
}