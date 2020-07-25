using System;
using Spectre.Console.Internal;

namespace Spectre.Console.Internal
{
    internal sealed class LineBreakElement : IRenderable
    {
        public int Width => 0;

        public void Render(IAnsiConsole renderer)
        {
            renderer.Write(Environment.NewLine);
        }
    }
}
