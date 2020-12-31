using System;

namespace Spectre.Console.Rendering
{
    class AsciiTreeRendering : ITreeRendering
    {

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

        public int PartSize => 4;
    }
}