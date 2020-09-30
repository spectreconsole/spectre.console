using System;

namespace Spectre.Console.Rendering
{
    /// <summary>
    /// Represents a rounded border.
    /// </summary>
    public sealed class RoundedBoxBorder : BoxBorder
    {
        /// <inheritdoc/>
        public override BoxBorder? SafeBorder => BoxBorder.Square;

        /// <inheritdoc/>
        protected override string GetBorderPart(BoxBorderPart part)
        {
            return part switch
            {
                BoxBorderPart.TopLeft => "╭",
                BoxBorderPart.Top => "─",
                BoxBorderPart.TopRight => "╮",
                BoxBorderPart.Left => "│",
                BoxBorderPart.Right => "│",
                BoxBorderPart.BottomLeft => "╰",
                BoxBorderPart.Bottom => "─",
                BoxBorderPart.BottomRight => "╯",
                _ => throw new InvalidOperationException("Unknown border part."),
            };
        }
    }
}
