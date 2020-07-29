using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using Spectre.Console.Composition;
using Spectre.Console.Internal;

namespace Spectre.Console
{
    /// <summary>
    /// Represents text with color and style.
    /// </summary>
    [SuppressMessage("Naming", "CA1724:Type names should not match namespaces")]
    public sealed class Text : IRenderable
    {
        private readonly string _text;
        private readonly Appearance _appearance;
        private readonly Justify _justify;

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="appearance">The appearance.</param>
        /// <param name="justify">The justification.</param>
        public Text(string text, Appearance appearance = null, Justify justify = Justify.Left)
        {
            _text = text ?? throw new ArgumentNullException(nameof(text));
            _appearance = appearance ?? Appearance.Plain;
            _justify = justify;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Text"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="foreground">The foreground.</param>
        /// <param name="background">The background.</param>
        /// <param name="style">The style.</param>
        /// <param name="justify">The justification.</param>
        /// <returns>A <see cref="Text"/> instance.</returns>
        public static Text New(
            string text, Color? foreground = null, Color? background = null,
            Styles? style = null, Justify justify = Justify.Left)
        {
            return new Text(text, new Appearance(foreground, background, style), justify);
        }

        /// <inheritdoc/>
        public int Measure(Encoding encoding, int maxWidth)
        {
            return _text.SplitLines().Max(x => x.CellLength(encoding));
        }

        /// <inheritdoc/>
        public IEnumerable<Segment> Render(Encoding encoding, int width)
        {
            var result = new List<Segment>();

            foreach (var line in Partition(encoding, _text, width))
            {
                result.Add(new Segment(line, _appearance));
            }

            return result;
        }

        private IEnumerable<string> Partition(Encoding encoding, string text, int width)
        {
            var lines = new List<string>();
            var line = new StringBuilder();

            var position = 0;
            foreach (var token in text)
            {
                if (token == '\n')
                {
                    lines.Add(line.ToString());
                    line.Clear();
                    position = 0;
                    continue;
                }

                if (position >= width)
                {
                    lines.Add(line.ToString());
                    line.Clear();
                    position = 0;
                }

                line.Append(token);
                position += token.CellLength(encoding);
            }

            if (line.Length > 0)
            {
                lines.Add(line.ToString());
            }

            // Justify lines
            for (var i = 0; i < lines.Count; i++)
            {
                if (_justify != Justify.Left && lines[i].CellLength(encoding) < width)
                {
                    if (_justify == Justify.Right)
                    {
                        var diff = width - lines[i].CellLength(encoding);
                        lines[i] = new string(' ', diff) + lines[i];
                    }
                    else if (_justify == Justify.Center)
                    {
                        var diff = (width - lines[i].CellLength(encoding)) / 2;
                        lines[i] = new string(' ', diff) + lines[i] + new string(' ', diff);
                    }
                }

                if (i < lines.Count - 1)
                {
                    lines[i] += "\n";
                }
            }

            return lines;
        }
    }
}
