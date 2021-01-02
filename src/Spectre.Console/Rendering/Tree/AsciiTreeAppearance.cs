using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// An ASCII rendering of a tree.
    /// </summary>
    public sealed class AsciiTreeAppearance : TreeAppearance
    {
        /// <inheritdoc/>
        public override int PartSize => 4;

        /// <inheritdoc/>
        public override string GetPart(TreePart part)
        {
            return part switch
            {
                TreePart.SiblingConnector => "│   ",
                TreePart.ChildBranch => "├── ",
                TreePart.BottomChildBranch => "└── ",
                _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Unknown tree part."),
            };
        }
    }
}