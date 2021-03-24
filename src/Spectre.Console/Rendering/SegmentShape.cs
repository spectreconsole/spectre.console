using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console.Rendering
{
    internal readonly struct SegmentShape
    {
        public int Width { get; }
        public int Height { get; }

        public SegmentShape(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public static SegmentShape Calculate(RenderContext context, List<SegmentLine> lines)
        {
            if (context is null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (lines is null)
            {
                throw new ArgumentNullException(nameof(lines));
            }

            var height = lines.Count;
            var width = lines.Max(l => Segment.CellCount(l));

            return new SegmentShape(width, height);
        }

        public SegmentShape Inflate(SegmentShape other)
        {
            return new SegmentShape(
                Math.Max(Width, other.Width),
                Math.Max(Height, other.Height));
        }

        public void Apply(RenderContext context, ref List<SegmentLine> lines)
        {
            foreach (var line in lines)
            {
                var length = Segment.CellCount(line);
                var missing = Width - length;
                if (missing > 0)
                {
                    line.Add(Segment.Padding(missing));
                }
            }

            if (lines.Count < Height && Width > 0)
            {
                var missing = Height - lines.Count;
                for (var i = 0; i < missing; i++)
                {
                    lines.Add(new SegmentLine
                    {
                        Segment.Padding(Width),
                    });
                }
            }
        }
    }
}
