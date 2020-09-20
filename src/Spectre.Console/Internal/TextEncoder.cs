using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console.Internal
{
    internal sealed class TextEncoder : IAnsiConsoleEncoder
    {
        public string Encode(IEnumerable<Segment> segments)
        {
            var builder = new StringBuilder();

            foreach (var segment in Segment.Merge(segments))
            {
                builder.Append(segment.Text);
            }

            return builder.ToString().TrimEnd('\n');
        }
    }
}
