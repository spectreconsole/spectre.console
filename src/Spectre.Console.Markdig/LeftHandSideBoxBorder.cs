using System;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class LeftHandSideBoxBorder : BoxBorder
    {
        /// <inheritdoc/>
        public override string GetPart(BoxBorderPart part)
        {
            return part switch
            {
                BoxBorderPart.TopLeft => " │",
                BoxBorderPart.Top => string.Empty,
                BoxBorderPart.TopRight => string.Empty,
                BoxBorderPart.Left => " │",
                BoxBorderPart.Right => string.Empty,
                BoxBorderPart.BottomLeft => " │",
                BoxBorderPart.Bottom => string.Empty,
                BoxBorderPart.BottomRight => string.Empty,
                _ => throw new InvalidOperationException("Unknown border part."),
            };
        }
    }
}