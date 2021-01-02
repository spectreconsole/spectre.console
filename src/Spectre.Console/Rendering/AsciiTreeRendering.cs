using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// An ASCII rendering of a tree.
    /// </summary>
    public sealed class AsciiTreeRendering : ITreeRendering
    {
        /// <inheritdoc/>
        public string GetPart(TreePart part)
        {
            return part switch
            {
                TreePart.SiblingConnector => "│   ",
                TreePart.ChildBranch => "├── ",
                TreePart.BottomChildBranch => "└── ",
                _ => throw new ArgumentOutOfRangeException(nameof(part), part, "Unknown tree part."),
            };
        }

        /// <inheritdoc/>
        public int PartSize => 4;
    }
}