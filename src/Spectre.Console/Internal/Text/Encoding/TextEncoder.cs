using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class TextEncoder : IAnsiConsoleEncoder
    {
        public string Encode(IEnumerable<Segment> segments)
        {
            var builder = new StringBuilder();

            foreach (var segment in Segment.Merge(segments))
            {
                if (segment.IsControlCode)
                {
                    continue;
                }

                builder.Append(segment.Text);
            }

            return builder.ToString().TrimEnd('\n');
        }
    }
}
