using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents an old school ASCII border.
    /// </summary>
    public sealed class AsciiBoxBorder : BoxBorder
    {
        /// <inheritdoc/>
        protected override string GetBorderPart(BoxBorderPart part)
        {
            return part switch
            {
                BoxBorderPart.TopLeft => "+",
                BoxBorderPart.Top => "-",
                BoxBorderPart.TopRight => "+",
                BoxBorderPart.Left => "|",
                BoxBorderPart.Right => "|",
                BoxBorderPart.BottomLeft => "+",
                BoxBorderPart.Bottom => "-",
                BoxBorderPart.BottomRight => "+",
                _ => throw new InvalidOperationException("Unknown border part."),
            };
        }
    }
}
