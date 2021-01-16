using System;
using System.Collections.Generic;
using System.Text;
using Spectre.Console.Rendering;

namespace Spectre.Console
{
    internal sealed class AnsiConsoleBackend : IAnsiConsoleBackend
    {
        private readonly AnsiBuilder _builder;
        private readonly Profile _profile;

        public IAnsiConsoleCursor Cursor { get; }

        public AnsiConsoleBackend(Profile profile)
        {
            _profile = profile ?? throw new ArgumentNullException(nameof(profile));
            _builder = new AnsiBuilder(profile);

            Cursor = new AnsiConsoleCursor(this);
        }

        public void Clear(bool home)
        {
            Render(new[] { Segment.Control("\u001b[2J") });

            if (home)
            {
                Cursor.SetPosition(0, 0);
            }
        }

        public void Render(IEnumerable<Segment> segments)
        {
            var builder = new StringBuilder();
            foreach (var segment in segments)
            {
                if (segment.IsControlCode)
                {
                    builder.Append(segment.Text);
                    continue;
                }

                var parts = segment.Text.NormalizeNewLines().Split(new[] { '\n' });
                foreach (var (_, _, last, part) in parts.Enumerate())
                {
                    if (!string.IsNullOrEmpty(part))
                    {
                        builder.Append(_builder.GetAnsi(part, segment.Style));
                    }

                    if (!last)
                    {
                        builder.Append(Environment.NewLine);
                    }
                }
            }

            if (builder.Length > 0)
            {
                _profile.Out.Write(builder.ToString());
                _profile.Out.Flush();
            }
        }
    }
}
