using System;
using System.Collections.Generic;
using System.Linq;

namespace Spectre.Console
{
    internal sealed class FigletCharacter
    {
        public int Code { get; }
        public int Width { get; }
        public int Height { get; }
        public IReadOnlyList<string> Lines { get; }

        public FigletCharacter(int code, IEnumerable<string> lines)
        {
            Code = code;
            Lines = new List<string>(lines ?? throw new ArgumentNullException(nameof(lines)));

            var min = Lines.Min(x => x.Length);
            var max = Lines.Max(x => x.Length);
            if (min != max)
            {
                throw new InvalidOperationException($"Figlet character #{code} has varying width");
            }

            Width = max;
            Height = Lines.Count;
        }
    }
}
