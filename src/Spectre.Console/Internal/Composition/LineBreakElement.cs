using System;

namespace Spectre.Console.Internal
{
    internal sealed class LineBreakElement : IConsoleElement
    {
        public int Width => 0;

        public void Render(IAnsiConsole renderer)
        {
            renderer.Write(Environment.NewLine);
        }
    }
}
