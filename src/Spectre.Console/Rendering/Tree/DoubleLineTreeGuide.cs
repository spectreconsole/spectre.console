using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// A tree guide made up of double lines.
    /// </summary>
    public sealed class DoubleLineTreeGuide : TreeGuide
    {
        /// <inheritdoc/>
        public override TreeGuide? SafeTreeGuide => Ascii;

        /// <inheritdoc/>
        public override string GetPart(TreeGuidePart part)
        {
            return part switch
            {
                TreeGuidePart.Space => "    ",
                TreeGuidePart.Continue => "║   ",
                TreeGuidePart.Fork => "╠══ ",
                TreeGuidePart.End => "╚══ ",
                _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Unknown tree part."),
            };
        }
    }
}